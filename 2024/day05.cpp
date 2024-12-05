#include <iostream>
#include <fstream>
#include <vector>
#include <unordered_map>
#include <sstream>
#include <algorithm>
#include <chrono>

bool includes(std::vector<int> vec, int val) {
    return std::find(vec.begin(), vec.end(), val) != vec.end();
}

int main() {
    // start clock
    auto start = std::chrono::high_resolution_clock::now();

    int part01 = 0;
    int part02 = 0;
    
    std::unordered_map<int, std::vector<int>> rules;

    std::ifstream input("input/day05.txt");
    std::string line;

    bool parseRules = true;

    while(getline(input, line)) {
        // change from parsing the rules to solving the puzzle
        if (!line.size()) {
            parseRules = false;
            continue;
        }
        
        // parses the rules
        if (parseRules) {
            int n1 = stoi(line.substr(0, 2));
            int n2 = stoi(line.substr(3, 2));
            rules[n1].push_back(n2);
        } else {
            // splits the line to a vector
            std::vector<int> order;
            std::stringstream ss(line);
            std::string temp;
            while (getline(ss, temp, ','))
                order.push_back(stoi(temp));

            // solves part 1 and part 2
            std::vector<int> sorted;
            bool isValid = true;

            for (int i = 0; i < order.size(); i++) {
                int length = sorted.size();
                for (int j = 0; j < i; j++) {
                    if (!includes(rules[order[j]], order[i]))
                        isValid = false;

                    if (includes(rules[sorted[j]], order[i])) {
                        sorted.insert(sorted.begin() + j, order[i]);
                        break;
                    }
                }
                if (length == sorted.size())
                    sorted.push_back(order[i]);
            }

            if (isValid) 
                part01 += order[order.size() / 2];
            else 
                part02 += sorted[sorted.size() / 2];
        }
    }

    input.close();

    // stop clock
    auto stop = std::chrono::high_resolution_clock::now();
    auto duration = std::chrono::duration_cast<std::chrono::milliseconds>(stop - start).count();

    std::cout << "part 1: " << part01 << "\n";
    std::cout << "part 2: " << part02 << "\n";
    std::cout << "time taken: " << duration << "ms\n";

    return 0;
}
