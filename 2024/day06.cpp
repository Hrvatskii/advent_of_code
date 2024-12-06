#include <iostream>
#include <fstream>
#include <vector>
#include <complex>
#include <array>
#include <algorithm>
#include <chrono>

bool isOoB(std::complex<double> pos, int xmin, int xmax, int ymin, int ymax) {
    return !(pos.imag() >= ymin && pos.imag() < ymax && pos.real() >= xmin && pos.real() < xmax);
}

std::complex<double> findObstruction(std::vector<std::string> &map, std::complex<double> direction, std::complex<double> position) {
    std::complex<double> newPosition = position;

    while (!isOoB(newPosition, 0, map[0].size(), 0, map.size()) && map[map.size() - newPosition.imag() - 1][newPosition.real()] != '#') {
        newPosition += std::complex{direction.real(), direction.imag()};
    }

    return newPosition - std::complex{direction.real(), direction.imag()};
}

bool isInRange(std::complex<double> p1, std::complex<double> p2, std::complex<double> p3) {
    double minRe = fmin(p1.real(), p2.real());
    double minIm = fmin(p1.imag(), p2.imag());

    std::complex<double> farthest = abs(p1) < abs(p2) ? p2 : p1;

    std::complex<double> z = p3 - std::complex{minRe, minIm};
    std::complex<double> w = farthest - std::complex{minRe, minIm};
    
    return (z.real() == 0 && (z.imag() <= w.imag() && z.imag() >= 0)) || ((z.real() <= w.real() && z.real() >= 0) && z.imag() == 0);
}

int findNonOverlapping(std::vector<std::array<std::complex<double>, 2>> visited, std::complex<double> currentPos, std::complex<double> newPos) {
    int range = abs(currentPos - newPos);
    int ret = range;
    double diffIm = newPos.imag() - currentPos.imag();
    double diffRe = newPos.real() - currentPos.real();
    std::complex<double> check = currentPos;
    for (int i = 0; i < range; i++) {
        if (diffIm != 0)
            check += std::complex<double>{0, diffIm / fabs(diffIm)};
        else
            check += std::complex<double>{diffRe / fabs(diffRe), 0};

        for (int j = 0; j < visited.size(); j++) {
            if (isInRange(visited[j][0], visited[j][1], check)) {
                ret--;
                break;
            }
        }
    }

    return ret;
}

bool includes(std::vector<std::array<std::complex<double>, 2>> vec, std::array<std::complex<double>, 2> val) {
    return std::find(vec.begin(), vec.end(), val) != vec.end();
}

int solve(std::vector<std::string> &map, std::complex<double> startPos) {
    std::complex<double> direction{-1, 0};
    std::complex<double> position = startPos;
    std::vector<std::array<std::complex<double>, 2>> visited;
    std::complex<double> newPosition = startPos;

    while (!isOoB(position + direction, 0, map[0].size(), 0, map.size())) {
        direction *= std::complex<double>{0, -1};
        newPosition = findObstruction(map, direction, position);
        if (includes(visited, {position, newPosition}))
            return 1;
        visited.push_back({position, newPosition});
        position = newPosition;
    }

    return 0;
}

bool isCloseTo(std::vector<std::array<std::complex<double>, 2>> visited, std::complex<double> pos) {
    for (std::array<std::complex<double>, 2> pair : visited) 
        if (isInRange(pair[0], pair[1], pos))
            return true;

    return false;
}

int main() {
    // start clock
    auto start = std::chrono::high_resolution_clock::now();

    std::ifstream input("input/day06.txt");
    std::string line;
    std::vector<std::string> map;

    while (getline(input, line))
        map.push_back(line);

    input.close();
    
    // programmers hate this one simple trick!
    int part01 = 1;
    std::complex<double> direction{-1, 0};
    std::vector<std::array<std::complex<double>, 2>> visited;
    std::complex<double> startPos;

    for (int i = 0; i < map.size(); i++) {
        if (map[i].find('^') < map[i].size()) {
            startPos = {(double)map[i].find('^'), (double)map.size() - i - 1};
        }
    }
    std::complex<double> position = startPos;
    std::complex<double> newPosition = startPos;

    while (!isOoB(position + direction, 0, map[0].size(), 0, map.size())) {
        direction *= std::complex<double>{0, -1};
        newPosition = findObstruction(map, direction, position);
        part01 += findNonOverlapping(visited, position, newPosition);
        visited.push_back({position, newPosition});
        position = newPosition;
    }

    int part02 = 0;
    for (int i = 0; i < map.size(); i++) {
        for (int j = 0; j < map[0].size(); j++) {
            std::complex<double> pos{(double)j, (double)map.size() - i - 1};
            if (map[i][j] != '.' || !isCloseTo(visited, pos))
                continue;
            map[i][j] = '#';
            part02 += solve(map, startPos);
            map[i][j] = '.';
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
