#include <iostream>
#include <fstream>
#include <vector>
#include <algorithm>
#include <chrono>

int main() {
    // start clock
    auto start = std::chrono::high_resolution_clock::now();

    int part01 = 0;
    int part02 = 0;

    std::vector<int> leftList;
    std::vector<int> rightList;

    // file handling
    std::string line;
    std::ifstream input;
    input.open("input/day01.txt");
    while (getline(input, line)) {
        // seperate left and right columns to different vectors
        leftList.push_back(std::stoi(line.substr(0, line.find("   "))));
        rightList.push_back(std::stoi(line.substr(line.find("   "))));
    }
    input.close();

    // sort vectors
    std::sort(leftList.begin(), leftList.end());
    std::sort(rightList.begin(), rightList.end());

    // solve for part 1 and 2
    for (int i = 0; i < leftList.size(); i++) {
        part01 += abs(leftList[i] - rightList[i]);
        // counts instances of leftList[i] in the rightList vector
        part02 += leftList[i] * std::count(rightList.begin(), rightList.end(), leftList[i]);
    }

    // stop clock
    auto stop = std::chrono::high_resolution_clock::now();
    auto duration = std::chrono::duration_cast<std::chrono::milliseconds>(stop - start).count();

    std::cout << "part 1: " << part01 << "\n";
    std::cout << "part 2: " << part02 << "\n";
    std::cout << "time taken: " << duration << "ms\n";
    return 0;
}
