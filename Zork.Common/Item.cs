namespace Zork.Common
{
    public class Item
    {
        public string Name { get; }

        public string LookDescription { get; }

        public string InventoryDescription { get; }

        public bool IsWeapon { get; }

        public string Element { get; set; }

        public int Durability { get; set; }

        public int Attack { get; set; }

        public bool IsUseable { get; }

        public Item(string name, string lookDescription, string inventoryDescription, bool isWeapon, string element, int durability, int attack, bool isUseable)
        {
            Name = name;
            LookDescription = lookDescription;
            InventoryDescription = inventoryDescription;
            IsWeapon = isWeapon;
            Element = element;
            Durability = durability;
            Attack = attack;
            IsUseable = isUseable;
        }

        public override string ToString() => Name;
    }
}