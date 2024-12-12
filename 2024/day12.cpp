#include <algorithm>
#include <fstream>
#include <iostream>
#include <vector>
#include <chrono>

typedef std::pair<int, int> coord;
typedef std::pair<char, coord> coordLabel;

std::vector<coordLabel> countPerimeters(std::vector<std::string> &map, int x, int y) {
    std::vector<coordLabel> count;

    if (x == 0 || map[y][x - 1] != map[y][x])
        count.push_back({'l', {x - 1, y}});
    if (x == map[0].size() - 1 || map[y][x + 1] != map[y][x])
        count.push_back({'r', {x + 1, y}});
    if (y == 0 || map[y - 1][x] != map[y][x])
        count.push_back({'u', {x, y - 1}});
    if (y == map.size() - 1 || map[y + 1][x] != map[y][x])
        count.push_back({'d', {x, y + 1}});

    return count;
}

inline bool includes(std::vector<coord> &vec, coord pos) {
    return std::find(vec.begin(), vec.end(), pos) != vec.end();
}

inline bool includes(std::vector<coordLabel> &vec, coordLabel pos) {
    return std::find(vec.begin(), vec.end(), pos) != vec.end();
}

int countSides(std::vector<coordLabel> &a) {
    int sides = 0;

    while (a.size() != 0) {
        coordLabel current = a[0];
        a.erase(a.begin());
        char type = current.first;
        int x = current.second.first;
        int y = current.second.second;

        for (int m = -1; m < 2; m += 2) {
            for (int i = 1; ; i++) {
                int nx = m * (type == 'u' || type == 'd') * i + x;
                int ny = m * (type == 'r' || type == 'l') * i + y;
                coordLabel pos = {type, {nx, ny}};
                if (!includes(a, pos)) {
                    break;
                }
                a.erase(std::remove(a.begin(), a.end(), pos), a.end());
            }
        }

        sides++;
    }

    return sides;
}

int main() {
    // it's okay to write weird code sometimes

    // start clock
    auto start = std::chrono::high_resolution_clock::now();

    std::ifstream input("input/day12.txt");
    std::string line;
    std::vector<std::string> map;

    while (getline(input, line)) 
        map.push_back(line);

    input.close();

    int part01 = 0;
    int part02 = 0;

    std::vector<coord> visited;

    for (int y = 0; y < map.size(); y ++) {
        for (int x = 0; x < map[0].size(); x++) {
            if (includes(visited, {x, y}))
                continue;

            char type = map[y][x];
            std::vector<coord> here;
            std::vector<coord> queue;
            queue.push_back({x, y});

            while (queue.size() != 0) {
                coord pos = queue[queue.size() - 1];
                int nx = pos.first;
                int ny = pos.second;
                queue.pop_back();
                visited.push_back(pos);
                here.push_back(pos);

                if (nx != 0 && map[ny][nx - 1] == type && !includes(visited, {nx - 1, ny}) && !includes(queue, {nx - 1, ny}))
                    queue.push_back({nx - 1, ny});
                if (nx != map[0].size() - 1 && map[ny][nx + 1] == type && !includes(visited, {nx + 1, ny}) && !includes(queue, {nx + 1, ny}))
                    queue.push_back({nx + 1, ny});
                if (ny != 0 && map[ny - 1][nx] == type && !includes(visited, {nx, ny - 1}) && !includes(queue, {nx, ny - 1}))
                    queue.push_back({nx, ny - 1});
                if (ny != map.size() - 1 && map[ny + 1][nx] == type && !includes(visited, {nx, ny + 1}) && !includes(queue, {nx, ny + 1}))
                    queue.push_back({nx, ny + 1});
            }
            

            std::vector<coordLabel> perimeterPoses;

            for (coord c : here) {
                std::vector<coordLabel> a = countPerimeters(map, c.first, c.second);
                for (coordLabel b : a)
                    perimeterPoses.push_back(b);
            }

            part01 += here.size() * perimeterPoses.size();
            part02 += here.size() * countSides(perimeterPoses);

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
