#include <iostream>
#include <fstream>
#include <vector>
#include <sstream>
#include <unordered_map>
#include <chrono>

long isValid(std::string design, std::vector<std::string> &available, std::unordered_map<std::string, long> &solutions) {
    long valid = 0;

    if (solutions.find(design) != solutions.end())
        return solutions[design];

    for (std::string pattern : available) {
        if (design.size() < pattern.size())
            continue;
        if (pattern == design.substr(0, pattern.size())) {
            if (pattern.size() < design.size())
                valid += isValid(design.substr(pattern.size()), available, solutions);
            else 
                valid++;
        }
    }

    solutions[design] = valid;

    return valid;
}

int main() {
    // start clock
    auto start = std::chrono::high_resolution_clock::now();

    std::ifstream input("input/day19.txt");
    std::string line;
    std::vector<std::string> available;
    std::vector<std::string> list;

    bool parsingAvailable = true;
    while (getline(input, line)) {
        if (line == "") {
            parsingAvailable = false;
            continue;
        }

        if (parsingAvailable) {
            std::string temp;
            std::stringstream ss(line);

            while (getline(ss, temp, ',')) {
                if (temp[0] == ' ')
                    temp = temp.substr(1);
                available.emplace_back(temp);
            }
        } else {
            list.emplace_back(line);
        }
    }

    input.close();

    int part01 = 0;
    long part02 = 0;
    for (auto design : list) {
        std::unordered_map<std::string, long> empty;
        long valid = isValid(design, available, empty);
        part01 += valid != 0;
        part02 += valid;
    }

    // stop clock
    auto stop = std::chrono::high_resolution_clock::now();
    auto duration = std::chrono::duration_cast<std::chrono::milliseconds>(stop - start).count();

    std::cout << "part 1: " << part01 << "\n";
    std::cout << "part 2: " << part02 << "\n";
    std::cout << "time taken: " << duration << "ms\n";

    return 0;
}
