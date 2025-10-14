# Robot Cleaner Simulation

A console-based vacuum robot simulator written in C#.
The program lets you choose from three cleaning strategies that define how the robot navigates and cleans the map.

---

## Features

* Dynamic map creation with adjustable size
* Obstacle and dirt placement
* Real-time grid visualization using ASCII characters
* Three cleaning strategies you can select at runtime

---

## Map Legends

```
# - Obstacle  
D - Dirt  
C - Cleaned  
R - Robot  
. - Empty
```

---

## Strategies

### 1. SomeStrategy (Zigzag)

* The robot moves left to right on one row
* Then right to left on the next row
* Continues until all rows are covered
* Covers the entire map efficiently

### 2. PerimeterHuggerStrategy

* The robot moves around the map border
* Cleans the outer edges only
* Useful for boundary cleaning or wall-following mode

### 3. SpiralStrategy

* The robot moves in a square spiral pattern starting from its initial position
* Expands outward with each loop
* Ensures full coverage with minimal overlap

---

## How to Run

1. Open the project in Visual Studio or any C# IDE
2. Build and run the program
3. Enter a number when prompted

   * `1` for Zigzag
   * `2` for Perimeter Hugger
   * `3` for Spiral
4. Watch the robot clean the map step by step in the console

---

## Example Output

```
Vacuum cleaner robot simulation  
--------------------------------  
Legends: #=Obstacles, D=Dirt, .=Empty, R=Robot, C=Cleaned  

. . . . . . . . . . . . . . . . . . .  
. . R . . D . . . . . # . . . . . . .  
. . . . . . . . . . . . . . . . . . .  
...
```

---

## Code Structure

* **Map.cs**
  Handles grid data, dirt, obstacles, and display logic

* **Robot.cs**
  Manages robot movement, cleaning, and strategy execution

* **IStrategy.cs**
  Interface for interchangeable cleaning algorithms

* **SomeStrategy.cs**
  Zigzag pattern implementation

* **PerimeterHuggerStrategy.cs**
  Perimeter cleaning algorithm

* **SpiralStrategy.cs**
  Expanding spiral cleaning algorithm

* **Program.cs**
  Entry point with menu for selecting a cleaning strategy

---

## Example Run

```
Initialize robot  
Choose cleaning strategy  
1 - SomeStrategy (Zigzag)  
2 - PerimeterHuggerStrategy  
3 - SpiralStrategy  
Enter choice: 3  

Robot starts cleaning using SpiralStrategy...  
Done.
```

---

## System Requirements

* .NET SDK 6.0 or later
* Windows, macOS, or Linux terminal

---

## Notes

* The robot ignores obstacles and boundaries
* Movement speed controlled by `Thread.Sleep(200)`
* You can adjust map size and dirt/obstacle positions directly in `Program.cs`
