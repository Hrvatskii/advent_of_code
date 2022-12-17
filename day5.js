/*
i am 99% certain there's some voodoo magic in this code
do NOT touch anything as it will probably break
i have no idea why it thought stacks == stacks1 before i added in currentStack1
i am not proud of this code whatsoever
*/

const {readFileSync, promises: fsPromises} = require('fs');
function syncReadFile(filename) {
  const contents = readFileSync(filename, 'utf-8');
  const arr = contents.split(/\r?\n/);
  return arr;
}

var board = syncReadFile('./day5input.txt').slice(0, 9);
var instructions = syncReadFile('./day5input.txt').slice(9);
var stacks = [];
var stacks1 = [];
var currentStack = [];
var currentStack1 = [];
var part2 = [];

// algorithm that creates the stack in a neat array
for (var column = 0; column < 9; column++) {
    for (var row = 0; row < 8; row++) {
        if (board[row][column*4+1] != " ") {
            currentStack.push(board[row][column*4+1]);
            currentStack1.push(board[row][column*4+1]);
        }
    }
    stacks.push(currentStack);
    stacks1.push(currentStack1);
    currentStack = [];
    currentStack1 = [];
}

// ??????
instructions.forEach((currentInstruction) => {
    var instruction = currentInstruction.split(" ");
    var [amount, start, end] = [instruction[1], instruction[3], instruction[5]];
    for (var i = 0; i < parseInt(amount); i++) {
        part2.push(stacks[start-1].shift()); 
        stacks1[end-1].unshift(stacks1[start-1].shift()); 
    }
    for (var j = 0; j < parseInt(amount); j++) {
        stacks[end-1].unshift(part2.pop());
    }
});

// cant be assed to make this prettier
var result1 = "";
stacks1.forEach((stack1) => {
    result1 += stack1[0];
});
console.log("part 1", result1);
var result = "";
stacks.forEach((stack) => {
    result += stack[0];
});
console.log("part 2", result);
