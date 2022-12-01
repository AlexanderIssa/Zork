namespace Zork.Common
{
    public class Item
    {
        public string Name { get; }

        public string LookDescription { get; }

        public string InventoryDescription { get; }

        public bool IsWeapon { get; }

        public string Element { get; set; }

        public Item(string name, string lookDescription, string inventoryDescription, bool isWeapon, string element)
        {
            Name = name;
            LookDescription = lookDescription;
            InventoryDescription = inventoryDescription;
            IsWeapon = isWeapon;
            Element = element;
        }

        public override string ToString() => Name;
    }
}