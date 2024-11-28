using Project_Seven;
using System.Globalization;
using System.Net;
using System.Xml.Linq;

namespace Project_Seven
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Console.Write("Введите дату доставки (гггг.мм.дд): ");
            DateTime ddate = DateTime.Parse(Console.ReadLine());
            Console.Write("Введите номер телефона курьера: ");
            string phonenumDel = Console.ReadLine();
            int i1 = phonenumDel.NumCount();
            if (i1 != 11)
            {

                Console.WriteLine("Что-то не так");
            }
            else
            {
                Console.Write("Введите адрес доставки: ");
                string address = Console.ReadLine();

                Console.Write("Введите имя курьера: ");
                string nameMan = Console.ReadLine();

                Console.WriteLine("Ваш курьер TruckMan/BikeMan/LegMan?");
                string mantype = Console.ReadLine();

                Console.WriteLine("Выберите тип доставки(укажите только число):\n1)Дом\n2)ПВЗ\n3)Магазин");
                int delivType = Convert.ToInt32(Console.ReadLine());

                switch (mantype)
                {
                    case "TruckMan":
                        Console.Write("Введите номер машины: ");
                        int carNum = Convert.ToInt32(Console.ReadLine());
                        Console.Write("Данные о курьере: ");
                        TruckMan truckMan = new TruckMan(nameMan, carNum, phonenumDel);
                        truckMan.DeliveManScript();
                        break;
                    case "BikeMan":
                        Console.Write("Введите цвет велосипеда: ");
                        string color = Console.ReadLine();
                        Console.Write("Данные о курьере: ");
                        BikeMan bikeMan = new BikeMan(nameMan, color, phonenumDel);
                        bikeMan.DeliveManScript();
                        break;
                    case "LegMan":
                        Console.Write("Данные о курьере: ");
                        LegMan legMan = new LegMan(nameMan, phonenumDel);
                        legMan.DeliveManScript();
                        break;
                    default:
                        Console.WriteLine("Что-то не так");
                        break;
                }


                if (delivType == 1 )
                {
                    
                    Console.Write("Введите имя контактного лица: ");
                    string contactPerson = Console.ReadLine();
                    Console.Write("Введите номер телефона контакного лица: ");
                    string phone = Console.ReadLine();
                    var homeDelivery = new HomeDelivery(ddate, contactPerson, phone)
                    {
                        Address = address
                    };

                    var order = new Order<HomeDelivery>
                    {
                        Delivery = homeDelivery,
                        Number = 1
                    };
                    while (true)
                    {
                        Console.WriteLine("Добавить продукт? (да/нет): ");
                        string answer = Console.ReadLine();
                        if (answer.ToLower() != "да")
                            break;

                        Console.WriteLine("Введите название продукта: ");
                        string itemName = Console.ReadLine();

                        Console.WriteLine("Введите срок годности продукта (дд.мм.гггг): ");
                        string expirationDate = Console.ReadLine();
                        order.AddItem(new FoodItem { Name = itemName, ExpirationDate = expirationDate });
                    }
                    Console.Write("Данные о о доставке и продукте: \nДоставка на дом");
                    order.ShowAllItems();
                    foreach (var item in order.items)
                    {
                        homeDelivery.DescriptionOrd(item);
                    }

                }
                else if (delivType == 2)
                {
                    Console.Write("Введите название ПВЗ: ");
                    string pointName = Console.ReadLine();
                    var pickPointDelivery = new PickPointDelivery(ddate, pointName)
                    {
                        Address = address
                    };
                    var order = new Order<PickPointDelivery>
                    {
                        Delivery = pickPointDelivery,
                        Number = 1
                    };
                    while (true)
                    {
                        Console.WriteLine("Добавить продукт? (да/нет): ");
                        string answer = Console.ReadLine();
                        if (answer.ToLower() != "да")
                            break;

                        Console.WriteLine("Введите название продукта: ");
                        string itemName = Console.ReadLine();

                        Console.WriteLine("Введите срок годности продукта (дд.мм.гггг): ");
                        string expirationDate = Console.ReadLine();
                        order.AddItem(new FoodItem { Name = itemName, ExpirationDate = expirationDate });
                    }
                    Console.Write("Данные о о доставке и продукте: \nДоставка в ПВЗ");
                    order.ShowAllItems();
                    foreach (var item in order.items)
                    {
                        pickPointDelivery.DescriptionOrd(item);
                    }
                }
                else if (delivType == 3)
                {
                    Console.Write("Введите название магазина: ");
                    string shopName = Console.ReadLine();
                    var shopDelivery = new ShopDelivery(ddate, shopName)
                    {
                        Address = address
                    };
                    var order = new Order<ShopDelivery>
                    {
                        Delivery = shopDelivery,
                        Number = 1
                    };
                    while (true)
                    {
                        Console.WriteLine("Добавить продукт? (да/нет): ");
                        string answer = Console.ReadLine();
                        if (answer.ToLower() != "да")
                            break;

                        Console.WriteLine("Введите название продукта: ");
                        string itemName = Console.ReadLine();

                        Console.WriteLine("Введите срок годности продукта (дд.мм.гггг): ");
                        string expirationDate = Console.ReadLine();
                        order.AddItem(new FoodItem { Name = itemName, ExpirationDate = expirationDate });
                    }
                    Console.Write("Данные о о доставке и продукте: \nДоставка в магазин");
                    order.ShowAllItems();
                    foreach (var item in order.items)
                    {
                        shopDelivery.DescriptionOrd(item);
                    }
                }
                else
                    Console.WriteLine("Что-то не так");
        }
    }
    
    public static class StringExtensions
    {
        public static int NumCount(this string phone)
        {
            int counter = 0;
            for (int i = 0; i < phone.Length; i++)
                counter++;

            return counter;
        }
    }
    static class DeliveryUtilities
    {
        public static string FormatDate(DateTime date)
        {
            return date.ToString("dd.MM.yyyy");
        }
    }
    abstract class Item
    {
        public string Name;
        public abstract void ShowItemDetails();
    }

    class FoodItem : Item
    {
        public string ExpirationDate;

        public override void ShowItemDetails()
        {
            Console.WriteLine("Продукт: {0}, срок годности: {1}", Name, ExpirationDate);
        }
    }

    static class OrderExtensions
    {
        public static void ShowDeliveryDetails<T>(this Order<T> order) where T : Delivery
        {
            Console.WriteLine($"Детали доставки для заказа #{order.Number}:");
            order.Delivery.DescriptionOrd(order[0]);
        }
    }

    abstract class Delivery     
    {

        public DateTime DDate;
        private string address;  

        public string Address
        {
            get
            {
                return address;  
            }
            set
            {
                if (value.Length <= 4)
                    Console.WriteLine("Что-то не так");
                else
                    address = value;  
            }
        }
        public Delivery(DateTime DDate)
        {
            this.DDate = DDate;
        }



        public abstract void DescriptionOrd<TItem>(TItem item) where TItem : Item;
       
    }

    
    class HomeDelivery : Delivery
    {
        public string contactPers;
        public string phone;

        public HomeDelivery(DateTime DDate, string contactPers, string phone) : base(DDate)
        {
            this.contactPers = contactPers;
            this.phone = phone;
        }

        public override void DescriptionOrd<TItem>(TItem item)
        {
            Console.WriteLine("Доставка по адресу {0}, Контакт:{1} Номер телефона: {2} Дата {3}", Address, contactPers, phone, DDate);
            item.ShowItemDetails();
        }
    }

    class PickPointDelivery : Delivery
    {
        public string pickupPointName;

        public PickPointDelivery(DateTime DDate, string pickupPointName) : base(DDate)
        {
            this.pickupPointName = pickupPointName;
        }

        public override void DescriptionOrd<TItem>(TItem item)
        {
            Console.WriteLine("Доставка в ПВЗ {0}, по адресу {1}, дата: {2}", pickupPointName, Address, DDate);
            item.ShowItemDetails();
        }
    }

    class ShopDelivery : Delivery
    {
        public string shopName;

        public ShopDelivery(DateTime DDate, string shopName) : base(DDate)
        {
            this.shopName = shopName;
        }

        public override void DescriptionOrd<TItem>(TItem item)
        {
            Console.WriteLine("Доставка в магазин {0}, по адресу {1}, дата: {2}", shopName, Address, DDate);
            item.ShowItemDetails();
        }
    }
    
    abstract class DeliveryMan
    {
        
        public static int shift;
        public string name;
        public string phone;

        
        public DeliveryMan(string name, string phone)
        {
            this.name = name;
            this.phone = phone;
        }
        public abstract void DeliveManScript();
    }

    class TruckMan: DeliveryMan
    {
        public int CarNum;
        
        public TruckMan(string name, int CarNum, string phone) : base(name, phone)
        {
            this.CarNum = CarNum;
        }

        public override void DeliveManScript()
        {
            Console.WriteLine("Курьер {0} номер машины {1}", name, CarNum);
        }
    }

    class BikeMan: DeliveryMan
    {
        public string color;
        public BikeMan(string name, string color, string phone) : base(name, phone)
        {
            this.color = color;
        }

        public override void DeliveManScript()
        {
            Console.WriteLine("Курьер {0} цвет велосипеда {1}", name, color);
        }
    }
    

    class LegMan: DeliveryMan
    {
        public string phonenum;
    public LegMan(string name, string phone) : base(name, phone) { }
    
    public override void DeliveManScript()
        {
        Console.WriteLine("Курьер {0}", name);
        }
    }

    
    

    class Order<TDelivery> where TDelivery : Delivery
    {
        public TDelivery Delivery;

        public int Number;

        public string Description;
        public List<Item> items;

        public Order()
        {
            items = new List<Item>();
        }
        public void DisplayAddress()
        {
            Console.WriteLine(Delivery.Address);
        }

        public Item this[int index]
        {
            get
            {
                return items[index];
            }
            set
            {
                items[index] = value;
            }
        }
        public void AddItem(Item item)
        {
            items.Add(item);
        }

        public void ShowAllItems()
        {
            Console.WriteLine("Предмет номер #{0}", Number);
            foreach (var item in items)
                item.ShowItemDetails();
        }
        public static Order<TDelivery> operator +(Order<TDelivery> order, Item item)
        {
            order.AddItem(item);
            return order;
        }
         
    }

}
