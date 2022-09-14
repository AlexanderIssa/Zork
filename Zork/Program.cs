﻿using System;
using System.Collections.Generic;

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

        private static void Main() //void infront of main is return type so returns nothing (void), private is implied
        {

            Room westOfHouse = new Room("West of House", "A rubber mat saying 'Welcome to Zork!' lies by the door." );


            List<Room> rooms = new List<Room>();
            rooms.Add(westOfHouse);












            //Welcome message
            Console.WriteLine("Welcome to Zork!");
            InitializeRoomDescriptions();
            bool isRunning = true;
            while (isRunning)
            {
                //">" to show where player input is being written
                Console.Write($"{CurrentRoom}\n> ");
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

        private static void InitializeRoomDescriptions()
        {
            _rooms[0, 0].Description = "You are on a rock-strewn trail.";
            _rooms[0, 1].Description = "You are facing the south side of a white house.";
            _rooms[0, 2].Description = "You are on a rock-strewn trail.";

            _rooms[1, 0].Description = "You are on a rock-strewn trail.";
            _rooms[1, 1].Description = "You are on a rock-strewn trail.";
            _rooms[1, 2].Description = "You are on a rock-strewn trail.";

            _rooms[2, 0].Description = "You are on a rock-strewn trail.";
            _rooms[2, 1].Description = "You are on a rock-strewn trail.";
            _rooms[2, 2].Description = "You are on a rock-strewn trail.";
        }

        //hardcode an array for rooms that is readonly (cant be changed during runtime)
        //rectangular array, every row has the same amount of columns
        private static readonly Room[,] _rooms = 
        {
            { new Room("Rocky Trail"), new Room("South of House"), new Room("Canyon View") },
            { new Room("Forest"), new Room("West of House"), new Room("Behind House") },
            { new Room("Dense Woods"), new Room("North of House"), new Room("Clearing") }
        };

        private static (int Row, int Column) _location = (1, 1); //tuple, two fields, Row and Column

        //1 way to do it
        //private static int _location.Row = 1;
        //private static int _location.Column = 1;

        //another way to do it
        //private static Location _location = new Location() { Row = 1, Column = 1 };
    }
}
