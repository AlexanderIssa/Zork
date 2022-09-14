namespace Zork
{
    internal class Room
    {
        public string Name { get; set; } //each room has it's own name and description in memory because these are NOT static

        public string Description { get; set; }

        //constructor method, must be public, doesn't have a return type, initializes the members of a class
        public Room(string name, string description = null)
        {
            Name = name;
            Description = description;
        }

        //overrides the ToString() method to return the name of the room
        public override string ToString()
        {
            return Name;
        }
    }   
}
