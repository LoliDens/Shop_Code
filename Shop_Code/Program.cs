using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Code
{  
    internal class Program
    {
        static void Main(string[] args)
        {
            Item sword = new Item("Sword", "Just sword", 10);
            Item shield = new Item("Shield", "Just Shield", 5);
            Item food = new Item("Food", "Just food", 1);
            Item map = new Item("Map", "Just map", 30);

            Seller seller = new Seller("Sem", 20 ,new List<Item>(){ sword, food, food, map,shield });
            Buyer buyer = new Buyer("Tom", 100 ,new List<Item>());

            Shop shop = new Shop();
            shop.Trade(seller,buyer);
        }
    }

    class Shop
    {
        private const string MessageErrorInput = "Error input";

        private const string CommandShowSellerItems = "1";
        private const string CommandShowBuyerItems = "2";
        private const string CommandBuyItem = "3";
        private const string CommandExit = "0";

        private bool isWork;
        
        public void Trade(Seller seller,Buyer buyer)
        {
            Console.WriteLine("Welcome to Shop");
            isWork = true;

            while (isWork)
            {               
                Console.WriteLine("Input Command:");
                Console.WriteLine($"{CommandShowSellerItems} - show items seller");
                Console.WriteLine($"{CommandShowBuyerItems} - show your item");
                Console.WriteLine($"{CommandBuyItem} - buy item");
                Console.WriteLine($"{CommandExit} - exit");
                string input = Console.ReadLine();

                switch (input)
                {
                    case CommandShowSellerItems:
                        ShowPlayer(seller);
                        break;

                    case CommandShowBuyerItems:
                        ShowPlayer(buyer);
                        break;

                    case CommandBuyItem:
                        RunCommandBuyItem(seller,buyer);
                        break;

                    case CommandExit:
                        isWork = false;
                        break;

                    default:
                        Console.WriteLine(MessageErrorInput);
                        break;
                }

                Console.ReadKey();
                Console.Clear();
            }
        }

        private void ShowPlayer(Player player)
        {
            Console.WriteLine(player);
            player.ShowItems();
        }

        private void RunCommandBuyItem(Seller seller,Buyer buyer)
        {
            Console.WriteLine("input number item: ");
            int number = ReadNumber(0,seller.GetAmountItems);

            if (seller.TryGetItemPrice(out int priceItem, number) && buyer.CanBuy(priceItem))
            {
                buyer.BuyItem(seller.SellItem());
            }
        }

        private int ReadNumber(int min = int.MinValue, int max = int.MaxValue)
        {
            int result = 0;
            bool isWrok = false;

            while (!isWrok)
            {
                string number = Console.ReadLine();

                while (int.TryParse(number, out result) == false)
                {
                    Console.WriteLine("Input error.Re-enter the number");
                    number = Console.ReadLine();
                }

                if (result <= max && result >= min)
                    isWrok = true;
                else
                    Console.WriteLine("Input error.Number is out for range");
            }

            return result;
        }
    }

    class Buyer : Player
    {
        public Buyer(string name, int amountCoins, List<Item> items) : base(name, amountCoins, items) { }

        public bool CanBuy(int itemPrice)
        {
            return AmountCoins >= itemPrice;
        }

        public void BuyItem(Item item)
        {
            if (item == null)
            {
                Console.WriteLine("Erorr.Item is empty");
                return;
            }                

            Items.Add(item);
            AmountCoins -= item._price;
            Console.WriteLine("You bought: "  + item._name);
        }
    }

    class Seller : Player
    {
        private Item _itemForSell;

        public Seller(string name, int amountCoins, List<Item> items) : base(name, amountCoins, items) { }

        public bool TryGetItemPrice(int numberItem, out int priceItem)
        {           
            if (TryGetItem(out Item item,numberItem))
            {
                _itemForSell = item;
                priceItem = _itemForSell._price;
                return true;
            }

            priceItem = 0;
            return false;
        }

        public Item SellItem()
        {
            if (_itemForSell == null)
            {
                Console.WriteLine("Error.Item is empty");
                return null;
            }
            
            Items.Remove(_itemForSell);
            AmountCoins += _itemForSell._price;
            return _itemForSell;
        }

        private bool TryGetItem(out Item needItem, int numberItem)
        {
            needItem = null;

            if (Items.Count < numberItem && Items[numberItem - 1] != null)
            {
                Console.WriteLine("Item not found");
                return false;
            }            

            needItem = Items[numberItem - 1];
            return true;
        }
    }

    abstract class Player
    {
        protected string Name;
        protected int AmountCoins;
        protected List<Item> Items;

        public Player(string name, int amountCoins, List<Item> items)
        {
            Name = name;
            AmountCoins = amountCoins;
            Items = items;
        }

        public int GetAmountItems => Items.Count;

        public override string ToString()
        {
            return ($"Name: {Name} || AmountCouns: {AmountCoins}");
        }

        public void ShowItems()
        {
            if (Items.Count == 0) 
            {
                Console.WriteLine("No items");
                return;
            }
            
            for (int i = 0; i < Items.Count; i++)
                Console.WriteLine($"{i + 1}." + Items[i]);
        }
    }

    class Item
    {
        public string _name { get; }
        public string _description { get; }
        public int _price { get; }
        public Item(string name, string description, int price)
        {
            _name = name;
            _description = description;
            _price = price;
        }

        public override string ToString()
        {
            return $"{_name}|Description - {_description}|Price - {_price}";
        }
    }
}
