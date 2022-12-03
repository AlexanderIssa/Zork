using System;
using System.Collections.Generic;

namespace Zork.Common
{
    public class Player
    {
        public event EventHandler<Room> LocationChanged;

        public event EventHandler<int> MovesChanged;

        public event EventHandler<int> ScoreChanged;

        public event EventHandler<float> HealthChanged;
        public Room CurrentRoom
        {
            get => _currentRoom;
            set
            {
                if (_currentRoom != value)
                {
                    _currentRoom = value;
                    LocationChanged?.Invoke(this, _currentRoom);
                }
            }
            
        }

        public int Moves
        {
            get => _moves;
            set
            {
                _moves = value;
                MovesChanged?.Invoke(this, _moves);
            }
        }

        public int Score
        {
            get => _score;
            set
            {
                _score = value;
                ScoreChanged?.Invoke(this, _score);
            }
        }

        public float Health
        {
            get => _health;
            set
            {
                _health = value;
                HealthChanged?.Invoke(this, _health);
            }
        }

        public IEnumerable<Item> Inventory => _inventory;

        public Player(World world, string startingLocation, int health)
        {
            _world = world;

            if (_world.RoomsByName.TryGetValue(startingLocation, out _currentRoom) == false)
            {
                throw new Exception($"Invalid starting location: {startingLocation}");
            }

            Health = health;

            _inventory = new List<Item>();
        }

        public string Move(Directions direction)
        {
            string returnString = "";
            bool didMove = _currentRoom.Neighbors.TryGetValue(direction, out Room neighbor);
            if (didMove)
            {
                if (neighbor.IsBlocked == true)
                {
                    Moves++;
                    returnString = "Blocked";
                    if (_currentRoom.HasEnemy == false)
                    {
                        neighbor.IsBlocked = false;
                        returnString = "Moved";
                        CurrentRoom = neighbor;
                    }
                }
                else
                {
                    returnString = "Moved";
                    CurrentRoom = neighbor;
                    Moves++;
                }

            }
            else
            {
                returnString = "nMove";
            }

            return returnString;
        }

        public void AddItemToInventory(Item itemToAdd)
        {
            if (_inventory.Contains(itemToAdd))
            {
                throw new Exception($"Item {itemToAdd} already exists in inventory.");
            }

            _inventory.Add(itemToAdd);
        }

        public void RemoveItemFromInventory(Item itemToRemove)
        {
            if (_inventory.Remove(itemToRemove) == false)
            {
                throw new Exception("Could not remove item from inventory.");
            }
        }

        private readonly World _world;
        private Room _currentRoom;
        private readonly List<Item> _inventory;
        private int _moves, _score;
        private float _health;
    }
}
