using System;
using System.Threading;

namespace RobotCleaner
{
    public class Map
    {
        private enum CellType { Empty, Dirt, Obstacle, Cleaned };
        private CellType[,] _grid;
        public int Width { get; private set; }
        public int Height { get; private set; }

        public Map(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            _grid = new CellType[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    _grid[x, y] = CellType.Empty;
                }
            }
        }

        public bool IsInBounds(int x, int y)
        {
            return x >= 0 && x < this.Width && y >= 0 && y < this.Height;
        }

        public bool IsDirt(int x, int y)
        {
            return IsInBounds(x, y) && _grid[x, y] == CellType.Dirt;
        }

        public bool IsObstacle(int x, int y)
        {
            return IsInBounds(x, y) && _grid[x, y] == CellType.Obstacle;
        }

        public void AddObstacle(int x, int y)
        {
            _grid[x, y] = CellType.Obstacle;
        }

        public void AddDirt(int x, int y)
        {
            _grid[x, y] = CellType.Dirt;
        }

        public void Clean(int x, int y)
        {
            if (IsInBounds(x, y))
            {
                _grid[x, y] = CellType.Cleaned;
            }
        }

        public void Display(int robotX, int robotY)
        {
            Console.Clear();
            Console.WriteLine("Vacuum cleaner robot simulation");
            Console.WriteLine("--------------------------------");
            Console.WriteLine("Legends: #=Obstacles, D=Dirt, .=Empty, R=Robot, C=Cleaned");

            for (int y = 0; y < this.Height; y++)
            {
                for (int x = 0; x < this.Width; x++)
                {
                    if (x == robotX && y == robotY)
                    {
                        Console.Write("R ");
                    }
                    else
                    {
                        switch (_grid[x, y])
                        {
                            case CellType.Empty: Console.Write(". "); break;
                            case CellType.Dirt: Console.Write("D "); break;
                            case CellType.Obstacle: Console.Write("# "); break;
                            case CellType.Cleaned: Console.Write("C "); break;
                        }
                    }
                }
                Console.WriteLine();
            }
            Thread.Sleep(200);
        }
    }

    public interface IStrategy
    {
        void Clean(Robot robot);
    }

    public class Robot
    {
        private readonly Map _map;
        private readonly IStrategy _strategy;

        public int X { get; set; }
        public int Y { get; set; }

        public Map Map { get { return _map; } }

        public Robot(Map map, IStrategy strategy)
        {
            _map = map;
            _strategy = strategy;
            X = 0;
            Y = 0;
        }

        public bool Move(int newX, int newY)
        {
            if (_map.IsInBounds(newX, newY) && !_map.IsObstacle(newX, newY))
            {
                X = newX;
                Y = newY;
                _map.Display(X, Y);
                return true;
            }
            return false;
        }

        public void CleanCurrentSpot()
        {
            if (_map.IsDirt(X, Y))
            {
                _map.Clean(X, Y);
                _map.Display(X, Y);
            }
        }

        public void StartCleaning()
        {
            _strategy.Clean(this);
        }
    }

    public class SomeStrategy : IStrategy
    {
        public void Clean(Robot robot)
        {
            int direction = 1;
            for (int y = 0; y < robot.Map.Height; y++)
            {
                int startX = (direction == 1) ? 0 : robot.Map.Width - 1;
                int endX = (direction == 1) ? robot.Map.Width : -1;

                for (int x = startX; x != endX; x += direction)
                {
                    robot.Move(x, y);
                    robot.CleanCurrentSpot();
                }
                direction *= -1;
            }
        }
    }

    public class PerimeterHuggerStrategy : IStrategy
    {
        public void Clean(Robot robot)
        {
            while (robot.Move(robot.X + 1, robot.Y))
            {
                robot.CleanCurrentSpot();
            }

            while (robot.Move(robot.X, robot.Y + 1))
            {
                robot.CleanCurrentSpot();
            }

            while (robot.Move(robot.X - 1, robot.Y))
            {
                robot.CleanCurrentSpot();
            }

            while (robot.Move(robot.X, robot.Y - 1))
            {
                robot.CleanCurrentSpot();
            }
        }
    }

    public class SpiralStrategy : IStrategy
    {
        public void Clean(Robot robot)
        {
            int[,] directions = new int[4, 2]
            {
                { 1, 0 }, 
                { 0, 1 }, 
                { -1, 0 }, 
                { 0, -1 } 
            };

            int dirIndex = 0;
            int segmentLength = 1;
            int stepsTaken = 0;
            int turns = 0; 

            robot.CleanCurrentSpot();

            while (true)
            {
                int nextX = robot.X + directions[dirIndex, 0];
                int nextY = robot.Y + directions[dirIndex, 1];
                int tryCount = 0;
                while ((!robot.Map.IsInBounds(nextX, nextY) || robot.Map.IsObstacle(nextX, nextY)) && tryCount < 4)
                {
                    dirIndex = (dirIndex + 1) % 4;
                    turns++;
                    if (turns % 2 == 0)
                        segmentLength++;
                    stepsTaken = 0;
                    nextX = robot.X + directions[dirIndex, 0];
                    nextY = robot.Y + directions[dirIndex, 1];
                    tryCount++;
                }
                if (tryCount == 4 && (!robot.Map.IsInBounds(nextX, nextY) || robot.Map.IsObstacle(nextX, nextY)))
                    break;

                
                if (robot.Move(nextX, nextY))
                {
                    robot.CleanCurrentSpot();
                    stepsTaken++;
                }
                else
                {
                    break;
                }
                
                if (stepsTaken == segmentLength)
                {
                    dirIndex = (dirIndex + 1) % 4;
                    turns++;
                    stepsTaken = 0;
                    if (turns % 2 == 0)
                        segmentLength++;
                }
            }
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Initialize robot");

            Map map = new Map(20, 10);
            map.AddDirt(5, 3);
            map.AddDirt(10, 8);
            map.AddDirt(1, 1);
            map.AddObstacle(2, 5);
            map.AddObstacle(12, 1);

            Console.WriteLine("Choose cleaning strategy");
            Console.WriteLine("1 - SomeStrategy (Zigzag)");
            Console.WriteLine("2 - PerimeterHuggerStrategy");
            Console.WriteLine("3 - SpiralStrategy");
            Console.Write("Enter choice: ");
            string input = Console.ReadLine();

            IStrategy strategy;

            if (input == "1")
                strategy = new SomeStrategy();
            else if (input == "2")
                strategy = new PerimeterHuggerStrategy();
            else
                strategy = new SpiralStrategy();

            Robot robot = new Robot(map, strategy);
            robot.Move(0, 0);
            robot.StartCleaning();

            Console.WriteLine("Done.");
        }
    }
}
