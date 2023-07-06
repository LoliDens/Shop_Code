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
        private const string MessageErrorInput = "Error input";//Создание Класса

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
                        seller.ShowPlayer();
                        seller.ShowItems();
                        break;

                    case CommandShowBuyerItems:
                        buyer.ShowPlayer();
                        buyer.ShowItems();
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

        private void RunCommandBuyItem(Seller seller,Buyer buyer)
        {
            Console.WriteLine("input number item: ");
            int number = ReadNumber();

            if (seller.SellItem(out Item item,buyer.GetAmountCoins, number)) 
                buyer.BuyItem(item);
        }

        private int ReadNumber()
        {
            int result;
            string numberForConvert = "";

            while (int.TryParse(numberForConvert, out result) == false)
            {
                Console.Write("Input number:");
                numberForConvert = Console.ReadLine();
            }

            return result;
        }
    }

    class Buyer : Player
    {
        public Buyer(string name, int amountCoins, List<Item> items) : base(name, amountCoins, items) { }

        public void BuyItem(Item item)
        {
            Items.Add(item);
            AmountCoins -= item.GetPirce;
            Console.WriteLine("You bought: "  + item.GetName);
        }
    }

    class Seller : Player
    {
        public Seller(string name, int amountCoins, List<Item> items) : base(name, amountCoins, items) { }

        public bool SellItem(out Item item,int amountCoinsBuyer,int numberItem)
        {
            item = null;

            if (FindItem(out Item needItem, numberItem)) 
            {
                if (amountCoinsBuyer < needItem.GetPirce)
                {
                    Console.WriteLine("Not enough coins");
                    return false;
                }

                Items.Remove(needItem);
                AmountCoins += needItem.GetPirce;
                item = needItem;
                return true;
            }

            return false;
        }

        private bool FindItem(out Item needItem, int numberItem)
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

        public int GetAmountCoins => AmountCoins;

        public Player(string name, int amountCoins, List<Item> items)
        {
            Name = name;
            AmountCoins = amountCoins;
            Items = items;
        }

        public void ShowPlayer()
        {
            Console.WriteLine($"Name: {Name} || AmountCouns: {AmountCoins}");
        }

        public void ShowItems()
        {
            if (Items.Count == 0)
                Console.WriteLine("No items");
            else
                for (int i = 0; i < Items.Count; i++)
                    Console.WriteLine($"{i + 1}." + Items[i].GetInfoItem());
        }
    }


    class Item
    {
        private string _name;
        private string _description;
        private int _price;

        public int GetPirce => _price;
        public string GetName => _name;

        public Item(string name, string description, int price)
        {
            _name = name;
            _description = description;
            _price = price;
        }

        public string GetInfoItem()
        {
            return $"{_name}|Description - {_description}|Price - {_price}";
        }
    }
}
