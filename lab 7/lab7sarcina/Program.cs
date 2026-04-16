using System;
using System.Collections.Generic;

// PARTEA I - Clasa de baza Comanda

class Comanda
{
    public int Id { get; set; }
    public string NumeClient { get; set; }
    public DateTime DataCrearii { get; set; }

    public Comanda(int id, string numeClient)
    {
        Id = id;
        NumeClient = numeClient;
        DataCrearii = DateTime.Now;
    }

    public double CalculeazaTotal(params double[] preturi)
    {
        double total = 0;
        foreach (double pret in preturi)
        {
            total += pret;
        }
        return total;
    }


    // PARTEA II - Parametri speciali

    public void CalculeazaTVA(double cotaTVA, out double totalFaraTVA, out double totalCuTVA, params double[] preturi)
    {
        totalFaraTVA = 0;
        foreach (double pret in preturi)
        {
            totalFaraTVA += pret;
        }
        totalCuTVA = totalFaraTVA + totalFaraTVA * cotaTVA;
    }

    public void AfiseazaDetalii(bool afiseazaData = true)
    {
        Console.WriteLine($"  ID comanda: {Id}");
        Console.WriteLine($"  Client: {NumeClient}");
        if (afiseazaData)
        {
            Console.WriteLine($"  Data crearii: {DataCrearii}");
        }
    }

    public void AplicaReducere(ref double procentReducere)
    {
        if (procentReducere > 30)
        {
            Console.WriteLine($"  Reducerea de {procentReducere}% depaseste limita! A fost limitata la 30%.");
            procentReducere = 30;
        }
        else
        {
            Console.WriteLine($"  Reducerea de {procentReducere}% este valida si ramane neschimbata.");
        }
    }
}


// PARTEA III - Mostenire: ComandaExpress

class ComandaExpress : Comanda
{
    public double TaxaLivrareRapida { get; set; }

    public ComandaExpress(int id, string numeClient, double taxaLivrareRapida)
        : base(id, numeClient)
    {
        TaxaLivrareRapida = taxaLivrareRapida;
    }

    public double CalculeazaTotalFinal(params double[] preturi)
    {
        double totalProduse = CalculeazaTotal(preturi);
        return totalProduse + TaxaLivrareRapida;
    }
}


// PARTEA IV - Scenariu de testare

class Program
{
    static void RuleazaValoriPrestabilite()
    {
        Comanda comanda1 = new Comanda(1, "Ion Popescu");
        ComandaExpress comanda2 = new ComandaExpress(2, "Maria Ionescu", 20);

        Console.WriteLine("Calculul totalului pentru 3 produse (comanda normala):");
        double total1 = comanda1.CalculeazaTotal(100, 50, 75);
        Console.WriteLine($"  Produse: 100, 50, 75 -> Total: {total1} lei\n");

        Console.WriteLine("Calculul totalului cu TVA:");
        double faraTVA;
        double cuTVA;
        comanda1.CalculeazaTVA(0.19, out faraTVA, out cuTVA, 100, 50);
        Console.WriteLine($"  Produse: 100, 50");
        Console.WriteLine($"  Total fara TVA: {faraTVA} lei");
        Console.WriteLine($"  Total cu TVA (19%): {cuTVA} lei\n");

        Console.WriteLine("Modificarea procentului de reducere prin referinta:");
        double reducere1 = 50;
        Console.WriteLine($"  Reducere introdusa: {reducere1}%");
        comanda1.AplicaReducere(ref reducere1);
        Console.WriteLine($"  Reducere dupa verificare: {reducere1}%\n");

        double reducere2 = 10;
        Console.WriteLine($"  Reducere introdusa: {reducere2}%");
        comanda1.AplicaReducere(ref reducere2);
        Console.WriteLine($"  Reducere dupa verificare: {reducere2}%\n");

        Console.WriteLine("Afisarea detaliilor cu data:");
        comanda1.AfiseazaDetalii(true);

        Console.WriteLine("\nAfisarea detaliilor fara data:");
        comanda1.AfiseazaDetalii(false);

        Console.WriteLine("\nDiferenta dintre comanda normala si comanda express:");
        double totalNormal = comanda1.CalculeazaTotal(100, 50);
        Console.WriteLine($"  Comanda normala  | Produse: 100, 50 | Total: {totalNormal} lei");

        double totalExpress = comanda2.CalculeazaTotalFinal(100, 50);
        Console.WriteLine($"  Comanda express  | Produse: 100, 50 | Taxa livrare rapida: {comanda2.TaxaLivrareRapida} lei | Total final: {totalExpress} lei");

        Console.WriteLine($"\n  Diferenta dintre express si normal: {totalExpress - totalNormal} lei");
    }

    static void RuleazaValoriManuale()
    {
        Console.Write("\nIntroduceti ID comanda normala: ");
        int id1 = int.Parse(Console.ReadLine());

        Console.Write("Introduceti numele clientului (comanda normala): ");
        string nume1 = Console.ReadLine();

        Comanda comanda1 = new Comanda(id1, nume1);

        Console.Write("\nIntroduceti ID comanda express: ");
        int id2 = int.Parse(Console.ReadLine());

        Console.Write("Introduceti numele clientului (comanda express): ");
        string nume2 = Console.ReadLine();

        Console.Write("Introduceti taxa de livrare rapida: ");
        double taxa = double.Parse(Console.ReadLine());

        ComandaExpress comanda2 = new ComandaExpress(id2, nume2, taxa);

        Console.Write("\nCate produse doriti sa adaugati la comanda normala (minim 3)? ");
        int nrProduse = int.Parse(Console.ReadLine());

        double[] preturi = new double[nrProduse];
        for (int i = 0; i < nrProduse; i++)
        {
            Console.Write($"  Pret produs {i + 1}: ");
            preturi[i] = double.Parse(Console.ReadLine());
        }

        Console.WriteLine("\nCalculul totalului pentru produsele introduse:");
        double total1 = comanda1.CalculeazaTotal(preturi);
        Console.Write("  Produse: ");
        for (int i = 0; i < preturi.Length; i++)
        {
            Console.Write(i < preturi.Length - 1 ? $"{preturi[i]}, " : $"{preturi[i]}");
        }
        Console.WriteLine($" -> Total: {total1} lei\n");

        Console.Write("Introduceti cota TVA (ex: 0.19 pentru 19%): ");
        double cotaTVA = double.Parse(Console.ReadLine());

        Console.WriteLine("\nCalculul totalului cu TVA:");
        double faraTVA;
        double cuTVA;
        comanda1.CalculeazaTVA(cotaTVA, out faraTVA, out cuTVA, preturi);
        Console.WriteLine($"  Total fara TVA: {faraTVA} lei");
        Console.WriteLine($"  Total cu TVA ({cotaTVA * 100}%): {cuTVA} lei\n");

        Console.Write("Introduceti primul procent de reducere de testat: ");
        double reducere1 = double.Parse(Console.ReadLine());
        Console.WriteLine($"  Reducere introdusa: {reducere1}%");
        comanda1.AplicaReducere(ref reducere1);
        Console.WriteLine($"  Reducere dupa verificare: {reducere1}%\n");

        Console.Write("Introduceti al doilea procent de reducere de testat: ");
        double reducere2 = double.Parse(Console.ReadLine());
        Console.WriteLine($"  Reducere introdusa: {reducere2}%");
        comanda1.AplicaReducere(ref reducere2);
        Console.WriteLine($"  Reducere dupa verificare: {reducere2}%\n");

        Console.WriteLine("Afisarea detaliilor cu data:");
        comanda1.AfiseazaDetalii(true);

        Console.WriteLine("\nAfisarea detaliilor fara data:");
        comanda1.AfiseazaDetalii(false);

        Console.WriteLine("\nDiferenta dintre comanda normala si comanda express:");
        double totalNormal = comanda1.CalculeazaTotal(preturi);
        Console.WriteLine($"  Comanda normala  | Total: {totalNormal} lei");

        double totalExpress = comanda2.CalculeazaTotalFinal(preturi);
        Console.WriteLine($"  Comanda express  | Taxa livrare rapida: {comanda2.TaxaLivrareRapida} lei | Total final: {totalExpress} lei");

        Console.WriteLine($"\n  Diferenta dintre express si normal: {totalExpress - totalNormal} lei");
    }

    static void Main(string[] args)
    {
        Console.WriteLine("\n");

        Console.WriteLine("?");
        Console.WriteLine("  1 - Folosesc valorile prestabilite");
        Console.WriteLine("  2 - Introduc eu valorile manual");
        Console.Write("\nAlege (1 sau 2): ");

        string optiune = Console.ReadLine();

        if (optiune == "1")
        {
            Console.WriteLine("\nAi ales valorile prestabilite.\n");
            RuleazaValoriPrestabilite();
        }
        else
        {
            Console.WriteLine("\nAi ales sa introduci valorile manual.");
            RuleazaValoriManuale();
        }

        Console.WriteLine("\nProgram finalizat!");
    }
}