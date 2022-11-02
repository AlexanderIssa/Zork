namespace Zork.Common
{
    public class Item
    {
        public string Name { get; }
        public string Description { get; }
        public string InvDescription { get; } //Description when the item is in the player's Inventory

        public Item(string name, string description, string invDescription)
        {
            Name = name;
            Description = description;
            InvDescription = invDescription;
        }
    }
}