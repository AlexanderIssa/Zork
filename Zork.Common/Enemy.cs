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

        public int HitChance { get; set; }

        public int MissChance { get; set; }

        public float AttackPower { get; set; }

        public int ScoreReward { get; set; }

        public Enemy (string name, string description, int health, string weakness, int hitChance, int missChance, float attackPower, int scoreReward)
        {
            Name = name;
            Description = description;
            _health = health;
            Weakness = weakness;
            HitChance = hitChance;
            MissChance = missChance;
            AttackPower = attackPower;
            ScoreReward = scoreReward;
        }

        private int _health;
    }
}
