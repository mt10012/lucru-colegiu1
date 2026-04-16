#include <iostream>
using namespace std;

int sumaIntr(int a, int b) {
    return a + b; 
}
int PrIntr(int c, int d) {
    return c * d;
}

int main() {
    int n1, n2;
    cout << "Introdu primul numar: " << endl;
    cin >> n1;
    cout << "Introdu al doilea numar: " << endl;
    cin >> n2;

    int suma = sumaIntr(n1, n2); 
    intprod = PrIntr(c, d);
    cout << "Suma numerelor este: " << suma << endl; 
    cout << "Produsul numerelor este: " << produs << endl;
    return 0;
}