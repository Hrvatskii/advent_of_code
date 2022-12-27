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
let values = 0;
let screen = "";

function cycleCheck() {
	// part 1 (14420)
	cycle++;
	if (cycleChecks.includes(cycle)) {
		values += X*cycleChecks[0];
		cycleChecks.shift();
	}

	// part 2 (RGLRBZAU)
	CRTCycle++;
	if (CRTCycle-1 >= X-1 && CRTCycle-1 <= X+1) {
		screen += "█";
	} else {
		screen += " ";
	}
	if (CRTCycle == 40) {
		CRTCycle = 0;
		screen += "\n";
	}
}

syncReadFile('./day10input.txt').forEach((command) => {
		cycleCheck();
		if (command != "noop") {
			cycleCheck();
			X += parseInt(command.split(" ")[1]);
		}
})

console.log("part 1:", values, "\npart 2:");
console.log(screen);