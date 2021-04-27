using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

class CSVLoader
{
    static string folderPath = "StoreData";
    static string storeCodesFile = "StoreCodes.csv";
    static string storeDataFolder = "StoreData";

    static ConcurrentDictionary<string, Store> stores = new ConcurrentDictionary<string, Store>();

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
        ConcurrentQueue <Date> dates = new ConcurrentQueue<Date>();
        ConcurrentQueue<Order> orders = new ConcurrentQueue<Order>();

        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();

        Parallel.ForEach(filePaths, filePath => 
        {
            string fileNameExt = Path.GetFileName(filePath.ToString());
            string fileName = Path.GetFileNameWithoutExtension(filePath.ToString());

            string[] fileNameSplit = fileName.Split('_');
            Store store = stores[fileNameSplit[0]];
            Date date = new Date { Week = Convert.ToInt32(fileNameSplit[1]), Year = Convert.ToInt32(fileNameSplit[2]) };
            dates.Enqueue(date);
            //fileNameSplit[0] = store code
            //fileNameSplit[1] = week number
            //fileNameSplit[2] = year

            string[] orderData = File.ReadAllLines(folderPath + @"\" + storeDataFolder + @"\" + fileNameExt);

            Parallel.ForEach(orderData, orderInfo =>
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
                orders.Enqueue(order);
                //orderSplit[0] = supplier name
                //orderSplit[1] = supplier type
                //orderSplit[2] = cost
            });
        });

        stopWatch.Stop();
        Console.WriteLine("[Execution Time]: " + stopWatch.Elapsed.TotalSeconds + " Seconds (Get orders from list of file paths)");

        filePaths.Clear();

        return orders.ToList();
    }

    public static List<string> FindAllFilePathsWithCode(string storeCode)
    {
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();

        //find all files with store code
        List<string> temp = Directory.GetFiles(folderPath + @"\" + storeDataFolder).ToList<string>();
        List<string> fileNames = new List<string>();

        foreach(string path in temp)
        {
            if (path.ToString().Contains(storeCode))
            {
                fileNames.Add(Path.GetFullPath(path.ToString()));
            }
        }

        stopWatch.Stop();
        Console.WriteLine("[Execution Time]: " + stopWatch.Elapsed.TotalSeconds + " Seconds (Get file paths for specific store code)");

        return fileNames;
    }

    public static Dictionary<string, Store> GetStoreData()
    {
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();

        stores.Clear();

        string storeCodesFilePath = Directory.GetCurrentDirectory() + @"\" + folderPath + @"\" + storeCodesFile;
        string[] storeCodesData = File.ReadAllLines(storeCodesFilePath);

        Parallel.ForEach(storeCodesData, storeData =>
        {
            //storeDataSplit[0] = store code, storeDataSplit[1] = store location
            string[] storeDataSplit = storeData.ToString().Split(',');
            Store store = new Store { StoreCode = storeDataSplit[0], StoreLocation = storeDataSplit[1] };

            if (!stores.ContainsKey(store.StoreCode))
                stores.TryAdd(store.StoreCode, store);
        });

        stopWatch.Stop();
        Console.WriteLine("[Execution Time]: " + stopWatch.Elapsed.TotalSeconds + " Seconds (Get all stores from stores csv)");

        return stores.OrderBy(x => x.Key).ToDictionary(s => s.Key, s => s.Value);
    }

    public static List<Order> GetFileOrders(string filename)
    {
        return GetStoreOrderData( new List<string>{ filename } );
    }

    //The total cost of all orders available in the supplied data
    public static float GetTotalOrderCost()
    {
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();

        List<Order> orders = GetStoreOrderData(Directory.GetFiles(folderPath + @"\" + storeDataFolder).ToList<string>());

        float totalCost = 0.0f;

        foreach(Order order in orders)
            totalCost += order.Cost;

        orders.Clear();

        stopWatch.Stop();
        Console.WriteLine("[Execution Time]: " + stopWatch.Elapsed.TotalSeconds + " Seconds (Calculate cost of all orders in data)");

        return totalCost;
    }

    //The total cost of all orders for a single store
    public static float GetStoreTotalOrderCost(string storeCode)
    {
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();

        List<Order> orders = GetStoreOrderData(FindAllFilePathsWithCode(storeCode));

        float totalCost = 0.0f;

        foreach (Order order in orders)
            totalCost += order.Cost;

        orders.Clear();

        stopWatch.Stop();
        Console.WriteLine("[Execution Time]: " + stopWatch.Elapsed.TotalSeconds + " Seconds (Calculate cost of all orders to a single store)");

        return totalCost;
    }

    //The total cost of orders in a week for all stores
    public static float GetWeeklyOrderCost(string week, string year)
    {
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();

        List<string> allPaths = Directory.GetFiles(folderPath + @"\" + storeDataFolder).ToList();
        ConcurrentQueue<string> paths = new ConcurrentQueue<string>();
        string comparison = "_" + week + "_" + year;

        Parallel.ForEach(allPaths, path =>
        {
            if (path.Contains(comparison))
            {
                paths.Enqueue(path);
            }
        });

        allPaths.Clear();

        float totalCost = 0.0f;
        ConcurrentQueue<Order> orders = new ConcurrentQueue<Order>(GetStoreOrderData(paths.ToList()));

        foreach (Order order in orders)
            totalCost += order.Cost;

        stopWatch.Stop();
        Console.WriteLine("[Execution Time]: " + stopWatch.Elapsed.TotalSeconds + " Seconds (Calculate cost of all orders to a single store)");

        return totalCost;
    }

    //The total cost of orders in a week for a single store
    public static float GetStoreWeeklyOrderCost(string filename)
    {
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();

        float totalCost = 0.0f;
        List<Order> orders = GetFileOrders(filename);

        foreach(Order order in orders)
            totalCost += order.Cost;

        orders.Clear();

        stopWatch.Stop();
        Console.WriteLine("[Execution Time]: " + stopWatch.Elapsed.TotalSeconds + " Seconds (Calculate cost of orders to a single store in a week)");

        return totalCost;
    }

    public static float GetStoreWeekTotalOrderCost(string code, string week, string year)
    {
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();

        List<string> fileNames = FindAllFilePathsWithCode(code);
        List<string> finalPaths = new List<string>();
        string comparison = "_" + week + "_" + year;
        float totalCost = 0.0f;

        foreach(string path in fileNames)
        {
            if (path.Contains(comparison))
            {
                finalPaths.Add(path);
            }
        }

        List<Order> orders = GetStoreOrderData(finalPaths.ToList());

        foreach (Order order in orders)
            totalCost += order.Cost;

        stopWatch.Stop();
        Console.WriteLine("[Execution Time]: " + stopWatch.Elapsed.TotalSeconds + " Seconds (Calculate cost of orders to a single store in a specified week)");

        return totalCost;
    }

    public static float GetTotalCostOfSupplierToStore(string code, string type)
    {
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();

        float totalCost = 0.0f;
        List<string> paths = FindAllFilePathsWithCode(code);
        ConcurrentQueue<Order> orders = new ConcurrentQueue<Order>(GetStoreOrderData(paths));

        paths.Clear();

        foreach(Order order in orders)
        {
            if (order.SupplierType == type)
                totalCost += order.Cost;
        }

        stopWatch.Stop();
        Console.WriteLine("[Execution Time]: " + stopWatch.Elapsed.TotalSeconds + " Seconds (Get cost of orders from specific supplier type to a store)");

        return totalCost;
    }

    //
    // Supplier
    //

    //The total cost of all orders to a supplier
    public static float GetSupplierOrderCost(string supplier)
    {
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();

        ConcurrentQueue<Order> orders = new ConcurrentQueue<Order>(GetStoreOrderData(Directory.GetFiles(folderPath + @"\" + storeDataFolder).ToList()));
        List<float> prices = new List<float>();
        float totalCost = 0.0f;

        Parallel.ForEach(orders, order =>
        {
            if (order.SupplierName == supplier)
                prices.Add(order.Cost);
        });

        foreach (Order order in orders)
            totalCost += order.Cost;

        prices.Clear();

        stopWatch.Stop();
        Console.WriteLine("[Execution Time]: " + stopWatch.Elapsed.TotalSeconds + " Seconds (Get the total cost of all orders to a supplier)");

        return totalCost;
    }

    //Get all supplier names
    public static List<string> GetSupplierNames()
    {
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();

        ConcurrentQueue<Order> orders = new ConcurrentQueue<Order>(GetStoreOrderData(Directory.GetFiles(folderPath + @"\" + storeDataFolder).ToList()));
        List<string> names = new List<string>();

        Parallel.ForEach(orders, order =>
        {
            if (!names.Contains(order.SupplierName))
                names.Add(order.SupplierName);
        });

        stopWatch.Stop();
        Console.WriteLine("[Execution Time]: " + stopWatch.Elapsed.TotalSeconds + " Seconds (Get all supplier names)");

        return names;
    }

    //Get all supplier types
    public static List<string> GetSupplierTypes()
    {
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();

        List<Order> orders = GetStoreOrderData(Directory.GetFiles(folderPath + @"\" + storeDataFolder).ToList());
        List<string> types = new List<string>();

        Parallel.ForEach(orders, order =>
        {
            if (!types.Contains(order.SupplierType))
                types.Add(order.SupplierType);
        });

        orders.Clear();

        stopWatch.Stop();
        Console.WriteLine("[Execution Time]: " + stopWatch.Elapsed.TotalSeconds + " Seconds (Get all supplier types)");

        return types;
    }

    public static float GetSupplierTypeWeeklyCost(string type, string week, string year)
    {
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();

        ConcurrentQueue<string> allPaths = new ConcurrentQueue<string>(Directory.GetFiles(folderPath + @"\" + storeDataFolder).ToList());
        List<string> paths = new List<string>();
        string comparison = "_" + week + "_" + year;

        Parallel.ForEach(allPaths, path =>
        {
            if (path.Contains(comparison))
            {
                paths.Add(path);
            }
        });

        float totalCost = 0.0f;
        ConcurrentQueue<Order> orders = new ConcurrentQueue<Order>(GetStoreOrderData(paths));

        foreach (Order order in orders)
        {
            if (order.SupplierType == type)
                totalCost += order.Cost;
        }

        stopWatch.Stop();
        Console.WriteLine("[Execution Time]: " + stopWatch.Elapsed.TotalSeconds + " Seconds (Get cost of orders to a supplier type in a specified week)");

        paths.Clear();

        return totalCost;
    }

    public static float GetSupplierTypeTotalCost(string type)
    {
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();

        float totalCost = 0.0f;

        List<string> paths = Directory.GetFiles(folderPath + @"\" + storeDataFolder).ToList();
        List<Order> orders = GetStoreOrderData(paths);

        paths.Clear();

        foreach(Order order in orders)
        {
            if (order.SupplierType == type)
            {
                totalCost += order.Cost;
            }
        }

        orders.Clear();

        stopWatch.Stop();
        Console.WriteLine("[Execution Time]: " + stopWatch.Elapsed.TotalSeconds + " Seconds (Get cost of all orders to supplier type)");

        return totalCost;
    }
}