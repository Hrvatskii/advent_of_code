#include <iostream>
#include <fstream>
#include <vector>
#include <algorithm>
#include <unordered_map>
#include <complex>
#include <chrono>

struct ComplexHash {
    std::size_t operator()(const std::complex<int>& c) const {
        return std::hash<int>()(c.real()) ^ std::hash<int>()(c.imag());
    }
};

// 0: up, 1: right, 2: down: 3: left
struct coord {
    public:
        int x, y, dir, sum;
        coord () {};
        coord (int a, int b, int c, int d) : x(a), y(b), dir(c), sum(d) {};
        coord operator+ (const coord&);
        bool operator== (const coord&);
};

coord coord::operator+ (const coord& param) {
    coord temp;
    temp.x = x + param.x;
    temp.y = y + param.y;
    temp.dir = param.dir;
    if (dir == param.dir)
        temp.sum = sum + 1;
    else
        temp.sum = sum + 1001;
    return temp;
}

bool coord::operator== (const coord& param) {
    return x == param.x && y == param.y;
}

typedef std::vector<coord> v_coord;

bool includes(v_coord &vec, coord pos) {
    return std::find(vec.begin(), vec.end(), pos) != vec.end();
}

std::vector<v_coord> find(coord pos, coord end, v_coord &available) {
    std::vector<v_coord> paths = {{pos}};
    std::vector<v_coord> finished;
    std::unordered_map<std::complex<int>, int, ComplexHash> visited;

    const std::vector<coord> directions = {
        {1, 0, 1, 0},   // Right
        {-1, 0, 3, 0},  // Left
        {0, -1, 0, 0},  // Up
        {0, 1, 2, 0}    // Down
    };

    while (paths.size() != 0) {
        v_coord current = paths[0];
        paths.erase(paths.begin());
        coord currPos = current.back();

        if (currPos == end) {
            finished.emplace_back(current);
            continue;
        }

        for (const auto& dir : directions) {
            coord newPos = currPos + dir;
            if (!includes(current, newPos) && includes(available, newPos)) {
                std::complex<int> goaway = {newPos.x, newPos.y};
                if (visited[goaway] == 0 || (visited[goaway] >= newPos.sum - 1001 && newPos.dir == currPos.dir) || (visited[goaway] >= newPos.sum && newPos.dir != currPos.dir)) {
                    visited[goaway] = newPos.sum;
                    paths.emplace_back(current);
                    paths.back().emplace_back(newPos);
                }
            }
        }
    }
    
    return finished;
}

int findOptimal(std::vector<v_coord> &paths) {
    int best = 1000000000;

    for (v_coord path : paths) {
        if (path.back().sum < best)
            best = path.back().sum;
    }

    return best;
}

int findGoodPlaces(std::vector<v_coord> &paths, int goodLength) {
    v_coord goodPlaces;

    for (v_coord path : paths) {
        if (path.back().sum != goodLength)
            continue;
        for (coord coordinate : path) {
            if (!includes(goodPlaces, coordinate))
                goodPlaces.emplace_back(coordinate);
        }
    }

    return goodPlaces.size();
}

int main() {
    // terribly unoptimized but like who cares no one's gonna read this lol
    // start clock
    auto t0 = std::chrono::high_resolution_clock::now();

    std::ifstream input("input/day16.txt");
    std::string line;
    std::vector<std::string> temp;

    while (getline(input, line))
        temp.emplace_back(line);

    input.close();

    v_coord available;
    coord start, end;

    for (int y = 0; y < temp.size(); y++) {
        for (int x = 0; x < temp[0].size(); x++) {
            if (temp[y][x] != '#')
                available.push_back({x, y, 0, 0});
            if (temp[y][x] == 'S')
                start = {x, y, 1, 0};
            else if (temp[y][x] == 'E')
                end = {x, y, 0, 0};
        }
    }

    std::vector<v_coord> paths = find(start, end, available);
    int part01 = findOptimal(paths); 
    int part02 = findGoodPlaces(paths, part01);

    // stop clock
    auto stop = std::chrono::high_resolution_clock::now();
    auto duration = std::chrono::duration_cast<std::chrono::seconds>(stop - t0).count();

    std::cout << "part 1: " << part01 << "\n";
    std::cout << "part 2: " << part02 << "\n";
    std::cout << "time taken: " << duration << "s\n";


    return 0;
}
