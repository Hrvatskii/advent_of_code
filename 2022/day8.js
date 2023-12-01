const {readFileSync, promises: fsPromises} = require('fs');
function syncReadFile(filename) {
  const contents = readFileSync(filename, 'utf-8');
  const arr = contents.split(/\r?\n/);
  return arr;
}
let start = Date.now();
main();
let end = Date.now();
console.log(`Execution time: ${end - start} ms`);

function main() {
    var visibleTrees = 0;
    var scenicScores = [];
    syncReadFile('./day8input.txt').forEach((row) => {
        if (row == syncReadFile('./day8input.txt')[0] || row == syncReadFile('./day8input.txt')[syncReadFile('./day8input.txt').length-1]) {
            visibleTrees += syncReadFile('./day8input.txt').length;
        } else {
            for (var i = 0; i < row.length; i++) {  
                if (i == 0 || i == row.length-1) {
                    visibleTrees++;
                    continue;
                }
                var left = row.slice(0, i).split("").map(Number);
                var right = row.slice(i+1).split("").map(Number).reverse();
                var up = syncReadFile('./day8input.txt').map(d => d[i]).slice(0, syncReadFile('./day8input.txt').indexOf(row)).map(Number);
                var down = syncReadFile('./day8input.txt').map(d => d[i]).slice(syncReadFile('./day8input.txt').indexOf(row) + 1).map(Number).reverse();

                // part 1
                var maxLeft = Math.max.apply(Math, left);
                var maxRight = Math.max.apply(Math, right);
                var maxUp = Math.max.apply(Math, up);
                var maxDown = Math.max.apply(Math, down);
                if (maxLeft < row[i] || maxRight < row[i] || maxUp < row[i] || maxDown < row[i]) {
                    visibleTrees++;
                }

                //part 2
                var arr = [left, right, up, down];
                var total = 1;
                arr.forEach((direction) => {
                    if (JSON.stringify(direction.filter(a => a >= row[i]).sort())[1] == "]") { //idk what this does really
                        total *= direction.length;
                    } else {
                        total *= [...direction].slice(direction.lastIndexOf(parseInt(JSON.stringify([...direction].reverse().filter(a => a >= row[i]))[1]))).length; //idk this either
                    }
                });
                scenicScores.push(total);

            }
        }
    });
console.log("part 1", visibleTrees);
console.log("part 2", Math.max.apply(Math, scenicScores));
}