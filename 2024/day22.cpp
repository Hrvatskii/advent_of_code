#include <iostream>
#include <fstream>
#include <unordered_map>
#include <vector>
#include <chrono>

long mixprune(long number, long secret) {
    secret ^= number;
    secret %= 16777216;
    return secret;
}

int main() {
    // start clock
    auto start = std::chrono::high_resolution_clock::now();

    std::ifstream input("input/day22.txt");
    std::string line;
    long part01 = 0;

    std::unordered_map<std::string, int> combos;

    while (getline(input, line)) {
        std::unordered_map<std::string, int> combosCurrent;
        std::vector<int> currentCombo;
        long secret = stol(line);
        long a;
        int price, previousPrice;
        for (int i = 0; i < 2000; i++) {
            previousPrice = price;
            price = std::to_string(secret).back() - '0';

            if (i != 0)
                currentCombo.emplace_back(price - previousPrice);

            if (i > 3) {
                std::string c;
                for (int pd : currentCombo)
                    c += std::to_string(pd);

                if (combos.find(c) == combos.end())
                    combos[c] = 0;

                if (combosCurrent.find(c) == combosCurrent.end()) {
                    combos[c] += price;
                    combosCurrent[c] = 0;
                }

                currentCombo.erase(currentCombo.begin());
            }

            a = secret * 64;
            secret = mixprune(a, secret);
            a = secret / 32;
            secret = mixprune(a, secret);
            a = secret * 2048;
            secret = mixprune(a, secret);
        }

        part01 += secret;
    }

    input.close();

    int part02 = 0;
    for (auto [a, b] : combos)
        if (b > part02)
            part02 = b;

    // stop clock
    auto stop = std::chrono::high_resolution_clock::now();
    auto duration = std::chrono::duration_cast<std::chrono::milliseconds>(stop - start).count();

    std::cout << "part 1: " << part01 << "\n";
    std::cout << "part 2: " << part02 << "\n";
    std::cout << "time taken: " << duration << "ms\n";

    return 0;
}
