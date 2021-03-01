using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class CSVLoader
{
    static string folderPath = "StoreData";
    static string storeCodesFile = "StoreCodes.csv";
    static string storeDataFolder = "StoreData";

    static Dictionary<string, Store> stores = new Dictionary<string, Store>();

    public class Store
    {
        public string StoreCode { get; set; }
        public string StoreLocation { get; set; }
    }

    public class Order
    {
        public Store Store { get; set; }

        public Date Date { get; set; }
        public string SupplierName { get; set; }
        public string SupplierType { get; set; }
        public double Cost { get; set; }
    }

    public class Date
    {
        public int Week { get; set; }
        public int Year { get; set; }
    }

    public static List<string> StoreData(string storeCode)
    {
        HashSet<Date> dates = new HashSet<Date>();
        List<Order> orders = new List<Order>();

        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();

        List<string> temp = Directory.GetFiles(folderPath + @"\" + storeDataFolder).ToList<string>();
        List<string> filePaths = new List<string>();
        List<string> fileNames = new List<string>();

        foreach (string name in temp)
        {
            if(name.Contains(storeCode))
            {
                filePaths.Add(name);
            }
        }
        temp.Clear(); //clear data

        foreach (var filePath in filePaths)
        {
            string fileNameExt = Path.GetFileName(filePath);
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            fileNames.Add(fileName);

            string[] fileNameSplit = fileName.Split('_');
            Store store = stores[fileNameSplit[0]];
            Date date = new Date { Week = Convert.ToInt32(fileNameSplit[1]), Year = Convert.ToInt32(fileNameSplit[2]) };
            dates.Add(date);
            //fileNameSplit[0] = store code
            //fileNameSplit[1] = week number
            //fileNameSplit[2] = year

            string[] orderData = File.ReadAllLines(folderPath + @"\" + storeDataFolder + @"\" + fileNameExt);
            foreach (var orderInfo in orderData)
            {
                string[] orderSplit = orderInfo.Split(',');
                Order order = new Order
                {
                    Store = store,
                    Date = date,
                    SupplierName = orderSplit[0],
                    SupplierType = orderSplit[1],
                    Cost = Convert.ToDouble(orderSplit[2])
                };
                orders.Add(order);
                //orderSplit[0] = supplier name
                //orderSplit[1] = supplier type
                //orderSplit[2] = cost
            }
        }

        stopWatch.Stop();
        Console.WriteLine("TimeToLoad: " + stopWatch.Elapsed.TotalSeconds);

        return fileNames;
    }

    public static Dictionary<string, Store> LoadStores()
    {
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();

        stores.Clear();

        string storeCodesFilePath = Directory.GetCurrentDirectory() + @"\" + folderPath + @"\" + storeCodesFile;
        string[] storeCodesData = File.ReadAllLines(storeCodesFilePath);
        foreach (var storeData in storeCodesData)
        {
            //storeDataSplit[0] = store code, storeDataSplit[1] = store location
            string[] storeDataSplit = storeData.Split(',');
            Store store = new Store { StoreCode = storeDataSplit[0], StoreLocation = storeDataSplit[1] };

            if (!stores.ContainsKey(store.StoreCode))
                stores.Add(store.StoreCode, store);
        }

        stopWatch.Stop();
        Console.WriteLine("Load store names and location time: " + stopWatch.Elapsed.TotalSeconds);

        return stores;
    }
}