using System;
using System.Collections.Concurrent;
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
    static List<Task> tasks = new List<Task>();

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
        public float Cost { get; set; }
    }

    public class Date
    {
        public int Week { get; set; }
        public int Year { get; set; }
    }

    public static List<Order> GetStoreOrderData(List<string> filePaths)
    {
        HashSet<Date> dates = new HashSet<Date>();
        List<Order> orders = new List<Order>();

        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();

        foreach (string filePath in filePaths)
        {
            string fileNameExt = Path.GetFileName(filePath.ToString());
            string fileName = Path.GetFileNameWithoutExtension(filePath.ToString());

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
                    Cost = float.Parse(orderSplit[2])
                };
                orders.Add(order);
                //orderSplit[0] = supplier name
                //orderSplit[1] = supplier type
                //orderSplit[2] = cost
            }
        }

        stopWatch.Stop();
        Console.WriteLine("Time To Load: " + stopWatch.Elapsed.TotalSeconds);

        filePaths.Clear();
        return orders;
    }

    public static List<string> FindAllFilePathsWithCode(string storeCode)
    {
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();

        //find all files with store code
        List<string> temp = Directory.GetFiles(folderPath + @"\" + storeDataFolder).ToList<string>();
        List<string> fileNames = new List<string>();

        foreach (string name in temp)
        {
            if (name.ToString().Contains(storeCode))
            {
                fileNames.Add(Path.GetFullPath(name.ToString()));
            }
        }

        temp.Clear(); //clear data

        stopWatch.Stop();
        Console.WriteLine("Time To Load: " + stopWatch.Elapsed.TotalSeconds);

        return fileNames;
    }

    public static Dictionary<string, Store> GetStoreData()
    {
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();

        stores.Clear();

        string storeCodesFilePath = Directory.GetCurrentDirectory() + @"\" + folderPath + @"\" + storeCodesFile;
        string[] storeCodesData = File.ReadAllLines(storeCodesFilePath);

        foreach (var storeData in storeCodesData)
        {
            //storeDataSplit[0] = store code, storeDataSplit[1] = store location
            string[] storeDataSplit = storeData.ToString().Split(',');
            Store store = new Store { StoreCode = storeDataSplit[0], StoreLocation = storeDataSplit[1] };

            if (!stores.ContainsKey(store.StoreCode))
                stores.Add(store.StoreCode, store);
        }

        stopWatch.Stop();
        Console.WriteLine("Load store names and location time: " + stopWatch.Elapsed.TotalSeconds);

        return stores;
    }

    public static List<Order> GetFileOrders(string filename)
    {
        return GetStoreOrderData( new List<string>{ filename } );
    }

    //The total cost of all orders available in the supplied data
    public static float GetTotalOrderCost()
    {
        List<Order> orders = GetStoreOrderData(Directory.GetFiles(folderPath + @"\" + storeDataFolder).ToList<string>());

        float totalCost = 0.0f;

        foreach (Order order in orders)
        {
            totalCost += order.Cost;
        }

        orders.Clear();

        return totalCost;
    }

    //The total cost of all orders for a single store
    public static float GetStoreTotalOrderCost(string storeCode)
    {
        List<Order> orders = GetStoreOrderData(FindAllFilePathsWithCode(storeCode));

        float totalCost = 0.0f;

        foreach(Order order in orders)
        {
            totalCost += order.Cost;
        }

        orders.Clear();

        return totalCost;
    }

    //The total cost of orders in a week for all stores
    public static float GetWeeklyOrderCost()
    {
        return 0.0f;
    }

    //The total cost of orders in a week for a single store
    public static float GetStoreWeeklyOrderCost(string filename)
    {
        float totalCost = 0.0f;
        List<Order> orders = GetFileOrders(filename);

        foreach(Order order in orders)
        {
            totalCost += order.Cost;
        }

        orders.Clear();

        return totalCost;
    }

    //The total cost of all orders to a supplier
    public static float GetSupplierOrderCost(string supplier)
    {
        return 0.0f;
    }
}