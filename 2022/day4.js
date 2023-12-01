const {readFileSync, promises: fsPromises} = require('fs');
function syncReadFile(filename) {
  const contents = readFileSync(filename, 'utf-8');
  const arr = contents.split(/\r?\n/);
  return arr;
}

let start = Date.now();
solution = solve();
let end = Date.now();
console.log(`Execution time: ${end - start} ms`);

function solve() {
    var [overlaps, overlaps1] = [0, 0];
    syncReadFile('./day4input.txt').forEach((pair) => {
        var [elfOne, elfTwo] = pair.split(","); // i.e 2-5,15-90 --> ['2-5'] and ['15-90']
        var [oneZero, oneOne] = elfOne.split('-').map(Number); // 2 and 5
        var [twoZero, twoOne] = elfTwo.split('-').map(Number); // 15 and 90
        if (oneZero >= twoZero && oneOne <= twoOne || oneZero <= twoZero && oneOne >= twoOne) { //1st is if elfOne is fully in elfTwo and second vice versa
            overlaps++;
            overlaps1++;
        // the first bit checks if twoOne is between oneZero and oneOne while the second checks if oneOne is between twoZero and twoOne
        } else if (oneZero <= twoOne && twoOne <= oneOne || twoZero <= oneOne && oneOne <= twoOne) {
            overlaps1++;
        }
    });
    console.log("part 1", overlaps);
    console.log("part 2", overlaps1);
    
}
