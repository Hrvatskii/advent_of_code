from time import time

def solve():
    with open("input/day06.txt", "r") as f:
        data = f.readlines()
    f.close()

    lengths = []
    current_length = 0
    for i in range(1, len(data[-1])):
        if data[-1][i] != " ":
            lengths.append(current_length)
            current_length = 0
        else:
            current_length += 1

    lengths.append(current_length + 1)

    for row_index in range(len(data)):
        row = data[row_index]
        index = 0
        row_split = list(row)
        for length in lengths:
            for _ in range(length):
                if row[index] == " ":
                    row_split[index] = "0"

                index += 1
            index += 1
        data[row_index] = "".join(map(str, row_split)).split(" ")
        for i in range(len(data[row_index])):
            data[row_index][i] = data[row_index][i].replace("0", " ")

    calculations01 = []
    calculations02 = []
    operations = []
    for operation in data[-1]:
        calculations01.append(0 if operation[0] == "+" else 1)
        calculations02.append(0 if operation[0] == "+" else 1)
        operations.append(operation[0])

    for i in range(len(data) - 1):
        for j in range(len(data[i])):
            if operations[j] == "+":
                calculations01[j] += int(data[i][j])
            else:
                calculations01[j] *= int(data[i][j])

    for i in range(len(lengths)):
        for j in range(lengths[i]):
            number = ""
            for k in range(len(data) - 1):
                number += data[k][i][j]

            if operations[i] == "+":
                calculations02[i] += int(number)
            else:
                calculations02[i] *= int(number)

    part01 = sum(calculations01)
    part02 = sum(calculations02)

    return part01, part02

start_time = time()
part01, part02 = solve()
stop_time = time()

print("part 1:\t\t", part01)
print("part 2:\t\t", part02)
print("duration:\t", round(1000 * (stop_time - start_time)), "ms")