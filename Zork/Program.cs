using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Zork
{
    internal class Program
    {
        //private is only accessible in this class
        private static Room CurrentRoom
        {
            get
            {
                return _rooms[_location.Row, _location.Column];
            }
        }

        private static void Main(string[] args) //void infront of main is return type so returns nothing (void), private is implied
        {
            string roomsFileName = args.Length > 0 ? args[(int)CommandLineArguments.RoomsFileName] : @"Content\Rooms.json";
            InitializeRoomDescriptions(roomsFileName);

            Console.WriteLine("Welcome to Zork!");

            Room previousRoom = null;
            bool isRunning = true;
            while (isRunning)
            {
                //">" to show where player input is being written
                Console.Write($"{CurrentRoom}\n> ");
                /*
                if (previousRoom != Player.CurrentRoom)
                {
                    Console.WriteLine(Player.CurrentRoom.Description);
                    previousRoom = Player.CurrentRoom;
                }
                */
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
                        //outputString = Player.CurrentRoom.Descripton;
                        outputString = CurrentRoom.Description;
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
                //GetLength(0) accesses the rows, GetLength(1) accesses the amount of columns
                case Commands.North when _location.Row < _rooms.GetLength(0) - 1:
                    _location.Row++;
                    didMove = true;
                    break;

                case Commands.South when _location.Row > 0:
                    _location.Row--;
                    didMove = true;
                    break;

                case Commands.East when _location.Column < _rooms.GetLength(1) - 1:
                    _location.Column++;
                    didMove = true;
                    break;

                case Commands.West when _location.Column > 0:
                    _location.Column--;
                    didMove = true;
                    break;
            }

            return didMove;
        } //returns true if player moved false if they didn't

        private static void InitializeRoomDescriptions(string roomsFilename)
        {
           _rooms = JsonConvert.DeserializeObject<Room[,]>(File.ReadAllText(roomsFilename));
        }

        private static Room[,] _rooms;

        private static (int Row, int Column) _location = (1, 1); //tuple, two fields, Row and Column

        private enum CommandLineArguments
        {
           RoomsFileName = 0
        }

    }
}
