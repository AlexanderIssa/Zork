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
            foreach (Item item in _currentRoom.Inventory)
            {

            }
        }

        public void RemoveFromInventory(Item itemToRemove)
        {

        }

        private World _world;
        private Room _currentRoom;
    }
}
