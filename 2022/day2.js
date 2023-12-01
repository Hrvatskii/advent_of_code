
const {readFileSync, promises: fsPromises} = require('fs');

function syncReadFile(filename) {
  const contents = readFileSync(filename, 'utf-8');
  const arr = contents.split(/\r?\n/);
  return arr;
}
var score = 0;
var winCombs = ["A Y", "B Z", "C X", 2, 3, 1];
var tieCombs = ["A X", "B Y", "C Z", 1, 2, 3];
var loseCombs = ["A Z", "B X", "C Y", 3, 1, 2];
var winCombs2 = ["A Z", "B Z", "C Z", 2, 3, 1];
var tieCombs2 = ["A Y", "B Y", "C Y", 1, 2, 3];
var loseCombs2 = ["A X", "B X", "C X", 3, 1, 2];
syncReadFile('./day2input.txt').forEach((round) => {
    if (winCombs.includes(round)) {
        score += 6 + winCombs[winCombs.indexOf(round) + 3];
    } else if (tieCombs.includes(round)) {
        score += 3 + tieCombs[tieCombs.indexOf(round) + 3];
    } else {
        score += loseCombs[loseCombs.indexOf(round) + 3];
    }
});
console.log("part 1 ", score);
score = 0;
syncReadFile('./day2input.txt').forEach((round) => {
    if (winCombs2.includes(round)) {
        score += 6 + winCombs2[winCombs2.indexOf(round) + 3];
    } else if (tieCombs2.includes(round)) {
        score += 3 + tieCombs2[tieCombs2.indexOf(round) + 3];
    } else {
        score += loseCombs2[loseCombs2.indexOf(round) + 3];
    }
});
console.log("part 2", score);