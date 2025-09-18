using System;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;

public class DieRoller
{
    //dice sides
    Random random = new Random();
    public int d4 = 4;
    public int d6 = 6;
    public int d8 = 8;
    public int d20 = 20;

    //creates a list to store results
    List<int> results = new List<int>();


    //rolls the dice
    public int RollDie(int sides)
    {
        return random.Next(1, sides);
    }

    //stores the results
    public void StoredResult(int result)
    {
        results.Add(result);
    }

    //call the results
    public List<int> GetResults()
    {
        return results;
    }
}


