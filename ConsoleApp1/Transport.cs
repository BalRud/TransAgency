using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Transport
    {
        int speed;
        int pricePerWeight;
        int income;
        int crashRate;
        string name;
        int crashLoses;
        int highwayCrashLoses;
        List<int> elapsedTime;


        public int PricePerWeight { get { return pricePerWeight; } }
        public int Speed { get { return speed; } }
        public int Income
        {
            set { this.income = value; }
            get { return this.income; }
        }
        public int CrashRate { get { return crashRate; } }
        public string Name { get { return name; } }
        public int CrashLoses
        {
            set { this.crashLoses = value; }
            get { return this.crashLoses; }
        }
        public int HighwayCrashLoses
        {
            set { this.highwayCrashLoses = value; }
            get { return this.highwayCrashLoses; }
        }
        public List<int> ElapsedTime { get { return elapsedTime; } }


        public Transport() { }
        public Transport(int speed, int pricePerWeight, int crashRate, string name)
        {
            this.elapsedTime = new List<int>();
            this.speed = speed;
            this.pricePerWeight = pricePerWeight;
            this.crashRate = crashRate;
            this.name = name;
        }
    }

    class Plane : Transport
    {
        public Plane()
            :base(8,10,65,"Plane")
        {
        }
    }

    class Train : Transport
    {
        public Train()
            :base(5,2,45,"Train")
        {
        }
    }

    class Car : Transport
    {
        public Car()
            :base(3,5,40,"Car")
        {
        }
    }
}
