using System;
using System.Collections.Generic;

namespace Zork
{
    public class Player
    {
        public int score, moveCount;
        //private is only accessible in this class
        public Room CurrentRoom
        {
            get => _currentRoom;
            set => _currentRoom = value;
        }

        public List<Item> Inventory { get; }

        public Player(World world, string startingLocation)
        {
            _world = world;

            if (_world.RoomsByName.TryGetValue(startingLocation, out _currentRoom) == false)
            {
                throw new Exception($"Invalid starting location: {startingLocation}"); //if above return didn't go throw, error case
            }
            Inventory = new List<Item>();
        }

        public bool Move(Directions direction)
        {
            bool didMove = _currentRoom.Neighbors.TryGetValue(direction, out Room neighbor);
            if (didMove)
            {
                CurrentRoom = neighbor;
                moveCount++;
            }

            return didMove;
        } 

        public void AddToInventory (Item itemToAdd)
        {
            Inventory.Add(itemToAdd);
        }

        public void RemoveFromInventory(Item itemToRemove)
        {
            Inventory.Remove(itemToRemove);
        }

        private World _world;
        private Room _currentRoom;
    }
}
