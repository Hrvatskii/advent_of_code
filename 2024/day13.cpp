#include <algorithm>
#include <iostream>
#include <fstream>
#include <vector>
#include <chrono>

// maths test
long long cost(double a, double b, double c, double d, long long e, long long f) {
    if (((long long)(e*d-c*f) % (long long)(d*a-b*c) == 0) && ((long long)(f*a-b*e) % (long long)(d*a-b*c) == 0))
        return 3 * (e*d-c*f)/(d*a-b*c) + (f*a-b*e)/(d*a-b*c);

    return 0;
}

int main() {
    // start clock
    auto start = std::chrono::high_resolution_clock::now();

    std::ifstream input("input/day13.txt");
    std::string line;
    std::vector<std::string> list;
    
    while(getline(input, line))
        list.emplace_back(line);

    input.close();
    list.emplace_back("");

    int part01 = 0;
    long long part02 = 0;

    int a, b, c, d, e, f;

    for (int i = 1; i <= list.size(); i++) {
        if ((i - 1) % 4 == 0) {
            a = stod(list[i - 1].substr(12, 2));
            b = stod(list[i - 1].substr(18, 2));
        } else if ((i - 2) % 4 == 0) {
            c = stod(list[i - 1].substr(12, 2));
            d = stod(list[i - 1].substr(18, 2));
        } else if ((i - 3) % 4 == 0) {
            e = stod(list[i - 1].substr(9, std::find(list[i - 1].begin(), list[i - 1].end(), ',') - list[i - 1].begin() - 9));
            f = stod(list[i - 1].substr(std::find(list[i - 1].begin(), list[i - 1].end(), 'Y') - list[i - 1].begin() + 2));
        } else {
            part01 += cost(a, b, c, d, e, f);
            part02 += cost(a, b, c, d, e + 10000000000000, f + 10000000000000);
        }
    }

    // stop clock
    auto stop = std::chrono::high_resolution_clock::now();
    auto duration = std::chrono::duration_cast<std::chrono::microseconds>(stop - start).count();

    std::cout << "part 1: " << part01 << "\n";
    std::cout << "part 2: " << part02 << "\n";
    std::cout << "time taken: " << duration << "µs\n";

    return 0;
}
