namespace LabWork
{
    internal class GameManager
    {
        static void Main(string[] args)
        {
            GameManager gameManager = new GameManager();
            gameManager.PlayGame();
        }

        // Game starting point
        public void PlayGame()
        {
            //welcome message
            Console.WriteLine("Welcome to Die vs, Die!");
            Console.WriteLine("Alex Lee - 09/16");

            //get player name
            Console.WriteLine("What is your name?");
            string playerName = Console.ReadLine();
            Console.WriteLine("Hello " + playerName + ", would you like to play? Y/N");
            string playResponse = Console.ReadLine().ToUpper();

            if (playResponse == "Y")
            {
                Console.WriteLine("Great! Let's begin!");

            }
            else if (playResponse == "N")
            {
                Console.WriteLine("No? Goodbye!");
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("Invalid intput, please enter Y or N.");
                return;
            }

            //explain rules
            Console.WriteLine("Here are the rules:");
            Console.WriteLine("there are 4 dice to choose from: d4, d6, d8, and d20");
            Console.WriteLine("You will choose two to roll, we will both roll them until we get a matching number," +
                " who ever has the least amount of rolls to get a match wins!");
            Console.WriteLine("You go first!");
            Console.WriteLine("Here are the dice you can choose from:");
            Console.WriteLine("please input your FIRST roll, 4, 6, 8, 20.");

            int playerTurns = 0;
            int computerTurns = 0;

            //get player dice choice
            string playerDiceChoiceOne = Console.ReadLine().ToLower();

            //get [player second dice choice
            string playerDiceChoiceTwo = Console.ReadLine().ToLower();

            Console.WriteLine("You chose " + playerDiceChoiceOne + " and " + playerDiceChoiceTwo + ".");

            DieRoller dieRoller = new DieRoller();

            //try parse playerchoice to int

            bool parseOne = int.TryParse(playerDiceChoiceOne, out int playerDieOne);

            //roll the dice selected
            if (parseOne == true)
            {
                int rollOne = dieRoller.RollDie(playerDieOne);
            }
            else
            {
                Console.WriteLine("Invalid input, please enter 4, 6, 8, 20.");
                return;
            }

            bool parseTwo = int.TryParse(playerDiceChoiceTwo, out int playerDieTwo);

            //roll the second die selected
            if (parseTwo == true)
            {
                int rollTwo = dieRoller.RollDie(playerDieTwo);
            }
            else
            {
                Console.WriteLine("Invalid input, please enter 4, 6, 8, 20.");
                return;
            }

            Console.WriteLine("You rolled a " + playerDieOne + " and a " + playerDieTwo + ".");

            if (playerDieOne == playerDieTwo)
            {
                Console.WriteLine("You got it!");
                //run Turn Checker method
                TurnChecker(playerTurns, computerTurns);

                return;
            }
            else
            {
                playerTurns++;
                Console.WriteLine("No match, computer's turn.");
            }

            //computer turn

            //create list (maybe array better?)
            List<int> Choices = new List<int>();
            Choices.Add(4); Choices.Add(6); Choices.Add(8); Choices.Add(20);

            //randomly choose from list
            Random randomOne = new Random();
            int compChoiceOne = Choices[randomOne.Next(Choices.Count)];

            Console.WriteLine("I think I will choose " + compChoiceOne + ", as my first die.");

            //randomly choose from list again
            Random randomTwo = new Random();
            int compChoiceTwo = Choices[randomTwo.Next(Choices.Count)];

            Console.WriteLine("I think I will choose " + compChoiceTwo + ", as my second die.");

            Console.WriteLine("I will now roll my dice.");

            int cpurollOne = dieRoller.RollDie(compChoiceOne);
            int cpurollTwo = dieRoller.RollDie(compChoiceTwo);

            Console.WriteLine("I got a " + cpurollOne + " and a " + cpurollTwo + ".");

            //check if they match
            if (cpurollOne == cpurollTwo)
            {
                Console.WriteLine("I got it!");
                //run Turn Checker method
                TurnChecker(playerTurns, computerTurns);
                return;
            }
            else
            {
                computerTurns++;
                Console.WriteLine("No match, your turn.");
                return;
            }

            //loop back to player


            //call DieRoller class
            Console.WriteLine("Rolling the dice...");





            //roll each die and store the results
            int rollD4 = dieRoller.RollDie(dieRoller.d4);
            dieRoller.StoredResult(rollD4);
            int rollD6 = dieRoller.RollDie(dieRoller.d6);
            dieRoller.StoredResult(rollD6);
            int rollD8 = dieRoller.RollDie(dieRoller.d8);
            dieRoller.StoredResult(rollD8);
            int rollD20 = dieRoller.RollDie(dieRoller.d20);
            dieRoller.StoredResult(rollD20);

            List<int> results = dieRoller.GetResults();

            foreach (int roll in results)
            {
                Console.WriteLine(roll);
            }

            //math time
            Console.WriteLine("+ example");
            int sum = results.Sum();
            Console.WriteLine("adds them up: " + sum);

            Console.WriteLine("- example");
            int difference = results[0];
            Console.WriteLine("subtracts them: " + difference);

            Console.WriteLine("/ example, in this example 6/3");
            int division = 6 / 3;
            Console.WriteLine("divides them: " + division);

            Console.WriteLine("* example, in this example 6*3");
            int product = 6 * 3;
            Console.WriteLine("multiplies them: " + product);

            Console.WriteLine("++ example, incremently adds value, starting value is 0, using increment++ it is changed to 1");
            int increment = 0;
            increment++;
            Console.WriteLine("increment: " + increment);

            Console.WriteLine("-- example, decremently subtracts value, starting value is 1, using decrement-- it is changed to 0");
            int decrement = 1;
            decrement--;
            Console.WriteLine("decrement: " + decrement);

            Console.WriteLine("% example, in this example 5%2");
            int modulus = 5 % 2;
            Console.WriteLine("modulus: " + modulus);

            Console.WriteLine("Thanks for playing!");


        }

    
        
        private void TurnChecker(int playerTurns, int computerTurns)
        {
            if (computerTurns > playerTurns)
            {
                Console.WriteLine("You win!");
                return;
            }
            else if (computerTurns < playerTurns)
            {
                Console.WriteLine("I win!");
                return;
            }
            else
            {
                Console.WriteLine("It's a tie!");
                return;
            }
        }

    }
}
