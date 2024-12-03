#include <iostream>
#include <fstream>
#include <sstream>
#include <vector>
#include <chrono>

int multiply(std::string memory, int i) {
    if (memory[i + 1] != '(')
        return 0;

    char allowed[] = {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9', ','};

    int closingIndex;

    for (int j = i + 2; ; j++) {
        if (memory[j] == ')') {
            closingIndex = j;
            break;
        }

        // checks if any illegal characters are within the brackets
        bool isBad = true;

        for (char character : allowed)
            if (character == memory[j]) {
                isBad = false;
                break;
            }

        if (isBad)
            return 0;
    }

    // splits the string to a vector
    std::string pair = memory.substr(i + 2, closingIndex - i - 2);
    std::vector<int> vPair;
    std::stringstream ss(pair);
    std::string temp;

    while (getline(ss, temp, ','))
        vPair.push_back(stoi(temp));

    return vPair[0] * vPair[1];
}

std::string checkInstruction(std::string memory, int i) {
    if (memory.substr(i - 2, 3) == "mul")
        return "mul";

    if (i < 6)
        return "";

    if (memory.substr(i - 3, 4) == "do()")
        return "do()";

    if (memory.substr(i - 6, 7) == "don't()")
        return "don't()";

    return "";
}

int main() {
    // start clock
    auto start = std::chrono::high_resolution_clock::now();

    int part01 = 0;
    int part02 = 0;
    bool enabled = true;

    // file handling
    std::ifstream input("input/day03.txt");
    std::string memory;

    while (getline(input, memory)) {
        for (int i = 2; i < memory.size(); i++) {
            std::string instruction = checkInstruction(memory, i);

            if (instruction == "mul") {
                int value = multiply(memory, i);
                part01 += value;
                if (enabled)
                    part02 += value;
            } else if (instruction == "do()") {
                enabled = true;
            } else if (instruction == "don't()") {
                enabled = false;
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
