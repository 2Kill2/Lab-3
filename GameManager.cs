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
            List<int> computerRolls = new List<int>();
            List<int> playerRolls = new List<int>();

            //keep playing ask
            bool keepPlaying = true;
            {
                int playerTurns = 0;
                int computerTurns = 0;
                bool Winner = false;

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
                while (playResponse != "Y" && playResponse != "N")
                {
                    Console.WriteLine("Invalid input, please enter Y or N.");
                    playResponse = Console.ReadLine().ToUpper();
                }

                //explain rules
                Console.WriteLine("okay" + playerName + ", Here are the rules:");
                Console.WriteLine("there are 4 dice to choose from: d4, d6, d8, and d20");
                Console.WriteLine("You will choose two to roll, we will both roll them until we get a matching number," +
                    " who ever has the least amount of rolls to get a match wins!");
                Console.WriteLine("You go first!");
                Console.WriteLine("Here are the dice you can choose from:");



                //try parse playerchoice to int



                //GAME LOOP STARTS HERE!!!!
                while (!Winner)
                {


                    Console.WriteLine("please input your FIRST then SECOND roll, 4, 6, 8, 20.");

                    //create variables to store rolls
                    int rollOne = 0;
                    int rollTwo = 0;

                    //get player dice choice
                    string playerDiceChoiceOne = Console.ReadLine().ToLower();

                    //get [player second dice choice
                    string playerDiceChoiceTwo = Console.ReadLine().ToLower();

                    Console.WriteLine("You chose " + playerDiceChoiceOne + " and " + playerDiceChoiceTwo + ".");

                    DieRoller dieRoller = new DieRoller();

                    bool parseOne = int.TryParse(playerDiceChoiceOne, out int playerDieOne);






                    //roll the dice selected
                    while (true)
                    {
                        if (int.TryParse(playerDiceChoiceOne, out playerDieOne) && (playerDieOne == 4 ||
                            playerDieOne == 6 || playerDieOne == 8 || playerDieOne == 20))
                        {
                            //roll the dice
                            rollOne = dieRoller.RollDie(playerDieOne);
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid input, please enter your FIRST die, 4, 6, 8, 20.");
                            playerDiceChoiceOne = Console.ReadLine().ToLower();
                        }
                    }



                    bool parseTwo = int.TryParse(playerDiceChoiceTwo, out int playerDieTwo);

                    //roll the second die selected
                    while (true)
                    {
                        if (int.TryParse(playerDiceChoiceTwo, out playerDieTwo) && (playerDieTwo == 4 ||
                            playerDieTwo == 6 || playerDieTwo == 8 || playerDieTwo == 20))
                        {
                            // roll the second dice
                            rollTwo = dieRoller.RollDie(playerDieOne);
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid input, please enter your SECOND die, 4, 6, 8, 20.");
                            playerDiceChoiceTwo = Console.ReadLine().ToLower();
                        }
                    }

                    Console.WriteLine("You rolled a " + rollOne + " and a " + rollTwo + ".");

                    //track dice for stats
                    playerRolls.Add(rollOne);
                    playerRolls.Add(rollTwo);

                    if (rollOne == rollTwo)
                    {
                        Console.WriteLine("You got it!");
                        //run Turn Checker method
                        Winner = TurnChecker(playerTurns, computerTurns);

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

                    //roll the dice
                    int cpurollOne = dieRoller.RollDie(compChoiceOne);
                    int cpurollTwo = dieRoller.RollDie(compChoiceTwo);

                    //track dice for stats
                    computerRolls.Add(cpurollOne);
                    computerRolls.Add(cpurollTwo);


                    Console.WriteLine("I got a " + cpurollOne + " and a " + cpurollTwo + ".");

                    //check if they match
                    if (cpurollOne == cpurollTwo)
                    {
                        Console.WriteLine("I got it!");
                        //run Turn Checker method
                        Winner = TurnChecker(playerTurns, computerTurns);

                    }
                    else
                    {
                        computerTurns++;
                        Console.WriteLine("No match, your turn.");

                    }

                    //calculate averages
                    double playerAverage = playerRolls.Average();
                    double computerAverage = computerRolls.Average();

                    Console.WriteLine("Your average roll was: " + playerAverage);
                    Console.WriteLine("My average roll was: " + computerAverage);


                    while (true)
                    {
                        Console.WriteLine("Would you like to keep playing? Y/N");
                        string againAsk = Console.ReadLine().ToUpper();
                        if (againAsk == "Y")
                        {
                            Console.WriteLine("Great! Let's begin!");
                        }
                        else if (againAsk == "N")
                        {
                            Console.WriteLine("No? Goodbye!");
                            keepPlaying = false;
                        }
                        else
                        {
                            Console.WriteLine("Invalid input, please enter Y or N.");
                            againAsk = Console.ReadLine().ToUpper();
                        }

                    }

                    

                    //END OF GAME LOOP!!!

                }
            }
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

    }
}
