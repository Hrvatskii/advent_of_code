#include <iostream>
#include <fstream>
#include <vector>
#include <chrono>

int main() {
    // start clock
    auto start = std::chrono::high_resolution_clock::now();

    std::ifstream input("input/day25.txt");
    std::string line;

    int i = 0;
    bool isKey = true;
    std::vector<std::vector<int>> keys, locks;
    std::vector<int> current;
    while (getline(input, line)) {
        if (i % 8 == 0) {
            current = {0, 0, 0, 0, 0};
            if (line[0] == '#')
                isKey = false;
            else
                isKey = true;
            i++;
            continue;
        }

        if (!(isKey && (i - 6) % 8 == 0)) {
            for (int j = 0; j < line.size(); j++) {
                current[j] += line[j] == '#';
            }
        }

        i++;

        if ((i - 7) % 8 == 0) {
            if (isKey)
                keys.emplace_back(current);
            else
                locks.emplace_back(current);
        }
    }

    input.close();

    int part01 = 0;
    for (std::vector<int> lock : locks) {
        for (std::vector<int> key : keys) {
            part01++;
            for (int i = 0; i < 5; i++) {
                if (key[i] + lock[i] > 5) {
                    part01--;
                    break;
                }
            }
        }
    }

    // stop clock
    auto stop = std::chrono::high_resolution_clock::now();
    auto duration = std::chrono::duration_cast<std::chrono::milliseconds>(stop - start).count();

    std::cout << "part 1: " << part01 << "\n";
    std::cout << "time taken: " << duration << "ms\n";

    return 0;
}
