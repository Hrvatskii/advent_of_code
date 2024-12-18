#include <iostream>
#include <fstream>
#include <unordered_map>
#include <complex>
#include <algorithm>
#include <vector>
#include <chrono>

#define SIZE 71
#define BYTECOUNT 1024

typedef std::complex<int> coord;

struct ComplexHash {
    std::size_t operator()(const std::complex<int>& c) const {
        return std::hash<int>()(c.real()) ^ std::hash<int>()(c.imag());
    }
};

int main() {
    // warning: this code is bad and slow and it took me an embarassing amount of time to figure out part 2 even though it's quite possibly one of the simplest part 2's in this aoc
    // start clock
    auto start = std::chrono::high_resolution_clock::now();

    // 1: free 0: corrupted
    std::unordered_map<coord, int, ComplexHash> map;

    for (int y = 0; y < SIZE; y++) {
        for (int x = 0; x < SIZE; x++) {
            map[{x, y}] = 1;
        }
    }

    std::ifstream input("input/day18.txt");
    std::string line;
    std::vector<coord> others;

    int current = 0;

    while (getline(input, line)) {
        int x = stoi(line.substr(0, std::find(line.begin(), line.end(), ',') - line.begin()));
        int y = stoi(line.substr(std::find(line.begin(), line.end(), ',') - line.begin() + 1));
        if (current < BYTECOUNT)
            map[{x, y}] = 0;
        else
            others.push_back({x, y});
        current++;
    }

    input.close();


    std::vector<coord> directions = {
        {1, 0},
        {0, 1},
        {-1, 0},
        {0, -1}
    };

    int part01 = 0;
    coord part02Coord = {0, 0};

    std::unordered_map<coord, int, ComplexHash> visited;

    for (int i = 0; i < others.size(); i++) {
        map[others[i]] = 0;

        if (i != 0 && visited[others[i]] != 1)
            continue;

        std::vector<std::vector<coord>> queue;
        queue.push_back({{0, 0}});

        std::unordered_map<coord, int, ComplexHash> mapCopy = map;
        bool found = false;

        while (queue.size() != 0) {
            std::vector<coord> path = queue[0];
            coord pos = path.back();

            if (pos == coord{SIZE - 1, SIZE - 1}) {
                if (part01 == 0)
                    part01 = path.size() - 1;

                for (coord a : path)
                    visited[a] = 1;

                found = true;
                break;
            }

            queue.erase(queue.begin());

            for (coord& dir : directions) {
                if (mapCopy[pos + dir] == 1) {
                    queue.push_back(path);
                    queue.back().push_back(pos + dir);
                    mapCopy[pos + dir] = 0;
                }
            }
        }

        if (!found) {
            part02Coord = others[i];
            break;
        }
    }

    std::string part02 = std::to_string(part02Coord.real()) + ',' + std::to_string(part02Coord.imag());

    // stop clock
    auto stop = std::chrono::high_resolution_clock::now();
    auto duration = std::chrono::duration_cast<std::chrono::milliseconds>(stop - start).count();

    std::cout << "part 1: " << part01 << "\n";
    std::cout << "part 2: " << part02 << "\n";
    std::cout << "time taken: " << duration << "ms\n";

    return 0;
}
