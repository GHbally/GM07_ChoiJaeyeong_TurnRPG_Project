using System.Xml.Serialization;
using Game.Characters;
using Game.Items;

namespace Game.Shop
{
    public class ShopManager
    {
        private List<Item> shopItems;

        public ShopManager()
        {
            shopItems = new List<Item>();

            shopItems.Add(new HPPotion());
            shopItems.Add(new MPPotion());

            shopItems.Add(new EquipmentItem(
                "낡은 기사의 검",
                100,
                "공격력을 조금 올려주는 검입니다.",
                Enums.ItemType.Weapon,
                5,
                0,
                0
            ));
            shopItems.Add(new EquipmentItem(
                "퀴퀴한 가죽 갑옷",
                100,
                "방어력과 생명력을 조금 올려주는 갑옷입니다.",
                Enums.ItemType.Armor,
                0,
                5,
                50
            ));
            shopItems.Add(new EquipmentItem(
                "활력의 반지",
                100,
                "생명력을 조금 올려주는 반지입니다.",
                Enums.ItemType.Acc,
                0,
                0,
                80
            ));
            shopItems.Add(new EquipmentItem(
                "전설의 검",
                350,
                "전설 속에서 등장하는 검입니다.",
                Enums.ItemType.Weapon,
                1000,
                1000,
                1000
            ));
        }
        public void OpenShop(Player player)
        {
            while (true)
            {
                
                Console.WriteLine("======= 상점 =======");
                Console.WriteLine($"보유 골드 : [{player.Gold}]G");

                ShowShopItems();

                Console.WriteLine();
                Console.WriteLine("1. 구매");
                Console.WriteLine("2. 인벤토리 확인");
                Console.WriteLine("3. 나가기");

                Console.Write("선택 : ");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        BuyItem(player);
                        break;
                    case "2":
                        player.OpenInventory();
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        break;
                }
            }
        }
        private void ShowShopItems()
        {
            for(int i = 0;i<shopItems.Count;i++)
            {
                Console.Write($"{i + 1}. ");
                shopItems[i].PrintInfo();
                Console.WriteLine();
            }
        }
        private void BuyItem(Player player)
        {
            Console.Write("구매할 아이템 번호 : ");
            string input = Console.ReadLine();
            Console.Clear();

            if (!int.TryParse(input, out int itemNumber))
            {
                Console.WriteLine("숫자를 입력해주세요.");
                return;
            }

            int index = itemNumber - 1;

            if(index < 0 || index >= shopItems.Count)
            {
                Console.WriteLine("잘못된 아이템 번호입니다.");
                return;
            }
            Item item = shopItems[index];

            if (!player.SpendGold(item.Price))
            {
                Console.WriteLine("돈이 부족합니다. 돈을 더 벌고 오십시오!");
                return;
            }

            player.AddItem(item);
            shopItems.RemoveAt(index);
            Console.WriteLine($"[{item.Name}]을(를) 구매했습니다.");
        }
    }
}
