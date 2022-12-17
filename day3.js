const {readFileSync, promises: fsPromises} = require('fs');

function syncReadFile(filename) {
  const contents = readFileSync(filename, 'utf-8');
  const arr = contents.split(/\r?\n/);
  return arr;
}

var priorities = ["0","a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z",
"A","B","C","D","E","F","G","H"," I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z"];
var sum = 0;
var group = [];
var sum2 = 0;
syncReadFile('./day3input.txt').forEach((rucksack) => {
    // part 1
    var compartmentOne = rucksack.slice(0, rucksack.length/2);
    var compartmentTwo = rucksack.slice(rucksack.length/2);
    for (var item = 0; item < compartmentOne.length; item++) {
        if (compartmentTwo.includes(compartmentOne[item])) {
            sum += priorities.indexOf(compartmentOne[item]);
            break;
        }
    }
    // part 2
    group.push(rucksack);
    if (group.length == 3) {
        var sortedGroup = group.sort((a, b) => a.length - b.length);
        for (var item = 0; item < sortedGroup[2].length; item++) {
            if (sortedGroup[0].includes(sortedGroup[2][item]) && sortedGroup[1].includes(group[2][item])) {
                sum2 += priorities.indexOf(sortedGroup[2][item]);
                break;
            }
        }
        group = [];
    }
});
console.log("part 1", sum);
console.log("part 2", sum2);