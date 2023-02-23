#include <bits/stdc++.h>

using namespace std;

int n, m, indx = 0;
int available[100], work[100], finish[100], seq[100];
int allocation[100][100], mx[100][100], need[100][100];

void input () {
	cout << "Processes and Resources\n";
	cin >> n >> m;
	
	cout << "Allocation\n";
	
	for (int i = 0; i < n; i++) {
		for (int j = 0; j < m; j++) {
			cin >> allocation[i][j];
		}
	}
	
	cout << "Max\n";
	
	for (int i = 0; i < n; i++) {
		for (int j = 0; j < m; j++) {
			cin >> mx[i][j];
		}
	}
	
	cout << "Available\n";
	
	for (int i = 0; i < m; i++) {
		cin >> available[i];
		work[i] = available[i];
	}
}

void calculateNeed () {
	for (int i = 0; i < n; i++) {
		for (int j = 0; j < m; j++) {
			need[i][j] = mx[i][j] - allocation[i][j];
		}
	}
}

void safetyCheck () {
	for (int k = 0; k < n; k++) {
		for (int i = 0; i < n; i++) {
			if (!finish[i]) {
				int f = 0;
				
				for (int j = 0; j < m; j++) {
					if (need[i][j] > work[j]) {
						f = 1;
						break;
					}
				}
				
				if (!f) {
					seq[indx] = i;
					indx++;
					
					for (int l = 0; l < m; l++) {
						work[l] += allocation[i][l];
					}
					
					finish[i] = 1;
				}
			}
		}
	}
}

void printResult () {
	for (int i = 0; i < n; i++) {
		if (!finish[i]) {
			cout << "Unsafe System\n";
			return;
		}
	}
	
	cout << "Safe System\nSequence: ";
	
	for (int i = 0; i < n; i++) {
		cout << seq[i] << ' ';
	}
	
	cout << endl;
}

int main () {
	input ();
	calculateNeed ();
	safetyCheck ();
	printResult ();
}

/*
5 3
0 1 0
2 0 0
3 0 2
2 1 1
0 0 2
7 5 3
3 2 2
9 0 2
2 2 2
4 3 3
3 3 2
*/