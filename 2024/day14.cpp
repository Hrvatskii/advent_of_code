#include <iostream>
#include <fstream>
#include <algorithm>
#include <vector>
#include <chrono>

#define WIDTH 101
#define HEIGHT 103

struct coord {
    public:
        int x, y;
        coord () {};
        coord (int a, int b) : x(a), y(b) {};
        coord operator+ (const coord&);
        bool operator== (const coord&);
};

coord coord::operator+ (const coord& param) {
    coord temp;
    temp.x = (WIDTH + x + param.x) % WIDTH;
    temp.y = (HEIGHT + y + param.y) % HEIGHT;
    return temp;
}

bool coord::operator== (const coord& other) {
    return x == other.x && y == other.y;
}

bool includes(std::vector<coord> &vec, coord entry) {
    return std::find(vec.begin(), vec.end(), entry) != vec.end();
}

bool check(std::vector<coord> &vec) {
    int alones = vec.size();
    for (int i = 0; i < vec.size(); i++) {
        coord &a = vec[i];
        if (includes(vec, a + coord{1, 0}) || includes(vec, a + coord{-1, 0}) || includes(vec, a + coord{0, 1}) || includes(vec, a + coord{0, -1}))
            alones--;
    }

    return alones <= vec.size() / 2;
}

int main() {
    // start clock
    auto start = std::chrono::high_resolution_clock::now();

    std::fstream input("input/day14.txt");
    std::string line;

    int first, second, third, fourth;
    first = second = third = fourth = 0;

    std::vector<coord> positionArr;
    std::vector<coord> velocityArr;

    while (getline(input, line)) {
        std::string positions = line.substr(0, std::find(line.begin(), line.end(), ' ') - line.begin());
        std::string velocities = line.substr(std::find(line.begin(), line.end(), ' ') - line.begin());
        int x = stoi(positions.substr(2, std::find(positions.begin(), positions.end(), ',') - positions.begin() - 2));
        int y = stoi(positions.substr(std::find(positions.begin(), positions.end(), ',') - positions.begin() + 1));
        int vx = stoi(velocities.substr(3, std::find(velocities.begin(), velocities.end(), ',') - velocities.begin() - 3));
        int vy = stoi(velocities.substr(std::find(velocities.begin(), velocities.end(), ',') - velocities.begin() + 1));

        positionArr.push_back(coord{x, y});
        velocityArr.push_back(coord{vx, vy});

        x = (1000 * WIDTH + x + vx * 100) % WIDTH;
        y = (1000 * HEIGHT + y + vy * 100) % HEIGHT;

        first += (x > (int)WIDTH / 2 && y < HEIGHT / 2);
        second += (x < (int)WIDTH / 2 && y < HEIGHT / 2);
        third += (x < (int)WIDTH / 2 && y > HEIGHT / 2);
        fourth += (x > (int)WIDTH / 2 && y > HEIGHT / 2);
    }

    input.close();

    int part01 = first * second * third * fourth;

    bool isTree = false;
    int part02 = 0;

    for (; !isTree; part02++) {
        for (int i = 0; i < velocityArr.size(); i++) {
            positionArr[i] = positionArr[i] + velocityArr[i];
        }
        isTree = check(positionArr);
    }

    // stop clock
    auto stop = std::chrono::high_resolution_clock::now();
    auto duration = std::chrono::duration_cast<std::chrono::seconds>(stop - start).count();

    std::cout << "part 1: " << part01 << "\n";
    std::cout << "part 2: " << part02 << "\n";
    std::cout << "duration: " << duration << "s\n";
    
    return 0;
}
