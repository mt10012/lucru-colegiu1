using System;
using System.Collections.Generic;
using System.IO;

namespace ManagerBiblioteca
{
    class Carte
    {
        private string titlu;
        private string autor;
        private int anPublicare;
        private double pret;
        private string gen;

        public string Titlu
        {
            get { return titlu; }
            set { titlu = value != "" ? value : "Necunoscut"; }
        }
        public string Autor
        {
            get { return autor; }
            set { autor = value != "" ? value : "Anonim"; }
        }
        public int AnPublicare
        {
            get { return anPublicare; }
            set { anPublicare = value > 0 ? value : 2000; }
        }
        public double Pret
        {
            get { return pret; }
            set { pret = value >= 0 ? value : 0; }
        }
        public string Gen
        {
            get { return gen; }
            set { gen = value != "" ? value : "General"; }
        }
        public Carte(string t, string a, int an, double p, string g)
        {
            Titlu = t;
            Autor = a;
            AnPublicare = an;
            Pret = p;
            Gen = g;
        }
        public Carte(Carte alta)
        {
            Titlu = alta.Titlu;
            Autor = alta.Autor;
            AnPublicare = alta.AnPublicare;
            Pret = alta.Pret;
            Gen = alta.Gen;
        }
        public void Afisare()
        {
            Console.WriteLine("| {0,-30} | {1,-20} | {2,-6} | {3,8:F2} lei | {4,-15} |",
                Titlu, Autor, AnPublicare, Pret, Gen);
        }
        public static Carte CitireFisier(string linie)
        {
            string[] p = linie.Split(';');
            return new Carte(p[0], p[1], int.Parse(p[2]), double.Parse(p[3]), p[4]);
        }
        public void InscriereFisier(string numeFisier)
        {
            StreamWriter sw = new StreamWriter(numeFisier, true);
            sw.WriteLine(Titlu + ";" + Autor + ";" + AnPublicare + ";" + Pret + ";" + Gen);
            sw.Close();
        }
    }
    class Utilizator
    {
        private string nume;
        private List<Carte> cartiImprumutate;
        private int limitaMaxima;
        public string Nume
        {
            get { return nume; }
            set { nume = value != "" ? value : "Utilizator"; }
        }
        public List<Carte> CartiImprumutate
        {
            get { return cartiImprumutate; }
        }
        public Utilizator(string n, int limita = 3)
        {
            Nume = n;
            limitaMaxima = limita;
            cartiImprumutate = new List<Carte>();
        }
        public bool ImprumutaCarte(List<Carte> biblioteca, string titlu)
        {
            if (cartiImprumutate.Count >= limitaMaxima)
            {
                Console.WriteLine("Nu poti imprumuta mai mult de " + limitaMaxima + " carti!");
                return false;
            }
            Carte gasita = biblioteca.Find(c => c.Titlu == titlu);
            if (gasita == null)
            {
                Console.WriteLine("Cartea '" + titlu + "' nu a fost gasita in biblioteca.");
                return false;
            }
            cartiImprumutate.Add(gasita);
            biblioteca.Remove(gasita);
            Console.WriteLine("Cartea '" + titlu + "' a fost imprumutata cu succes de " + Nume + ".");
            return true;
        }
        public void ReturneazaCarte(List<Carte> biblioteca, string titlu)
        {
            Carte gasita = cartiImprumutate.Find(c => c.Titlu == titlu);
            if (gasita == null)
            {
                Console.WriteLine("Nu ai aceasta carte imprumutata.");
                return;
            }
            biblioteca.Add(gasita);
            cartiImprumutate.Remove(gasita);
            Console.WriteLine("Cartea '" + titlu + "' a fost returnata in biblioteca.");
        }
        public void AfisareCartiImprumutate()
        {
            Console.WriteLine("\nCartile imprumutate de " + Nume + " (" + cartiImprumutate.Count + "/" + limitaMaxima + "):");
            if (cartiImprumutate.Count == 0)
                Console.WriteLine("  (nicio carte imprumutata)");
            else
                foreach (Carte c in cartiImprumutate)
                    c.Afisare();
        }
    }
    class Program
    {
        static List<Carte> lista = new List<Carte>();
        static string fisier = "carti.txt";
        static void InscriereFisier()
        {
            StreamWriter sw = new StreamWriter(fisier, false);
            foreach (Carte c in lista)
                sw.WriteLine(c.Titlu + ";" + c.Autor + ";" + c.AnPublicare + ";" + c.Pret + ";" + c.Gen);
            sw.Close();
            Console.WriteLine("Lista a fost salvata in fisier.");
        }
        static void InscriereFisierAutor(string autor)
        {
            string numeFisier = "carti_" + autor + ".txt";
            StreamWriter sw = new StreamWriter(numeFisier, false);
            foreach (Carte c in lista)
                if (c.Autor == autor)
                    sw.WriteLine(c.Titlu + ";" + c.Autor + ";" + c.AnPublicare + ";" + c.Pret + ";" + c.Gen);
            sw.Close();
            Console.WriteLine("Cartile autorului '" + autor + "' au fost salvate in " + numeFisier);
        }
        static void InscriereFisierAn(int an)
        {
            string numeFisier = "carti_" + an + ".txt";
            StreamWriter sw = new StreamWriter(numeFisier, false);
            foreach (Carte c in lista)
                if (c.AnPublicare == an)
                    sw.WriteLine(c.Titlu + ";" + c.Autor + ";" + c.AnPublicare + ";" + c.Pret + ";" + c.Gen);
            sw.Close();
            Console.WriteLine("Cartile din anul " + an + " au fost salvate in " + numeFisier);
        }
        static void AfisareTabel()
        {
            Console.WriteLine("\n" + new string('-', 100));
            Console.WriteLine("| {0,-30} | {1,-20} | {2,-6} | {3,13} | {4,-15} |",
                "TITLU", "AUTOR", "AN", "PRET", "GEN");
            Console.WriteLine(new string('-', 100));
            foreach (Carte c in lista)
                c.Afisare();
            Console.WriteLine(new string('-', 100));
            Console.WriteLine("Total carti: " + lista.Count);
        }
        static void AfisareMeniu()
        {
            Console.WriteLine("\n========== MENIU BIBLIOTECA ==========");
            Console.WriteLine("1.  Afiseaza toate cartile");
            Console.WriteLine("2.  Adauga o carte");
            Console.WriteLine("3.  Cauta dupa titlu");
            Console.WriteLine("4.  Elimina o carte dupa titlu");
            Console.WriteLine("5.  Elimina carti mai vechi decat un an");
            Console.WriteLine("6.  Inserare la pozitie specifica");
            Console.WriteLine("7.  Sortare dupa pret (crescator)");
            Console.WriteLine("8.  Sortare dupa pret (descrescator)");
            Console.WriteLine("9.  Sortare dupa an publicare");
            Console.WriteLine("10. Filtrare dupa pret minim");
            Console.WriteLine("11. Filtrare dupa interval de ani");
            Console.WriteLine("12. Verifica existenta unui autor");
            Console.WriteLine("13. Afiseaza autori unici");
            Console.WriteLine("14. Copiere lista (shallow vs deep copy)");
            Console.WriteLine("15. Sterge toata lista");
            Console.WriteLine("16. Salveaza in fisier");
            Console.WriteLine("17. Salveaza dupa autor");
            Console.WriteLine("18. Salveaza dupa an");
            Console.WriteLine("19. Gestionare utilizatori (imprumut)");
            Console.WriteLine("0.  Iesire");
            Console.Write("Alegeti optiunea: ");
        }
        static void Main()
        {
            lista.Add(new Carte("Ion", "Liviu Rebreanu", 1920, 45.99, "Roman"));
            lista.Add(new Carte("Maitreyi", "Mircea Eliade", 1933, 38.50, "Roman"));
            lista.Add(new Carte("Enigma Otiliei", "George Calinescu", 1938, 52.00, "Roman"));

            if (File.Exists(fisier))
            {
                StreamReader sr = new StreamReader(fisier);
                string linie;
                while ((linie = sr.ReadLine()) != null)
                {
                    if (linie.Trim() != "")
                    {
                        Carte c = Carte.CitireFisier(linie);
                        lista.Add(c);
                    }
                }
                sr.Close();
                Console.WriteLine("Carti incarcate din fisier.");
            }

            Utilizator utilizator1 = new Utilizator("Ana", 2);
            Utilizator utilizator2 = new Utilizator("Mihai", 3);

            bool rulare = true;
            while (rulare)
            {
                AfisareMeniu();
                string optiune = Console.ReadLine();

                switch (optiune)
                {
                    case "1":
                        AfisareTabel();
                        break;

                    case "2":
                        Console.Write("Titlu: ");
                        string t = Console.ReadLine();
                        Console.Write("Autor: ");
                        string a = Console.ReadLine();
                        Console.Write("An publicare: ");
                        int an = int.Parse(Console.ReadLine());
                        Console.Write("Pret: ");
                        double p = double.Parse(Console.ReadLine());
                        Console.Write("Gen literar: ");
                        string g = Console.ReadLine();
                        lista.Add(new Carte(t, a, an, p, g));
                        Console.WriteLine("Cartea a fost adaugata!");
                        break;

                    case "3":
                        Console.Write("Introduceti titlul (sau o parte): ");
                        string cauta = Console.ReadLine().ToLower();
                        List<Carte> rezultate = lista.FindAll(c => c.Titlu.ToLower().Contains(cauta));
                        if (rezultate.Count == 0)
                            Console.WriteLine("Nu s-a gasit nicio carte.");
                        else
                        {
                            Console.WriteLine("Rezultate gasite: " + rezultate.Count);
                            foreach (Carte c in rezultate)
                                c.Afisare();
                        }
                        break;

                    case "4":
                        Console.Write("Titlul cartii de eliminat: ");
                        string titluSterg = Console.ReadLine();
                        int sterse = lista.RemoveAll(c => c.Titlu == titluSterg);
                        Console.WriteLine(sterse > 0 ? "Cartea a fost eliminata." : "Cartea nu a fost gasita.");
                        break;

                    case "5":
                        Console.Write("Eliminati cartile mai vechi decat anul: ");
                        int anMinim = int.Parse(Console.ReadLine());
                        int numarSterse = lista.RemoveAll(c => c.AnPublicare < anMinim);
                        Console.WriteLine("Au fost eliminate " + numarSterse + " carti.");
                        break;

                    case "6":
                        Console.Write("Titlu: ");
                        string t2 = Console.ReadLine();
                        Console.Write("Autor: ");
                        string a2 = Console.ReadLine();
                        Console.Write("An publicare: ");
                        int an2 = int.Parse(Console.ReadLine());
                        Console.Write("Pret: ");
                        double p2 = double.Parse(Console.ReadLine());
                        Console.Write("Gen literar: ");
                        string g2 = Console.ReadLine();
                        Console.Write("Pozitia de inserare (0 - " + lista.Count + "): ");
                        int pozitie = int.Parse(Console.ReadLine());

                        if (pozitie < 0 || pozitie > lista.Count)
                            Console.WriteLine("Pozitie invalida! Trebuie sa fie intre 0 si " + lista.Count);
                        else
                        {
                            lista.Insert(pozitie, new Carte(t2, a2, an2, p2, g2));
                            Console.WriteLine("Cartea a fost inserata la pozitia " + pozitie + ".");
                        }
                        break;

                    case "7":
                        lista.Sort((x, y) => x.Pret.CompareTo(y.Pret));
                        Console.WriteLine("Lista sortata dupa pret (crescator):");
                        AfisareTabel();
                        break;

                    case "8":
                        lista.Sort((x, y) => y.Pret.CompareTo(x.Pret));
                        Console.WriteLine("Lista sortata dupa pret (descrescator):");
                        AfisareTabel();
                        break;

                    case "9":
                        lista.Sort((x, y) => x.AnPublicare.CompareTo(y.AnPublicare));
                        Console.WriteLine("Lista sortata dupa an publicare:");
                        AfisareTabel();
                        break;

                    case "10":
                        Console.Write("Afiseaza cartile cu pretul mai mare decat: ");
                        double pretMinim = double.Parse(Console.ReadLine());
                        List<Carte> scumpe = lista.FindAll(c => c.Pret > pretMinim);
                        Console.WriteLine("Carti cu pret > " + pretMinim + ": " + scumpe.Count);
                        foreach (Carte c in scumpe)
                            c.Afisare();
                        break;

                    case "11":
                        Console.Write("An de inceput: ");
                        int anStart = int.Parse(Console.ReadLine());
                        Console.Write("An de sfarsit: ");
                        int anEnd = int.Parse(Console.ReadLine());
                        List<Carte> interval = lista.FindAll(c => c.AnPublicare >= anStart && c.AnPublicare <= anEnd);
                        Console.WriteLine("Carti publicate intre " + anStart + " si " + anEnd + ": " + interval.Count);
                        foreach (Carte c in interval)
                            c.Afisare();
                        break;

                    case "12":
                        Console.Write("Introduceti numele autorului: ");
                        string autorCautat = Console.ReadLine();
                        bool exista = lista.Exists(c => c.Autor == autorCautat);
                        if (exista)
                            Console.WriteLine("Autorul '" + autorCautat + "' are carti in biblioteca.");
                        else
                            Console.WriteLine("Autorul '" + autorCautat + "' nu are carti in biblioteca.");
                        break;

                    case "13":
                        List<string> autoriUnici = new List<string>();
                        foreach (Carte c in lista)
                        {
                            if (!autoriUnici.Contains(c.Autor))
                                autoriUnici.Add(c.Autor);
                        }
                        Console.WriteLine("\nAutori unici in biblioteca (" + autoriUnici.Count + "):");
                        foreach (string autor in autoriUnici)
                            Console.WriteLine("  - " + autor);
                        break;

                    case "14":
                        List<Carte> copieSuperficiala = new List<Carte>(lista);
                        Console.WriteLine("Copie superficiala creata cu " + copieSuperficiala.Count + " carti.");
                        Console.WriteLine("Obiectele din shallow copy sunt aceleasi ca in lista originala.");

                        List<Carte> copieAdanca = new List<Carte>();
                        foreach (Carte c in lista)
                            copieAdanca.Add(new Carte(c));
                        Console.WriteLine("Copie adanca creata cu " + copieAdanca.Count + " carti.");
                        Console.WriteLine("Obiectele din deep copy sunt independente de cele originale.");
                        break;

                    case "15":
                        Console.Write("Esti sigur ca vrei sa stergi toata lista? (da/nu): ");
                        string confirmare = Console.ReadLine().ToLower();
                        if (confirmare == "da")
                        {
                            List<Carte> referintaVeche = lista;
                            lista.Clear();
                            Console.WriteLine("Dupa Clear(), referinta veche arata " + referintaVeche.Count + " elemente.");
                            lista = new List<Carte>();
                            Console.WriteLine("Lista noua creata. Toate cartile au fost sterse.");
                        }
                        else
                        {
                            Console.WriteLine("Operatia a fost anulata.");
                        }
                        break;

                    case "16":
                        InscriereFisier();
                        break;

                    case "17":
                        Console.Write("Autorul ale carui carti se vor salva: ");
                        string autorSalv = Console.ReadLine();
                        InscriereFisierAutor(autorSalv);
                        break;

                    case "18":
                        Console.Write("Anul cartilor de salvat: ");
                        int anSalv = int.Parse(Console.ReadLine());
                        InscriereFisierAn(anSalv);
                        break;

                    case "19":
                        Console.WriteLine("\n--- Gestionare Useri ---");
                        Console.WriteLine("1. " + utilizator1.Nume + " (limita: 3 carti)");
                        Console.WriteLine("2. " + utilizator2.Nume + " (limita: 3 carti)");
                        Console.Write("Alegeti utilizatorul (1/2): ");
                        string alegere = Console.ReadLine();

                        Utilizator utilizatorActiv = alegere == "1" ? utilizator1 : utilizator2;

                        Console.WriteLine("\n1. Imprumuta o carte");
                        Console.WriteLine("2. Returneaza o carte");
                        Console.WriteLine("3. Afiseaza cartile imprumutate");
                        Console.Write("Optiune: ");
                        string actiune = Console.ReadLine();

                        if (actiune == "1")
                        {
                            Console.Write("Titlul cartii de imprumutat: ");
                            string titluImprumut = Console.ReadLine();
                            utilizatorActiv.ImprumutaCarte(lista, titluImprumut);
                        }
                        else if (actiune == "2")
                        {
                            Console.Write("Titlul cartii de returnat: ");
                            string titluReturn = Console.ReadLine();
                            utilizatorActiv.ReturneazaCarte(lista, titluReturn);
                        }
                        else if (actiune == "3")
                        {
                            utilizatorActiv.AfisareCartiImprumutate();
                        }
                        break;

                    case "0":
                        rulare = false;
                        Console.WriteLine("La revedere!");
                        break;
                }
            }
        }
    }
}