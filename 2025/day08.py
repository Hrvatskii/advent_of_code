
from time import time
from math import pow, log10, ceil

def solve():
    with open("input/day08.txt", "r") as f:
        data = [line.strip() for line in f.readlines()]
    f.close()

    string_map = {}
    for i in range(len(data)):
        data[i] = list(map(int, data[i].split(",")))
        string_map["".join(map(str, data[i]))] = data[i]

    distances = {}
    for i in range(len(data)):
        x1, y1, z1 = data[i]
        length_one = pow(x1, 2) + pow(y1, 2) + pow(z1, 2) 
        for j in range(i + 1, len(data)):
            x2, y2, z2 = data[j]
            length_two = pow(x2, 2) + pow(y2, 2) + pow(z2, 2) 
            distance = int(pow(x2 - x1, 2) + pow(y2 - y1, 2) + pow(z2 - z1, 2))
            distances[f"{x1}{y1}{z1}-{x2}{y2}{z2}"] = distance

    distances = dict(sorted(distances.items(), key=lambda a: a[1]))

    circuits = []

    connection_count = 0
    while True:
        pair = list(distances.keys())[0]
        del distances[pair]
        b1, b2 = pair.split("-")
        connection_count += 1

        is_in = False
        for i in range(len(circuits)):
            circuit = circuits[i]
            if b1 in circuit or b2 in circuit:
                is_in = True
                if b2 not in circuit:
                    circuits[i].append(b2)
                if b1 not in circuit:
                    circuits[i].append(b1)
                break
                    
        if not is_in:
            circuits.append([b1, b2])
            
        seen = []
        deletion_indexes = []

        for i in range(len(circuits)):
            for c1 in circuits[i]:
                for j in range(i + 1, len(circuits)):
                    if c1 in circuits[j]:
                        if c1 not in seen:
                            for c2 in circuits[i]:
                                if c2 not in circuits[j]:
                                    circuits[j].append(c2)
                                if c2 not in seen:
                                    seen.append(c2)
                            deletion_indexes.append(i)

        for i in range(len(deletion_indexes)):
            circuits.pop(deletion_indexes[i] - i)

        if connection_count == 1000:
            circuits.sort(key=len)
            part01 = len(circuits[-1]) * len(circuits[-2]) * len(circuits[-3])

        if (len(circuits[0]) == len(data)):
            part02 = string_map[b1][0] * string_map[b2][0]
            break

    return part01, part02

start_time = time()
part01, part02 = solve()
stop_time = time()

print("part 1:\t\t", part01)
print("part 2:\t\t", part02)
print("duration:\t", round((stop_time - start_time)), "s") # 224 seconds