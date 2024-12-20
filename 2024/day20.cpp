#include <iostream>
#include <fstream>
#include <complex>
#include <unordered_map>
#include <vector>
#include <algorithm>
#include <chrono>

typedef std::complex<int> coord;

struct ComplexHash {
    std::size_t operator()(const std::complex<int>& c) const {
        return std::hash<int>()(c.real()) ^ std::hash<int>()(c.imag());
    }
};

bool includes(const std::vector<coord> &vec, const coord pos) {
    return std::find(vec.begin(), vec.end(), pos) != vec.end();
}

int main() {
    // start clock
    auto start = std::chrono::high_resolution_clock::now();

    std::ifstream input("input/day20.txt");
    std::string line;
    std::vector<std::string> temp;

    while (getline(input, line))
        temp.emplace_back(line);

    input.close();

    std::unordered_map<coord, int, ComplexHash> free;
    coord endPos;
    
    // find start and end and mark valid places to go
    for (int y = 0; y < temp.size(); y++) {
        for (int x = 0; x < temp[0].size(); x++) {
            if (temp[y][x] != '#')
                free[{x, y}] = 0;
            if (temp[y][x] == 'E')
                endPos = {x, y};
        }
    }

    // flood fill to find distance from end to each point
    std::vector<coord> visited, queue;
    std::vector<int> distances;

    queue.emplace_back(endPos);
    distances.emplace_back(0);

    std::vector<coord> directions = {{1, 0}, {0, 1}, {-1, 0}, {0, -1}};

    while (queue.size() != 0) {
        coord current = queue[0];
        int distance = distances[0];
        queue.erase(queue.begin());
        distances.erase(distances.begin());
        visited.emplace_back(current);

        for (coord direction : directions) {
            if (free.find(current + direction) != free.end() && !includes(queue, current + direction) && !includes(visited, current + direction)) {
                queue.push_back(current + direction);
                distances.push_back(distance + 1);
                free[current + direction] = distance + 1;
            }
        }
    }

    // finds cheats
    int part01 = 0;
    int part02 = 0;
    for (auto [pos, distance] : free) {
        std::unordered_map<coord, int, ComplexHash> endPoses;
        for (int x = -20; x <= 20; x++) {
            if ((x < 0 && pos.real() - abs(x) < 0) || (x > 0 && pos.real() + abs(x) > temp[0].size()))
                continue;
            for (int y = -abs(abs(x) - 20); y <= abs(abs(x) - 20); y++) {
                if ((y < 0 && pos.imag() - abs(y) < 0) || (y > 0 && pos.imag() + abs(y) > temp.size()))
                    continue;
                const coord newPos = pos + coord{x, y};
                if (free.find(newPos) != free.end() && free[pos] - free[newPos] - abs(x) - abs(y) >= 100 && endPoses[newPos] != 1) {
                    part01 += ((abs(x) == 2 && y == 0) || (x == 0 && abs(y) == 2));
                    endPoses[newPos] = 1;
                    part02++;
                }
            }
        }
    }

    // stop clock
    auto stop = std::chrono::high_resolution_clock::now();
    auto duration = std::chrono::duration_cast<std::chrono::seconds>(stop - start).count();

    std::cout << "part 1: " << part01 << "\n";
    std::cout << "part 2: " << part02 << "\n";
    std::cout << "time taken: " << duration << "s\n";

    return 0;
}
