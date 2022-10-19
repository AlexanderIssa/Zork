using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Zork
{
    //contains all the rooms in the game
    public class World
    {
        public Room[] Rooms { get; }

        [JsonIgnore]
        public Dictionary<string, Room> RoomsByName { get; }

        public Item[] Items { get; }

        [JsonIgnore]
        public Dictionary<string, Item> ItemsByName { get; }

        public World(Room[] rooms, Item[] items)
        {
            Rooms = rooms;
            RoomsByName = new Dictionary<string, Room>(StringComparer.OrdinalIgnoreCase);
            foreach (Room room in rooms)
            {
                RoomsByName.Add(room.Name, room);
            }
            Items = items;
            ItemsByName = new Dictionary<string, Item>(StringComparer.OrdinalIgnoreCase);
            foreach (Item item in Items)
            {
                ItemsByName.Add(item.Name, item);
            }
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext streamingContext)
        {
            foreach (Room room in Rooms)
            {
                room.UpdateNeighbors(this);
                room.UpdateInvetory(this);
            }
        }
    }
}
