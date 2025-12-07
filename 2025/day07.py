from time import time
from math import log10, ceil, pow

def solve():
    with open("input/day07.txt", "r") as f:
        data = [line.strip() for line in f.readlines()]
    f.close()

    map_width_multiplier = int(pow(10, ceil(log10(len(data[0]) + 1))))

    splitter_positions = {}
    for y in range(len(data)):
        for x in range(len(data[y])):
            position = y * map_width_multiplier + x
            if data[y][x] == "^":
                splitter_positions[position] = 1
            else:
                splitter_positions[position] = 0

    queue = [data[0].index("S")]
    amounts = [1]

    part01 = 0
    part02 = 0

    def queue_logic(position, amount):
        if position not in queue:
            queue.append(position)
            amounts.append(amount)
        else:
            amounts[queue.index(position)] += amount

    while len(queue) != 0:
        position = queue.pop(0)
        amount = amounts.pop(0)
        if int(position / map_width_multiplier) == len(data) - 1:
            part02 += amount
            continue
        if splitter_positions[position + map_width_multiplier] == 0:
            queue_logic(position + map_width_multiplier, amount)
        else:
            queue_logic(position + map_width_multiplier + 1, amount)
            queue_logic(position + map_width_multiplier - 1, amount)
            part01 += 1

    return part01, part02

start_time = time()
part01, part02 = solve()
stop_time = time()

print("part 1:\t\t", part01)
print("part 2:\t\t", part02)
print("duration:\t", round(1000 * (stop_time - start_time)), "ms")