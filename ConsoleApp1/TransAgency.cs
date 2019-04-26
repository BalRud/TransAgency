using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class TransAgency
    {
        Branch[] branches;
        Transport[] transports;
        Random r = new Random();
        double totalIncome;
        double badWeatherLoses;
        double totalCrashLoses;
        int orderCount;
        List<Order> newOrders = new List<Order>();                                   // Список заказов 
        List<Order> activeOrders = new List<Order>();                                // Список активных заказов 
        List<Order> delayedOrders = new List<Order>();                               // Список задержанных заказов 
        List<int> elapsedTime = new List<int>();                                     // Список затраченного времени на каждый заказ
        int totalHighwayWeight;
        int totalRegularWeight;
        int totalHighwayDistance;
        int totalRegularDistance;
        int totalHighwayIncome;
        int totalRegularIncome;


        public Branch[] Branches { get { return branches; } }
        public Transport[] Transports { get { return transports; } }
        public Random R { get { return r; } }
        public double TotalIncome
        {
            set { totalIncome = value; }
            get { return totalIncome; }
        }
        public double BadWeatherLoses
        {
            set { badWeatherLoses = value; }
            get { return badWeatherLoses; }
        }
        public double TotalCrashLoses
        {
            set { totalCrashLoses = value; }
            get { return totalCrashLoses; }
        }
        public int OrderCount
        {
            get { return orderCount; }
            set { orderCount = value; }
        }
        public List<Order> NewOrders { get { return newOrders; } }
        public List<Order> ActiveOrders { get { return activeOrders; } }
        public List<Order> DelayedOrders { get { return delayedOrders; } }
        public List<int> ElapsedTime { get { return elapsedTime; } }


        public TransAgency()                                                         // Конструктор
        {
            branches = new Branch[]
            {
                new Branch("Киев", 3, 20),
                new Branch("Херсон", 2, 15),
                new Branch("Харьков", 3, 10),
                new Branch("Львов", 1, 10),
                new Branch("Донецк", 2, 20),
                new Branch("Москва", 3, 35),
                new Branch("Севастополь", 2, 30),
                new Branch("Одесса", 1, 15)
            };

            transports = new Transport[]
            {
                new Plane(),
                new Train(),
                new Car()
            };
        }

        public void CreateRandomOrder()                                              // Создание случайного заказа
        {
            int weight = r.Next(5, 30);
            int speed = -1;
            int price = -1;
            int num = r.Next(0, 4);
            int branchIndex = r.Next(0, 8);
            int branchIndex2 = r.Next(0, 8);


            if (num == 0)
                speed = r.Next(1, 6);
            else if (num == 1)
                price = r.Next(2, 11);

            while (branchIndex == branchIndex2)
            {
                branchIndex2 = r.Next(0, 8);
            }

            branches[branchIndex].NewOrder(branches[branchIndex2], weight, (branches[branchIndex].Distance+branches[branchIndex2].Distance),speed, price);
        }

        public void DeliveringProcess()                                              // Процесс доставки 
        {
            for (int i = 0; i < DelayedOrders.Count; i++)
            {
                try
                {
                    if (DelayedOrders[i].WeatherCheck())
                        DelayedOrders.Remove(DelayedOrders[i]);
                }
                catch { }
            }
            for (int i = 0; i < ActiveOrders.Count; i++)
            {
                string tmp = ActiveOrders[i].OrderInProgress();
                if (tmp == "Delivered")
                {
                    if (ActiveOrders.Count != 0 && ActiveOrders[i].Highway && ActiveOrders[i].TransportType != "Plane")
                    {
                        totalHighwayDistance += ActiveOrders[i].DeliveringDistance;
                        totalHighwayWeight += ActiveOrders[i].Weight;
                        totalHighwayIncome += ActiveOrders[i].Price;
                    }
                    else if(!ActiveOrders[i].Highway && ActiveOrders[i].TransportType != "Plane")
                    {
                        totalRegularDistance += ActiveOrders[i].DeliveringDistance;
                        totalRegularWeight += ActiveOrders[i].Weight;
                        totalRegularIncome += ActiveOrders[i].Price;
                    }
                    ActiveOrders.Remove(ActiveOrders[i]);
                    if (i == ActiveOrders.Count)
                        break;
                }
                else if (tmp == "Accident")
                {
                    if (ActiveOrders.Count != 0 && ActiveOrders[i].Highway && ActiveOrders[i].TransportType != "Plane")
                    {
                        totalHighwayIncome -= ActiveOrders[i].Price;
                    }
                    else if (!ActiveOrders[i].Highway && ActiveOrders[i].TransportType != "Plane")
                    {
                        totalRegularIncome -= ActiveOrders[i].Price;
                    }
                    ActiveOrders.Remove(ActiveOrders[i]);
                    if (i == ActiveOrders.Count)
                        break;
                }
            }
        }

        public void ShowInfo()                                                       // Вывод меню после остановки симюляции
        {
            while (true)
            {
                Console.WriteLine("Команды: \n 1 - Активные заказы; \n 2 - Задержанные заказы; \n 3 - Доходы; \n 4 - Потери из-за погоды; " +
                                  "\n 5 - Потери от аварий; \n 6 - Среднее время доставки; \n 7 - Доход на тонно-километр; \n 0 - Выход из меню;");
                int cmd = Convert.ToInt32(Console.ReadLine());
                Console.Clear();
                switch (cmd)
                {
                    case 0:
                        return;
                    case 1:                        //  Список исполняемых заказов с возможность сортировки по городам, видам транспорта, стоимости перевозки.
                        Command1();
                        break;
                    case 2:                        //  Список задерживаемых заказов в связи с плохой погодой.
                        Command2();
                        break;
                    case 3:                        //  Доход трансагенства, в том числе с разбивкой по видам транспорта и городам.
                        Command3();
                        break;
                    case 4:                        //  Потери, связанные с плохой погодой.
                        Command4();
                        break;
                    case 5:                        //  Потери, связанные с аварийностью, в том числе с разбивкой по видам транспорта и по видам дорог.
                        Command5();
                        break;
                    case 6:                        //  Среднее время доставки груза, в том числе с разбивкой по видам транспорта и городам.
                        Command6();
                        break;
                    case 7:                        //  Доход на тонно-километр скоростных магистралей в сравнении с таким же доход на обычных дорогах.
                        Command7();
                        break;
                }

                Console.ReadLine();
                Console.Clear();
            }

            // Методы команд выбранных в ShowInfo()
            void Command1()
            {
                Order tmp;
                Console.WriteLine("Всего заказов: " + ActiveOrders.Count);
                Console.WriteLine("Выберите метод сортировки: \n " +
                                  "1 - без сортировки; \n " +
                                  "2 - сортировка по городам; \n " +
                                  "3 - сортировка по видам транспорта; \n " +
                                  "4 - сортировка по стоимости перевозки;");
                int cmd = Convert.ToInt32(Console.ReadLine());
                Console.Clear();
                switch (cmd)
                {
                    case 1:
                        Console.Clear();
                        Console.WriteLine("Без сортировки: ");
                        Console.WriteLine(new string('-', 50));
                        for (int i = 0; i < ActiveOrders.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}: " + ActiveOrders[i].Name);
                            Console.WriteLine(new string('-', 50));
                        }        // Вывод
                        break;
                    case 2:
                        Console.Clear();
                        Console.WriteLine("Сортировка по городам: ");
                        Console.WriteLine(new string('-', 50));
                        for (int i = 0; i < ActiveOrders.Count; i++)
                        {
                            for (int j = 0; j < ActiveOrders.Count; j++)
                            {
                                if (String.Compare(ActiveOrders[i].DepartureCity, ActiveOrders[j].DepartureCity) == -1)
                                {
                                    tmp = ActiveOrders[i];
                                    ActiveOrders[i] = ActiveOrders[j];
                                    ActiveOrders[j] = tmp;
                                }
                            }
                        }        // Соритировка
                        for (int i = 0; i < ActiveOrders.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}: " + ActiveOrders[i].Name);
                            Console.WriteLine(new string('-', 50));
                        }        // Вывод
                        break;
                    case 3:
                        Console.Clear();
                        Console.WriteLine("Сортировка по видам транспорта: ");
                        Console.WriteLine(new string('-', 50));
                        for (int i = 0; i < ActiveOrders.Count; i++)
                        {
                            for (int j = 0; j < ActiveOrders.Count; j++)
                            {
                                if (String.Compare(ActiveOrders[i].TransportType, ActiveOrders[j].TransportType) == -1)
                                {
                                    tmp = ActiveOrders[i];
                                    ActiveOrders[i] = ActiveOrders[j];
                                    ActiveOrders[j] = tmp;
                                }
                            }
                        }        // Соритировка
                        for (int i = 0; i < ActiveOrders.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}: " + ActiveOrders[i].Name);
                            Console.WriteLine(new string('-', 50));
                        }        // Вывод
                        break;
                    case 4:
                        Console.Clear();
                        Console.WriteLine("Сортировка по стоимости перевозки: ");
                        Console.WriteLine(new string('-', 50));
                        for (int i = 0; i < ActiveOrders.Count; i++)
                        {
                            for (int j = 0; j < ActiveOrders.Count; j++)
                            {
                                if (ActiveOrders[i].Price > ActiveOrders[j].Price)
                                {
                                    tmp = ActiveOrders[i];
                                    ActiveOrders[i] = ActiveOrders[j];
                                    ActiveOrders[j] = tmp;
                                }
                            }
                        }        // Соритировка
                        for (int i = 0; i < ActiveOrders.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}: " + ActiveOrders[i].Name);
                            Console.WriteLine(new string('-', 50));
                        }        // Вывод
                        break;

                }
            }
            void Command2()
            {
                Console.WriteLine("Всего задержанных заказов: " + DelayedOrders.Count);
                for (int i = 0; i < DelayedOrders.Count; i++)
                {
                    Console.WriteLine(new string('-', 50));
                    Console.WriteLine($"{i + 1}: " + DelayedOrders[i].Name);
                }
            }
            void Command3()
            {
                int sum;
                Console.WriteLine("Доходы: ");
                Console.WriteLine("Всего: " + TotalIncome);

                Console.WriteLine(new string('-', 50));

                Console.WriteLine("По виду транспорта: ");
                sum = 0;
                for (int i = 0; i < Transports.Length; i++)
                {
                    Console.WriteLine(Transports[i].Name + ":  " + Transports[i].Income);
                    sum += Transports[i].Income;
                }
                Console.WriteLine("\nВсего: " + sum);

                Console.WriteLine(new string('-', 50));

                Console.WriteLine("По городам: ");
                sum = 0;
                for (int i = 0; i < Branches.Length; i++)
                {
                    Console.WriteLine(Branches[i].CityName + ":  " + Branches[i].Income);
                    sum += Branches[i].Income;
                }
                Console.WriteLine("\nВсего: " + sum);
                Console.WriteLine(new string('-', 50));
            }
            void Command4()
            {
                Console.WriteLine(new string('-', 50));
                Console.WriteLine("Потери изза погоды: " + BadWeatherLoses);
                Console.WriteLine(new string('-', 50));
            }
            void Command5()
            {
                Console.WriteLine(new string('-', 50));
                Console.WriteLine("Потери изза аварий: " + TotalCrashLoses);
                Console.WriteLine(new string('-', 50));
                Console.WriteLine("По виду транспорта: ");
                for (int i = 0; i < Transports.Length; i++)
                {
                    if (i == 0)
                    {
                        Console.WriteLine(Transports[i].Name + ":  \t" + Transports[i].CrashLoses);
                        continue;
                    }
                    else
                    {
                        Console.WriteLine(Transports[i].Name + ":  \t" + Transports[i].CrashLoses + "\n " +
                                         "На магистралях:     " + Transports[i].HighwayCrashLoses + "\n " +
                                         "На обычных дорогах: " + (Transports[i].CrashLoses - Transports[i].HighwayCrashLoses));
                    }
                }
                Console.WriteLine(new string('-', 50));
            }
            void Command6()
            {
                double n;
                Console.WriteLine("Введите команду: \n" +
                                  " 1 - среднее время доставки груза; \n" +
                                  " 2 - среднее время доставки груза с разбивкой по видам транспорта \n" +
                                  " 3 - среднее время доставки груза с разбивкой по городам");
                int cmd = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine(new string('-', 50));
                switch (cmd)
                {
                    case 1:
                        n = 0;
                        for (int i = 0; i < elapsedTime.Count; i++)
                        {
                            n += elapsedTime[i];
                        }
                        if (n != 0)
                            n /= elapsedTime.Count;
                        Console.Write("Среднее время доставки: " + n + " часов");
                        break;
                    case 2:
                        for (int i = 0; i < transports.Length; i++)
                        {
                            n = 0;
                            for (int j = 0; j < transports[i].ElapsedTime.Count; j++)
                            {
                                n += transports[i].ElapsedTime[j];
                            }
                            if (n != 0)
                                n /= transports[i].ElapsedTime.Count;
                            Console.WriteLine("Среднее время доставки с разбивкой по видам транспорта:");
                            Console.WriteLine(transports[i].Name + ": " + n + " часов");
                        }
                        break;
                    case 3:
                        for (int i = 0; i < transports.Length; i++)
                        {
                            n = 0;
                            for (int j = 0; j < branches[i].ElapsedTime.Count; j++)
                            {
                                n += branches[i].ElapsedTime[j];
                            }
                            if (n != 0)
                                n /= branches[i].ElapsedTime.Count;
                            Console.WriteLine("Среднее время доставки с разбивкой по городам:");
                            Console.WriteLine(branches[i].CityName + ": " + n + " часов");
                        }
                        break;
                }
            }
            void Command7()
            {
                Console.WriteLine("Доход на тонно-километр: ");
                Console.WriteLine(new string('-', 50));
                Console.WriteLine("Скоростные магистрали: " + (totalIncome / (totalHighwayDistance * totalHighwayWeight)));
                Console.WriteLine(new string('-', 50));
                Console.WriteLine("Обычные дороги: " + (totalIncome / (totalHighwayDistance * totalHighwayWeight)));
                Console.WriteLine(new string('-', 50));
            }
        }
    }

}
