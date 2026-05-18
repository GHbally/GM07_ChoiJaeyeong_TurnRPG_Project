using Game.BattleManagers;
using Game.Characters;
using Game.Enums;
using Game.Shop;


namespace MainGames
{
    internal class MainGame
    {
        ShopManager shopmanager = new ShopManager();
        public void Run()
        {
            Console.WriteLine("================================================");
            Console.WriteLine("                 콘솔 턴제 RPG                   ");
            Console.WriteLine("================================================");
            Console.WriteLine();


            Console.Write("플레이어 이름을 입력하세요: ");
            string playerName = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(playerName))
            {
                playerName = "무명";
            }

            JobType selectedJob = SelectJob();
            Player player = CreatePlayer(playerName, selectedJob);

            Console.WriteLine();
            Console.WriteLine("캐릭터 생성 완료!");
            Console.WriteLine();

            player.PrintCharacterInfo();
            Console.WriteLine();
            Console.WriteLine("전투를 시작하려면 아무 키나 누르세요...");
            Console.ReadKey();

            bool isClear = true;

            isClear = StartStageBattle(player, "슬라임 무리", CreateSlimes());

            if (isClear)
            {
                EnterTown(player);
                isClear = StartStageBattle(player, "고블린 무리", CreateGoblins());
            }

            if (isClear)
            {
                EnterTown(player);
                isClear = StartStageBattle(player, "오크 무리", CreateOrcs());
            }

            Console.WriteLine();

            if (isClear)
            {
                Console.WriteLine("모든 전투에서 승리했습니다!");
                Console.WriteLine("      ★ ★ ★ ★ ★ ★ ★ ★ ★ ★ ★ ★ ★ ★ ★ ★ ★ ★");
                Console.WriteLine("      ★ ★ ★ ★ ★ ★ ★ ★ ★ ★ ★ ★ ★ ★ ★ ★ ★ ★");
                Console.WriteLine("      ★ ★ ★ ★ ★ [ 게임 클리어! ]★ ★ ★ ★ ★");
                Console.WriteLine("      ★ ★ ★ ★ ★ ★ ★ ★ ★ ★ ★ ★ ★ ★ ★ ★ ★ ★");
                Console.WriteLine("      ★ ★ ★ ★ ★ ★ ★ ★ ★ ★ ★ ★ ★ ★ ★ ★ ★ ★");
            }
            else
            {
                Console.WriteLine("모험이 끝났습니다...");
            }

            Console.WriteLine();
            Console.WriteLine("게임을 종료합니다.");
            Console.ReadKey();
        }
        static bool StartStageBattle(Player player, string stageName, List<Monster> monsters)
        {
            Console.Clear();
            Thread.Sleep(500);
            Console.WriteLine("================================");
            Console.WriteLine($"        {stageName}");
            Console.WriteLine("================================");
            Console.WriteLine();

            Console.WriteLine("등장 몬스터");

            foreach (Monster monster in monsters)
            {
                Console.WriteLine($"- [{monster.Name}]");
            }

            Console.WriteLine();
            Console.WriteLine("전투를 시작하려면 아무 키나 누르세요...");
            Console.ReadKey();

            BattleManager battleManager = new BattleManager(player, monsters);
            bool result = battleManager.StartBattle();

            Console.WriteLine();

            if (result)
            {
                Console.WriteLine($"[{stageName}] 전투에서 승리했습니다!");
                Console.WriteLine("다음 전투로 이동합니다.");
            }
            else
            {
                Console.WriteLine($"[{stageName}] 전투에서 패배했습니다.");
            }
            Console.WriteLine();
            Console.WriteLine("계속하려면 아무 키나 누르세요...");
            Console.ReadKey();
            Console.Clear();

            return result;
        }

        static JobType SelectJob()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("직업을 선택하세요.");
                Console.WriteLine("1. 전사");
                Console.WriteLine("2. 마법사");
                Console.WriteLine("3. 도적");

                Console.Write("직업 번호: ");
                string input = Console.ReadLine();
                Console.WriteLine();

                switch (input)
                {
                    case "1":
                        Console.WriteLine("전사를 선택했습니다.");
                        return JobType.Warrior;

                    case "2":
                        Console.WriteLine("마법사를 선택했습니다.");
                        return JobType.Mage;

                    case "3":
                        Console.WriteLine("도적을 선택했습니다.");
                        return JobType.Rogue;

                    default:
                        Console.WriteLine("잘못된 입력입니다. 다시 선택해주세요.");
                        break;
                }
            }
        }

        static Player CreatePlayer(string name, JobType jobType)
        {
            switch (jobType)
            {
                case JobType.Warrior:
                    return new Player(
                        name,
                        jobType,
                        hp: 100,
                        mp: 100,
                        attackPower: 25,
                        defence: 15,
                        avoidChance: 10
                    );

                case JobType.Mage:
                    return new Player(
                        name,
                        jobType,
                        hp: 100,
                        mp: 200,
                        attackPower: 15,
                        defence: 5,
                        avoidChance: 5
                    );

                case JobType.Rogue:
                    return new Player(
                        name,
                        jobType,
                        hp: 100,
                        mp: 100,
                        attackPower: 22,
                        defence: 7,
                        avoidChance: 20
                    );

                default:
                    return new Player(
                        name,
                        JobType.Warrior,
                        hp: 120,
                        mp: 80,
                        attackPower: 20,
                        defence: 8,
                        avoidChance: 5
                    );
            }
        }
        static List<Monster> CreateSlimes()
        {
            return new List<Monster>
                {
                    new Monster("슬라임 A", 50, 8, 5, 10, 120),
                    new Monster("슬라임 B", 50, 8, 2, 10, 120),
                    new Monster("슬라임 C", 50, 8, 2, 10, 120)
                };
        }

        static List<Monster> CreateGoblins()
        {
            return new List<Monster>
                {
                    new Monster("고블린 A", 80, 14, 4, 15, 150),
                    new Monster("고블린 B", 80, 14, 4, 15, 150),
                    new Monster("고블린 C", 80, 14, 4, 15, 150)
                };
        }

        static List<Monster> CreateOrcs()
        {
            return new List<Monster>
                {
                    new Monster("오크 A", 120, 20, 8, 20, 200),
                    new Monster("오크 B", 120, 20, 8, 20, 200),
                    new Monster("오크 C", 120, 20, 8, 20, 200)
                };
        }
        private void EnterTown(Player player)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("======= 마을 =======");
                Console.WriteLine("1. 스텟 및 상태 확인");
                Console.WriteLine("2. 인벤토리 확인");
                Console.WriteLine("3. 상점 이용");
                Console.WriteLine("4. 다음 스테이지로 이동");

                Console.Write("선택 : ");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        player.PrintCharacterInfo();
                        Console.WriteLine();
                        Console.WriteLine("계속하려면 아무 키나 누르세요...");
                        Console.ReadKey();
                        break;
                    case "2":
                        player.OpenInventory();
                        break;
                    case "3":
                        shopmanager.OpenShop(player);
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        break;
                }

            }
        }
    } // End Of MainGame
}
