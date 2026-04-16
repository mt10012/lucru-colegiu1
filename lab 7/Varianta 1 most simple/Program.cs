using System;
using System.Collections.Generic;

// PARTEA I - Clasa de baza abstracta

abstract class Produs
{
    public string Cod { get; set; }
    public string Denumire { get; set; }

    protected double pretBaza;
    public double PretBaza
    {
        get { return pretBaza; }
        set
        {
            if (value < 0)
            {
                Console.WriteLine("Eroare: Pretul nu poate fi negativ!");
                pretBaza = 0;
            }
            else
            {
                pretBaza = value;
            }
        }
    }
    private int cantitateStoc;
    public int CantitateStoc
    {
        get { return cantitateStoc; }
        set
        {
            if (value < 0)
            {
                Console.WriteLine("Eroare: Cantitatea nu poate fi negativa!");
                cantitateStoc = 0;
            }
            else
            {
                cantitateStoc = value;
            }
        }
    }

    public Produs(string cod, string denumire, double pretBaza, int cantitateStoc)
    {
        Cod = cod;
        Denumire = denumire;
        PretBaza = pretBaza;
        CantitateStoc = cantitateStoc;
    }
    public virtual double CalculeazaPretFinal()
    {
        return pretBaza;
    }

    public virtual string Descriere()
    {
        return $"[{Cod}] {Denumire} | Pret: {CalculeazaPretFinal():F2} lei | Stoc: {CantitateStoc}";
    }
}

// PARTEA II - Clase derivate

class ProdusPerisabil : Produs
{
    public DateTime DataExpirare { get; set; }
    public double DiscountPerisabil { get; set; }

    public ProdusPerisabil(string cod, string denumire, double pretBaza, int stoc,
                           DateTime dataExpirare, double discountPerisabil)
        : base(cod, denumire, pretBaza, stoc)
    {
        DataExpirare = dataExpirare;
        DiscountPerisabil = discountPerisabil;
    }
    public override double CalculeazaPretFinal()
    {
        int zilePanaLaExpirare = (DataExpirare - DateTime.Today).Days;

        if (zilePanaLaExpirare < 3)
        {
            double reducere = pretBaza * DiscountPerisabil;
            return pretBaza - reducere;
        }

        return pretBaza;
    }
    public override string Descriere()
    {
        int zile = (DataExpirare - DateTime.Today).Days;
        string avertisment = zile < 3 ? $" *** DISCOUNT {DiscountPerisabil * 100}% APLICAT! ***" : "";
        return $"[PERISABIL] {base.Descriere()} | Expira in: {zile} zile{avertisment}";
    }
}


class ProdusDigital : Produs
{
    public double DimensiuneMB { get; set; }
    public double TaxaLicenta { get; set; }

    public ProdusDigital(string cod, string denumire, double pretBaza, int stoc,
                         double dimensiuneMB, double taxaLicenta)
        : base(cod, denumire, pretBaza, stoc)
    {
        DimensiuneMB = dimensiuneMB;
        TaxaLicenta = taxaLicenta;
    }
    public override double CalculeazaPretFinal()
    {
        return pretBaza + TaxaLicenta;
    }

    public override string Descriere()
    {
        return $"[DIGITAL] {base.Descriere()} | Marime: {DimensiuneMB} MB | Taxa licenta: {TaxaLicenta:F2} lei";
    }
}
class ProdusPremium : Produs
{
    public double ProcentAdaosBrand { get; set; }

    public ProdusPremium(string cod, string denumire, double pretBaza, int stoc,
                         double procentAdaosBrand)
        : base(cod, denumire, pretBaza, stoc)
    {
        ProcentAdaosBrand = procentAdaosBrand;
    }
    public override double CalculeazaPretFinal()
    {
        double adaos = pretBaza * ProcentAdaosBrand;
        return pretBaza + adaos;
    }

    public override string Descriere()
    {
        return $"[PREMIUM] {base.Descriere()} | Adaos brand: {ProcentAdaosBrand * 100}%";
    }
}

class Program
{
    // PARTEA III - Metode cu params

    public static double TotalGeneral(params Produs[] produse)
    {
        double total = 0;
        foreach (Produs p in produse)
        {
            total += p.CalculeazaPretFinal() * p.CantitateStoc;
        }
        return total;
    }
    public static Produs ProdusCelMaiScump(params Produs[] produse)
    {
        Produs celMaiScump = produse[0];

        for (int i = 1; i < produse.Length; i++)
        {
            if (produse[i].CalculeazaPretFinal() > celMaiScump.CalculeazaPretFinal())
            {
                celMaiScump = produse[i];
            }
        }

        return celMaiScump;
    }

    // PARTEA IV - Metode cu ref

    public static void AjusteazaPret(ref Produs p, double suma)
    {
        double pretNou = p.PretBaza + suma;

        if (pretNou < 0)
        {
            Console.WriteLine($"  Atentie: Ajustarea ar face pretul negativ! Pretul ramane {p.PretBaza:F2} lei.");
            return;
        }

        p.PretBaza = pretNou;
        Console.WriteLine($"  Pretul produsului '{p.Denumire}' a fost ajustat cu {suma:F2} lei. Pret nou: {p.PretBaza:F2} lei");
    }
    public static void VindeProdus(ref Produs p, int cantitate)
    {
        if (cantitate > p.CantitateStoc)
        {
            Console.WriteLine($"  Stoc insuficient pentru '{p.Denumire}'! Disponibil: {p.CantitateStoc}, cerut: {cantitate}");
            return;
        }

        p.CantitateStoc -= cantitate;
        Console.WriteLine($"  Vandut {cantitate} buc din '{p.Denumire}'. Stoc ramas: {p.CantitateStoc}");
    }

    // PARTEA V - Metode cu out

    public static bool CautaDupaIntervalPret(List<Produs> lista, double min, double max,
                                              out List<Produs> rezultate)
    {
        rezultate = new List<Produs>();

        foreach (Produs p in lista)
        {
            double pret = p.CalculeazaPretFinal();
            if (pret >= min && pret <= max)
            {
                rezultate.Add(p);
            }
        }

        return rezultate.Count > 0;
    }
    public static bool ExistaProduseExpirate(List<Produs> lista, out int numarProduseExpirate)
    {
        numarProduseExpirate = 0;

        foreach (Produs p in lista)
        {
            if (p is ProdusPerisabil perisabil)
            {
                if (perisabil.DataExpirare < DateTime.Today)
                {
                    numarProduseExpirate++;
                }
            }
        }

        return numarProduseExpirate > 0;
    }


    // PARTEA VI - Metode cu in

    public static void AfiseazaRezumat(in Produs p)
    {
        Console.WriteLine($"  Rezumat: {p.Descriere()}");
    }
    public static void AfiseazaRaport(in List<Produs> lista)
    {
        Console.WriteLine("\n  Raport complet:\n");

        Console.WriteLine("  Produse Perisabile:");
        foreach (Produs p in lista)
        {
            if (p is ProdusPerisabil)
                Console.WriteLine("    " + p.Descriere());
        }

        Console.WriteLine("\n  Produse Digitale:");
        foreach (Produs p in lista)
        {
            if (p is ProdusDigital)
                Console.WriteLine("    " + p.Descriere());
        }

        Console.WriteLine("\n  Produse Premium:");
        foreach (Produs p in lista)
        {
            if (p is ProdusPremium)
                Console.WriteLine("    " + p.Descriere());
        }
    }

    // PARTEA VII - Simulare si Black Friday

    public static void AplicaBlackFriday(params Produs[] produse)
    {
        Console.WriteLine("\n  BLACK FRIDAY - 10% reducere la toate produsele!");

        foreach (Produs p in produse)
        {
            double reducere = p.PretBaza * 0.10;
            p.PretBaza = p.PretBaza - reducere;
            Console.WriteLine($"  '{p.Denumire}' -> pret nou: {p.PretBaza:F2} lei");
        }
    }
    public static List<Produs> CitesteProduseDeLaUtilizator()
    {
        List<Produs> lista = new List<Produs>();

        Console.WriteLine("\nVei introduce 3 produse perisabile, 2 digitale si 3 premium.\n");

        for (int i = 1; i <= 3; i++)
        {
            Console.WriteLine($"  Produs Perisabil nr. {i}:");

            Console.Write("    Cod: ");
            string cod = Console.ReadLine();

            Console.Write("    Denumire: ");
            string denumire = Console.ReadLine();

            Console.Write("    Pret baza (lei): ");
            double pret = double.Parse(Console.ReadLine());

            Console.Write("    Cantitate stoc: ");
            int stoc = int.Parse(Console.ReadLine());

            Console.Write("    Zile pana la expirare: ");
            int zile = int.Parse(Console.ReadLine());

            Console.Write("    Discount perisabil (0.20 pentru 20%): ");
            double discount = double.Parse(Console.ReadLine());

            lista.Add(new ProdusPerisabil(cod, denumire, pret, stoc, DateTime.Today.AddDays(zile), discount));
            Console.WriteLine();
        }

        for (int i = 1; i <= 2; i++)
        {
            Console.WriteLine($"  Produs Digital nr. {i}:");

            Console.Write("    Cod: ");
            string cod = Console.ReadLine();

            Console.Write("    Denumire: ");
            string denumire = Console.ReadLine();

            Console.Write("    Pret baza (lei): ");
            double pret = double.Parse(Console.ReadLine());

            Console.Write("    Cantitate stoc: ");
            int stoc = int.Parse(Console.ReadLine());

            Console.Write("    Dimensiune (MB): ");
            double mb = double.Parse(Console.ReadLine());

            Console.Write("    Taxa licenta (lei): ");
            double taxa = double.Parse(Console.ReadLine());

            lista.Add(new ProdusDigital(cod, denumire, pret, stoc, mb, taxa));
            Console.WriteLine();
        }

        for (int i = 1; i <= 3; i++)
        {
            Console.WriteLine($"  Produs Premium nr. {i}:");

            Console.Write("    Cod: ");
            string cod = Console.ReadLine();

            Console.Write("    Denumire: ");
            string denumire = Console.ReadLine();

            Console.Write("    Pret baza (lei): ");
            double pret = double.Parse(Console.ReadLine());

            Console.Write("    Cantitate stoc: ");
            int stoc = int.Parse(Console.ReadLine());

            Console.Write("    Procent adaos brand (0.30 pentru 30%): ");
            double adaos = double.Parse(Console.ReadLine());

            lista.Add(new ProdusPremium(cod, denumire, pret, stoc, adaos));
            Console.WriteLine();
        }

        return lista;
    }

    public static List<Produs> CreeazaDateDeTestare()
    {
        Produs lapte = new ProdusPerisabil("P001", "Lapte 1L", 8.50, 100, DateTime.Today.AddDays(1), 0.30);
        Produs paine = new ProdusPerisabil("P002", "Paine alba", 4.00, 200, DateTime.Today.AddDays(5), 0.20);
        Produs iaurt = new ProdusPerisabil("P003", "Iaurt natural", 6.00, 50, DateTime.Today.AddDays(2), 0.25);
        Produs antivirus = new ProdusDigital("D001", "Antivirus Pro", 99.00, 500, 350.5, 25.00);
        Produs joc = new ProdusDigital("D002", "Joc video", 199.00, 300, 50000, 15.00);
        Produs parfum = new ProdusPremium("R001", "Parfum Luxury", 450.00, 30, 0.40);
        Produs ceas = new ProdusPremium("R002", "Ceas scump", 2500.00, 10, 0.50);
        Produs geanta = new ProdusPremium("R003", "Geanta scump", 800.00, 20, 0.35);

        return new List<Produs>() { lapte, paine, iaurt, antivirus, joc, parfum, ceas, geanta };
    }

    //  PARTEA VII Scenariu complex de simulare
    static void Main(string[] args)
    {
        Console.WriteLine("Platforma de vanzare - Simulare\n");

        Console.WriteLine("Cum doresti sa introduci produsele?");
        Console.WriteLine("  1 - Introduc eu datele manual");
        Console.WriteLine("  2 - Folosesc datele de testare predefinite");
        Console.Write("\nAlege optiunea (1 sau 2): ");

        string optiune = Console.ReadLine();

        List<Produs> toateProdusele;

        if (optiune == "1")
        {
            Console.WriteLine("\nAi ales sa introduci datele manual.");
            toateProdusele = CitesteProduseDeLaUtilizator();
        }
        else
        {
            Console.WriteLine("\nAi ales datele de testare predefinite.");
            toateProdusele = CreeazaDateDeTestare();
        }

        Console.WriteLine("\nToate produsele:");
        foreach (Produs p in toateProdusele)
        {
            Console.WriteLine(p.Descriere());
        }

        Console.WriteLine("\nSimulare Vanzari:");
        Produs prod0 = toateProdusele[0];
        Produs prod3 = toateProdusele[3];
        Produs prod5 = toateProdusele[5];
        VindeProdus(ref prod0, 10);
        VindeProdus(ref prod3, 50);
        VindeProdus(ref prod5, 5);
        toateProdusele[0] = prod0;
        toateProdusele[3] = prod3;
        toateProdusele[5] = prod5;

        Console.WriteLine("\nAjustare Preturi:");
        Produs prod1 = toateProdusele[1];
        Produs prod4 = toateProdusele[4];
        AjusteazaPret(ref prod1, 1.50);
        AjusteazaPret(ref prod4, -20.00);
        toateProdusele[1] = prod1;
        toateProdusele[4] = prod4;

        Console.WriteLine("\nProdus cu valoarea maxima de stoc (pret x cantitate):");
        Produs celMaiValoros = toateProdusele[0];
        double valoareMax = celMaiValoros.CalculeazaPretFinal() * celMaiValoros.CantitateStoc;

        foreach (Produs p in toateProdusele)
        {
            double valoare = p.CalculeazaPretFinal() * p.CantitateStoc;
            if (valoare > valoareMax)
            {
                valoareMax = valoare;
                celMaiValoros = p;
            }
        }

        Console.WriteLine($"  {celMaiValoros.Denumire} | Valoare totala stoc: {valoareMax:F2} lei");

        Console.WriteLine("\nTotal General:");
        double total = TotalGeneral(toateProdusele.ToArray());
        Console.WriteLine($"  Valoarea totala a tuturor stocurilor: {total:F2} lei");

        AfiseazaRaport(in toateProdusele);

        Console.WriteLine("\nCautare produse intre 10 si 250 lei:");
        List<Produs> produseCautate;
        bool gasit = CautaDupaIntervalPret(toateProdusele, 10, 250, out produseCautate);

        if (gasit)
        {
            Console.WriteLine($"  Gasit {produseCautate.Count} produs(e):");
            foreach (Produs p in produseCautate)
                Console.WriteLine("  " + p.Descriere());
        }
        else
        {
            Console.WriteLine("  Nu s-au gasit produse in intervalul specificat.");
        }

        Console.WriteLine("\nVerificare produse expirate:");
        int numarExpirate;
        bool existaExpirate = ExistaProduseExpirate(toateProdusele, out numarExpirate);

        if (existaExpirate)
            Console.WriteLine($"  Atentie! Exista {numarExpirate} produs(e) expirat(e)!");
        else
            Console.WriteLine("  Nu exista produse expirate.");

        Console.WriteLine("\nCel mai scump produs:");
        Produs celMaiScump = ProdusCelMaiScump(toateProdusele.ToArray());
        Console.WriteLine($"  {celMaiScump.Denumire} - {celMaiScump.CalculeazaPretFinal():F2} lei");

        Console.WriteLine("\nRezumat produs individual (al 7-lea produs):");
        Produs prodRezumat = toateProdusele[6];
        AfiseazaRezumat(in prodRezumat);

        AplicaBlackFriday(toateProdusele.ToArray());

        Console.WriteLine("\nPreturi dupa Black Friday:");
        foreach (Produs p in toateProdusele)
            Console.WriteLine($"  {p.Denumire}: {p.PretBaza:F2} lei");

        Console.WriteLine("\nSimulare finalizata!");
    }
}