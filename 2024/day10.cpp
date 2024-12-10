#include <algorithm>
#include <iostream>
#include <fstream>
#include <vector>
#include <array>
#include <chrono>

typedef std::array<int, 3> coordinate;

bool isOoB(int x, int y, int xmax, int ymax) {
    return x < 0 || x > xmax || y < 0 || y > ymax;
}

bool includes(std::vector<coordinate> vec, coordinate pos) {
    return std::find(vec.begin(), vec.end(), pos) != vec.end();
}

int main() {
    // start clock
    auto start = std::chrono::high_resolution_clock::now();

    int part01 = 0;
    int part02 = 0;

    std::ifstream input("input/day10.txt");
    std::string line;
    std::vector<std::string> map;

    while (getline(input, line))
        map.push_back(line);

    input.close();

    for (int y = 0; y < map.size(); y++) {
        for (int x = 0; x < map[0].size(); x++) {
            if (map[y][x] != '0')
                continue;

            std::vector<coordinate> visited;
            std::vector<coordinate> queue;
            
            queue.push_back({x, y, map[y][x]});

            while (queue.size() != 0) {
                coordinate pos = queue[queue.size() - 1];
                queue.pop_back();

                int nx = pos[0];
                int ny = pos[1];
                int val = pos[2];

                if (val - '0' == 9) {
                    part02++;
                    if (!includes(visited, pos))
                        part01++;
                    visited.push_back(pos);
                    continue;
                }

                visited.push_back(pos);

                if (!isOoB(nx, ny - 1, map[0].size() - 1, map.size() - 1) && map[ny - 1][nx] == val + 1)
                    queue.push_back({nx, ny - 1, val + 1});
                if (!isOoB(nx, ny + 1, map[0].size() - 1, map.size() - 1) && map[ny + 1][nx] == val + 1)
                    queue.push_back({nx, ny + 1, val + 1});
                if (!isOoB(nx - 1, ny, map[0].size() - 1, map.size() - 1) && map[ny][nx - 1] == val + 1)
                    queue.push_back({nx - 1, ny, val + 1});
                if (!isOoB(nx + 1, ny, map[0].size() - 1, map.size() - 1) && map[ny][nx + 1] == val + 1)
                    queue.push_back({nx + 1, ny, val + 1});
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
