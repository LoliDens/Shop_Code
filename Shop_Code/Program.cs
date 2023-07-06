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
        const string MessageErrorInput = "Error input";

        const string CommandShowSellerItems = "1";
        const string CommandShowBuyerItems = "2";
        const string CommandBuyItem = "3";
        const string CommandExit = "0";

        private bool isWrok;

        
        public void Trade(Seller seller,Buyer buyer)
        {
            Console.WriteLine("Welcome to Shop");
            isWrok = true;

            while (isWrok)
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
                        isWrok = false;
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
            string input = Console.ReadLine();

            if (!CorrectCharacters(input))
            {
                Console.WriteLine(MessageErrorInput);
                return;
            }

            int number = int.Parse(input);

            if (seller.SellItem(out Item item,buyer.GetAmountCoins, number)) 
                buyer.BuyItem(item);
        }

        private bool CorrectCharacters(string number)
        {
            int i = 0;
            char[] all = { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };

            foreach (var item in number.Union(all))
                i++;

            return i == all.Length;
        }

    }

    class Buyer : Player
    {
        public Buyer(string name, int amountCoins, List<Item> items) : base(name, amountCoins, items) { }

        public void BuyItem(Item item)
        {
            _items.Add(item);
            _amountCoins -= item.GetPirce;
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

                _items.Remove(needItem);
                _amountCoins += needItem.GetPirce;
                item = needItem;
                return true;
            }

            return false;
        }

        private bool FindItem(out Item needItem, int numberItem)
        {
            needItem = null;

            if (_items.Count < numberItem && _items[numberItem - 1] != null)
            {
                Console.WriteLine("Item not found");
                return false;
            }            

            needItem = _items[numberItem - 1];
            return true;

        }
    }

    class Player
    {
        protected string _name;
        protected int _amountCoins;
        protected List<Item> _items;

        public int GetAmountCoins => _amountCoins;

        public Player(string name, int amountCoins, List<Item> items)
        {
            _name = name;
            _amountCoins = amountCoins;
            _items = items;
        }

        public void ShowPlayer()
        {
            Console.WriteLine($"Name: {_name} || AmountCouns: {_amountCoins}");
        }

        public void ShowItems()
        {
            if (_items.Count == 0)
                Console.WriteLine("No items");
            else
                for (int i = 0; i < _items.Count; i++)
                    Console.WriteLine($"{i + 1}." + _items[i].GetInfoItem());
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
