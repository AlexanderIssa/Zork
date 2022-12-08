using System;
using System.Linq;
using Newtonsoft.Json;

namespace Zork.Common
{
    public class Game
    {
        public World World { get; }

        [JsonIgnore]
        public Player Player { get; }

        [JsonIgnore]
        public IInputService Input { get; private set; }

        [JsonIgnore]
        public IOutputService Output { get; private set; }

        [JsonIgnore]
        public bool IsRunning { get; private set; }

        public Game(World world, string startingLocation, int playerHealth)
        {
            World = world;
            Player = new Player(World, startingLocation, playerHealth);
        }

        Random rnd = new Random();

        public void Run(IInputService input, IOutputService output)
        {
            Input = input ?? throw new ArgumentNullException(nameof(input));
            Output = output ?? throw new ArgumentNullException(nameof(output));

            IsRunning = true;
            Input.InputReceived += OnInputReceived;
            Output.WriteLine("Welcome to Zork!");
            Look();
            Output.WriteLine($"\n{Player.CurrentRoom}");
        }

        public void OnInputReceived(object sender, string inputString)
        {
            char separator = ' ';
            string[] commandTokens = inputString.Split(separator);

            string verb;
            string subject = null;
            string withString = null;
            string weaponString = null;

            switch (commandTokens.Length)
            {
                case 1:
                    verb = commandTokens[0];
                    break;

                case 2:
                    verb = commandTokens[0];
                    subject = commandTokens[1];
                    break;

                case 3:
                    if (string.Compare(commandTokens[2], "with", ignoreCase: true) == 0)
                    {
                        verb = commandTokens[0];
                        subject = commandTokens[1];
                        withString = commandTokens[2];
                    }
                    else
                    {
                        verb = commandTokens[0];
                        subject = commandTokens[1];
                        break;
                    }
                    break;

                case 4:
                    if (string.Compare(commandTokens[2], "with", ignoreCase: true) == 0 && !string.IsNullOrEmpty(commandTokens[3]))
                    {
                        verb = commandTokens[0];
                        subject = commandTokens[1];
                        withString = commandTokens[2];
                        weaponString = commandTokens[3];
                    }
                    else
                    {
                        verb = commandTokens[0];
                        subject = commandTokens[1];
                        break;
                    }
                    break;

                default:
                    verb = commandTokens[0];
                    break;
            }

            Room previousRoom = Player.CurrentRoom;
            Commands command = ToCommand(verb);
            switch (command)
            {
                case Commands.Quit:
                    IsRunning = false;
                    Player.Moves++;
                    Output.WriteLine("Thank you for playing!");
                    break;

                case Commands.Look:
                    Look();
                    Player.Moves++;
                    break;

                case Commands.North:
                case Commands.South:
                case Commands.East:
                case Commands.West:
                    Directions direction = (Directions)command;
                    if (string.Compare(Player.Move(direction), "Moved", ignoreCase: true) == 0)
                    {
                        Output.WriteLine($"You moved {direction}.");
                    }
                    else if ((string.Compare(Player.Move(direction), "nMove", ignoreCase: true) == 0))
                    {
                        Output.WriteLine("The way is shut!");
                    }
                    else if (string.Compare(Player.Move(direction), "Blocked", ignoreCase: true) == 0)
                    {
                        Output.WriteLine("An enemy blocks that path.");
                    }
                        break;

                case Commands.Take:
                    if (string.IsNullOrEmpty(subject))
                    {
                        Output.WriteLine("This command requires a subject.");
                    }
                    else
                    {
                        Take(subject);
                    }
                    break;

                case Commands.Drop:
                    if (string.IsNullOrEmpty(subject))
                    {
                        Output.WriteLine("This command requires a subject.");
                    }
                    else
                    {
                        Drop(subject);
                    }
                    break;

                case Commands.Inventory:
                    if (Player.Inventory.Count() == 0)
                    {
                        Output.WriteLine("You are empty handed.");
                        Player.Moves++;
                    }
                    else
                    {
                        Output.WriteLine("You are carrying:");
                        Player.Moves++;
                        foreach (Item item in Player.Inventory)
                        {
                            Output.WriteLine(item.InventoryDescription);
                            if (item.IsWeapon)
                            {
                                Output.WriteLine($"Element: {item.Element}");
                                Output.WriteLine($"Durability: {item.Durability}");
                            }
                        }
                    }
                    break;

                case Commands.Attack:
                    if (string.IsNullOrEmpty(subject))
                    {
                        Output.WriteLine("This command requires a subject.");
                    }
                    else if (string.IsNullOrEmpty(withString))
                    {
                        Output.WriteLine("This command requires you to provide a weapon to use.");
                    }
                    else
                    {
                        Attack(subject, weaponString);
                    }
                    break;

                case Commands.Use:
                    if (subject == null)
                    {
                        Output.WriteLine("This command requires a subject.");
                    }
                    else
                    {
                        Item itemToUse = Player.Inventory.FirstOrDefault(item => string.Compare(item.Name, subject, ignoreCase: true) == 0);
                        if (itemToUse == null)
                        {
                            Output.WriteLine("You have no such item.");
                        }
                        else
                        {
                            if (itemToUse.IsUseable == false)
                            {
                                Output.WriteLine($"{itemToUse.Name} is not useable.");
                            }
                            else
                            {
                                switch (itemToUse.Name)
                                {
                                    case "Potion":
                                        Player.Health = 5;
                                        Output.WriteLine("You drank the potion. Your health went back to max!");
                                        Player.RemoveItemFromInventory(itemToUse);
                                        break;

                                    case "Kit":
                                        foreach (Item item in Player.Inventory)
                                        {
                                            if (item.IsWeapon)
                                            {
                                                item.Durability++;
                                            }
                                            else
                                            {
                                                continue;
                                            }
                                        }
                                        Output.WriteLine("You used the kit! All weapons in inventory got their durability raised by 1!");
                                        Player.RemoveItemFromInventory(itemToUse);
                                        break;

                                    case "Note":
                                        Output.WriteLine("The note reads: Thank you for playing the game! Congrats on defeating the demon!");
                                        break;

                                    default:
                                        break;
                                }
                            }
                        }
                    }
                    break;

                case Commands.Health:
                    Output.WriteLine(Player.Health);
                    break;

                case Commands.Score:
                    Output.WriteLine($"Your score would be {Player.Score}, in {Player.Moves} move(s).");
                    break;

                case Commands.Reward:
                    Player.Score++;
                    Output.WriteLine("Score increased by 1!");
                    break;

                case Commands.Damage:
                    Player.Health = Player.Health - 0.5f;
                    break;

                default:
                    Output.WriteLine("Unknown command.");
                    break;
            }

            if (ReferenceEquals(previousRoom, Player.CurrentRoom) == false)
            {
                Look();
            }

            if (Player.Health <= 0)
            {
                IsRunning = false;
            }
        }
        
        private void Look()
        {
            Output.WriteLine(Player.CurrentRoom.Description);
            foreach (Item item in Player.CurrentRoom.Inventory)
            {
                Output.WriteLine(item.LookDescription);
            }
            foreach (Enemy enemy in Player.CurrentRoom.Enemies)
            {
                Output.WriteLine(enemy.Description);
            }
        }

        private void Attack(string enemyName, string weaponName)
        {
            bool EnemyDead = false;
            // Find enemy to attack and get ref and do same with weapon
            Enemy enemyToAttack = Player.CurrentRoom.Enemies.FirstOrDefault(enemy => string.Compare(enemy.Name, enemyName, ignoreCase: true) == 0);
            Item weapon = Player.Inventory.FirstOrDefault(item => string.Compare(item.Name, weaponName, ignoreCase: true) == 0);
            // Check to see if refs are null or not
            if (enemyToAttack == null)
            {
                Output.WriteLine("There is no such enemy here.");
            }
            else
            {
                if (weapon == null)
                {
                    Output.WriteLine("You don't have any such weapon in your inventory.");
                }
                else if (weapon.IsWeapon == true)
                {
                    Player.Moves++;

                    if (rnd.Next(100) >= enemyToAttack.HitChance) // Roll a number, if that number is >= the enemy's hit chance then you successfully hit it
                    {
                        if (string.Compare(weapon.Element, enemyToAttack.Weakness, ignoreCase: true) == 0) // if the weapon has an element the enemy is weak to, do double damage
                        {
                            enemyToAttack.Health -= weapon.Attack * 2;
                            Output.WriteLine($"Hit {enemyToAttack.Name} with it's weakness! Damage Doubled!");
                            weapon.Durability--;
                        }
                        else
                        {
                            Output.WriteLine($"Successfully hit {enemyToAttack.Name}!");
                            enemyToAttack.Health -= weapon.Attack;
                            weapon.Durability--;
                        }

                        // If the enemy dies remove it from the room, add score relative to enemy's score reward, and tell the player
                        if (enemyToAttack.Health <= 0)
                        {
                            Player.CurrentRoom.RemoveEnemy(enemyToAttack);
                            Player.Score += enemyToAttack.ScoreReward;
                            Output.WriteLine($"{enemyToAttack.Name} has been slain.");
                            foreach (var room in Player.CurrentRoom.Neighbors)
                            {
                                if (room.Value.IsBlocked)
                                {
                                    room.Value.IsBlocked = false;
                                }
                                else
                                {
                                    continue;
                                }
                            }
                            EnemyDead = true;
                        }
                    }
                    else
                    {
                        Output.WriteLine("Your attack missed.");
                    }

                    if (weapon.Durability <= 0)
                    {
                        Output.WriteLine($"{weapon.Name} broke!");
                        Player.RemoveItemFromInventory(weapon);
                    }

                    if (EnemyDead == false)
                    {
                        Output.WriteLine($"{enemyToAttack.Name} attacks!");
                        if (rnd.Next(100) >= enemyToAttack.MissChance)
                        {
                            Output.WriteLine($"{enemyToAttack.Name}'s attack missed.");
                        }
                        else
                        {
                            Output.WriteLine($"{enemyToAttack.Name}'s attack hit!");
                            Player.Health -= enemyToAttack.AttackPower;
                            if (Player.Health <= 0)
                            {
                                IsRunning = false;
                            }
                        }
                    }
                }
                else
                {
                    Output.WriteLine("That item cannot be used as a weapon.");
                }
            }
        }

        private void Take(string itemName)
        {
            Item itemToTake = Player.CurrentRoom.Inventory.FirstOrDefault(item => string.Compare(item.Name, itemName, ignoreCase: true) == 0);
            if (itemToTake == null)
            {
                Output.WriteLine("You can't see any such thing.");                
            }
            else
            {
                Player.AddItemToInventory(itemToTake);
                Player.CurrentRoom.RemoveItemFromInventory(itemToTake);
                Player.Moves++;
                Output.WriteLine("Taken.");
            }
        }

        private void Drop(string itemName)
        {
            Item itemToDrop = Player.Inventory.FirstOrDefault(item => string.Compare(item.Name, itemName, ignoreCase: true) == 0);
            if (itemToDrop == null)
            {
                Output.WriteLine("You can't see any such thing.");
            }
            else
            {
                Player.CurrentRoom.AddItemToInventory(itemToDrop);
                Player.RemoveItemFromInventory(itemToDrop);
                Player.Moves++;
                Output.WriteLine("Dropped.");
            }
        }

        private static Commands ToCommand(string commandString) => Enum.TryParse(commandString, true, out Commands result) ? result : Commands.Unknown;
    }
}