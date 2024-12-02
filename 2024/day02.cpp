#include <iostream>
#include <fstream>
#include <sstream>
#include <vector>
#include <chrono>

// performs a check on a report. true if valid. false if invalid
bool check(std::vector<int> line) {
    bool increasing = (line[1]) > (line[0]);

    for (int i = 1; i < line.size(); i++) {
        int difference = (line[i]) - (line[i - 1]);

        if (difference == 0 || abs(difference) > 3) 
            return false;

        if ((increasing && difference < 0) || (!increasing && difference > 0))
            return false;
    }

    return true;
}

int main() {
    // start clock
    auto start = std::chrono::high_resolution_clock::now();

    int part01 = 0;
    int part02 = 0;

    // file handling
    std::string raw;
    std::ifstream input("input/day02.txt");

    while (getline(input, raw)) {
        // copy raw input line into a vector
        std::vector<int> line;
        std::stringstream ss(raw);
        std::string s;

        while(getline(ss, s, ' ')) 
            line.push_back(stoi(s));

        // assume the report is valid
        part01++;
        part02++;

        int result = check(line);
        if (!result) {
            part01--;
            part02--;

            // do a more thorough check by iteratively removing a level until the report is valid
            std::vector<int> copy = line;
            for (int i = 0; i < copy.size(); i++) {
                line.erase(line.begin() + i);
                if (check(line)) {
                    part02++;
                    break;
                }
                line = copy;
            }

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
