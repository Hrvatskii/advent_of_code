#include <algorithm>
#include <iostream>
#include <fstream>
#include <vector>
#include <complex>
#include <unordered_map>
#include <chrono>

typedef std::complex<int> coord; 

struct ComplexHash {
    std::size_t operator()(const std::complex<int>& c) const {
        return std::hash<int>()(c.real()) ^ std::hash<int>()(c.imag());
    }
};

void move(coord &pos, std::unordered_map<coord, int, ComplexHash> &map, coord direction) {
    int i = 1;
    for (; map[pos + direction * i] != 0; i++) {
        if (map[pos + direction * i] == 2)
            return;
    }

    for (; i > 0; i--) {
        map[pos + direction * i] = map[pos + direction * i - direction];
    }
    pos += direction;
}

bool includes(std::vector<coord> &vec, coord pos) {
    return std::find(vec.begin(), vec.end(), pos) != vec.end();
}

void move02(coord &pos, std::unordered_map<coord, int, ComplexHash> &map, coord direction) {
    if (map[pos + direction] == 0) {
        pos += direction;
        return;
    }

    std::vector<coord> toMove = {pos};

    for (int i = 0; ; i++) {
        int size1 = toMove.size();
        for (coord movable : toMove) {
            if (map[movable + direction] == 1) {
                if (!includes(toMove, movable + direction))
                    toMove.emplace_back(movable + direction);
                if (!includes(toMove, movable + direction + coord{1, 0}))
                    toMove.emplace_back(movable + direction + coord{1, 0});
            } else if (map[movable + direction] == 3) {
                if (!includes(toMove, movable + direction))
                    toMove.emplace_back(movable + direction);
                if (!includes(toMove, movable + direction - coord{1, 0}))
                    toMove.emplace_back(movable + direction - coord{1, 0});
            } else if (map[movable + direction] == 2) {
                return;
            }
        }
        if (size1 == toMove.size())
            break;
    }

    for (int i = toMove.size() - 1; i > 0; i--) {
        coord movable = toMove[i];
        map[movable + direction] = map[movable];
        map[movable] = 0;
    }

    pos += direction;
}

int main() {
    // start clock
    auto start = std::chrono::high_resolution_clock::now();

    std::fstream input("input/day15.txt");
    std::string line;
    std::vector<std::string> mapArr;
    std::string moves;

    bool parseMap = true;
    while (getline(input, line)) { 
        if (line == "") {
            parseMap = false;
            continue;
        }

        if (parseMap)
            mapArr.emplace_back(line);
        else
            moves += line;
    }

    input.close();

    coord pos;
    coord pos02;
    // 0: empty
    // 1 + 3: box
    // 2: wall
    std::unordered_map<coord, int, ComplexHash> map;
    std::unordered_map<coord, int, ComplexHash> map02;


    for (int y = 0; y < mapArr.size(); y++) {
        for (int x = 0; x < mapArr[0].size(); x++) {
            if (mapArr[y][x] == '@') {
                pos = {x, y};
                pos02 = {2*x, y};
                map[{x, y}] = 0;
                map02[{2*x, y}] = 0;
                map02[{2*x + 1, y}] = 0;
            } else if (mapArr[y][x] == 'O') {
                map[{x, y}] = 1;
                map02[{2*x, y}] = 1;
                map02[{2*x + 1, y}] = 3;
            } else if (mapArr[y][x] == '#') {
                map[{x, y}] = 2;
                map02[{2*x, y}] = 2;
                map02[{2*x + 1, y}] = 2;
            } else {
                map[{x, y}] = 0;
                map02[{2*x, y}] = 0;
                map02[{2*x + 1, y}] = 0;
            }
        }
    }

    coord direction;
    for (char m : moves) {
        if (m == '^')
            direction = {0, -1};
        else if (m == '<')
            direction = {-1, 0};
        else if (m == 'v')
            direction = {0, 1};
        else
            direction = {1, 0};

        move(pos, map, direction);

        if (direction.imag() == 0)
            move(pos02, map02, direction);
        else
            move02(pos02, map02, direction);
    }

    int part01 = 0;
    for (auto [a, b] : map) {
        if (b == 1)
            part01 += 100 * a.imag() + a.real();
    }

    int part02 = 0;
    for (auto [a, b] : map02) {
        if (b == 1)
            part02 += 100 * a.imag() + a.real();
    }

    // stop clock
    auto stop = std::chrono::high_resolution_clock::now();
    auto duration = std::chrono::duration_cast<std::chrono::milliseconds>(stop - start).count();

    std::cout << "part 1: " << part01 << "\n";
    std::cout << "part 2: " << part02 << "\n";
    std::cout << "time taken: " << duration << "ms\n";

    return 0;
}
