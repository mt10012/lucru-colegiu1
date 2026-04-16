
using System;
using System.IO;

interface IEvaluarePerformanta
{
    double CalculeazaScorPerformanta();
    void GenereazaRaportEvaluare();
}
interface IAccesAvansat
{
    void SolicitaAccesSpecial();
    string VerificaNivelAcces();
}
enum NivelProgramatorEnum
{
    Junior,
    Middle,
    Senior
}
enum NivelResponsabilitateEnum
{
    Scazut,
    Mediu,
    Ridicat
}
enum NivelAccesEnum
{
    Standard,
    Extins,
    Administrativ
}
class Angajat
{
    private string codAngajat;
    private string numeComplet;
    private decimal salariuBaza;
    private DateTime dataAngajarii;
    private int aniExperienta;
    public string CodAngajat
    {
        get { return codAngajat; }
        set
        {
            if (value == null || value.Length != 13)
                throw new Exception("CodAngajat trebuie sa aiba exact 13 cifre.");
            foreach (char c in value)
                if (!char.IsDigit(c))
                    throw new Exception("CodAngajat trebuie sa contina doar cifre.");
            codAngajat = value;
        }
    }
    public string NumeComplet
    {
        get { return numeComplet; }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new Exception("NumeComplet nu poate fi gol.");
            foreach (char c in value)
                if (!char.IsLetter(c) && c != ' ')
                    throw new Exception("NumeComplet poate contine doar litere si spatii.");
            numeComplet = value;
        }
    }
    public string Functie { get; set; }
    public string Departament { get; set; }

    public decimal SalariuBaza
    {
        get { return salariuBaza; }
        set
        {
            if (value <= 0)
                throw new Exception("SalariuBaza trebuie sa fie pozitiv.");
            salariuBaza = value;
        }
    }
    public DateTime DataAngajarii
    {
        get { return dataAngajarii; }
        set
        {
            if (value > DateTime.Now)
                throw new Exception("DataAngajarii nu poate fi in viitor.");
            dataAngajarii = value;
        }
    }
    public int AniExperienta
    {
        get { return aniExperienta; }
        set
        {
            if (value < 0)
                throw new Exception("AniExperienta nu poate fi negativ.");
            aniExperienta = value;
        }
    }
    public bool EsteActiv { get; set; }
    public bool AreAccesInSistem { get; set; }
    public Angajat()
    {
        EsteActiv = true;
    }
    public virtual void AfiseazaDetalii()
    {
        Console.WriteLine("Cod angajat: " + CodAngajat);
        Console.WriteLine("Nume complet: " + NumeComplet);
        Console.WriteLine("Functie: " + Functie);
        Console.WriteLine("Departament: " + Departament);
        Console.WriteLine("Salariu baza: " + SalariuBaza);
        Console.WriteLine("Este activ: " + EsteActiv);
        Console.WriteLine("Are acces in sistem: " + AreAccesInSistem);
        Console.WriteLine("Data angajarii: " + DataAngajarii.ToShortDateString());
        Console.WriteLine("Ani experienta: " + AniExperienta);
    }
    public void ActiveazaAngajat()
    {
        EsteActiv = true;
    }

    public void DezactiveazaAngajat()
    {
        EsteActiv = false;
    }

    public void AcordaAcces()
    {
        AreAccesInSistem = true;
    }

    public void RevocaAcces()
    {
        AreAccesInSistem = false;
    }

    public virtual decimal CalculeazaSalariu()
    {
        return SalariuBaza;
    }

    public virtual void AcordaBonus(decimal suma)
    {
        if (EsteActiv)
        {
            SalariuBaza += suma;
        }
        else
        {
            Console.WriteLine("Angajatul nu este activ. Bonusul nu poate fi acordat.");
        }
    }
}
class Programator : Angajat, IEvaluarePerformanta
{
    private int numarProiecte;

    public string LimbajPrincipal { get; set; }

    public int NumarProiecte
    {
        get { return numarProiecte; }
        set
        {
            if (value < 0)
                throw new Exception("NumarProiecte nu poate fi negativ.");
            numarProiecte = value;
        }
    }

    public NivelProgramatorEnum NivelProgramator { get; set; }

    public override decimal CalculeazaSalariu()
    {
        decimal salariu = SalariuBaza;
        if (NivelProgramator == NivelProgramatorEnum.Middle)
            salariu += SalariuBaza * 0.10m;
        else if (NivelProgramator == NivelProgramatorEnum.Senior)
            salariu += SalariuBaza * 0.20m;
        salariu += NumarProiecte * 2000;
        return salariu;
    }
    public override void AfiseazaDetalii()
    {
        base.AfiseazaDetalii();
        Console.WriteLine("Limbaj principal: " + LimbajPrincipal);
        Console.WriteLine("Numar proiecte: " + NumarProiecte);
        Console.WriteLine("Nivel programator: " + NivelProgramator);
    }

    public double CalculeazaScorPerformanta()
    {
        return NumarProiecte * 1.5;
    }

    public void GenereazaRaportEvaluare()
    {
        Console.WriteLine("Raport evaluare programator " + NumeComplet + ":");
        Console.WriteLine("  Limbaj principal: " + LimbajPrincipal);
        Console.WriteLine("  Numar proiecte: " + NumarProiecte);
        Console.WriteLine("  Nivel: " + NivelProgramator);
        Console.WriteLine("  Scor performanta: " + CalculeazaScorPerformanta());
    }
}
class Contabil : Angajat, IEvaluarePerformanta
{
    private int numarRapoarteLunare;

    public int NumarRapoarteLunare
    {
        get { return numarRapoarteLunare; }
        set
        {
            if (value < 0)
                throw new Exception("NumarRapoarteLunare nu poate fi negativ.");
            numarRapoarteLunare = value;
        }
    }

    public bool AreDreptDeSemnatura { get; set; }
    public NivelResponsabilitateEnum NivelResponsabilitate { get; set; }
    public void GenereazaRaportFinanciar()
    {
        Console.WriteLine("Raport financiar generat pentru " + NumeComplet + ".");
    }
    public override decimal CalculeazaSalariu()
    {
        decimal salariu = SalariuBaza;
        if (NivelResponsabilitate == NivelResponsabilitateEnum.Mediu)
            salariu += SalariuBaza * 0.15m;
        else if (NivelResponsabilitate == NivelResponsabilitateEnum.Ridicat)
            salariu += SalariuBaza * 0.30m;
        salariu += NumarRapoarteLunare * 500;
        return salariu;
    }
    public override void AfiseazaDetalii()
    {
        base.AfiseazaDetalii();
        Console.WriteLine("Numar rapoarte lunare: " + NumarRapoarteLunare);
        Console.WriteLine("Drept de semnatura: " + AreDreptDeSemnatura);
        Console.WriteLine("Nivel responsabilitate: " + NivelResponsabilitate);
    }

    public double CalculeazaScorPerformanta()
    {
        return NumarRapoarteLunare * 2.0;
    }

    public void GenereazaRaportEvaluare()
    {
        Console.WriteLine("Raport evaluare contabil " + NumeComplet + ":");
        Console.WriteLine("  Numar rapoarte lunare: " + NumarRapoarteLunare);
        Console.WriteLine("  Drept de semnatura: " + AreDreptDeSemnatura);
        Console.WriteLine("  Nivel responsabilitate: " + NivelResponsabilitate);
        Console.WriteLine("  Scor performanta: " + CalculeazaScorPerformanta());
    }
}
class AngajatRemote : Angajat, IAccesAvansat
{
    public string OrasLucru { get; set; }
    public bool AreLaptopServiciu { get; set; }
    public NivelAccesEnum NivelAcces { get; set; }
    public bool CerereAccesSpecial { get; set; }

    public void SolicitaAccesSpecial()
    {
        CerereAccesSpecial = true;
        Console.WriteLine("Cerere acces special inregistrata pentru " + NumeComplet + ".");
    }

    public string VerificaNivelAcces()
    {
        return NivelAcces.ToString();
    }

    public override decimal CalculeazaSalariu()
    {
        return SalariuBaza + 10000;
    }
    public override void AfiseazaDetalii()
    {
        base.AfiseazaDetalii();
        Console.WriteLine("Oras de lucru: " + OrasLucru);
        Console.WriteLine("Laptop de serviciu: " + AreLaptopServiciu);
        Console.WriteLine("Nivel acces: " + NivelAcces);
        Console.WriteLine("Cerere acces special: " + CerereAccesSpecial);
    }
}

class ProgramatorSenior : AngajatRemote
{
    private int numarEchipeCoordonate;
    private decimal bonusLeadership;

    public int NumarEchipeCoordonate
    {
        get { return numarEchipeCoordonate; }
        set
        {
            if (value < 0)
                throw new Exception("NumarEchipeCoordonate nu poate fi negativ.");
            numarEchipeCoordonate = value;
        }
    }

    public bool AreRolMentorat { get; set; }

    public decimal BonusLeadership
    {
        get { return bonusLeadership; }
        set
        {
            if (value < 0)
                throw new Exception("BonusLeadership nu poate fi negativ.");
            bonusLeadership = value;
        }
    }

    public string TehnologieSecundara { get; set; }

    public void AprobaCereriTehnice()
    {
        Console.WriteLine("Cererile tehnice au fost aprobate de " + NumeComplet + ".");
    }

    public decimal CalculeazaBonusLeadership()
    {
        return NumarEchipeCoordonate * 5000;
    }

    public override decimal CalculeazaSalariu()
    {
        decimal bonus = CalculeazaBonusLeadership();
        decimal sporMentorat = AreRolMentorat ? 10000 : 0;
        return SalariuBaza + bonus + sporMentorat;
    }

    public override void AfiseazaDetalii()
    {
        base.AfiseazaDetalii();
        Console.WriteLine("Numar echipe coordonate: " + NumarEchipeCoordonate);
        Console.WriteLine("Rol de mentorat: " + AreRolMentorat);
        Console.WriteLine("Bonus leadership: " + CalculeazaBonusLeadership());
        Console.WriteLine("Tehnologie secundara: " + TehnologieSecundara);
    }
}
class Program
{
    static string fisierLog = Path.Combine("date", "operatii_personal.out");
    static void SalveazaOperatie(string codAngajat, string operatie, string valoare, string stare)
    {
        Directory.CreateDirectory("date");
        StreamWriter sw = new StreamWriter(fisierLog, true);
        sw.WriteLine(DateTime.Now + " | " + codAngajat + " | " + operatie + " | " + valoare + " | " + stare);
        sw.Close();
    }
    static Angajat CreeazaAngajat()
    {
        Console.WriteLine("Tip angajat:");
        Console.WriteLine("1 - Programator");
        Console.WriteLine("2 - Contabil");
        Console.WriteLine("3 - Angajat Remote");
        Console.WriteLine("4 - Programator Senior");
        Console.Write("Optiunea: ");
        int tip = Convert.ToInt32(Console.ReadLine());
        Angajat a;
        if (tip == 1) a = new Programator();
        else if (tip == 2) a = new Contabil();
        else if (tip == 3) a = new AngajatRemote();
        else a = new ProgramatorSenior();
        Console.Write("Cod angajat (13 cifre): ");
        a.CodAngajat = Console.ReadLine();
        Console.Write("Nume complet: ");
        a.NumeComplet = Console.ReadLine();
        Console.Write("Functie: ");
        a.Functie = Console.ReadLine();
        Console.Write("Departament: ");
        a.Departament = Console.ReadLine();
        Console.Write("Salariu baza: ");
        a.SalariuBaza = Convert.ToDecimal(Console.ReadLine());
        Console.Write("Data angajarii (zi.luna.an CU PUNCT): ");
        a.DataAngajarii = DateTime.ParseExact(Console.ReadLine(), "dd.MM.yyyy", null);
        Console.Write("Ani experienta: ");
        a.AniExperienta = Convert.ToInt32(Console.ReadLine());
        if (a is Programator prog)
        {
            Console.Write("Limbaj principal: ");
            prog.LimbajPrincipal = Console.ReadLine();

            Console.Write("Numar proiecte: ");
            prog.NumarProiecte = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Nivel (0=Junior, 1=Middle, 2=Senior): ");
            prog.NivelProgramator = (NivelProgramatorEnum)Convert.ToInt32(Console.ReadLine());
        }
        if (a is Contabil cont)
        {
            Console.Write("Numar rapoarte lunare: ");
            cont.NumarRapoarteLunare = Convert.ToInt32(Console.ReadLine());

            Console.Write("Drept de semnatura (true/false): ");
            cont.AreDreptDeSemnatura = Convert.ToBoolean(Console.ReadLine());

            Console.WriteLine("Nivel responsabilitate (0=Scazut, 1=Mediu, 2=Ridicat): ");
            cont.NivelResponsabilitate = (NivelResponsabilitateEnum)Convert.ToInt32(Console.ReadLine());
        }
        if (a is AngajatRemote remote)
        {
            Console.Write("Oras de lucru: ");
            remote.OrasLucru = Console.ReadLine();

            Console.Write("Are laptop de serviciu (true/false): ");
            remote.AreLaptopServiciu = Convert.ToBoolean(Console.ReadLine());

            Console.WriteLine("Nivel acces (0=Standard, 1=Extins, 2=Administrativ): ");
            remote.NivelAcces = (NivelAccesEnum)Convert.ToInt32(Console.ReadLine());
        }
        if (a is ProgramatorSenior ps)
        {
            Console.Write("Numar echipe coordonate: ");
            ps.NumarEchipeCoordonate = Convert.ToInt32(Console.ReadLine());

            Console.Write("Are rol de mentorat (true/false): ");
            ps.AreRolMentorat = Convert.ToBoolean(Console.ReadLine());

            Console.Write("Bonus leadership: ");
            ps.BonusLeadership = Convert.ToDecimal(Console.ReadLine());

            Console.Write("Tehnologie secundara: ");
            ps.TehnologieSecundara = Console.ReadLine();
        }
        return a;
    }
    static void Main()
    {
        Angajat angajatCurent = null;
        int optiune;

        do
        {
            Console.WriteLine();
            Console.WriteLine("--- MENIU PRINCIPAL ---");
            Console.WriteLine("1 - Creeaza angajat nou");
            Console.WriteLine("2 - Afiseaza detalii angajat");
            Console.WriteLine("3 - Calculeaza salariu");
            Console.WriteLine("4 - Acorda bonus");
            Console.WriteLine("5 - Activeaza angajat");
            Console.WriteLine("6 - Dezactiveaza angajat");
            Console.WriteLine("7 - Acorda acces in sistem");
            Console.WriteLine("8 - Revoca acces din sistem");
            Console.WriteLine("9 - Solicita acces special (AngajatRemote/ProgramatorSenior)");
            Console.WriteLine("10 - Verifica nivel acces (AngajatRemote/ProgramatorSenior)");
            Console.WriteLine("11 - Calculeaza scor performanta (Programator/Contabil)");
            Console.WriteLine("12 - Genereaza raport evaluare (Programator/Contabil)");
            Console.WriteLine("13 - Genereaza raport financiar (Contabil)");
            Console.WriteLine("14 - Aproba cereri tehnice (ProgramatorSenior)");
            Console.WriteLine("15 - Calculeaza bonus leadership (ProgramatorSenior)");
            Console.WriteLine("0 - Iesire");
            Console.Write("Optiunea: ");
            optiune = Convert.ToInt32(Console.ReadLine());

            if (optiune == 1)
            {
                try
                {
                    angajatCurent = CreeazaAngajat();
                    Console.WriteLine("Angajat creat cu succes.");
                    SalveazaOperatie(angajatCurent.CodAngajat, "creare", "-", "activ=" + angajatCurent.EsteActiv);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Eroare: " + ex.Message);
                }
            }
            else if (optiune == 0)
            {
                Console.WriteLine("La revedere!");
            }
            else if (angajatCurent == null)
            {
                Console.WriteLine("Nu exista niciun angajat creat. Alegeti optiunea 1.");
            }
            else if (optiune == 2)
            {
                Console.WriteLine();
                angajatCurent.AfiseazaDetalii();
                SalveazaOperatie(angajatCurent.CodAngajat, "afisare detalii", "-", "activ=" + angajatCurent.EsteActiv);
            }
            else if (optiune == 3)
            {
                decimal salariu = angajatCurent.CalculeazaSalariu();
                Console.WriteLine("Salariu total: " + salariu);
                SalveazaOperatie(angajatCurent.CodAngajat, "calcul salariu", salariu.ToString(), "activ=" + angajatCurent.EsteActiv);
            }
            else if (optiune == 4)
            {
                Console.Write("Introduceti suma bonusului: ");
                decimal bonus = Convert.ToDecimal(Console.ReadLine());
                angajatCurent.AcordaBonus(bonus);
                Console.WriteLine("Bonus acordat.");
                SalveazaOperatie(angajatCurent.CodAngajat, "acordare bonus", bonus.ToString(), "activ=" + angajatCurent.EsteActiv);
            }
            else if (optiune == 5)
            {
                angajatCurent.ActiveazaAngajat();
                Console.WriteLine("Angajatul a fost activat.");
                SalveazaOperatie(angajatCurent.CodAngajat, "activare", "-", "activ=True");
            }
            else if (optiune == 6)
            {
                angajatCurent.DezactiveazaAngajat();
                Console.WriteLine("Angajatul a fost dezactivat.");
                SalveazaOperatie(angajatCurent.CodAngajat, "dezactivare", "-", "activ=False");
            }
            else if (optiune == 7)
            {
                angajatCurent.AcordaAcces();
                Console.WriteLine("Acces acordat.");
                SalveazaOperatie(angajatCurent.CodAngajat, "acordare acces", "-", "acces=True");
            }
            else if (optiune == 8)
            {
                angajatCurent.RevocaAcces();
                Console.WriteLine("Acces revocat.");
                SalveazaOperatie(angajatCurent.CodAngajat, "revocare acces", "-", "acces=False");
            }
            else if (optiune == 9)
            {
                if (angajatCurent is IAccesAvansat ia)
                {
                    ia.SolicitaAccesSpecial();
                    SalveazaOperatie(angajatCurent.CodAngajat, "solicitare acces special", "-", "activ=" + angajatCurent.EsteActiv);
                }
                else
                {
                    Console.WriteLine("Aceasta operatie nu este disponibila pentru tipul curent de angajat.");
                }
            }
            else if (optiune == 10)
            {
                if (angajatCurent is IAccesAvansat ia)
                {
                    string nivel = ia.VerificaNivelAcces();
                    Console.WriteLine("Nivel acces: " + nivel);
                    SalveazaOperatie(angajatCurent.CodAngajat, "verificare nivel acces", nivel, "activ=" + angajatCurent.EsteActiv);
                }
                else
                {
                    Console.WriteLine("Aceasta operatie nu este disponibila pentru tipul curent de angajat.");
                }
            }
            else if (optiune == 11)
            {
                if (angajatCurent is IEvaluarePerformanta ie)
                {
                    double scor = ie.CalculeazaScorPerformanta();
                    Console.WriteLine("Scor performanta: " + scor);
                    SalveazaOperatie(angajatCurent.CodAngajat, "calcul scor performanta", scor.ToString(), "activ=" + angajatCurent.EsteActiv);
                }
                else
                {
                    Console.WriteLine("Aceasta operatie nu este disponibila pentru tipul curent de angajat.");
                }
            }
            else if (optiune == 12)
            {
                if (angajatCurent is IEvaluarePerformanta ie)
                {
                    ie.GenereazaRaportEvaluare();
                    SalveazaOperatie(angajatCurent.CodAngajat, "generare raport evaluare", "-", "activ=" + angajatCurent.EsteActiv);
                }
                else
                {
                    Console.WriteLine("Aceasta operatie nu este disponibila pentru tipul curent de angajat.");
                }
            }
            else if (optiune == 13)
            {
                if (angajatCurent is Contabil cont)
                {
                    cont.GenereazaRaportFinanciar();
                    SalveazaOperatie(angajatCurent.CodAngajat, "generare raport financiar", "-", "activ=" + angajatCurent.EsteActiv);
                }
                else
                {
                    Console.WriteLine("Aceasta operatie este disponibila doar pentru Contabil.");
                }
            }
            else if (optiune == 14)
            {
                if (angajatCurent is ProgramatorSenior ps)
                {
                    ps.AprobaCereriTehnice();
                    SalveazaOperatie(angajatCurent.CodAngajat, "aprobare cereri tehnice", "-", "activ=" + angajatCurent.EsteActiv);
                }
                else
                {
                    Console.WriteLine("Aceasta operatie este disponibila doar pentru ProgramatorSenior.");
                }
            }
            else if (optiune == 15)
            {
                if (angajatCurent is ProgramatorSenior ps)
                {
                    decimal bonusL = ps.CalculeazaBonusLeadership();
                    Console.WriteLine("Bonus leadership: " + bonusL);
                    SalveazaOperatie(angajatCurent.CodAngajat, "calcul bonus leadership", bonusL.ToString(), "activ=" + angajatCurent.EsteActiv);
                }
                else
                {
                    Console.WriteLine("Aceasta operatie este disponibila doar pentru ProgramatorSenior.");
                }
            }
            else
            {
                Console.WriteLine("Optiune invalida.");
            }

        } while (optiune != 0);
    }
}

