from time import time

def solve():
    with open("input/day12.txt", "r") as f:
        data = [line.strip() for line in f.readlines()]
    f.close()

    part01 = 0
    shapes = {}
    shape_index = 0
    parsing_shapes = True
    for row in data:
        if shape_index == 5 and row == "":
            parsing_shapes = False
            continue
        
        if parsing_shapes:
            if row == "":
                continue
            elif row[1] == ":":
                shape_index = int(row[0])
                continue
            else:
                if shape_index not in shapes.keys():
                    shapes[shape_index] = 0
                shapes[shape_index] += row.count("#")
        else:
            size, indexes = row.split(": ")
            width, height = map(int, size.split("x"))
            indexes = list(map(int, (indexes.split(" "))))
            area = width * height

            total_sum = 0

            for i in range(len(indexes)):
                total_sum += indexes[i] * shapes[i]

            if total_sum <= area:
                part01 += 1

    return part01

start_time = time()
part01 = solve()
stop_time = time()

print("part 1:\t\t", part01)
print("duration:\t", round(1000 * (stop_time - start_time)), "ms")