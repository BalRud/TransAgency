using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Branch
    {
        string cityName;
        int citySize;
        int income;
        int distance;
        List<int> elapsedTime = new List<int>();


        public string CityName { get { return cityName; } }
        public int CitySize { get { return citySize; } }
        public int Income
        {
            set { income = value; }
            get { return income; }
        }
        public int Distance { get { return distance; } }
        public List<int> ElapsedTime { get { return elapsedTime; } }        


        public Branch(string cityName, int citySize, int distance)                            // Конструктор
        {
            this.cityName = cityName;
            this.citySize = citySize;
            this.distance = distance;
        }

        int CarHighwayCheck(Branch branch)                                                    // Проверка наличия авто магистрали
        {
            if (this.cityName == "Киев" && branch.CityName == "Харьков" || this.cityName == "Харьков" && branch.CityName == "Киев"
             || this.cityName == "Киев" && branch.CityName == "Львов" || this.cityName == "Львов" && branch.CityName == "Киев"
             || this.cityName == "Москва" && branch.CityName == "Севастополь" || this.cityName == "Севастополь" && branch.CityName == "Москва"
             || this.cityName == "Одесса" && branch.CityName == "Севастополь" || this.cityName == "Севастополь" && branch.CityName == "Одесса")
            {
                return 2;
            }
            return 1;
        }
        int TrainHighwayCheck(Branch branch)                                                  // Проверка наличия ж/д магистрали
        {
            if (this.cityName == "Киев" && branch.CityName == "Севастополь" || this.cityName == "Севастополь" && branch.CityName == "Киев"
             || this.cityName == "Киев" && branch.CityName == "Москва" || this.cityName == "Москва" && branch.CityName == "Киев"
             || this.cityName == "Донецк" && branch.CityName == "Москва" || this.cityName == "Москва" && branch.CityName == "Донецк")
            {
                return 2;
            }
            return 1;
        }
        int PlaneCheck(Branch branch)                                                         // Проверка на возможность доставки самолетом
        {
            if (this.citySize == 3 && this.citySize == branch.CitySize)
            {
                return 0;
            }
            return 1;
        }
        bool TrainCheck(Branch branch)                                                        // Проверка на возможность доставки поездом
        {
            if (this.citySize >= 2 && branch.CitySize >= 2)
            {
                return true;
            }
            return false;
        }

        public void NewOrder(Branch branch, int weight, int distance, int speed = -1, int price = -1)       // Заказ на перевозку, и подбор типа доставки
        {
            bool highway = false;
            int n = PlaneCheck(branch);
            int speedMult;

            if (price != -1)
            {
                speedMult = 1;
                for (int i = n; i < Simulation.Agency.Transports.Length; i++)
                {
                    if (i == 1)
                    {
                        if (!TrainCheck(branch))
                        {
                            continue;
                        }
                        speedMult = TrainHighwayCheck(branch);
                    }
                    else if (i == 2)
                    {
                        speedMult = CarHighwayCheck(branch);
                    }

                    if (price <= (Simulation.Agency.Transports[i].PricePerWeight))
                    {
                        //Создать заказ
                        if (speedMult == 2)
                            highway = true;
                        Simulation.Agency.NewOrders.Add(new Order(this.cityName, branch.CityName, weight, i, Simulation.Agency.Transports, highway, distance));
                        Simulation.Agency.OrderCount++;
                        Simulation.Agency.NewOrders.Remove(Simulation.Agency.NewOrders[Simulation.Agency.NewOrders.Count-1]);
                        return;
                    }
                }
            }               // С пожеланием о цене


            else if (speed != -1)
            {
                speedMult = 1;
                for (int i = n; i < Simulation.Agency.Transports.Length; i++)
                {
                    if (i == 1)
                    {
                        if (!TrainCheck(branch))
                        {
                            continue;
                        }
                        speedMult = TrainHighwayCheck(branch);
                    }
                    else if (i == 2)
                    {
                        speedMult = CarHighwayCheck(branch);
                    }

                    if (speed <= (Simulation.Agency.Transports[i].Speed * speedMult))
                    {
                        //Создать заказ
                        if (speedMult == 2)
                            highway = true;
                        Simulation.Agency.NewOrders.Add(new Order(this.cityName, branch.CityName, weight, i, Simulation.Agency.Transports, highway, distance));
                        Simulation.Agency.OrderCount++;
                        Simulation.Agency.NewOrders.Remove(Simulation.Agency.NewOrders[Simulation.Agency.NewOrders.Count - 1]);
                        return;
                    }
                }
            }          // С пожеланием о скорости


            // Без пожеланий, или если не удалось подобрать тип доставки по пожеланиям
            speedMult = 1;
            for (int i = n; i < Simulation.Agency.Transports.Length; i++)
            {
                if (i == 1)
                {
                    if (!TrainCheck(branch))
                    {
                        continue;
                    }
                    speedMult = TrainHighwayCheck(branch);
                }
                else if (i == 2)
                {
                    speedMult = CarHighwayCheck(branch);
                }
                //Создать заказ
                if (speedMult == 2)
                    highway = true;
                Simulation.Agency.NewOrders.Add(new Order(this.cityName, branch.CityName, weight, i, Simulation.Agency.Transports, highway, distance));
                Simulation.Agency.OrderCount++;
                Simulation.Agency.NewOrders.Remove(Simulation.Agency.NewOrders[Simulation.Agency.NewOrders.Count - 1]);
                break;
            }
        }
    }
}
