const {readFileSync, promises: fsPromises, cpSync} = require('fs');
function syncReadFile(filename) {
  const contents = readFileSync(filename, 'utf-8');
  const arr = contents.split(/\r?\n/);
  return arr;
}

let cycle = 0;
let X = 1;
let cycleChecks = [20, 60, 100, 140, 180, 220];
let CRTCycle = 0;
let values = [];
let row = "";
console.log("part 2:")

function cycleCheck() {
	// part 1 (RGLRBZAU)
	cycle++;
	if (cycleChecks.includes(cycle)) {
		values.push(X*cycleChecks[0]);
		cycleChecks.shift();
	}

	// part 2 (14420)
	CRTCycle++;
	if (CRTCycle-1 >= X-1 && CRTCycle-1 <= X+1) {
		row += "█";
	} else {
		row += " ";
	}
	if (CRTCycle == 40) {
		console.log(row);
		CRTCycle = 0;
		row = "";
	}
}

syncReadFile('./day10input.txt').forEach((command) => {
	if (command == "noop") {
		cycleCheck();
	} else {
		let V = parseInt(command.split(" ")[1]);
		cycleCheck();
		cycleCheck();
		X += V;
	}
})

console.log("part 1:", values.reduce((partialSum, a) => partialSum + a, 0));