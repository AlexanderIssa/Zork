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
            Commands command = ToCommand(inputString); //created a data type (enumeration) off of the Commands.cs file

            /*
            if (inputString == "QUIT")
            {
                command = Commands.Quit;
            }
            else if (inputString == "LOOK")
            {
                command = Commands.Look;
            }
            else
            {
                command = Commands.Uknown;
            }
            */

            //strgcmp is case sensitive
            if (command == Commands.Quit)
            {
                Console.WriteLine("Thank you for playing.");
            }
            else if (command == Commands.Look)
            {
                Console.WriteLine("This is an open field west of a white house, with a boarded front door. \nA rubber may saying 'Welcome to Zork!' lies by the door.");
            }
            else
            {
                Console.WriteLine($"Unrecognized command: {inputString}"); //the $ makes anything in the {} an interpolating string, which is a string C# tries to convert to an actual string
            }
        } //void infront of main is return type so returns nothing

        static Commands ToCommand(string commandString) //this will return a Commands variable
        {

            if (Enum.TryParse<Commands>(commandString, true, out Commands command))
            {
                return command;
            }
            else
            {
                return Commands.Uknown;
            }



            //above code is a simplified version of this below code
            /*
            try //try to do this command, if it throws an exception move to catch
            {
                return (Commands)Enum.Parse(typeof(Commands), commandString, true);//static method that deals with any type of enum, tell it what type of enum u want to use, can have a third arg that ignores case
                                                                                   //enum parse returns an object (everything, ALL DATA, is an object)
            }
            catch (ArgumentException) //catch that argument exception (inherts from exception) and do what's inside instead
            {
                return Commands.Uknown;
            }
            */

        }

    }
}
