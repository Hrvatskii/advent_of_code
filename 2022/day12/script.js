const shadesOfGray = {
  "a": "#F9F9F9",
  "b": "#F2F2F2",
  "c": "#EBEBEB",
  "d": "#E5E5E5",
  "e": "#DEDEDE",
  "f": "#D7D7D7",
  "g": "#D0D0D0",
  "h": "#C9C9C9",
  "i": "#C2C2C2",
  "j": "#BBBBBB",
  "k": "#B4B4B4",
  "l": "#ADADAD",
  "m": "#A6A6A6",
  "n": "#9F9F9F",
  "o": "#999999",
  "p": "#929292",
  "q": "#8B8B8B",
  "r": "#848484",
  "s": "#7D7D7D",
  "t": "#767676",
  "u": "#6F6F6F",
  "v": "#686868",
  "w": "#616161",
  "x": "#5A5A5A",
  "y": "#535353",
  "z": "#4C4C4C",
  "S": "#00CC00",
  "E": "#CC0000"
};

const letterValues = {
  "a": 1,
  "b": 2,
  "c": 3,
  "d": 4,
  "e": 5,
  "f": 6,
  "g": 7,
  "h": 8,
  "i": 9,
  "j": 10,
  "k": 11,
  "l": 12,
  "m": 13,
  "n": 14,
  "o": 15,
  "p": 16,
  "q": 17,
  "r": 18,
  "s": 19,
  "t": 20,
  "u": 21,
  "v": 22,
  "w": 23,
  "x": 24,
  "y": 25,
  "z": 26,
  "S": 1,
  "E": 27
};


let gameBoard = [];
let tempPositions = [];
const root = document.querySelector(":root");
let squareId = 0; 
let endCoordinates, startCoordinates, playerPositions;
let isSolved = false;

// create an instance of XMLHttpRequest object
const xhr = new XMLHttpRequest();

// specify the path to the text file
const filePath = 'input.txt';

// send a GET request to fetch the text file
xhr.open('GET', filePath, true);
xhr.onload = function() {
  // check if the status code is OK (200)
  if (xhr.status === 200) {
    createArray();
    createCSSGrid();
  }
};
xhr.send();


function createArray() {
  const lines = xhr.responseText.split("\n");
  for (let i = 0; i < lines.length; i++) {
    gameBoard.push(lines[i]);
  }
}

function createCSSGrid() {
  root.style.setProperty("--height", gameBoard.length);
  root.style.setProperty("--length", gameBoard[0].length - 1);

  gameBoard.forEach((row, yVal) => {
    let squares = row.split("");
    if (squares[squares.length-1] == "\r") squares.pop();
    squares.forEach((square, xVal) => {
      let div = document.createElement("div");
      let position = {"x": xVal,"y": yVal};
      div.style.backgroundColor = shadesOfGray[square];
      div.id = `${position.x} ${position.y}`;
      div.innerText = square;
      //div.style.color = shadesOfGray[square];
      if (square == "S") {
        startCoordinates = [position.x, position.y];
        div.dataset.startPos = "true";
        //div.style.backgroundColor = "#35AED9";
      } 
      if (square == "E") endCoordinates = `${position.x} ${position.y}`;
      document.getElementById("map").appendChild(div);
    })
  })
  playerPositions = [
    {
      "position": {"x": startCoordinates[0], "y": startCoordinates[1]},
      "previousPosition": {},
      "path": [[startCoordinates[0], startCoordinates[1]]]
    }
  ]

}


let neighbors, goodNeighbors = [];
let currentSquare, previousSquare, currentSnake;

let counter = 0;
let badPositions = [];
let finalLengths = [];
let startTime, winSnakePath;

function updateSnake(snakeIndex) {
  let currentPositions = [];
  let pathLengths = [];
  goodNeighbors = [];
  currentSquare = document.getElementById(`${playerPositions[snakeIndex].position.x} ${playerPositions[snakeIndex].position.y}`) // gets html element for the current square
  //playerPositions[snakeIndex].path.push([playerPositions[snakeIndex].position.x, playerPositions[snakeIndex].position.y]); // updates path
  //playerPositions[snakeIndex].previousPosition = [previousSquare.id.split(" ")]; // updates the previous position to the playerpositions object

  if (playerPositions[snakeIndex].position.x == Number(endCoordinates.split(" ")[0]) && playerPositions[snakeIndex].position.y == Number(endCoordinates.split(" ")[1])) { // just checks if we're done
    let endTime = new Date();
    winSnakePath = playerPositions[snakeIndex].path;
    finalLengths.push(winSnakePath.length - 1);
    document.getElementById("execution-time-html").innerText = `Execution time: ${endTime - startTime}ms`
    document.getElementById("answer-html").innerText = `Answer: ${playerPositions[snakeIndex].path.length - 1}`
    document.getElementById("working-html").style.display = "none"; 
    isSolved = true;
  }

  neighbors = [
    [playerPositions[snakeIndex].position.x, playerPositions[snakeIndex].position.y - 1], // up
    [playerPositions[snakeIndex].position.x, playerPositions[snakeIndex].position.y + 1], // down
    [playerPositions[snakeIndex].position.x - 1, playerPositions[snakeIndex].position.y], // left
    [playerPositions[snakeIndex].position.x + 1, playerPositions[snakeIndex].position.y] // right
  ]
  
  neighbors.forEach((neighbor) => {
    if (!neighbor.includes(-1) && // checks so it isn't oob
        neighbor[1] != gameBoard.length && // checks so it isn't oob
        neighbor[0] != gameBoard[0].length - 1 && // checks so it isn't oob
        !JSON.stringify(playerPositions[snakeIndex].path).includes(JSON.stringify(neighbor)) && // checks so it doesn't go where it already has been
        letterValues[document.getElementById(`${neighbor[0]} ${neighbor[1]}`).innerText] - letterValues[currentSquare.innerText] <= 1 && // checks so its not climbing by more than 1
        !JSON.stringify(badPositions).includes(JSON.stringify(neighbor))) { 

      goodNeighbors.push(neighbor);
      badPositions.push(neighbor);
    }
  })
  //playerPositions[snakeIndex].neighbors = goodNeighbors;
  if (goodNeighbors.length >= 1) {
    goodNeighbors.forEach((newPosition, a) => {
      let newSnake = new createNewPosition([...playerPositions[snakeIndex].path], newPosition);
      tempPositions.push(newSnake);
      tempPositions[tempPositions.length-1].path.push(newPosition);

      if (currentPositions.includes(newPosition) && tempPositions[tempPositions.length-1].path.length <= pathLengths[tempPositions.length-1]) {
        currentPositions.push(newPosition);
        pathLengths.push(tempPositions[tempPositions.length-1].path.length);
      } else if (!currentPositions.includes(newPosition)) {
        currentPositions.push(newPosition);
        pathLengths.push(tempPositions[tempPositions.length-1].path.length);
      } else {
        tempPositions.pop();
      }
    })
  }
}

function loopThrough(part) {
  document.getElementById("working-html").style.display = "block";
  if (part == 2) document.getElementById("working-html").innerText = "this might take a while... do not refresh"
  startTime = new Date();
  setTimeout(() => {
    if (part == 1) {

      
        
      while (true) {
        if (isSolved) break;
          
        for (currentSnake = 0; currentSnake < playerPositions.length; currentSnake++) {
          if (isSolved) break;
          updateSnake(currentSnake);
        }
        playerPositions = [];
        tempPositions.forEach((array) => {
          playerPositions.push(array);
        })
        tempPositions = [];
      }
    } else {
      let firstPosition = "0 0"
      let secondPosition = `0 ${gameBoard.length - 1}`
      let startPositions = [firstPosition, secondPosition];
      let exitPoints = [];

      for (let i = 0; i < 10; i++) {
        badPositions = [];
        playerPositions[0].position.x = Number(startPositions[i].split(" ")[0]);
        playerPositions[0].position.y = Number(startPositions[i].split(" ")[1]);
        isSolved = false;
        while (!isSolved) {
          for (currentSnake = 0; currentSnake < playerPositions.length; currentSnake++) {
            if (isSolved) break;
            updateSnake(currentSnake);
          }
          playerPositions = [];
          tempPositions.forEach((array) => {
            playerPositions.push(array);
          })
          tempPositions = [];
        }
        while (winSnakePath[0][0] != 1) {
          winSnakePath.shift();
        }
        
        // winSnakePath[0][0] is "exit path" (first "b" y coordinate)
        playerPositions = [
          {
            "position": {"x": 0, "y": 40},
            "path": [[playerPositions[0].position.x, playerPositions[0].position.y]]
          }
        ]
        
        exitPoints.push(winSnakePath[0][1])
        
        if (exitPoints.length > 2) {
          let differenceOne = Math.abs(winSnakePath[0][1] - exitPoints[0]);
          let differenceTwo = Math.abs(winSnakePath[0][1] - exitPoints[1]);
          if (differenceOne > differenceTwo) {
            exitPoints[0] = winSnakePath[0][1];
          } else if (differenceOne < differenceTwo) {
            exitPoints[1] = winSnakePath[0][1];
          } else {
          }
          exitPoints.pop();
          startPositions.push(`0 ${Math.round((exitPoints[0] + exitPoints[1]) / 2)}`);
        }

        if (i == 1) {
          startPositions.push(`0 ${Math.round((exitPoints[0] + exitPoints[1]) / 2)}`);
        }



        if (exitPoints[0] == exitPoints[1]) {
          document.getElementById("answer-html").innerText = `Answer: ${winSnakePath.length}`
          break;
        }
      }
    }
    document.getElementById("visualize").style.display = "inline";
  }, 100)
  document.getElementById("solve-one").style.display = "none";
  document.getElementById("solve-two").style.display = "none";
}

function createNewPosition(path, newCurrentSquare) {
  this.position = {"x": newCurrentSquare[0], "y": newCurrentSquare[1]},
  this.path = path
}

let textColor = "normal";

function visualize() {
  document.getElementById("visualize").style.display = "none";
  winSnakePath.forEach((position, index) => {
    setTimeout(() => {
      document.getElementById(position.join(" ")).style.backgroundColor = "#35AED9";
      if (textColor == "normal") {
        document.getElementById(position.join(" ")).style.color = "black";
      } else {
        document.getElementById(position.join(" ")).style.color = "#35AED9";
      }
    }, index * 25);
  })
}


function solve() {
  createArray();
  createCSSGrid();
  loopThrough();
}

function toggleLetters() {
  let map = document.getElementById("map").children;

  for (let j = 0; j < map.length; j++) {
    if (map[j].style.color == map[j].style.backgroundColor) {
      textColor = "normal";
      map[j].style.color = "black";
    } else {
      textColor = "crazy";
      map[j].style.color = map[j].style.backgroundColor;
    }
  }
}
