const {readFileSync, promises: fsPromises} = require('fs');
function syncReadFile(filename) {
  const contents = readFileSync(filename, 'utf-8'); //return if you want string
  const arr = contents.split(/\r?\n/); // return if you want array
  return contents;
}

function main(length) {
    var arr = [];
    for (var char = 0; char < syncReadFile('./day6input.txt').length; char++) {
        arr.push(syncReadFile('./day6input.txt')[char]);
        let findDuplicates = arr => arr.filter((item, index) => arr.indexOf(item) != index)
        if (arr.length > length) {
            arr.shift();
            if (!findDuplicates(arr).length) {
                return char + 1;
            } 
        }
    } 
}

console.log("part 1", main(4));
console.log("part 2", main(14));