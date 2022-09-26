namespace Zork
{
    internal class Room
    {
        public string Name { get; set; }

        public string Description { get; set; }

        //constructor method, must be public, doesn't have a return type, initializes the members of a class
        public Room(string name)
        {
            Name = name;
        }

        //overrides the ToString() method to return the name of the room
        public override string ToString()
        {
            return Name;
        }
    }   
}
