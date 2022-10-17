using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Zork
{
    public class World
    {
        public Room[] Rooms { get; }
        public Dictionary<string, Room> RoomsByName { get; }

        public World(Room[] rooms)
        {
            Rooms = rooms;
            RoomsByName = new Dictionary<string, Room>(StringComparer.OrdinalIgnoreCase);
            foreach (Room room in rooms)
            {
                RoomsByName.Add(room.Name, room);
            }
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext streamingContext)
        {
            foreach (Room room in Rooms)
            {
                room.UpdateNeighbors(this);
            }
        }
    }
}
