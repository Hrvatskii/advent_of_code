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
    createMap();
  }
};
xhr.send();

let [lowestX, highestX, lowestY, hightestY] = [498, 498, 6, 6]
const root = document.querySelector(":root");
let horizontalLines = [];
let filled = [];

function createMap() {
  input.forEach((path) => {
    let splitPath = path.split(" -> ");
    splitPath.forEach((square, index) => {
      let line = [];
      let differenceX, differenceY;
      let [currentX, currentY] = [Number(square.split(",")[0]), Number(square.split(",")[1])]
      if (splitPath[index + 1] != undefined) {
        differenceX = currentX - Number(splitPath[index + 1].split(",")[0]);
        differenceY = currentY - Number(splitPath[index + 1].split(",")[1]);
      } 
      let direction = differenceX == 0 ? differenceY : differenceX;
      for (let counter = 0; counter <= Math.abs(direction); counter++) {
        if (direction == differenceX) {
          if (direction < 0) {
            filled.push(`${currentX + counter},${currentY}`);
            line.push(`${currentX + counter},${currentY}`);
            if (currentX + counter > highestX) highestX = currentX + counter;
          } else {
            filled.push(`${currentX - counter},${currentY}`);
            line.push(`${currentX - counter},${currentY}`);
            if (currentX - counter < lowestX) lowestX = currentX - counter;
          } 
          if (currentX < lowestX) lowestX = currentX;
          if (currentX > highestX) highestX = currentX;
        } else {
          if (direction < 0) {
            filled.push(`${currentX},${currentY + counter}`);
            if (currentY + counter > hightestY) hightestY = currentY + counter;
          } else {
            filled.push(`${currentX},${currentY - counter}`);
            if (currentY - counter < lowestY) lowestY = currentY - counter;
          } 
          if (currentY < lowestY) lowestY = currentY;
          if (currentY > hightestY) lowestY = currentY;
        }
      }
      //console.log(differenceX, differenceY, direction, filled);
      //console.log(currentX, currentY)
      horizontalLines.push(line);
    })

  })

  root.style.setProperty("--lowestX", lowestX);
  root.style.setProperty("--highestX", highestX);
  root.style.setProperty("--lowestY", lowestY);
  root.style.setProperty("--highestY", hightestY);

  // draw
  for (let row = 0; row < hightestY + 10; row++) {
    let rowDiv = document.createElement("div");
    rowDiv.className = "row";
    for (let square = 500 - hightestY - 5; square < 500 + hightestY + 5; square++) {
      let squareDiv = document.createElement("div");
      squareDiv.className = "square";
      squareDiv.id = [`${square},${row}`];
      if (JSON.stringify(filled).includes(JSON.stringify(squareDiv.id))) {
        squareDiv.dataset.state = "filled";
        //squareDiv.style.backgroundColor = "gray";
        
      } else {
        squareDiv.dataset.state = "a";

      }
//      if (squareDiv.id == "500,0") squareDiv.style.backgroundColor = "green";
      rowDiv.appendChild(squareDiv);
    }
    document.getElementById("map").appendChild(rowDiv);
  }
  for (let i = 500 - hightestY - 5; i < 500 + hightestY + 5; i++) {
    document.getElementById(`${i},${hightestY + 2}`).dataset.state = "filled";
    filled.push(`${i},${hightestY + 2}`);
//    document.getElementById(`${i},${hightestY + 5}`).style.backgroundColor = "gray";
  }

}

let amountOfSand = 0;
let sand = [500,0];
let part1 = false;
function newSand() {

  sand = [500,0];
//  document.getElementById(sand).style.backgroundColor = "#e3d146";
  document.getElementById(sand).dataset.state = "filled";

  amountOfSand++;

}

function sandDrop() {
  document.getElementById("start").style.display = "none";
  setTimeout(() => {
    
    let startTime = Date.now();
    while (true) {
      let i = 0;
      if (document.getElementById(`${sand[0]},${sand[1] + 1}`).dataset.state != "filled") {
        document.getElementById(sand).dataset.state = "a";
        sand[1]++;
  //      document.getElementById(sand).style.backgroundColor = "#e3d146";
        document.getElementById(sand).dataset.state = "filled";
      } else if (document.getElementById(`${sand[0] - 1},${sand[1] + 1}`).dataset.state != "filled") {
        document.getElementById(sand).dataset.state = "a";
  
  //      document.getElementById(sand).style.backgroundColor = "black";
        sand[1]++;
        sand[0]--;
  //      document.getElementById(sand).style.backgroundColor = "#e3d146";
        document.getElementById(sand).dataset.state = "filled";
      } else if (document.getElementById(`${sand[0] + 1},${sand[1] + 1}`).dataset.state != "filled") {
        document.getElementById(sand).dataset.state = "a";
  
  //      document.getElementById(sand).style.backgroundColor = "black";
        sand[1]++;
        sand[0]++;
  //      document.getElementById(sand).style.backgroundColor = "#e3d146";
        document.getElementById(sand).dataset.state = "filled";
      } else {
        newSand();
      }
      if (!part1) {
        if (sand[1] >= hightestY) {
          let endTimeOne = Date.now();
          document.getElementById("p1a").innerText = `Part 1 answer: ${amountOfSand}`;
          document.getElementById("p1e").innerText = `Part 1 execution time: ${endTimeOne - startTime}ms`;
          part1 = true;
        }
      }
      if (document.getElementById("500,0").dataset.state == "filled" && document.getElementById("501,1").dataset.state == "filled") {
        let endTimeTwo = Date.now();
        document.getElementById("p2a").innerText = `Part 2 answer: ${amountOfSand + 1}`;
        document.getElementById("p2e").innerText = `Part 2 execution time: ${endTimeTwo - startTime}ms`;
        document.getElementById("visualize").style.display = "block";
        return true;
      }
    }
  }, 50);
  
}

function start() {
  sandDrop();
}

function visualize() {
  let filledElements = document.querySelectorAll('[data-state="filled"]');
  for (let i = 0; i < filledElements.length; i++) {
    if (JSON.stringify(filled).includes(`"${filledElements[i].id}"`)) filledElements[i].style.backgroundColor = "gray";
    else filledElements[i].style.backgroundColor = "#e3d146";
  }
}