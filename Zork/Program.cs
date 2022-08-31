using System;

namespace Zork
{
    class Program
    {
        static void Main() //void infront of main is return type so returns nothing (void)
        {
            //Welcome message
            Console.WriteLine("Welcome to Zork!");
            bool isRunning = true;
            while (isRunning)
            {
                //">" to show where player input is being written
                Console.Write("> ");
                //set up a string variable that we are going to use for user inputs, stopping ___
                //Trim() gets rid of whitespace (spaces), LeftTrim and RightTrim are also syntax. To.Upper() makes everything uppercase so case sensitive stuff is easier to manage
                string inputString = Console.ReadLine().Trim().ToUpper();
                //create a data type (enumeration) off of the Commands.cs file
                Commands command = ToCommand(inputString);

                string outputString; //assigning a value here would be inefficient as we would be assigning it in all cases in switch below
                switch (command)
                {
                    case Commands.Quit:
                        isRunning = false; //this will not stop the while loop mid run, the while loop goes through the whole block before updating
                        outputString = "Thank you for playing!"; //so this will run
                        break;

                    case Commands.Look:
                        outputString = "This is an open field west of a white house, with a boarded front door. \nA rubber mat saying 'Welcome to Zork!' lies by the door.";
                        break;

                    case Commands.North:
                    case Commands.South:
                    case Commands.East:
                    case Commands.West:
                        outputString = $"You moved {command}."; //interpolate the string, very efficient
                        break;
                    
                    default:
                        outputString = "Unknown command.";
                        break;
                }

                Console.WriteLine(outputString); //write the output string from respective case
            } 

        }

        static Commands ToCommand(string commandString) //this method will return a Commands variable
        {

            if (Enum.TryParse<Commands>(commandString, true, out Commands command)) //if enum TryParse of commands...
            {
                return command;
            }
            else //if that command passed through the commandString is not existent in the Commands.cs file then return Unkown
            {
                return Commands.Uknown;
            }

        }

    }
}
