#include <iostream>
#include <fstream>
#include <string>
#include <vector>
#include <sstream>
#include <chrono>

bool isValid(long testValue, std::vector<int> numbers, int operation, bool p1) {
    // 0 = addition (subtraction)
    // 1 = multiplication (division)
    // 2 = concatenation (truncation)
    bool valid = false;

    if (numbers.size() == 0 && ((testValue == 0 && operation == 0) || (testValue == 1 && operation == 1)))
        return true;
    else if (numbers.size() != 0) {
        long n = numbers[0];
        numbers.erase(numbers.begin());

        if (testValue % n == 0)
            valid = isValid(testValue / n, numbers, 1, p1);
        if (!valid)
            valid = isValid(testValue - n, numbers, 0, p1);

        std::string tV_s = std::to_string(testValue);
        std::string n_s = std::to_string(n);
        if (!p1 && !valid && testValue > 9 && tV_s.size() > n_s.size() && tV_s.substr(tV_s.size() - n_s.size(), n_s.size()) == n_s)
            valid = isValid(stol(tV_s.substr(0, tV_s.size() - n_s.size())), numbers, 2, p1);
    }
    else
        return false;

    return valid;
}

int main() {
    // start clock
    auto start = std::chrono::high_resolution_clock::now();

    long part01 = 0;
    long part02 = 0;
    std::ifstream input("input/day07.txt");
    std::string line;

    while (getline(input, line)) {
        long testValue = stol(line.substr(0, line.find(':')));
        std::string temp = line.substr(line.find(':') + 2, line.size() - line.find(':'));
        std::stringstream ss(temp);
        std::string s;
        std::vector<int> numbers;
        while (getline(ss, s, ' '))
            numbers.insert(numbers.begin(), stoi(s));

        part01 += testValue * isValid(testValue, numbers, false, true);
        part02 += testValue * isValid(testValue, numbers, false, false);
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
