#include <iostream>
#include <fstream>
#include <sstream>
#include <unordered_map>
#include <chrono>

long solve(std::unordered_map<long, long> arrangement, int blinks) {
    for (int i = 0; i < blinks; i++) {
        std::unordered_map<long, long> newArrangement;

        for (auto [num, count] : arrangement) {
            if (num == 0) {
                newArrangement[1] += count;
            } else if (std::to_string(num).size() % 2 == 0) {
                std::string snum = std::to_string(num);
                int hsnumsize = snum.size() / 2;
                newArrangement[stol(snum.substr(0, hsnumsize))] += count;
                newArrangement[stol(snum.substr(hsnumsize, hsnumsize))] += count;
            } else {
                newArrangement[num * 2024] += count;
            }
        }

        arrangement = newArrangement;
    }

    long amount = 0;
    for (auto [_, count] : arrangement)
        amount += count;

    return amount;
}

int main() {
    // start clock
    auto start = std::chrono::high_resolution_clock::now();

    std::ifstream input("input/day11.txt");
    std::string line;
    getline(input, line);
    input.close();
    std::stringstream ss(line);
    std::string temp;
    std::unordered_map<long, long> arrangement;
    while(getline(ss, temp, ' '))
        arrangement[stol(temp)] = 1;

    long part01 = solve(arrangement, 25);
    long part02 = solve(arrangement, 75);

    // stop clock
    auto stop = std::chrono::high_resolution_clock::now();
    auto duration = std::chrono::duration_cast<std::chrono::milliseconds>(stop - start).count();

    std::cout << "part 1: " << part01 << "\n";
    std::cout << "part 2: " << part02 << "\n";
    std::cout << "time taken: " << duration << "ms\n";

    return 0;
}
