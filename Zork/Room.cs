using Newtonsoft.Json;
using System.Collections.Generic;

namespace Zork
{
    public class Room
    {
        public string Name { get; } //each room has it's own name and description in memory because these are NOT static

        public string Description { get; set; }
        [JsonIgnore]
        public Dictionary<Directions,Room> Neighbors { get; private set; }

        [JsonProperty]
        private Dictionary<Directions, string> NeighborNames { get; set; }
        //constructor method, must be public, doesn't have a return type, initializes the members of a class
        public Room(string name, string description, Dictionary<Directions,string> neighborNames)
        {
            Name = name;
            Description = description;
            NeighborNames = neighborNames ?? new Dictionary<Directions, string>();
        }

        public void UpdateNeighbors(World world)
        {
            Neighbors = new Dictionary<Directions, Room>();
            foreach (KeyValuePair<Directions, string> neighborName in NeighborNames)
            {
                Neighbors.Add(neighborName.Key, world.RoomsByName[neighborName.Value]);
            }

            NeighborNames = null;
        }

        //overrides the ToString() method to return the name of the room
        public override string ToString()
        {
            return Name;
        }
    }   
}
