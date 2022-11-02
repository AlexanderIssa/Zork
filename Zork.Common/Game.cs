using System;

namespace Zork.Common
{
    public class Game
    {
        public World World { get; }

        public Player Player { get; }

        public IOutputService Output { get; private set; }

        public Game(World world, string startingLocation)
        {
            World = world;
            Player = new Player(World, startingLocation);
        }

        public void Run(IOutputService output)
        {
            Output = output;

            Room previousRoom = null;
            bool isRunning = true;
            while (isRunning)
            {
                Output.WriteLine(Player.CurrentRoom);
                if (previousRoom != Player.CurrentRoom)
                {
                    Output.WriteLine(Player.CurrentRoom.Description);
                    previousRoom = Player.CurrentRoom;
                }

                Output.Write("> ");

                string inputString = Console.ReadLine().Trim();
                char  separator = ' ';
                string[] commandTokens = inputString.Split(separator);
                
                string verb = null;
                string subject = null;
                if (commandTokens.Length == 0)
                {
                    continue;
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
                string outputString;
                switch (command)
                {
                    case Commands.Quit:
                        isRunning = false;
                        outputString = "Thank you for playing!";
                        break;

                    case Commands.Look:
                        outputString = $"{Player.CurrentRoom.Description}\n";
                        foreach (Item item in Player.CurrentRoom.Inventory)
                        {
                            outputString += $"{item.Description}\n";
                        }
                        break;

                    case Commands.North:
                    case Commands.South:
                    case Commands.East:
                    case Commands.West:
                        Directions direction = (Directions)command;
                        if (Player.Move(direction))
                        {
                            outputString = $"You moved {direction}.";
                        }
                        else
                        {
                            outputString = "The way is shut!";
                        }
                        break;

                    case Commands.Take:
                        outputString = "";
                        if (subject == null)
                        {
                            outputString = "This command requires a subject.";
                        }
                        else
                        {
                            StringComparer comparer = StringComparer.OrdinalIgnoreCase; //string comparer that ignores case sensitivity
                            if (Player.Inventory.Count > 0)
                            {
                                foreach (Item item in Player.Inventory)
                                {
                                    if (comparer.Compare(subject, item.Name) == 0) //Item name matches an item's name in the Player's inventory
                                    {
                                        outputString = "You already have that in your Inventory.";
                                        break;
                                    }
                                    else
                                    {
                                        if (Player.CurrentRoom.Inventory.Count > 0) //if the room has any items at all
                                        {
                                            foreach (Item items in Player.CurrentRoom.Inventory)
                                            {
                                                if (comparer.Compare(subject, items.Name) == 0) //Item name matches Item's name in Room's inventory
                                                {
                                                    //Add item to Player's Inventory then remove it from the Room's inventory
                                                    Player.AddToInventory(items);
                                                    Player.CurrentRoom.RemoveFromInventory(items);
                                                    outputString = "Taken.";
                                                    break;
                                                }
                                                else
                                                {
                                                    outputString = "You can't see any such thing.";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            outputString = "You can't see any such thing.";
                                        }
                                        break;
                                    }
                                }

                            }
                            else
                            {
                                if (Player.CurrentRoom.Inventory.Count > 0) //if the room has any items at all
                                {
                                    foreach (Item items in Player.CurrentRoom.Inventory)
                                    {
                                        if (comparer.Compare(subject, items.Name) == 0) //Item name matches Item's name in Room's inventory
                                        {
                                            //Add item to Player's Inventory then remove it from the Room's inventory
                                            Player.AddToInventory(items);
                                            Player.CurrentRoom.RemoveFromInventory(items);
                                            outputString = "Taken.";
                                            break;
                                        }
                                        else
                                        {
                                            outputString = "You can't see any such thing.";
                                        }
                                    }
                                }
                                else
                                {
                                    outputString = "You can't see any such thing.";
                                }
                                break;
                            }
                        }
                        break;

                    case Commands.Drop:
                        outputString = "";
                        if (subject == null)
                        {
                            outputString = "This command requires a subject.";
                        }
                        else
                        {
                            StringComparer comparer = StringComparer.OrdinalIgnoreCase; //string comparer that ignores case sensitivity
                            if (Player.Inventory.Count > 0) //if the player has any items at all
                            { 
                                foreach (Item items in Player.Inventory)
                                {
                                    if (comparer.Compare(subject, items.Name) == 0) //Item name matches item in Player Inventory
                                    {
                                        //Add to Room inventory and remove from Player Inventory
                                        Player.CurrentRoom.AddToInventory(items);
                                        Player.RemoveFromInventory(items);
                                        outputString = "Dropped.";
                                        break;
                                    }
                                    else
                                    {
                                        outputString = "You don't have any such item in your inventory.";
                                    }
                                }
                            }
                            else
                            {
                                outputString = "You don't have any such item in your inventory.";
                            }
                            break;
                        }
                        break;

                    case Commands.Inventory:
                        outputString = "";
                        if (Player.Inventory == null || Player.Inventory.Count <= 0) //if the player's inventory is null or empty
                        {
                            outputString = "You are empty handed";
                            break;
                        }
                        else
                        {
                            foreach (Item item in Player.Inventory)
                            {
                                outputString += $"{item.InvDescription}\n";
                            }
                        }
                        break;

                    default:
                        outputString = "Unknown command.";
                        break;
                }

                Output.WriteLine(outputString);
            }
        }

        private static Commands ToCommand(string commandString) => Enum.TryParse(commandString, true, out Commands result) ? result : Commands.Unknown;
    }
}
