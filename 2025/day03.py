from time import time
from math import pow

def solve():
    with open("input/day03.txt", "r") as f:
        data = [line.strip() for line in f.readlines()]
    f.close()

    def find_best(start, stop, bank):
        best_joltage = 0
        best_index = 0
        for i in range(start + 1, stop):
            value = int(bank[i])
            if value > best_joltage:
                best_joltage = value
                best_index = i

        return best_joltage, best_index

    def solve_part(amount):
        part = 0
        for bank in data:
            previous_start = -1
            for i in range(amount - 1, -1, -1):
                joltage, previous_start = find_best(previous_start, len(bank) - i, bank)
                part += int(joltage * pow(10, i))

        return part

    part01 = solve_part(2)
    part02 = solve_part(12)

    return part01, part02

start_time = time()
part01, part02 = solve()
stop_time = time()

print("part 1:\t\t", part01)
print("part 2:\t\t", part02)
print("duration:\t", round(1000 * (stop_time - start_time)), "ms")