namespace LabWork
{
    internal class GameManager
    {
        /*
            greet player
            ask to play
            (if yes continue, if no exit)
            explain rules
            display list of dice
            decide on 2 dice to have
            first round of rolling
            (player rolls first, computer rolls second)
            check for matching numbers
            roll until dice match numbers
            higher number of rolls it took to get a match loses
         */

        //entry point of the application or else I enoucnter an error
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

            //call DieRoller class
            Console.WriteLine("Rolling the dice...");




            DieRoller dieRoller = new DieRoller();

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

    }
}
