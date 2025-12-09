# good luck trying to understand this mess lmao.

# i define a shape as left oriented if there are more left turns
# than right turns, if you start walking from the first position
# in order. same goes for right oriented but the opposite.
# a corner is convex if it turns the same way as the shape of the system
# and concave if the turn direction is different from the shape.
# invalid places are the five places bordering a convex corner or the one
# place in the corner of a concave corner.

# this program's part 2 is optimized for the actual puzzle input (ie only
# considering the corners of the bit of the shape that cuts into the main shape)
# (this seems to be consistent across inputs [n=2 lol])
# so it probably won't work on the sample input.

from time import time

def solve():
    with open("input/day09.txt", "r") as f:
        data = [line.strip() for line in f.readlines()]
    f.close()

    for i in range(len(data)):
        data[i] = list(map(int, data[i].split(",")))

    left_right_combinations = {
        "ur": "r",
        "rd": "r",
        "dl": "r",
        "lu": "r",
        "ul": "l",
        "ld": "l",
        "dr": "l",
        "ru": "l",
    }

    def direction(v):
        if v[0] < 0:
            return "l"
        if v[0] > 0:
            return "r"
        if v[1] < 0:
            return "d"
        if v[1] > 0:
            return "u"

    def add(v, u):
        return [v[0] + u[0], v[1] + u[1]]
    
    def multiply(v, l):
        return [v[0] * l, v[1] * l]

    right_left_map = []
    right_turns = 0
    left_turns = 0
    circumfrance = 0
    for i in range(len(data)):
        corner = data[i]
        previous_i = (i - 1) % len(data)
        previous_corner = data[previous_i]
        next_i = (i + 1) % len(data)
        next_corner = data[next_i]

        direction_one = direction(add(previous_corner, multiply(corner, -1)))
        direciton_two = direction(add(next_corner, multiply(corner, -1)))
        corner_direction = left_right_combinations[direction_one + direciton_two]

        side = add(previous_corner, multiply(corner, -1))
        side_length = abs(side[0]) + abs(side[1])
        circumfrance += side_length

        right_left_map.append(corner_direction)

        if corner_direction == "r":
            right_turns += 1
        else:
            left_turns += 1

    average_side_length = circumfrance / len(data)

    starting_positions = []
    for i in range(len(data)):
        corner = data[i]
        previous_i = (i - 1) % len(data)
        previous_corner = data[previous_i]
        next_i = (i + 1) % len(data)
        next_corner = data[next_i]
        side1 = add(previous_corner, multiply(corner, -1))
        side_length1 = abs(side1[0]) + abs(side1[1])
        side2 = add(next_corner, multiply(corner, -1))
        side_length2 = abs(side2[0]) + abs(side2[1])
        if 10 * average_side_length < side_length1 or 10 * average_side_length < side_length2:
            starting_positions.append(corner)

    system_orientation = "r" if right_turns > left_turns else "l"

    convex_concave_map = []
    for direction in right_left_map:
        if system_orientation == direction:
            convex_concave_map.append("convex")
        else:
            convex_concave_map.append("concave")

    neighbors = [
        [-1, -1],
        [-1, 0],
        [-1, 1],
        [0, -1],
        [0, 1],
        [1, -1],
        [1, 0],
        [1, 1],
    ]

    outside_places = []
    for i in range(len(data)):
        shape = convex_concave_map[i]
        direction = right_left_map[i]

        corner = data[i]
        previous_i = (i - 1) % len(data)
        previous_corner = data[previous_i]
        next_i = (i + 1) % len(data)
        next_corner = data[next_i]

        x1, y1 = previous_corner
        x2, y2 = next_corner

        outside_places.append([])
        for neighbor in neighbors:
            new_corner = add(corner, neighbor)
            nx, ny = new_corner
            if shape == "convex":
                if not (nx >= min(x1, x2) and nx <= max(x1, x2) and ny >= min(y1, y2) and ny <= max(y1, y2)):
                    outside_places[-1].append(new_corner)
            else:
                if (nx > min(x1, x2) and nx < max(x1, x2) and ny > min(y1, y2) and ny < max(y1, y2)):
                    outside_places[-1].append(new_corner)
                    break

        if corner in starting_positions:
            outside_places[-1].append(multiply(corner, 1/2))
        
    def solve_part(part):
        part_solution = 0
        if part == 1:
            list_one = data
        else:
            list_one = starting_positions
        for i in range(len(list_one)):
            x1, y1 = list_one[i]
            for j in range(i + 1, len(data)):
                x2, y2 = data[j]
                dx = abs(x2 - x1)
                dy = abs(y2 - y1)
                area = (dx + 1) * (dy + 1)
                if part == 1 and area > part_solution:
                    part_solution = area
                if part == 2 and area > part_solution:
                    okay = True
                    for k in range(len(data)):
                        corner = data[k]
                        corner_x, corner_y = corner
                        if corner == list_one[i] or corner == data[j]:
                            continue

                        if not (corner_x >= min(x1, x2) and corner_x <= max(x1, x2) and corner_y >= min(y1, y2) and corner_y <= max(y1, y2)):
                            continue

                        for illegal_place in outside_places[k]:
                            nx, ny = illegal_place
                            if (nx >= min(x1, x2) and nx <= max(x1, x2) and ny >= min(y1, y2) and ny <= max(y1, y2)):
                                okay = False
                                break

                        if not okay:
                            break

                    if okay:
                        part_solution = area

        return part_solution

    part01 = solve_part(1)
    part02 = solve_part(2)

    return part01, part02

start_time = time()
part01, part02 = solve()
stop_time = time()

print("part 1:\t\t", part01)
print("part 2:\t\t", part02)
print("duration:\t", round(1000 * (stop_time - start_time)), "ms")