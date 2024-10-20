using Library.Models;
using System.Xml.Serialization;

namespace Library.Servises
{
        public static class DataStorage
        {
            // Шлях до файлу, де зберігаються дані
            private static readonly string filePath = "rentals_data.xml";

            // Метод для серіалізації списку оренд у XML-файл
            public static void SaveToFile(List<Rental> rentals)
            {
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Rental>));
                    using (StreamWriter writer = new StreamWriter(filePath))
                    {
                        serializer.Serialize(writer, rentals);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error saving data to file: {e.Message}");
                }
            }

            // Метод для десеріалізації списку оренд із XML-файлу
            public static List<Rental> LoadFromFile()
            {
                try
                {
                    if (!File.Exists(filePath))
                    {
                        // Якщо файл не існує, повертаємо порожній список
                        return new List<Rental>();
                    }

                    XmlSerializer serializer = new XmlSerializer(typeof(List<Rental>));
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        return (List<Rental>)serializer.Deserialize(reader);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error loading data from file: {e.Message}");
                    return new List<Rental>(); // Повертаємо порожній список у разі помилки
                }
            }
        }
    
}
