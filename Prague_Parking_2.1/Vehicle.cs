using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Prague_Parking_2._1
{
    public class Vehicle
    {
        private string _regNumber;
        private DateTime _checkIn;
        public string VehicleIdentifier { get; init; } //typen är INTE till för att definiera vilken typ som ska läggas till, utan har bara en typ för att göra en klarare utskrift
        public int Size { get; init; }

        public Vehicle(string regNum)
        {
            RegNumber = regNum;
            CheckIn = DateTime.Now;
        }

        public string RegNumber
        {
            get => _regNumber; 
            init => _regNumber = value; 
        }                               
        public DateTime CheckIn
        {
            get => _checkIn;
            init => _checkIn = value;
        }

        /// <summary>
        /// check if a regnumber is valid, meaning only capital letters, followed by numbers
        /// all within the range of 1-10
        /// </summary>
        /// <param name="regNr"></param>
        /// <returns></returns>
        public static bool IsValidRegNum(string regNr) 
        {
            Regex input = new Regex("[A-Z][1-9]{1,10}");
            Match validation = input.Match(regNr);

            return validation.Success;
        }

        public override string ToString() //en override strängmetod för fordon ska "skriva ut sig själv" med regnummer
        {
            return $"{VehicleIdentifier} : {RegNumber}";
        }
    }
}
