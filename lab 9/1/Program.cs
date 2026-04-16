using System;
using System.Collections.Generic;
using System.IO;

class Item
{
    public string Nume;
    public int Valoare;
    public int Damage;

    public Item() { }

    public Item(string nume, int valoare, int damage)
    {
        Nume = nume;
        Valoare = valoare;
        Damage = damage;
    }

    public Item(Item i)
    {
        Nume = i.Nume;
        Valoare = i.Valoare;
        Damage = i.Damage;
    }

    public void Afisare()
    {
        Console.WriteLine($"{Nume} | Val: {Valoare} | DMG: {Damage}");
    }

    public string ToFile()
    {
        return $"{Nume},{Valoare},{Damage}";
    }
}

class Inventar
{
    public List<Item> items = new List<Item>();

    public Inventar() { }

    public Inventar(List<Item> lista)
    {
        items = lista;
    }

    public Inventar(Inventar inv)
    {
        items = new List<Item>(inv.items);
    }

    public void AdaugaItem(Item i)
    {
        items.Add(i);
    }

    public void Afisare()
    {
        foreach (var i in items)
            i.Afisare();
    }

    public int ValoareTotala()
    {
        int s = 0;
        foreach (var i in items)
            s += i.Valoare;
        return s;
    }

    public void DetecteazaDuplicate()
    {
        for (int i = 0; i < items.Count; i++)
            for (int j = i + 1; j < items.Count; j++)
                if (items[i].Nume == items[j].Nume)
                    Console.WriteLine("Duplicat: " + items[i].Nume);
    }
}

class Jucator
{
    public string Nume;
    public int Viata;
    public Inventar inventar = new Inventar();
    public Item echipat;

    public Jucator() { }

    public Jucator(string nume, int viata)
    {
        Nume = nume;
        Viata = viata;
    }

    public Jucator(Jucator j)
    {
        Nume = j.Nume;
        Viata = j.Viata;
        inventar = new Inventar(j.inventar);
    }

    public void Afisare()
    {
        Console.WriteLine($"{Nume} | HP: {Viata}");
    }

    public void Echipeaza(int index)
    {
        if (index >= 0 && index < inventar.items.Count)
            echipat = inventar.items[index];
    }

    public void Dezechipeaza()
    {
        echipat = null;
    }

    public int Atac()
    {
        if (echipat != null)
            return echipat.Damage;
        return 5;
    }
}

class Lupta
{
    public Jucator j1;
    public Jucator j2;
    public List<string> log = new List<string>();

    public Lupta() { }

    public Lupta(Jucator a, Jucator b)
    {
        j1 = new Jucator(a);
        j2 = new Jucator(b);
    }

    public Lupta(Lupta l)
    {
        j1 = l.j1;
        j2 = l.j2;
    }

    public void Simulare()
    {
        int runda = 1;

        while (j1.Viata > 0 && j2.Viata > 0)
        {
            int dmg1 = j1.Atac();
            int dmg2 = j2.Atac();

            j2.Viata -= dmg1;
            j1.Viata -= dmg2;

            string linie = $"Runda {runda}: {j1.Nume} -> {dmg1} dmg | {j2.Nume} -> {dmg2} dmg";
            Console.WriteLine(linie);
            log.Add(linie);

            if (dmg1 > 100 || dmg2 > 100)
                Console.WriteLine(" Cheat detectat!");

            runda++;
        }

        if (j1.Viata > 0)
            Console.WriteLine("Castigator: " + j1.Nume);
        else
            Console.WriteLine("Castigator: " + j2.Nume);
    }

    public void AfisareLog()
    {
        foreach (var l in log)
            Console.WriteLine(l);
    }
}

class Program
{
    static List<Jucator> jucatori = new List<Jucator>();

    static void Salveaza()
    {
        using (StreamWriter sw = new StreamWriter("jucatori.txt"))
        {
            foreach (var j in jucatori)
                sw.WriteLine($"{j.Nume},{j.Viata}");
        }
    }

    static void Incarca()
    {
        if (!File.Exists("jucatori.txt")) return;

        string[] linii = File.ReadAllLines("jucatori.txt");

        foreach (var l in linii)
        {
            var p = l.Split(',');
            jucatori.Add(new Jucator(p[0], int.Parse(p[1])));
        }
    }

    static void Main()
    {
        Incarca();

        while (true)
        {
            Console.WriteLine("\n1.Creare jucator");
            Console.WriteLine("2.Afisare jucatori");
            Console.WriteLine("3.Adauga item");
            Console.WriteLine("4.Lupta");
            Console.WriteLine("5.Salvare");
            Console.WriteLine("0.Exit");

            int op = int.Parse(Console.ReadLine());

            if (op == 1)
            {
                Console.Write("Nume: ");
                string n = Console.ReadLine();
                jucatori.Add(new Jucator(n, 100));
            }

            if (op == 2)
            {
                for (int i = 0; i < jucatori.Count; i++)
                {
                    Console.Write(i + ": ");
                    jucatori[i].Afisare();
                }
            }

            if (op == 3)
            {
                Console.Write("Index jucator: ");
                int idx = int.Parse(Console.ReadLine());

                Console.Write("Nume item: ");
                string n = Console.ReadLine();

                Console.Write("Valoare: ");
                int v = int.Parse(Console.ReadLine());

                Console.Write("Damage: ");
                int d = int.Parse(Console.ReadLine());

                jucatori[idx].inventar.AdaugaItem(new Item(n, v, d));
            }

            if (op == 4)
            {
                Console.Write("J1: ");
                int a = int.Parse(Console.ReadLine());

                Console.Write("J2: ");
                int b = int.Parse(Console.ReadLine());

                Lupta l = new Lupta(jucatori[a], jucatori[b]);
                l.Simulare();
            }

            if (op == 5)
                Salveaza();

            if (op == 0)
                break;
        }
    }
}