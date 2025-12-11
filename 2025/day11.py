from time import time

def solve():
    with open("input/day11.txt", "r") as f:
        data = [line.strip() for line in f.readlines()]
    f.close()

    directions = {}
    for row in data:
        start, next_points = row.split(": ")
        directions[start] = next_points.split(" ")

    # without anything, with dac, with fft, with both
    found_exits = {}
    def DFS(start, path, part):
        count = 0
        if part == 2:
            if start in found_exits.keys():
                found_dac = False
                found_fft = False
                end_node_stats = found_exits[start]
                for i in range(len(path) - 2, -1, -1):
                    node = path[i]
                    if node == "dac":
                        found_dac = True
                    elif node == "fft":
                        found_fft = True
                    
                    if node not in found_exits.keys():
                        found_exits[node] = [0, 0, 0, 0]
                    
                    node_stats = found_exits[node]
                    if not found_dac and not found_fft:
                        node_stats[0] += end_node_stats[0]
                        node_stats[1] += end_node_stats[1]
                        node_stats[2] += end_node_stats[2]
                        node_stats[3] += end_node_stats[3]
                    elif found_dac and not found_fft:
                        node_stats[3] += end_node_stats[2] + end_node_stats[3]
                        node_stats[1] += end_node_stats[0]
                    elif not found_dac and found_fft:
                        node_stats[3] += end_node_stats[1] + end_node_stats[3]
                        node_stats[2] += end_node_stats[0]
                    else:
                        node_stats[3] += end_node_stats[0] + end_node_stats[3]
                        node_stats[1] += end_node_stats[1]
                        node_stats[2] += end_node_stats[2]
                return

            if start == "out":
                found_dac = False
                found_fft = False
                for i in range(len(path) - 1, -1, -1):
                    node = path[i]
                    if node == "dac":
                        found_dac = True
                    elif node == "fft":
                        found_fft = True
                    
                    if node not in found_exits.keys():
                        found_exits[node] = [0, 0, 0, 0]
                    
                    if not found_dac and not found_fft:
                        found_exits[node][0] += 1
                    elif found_dac and not found_fft:
                        found_exits[node][1] += 1
                    elif not found_dac and found_fft:
                        found_exits[node][2] += 1
                    else:
                        found_exits[node][3] += 1
                return
        else:
            if start == "out":
                return 1

        for next_node in directions[start]:
            if next_node in path:
                continue

            path.append(next_node)
            value = DFS(next_node, path, part)
            if part == 1:
                count += value
            path.pop(-1)

        return count

    DFS("svr", ["svr"], 2)
    part01 = DFS("you", ["you"], 1)
    part02 = found_exits["svr"][3]

    return part01, part02

start_time = time()
part01, part02 = solve()
stop_time = time()

print("part 1:\t\t", part01)
print("part 2:\t\t", part02)
print("duration:\t", round(1000 * (stop_time - start_time)), "ms")