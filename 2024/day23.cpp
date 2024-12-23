#include <algorithm>
#include <iostream>
#include <fstream>
#include <unordered_map>
#include <vector>
#include <chrono>

template <typename T>
bool includes(std::vector<T> &vec, T val) {
    return std::find(vec.begin(), vec.end(), val) != vec.end();
}

int main() {
    // start clock
    auto start = std::chrono::high_resolution_clock::now();

    std::ifstream input("input/day23.txt");
    std::string line;
    std::unordered_map<std::string, std::vector<std::string>> map;
    std::vector<std::vector<std::string>> sets;

    while (getline(input, line)) {
        std::string first = line.substr(0, 2);
        std::string second = line.substr(3);

        map[first].emplace_back(second);
        map[second].emplace_back(first);
    }

    input.close();

    // ??? readability is for cowards anyway
    for (auto [a, b] : map) {
        for (std::string c : b) {
            for (std::string d : map[c]) {
                if (!(a[0] == 't' || c[0] == 't' || d[0] == 't'))
                    continue;
                if (includes(map[d], a)) {
                    std::vector<std::string> curSet = {a, c, d};
                    std::sort(curSet.begin(), curSet.end());
                    if (!includes(sets, curSet))
                        sets.emplace_back(curSet);
                }
            }
        }
    }

    int part01 = 0;
    for (std::vector<std::string> set : sets) {
        for (std::string computer : set) {
            if (computer[0] == 't') {
                part01++;
                break;
            }
        }
    }

    std::vector<std::string> longestSet = {};
    for (auto [a, b] : map) {
        std::vector<std::string> copy = b;
        copy.emplace_back(a);
        for (std::string c : copy) {
            for (std::string d : copy) {
                if (d == c)
                    continue;
                if (!includes(map[c], d) && includes(copy, c))
                    copy.erase(copy.begin() + (std::find(copy.begin(), copy.end(), c) - copy.begin()));
            }
        }
        if (copy.size() > longestSet.size())
            longestSet = copy;
    }
    std::sort(longestSet.begin(), longestSet.end());

    std::string part02 = "";
    for (std::string a : longestSet)
        part02 += a + ',';
    part02 = part02.substr(0, part02.size() - 1);

    // stop clock
    auto stop = std::chrono::high_resolution_clock::now();
    auto duration = std::chrono::duration_cast<std::chrono::milliseconds>(stop - start).count();

    std::cout << "part 1: " << part01 << "\n";
    std::cout << "part 2: " << part02 << "\n";
    std::cout << "time taken: " << duration << "ms\n";

    return 0;
}
