from time import time
from math import log10, ceil, pow

def solve():
    with open("input/day04.txt", "r") as f:
        data = [line.strip() for line in f.readlines()]
    f.close()

    part01 = 0
    part02 = 0

    paper_map = {}
    map_width_multiplier = int(pow(10, ceil(log10(len(data[0]) + 1))))

    for y in range(len(data)):
        for x in range(len(data[y])):
            coordinate = y * map_width_multiplier + x
            if data[y][x] == '@':
                paper_map[coordinate] = '@'

    neighbors = [
        -map_width_multiplier - 1,
        -map_width_multiplier,
        -map_width_multiplier + 1,
        -1,
        1,
        map_width_multiplier - 1,
        map_width_multiplier,
        map_width_multiplier + 1,
    ]

    def count_valid(paper_map):
        paper_map_copy = paper_map.copy()
        valid = 0
        for coordinate in paper_map:
            paper_count = 0
            for neighbor in neighbors:
                if (coordinate + neighbor) in paper_map:
                    paper_count += 1

            if paper_count < 4:
                valid += 1
                del paper_map_copy[coordinate]

        return valid, paper_map_copy
    
    while True:
        valid, paper_map = count_valid(paper_map)
        if valid == 0:
            break

        if part01 == 0:
            part01 = valid

        part02 += valid

    return part01, part02

start_time = time()
part01, part02 = solve()
stop_time = time()

print("part 1:\t\t", part01)
print("part 2:\t\t", part02)
print("duration:\t", round(1000 * (stop_time - start_time)), "ms")