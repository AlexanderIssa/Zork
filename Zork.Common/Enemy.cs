namespace Zork.Common
{
    public class Enemy
    {
        public string Name { get; }

        public string Description { get; set; }

        public string Weakness { get; set; }
        
        public int Health
        {
            get => _health;
            set
            {
                _health = value;
            }
        }

        public Enemy (string name, string description, int health, string weakness)
        {
            Name = name;
            Description = description;
            _health = health;
            Weakness = weakness;
        }

        private int _health;
    }
}
