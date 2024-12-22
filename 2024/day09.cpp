#include <iostream>
#include <fstream>
#include <vector>
#include <chrono>

int main() {
    // start clock
    auto start = std::chrono::high_resolution_clock::now();

    std::ifstream input("input/day09.txt");
    std::string line;
    std::vector<short> diskMap;

    while (getline(input, line)) {
        for (int i = 0; i < line.size(); i++) {
            for (int j = 0; j < line[i] - '0'; j++) {
                if (i % 2 == 0)
                    diskMap.emplace_back(i / 2 + 1);
                else
                    diskMap.emplace_back(0);
            }
        }
    }

    input.close();
    
    // part 1
    std::vector<short> dMCopy = diskMap;

    for (int i = 0; i < dMCopy.size(); i++) {
        if (dMCopy[i] == 0) {
            int finalIndex;
            for (int j = dMCopy.size() - 1; j > 0; j--) {
                if (dMCopy[j] != 0) {
                    finalIndex = j;
                    break;
                }
            }
            if (finalIndex < i)
                break;
            dMCopy[i] = dMCopy[finalIndex];
            dMCopy[finalIndex] = 0;
        }
    }

    // part 2
    for (int i = diskMap.size() - 1; i > 0; i--) {
        if (diskMap[i] != 0) {
            int size = 0;
            for (int j = i; diskMap[j] == diskMap[i]; j--)
                size++;

            int available = 0;
            int j = 0;
            for (; j < i; j++) {
                if (diskMap[j] == 0)
                    available++;
                else
                    available = 0;
                if (available == size)
                    break;
            }
            
            if (available != 0) {
                for (int k = j; k > j - size; k--) {
                    diskMap[k] = diskMap[i];
                }

                for (int k = i; k > i - size; k--) {
                    diskMap[k] = 0;
                }
            }

            i -= size - 1;
        }
    }

    long part01 = 0;
    long part02 = 0;

    for (int i = 0; dMCopy[i] != 0; i++)
        part01 += i * (dMCopy[i] - 1);

    for (int i = 0; i < diskMap.size(); i++)
        if (diskMap[i] != 0)
            part02 += i * (diskMap[i] - 1);

    // stop clock
    auto stop = std::chrono::high_resolution_clock::now();
    auto duration = std::chrono::duration_cast<std::chrono::milliseconds>(stop - start).count();

    std::cout << "part 1: " << part01 << "\n";
    std::cout << "part 2: " << part02 << "\n";
    std::cout << "time taken: " << duration << "ms\n";

    return 0;
}

