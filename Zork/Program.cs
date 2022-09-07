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
                Console.Write($"{_rooms[_currentRoom]}\n> ");
                //set up a string variable that we are going to use for user inputs, stopping ___
                //Trim() gets rid of whitespace (spaces), LeftTrim and RightTrim are also syntax. To.Upper() makes everything uppercase so case sensitive stuff is easier to manage (took out)
                string inputString = Console.ReadLine().Trim();
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
                        if (Move(command))
                        {
                            outputString = $"You moved {command}."; //interpolate the string, very effective
                        }
                        else
                        {
                            outputString = "The way is shut!";
                        }
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

        private static bool Move(Commands command)
        {
            bool didMove = false;

            switch (command)
            {
                case Commands.North:
                case Commands.South:
                    break;

                case Commands.East when _currentRoom < _rooms.Length - 1:
                    _currentRoom++;
                    didMove = true;
                    break;

                case Commands.West when _currentRoom > 0:
                    if (_currentRoom > 0)
                    _currentRoom--;
                    didMove = true;
                    break;
            }

            return didMove;
        } //returns true if player moved false if they didn't

        //hardcode an array for rooms that is readonly (cant be changed during runtime)
        private static readonly string[] _rooms = { "Forest", "West of House", "Behind House", "Clearing", "Canyon View" };
        private static int _currentRoom = 1;


    }
}
