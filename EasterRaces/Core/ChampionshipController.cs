using EasterRaces.Core.Contracts;
using EasterRaces.Models.Cars;
using EasterRaces.Models.Cars.Contracts;
using EasterRaces.Models.Drivers;
using EasterRaces.Models.Drivers.Contracts;
using EasterRaces.Models.Races;
using EasterRaces.Models.Races.Contracts;
using EasterRaces.Repositories;
using EasterRaces.Repositories.Contracts;
using EasterRaces.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasterRaces.Core
{
    public class ChampionshipController : IChampionshipController
    {
        private readonly IRepository<IDriver> drivers;
        private readonly IRepository<ICar> cars;
        private readonly IRepository<IRace> races;

        public ChampionshipController()
        {
            this.drivers = new DriverRepository();
            this.cars = new CarRepository();
            this.races = new RaceRepository();
        }
        
        public string AddCarToDriver(string driverName, string carModel)
        {
            IDriver driver  = this.drivers.GetByName(driverName);
            ICar car = this.cars.GetByName(carModel);

            if (driver == null)
            {
                string msg1 = String.Format(ExceptionMessages.DriverNotFound, driverName);
                throw new InvalidOperationException(msg1);
            }
            if (car == null)
            {
                string msg1 = String.Format(ExceptionMessages.CarNotFound, carModel);
                throw new InvalidOperationException(msg1);
            }
            driver.AddCar(car);
            string msg = String.Format(OutputMessages.CarAdded, driverName, carModel);
            return msg;

        }

        public string AddDriverToRace(string raceName, string driverName)
        {
            IDriver driver = this.drivers.GetByName(driverName);
            IRace race = this.races.GetByName(raceName);

            if (race == null)
            {
                string msg1 = String.Format(ExceptionMessages.RaceNotFound, raceName);
                throw new InvalidOperationException(msg1);
            }
            if (driver == null)
            {
                string msg1 = String.Format(ExceptionMessages.DriverNotFound, driverName);
                throw new InvalidOperationException(msg1);
            }
            race.AddDriver(driver);
            string msg = String.Format(OutputMessages.DriverAdded, driverName, raceName);
            return msg;
        }

        public string CreateCar(string type, string model, int horsePower)
        {
            ICar car = this.cars.GetByName(model);

            if (car != null)
            {
                string msg1 = String.Format(ExceptionMessages.CarExists, model);
                throw new ArgumentException(msg1);
            }

            if (type == "Muscle")
            {
                car = new MuscleCar(model, horsePower);
            }
            else if (type == "Sports")
            {
                car = new SportsCar(model, horsePower);
            }
            this.cars.Add(car);
            string msg = String.Format(OutputMessages.CarCreated, car.GetType().Name,model);
            return msg;
        }

        public string CreateDriver(string driverName)
        {
            IDriver driver = this.drivers.GetByName(driverName);

            if (driver != null)
            {

                string msg1 = String.Format(ExceptionMessages.DriversExists, driverName);
                throw new ArgumentException(msg1);
            }
            driver = new Driver(driverName);
            this.drivers.Add(driver);
            string msg = String.Format(OutputMessages.DriverCreated, driverName);
            return msg;

        }

        public string CreateRace(string name, int laps)
        {
            IRace race = this.races.GetByName(name);

            if (race != null)
            {
                string msg1 = String.Format(ExceptionMessages.RaceExists, name);
                throw new InvalidOperationException(msg1);
            }
            race = new Race(name, laps);
            
            this.races.Add(race);
            string msg = String.Format(OutputMessages.RaceCreated, name);
            return msg;
        }

        public string StartRace(string raceName)
        {
            IRace race = this.races.GetByName(raceName);

            if (race == null)
            {
                string msg1 = String.Format(ExceptionMessages.RaceNotFound, raceName);
                throw new InvalidOperationException(msg1);
            }
            if (race.Drivers.Count < 3)
            {
                string msg1 = String.Format(ExceptionMessages.RaceInvalid, raceName, 3);
                throw new InvalidOperationException(msg1);
            }

            List<IDriver> drivers = race.Drivers.OrderByDescending(d => d.Car.CalculateRacePoints(race.Laps)).ToList();

            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Driver {drivers[0].Name} wins {raceName} race.");
            sb.AppendLine($"Driver {drivers[1].Name} is second in {raceName} race.");
            sb.AppendLine($"Driver {drivers[2].Name} is third in {raceName} race.");

            return sb.ToString().Trim();
        }
    }
}
