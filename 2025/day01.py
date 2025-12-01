from time import time

def solve():
    with open("input/day01.txt", "r") as f:
        data = [line.strip() for line in f.readlines()]
    f.close()

    part01 = 0
    part02 = 0
    
    current_number = 50
    for instruction in data:
        direction = 1 if instruction[0] == "R" else -1
        length = int(instruction[1:len(instruction)])

        current_number += direction * length

        if current_number > 100:
            part02 += int(current_number / 100)
            if current_number - length == 100 or current_number % 100 == 0:
                part02 -= 1
        elif current_number < 0:
            part02 += int(abs(current_number - 100) / 100)
            if current_number + length == 0 or current_number % 100 == 0:
                part02 -= 1

        current_number %= 100

        if (current_number == 0):
            part01 += 1
            part02 += 1

    return part01, part02

start_time = time()
part01, part02 = solve()
stop_time = time()

print("part 1:\t\t", part01)
print("part 2:\t\t", part02)
print("duration:\t", round(1000000 * (stop_time - start_time)), "µs")
