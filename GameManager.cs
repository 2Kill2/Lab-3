namespace LabWork
{
    internal class GameManager
    {
        public List<int> Inventory { get; set; } = new List<int>();
        public void ShowInventory()
        {
            if (Inventory.Count == 0)
            {
                Console.WriteLine("Your pockets are empty.");
            }
            else
            {
                Console.WriteLine("You have the following items in your inventory:");
                foreach (int d in Inventory)
                    Console.WriteLine(d);
            }
        }

        Room[,] house = new Room[3, 3];

        private Room[,] map;
        private int rows = 3;
        private int cols = 3;
        private (int row, int col) playerPos;
        private Random rng = new Random();

        static void Main(string[] args)
        {
            GameManager gameManager = new GameManager();
            gameManager.ExploreRooms();
        }

        // Game starting point
        public void PlayGame()
        {
            List<int> computerRolls = new List<int>();
            List<int> playerRolls = new List<int>();

            int PlayerTurns = 0;
            int ComputerTurns = 0;
            bool Winner = false;

            //I had to rewrite this whole thing because it sucked

            Console.WriteLine("DICE BATTLE!");

            Console.WriteLine("Choose two dice to roll with.");
            ShowInventory();

            int playerDieOne = GetDieChoice("first");
            int playerDieTwo = GetDieChoice("second");

            DieRoller dieRoller = new DieRoller();

            while (!Winner)
            {
                Console.WriteLine("Player's turn!");
                Console.WriteLine("Rolling dice...");
                Console.WriteLine("press ENTER to continue...");
                PlayerTurns++;
                Console.ReadLine();

                int rollOne = dieRoller.RollDie(playerDieOne);
                int rollTwo = dieRoller.RollDie(playerDieTwo);

                playerRolls.Add(rollOne);
                playerRolls.Add(rollTwo);

                Console.WriteLine($"You rolled a {rollOne} and a {rollTwo}.");

                if (rollOne == rollTwo)
                {
                    Console.WriteLine("You got a match!");
                    Winner = TurnChecker(PlayerTurns, ComputerTurns);
                    break;
                }


                Console.WriteLine("Computer's turn!");
                Console.WriteLine("Rolling dice...");
                Console.WriteLine("press ENTER to continue...");
                ComputerTurns++;
                Console.ReadLine();

                int[] computerDice = { 4, 6, 8, 20 };
                int computerDieOne = computerDice[rng.Next(computerDice.Length)];
                int computerDieTwo = computerDice[rng.Next(computerDice.Length)];

                int compRollOne = dieRoller.RollDie(computerDieOne);
                int compRollTwo = dieRoller.RollDie(computerDieTwo);

                Console.WriteLine($"Computer rolled a {compRollOne} and a {compRollTwo}.");

                if (compRollOne == compRollTwo)
                {
                    Console.WriteLine("Computer got a match!");
                    Winner = TurnChecker(PlayerTurns, ComputerTurns);
                    return;
                }

                Console.WriteLine("No matches this round, rolling again...");

                Console.WriteLine("Press ENTER to continue...");
                Console.ReadLine();
            }
            Console.WriteLine("Game Over!");

            Console.WriteLine("press ENTER to continue...");
            Console.ReadLine();
        }

        public int GetDieChoice(string order)
        {
            while (true)
            {
                Console.WriteLine($"Choose your {order} die from your inventory (e.g., 4, 6, 8, 20):");
                string input = Console.ReadLine();
                if (int.TryParse(input, out int dieSides) && Inventory.Contains(dieSides))
                {
                    return dieSides;
                }
                else
                {
                    Console.WriteLine("Invalid choice. Please select a die you have in your inventory.");
                }
            }
        }

        public void ExploreRooms()
        {
            //generate the map
            map = GenerateMap(3, 3);

            //set sstarting position
            playerPos = (1, 1); // start in the center
            Room currentRoom = map[playerPos.row, playerPos.col];
            currentRoom.beenHere = true;

            //give beginner dice
            Inventory.Add(16);
            Inventory.Add(20);

            Console.WriteLine("You are bored in your house, find something to eliviate your boredom.");

            bool exploring = true;
            while (exploring)
            {
                DrawMap(map, playerPos);

                currentRoom = map[playerPos.row, playerPos.col];
                Console.WriteLine(currentRoom.RoomEntered(this));

                Console.WriteLine("What' next?");
                string cmd = Console.ReadLine().ToLower();

                switch (cmd)
                {
                    case "n":
                    case "north":
                        MovePlayer(-1, 0);
                        break;
                    case "s":
                    case "south":
                        MovePlayer(1, 0);
                        break;
                    case "e":
                    case "east":
                        MovePlayer(0, 1);
                        break;
                    case "w":
                    case "west":
                        MovePlayer(0, -1);
                        break;
                    case "i":
                    case "inventory":
                        ShowInventory();
                        Console.WriteLine("Press Enter to continue...");
                        Console.ReadLine();
                        break;
                    case "sc":
                    case "search":
                        Console.WriteLine(currentRoom.RoomSearch(this));
                        Console.WriteLine("Press Enter to continue...");
                        Console.ReadLine();
                        break;
                    case "q":
                    case "quit":
                        exploring = false;
                        Console.WriteLine("you exit the house and go outside.");
                        break;
                    default:
                        Console.WriteLine("Invalid command. Use n/s/e/w to move, i for inventory, search to search the room, or q to quit.");
                        Console.WriteLine("Press Enter to continue...");
                        Console.ReadLine();
                        break;
                }
            }
        }

        //chat gpt helped me with this function because I was struggling to make it work
        private Room[,] GenerateMap(int rows, int cols)
        {
            Room[,] map = new Room[rows, cols];
            Random rng = new Random();
            int counter = 1; // for unique room names

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    Room room;

                    int roomType = rng.Next(1, 4); // 1 = Treasure, 2 = Encounter
                    if (roomType == 1)
                        room = new TreasureRoom();
                    else if (roomType == 2)
                        room = new EncounterRoom();
                    else
                        // Default to Center room
                        room = new Center();

                    // give each room a unique name
                    room.nameof = $"Room {counter}";
                    counter++;

                    map[r, c] = room;
                }
            }

            // Link neighbors
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    Room room = map[r, c];
                    if (r > 0) room.AddExit("north", map[r - 1, c]);
                    if (r < rows - 1) room.AddExit("south", map[r + 1, c]);
                    if (c > 0) room.AddExit("west", map[r, c - 1]);
                    if (c < cols - 1) room.AddExit("east", map[r, c + 1]);
                }
            }

            return map;
        }

        public void DrawMap(Room[,] map, (int row, int col) playerPos)
        {
            int rows = map.GetLength(0);
            int cols = map.GetLength(1);

            Console.WriteLine();

            //renders lines on top of rows
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    Console.Write("+---");
                }
                Console.WriteLine("+");


                //middle cel with player symbol
                for (int c = 0; c < cols; c++)
                {
                    //check for player and render icon
                    if (r == playerPos.row && c == playerPos.col)
                    {
                        Console.Write("| * ");
                    }
                    else if (map[r, c] != null)
                    {
                        //mark visited rooms
                        Console.Write(map[r, c]?.beenHere == true ? "| . " : "|  ");
                    }
                    else
                    {
                        Console.Write("|  ");
                    }
                }
                Console.WriteLine("|");

                //bottom border
                for (int c = 0; c < map.GetLength(1); c++)
                {
                    Console.Write("+---");
                }
                Console.WriteLine("+");
            }
        }

        private void MovePlayer(int dRow, int dCol)
        {
            int newRow = playerPos.row + dRow;
            int newCol = playerPos.col + dCol;

            if (newRow < 0 || newRow >= rows || newCol < 0 || newCol >= cols)
            {
                Console.WriteLine("You would go that way normally, but someone put a wall in the way.");
                return;
            }

            Room currentRoom = map[playerPos.row, playerPos.col];
            currentRoom.RoomExit();

            playerPos = (newRow, newCol);
        }

        //checks who has the least amount of turns and declares winner
        private bool TurnChecker(int playerTurns, int computerTurns)
        {
            if (computerTurns > playerTurns)
            {
                Console.WriteLine("You win!");
                Console.WriteLine("Player turns: " + playerTurns);
                Console.WriteLine("Computer turns: " + computerTurns);
                return true;
            }
            else if (computerTurns < playerTurns)
            {
                Console.WriteLine("I win!");

                Console.WriteLine("Player turns: " + playerTurns);
                Console.WriteLine("Computer turns: " + computerTurns);
                return true;

            }
            else
            {
                Console.WriteLine("Player turns: " + playerTurns);
                Console.WriteLine("Computer turns: " + computerTurns);
                return false;
            }
        }

       

        public abstract class Room
        {
            public Room North, South, East, West;
            //each room connects to other rooms
            public Dictionary<String, Room> Exits { get; set; } = new();

            public string nameof { get; set; }
            public bool beenHere { get; set; }

            //methods every room must implement
            public abstract string RoomDescription();
            public abstract string RoomEntered(GameManager game);
            public abstract string RoomSearch(GameManager game);
            public abstract string RoomExit();

            //exit the room
            public void AddExit(string direction, Room destination)
            {
                Exits[direction.ToLower()] = destination;
            }

            //movement between rooms
            public Room Move(string direction)
            {
                direction = direction.ToLower();
                if (Exits.ContainsKey(direction))
                {
                    Console.WriteLine((RoomExit()));
                    return Exits[direction];
                }
                else
                {
                    Console.WriteLine("You can't go that way.");
                    return this;
                }
            }

            public virtual void EnterRoom()
            {
                if (!beenHere)
                {
                    Console.WriteLine($"You enter the {nameof}, for the first time.");
                    beenHere = true;
                }
                else
                {
                    Console.WriteLine($"You return to the {nameof}.");
                }
            }
        }

        public class Center : Room
        {
            public Center() { nameof = "a Room"; }
            public override string RoomDescription() => "You are in a regular room of your house.";

            public override string RoomEntered(GameManager game)
            {
                string desc = RoomDescription();
                if (!beenHere)
                {
                    beenHere = true;
                    return $"You enter the {nameof}, for the first time. \n{desc}";
                }
                else
                {
                    return $"You return to the {nameof}. \n{desc}";
                }
            }

            public override string RoomSearch(GameManager game)
            {
                return "You look around and fun nothing fun.";
            }

            public override string RoomExit()
            {
                return "You leave the center room.";
            }
        }

        public class TreasureRoom : Room
        {
            private bool treasureTaken = false;
            public TreasureRoom() { nameof = "Treasure Room"; }
            public override string RoomDescription() => "You are in the treasure room of the house.";
            public override string RoomEntered(GameManager game)
            {
                string desc = RoomDescription();
                if (!beenHere)
                {
                    Console.WriteLine($"You enter the {nameof}, for the first time. Why do you have this in your house? \n{desc}");
                    beenHere = true;
                }
                return $"You {(beenHere ? "return to" : "enter")} the {nameof}.";
            }
            public override string RoomSearch(GameManager game)
            {
                if (!treasureTaken)
                {
                    int[] treasure = { 4, 6, 8, 20 };
                    Random rng = new Random();
                    int newDie = treasure[rng.Next(treasure.Length)];
                    Console.WriteLine($"You look around and find a d{newDie}, you take it.");
                    game.Inventory.Add(newDie);
                    treasureTaken = true;
                    return $"You found a d{newDie} and added it to your inventory.";
                }
                else
                {
                    return "You already took the treasure.";
                }
            }
            public override string RoomExit() => "you leave the treasure room.";
        }

        public class EncounterRoom : Room
        {
            public EncounterRoom() { nameof = "Encounter Room"; }
            public override string RoomDescription() => "You are in the encounter room of the house, wait what?:";
            public override string RoomEntered(GameManager game)
            {
                Console.WriteLine(RoomDescription());
                if (!beenHere)
                {
                    Console.WriteLine("You enter the encounter room, for the first time.");
                    beenHere = true;
                    Console.WriteLine("An enemy appears!");
                    game.PlayGame();
                }
                return $"You {(beenHere ? "return to" : "enter")} the {nameof}.";
            }
            public override string RoomSearch(GameManager game)
            {
                return "You look around and see the body of a dead dice man.";
            }
            public override string RoomExit() => "You leave the encounter room, why do you have a room like this?";
        }
    }
}
