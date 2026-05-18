using Game.Characters;
using Game.Skills;

namespace Game.BattleManagers
{
    public class BattleManager
    {
        private Player Player;
        private List<Monster> Monsters;

        public BattleManager(Player player, List<Monster> monsters)
        {
            Player = player;
            Monsters = monsters;
        }
        public bool StartBattle()
        {
            Console.Clear();
            Console.WriteLine("======= 전투 시작!! =======");

            while (Player.IsAlive && HasAliveMonster())
            {
                PrintStatus();

                PlayerTurn();

                if (!HasAliveMonster())
                {
                    WinBattle();
                    Console.WriteLine("======= 전투 종료 =======");
                    return true;
                }
                MonsterTurn();

                if (!Player.IsAlive)
                {
                    LoseBattle();
                    Console.WriteLine("======= 전투 종료 =======");
                    return false;
                }
            }
            Console.WriteLine("======= 전투 종료 =======");
            return Player.IsAlive;
        }

        private void PlayerTurn()
        {
            Console.WriteLine();
            Console.WriteLine("------- 플레이어 턴 -------");
            Console.WriteLine("1. 일반 공격");
            Console.WriteLine("2. 스킬 사용");
            Console.WriteLine("3. 상태 확인");
            Console.WriteLine("4. 인벤토리");

            Console.Write("무엇을 할까? : ");
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    PlayerAttack();
                    break;

                case "2":
                    PlayerUseSkill();
                    break;

                case "3":
                    PrintStatus();
                    PlayerTurn();
                    break;
                case "4":
                    Player.OpenInventory();
                    PlayerTurn();
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다. 다시 선택해주세요.");
                    PlayerTurn();
                    break;
            }
        }
        private void PlayerAttack()
        {
            Monster target = SelectTarget();

            if (target == null)
            {
                Console.WriteLine("공격할 대상이 없습니다.");
                return;
            }
            Player.Attack(target);
        }
        private void PlayerUseSkill()
        {
            while (true)
            {
                Player.ShowSkills();

                Console.Write("사용할 스킬 번호 : ");
                string input = Console.ReadLine();

                Console.Clear();

                if (!int.TryParse(input, out int skillNumber))
                {
                    Console.WriteLine("숫자를 입력해 주세요.");
                    continue;
                }

                Skill skill = Player.GetSkill(skillNumber);

                if (skill == null)
                {
                    Console.WriteLine("잘못된 스킬 번호입니다. 다시 선택해주세요.");
                    continue;
                }
                if (skill is HealSkill)
                {
                    skill.Use(Player, null);
                    break;
                }

                if (IsAreaSkill(skill))
                {
                    skill.UseArea(Player, Monsters);
                    break;
                }

                Monster target = SelectTarget();

                if (target == null)
                {
                    Console.WriteLine("대상이 없습니다.");
                    continue;
                }

                skill.Use(Player, target);
                break;
               
            }
        }
        private bool IsAreaSkill(Skill skill)
        {
            return (skill is FireballSkill || skill is FanOfKnivesSkill);
        }
        private Monster SelectTarget()
        {
            while (true)
            {
                List<Monster> aliveMonsters = GetAliveMonsters();

                if (aliveMonsters.Count == 0)
                {
                    Console.WriteLine("공격할 대상이 없습니다.");
                    return null;
                }

                Console.WriteLine();
                Console.WriteLine("--- 대상 선택 ---");

                for (int i = 0; i < aliveMonsters.Count; i++)
                {
                    Monster monster = aliveMonsters[i];

                    Console.WriteLine(
                        $"{i + 1}. [{monster.Name}] HP : [{monster.Hp}/{monster.MaxHp}] " +
                        $"ATK : [{monster.AttackPower}] DEF : [{monster.Defence}]"
                    );
                }

                Console.Write("대상 번호 : ");
                string input = Console.ReadLine();

                if (!int.TryParse(input, out int targetNumber))
                {
                    Console.WriteLine("숫자를 입력해 주세요.");
                    continue;
                }

                int index = targetNumber - 1;

                if (index < 0 || index >= aliveMonsters.Count)
                {
                    Console.WriteLine("잘못된 번호입니다. 다시 선택해주세요.");
                    continue;
                }

                return aliveMonsters[index];
            }

        }
        private void MonsterTurn()
        {
            Console.WriteLine("------- 몬스터 턴 -------");
            foreach(Monster monster in Monsters)
            {
                if (!monster.IsAlive)
                {
                    continue;
                }
                if (!Player.IsAlive)
                {
                    break;
                }
                monster.TakeTurn(Player);
            }
        }
        private bool HasAliveMonster()
        {
            foreach(Monster monster in Monsters)
            {
                if (monster.IsAlive)
                {
                    return true;
                }
            }
            return false;
        }
        private List<Monster> GetAliveMonsters()
        {
            List<Monster> aliveMonsters = new List<Monster>();

            foreach(Monster monster in Monsters)
            {

                if (monster.IsAlive)
                {
                    aliveMonsters.Add(monster);
                }
            }
            return aliveMonsters;
        }
        private void WinBattle()
        {
            Console.WriteLine();
            Console.WriteLine("전투에서 승리했습니다!");

            foreach(Monster monster in Monsters)
            {
                monster.GiveReward(Player);
            }
        }
        private void LoseBattle()
        {
            Console.WriteLine();
            Console.WriteLine("전투에서 패배했습니다...");
        }
        private void PrintStatus()
        {
            Console.WriteLine();
            Console.WriteLine("--- 현재 상태 ---");

            Console.WriteLine();
            Console.WriteLine("[플레이어]");
            Player.PrintCharacterInfo();

            Console.WriteLine();
            Console.WriteLine("[몬스터]");
            foreach(Monster monster in Monsters)
            {
                monster.PrintCharacterInfo();
            }
        }
    }
}
