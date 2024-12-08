#include <algorithm>
#include <iostream>
#include <fstream>
#include <unordered_map>
#include <vector>
#include <chrono>

typedef std::pair<int, int> coordinate;

bool isOoB(int x, int y, int maxX, int maxY) {
    return x < 0 || x > maxX || y < 0 || y > maxY;
}

bool includes(std::vector<coordinate> &vec, coordinate coordinate) {
    return std::count(vec.begin(), vec.end(), coordinate) > 0;
}

int main() {
    // physics 2 chapter 2 Aware

    // start clock
    auto start = std::chrono::high_resolution_clock::now();

    std::ifstream input("input/day08.txt");

    std::unordered_map<char, std::vector<coordinate>> positions;
    std::string line;
    int i = 0;
    int maxX;
    int maxY;

    while (getline(input, line)) {
        for (int j = 0; j < line.size(); j++) {
            if (line[j] == '.')
                continue;
            positions[line[j]].push_back({j, i});
        }
        maxX = line.size() - 1;
        maxY = i;
        i++;
    }
    input.close();

    std::vector<coordinate> occupied01;
    std::vector<coordinate> occupied02;
    int part01 = 0;
    int part02 = 0;

    for (auto antenna : positions) {
        for (int i = 0; i < antenna.second.size(); i++) {
            for (int j = 0; j < antenna.second.size(); j++) {
                if (i == j)
                    continue;

                coordinate a1 = antenna.second[i];
                coordinate a2 = antenna.second[j];
                
                int x = a1.first;
                int y = a1.second;
                coordinate coordinate = {x, y};

                for (int n = 0; !isOoB(x, y, maxX, maxY); n++) {
                    if (n == 1 && !includes(occupied01, coordinate)) {
                        part01++;
                        occupied01.push_back(coordinate);
                    }

                    if (!includes(occupied02, coordinate)) {
                        part02++;
                        occupied02.push_back(coordinate);
                    }

                    x -= (a2.first - a1.first);
                    y -= (a2.second - a1.second);
                    coordinate = {x, y};
                }
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
