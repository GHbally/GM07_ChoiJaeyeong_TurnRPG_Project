
using Game.Enums;
using Game.Characters;

namespace Game.Items
{
    public abstract class Item
    {
        public string Name { get; protected set; }
        public int Price { get; protected set; }
        public string Description { get; protected set; }

        public Item(string name, int price, string description)
        {
            Name = name;
            Price = price;
            Description = description;
        }
        public virtual void PrintInfo()
        {
            Console.WriteLine($"[{Name}] 가격 : [{Price}]G");
            Console.WriteLine($"설명 : [{Description}]");
        }
    }

    public class HPPotion : Item
    {
        private int healAmount;
        public HPPotion()
            : base("체력 물약", 20, $"HP를 [50]만큼 회복합니다.")
        {
            healAmount = 50;
        }
        public void Use(Player player)
        {
            player.Heal(healAmount);
        }
    }
    public class MPPotion : Item
    {
        private int manaAmount;
        public MPPotion()
            : base("마나 물약", 20, $"MP를 [50]만큼 회복합니다.")
        {
            manaAmount = 50;
        }
        public void Use(Player player)
        {
            player.RecoverMP(manaAmount);
        }
    }
    public class EquipmentItem : Item
    {
        public ItemType ItemType { get; private set; }

        public int AtkBonus { get; private set; }
        public int DefBonus { get; private set; }
        public int MaxHPBonus { get; private set; }

        public EquipmentItem(string name, int price, string description, ItemType itemType,
            int atkBonus, int defBonus, int maxHPBonus)
            : base(name, price, description)
        {
            ItemType = itemType;
            AtkBonus = atkBonus;
            DefBonus = defBonus;
            MaxHPBonus = maxHPBonus;
        }


        public override void PrintInfo()
        {
            base.PrintInfo();
            Console.WriteLine($"종류 : [{ItemType}]");
            Console.WriteLine($"ATK +[{AtkBonus}] | DEF +[{DefBonus}], HP +[{MaxHPBonus}]");
        }
    }
}
