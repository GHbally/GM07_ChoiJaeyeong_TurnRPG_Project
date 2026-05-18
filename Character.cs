
using Game.Enums;
using Game.Items;
using Game.Skills;
using static System.Net.Mime.MediaTypeNames;

namespace Game.Characters
{
    public abstract class Character
    {
        protected static Random random = new Random();

        public string Name { get; protected set; }
        public int Hp { get; protected set; }
        public int MaxHp {  get; protected set; }

        public int AttackPower {  get; protected set; }
        public int Defence { get; protected set; }
        public int AvoidChance { get; protected set; }

        public int CriticalChance {  get; protected set; }
        public float CriticalDamage { get; protected set; }

        public bool IsAlive { get; protected set; }
        public Character(string name, int hp, int attackPower, int defence, int avoidChance, int criticalChace, float criticalDamage)
        {
            Name = name;
            MaxHp = hp;
            Hp = hp;
            AttackPower = attackPower;
            Defence = defence;
            IsAlive = true;
            CriticalChance = criticalChace;
            CriticalDamage = criticalDamage;
            AvoidChance = avoidChance;
        }
        public virtual void PrintCharacterInfo()
        {
            Console.WriteLine($"[{Name}]");
            Console.WriteLine($"HP : [{Hp}/{MaxHp}] | ATK : [{AttackPower}] | DEF : [{Defence}]");
        }

        public virtual void Attack(Character target)
        {
            if (!IsAlive)
            {
                Console.WriteLine($"[{Name}]은(는) 이미 쓰러져서 공격할 수 없습니다.");
                return;
            }

            int damage = CalculateCriDamage( AttackPower);
       
            Console.WriteLine($"[{Name}]이(가) [{target.Name}]을(를) 공격합니다!");
            target.TakeDamage(damage);
        }
        protected bool IsCritical()
        {
            int value = random.Next(1, 101);
            return value <= CriticalChance;
        }
        public int CalculateCriDamage(int baseDamage)
        {
            int damage = baseDamage;

            if (IsCritical())
            {
                damage = (int)(damage * CriticalDamage);
                Console.WriteLine("[치명타 발동!]");
            }
            return damage;
        }
        protected bool IsAvoid()
        {
            int value = random.Next(1, 101);
            return value <= AvoidChance;
        }

        public virtual void TakeDamage(int damage)
        {
            if (!IsAlive)
            {
                return;
            }

            if (IsAvoid())
            {
                Console.WriteLine($"[{Name}]은(는) 공격을 회피했습니다!");
                return;
            }

            int finalDamage = Math.Max(1, damage - Defence);

            Hp -= finalDamage;
            if (Hp < 0) { Hp = 0; }

            Console.WriteLine($"[{Name}]은(는) [{finalDamage}]의 피해를 받았습니다.");
            Console.WriteLine($"[{Name}]의 남은 체력 : [{Hp}/{MaxHp}]");

            if(Hp <= 0)
            {
                Die();
            }
        }
        public virtual void FixedDamage(int fixedDamage)
        {
            if (!IsAlive)
            {
                return;
            }
            if (IsAvoid())
            {
                Console.WriteLine($"[{Name}]은(는) 공격을 회피했습니다!");
                return;
            }
            
            Hp -= fixedDamage;
            if (Hp < 0) { Hp = 0; }

            Console.WriteLine($"[{Name}]은(는) [{fixedDamage}]의 피해를 받았습니다.");
            Console.WriteLine($"[{Name}]의 남은 체력 : [{Hp}/{MaxHp}]");

            if (Hp <= 0)
            {
                Die();
            }
        }
        public virtual void Heal(int amount)
        {
            if (MaxHp == Hp)
            {
                Console.WriteLine("현재 HP가 꽉 찼습니다. 더 이상 체력을 회복 할 수 없습니다.");
                return;
            }


            int tempHp = Hp;        
            Hp += amount;

            if (Hp >= MaxHp) { Hp = MaxHp; }

            int healAmount = Hp - tempHp;


            Console.WriteLine($"[{Name}]가 [{healAmount}]만큼 회복했습니다.");
            Console.WriteLine($"[{Name}]의 현재 체력 : [{Hp}/{MaxHp}]");
        }
        protected virtual void Die()
        {
            IsAlive = false;
            Console.WriteLine($"[{Name}]은(는) 쓰러졌습니다.");
        }
        public virtual void AddDefence(int amount)
        {
            Defence += amount;

            if (Defence < 0)
            {
                Defence = 0;
            }

            Console.WriteLine($"[{Name}]의 방어력이 [{Defence}]이 되었습니다.");
        }

        public virtual void MultiplyDefence(float multiplier)
        {
            Defence = (int)(Defence * multiplier);

            if (Defence < 0)
            {
                Defence = 0;
            }

            Console.WriteLine($"[{Name}]의 방어력이 [{Defence}]이 되었습니다.");
        }

        public virtual void AddAvoidChance(int amount)
        {
            AvoidChance += amount;

            if (AvoidChance < 0)
            {
                AvoidChance = 0;
            }

            if (AvoidChance > 100)
            {
                AvoidChance = 100;
            }

            Console.WriteLine($"[{Name}]의 회피율이 [{AvoidChance}%]가 되었습니다.");
        }
    }

    // 플레이어
    public class Player : Character
    {
        public JobType JobType { get; private set; }
        public int MP { get; private set; }
        public int MaxMP {  get; private set; }
        private int gold = 0;
        private int exp = 0;
        public int MaxExp { get; private set; }
        public int Level {  get; private set; }

        private EquipmentItem weapon;
        private EquipmentItem armor;
        private EquipmentItem acc;

        List<Skill> skills;
        List<Item> inventory;

        public int Exp
        {
            get { return exp; }
            private set
            {
                exp = Math.Max(0, value);
            }
        }
        public int Gold 
        {
            get { return gold; }
            private set
            {
                gold = Math.Max(0, value);
            }
        }
        
        public Player(string name,JobType jobType, int hp,int mp,int attackPower, int defence, int avoidChance) 
            : base(name, hp, attackPower, defence, avoidChance, 50,2.0f)
        {
            MP = mp;
            MaxMP = MP;
            Gold = 0;
            Level = 1;
            Exp = 0;
            MaxExp = 10;

            skills = new List<Skill>();
            inventory = new List<Item>();

            JobType = jobType;
            if(JobType == JobType.Warrior)
            {
                AddSkill(new ShieldBashSkill());
                AddSkill(new PowerStrikeSkill());
                AddSkill(new ShieldUpSkill());
            }
            else if (JobType == JobType.Mage)
            {
                AddSkill(new FireballSkill());
                AddSkill(new MagicArrowSkill());
                AddSkill(new HealSkill());
            }
            else if (JobType == JobType.Rogue)
            {
                AddSkill(new ExposeWeaknessSkill());
                AddSkill(new SmokeSkill());
                AddSkill(new FanOfKnivesSkill());
            }
        }
        public override void PrintCharacterInfo()
        {
            Console.WriteLine($"[{Name}]");
            Console.WriteLine($"HP : [{Hp}/{MaxHp}] | MP : [{MP}/{MaxMP}] | ATK : [{AttackPower}] | DEF : [{Defence}]");
            Console.WriteLine($"Level : [{Level}] | Gold : [{Gold}]");
            Console.WriteLine($"EXP : [{Exp}/{MaxExp}]");
        }

        public void GainExp(int amount)
        {
            if (amount < 0) return;

            Exp += amount;
            Console.WriteLine($"[{amount}] 경험치를 획득했습니다.");
            
            while (Exp >= MaxExp) { LevelUP(); }
        }
        public void GainGold(int amount)
        {
            if (amount < 0) return;

            Gold += amount;
            Console.WriteLine($"[{amount}] Gold 를 획득했습니다.");
        }
        public void LevelUP()
        {
            Level++;
            Exp -= MaxExp;

            MaxHp += 30;
            Hp = MaxHp;
            MaxMP += 20;
            MP = MaxMP;
            AttackPower += 5;
            Defence += 2;
            MaxExp += 10;

            Console.WriteLine($"레벨 업! 현재 레벨 : [{Level}]");
        }

        public void AddSkill(Skill skill)
        {
            if (skill == null)
            {
                return;
            }
            skills.Add(skill);
            Console.WriteLine($"[{skill.Name}] 스킬을 배웠습니다.");
        }
        public void ShowSkills()
        {
            if(skills.Count == 0)
            {
                Console.WriteLine("보유한 스킬이 없습니다.");
                return;
            }
            Console.WriteLine("---스킬 목록---");

            for(int i=0;i<skills.Count;i++)
            {
                Console.WriteLine($"{i+1}. [{skills[i].Name}]");
            }
        }
        public void UseSkill(int skillNumber, Character target)
        {
            int index = skillNumber - 1;
            if(index < 0 || index >= skills.Count)
            {
                Console.WriteLine("잘못된 스킬 번호입니다.");
                return;
            }
            skills[index].Use(this, target);
        }
        public bool UseMP(int amount)
        {
            if (amount <= 0)
            {
                return true;
            }
            if (MP < amount)
            {
                Console.WriteLine("MP가 부족합니다.");
                return false;
            }

            MP -= amount;
            Console.WriteLine($"MP를 [{amount}] 사용했습니다. 현재 MP : [{MP}/{MaxMP}]");
            return true;
        }

        public void RecoverMP(int amount)
        {
            if (amount <= 0)
            {
                return;
            }

            MP += amount;

            if(MP > MaxMP)
            {
                MP = MaxMP;
            }
            Console.WriteLine($"MP를 [{amount} 만큼 회복했습니다. 현재 MP : [{MP}/{MaxMP}]");
        }

        public void AddItem(Item item)
        {
            if (item == null)
            {
                return;
            }
            inventory.Add(item);
            
        }
        public void ShowInventory()
        {
            Console.Clear();
            Console.WriteLine("---인벤토리 목록---");

            if (inventory.Count <= 0)
            {
                Console.WriteLine("보유한 아이템이 없습니다.");
                return;
            }

            for (int i = 0; i < inventory.Count; i++)
            {
                Item item = inventory[i];

                string equippedText = "";

                if (item == weapon)
                {
                    equippedText = " [장착 중 - 무기]";
                }
                else if (item == armor)
                {
                    equippedText = " [장착 중 - 방어구]";
                }
                else if (item == acc)
                {
                    equippedText = " [장착 중 - 악세서리]";
                }

                Console.WriteLine($"{i + 1}. [{item.Name}]{equippedText}");
            }
        }
        public void OpenInventory()
        {
            while (true)
            {
                ShowInventory();
                ShowEquipment();


                Console.WriteLine();
                Console.WriteLine("1. 아이템 장착/해제 및 사용");
                Console.WriteLine("2. 나가기");

                Console.Write("무엇을 할까? : ");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        EquipOrUseItem();
                        break;
                    case "2":
                        return;
                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        break;
                }
            }
        }
        private void EquipOrUseItem()
        {
            if (inventory.Count == 0)
            {
                Console.WriteLine("보유한 아이템이 없습니다.");
                return;
            }
            Console.Write("사용/장착할 아이템 번호 : ");
            string input = Console.ReadLine();

            if(!int.TryParse(input, out int itemNumber))
            {
                Console.WriteLine("숫자를 입력해주세요.");
                return;
            }

            int index = itemNumber - 1;
            if(index < 0||index >= inventory.Count)
            {
                Console.WriteLine("잘못된 아이템 번호입니다.");
                return;
            }

            Item item = inventory[index];

            if(item is HPPotion hPPotion)
            {
                hPPotion.Use(this);
                inventory.RemoveAt(index);
            }
            else if(item is MPPotion mPPotion)
            {
                mPPotion.Use(this);
                inventory.RemoveAt(index);
            }
            else if(item is EquipmentItem equipment)
            {
                EquipItem(equipment);
            }
            else
            {
                Console.WriteLine("사용할 수 없는 아이템입니다.");
            }
        }


        public Skill GetSkill(int skillNumber)
        {
            int index = skillNumber - 1;
            if (index <  0 || index >= skills.Count)
            {
                return null;
            }
            return skills[index];
        }

        public void EquipItem(EquipmentItem equipment)
        {
            if(equipment == null)
            {
                Console.WriteLine("장착할 장비가 없습니다.");
                return;
            }

            if(equipment == weapon)
            {
                UnequipItem(weapon);
                weapon = null;
                Console.WriteLine($"[{equipment.Name}]을(를) 해제했습니다.");
                return;
            }
            if(equipment == armor)
            {
                UnequipItem(armor);
                armor = null;
                Console.WriteLine($"[{equipment.Name}]을(를) 해제했습니다.");
                return;
            }
            if(equipment == acc)
            {
                UnequipItem(acc);
                acc = null;
                Console.WriteLine($"[{equipment.Name}]을(를) 해제했습니다.");
                return;
            }

            switch (equipment.ItemType)
            {
                case ItemType.Weapon:
                    UnequipItem(weapon);
                    weapon = equipment;
                    break;
                case ItemType.Armor:
                    UnequipItem(armor);
                    armor = equipment;
                    break;
                case ItemType.Acc:
                    UnequipItem(acc);
                    acc = equipment;
                    break;
            }
            ApplyEquipmentStatus(equipment);
            Console.WriteLine($"[{equipment.Name}]을 장착했습니다.");
        }
        private void ApplyEquipmentStatus(EquipmentItem equipment)
        {
            AttackPower += equipment.AtkBonus;
            Defence += equipment.DefBonus;
            MaxHp += equipment.MaxHPBonus;
            Hp+= equipment.MaxHPBonus;

            if (Hp> MaxHp)
            {
                Hp = MaxHp;
            }
            if (MP > MaxMP)
            {
                MP= MaxMP;
            }
        }
        private void UnequipItem(EquipmentItem equipment)
        {
            if( equipment == null)
            {
                return;
            }

            AttackPower -= equipment.AtkBonus;
            Defence -= equipment.DefBonus;
            MaxHp -= equipment.MaxHPBonus;

            if (Hp > MaxHp)
            {
                Hp = MaxHp;
            }
            if (MP > MaxMP)
            {
                MP = MaxMP;
            }
        }
        public void ShowEquipment()
        {
            Console.WriteLine("--- 장비 목록 ---");
            Console.WriteLine($"무기 : [{(weapon == null ? "없음" : weapon.Name)}]");
            Console.WriteLine($"방어구 : [{(armor == null ? "없음" : armor.Name)}]");
            Console.WriteLine($"악세서리 : [{(acc == null ? "없음" : acc.Name)}]");
        }
        public bool SpendGold(int amount)
        {
            if( amount <= 0)
            {
                return true;
            }
            if(Gold < amount)
            {
                Console.WriteLine("골드가 부족합니다.");
                return false;
            }

            Gold -= amount;
            Console.WriteLine($"[{amount}]G 를 사용했습니다. 남은 Gold : [{Gold}]");
            return true;
        }

    } // End Of Player

    // 몬스터
    public class Monster : Character
    {
        public int RewardExp { get; private set; }
        public int RewardGold { get; private set; }

        public Monster(string name,int hp, int attackPower,int defence, int rewardExp, int rewardGold)
            :base(name,hp,attackPower, defence,0, 5, 1.5f)
        {
            RewardExp = rewardExp;
            RewardGold = rewardGold;
        }
        public void GiveReward(Player player)
        {
            Console.WriteLine($"[{Name}] 처치 보상을 획득했습니다!");

            player.GainExp(RewardExp);
            player.GainGold(RewardGold);
        }
        public virtual void TakeTurn(Player player)
        {
            Attack(player);
        }
    }
}
