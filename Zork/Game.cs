﻿using Newtonsoft.Json;
using System;

namespace Zork
{
    public class Game
    {
        //Property named World that is readonly
        public World World { get; private set; }
        public Player Player { get; private set; }
        //constructor that assigns it
        public Game(World world, string startingLocation)
        {
            World = world;
            Player = new Player(World, startingLocation);
        }

        public void Run()
        {
            Room previousRoom = null;
            bool isRunning = true;
            while (isRunning)
            {
                Console.WriteLine(Player.CurrentRoom);
                if (previousRoom != Player.CurrentRoom)
                {
                    Console.WriteLine(Player.CurrentRoom.Description);
                    previousRoom = Player.CurrentRoom;
                }
                //">" to show where player input is being written
                Console.Write("> ");
                //Trim() gets rid of whitespace (spaces), LeftTrim and RightTrim are also syntax. To.Upper() makes everything uppercase so case sensitive stuff is easier to manage (took out)
                string inputString = Console.ReadLine().Trim();
                const char seperator = ' ';
                string[] commandTokens = inputString.Split(seperator);
                string verb = null, subject = null;
                //create a data type (enumeration) off of the Commands.cs file
                if (commandTokens.Length == 0)
                {
                    continue; //skip rest of while loop and go back to the top of the while loop
                }
                else if (commandTokens.Length == 1)
                {
                    verb = commandTokens[0];
                }
                else
                {
                    verb = commandTokens[0];
                    subject = commandTokens[1];
                }
                Commands command = ToCommand(verb);
                string outputString; //assigning a value here would be inefficient as we would be assigning it in all cases in switch below
                switch (command)
                {
                    case Commands.Quit:
                        isRunning = false; //this will not stop the while loop mid run, the while loop goes through the whole block before updating
                        outputString = "Thank you for playing!"; //so this will run
                        break;

                    case Commands.Look:
                        //outputString = "Player.CurrentRoom.Description";
                        outputString = $"{Player.CurrentRoom.Description}\n";
                        //Console.WriteLine(Player.CurrentRoom.Description);
                        foreach(Item item in Player.CurrentRoom.Inventory)
                        {
                            outputString += $"{item.Description}\n";
                        }
                        break;

                    case Commands.North:
                    case Commands.South:
                    case Commands.East:
                    case Commands.West:
                        Directions direction = (Directions)command;
                        if (Player.Move(direction) == false)
                        {
                            outputString = "The way is shut!";
                        }
                        else
                        {
                            outputString = $"You moved {command}."; //interpolate the string, very effective
                        }
                        break;

                    case Commands.Score:
                        outputString = $"Your score would be {Player.score}, in {Player.moveCount} move(s)";
                        break;

                    case Commands.Reward:
                        Player.score++;
                        outputString = "Score increased by 1!";
                        break;

                    case Commands.Take:
                        bool itemInWorld, itemInRoom;
                        switch (subject)
                        {
                            //case (subject == null):

                            default:
                                break;
                        }
                        outputString = "";
                        if (subject == null)
                        {
                            outputString = "This command requires a subject.";
                        }
                        else
                        {
                            foreach (Item item in World.Items)
                            {
                                if (string.Compare(subject, item.Name) == 0)
                                {
                                    itemInWorld = true;
                                    break;
                                }
                                else
                                {
                                    itemInWorld = false;
                                    //write stuff
                                }

                                if (itemInWorld == true)
                                {
                                    foreach (Item items in Player.CurrentRoom.Inventory)
                                    {
                                        if (string.Compare(subject, items.Name) == 0)
                                        {
                                            itemInRoom = true;
                                            Player.AddToInventory(items);
                                            Player.CurrentRoom.RemoveFromInventory(items);
                                            outputString = "Taken.";
                                            break;
                                        }
                                        else
                                        {
                                            itemInRoom = false;
                                            outputString = "No such thing exists.";
                                        }
                                    }
                                }

                            }
                            
                            
                        }
                        break;

                    case Commands.Drop:
                        if (subject == null)
                        {
                            outputString = "This command requires a subject.";
                        }
                        else
                        {
                            outputString = "Score increased by 1!";

                        }
                        break;

                    case Commands.Inventory:
                        outputString = "Score increased by 1!";
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
