
const {readFileSync, promises: fsPromises} = require('fs');

function syncReadFile(filename) {
  const contents = readFileSync(filename, 'utf-8');
  const arr = contents.split(/\r?\n/);
  return arr;
}


function main() {
  var total = [];
  var totalCalories = 0;
  var maxTotal = 0;
  syncReadFile('./day1input.txt').forEach((item) => {
    if (item.length != 0) {
      totalCalories += parseInt(item);
    } else {
      total.push(totalCalories);
      totalCalories = 0;
    }
  });
  for (var i = 0; i < 3; i++) {
    var max = Math.max(...total);
    if (total.length == 251) {
      console.log("part 1 ", max);
    }
    maxTotal += max;
    total.splice(total.indexOf(max), 1);
  }
  console.log("part 2", maxTotal);
}

main();

