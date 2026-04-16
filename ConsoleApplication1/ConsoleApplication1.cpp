#include <iostream>
#include <cmath>
using namespace std;

int nr_C(int b) {
    int nr = 0;
    while (b) {
        b /= 10;
        nr++;
    }
    return nr;
}

int nr_CP(int d) {
    int nrP = 0;
    int nrI = 0;

    while (d) {
        if ((d % 10) % 2 == 0)
            nrP++;
        else
            nrI++;
        d /= 10;
    }
    cout << "Numarul de cifre impare este: " << nrI << endl;
    return nrP;
}

int suma_cifrelor(int n) {
    int suma = 0;
    while (n) {
        suma += n % 10;
        n /= 10;
    }
    return suma;
}

int cifra_maxima(int n) {
    int maxCifra = 0;
    while (n) {
        int cifra = n % 10;
        if (cifra > maxCifra)
            maxCifra = cifra;
        n /= 10;
    }
    return maxCifra;
}

int cifra_minima(int n) {
    int minCifra = 9;
    while (n) {
        int cifra = n % 10;
        if (cifra < minCifra)
            minCifra = cifra;
        n /= 10;
    }
    return minCifra;
}

double media_aritmetica_cifrelor(int n) {
    int suma = 0;
    int nrCifre = 0;
    while (n) {
        suma += n % 10;
        nrCifre++;
        n /= 10;
    }
    return (suma) / nrCifre;
}

int cifre_repetate(int n) {
    int frecventa[10] = { 0 };
    while (n) {
        frecventa[n % 10]++;
        n /= 10;
    }
    int repetate = 0;
    for (int i = 0; i < 10; i++) {
        if (frecventa[i] >= 2) {
            cout << i << " ";
            repetate = 1;
        }
    }
    return repetate;
}

int afiseaza_cifre(int n) {
    cout << "Cifrele numarului sunt: ";
    int cifre[10], i = 0;
    while (n) {
        cifre[i++] = n % 10;
        n /= 10;
    }
    for (int j = i - 1; j >= 0; j--) {
        cout << cifre[j] << " ";
    }
    cout << endl;
    return 0;
}

void divizori(int n) {
    cout << "Divizorii numarului sunt: ";
    for (int i = 1; i <= n; i++) {
        if (n % i == 0) {
            cout << i << " ";
        }
    }
    cout << endl;
}

int inversul_numarului(int n) {
    int invers = 0;
    while (n) {
        invers = invers * 10 + n % 10;
        n /= 10;
    }
    return invers;
}

bool este_prim(int n) {
    if (n <= 1) return false;
    for (int i = 2; i <= sqrt(n); i++) {
        if (n % i == 0) return false;
    }
    return true;
}

int cel_mai_mare_numar(int n) {
    int cifre[10], i = 0;
    while (n) {
        cifre[i++] = n % 10;
        n /= 10;
    }
    int numar_maxim = 0;
    while (i > 0) {
        int maxCifra = -1;
        int id = -1;
        for (int j = 0; j < i; j++) {
            if (cifre[j] > maxCifra) {
                maxCifra = cifre[j];
                id = j;
            }
        }
        numar_maxim = numar_maxim * 10 + maxCifra;
        for (int k = id; k < i - 1; k++) {
            cifre[k] = cifre[k + 1];
        }
        i--;
    }
    return numar_maxim;
}

int main() {
    int n;
    cout << "Introduceti un numar n: ";
    cin >> n;

    int cifre = nr_C(n);
    int cifrePare = nr_CP(n);
    int sumaCifrelor = suma_cifrelor(n);
    int maxCifra = cifra_maxima(n);
    int minCifra = cifra_minima(n);
    double mediaCifrelor = media_aritmetica_cifrelor(n);
    cout << "Numarul de cifre este: " << cifre << endl;
    cout << "Numarul de cifre pare este: " << cifrePare << endl;
    cout << "Suma cifrelor este: " << sumaCifrelor << endl;
    cout << "Cifra maxima este: " << maxCifra << endl;
    cout << "Cifra minima este: " << minCifra << endl;
    cout << "Media aritmetica a cifrelor este: " << mediaCifrelor << endl;

    if (!cifre_repetate(n)) cout << "Nu exista cifre repetate." << endl;
    afiseaza_cifre(n);
    divizori(n);

    int invers = inversul_numarului(n);
    cout << "Inversul numarului este: " << invers << endl;

    if (este_prim(n)) {
        cout << "Numarul este PRIM." << endl;
    }
    else {
        cout << "Numarul nu este PRIM." << endl;
    }

    int numar_maxim = cel_mai_mare_numar(n);
    cout << "Cel mai mare numar posibil din aceste cifre este: " << numar_maxim << endl;

    return 0;
}
