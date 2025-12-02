from time import time
from math import log10, ceil, pow

def solve():
    with open("input/day02.txt", "r") as f:
        data = f.readline().split(",")
    f.close()

    def solve_part(p1):
        part = 0

        for id_range in data:
            lower, upper = [int(a) for a in id_range.split("-")]
            lower_exponent = ceil(log10(lower))
            upper_exponent = ceil(log10(upper))

            found_invalids = []

            for depth in range(ceil(upper_exponent / 2)):
                for i in range(int(pow(10, depth)), ceil(pow(10, depth + 1))):
                    for attempt in range(2):
                        amount_of_times = 2 if p1 else int(upper_exponent / (depth + 1)) if attempt == 0 else int(lower_exponent / (depth + 1))

                        if amount_of_times < 2 and not p1: 
                            continue

                        test_number = 0
                        jump_size = ceil(log10(i + 1)) if i % 10 == 0 or i == 1 else ceil(log10(i))

                        for j in range(0, amount_of_times):
                            test_number += i * int(pow(10, j * jump_size))

                        if test_number >= lower and test_number <= upper and not test_number in found_invalids:
                            found_invalids.append(test_number)
                            part += test_number

        return part

    part01 = solve_part(True)
    part02 = solve_part(False)

    return part01, part02

start_time = time()
part01, part02 = solve()
stop_time = time()

print("part 1:\t\t", part01)
print("part 2:\t\t", part02)
print("duration:\t", round(1000 * (stop_time - start_time)), "ms")