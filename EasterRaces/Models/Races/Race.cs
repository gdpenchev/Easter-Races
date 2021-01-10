using EasterRaces.Models.Drivers.Contracts;
using EasterRaces.Models.Races.Contracts;
using EasterRaces.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasterRaces.Models.Races
{
    public class Race : IRace
    {
        private string name;
        private int laps;
        private readonly List<IDriver> drivers;

        public Race(string name, int laps)
        {
            this.Name = name;
            this.Laps = laps;
            this.drivers = new List<IDriver>();
        }
        
        public string Name
        {
            get
            {
                return this.name;
            }
            private set
            {
                if (string.IsNullOrWhiteSpace(value) || value.Length < 5)
                {
                    string msg = String.Format(ExceptionMessages.InvalidName, this.Name, 5); // check this exception message

                    throw new ArgumentException(msg);
                }
                this.name = value;
            }
        }

        public int Laps
        {
            get
            {
                return this.laps;
            }
            private set
            {
                if (value < 1)
                {

                    string msg = String.Format(ExceptionMessages.InvalidNumberOfLaps, 1);
                    throw new ArgumentException(msg);
                }
                this.laps = value;
            }
        }

        public IReadOnlyCollection<IDriver> Drivers => this.drivers.AsReadOnly();

        public void AddDriver(IDriver driver)
        {
            if (driver == null)
            {
                throw new ArgumentNullException(ExceptionMessages.DriverInvalid);
            }
            else if (driver.CanParticipate == false)
            {
                string msg = String.Format(ExceptionMessages.DriverNotParticipate, driver.Name);
                throw new ArgumentException(msg);
            }
            else if (this.drivers.Contains(driver))
            {
                string msg = String.Format(ExceptionMessages.DriverAlreadyAdded, driver.Name, this.Name);
                throw new ArgumentNullException(msg);
            }
            this.drivers.Add(driver);
        }
    }
}
