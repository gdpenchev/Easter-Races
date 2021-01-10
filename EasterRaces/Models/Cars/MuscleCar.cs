using EasterRaces.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasterRaces.Models.Cars
{
    public class MuscleCar : Car
    {
        private const double CUBIC_CENTIMETERS = 5000;
        private const int MIN_HOURSE_POWER = 400;
        private const int MAX_HOURSE_POWER = 600;
        private int horsePower;
        public MuscleCar(string model, int horsePower) : base(model, horsePower, CUBIC_CENTIMETERS, MIN_HOURSE_POWER, MAX_HOURSE_POWER)
        {
        }

        public override int HorsePower
        {
            get
            {
                return this.horsePower;
            }
            protected set
            {
                if (value < MIN_HOURSE_POWER || value > MAX_HOURSE_POWER)
                {
                    string msg = String.Format(ExceptionMessages.InvalidHorsePower, value); // check this exception message

                    throw new ArgumentException(msg);
                }
                this.horsePower = value;

            }
        }

    }
}
