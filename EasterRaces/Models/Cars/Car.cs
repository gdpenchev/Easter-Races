using EasterRaces.Models.Cars.Contracts;
using EasterRaces.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasterRaces.Models.Cars
{
    public abstract class Car : ICar
    {
        private string model;
        private int horsePower;
        private double cubicCentimeters;

        protected Car(string model, int horsePower, double cubicCentimeters, int minHorsePower, int maxHorsePower)
        {
            this.Model = model;
            
            this.CubicCentimeters = cubicCentimeters;
        }

        public string Model 
        {
            get 
            {
                return this.model;
            }
            private set 
            {
                if (string.IsNullOrWhiteSpace(value) || value.Length < 4)
                {
                    string msg = String.Format(ExceptionMessages.InvalidModel, this.model, 4); // check this exception message

                    throw new ArgumentException(msg);
                }
                this.model = value;
            }
        }
        public virtual int HorsePower { get; protected set; }

        public double CubicCentimeters { get; private set; }

        public double CalculateRacePoints(int laps)
        {
            double res = this.CubicCentimeters / this.HorsePower * laps;

            return res;
        }

    }
}
