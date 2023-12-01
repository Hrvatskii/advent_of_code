let input, startPos;
let lengths = [0, 0, 0, 0]; //up, right, down, left
let directions = ["U", "R", "D", "L"];
let directionValues = [-1, 1, 1, -1];
let snakePositions = [];
const root = document.querySelector(":root");
let answer = 1;

// create an instance of XMLHttpRequest object
const xhr = new XMLHttpRequest();

// specify the path to the text file
const filePath = 'input.txt';

// send a GET request to fetch the text file
xhr.open('GET', filePath, true);
xhr.onload = function() {
  // check if the status code is OK (200)
  if (xhr.status === 200) {
    input = xhr.responseText.split("\n");
    createCSSGrid();
  }
};
xhr.send();

function createCSSGrid() {
  let currentX = 0;
  let currentY = 0;
  input.forEach((turn) => {
    let [direction, amount] = turn.split(" ");
    if (direction == "U" || direction == "D") {
      currentY += directionValues[directions.indexOf(direction)] * amount;
      if (direction == "D" && lengths[2] < currentY) lengths[2] = currentY;
      else if (direction == "U" && currentY < 0 && lengths[0] < Math.abs(currentY)) lengths[0] = Math.abs(currentY);
    } else {
      currentX += directionValues[directions.indexOf(direction)] * amount;
      if (direction == "R" && lengths[1] < currentX) lengths[1] = currentX;
      else if (direction == "L" && currentX < 0 && lengths[3] < Math.abs(currentX)) lengths[3] = Math.abs(currentX);
    }
  })
  let totalX = lengths[1] + lengths[3] + 1;
  let totalY = lengths[0] + lengths[2] + 1;

  root.style.setProperty("--height", totalY);
  root.style.setProperty("--length", totalX);

  for (let row = 0; row < totalY; row++) {
    let rowDiv = document.createElement("div");
    rowDiv.className = "row";
    for (let square = 0; square < totalX; square++) {
      let squareDiv = document.createElement("div");
      squareDiv.className = "square";
      squareDiv.id = [`${square}, ${row}`];
      if (totalY - lengths[2] - 1 == row && totalX - lengths[1] - 1 == square) {
        squareDiv.style.backgroundColor = "red";
        startPos = squareDiv.id.split(",").map(Number);
      }
      rowDiv.appendChild(squareDiv);
    }
    document.getElementById("playing-area").appendChild(rowDiv);
  }
  snakePositions = [[...startPos], [...startPos], [...startPos], [...startPos], [...startPos], [...startPos], [...startPos], [...startPos], [...startPos], [...startPos]];
}


function move() {
  document.getElementById("button").style.display = "none"
  let startTime = new Date();
  for (let i = 0; i < input.length; i++) { // input.length
    let currentMove = input[i];
    // console.log(currentMove);
    let [direction, amount] = currentMove.split(" ");
    for (let time = 0; time < amount; time++) {
      let previousPosition = [...snakePositions[0]];
      switch (direction) {
        case "U":
          snakePositions[0][1]--;
          break;
        case "R":
          snakePositions[0][0]++;
          break;
        case "D":
          snakePositions[0][1]++;
          break;
        default:
          snakePositions[0][0]--;
      }
      for (let segment = 1; segment < snakePositions.length; segment++) {
        let hypotenuse = Math.sqrt(Math.pow(snakePositions[segment][0] - snakePositions[segment - 1][0], 2) + Math.pow(snakePositions[segment][1] - snakePositions[segment - 1][1], 2));
        let tempPos = [...snakePositions[segment]];
      //  console.log(hypotenuse)
        if (hypotenuse > Math.sqrt(2)) {

          if (!findOptimalPosition(segment)) snakePositions[segment] = [...previousPosition];
          else snakePositions[segment] = findOptimalPosition(segment);
          
          if (segment == 9) {
            if (document.getElementById(snakePositions[9].join(", ")).style.backgroundColor != "green" && document.getElementById(snakePositions[9].join(", ")).style.backgroundColor != "red") answer++
            document.getElementById(snakePositions[9].join(", ")).style.backgroundColor = "green";
          }
        }
        previousPosition = tempPos;
      }
    }
  }
  let endTime = new Date();
  document.getElementById("time").innerText = `Execution time: ${endTime - startTime}ms`
  document.getElementById("answer").innerText = "Answer: "+answer
}

function findOptimalPosition(segment) {
  for (let i = -1; i < 2; i++) {
    for (let j = -1; j < 2; j++) {
      let hypotenuse = Math.sqrt(Math.pow(snakePositions[segment][0] + j - snakePositions[segment - 1][0], 2) + Math.pow(snakePositions[segment][1] + i - snakePositions[segment - 1][1], 2));
      if (hypotenuse == 1) return [snakePositions[segment][0] + j, snakePositions[segment][1] + i];
    }
  }
  return false;
}