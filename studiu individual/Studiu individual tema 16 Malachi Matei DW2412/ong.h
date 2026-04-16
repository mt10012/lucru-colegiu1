#include <string>
using namespace std;

/* LISTE TEXT */
struct StrNode {
    string text;
    StrNode* next;
};

/* NOTIFICĂRI */
struct Notificare {
    string mesaj;
    Notificare* next;
};

/* ACTIVITĂȚI */
struct Activitate {
    string nume;
    int ore_lucrate;
    Activitate* next;
};

/* EVENIMENTE */
struct Eveniment {
    string titlu;
    Activitate* activitati;
    Eveniment* next;
};

/* VOLUNTARI */
struct Voluntar {
    string nume;
    StrNode* domenii;
    int total_ore;
    Notificare* notificari;
    Voluntar* next;
};

/* Liste globale */
extern Voluntar* listaVoluntari;
extern Eveniment* listaEvenimente;

/* LISTE TEXT */
StrNode* creeazaStrNode(const string& t);
void adaugaStr(StrNode*& cap, const string& t);
void afiseazaStrList(StrNode* cap);
void elibereazaStrList(StrNode*& cap);

/* NOTIFICĂRI */
Notificare* creeazaNotificare(const string& msg);
void adaugaNotificare(Voluntar* v, const string& msg);
void afiseazaNotificari(Notificare* n);
void elibereazaNotificari(Notificare*& n);

/* ACTIVITĂȚI */
Activitate* creeazaActivitate(const string& nume, int ore);
void adaugaActivitate(Eveniment* ev, Activitate* a);
void afiseazaActivitati(Activitate* a);
bool stergeActivitate(Eveniment* ev, const string& numeAct);
void elibereazaActivitati(Activitate*& a);

/* EVENIMENTE */
Eveniment* creeazaEveniment(const string& titlu);
void adaugaEveniment(Eveniment* e);
void afiseazaEvenimente();
Eveniment* cautaEveniment(const string& titlu);
bool stergeEveniment(const string& titlu);
int numarEvenimente();
Eveniment* evenimentMaxOre();
void elibereazaEvenimente();

/* VOLUNTARI */
Voluntar* creeazaVoluntar(const string& nume);
void adaugaVoluntar(Voluntar* v);
void afiseazaVoluntari();
Voluntar* cautaVoluntar(const string& nume);
bool stergeVoluntar(const string& nume);
void adaugaOreVoluntar(Voluntar* v, Eveniment* ev, const string& act, int ore);
int numarVoluntari();
int numarActivitatiTotale();
int totalOreONG();
Voluntar* voluntarTop();
void elibereazaVoluntari();

/* FIȘIERE */
void salveazaVoluntari();
void incarcaVoluntari();
void salveazaEvenimente();
void incarcaEvenimente();

/* UTILE */
void clearScreen();
void elibereazaMemorie();

/* MENIU */
void meniu();
