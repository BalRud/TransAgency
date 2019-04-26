using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Order : IDisposable
    {
        int speed;
        int price;
        int crashRate;
        string departureCity;
        string destinationCity;
        string transportType;
        string name;
        bool highway;
        int transportIndex;
        int cityIndex;
        int elapsedTime;
        Random r = new Random();
        int distance;
        int weight;
        int deliveringDistance;


        public string Name { get { return name; } }
        public string DepartureCity { get { return departureCity; } }
        public string DestinationCity { get { return destinationCity; } }
        public string TransportType { get { return transportType; } }
        public int Price { get { return price; } }
        public int ElapsedTime { get { return elapsedTime; } }
        public int Distance { get { return distance; } }
        public int DeliveringDistance { get { return deliveringDistance; } }
        public int Weight { get { return weight; } }
        public bool Highway { get { return highway; } }


        // Конструктор задержанных заказов
        public Order(int speed, int price, int crashRate, string departureCity, string destinationCity,
                                string transportType, string whyDelay, int weight, int transportIndex, int cityIndex, bool highway, int distance)
        {
            this.speed = speed;
            this.price = price;
            this.crashRate = crashRate;
            this.departureCity = departureCity;
            this.destinationCity = destinationCity;
            this.transportType = transportType;
            this.transportIndex = transportIndex;
            this.cityIndex = cityIndex;
            this.distance = distance;
            this.deliveringDistance = distance;
            this.weight = weight;
            this.highway = highway;

            this.name = $"{departureCity} => {destinationCity}; \n Стоимость: {this.price}; \n Тип: {this.transportType}; \n {whyDelay}";
        }

        // Конструктор для активных заказов
        public Order(int speed, int price, int crashRate, string departureCity, string destinationCity,
                        string transportType, int weight, int transportIndex, int cityIndex, bool highway, int distance, int elapsedTime = 0)
        {
            this.speed = speed;
            this.price = price;
            this.crashRate = crashRate;
            this.departureCity = departureCity;
            this.destinationCity = destinationCity;
            this.transportType = transportType;
            this.transportIndex = transportIndex;
            this.cityIndex = cityIndex;
            this.distance = distance;
            this.deliveringDistance = distance;
            this.elapsedTime = elapsedTime;
            this.weight = weight;
            this.highway = highway;

            this.name = $"{departureCity} => {destinationCity}; \n Стоимость: {this.price}; \n Тип: {this.transportType};";
        }

        // Контсруктор новых заказов
        public Order(string departureCity, string destinationCity, int weight, int transportType, Transport[] transports, bool highway, int distance)
        {
            this.departureCity = departureCity;
            this.destinationCity = destinationCity;
            this.highway = highway;
            this.transportIndex = transportType;
            this.distance = distance;
            this.deliveringDistance = distance;
            this.weight = weight;
            switch (transportType)
            {
                case 0:
                    this.speed = transports[0].Speed;
                    this.price = (transports[0].PricePerWeight * weight) * distance;
                    this.crashRate = transports[0].CrashRate;
                    this.transportType = "Plane";
                    break;
                case 1:
                    this.speed = transports[1].Speed * (highway ? 2 : 1);
                    this.price = (transports[1].PricePerWeight * weight) * distance;
                    this.crashRate = transports[1].CrashRate * (highway ? 2 : 1);
                    this.transportType = "Train";
                    break;
                case 2:
                    this.speed = transports[2].Speed * (highway ? 2 : 1);
                    this.price = (transports[2].PricePerWeight * weight) * distance;
                    this.crashRate = transports[2].CrashRate * (highway ? 2 : 1);
                    this.transportType = "Car";
                    break;
            }

            for (int i = 0; i < Simulation.Agency.Branches.Length; i++)
            {
                if (departureCity == Simulation.Agency.Branches[i].CityName)
                {
                    this.cityIndex = i;
                }
            }

            this.name = $"{departureCity} => {destinationCity}; \n Стоимость: {this.price}; \n Тип: {this.transportType}; \n Скорость: {this.speed}";

            SortOrders();
        }


        void SortOrders()                                              // Перемещение заказов в список активных или задержанных
        {

            // Проверка погоды
            if (transportType == "Plane")
            {
                // 0 - Storm weather; 1,2 - Clear;
                int departureCityWeather = r.Next(0, 3);
                int destinationCityWeather = r.Next(0, 3);

                if (departureCityWeather == 0 || destinationCityWeather == 0)
                {
                    string whyDelay;
                    if (departureCityWeather == 0)
                        whyDelay = $"Задержан изза плохой погоды в городе {departureCity}";
                    else if (destinationCityWeather == 0)
                        whyDelay = $"Задержан изза плохой погоды в городе {destinationCity}";
                    else
                        whyDelay = $"Задержан изза плохой погоды в городах {departureCity} и {destinationCity}";
                    Simulation.Agency.DelayedOrders.Add(new Order(speed, price, crashRate, departureCity, destinationCity,
                                                                    transportType, whyDelay, weight, transportIndex, cityIndex, highway, distance));
                    Simulation.Agency.BadWeatherLoses += price;
                    this.Dispose();
                    return;
                }
            }
            Simulation.Agency.ActiveOrders.Add(new Order(speed, price, crashRate, departureCity, destinationCity,
                                            transportType, weight, transportIndex, cityIndex, highway, distance));
            this.Dispose();
        }

        public bool WeatherCheck()                                     // Проверка погоды и перемещение задержанных заказов в активные
        {
            // 0 - Storm weather; 1,2 - Clear;
            int departureCityWeather = r.Next(0, 3);
            int destinationCityWeather = r.Next(0, 3);

            if (departureCityWeather == 0 || destinationCityWeather == 0)
            {
                elapsedTime++;
                return false;
            }
            else
            {
                Simulation.Agency.ActiveOrders.Add(new Order(speed, price, crashRate, departureCity, destinationCity,
                                                transportType, weight, transportIndex, cityIndex, highway, distance, elapsedTime));
                Simulation.Agency.BadWeatherLoses -= price;
                return true;
                //Dispose();
            }
        }

        public string OrderInProgress()                                // Процесс доставки
        {
            if (r.Next(0, crashRate) == 0)
            {
                // Возврат денег
                Simulation.Agency.TotalIncome -= this.price * 2;
                Simulation.Agency.Transports[transportIndex].Income -= this.price * 2;
                Simulation.Agency.Branches[cityIndex].Income -= this.price * 2;

                Simulation.Agency.TotalCrashLoses += this.price * 2;
                Simulation.Agency.Transports[transportIndex].CrashLoses += this.price * 2;
                if (highway && transportIndex != 0)
                    Simulation.Agency.Transports[transportIndex].HighwayCrashLoses += this.price * 2;
                return "Accident";
            }                          // Проверка на аварию во время доставки и возврат денег в случае аварии

            else
            {
                distance -= speed;
                elapsedTime++;
            }

            if (distance <= 0)
            {
                // Получение денег
                Simulation.Agency.TotalIncome += this.price;
                Simulation.Agency.Transports[transportIndex].Income += this.price;
                Simulation.Agency.Branches[cityIndex].Income += this.price;

                // Учет затраченного на доставку времени
                Simulation.Agency.Transports[transportIndex].ElapsedTime.Add(elapsedTime);
                Simulation.Agency.Branches[cityIndex].ElapsedTime.Add(elapsedTime);
                Simulation.Agency.ElapsedTime.Add(elapsedTime);

                return "Delivered";
            }                                // Груз доставлен, получение оплаты
            return "Still in progress";
        }



        // Деструктор
        public void Dispose()
        {
            //Console.WriteLine("Заказ уничтожен!");
        }
    }
}
