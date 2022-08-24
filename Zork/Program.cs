using System;

namespace Zork
{
    class Program
    {
        static void Main(string[] args)
        {
            //Welcome message
            Console.WriteLine("Welcome to Zork!");
            //set up a string variable that we are going to use for user inputs
            //stopping ___
            //Trim() gets rid of whitespace (spaces) LeftTrim and RightTrim are also syntax
            string inputString = Console.ReadLine().Trim(); //.ToUpper() can be placed here
            inputString = inputString.ToUpper();  //to upper makes everything uppercase so case sensitive stuff is easier to manage
            //strgcmp is case sensitive
            if (inputString == "QUIT")
            {
                Console.WriteLine("Thank you for playing.");
            }
            else if (inputString == "LOOK")
            {
                Console.WriteLine("This is an open field west of a white house, with a boarded front door. \nA rubber may saying 'Welcome to Zork!' lies by the door.");
            }
            else
            {
                Console.WriteLine($"Unrecognized command: {inputString}"); //the $ makes anything in the {} an interpolating string, which is a string C# tries to convert to an actual string
            }
        }
    }
}
