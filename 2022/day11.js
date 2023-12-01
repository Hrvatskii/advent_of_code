// had to cheat a tiiiiny bit with part 2. did part 1 legitly though

const {readFileSync, promises: fsPromises, cpSync} = require('fs');
const { start } = require('repl');
function syncReadFile(filename) {
  const contents = readFileSync(filename, 'utf-8');
  const arr = contents.split(/\r\n\s*\r\n/);
  return arr;
}
console.log("part 1:", solve(20, true));
console.log("part 2:", solve(10000, false));

function solve(rounds, part1) {
  let input = syncReadFile('./day11input.txt');
  let monkeys = [];
  let items = ['Starting_items', 'Operation', 'Test', 'True', 'False'];
  let monkeyTemp = {};
  let totalInspects = [];
  let part2 = 1;
  // parse input and make a monkeys object
  for (let monkey = 0; monkey < syncReadFile('./day11input.txt').length; monkey++) {
    let currentMonkey = input[monkey].split('\n');
    for (let item = 1; item < currentMonkey.length; item++) {
      if (item == 1) {
        monkeyTemp[items[item-1]] = currentMonkey[item].split(':')[1].split(',').map(Number);
      } else {
        monkeyTemp[items[item-1]] = currentMonkey[item].split(':')[1];
      }
    }
    part2 *= parseInt(monkeyTemp['Test'].split(' ')[3]);
    monkeys.push(monkeyTemp);
    monkeyTemp = {};
    totalInspects.push(0);
  }

  for (let round = 0; round < rounds; round++) {
    for (let monkey = 0; monkey < monkeys.length; monkey++) {
      while (monkeys[monkey]['Starting_items'].length != 0) {
        // initialize variables
        let startingItems = monkeys[monkey]['Starting_items'];
        let value = parseInt(monkeys[monkey]['Operation'].split(' ')[5]);
        if (isNaN(value)) {
          value = startingItems[0];
        }
        let operation = monkeys[monkey]['Operation'][11];
        let test = parseInt(monkeys[monkey]['Test'].split(' ')[3]);
        let testSuccess = parseInt(monkeys[monkey]['True'].split(' ')[4]);
        let testFail = parseInt(monkeys[monkey]['False'].split(' ')[4]);

        // inspects item
        if (operation == '+') {
          startingItems[0] += value;
        } else {
          startingItems[0] *= value;
        }
        totalInspects[monkey]++;
        if (part1) {
          startingItems[0] = Math.floor(startingItems[0] / 3);
        } else {
          startingItems[0] %= part2;
        }

        // test worry level
        if (startingItems[0] % test == 0) {
          monkeys[testSuccess]['Starting_items'].push(startingItems[0]); 
        } else {
          monkeys[testFail]['Starting_items'].push(startingItems[0]);
        }
        monkeys[monkey]['Starting_items'].shift();
      }
    }
  }

  let highestInspects = totalInspects.sort((a,b)=>a-b).reverse().slice(0, 2);
  let monkeyBusiness = highestInspects.reduce((a, b)=> a*b, 1);
  return monkeyBusiness;
}