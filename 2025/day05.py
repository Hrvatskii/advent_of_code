from time import time

def solve():
    with open("input/day05.txt", "r") as f:
        data = [line.strip() for line in f.readlines()]
    f.close()

    part01 = 0
    part02 = 0

    def check_if_in_range(lower, upper, i):
        is_in_range = False

        if lower <= ranges[i][0] and upper >= ranges[i][0]:
            ranges[i][0] = lower
            is_in_range = True
        
        if upper >= ranges[i][1] and lower <= ranges[i][1]:
            ranges[i][1] = upper
            is_in_range = True

        return is_in_range

    def compress_ranges():
        deletion_indexes = []

        for i in range(len(ranges)):
            lower, upper = ranges[i]
            for j in range(i + 1, len(ranges)):
                if check_if_in_range(lower, upper, j):
                    deletion_indexes.append(i)
                    break

        for i in range(len(deletion_indexes)):
            del ranges[deletion_indexes[i] - i]

    ranges = []
    parsing_ranges = True
    for row in data:
        if row == "":
            parsing_ranges = False
            continue

        if parsing_ranges:
            lower, upper = [int(a) for a in row.split("-")]
            if len(ranges) == 0:
                ranges.append([lower, upper])
                continue

            for i in range(len(ranges)):
                is_in_range = check_if_in_range(lower, upper, i)

            if not is_in_range:
                ranges.append([lower, upper])
        else:
            current_number = int(row)
            for checking_range in ranges:
                lower, upper = checking_range
                if current_number >= lower and current_number <= upper:
                    part01 += 1
                    break

    compress_ranges()
    for current_range in ranges:
        part02 += current_range[1] - current_range[0] + 1

    return part01, part02

start_time = time()
part01, part02 = solve()
stop_time = time()

print("part 1:\t\t", part01)
print("part 2:\t\t", part02)
print("duration:\t", round(1000 * (stop_time - start_time)), "ms")