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
        [JsonIgnore]
        public List<Item> Inventory { get; private set; }
        [JsonProperty("Inventory")] //lets you use "Inventory" as key in json file
        private string[] InventoryNames { get; set; }

        [JsonProperty(PropertyName = "Neighbors", Order = 3)]
        private Dictionary<Directions, string> NeighborNames { get; set; }
        //constructor method, must be public, doesn't have a return type, initializes the members of a class
        public Room(string name, string description, Dictionary<Directions,string> neighborNames,string[] inventoryNames)
        {
            Name = name;
            Description = description;
            NeighborNames = neighborNames ?? new Dictionary<Directions, string>();
            InventoryNames = inventoryNames ?? new string[0];
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

        public void UpdateInvetory(World world)
        {
            Inventory = new List<Item>();
            foreach (var inventoryName in InventoryNames)
            {
                Inventory.Add(world.ItemsByName[inventoryName]);
            }
            InventoryNames = null;
        }

        //overrides the ToString() method to return the name of the room
        public override string ToString()
        {
            return Name;
        }

        public void AddToInventory(Item itemToAdd)
        {
            Inventory.Add(itemToAdd);
        }

        public void RemoveFromInventory(Item itemToRemove)
        {
            Inventory.Remove(itemToRemove);
        }

    }   
}
