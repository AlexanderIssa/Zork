using System;

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

        private World _world;
        private Room _currentRoom;
    }
}
