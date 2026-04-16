#include <iostream>
#include <fstream>
#include "ong.h"

using namespace std;

/* Liste globale */
Voluntar* listaVoluntari = nullptr;
Eveniment* listaEvenimente = nullptr;

/*LUCRU LISTE TEXT*/

StrNode* creeazaStrNode(const string& t) {
    StrNode* nd = new StrNode;
    nd->text = t;
    nd->next = nullptr;
    return nd;
}

void adaugaStr(StrNode*& cap, const string& t) {
    StrNode* nd = creeazaStrNode(t);
    nd->next = cap;
    cap = nd;
}

void afiseazaStrList(StrNode* cap) {
    while (cap != nullptr) {
        cout << cap->text;
        if (cap->next) cout << ", ";
        cap = cap->next;
    }
}

void elibereazaStrList(StrNode*& cap) {
    while (cap != nullptr) {
        StrNode* p = cap;
        cap = cap->next;
        delete p;
    }
}

/*SISTEM DE NOTIFICARI*/

Notificare* creeazaNotificare(const string& msg) {
    Notificare* n = new Notificare;
    n->mesaj = msg;
    n->next = nullptr;
    return n;
}

void adaugaNotificare(Voluntar* v, const string& msg) {
    if (!v) return;
    Notificare* n = creeazaNotificare(msg);
    n->next = v->notificari;
    v->notificari = n;
}

void afiseazaNotificari(Notificare* n) {
    if (!n) {
        cout << "   (fara notificari)\n";
        return;
    }
    while (n != nullptr) {
        cout << "   * " << n->mesaj << "\n";
        n = n->next;
    }
}

void elibereazaNotificari(Notificare*& n) {
    while (n != nullptr) {
        Notificare* p = n;
        n = n->next;
        delete p;
    }
}

/*LUCRU CU ACTIVATI*/

Activitate* creeazaActivitate(const string& nume, int ore) {
    Activitate* a = new Activitate;
    a->nume = nume;
    a->ore_lucrate = ore;
    a->next = nullptr;
    return a;
}

void adaugaActivitate(Eveniment* ev, Activitate* a) {
    if (!ev || !a) return;
    a->next = ev->activitati;
    ev->activitati = a;
}

void afiseazaActivitati(Activitate* a) {
    if (!a) {
        cout << "   (fara activitati)\n";
        return;
    }
    while (a != nullptr) {
        cout << "   - " << a->nume << " (" << a->ore_lucrate << " ore)\n";
        a = a->next;
    }
}

bool stergeActivitate(Eveniment* ev, const string& numeAct) {
    if (!ev) return false;
    Activitate* cur = ev->activitati;
    Activitate* prev = nullptr;

    while (cur != nullptr) {
        if (cur->nume == numeAct) {
            if (prev == nullptr)
                ev->activitati = cur->next;
            else
                prev->next = cur->next;
            delete cur;
            return true;
        }
        prev = cur;
        cur = cur->next;
    }
    return false;
}

void elibereazaActivitati(Activitate*& a) {
    while (a != nullptr) {
        Activitate* p = a;
        a = a->next;
        delete p;
    }
}

/*LUCRU CU EVENIMENTE*/

Eveniment* creeazaEveniment(const string& titlu) {
    Eveniment* e = new Eveniment;
    e->titlu = titlu;
    e->activitati = nullptr;
    e->next = nullptr;
    return e;
}

void adaugaEveniment(Eveniment* e) {
    if (!e) return;
    e->next = listaEvenimente;
    listaEvenimente = e;
}

void afiseazaEvenimente() {
    cout << "\n===== EVENIMENTE =====\n";
    Eveniment* e = listaEvenimente;
    if (!e) {
        cout << "Nu exista evenimente.\n";
        return;
    }
    while (e != nullptr) {
        cout << "Eveniment: " << e->titlu << "\n";
        afiseazaActivitati(e->activitati);
        cout << "------------------\n";
        e = e->next;
    }
}

Eveniment* cautaEveniment(const string& titlu) {
    Eveniment* e = listaEvenimente;
    while (e != nullptr) {
        if (e->titlu == titlu) return e;
        e = e->next;
    }
    return nullptr;
}

bool stergeEveniment(const string& titlu) {
    Eveniment* cur = listaEvenimente;
    Eveniment* prev = nullptr;

    while (cur != nullptr) {
        if (cur->titlu == titlu) {
            if (prev == nullptr)
                listaEvenimente = cur->next;
            else
                prev->next = cur->next;

            elibereazaActivitati(cur->activitati);
            delete cur;
            return true;
        }
        prev = cur;
        cur = cur->next;
    }
    return false;
}

int numarEvenimente() {
    int nr = 0;
    Eveniment* e = listaEvenimente;
    while (e != nullptr) {
        nr++;
        e = e->next;
    }
    return nr;
}

Eveniment* evenimentMaxOre() {
    if (!listaEvenimente) return nullptr;

    Eveniment* best = nullptr;
    int maxOre = -1;

    Eveniment* e = listaEvenimente;
    while (e != nullptr) {
        int total = 0;
        Activitate* a = e->activitati;
        while (a != nullptr) {
            total += a->ore_lucrate;
            a = a->next;
        }
        if (total > maxOre) {
            maxOre = total;
            best = e;
        }
        e = e->next;
    }
    return best;
}
/*LUCRU CU VOLUNTARI*/

Voluntar* creeazaVoluntar(const string& nume) {
    Voluntar* v = new Voluntar;
    v->nume = nume;
    v->domenii = nullptr;
    v->total_ore = 0;
    v->notificari = nullptr;
    v->next = nullptr;
    return v;
}

void adaugaVoluntar(Voluntar* v) {
    if (!v) return;
    v->next = listaVoluntari;
    listaVoluntari = v;
}

void afiseazaVoluntari() {
    cout << "\n===== VOLUNTARI =====\n";
    Voluntar* v = listaVoluntari;
    if (!v) {
        cout << "Nu exista voluntari.\n";
        return;
    }
    while (v != nullptr) {
        cout << "Voluntar: " << v->nume << "\n";
        cout << "Domenii: ";
        if (v->domenii)
            afiseazaStrList(v->domenii);
        else
            cout << "(niciun domeniu)";
        cout << "\nTotal ore lucrate: " << v->total_ore << "\n";
        cout << "Notificari:\n";
        afiseazaNotificari(v->notificari);
        cout << "------------------\n";
        v = v->next;
    }
}

Voluntar* cautaVoluntar(const string& nume) {
    Voluntar* v = listaVoluntari;
    while (v != nullptr) {
        if (v->nume == nume) return v;
        v = v->next;
    }
    return nullptr;
}

bool stergeVoluntar(const string& nume) {
    Voluntar* cur = listaVoluntari;
    Voluntar* prev = nullptr;

    while (cur != nullptr) {
        if (cur->nume == nume) {
            if (prev == nullptr)
                listaVoluntari = cur->next;
            else
                prev->next = cur->next;

            elibereazaStrList(cur->domenii);
            elibereazaNotificari(cur->notificari);
            delete cur;
            return true;
        }
        prev = cur;
        cur = cur->next;
    }
    return false;
}

void adaugaOreVoluntar(Voluntar* v, Eveniment* ev,
    const string& act, int ore) {
    if (!v || !ev) return;
    Activitate* a = creeazaActivitate(act, ore);
    adaugaActivitate(ev, a);
    v->total_ore += ore;
}

/*STATISTICA*/

int numarVoluntari() {
    int nr = 0;
    Voluntar* v = listaVoluntari;
    while (v != nullptr) {
        nr++;
        v = v->next;
    }
    return nr;
}

int numarActivitatiTotale() {
    int nr = 0;
    Eveniment* e = listaEvenimente;
    while (e != nullptr) {
        Activitate* a = e->activitati;
        while (a != nullptr) {
            nr++;
            a = a->next;
        }
        e = e->next;
    }
    return nr;
}

int totalOreONG() {
    int total = 0;
    Voluntar* v = listaVoluntari;
    while (v != nullptr) {
        total += v->total_ore;
        v = v->next;
    }
    return total;
}

Voluntar* voluntarTop() {
    if (!listaVoluntari) return nullptr;
    Voluntar* best = listaVoluntari;
    Voluntar* v = listaVoluntari->next;
    while (v != nullptr) {
        if (v->total_ore > best->total_ore)
            best = v;
        v = v->next;
    }
    return best;
}

/*LUCRU CU FISIERE*/

void salveazaVoluntari() {
    ofstream f("voluntari.txt");
    if (!f) return;

    Voluntar* v = listaVoluntari;
    while (v != nullptr) {
        f << v->nume << "\n";
        f << v->total_ore << "\n";

        StrNode* d = v->domenii;
        while (d != nullptr) {
            f << "DOM:" << d->text << "\n"; // DOM - DOMENIU in fisierul de iesire
            d = d->next;
        }

        Notificare* n = v->notificari;
        while (n != nullptr) {
            f << "NOT:" << n->mesaj << "\n";// NOT - NOTIFICARE in fisierul de iesire
            n = n->next;
        }

        f << "END\n";// END - FINAL
        v = v->next;
    }
}

void incarcaVoluntari() {
    ifstream f("voluntari.txt");
    if (!f) return;

    string line;
    while (true) {
        if (!getline(f, line)) break;
        if (line == "") continue;

        Voluntar* v = creeazaVoluntar(line);

        if (!getline(f, line)) { delete v; break; }
        v->total_ore = stoi(line);

        while (getline(f, line) && line != "END") {
            if (line.rfind("DOM:", 0) == 0) {
                string domeniu = line.substr(4);
                adaugaStr(v->domenii, domeniu);
            }
            else if (line.rfind("NOT:", 0) == 0) {
                string msg = line.substr(4);
                adaugaNotificare(v, msg);
            }
        }

        adaugaVoluntar(v);
    }
}

void salveazaEvenimente() {
    ofstream f("evenimente.txt");
    if (!f) return;

    Eveniment* e = listaEvenimente;
    while (e != nullptr) {
        f << e->titlu << "\n";

        Activitate* a = e->activitati;
        while (a != nullptr) {
            f << a->nume << " " << a->ore_lucrate << "\n";
            a = a->next;
        }

        f << "END\n";
        e = e->next;
    }
}

void incarcaEvenimente() {
    ifstream f("evenimente.txt");
    if (!f) return;

    string titlu, linie;
    while (true) {
        if (!getline(f, titlu)) break;
        if (titlu == "") continue;

        Eveniment* e = creeazaEveniment(titlu);

        while (getline(f, linie) && linie != "END") {
            int sp = -1;
            for (int i = 0; i < (int)linie.size(); i++) {
                if (linie[i] == ' ') {
                    sp = i;
                    break;
                }
            }
            if (sp == -1) continue;

            string nume = linie.substr(0, sp);
            string oreStr = linie.substr(sp + 1);
            int ore = 0;
            if (oreStr != "") ore = stoi(oreStr);

            adaugaActivitate(e, creeazaActivitate(nume, ore));
        }

        adaugaEveniment(e);
    }
}


/*ELIBERARE MEMORIE*/

void elibereazaVoluntari() {
    while (listaVoluntari != nullptr) {
        Voluntar* p = listaVoluntari;
        listaVoluntari = listaVoluntari->next;
        elibereazaStrList(p->domenii);
        elibereazaNotificari(p->notificari);
        delete p;
    }
}

void elibereazaEvenimente() {
    while (listaEvenimente != nullptr) {
        Eveniment* p = listaEvenimente;
        listaEvenimente = listaEvenimente->next;
        elibereazaActivitati(p->activitati);
        delete p;
    }
}

void elibereazaMemorie() {
    elibereazaVoluntari();
    elibereazaEvenimente();
}

/*functie pt infrumusetare dupa alegera unei optiuni se curata ecranul*/

void clearScreen() {
    system("cls");
}

/*MENIU*/

void meniu() {
    while (true) {
        cout << "\n====== MENIU APLICATIE MANAGEMENT ONG ======\n";
        cout << "1. Adauga voluntar\n";
        cout << "2. Afiseaza voluntari\n";
        cout << "3. Adauga eveniment\n";
        cout << "4. Afiseaza evenimente\n";
        cout << "5. Adauga activitate la eveniment\n";
        cout << "6. Inregistreaza ore voluntar\n";
        cout << "7. Statistica\n";
        cout << "8. Sterge voluntar\n";
        cout << "9. Sterge eveniment\n";
        cout << "10. Salvare date\n";
        cout << "11. Incarcare date\n";
        cout << "0. Iesire\n";

        int opt;
        cin >> opt;
        clearScreen();
        if (opt == 0) {
            elibereazaMemorie();
            break;
        }
        if (opt == 1) {
            string nume, domeniu;
            cout << "Nume voluntar: ";
            cin >> nume;

            Voluntar* v = creeazaVoluntar(nume);

            cout << "Domeniu (educatie/mediu/social): ";
            cin >> domeniu;
            adaugaStr(v->domenii, domeniu);

            adaugaVoluntar(v);

            cout << "Voluntar adaugat!\n";
        }
        else if (opt == 2) {
            afiseazaVoluntari();
        }
        else if (opt == 3) {
            string titlu;
            cout << "Titlu eveniment: ";
            cin >> titlu;
            Eveniment* e = creeazaEveniment(titlu);
            adaugaEveniment(e);
            Voluntar* v = listaVoluntari;
            while (v != nullptr) {
                adaugaNotificare(v, "A fost adaugat evenimentul: " + titlu);
                v = v->next;
            }

            cout << "Eveniment adaugat!\n";
        }
        else if (opt == 4) {
            afiseazaEvenimente();
        }
        else if (opt == 5) {
            string titlu, act;
            int ore;

            cout << "Eveniment: ";
            cin >> titlu;
            Eveniment* e = cautaEveniment(titlu);
            if (!e) {
                cout << "Nu exista eveniment!\n";
                continue;
            }

            cout << "Activitate (un cuvant): ";
            cin >> act;

            cout << "Ore lucrate: ";
            cin >> ore;

            adaugaActivitate(e, creeazaActivitate(act, ore));

            /* notificare pt toti voluntarii */
            Voluntar* v = listaVoluntari;
            while (v != nullptr) {
                adaugaNotificare(v, "Evenimentul " + titlu +
                    " are o noua activitate: " + act);
                v = v->next;
            }

            cout << "Activitate adaugata!\n";
        }
        else if (opt == 6) {
            string nume, titlu, act;
            int ore;

            cout << "Nume voluntar: ";
            cin >> nume;
            Voluntar* v = cautaVoluntar(nume);
            if (!v) {
                cout << "Ne pare rau, nu exista acest voluntar :( \n";
                continue;
            }

            cout << "Eveniment: ";
            cin >> titlu;
            Eveniment* e = cautaEveniment(titlu);
            if (!e) {
                cout << "Ne pare rau, nu exista acest eveniment :( \n";
                continue;
            }

            cout << "Activitate: ";
            cin >> act;

            cout << "Ore lucrate: ";
            cin >> ore;

            adaugaOreVoluntar(v, e, act, ore);

            cout << "Ore inregistrate!\n";
        }
        else if (opt == 7) {
            clearScreen();
            cout << "=========== SUBMENIU STATISTICĂ ===========" << endl;
            cout << "1. Numar total de voluntari" << endl;
            cout << "2. Numar total de evenimente" << endl;
            cout << "3. Numar total de activitati" << endl;
            cout << "4. Total ore lucrate in ONG" << endl;
            cout << "5. Evenimentul cu cele mai multe ore" << endl;
            cout << "6. Voluntarul cu cele mai multe ore" << endl;
            cout << "0. Intoarcere la meniu" << endl;

            int st;
            cin >> st;
            clearScreen();

            if (st == 0) {
                // nimic, doar revenim
            }
            else if (st == 1) {
                cout << "Total voluntari: " << numarVoluntari() << "\n";
            }
            else if (st == 2) {
                cout << "Total evenimente: " << numarEvenimente() << "\n";
            }
            else if (st == 3) {
                cout << "Total activitati: " << numarActivitatiTotale() << "\n";
            }
            else if (st == 4) {
                cout << "Total ore lucrate in ONG: " << totalOreONG() << "\n";
            }
            else if (st == 5) {
                Eveniment* ev = evenimentMaxOre();
                if (ev)
                    cout << "Eveniment cu cele mai multe ore este: "
                    << ev->titlu << "\n";
                else
                    cout << "Nu exista evenimente!\n";
            }
            else if (st == 6) {
                Voluntar* top = voluntarTop();
                if (top)
                    cout << "Voluntarul cu cele mai multe ore: "
                    << top->nume << " (" << top->total_ore << " ore)\n";
                else
                    cout << "Nu exista voluntari!\n";
            }
        }
        else if (opt == 8) {
            string nume;
            cout << "Nume voluntar de sters: ";
            cin >> nume;
            if (stergeVoluntar(nume))
                cout << "Voluntar sters.\n";
            else
                cout << "Voluntarul nu a fost gasit.\n";
        }
        else if (opt == 9) {
            string titlu;
            cout << "Titlu eveniment de sters: ";
            cin >> titlu;
            if (stergeEveniment(titlu))
                cout << "Eveniment sters.\n";
            else
                cout << "Evenimentul nu a fost gasit.\n";
        }
        else if (opt == 10) {
            salveazaVoluntari();
            salveazaEvenimente();
            cout << "Date salvate in fisiere.\n";
        }
        else if (opt == 11) {
            incarcaVoluntari();
            incarcaEvenimente();
            cout << "Date incarcate din fisiere.\n";
        }

        cout << "\nApasa Enter ca sa continui...";
        cin.ignore();
        cin.get();
        clearScreen();
    }
}
