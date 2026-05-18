using Game.Characters;
using Game.Enums;

namespace Game.Skills
{
    public abstract class Skill
    {
        public string Name { get; protected set; }
        public int MpCost { get; protected set; }
        public string Description {get; protected set; } 
        public Skill(string name, int mpcost,string description)
        {
            Name = name;
            MpCost = mpcost;
            Description = description;
        }

        public virtual void Use(Player player, Character target)
        {
            Console.WriteLine($"[{Name}]은(는) 단일 대상 스킬이 아닙니다.");
        }

        public virtual void UseArea(Player player, List<Monster> targets)
        {
            Console.WriteLine($"[{Name}]은(는) 광역 스킬이 아닙니다.");
        }    
    }
    
    public class ShieldBashSkill : Skill
    {
        private int defenceMultiple;

        public ShieldBashSkill()
            : base("방패 밀치기", 5, "방어력의 2배 피해를 줍니다.(치명타 적용 x)")
        {
            defenceMultiple = 2;
        }
        public override void Use(Player player, Character target)
        {
            if (target == null)
            {
                Console.WriteLine("대상이 없습니다.");
                return;
            }
            if (!player.UseMP(MpCost))
            {
                
                return;
            }

            int damage = player.Defence * defenceMultiple;

            Console.WriteLine($"[{player.Name}]이(가) [{Name}]을(를) 사용했습니다!");
            target.TakeDamage(damage);
        }
    }
    public class PowerStrikeSkill : Skill
    {
        private float damageMultipl;
        public PowerStrikeSkill()
            : base("파워 스트라이크", 5, "현재 공격력의 2배 피해를 줍니다.(치명타 적용 X)")
        {
            damageMultipl = 2.0f;
        }
        public override void Use(Player player, Character target)
        {
            if (target == null)
            {
                Console.WriteLine("대상이 없습니다.");
                return;
            }
            if (!player.UseMP(MpCost))
            {
                return;
            }

            int damage = (int)(player.AttackPower * damageMultipl);

            Console.WriteLine($"[{player.Name}]이(가) [{Name}]을(를) 사용했습니다!");
            target.TakeDamage(damage);
        }
    }
    public class ShieldUpSkill : Skill
    {
        int defenceBonus;
        public ShieldUpSkill()
            : base("방패 올리기", 5, "대상을 일반 공격한 후 전투 동안 방어력을 10 올린다.(치명타 적용 O)")
        {
            defenceBonus = 10;
        }

        public override void Use(Player player, Character target)
        {
            if (target == null)
            {
                Console.WriteLine("대상이 없습니다.");
                return;
            }
            if (!player.UseMP(MpCost))
            {
                return;
            }
            Console.WriteLine($"[{player.Name}]이(가) [{Name}]을(를) 사용했습니다!");
            player.Attack(target);
            player.AddDefence(defenceBonus);
        }
    }
    
    public class FireballSkill : Skill
    {
        private int damage;

        public FireballSkill()
            : base("화염구", 20, "모든 적에게 광역 화염 피해를 줍니다.(치명타 적용 X)")
        {
            damage = 20;
        }
        public override void UseArea(Player player, List<Monster> targets)
        {
            if (!player.UseMP(MpCost))
            {
                return;
            }
            if (targets == null || targets.Count == 0)
            {
                Console.WriteLine("공격할 대상이 없습니다.");
                return;
            }
            Console.WriteLine($"[{player.Name}]이(가) [{Name}]을(를) 사용했습니다!");

            foreach (Monster monster in targets)
            {
                if(monster.IsAlive)
                {
                    Console.WriteLine($"[{monster.Name}]에게 화염 피해!");
                    monster.TakeDamage(damage);
                }
            }
        }
    }
    public class MagicArrowSkill : Skill
    {
        private int FixedDamage;

        public MagicArrowSkill()
            : base("매직 애로우", 20, "고정 피해를 줍니다.(치명타 적용 X)")
        {
            FixedDamage = 40;
        }

        public override void Use(Player player, Character target)
        {
            if (target == null)
            {
                Console.WriteLine("대상이 없습니다.");
                return;
            }
            if (!player.UseMP(MpCost))
            {
                return;
            }

            Console.WriteLine($"[{player.Name}]이(가) [{Name}]을(를) 사용했습니다!");
            target.FixedDamage(FixedDamage);
        }
    }
    public class HealSkill : Skill
    {
        private int healAmount;

        public HealSkill()
            : base("힐링", 10, "자신의 체력을 회복합니다.")
        {
            healAmount = 50;
        }
        public override void Use(Player player, Character target)
        {
            if (!player.UseMP(MpCost))
            {
                return;
            }

            Console.WriteLine($"[{player.Name}]이(가) [{Name}]을(를) 사용했습니다!");
            player.Heal(healAmount);
        }
    }
    public class ExposeWeaknessSkill : Skill
    {
        public ExposeWeaknessSkill()
            : base("약점 노출", 5, "대상의 방어력을 50% 감소 시킨 후 일반 공격 합니다.(치명타 적용 O)")
        {
        }
        public override void Use(Player player, Character target)
        {
            if(target == null)
            {
                Console.WriteLine("대상이 없습니다.");
                return;
            }
            if (!player.UseMP(MpCost))
            {
                return;
            }
            Console.WriteLine($"[{player.Name}]이(가) [{Name}]을(를) 사용했습니다!");

            target.MultiplyDefence(0.5f);
            player.Attack(target);
        }
    }
    public class SmokeSkill : Skill
    {
        private int avoidBonous;

        public SmokeSkill()
            : base("연막탄", 5, "이번 전투 동안 회피율이 30 증가합니다.")
        {
            avoidBonous = 30;
        }
        public override void Use(Player player, Character target)
        {
            if (!player.UseMP(MpCost))
            {
                return;
            }
            Console.WriteLine($"[{player.Name}]이(가) [{Name}]을(를) 사용했습니다!");
            player.AddAvoidChance(avoidBonous);
            
        }
    }
    public class FanOfKnivesSkill : Skill
    {
        float damageMultiple;
        public FanOfKnivesSkill()
            : base("단검 난사", 5, "모든 적에게 단검을 날려 피해를 입힙니다.(치명타 적용 O)")
        {
            damageMultiple = 0.7f;
        }
        public override void UseArea(Player player, List<Monster> targets)
        {
            if (!player.UseMP(MpCost))
            {
                return;
            }
            if (targets == null || targets.Count == 0)
            {
                Console.WriteLine("공격할 대상이 없습니다.");
                return;
            }

            Console.WriteLine($"[{player.Name}]이(가) [{Name}]을(를) 사용했습니다!");

            foreach (Monster monster in targets)
            {
                if (monster.IsAlive)
                {

                    int damage = (int)(player.AttackPower * damageMultiple);
                    damage = player.CalculateCriDamage(damage);
                    Console.WriteLine($"[{monster.Name}]에게 단검을 던집니다!");
                    monster.TakeDamage(damage);
                }
            }
        }
    }
}
