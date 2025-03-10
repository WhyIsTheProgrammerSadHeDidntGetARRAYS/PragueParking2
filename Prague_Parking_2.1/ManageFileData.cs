﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace Prague_Parking_2._1
{
    public static class ManageFileData
    {
        private const string ParkingListPath = @"../../../Datafiles/Parkinglist.json";

        /// <summary>
        /// reads the parkinglistfile and returns it as POCO (Plain old CLR object)
        /// </summary>
        /// <returns></returns>
        public static List<ParkingSpot> ReadParkinglist()//returns file as a non-json list of objects //TODO: handle exceptions
        {
            try
            {
                string temp = File.ReadAllText(ParkingListPath);
                var tempList = JsonConvert.DeserializeObject<List<ParkingSpot>>(temp);
                return tempList;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }
        /// <summary>
        /// Updates the parkinglist, by writing the changes to the file
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void UpdateParkingList<T>(List<T> list)
        {
            string parkingHouseString = JsonConvert.SerializeObject(list, Formatting.Indented);
            File.WriteAllText(ParkingListPath, parkingHouseString);
        }
    }
}
