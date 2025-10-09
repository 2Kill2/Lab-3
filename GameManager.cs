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

            //I had to rewrite this whole thing because it was terrible mess the first time

            Console.WriteLine("DICE BATTLE!");

            Console.WriteLine("Choose two dice to roll with.");
            ShowInventory();

           
            // With this corrected line:
            int playerDieOne = GetDieChoice("first");
            int playerDieTwo = GetDieChoice("second");

            DieRoller dieRoller = new DieRoller();

            while (!Winner)
            {
                Console.WriteLine("Rolling dice...");
                int rollOne = dieRoller.RollDie(playerDieOne);
                int rollTwo = dieRoller.RollDie(playerDieTwo);

                playerRolls.Add(rollOne);
                playerRolls.Add(rollTwo);

                Console.WriteLine($"You rolled a {rollOne} and a {rollTwo}.");

                if (rollOne == rollTwo)
                {
                    Console.WriteLine("You got a match!");
                    Winner = TurnChecker(PlayerTurns, ComputerTurns);
                }
                else
                {
                    PlayerTurns++;
                    Console.WriteLine("No match, computer's turn.");
                }
            }

            //computer turn logic here...
            Random rng = new Random();
            List<int> choices = new List<int> { 4, 6, 8, 20 };
            int compDieOne = choices[rng.Next(choices.Count)];
            int compDieTwo = choices[rng.Next(choices.Count)];

            int cpuRollOne = dieRoller.RollDie(compDieOne);
            int cpuRollTwo = dieRoller.RollDie(compDieTwo);

            computerRolls.Add(cpuRollOne);
            computerRolls.Add(cpuRollTwo);

            Console.WriteLine($"Computer rolled a {cpuRollOne} and a {cpuRollTwo}.");

            if (cpuRollOne == cpuRollTwo)
            {
                Console.WriteLine("Computer got a match!");
                Winner = TurnChecker(PlayerTurns, ComputerTurns);
            }
            else
            {
                ComputerTurns++;
                Console.WriteLine("No match, your turn.");
            }

            Console.WriteLine("DICE BATTLE OVER!");
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
            GenerateMap();


            //begginer dice
            Inventory.Add(20);
            Inventory.Add(16);

            //create rooms
            var center = new Center();
            var treasureRoom = new TreasureRoom();
            var encounterRoom = new EncounterRoom();

            //connect rooms
            center.AddExit("north", treasureRoom);
            treasureRoom.AddExit("south", center);
            center.AddExit("east", encounterRoom);
            encounterRoom.AddExit("west", center);

            Room currentRoom = center;

            Console.WriteLine("You are stuck in your house, go do something to elivate your boredom.");
            currentRoom.EnterRoom();

            bool exploring = true;
            while (exploring)
            {
                Room selectedRoom = map[playerPos.row, playerPos.col];
                currentRoom.EnterRoom();

                Console.WriteLine("What next?");
                string cmd = Console.ReadLine().ToLower();

                switch (cmd)
                {
                    case "n":
                        MovePlayer(-1, 0);
                        break;
                    case "e":
                        MovePlayer(0, 1);
                        break;
                    case "s":
                        MovePlayer(1, 0);
                        break;
                    case "w":
                        MovePlayer(0, -1);
                        break;
                    case "search":
                        Console.WriteLine(currentRoom.RoomSearch(this));
                        break;
                    case "inventory":
                        ShowInventory();
                        break;
                    case "quit":
                        exploring = false;
                        Console.WriteLine("You exit the house and go outside.");
                        break;
                    default:
                        Console.WriteLine("Invalid command. Use N, E, S, W to move, 'search' to look around, 'inventory' to check your items, or 'quit' to exit.");
                        break;
                }
            }
        }

        private void GenerateMap()
        {
            map = new Room[rows, cols];

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    int roll = rng.Next(1, 3);

                    Room room;
                    if (roll == 1)
                    {
                        room = new TreasureRoom();
                    }
                    else if (roll == 2)
                    {
                        room = new EncounterRoom();
                    }
                    else
                    {
                        room = new Center();

                        
                    }
                        map[r, c] = room;
                }
            }

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    Room room = map[r, c];
                    room.North = (r > 0) ? map[r - 1, c] : null;
                    room.South = (r < rows - 1) ? map[r + 1, c] : null;
                    room.West = (c > 0) ? map[r, c - 1] : null;
                    room.East = (c < cols - 1) ? map[r, c + 1] : null;
                }
            }

            playerPos = (rng.Next(rows), rng.Next(cols));
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
                if (!beenHere)
                {
                    beenHere = true;
                    return $"You enter the {nameof}, for the first time.";
                }
                else
                {
                    return $"You return to the {nameof}.";
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
                Console.WriteLine(RoomDescription());
                if (!beenHere)
                {
                    Console.WriteLine($"You enter the {nameof}, for the first time. Why do you have this in your house?");
                    beenHere = true;
                }
                else
                {
                    Console.WriteLine($"You return to the {nameof}.");
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
                    Console.WriteLine("You already took the treasure.");
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
                }
                else
                {
                    Console.WriteLine("You return to the encounter room.");
                }
                Console.WriteLine("An enemy appears!");
                game.PlayGame();
                beenHere = true;
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
