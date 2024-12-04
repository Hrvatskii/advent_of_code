#include <iostream>
#include <fstream>
#include <vector>
#include <chrono>

bool check(std::vector<std::string> board, int x, int y, int xM, int yM) {
    // must not be close to an edge
    if (x + 3 * xM < 0 || x + 3 * xM >= board[0].size() || y + 3 * yM < 0 || y + 3 * yM >= board.size())
        return false;

    // wander along the line we're given and check if the character there is in "XMAS"
    for (int i = 1; i < 4; i++) 
        if(board[y + i * yM][x + i * xM] != "XMAS"[i])
            return false;

    return true;
}

bool check02(std::vector<std::string> board, int x, int y) {
    // must not be close to an edge
    if (x == 0 || x == board[0].size() - 1 || y == 0 || y == board.size() - 1)
        return false;

    // gather character for relevant coordinates
    char TL = board[y - 1][x - 1];
    char BR = board[y + 1][x + 1];
    char TR = board[y - 1][x + 1];
    char BL = board[y + 1][x - 1];

    // increase trueCount only if there is a valid X-MAS
    int trueCount = 0;

    if ((TL == 'M' && BR == 'S') || (TL == 'S' && BR == 'M'))
        trueCount++;

    if ((TR == 'M' && BL == 'S') || (TR == 'S' && BL == 'M'))
        trueCount++;

    return trueCount == 2;
}

int main() {
    // start clock
    auto start = std::chrono::high_resolution_clock::now();

    int part01 = 0;
    int part02 = 0;

    std::vector<std::string> wordSearch;

    // file handling and convert input to a vector of strings
    std::ifstream input("input/day04.txt");
    std::string line;

    while (getline(input, line)) 
        wordSearch.push_back(line);

    input.close();

    // iterate through each coordinate
    for (int i = 0; i < wordSearch.size(); i++) {
        for (int j = 0; j < wordSearch[i].size(); j++) {
            // if we find an x then we want to check for part01 solutions around that coordinate
            if (wordSearch[i][j] == 'X') {
                for (int y = -1; y < 2; y++) 
                    for (int x = -1; x < 2; x++) 
                        if (check(wordSearch, j, i, x, y))
                            part01++;
            } else if (wordSearch[i][j] == 'A') {
                if (check02(wordSearch, j, i))
                    part02++;
            }
        }
    }

    // stop clock
    auto stop = std::chrono::high_resolution_clock::now();
    auto duration = std::chrono::duration_cast<std::chrono::milliseconds>(stop - start).count();

    std::cout << "part 1: " << part01 << "\n";
    std::cout << "part 2: " << part02 << "\n";
    std::cout << "time taken: " << duration << "ms\n";

    return 0;
}
