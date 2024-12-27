using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace AirlineReservationSystem
{
    public static class FileHandler<T>
    {
        public static List<T> LoadData(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    string jsonData = File.ReadAllText(filePath);
                    return JsonConvert.DeserializeObject<List<T>>(jsonData) ?? new List<T>();
                }
                else
                {
                    Console.WriteLine($"Error: File '{filePath}' not found.");
                    return new List<T>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading data from file: {ex.Message}");
                return new List<T>();
            }
        }

        public static void SaveData(string filePath, List<T> data)
        {
            try
            {
                string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);
                File.WriteAllText(filePath, jsonData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving data to file: {ex.Message}");
            }
        }
    }
}
