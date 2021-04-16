using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Windows.Threading;
using System.Collections.Concurrent;

namespace Spur_Data_Access
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        static Dictionary<string, CSVLoader.Store> stores = new Dictionary<string, CSVLoader.Store>();
        static TaskScheduler taskScheduler = TaskScheduler.FromCurrentSynchronizationContext();

        public MainWindow()
        {
            InitializeComponent();
            LoadStoreNames();
        }

        //get all store names
        void LoadStoreNames()
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action
                (
                () =>
                    {
                        stores = CSVLoader.GetStoreData();
                        foreach (string index in stores.Keys)
                        {
                            ListBoxItem item = new ListBoxItem
                            {
                                Content = stores[index].StoreCode + " - " + stores[index].StoreLocation
                            };

                            item.Selected += new RoutedEventHandler(GetStoreData);

                            Shops.Items.Add(item);
                        }
                    }
                ));
        }

        //get all files related to store
        void GetStoreData(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action
            (
            () =>
                {
                    ListBoxItem temp = (ListBoxItem)sender;
                    List<string> fileNames = CSVLoader.FindAllFilePathsWithCode(temp.Content.ToString().Substring(0, 4));

                    ShopData.Items.Clear(); //clear any remaining items in list box

                    //clear total order value
                    TotalCostStore.Text = "-";

                    foreach (string file in fileNames)
                    {
                        ListBoxItem item = new ListBoxItem
                        {
                            Content = System.IO.Path.GetFileName(file)
                        };

                        item.Selected += new RoutedEventHandler(LoadOrdersFromFile);

                        ShopData.Items.Add(item);
                    }

                    //calculate total of all orders and display
                    //TotalCostStore.Text = String.Format("{0:$#,##0.00;(£#,##0.00);Zero}", CSVLoader.GetStoreTotalOrderCost(temp.Content.ToString().Substring(0, 4)));
                }
             ));
        }

        //get all orders from file
        void LoadOrdersFromFile(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action
                (
                () =>
                {
                    ListBoxItem temp = (ListBoxItem)sender;
                    List<CSVLoader.Order> orders = CSVLoader.GetFileOrders(temp.Content.ToString());

                    foreach (CSVLoader.Order order in orders)
                    {
                        ListBoxItem item = new ListBoxItem
                        {
                            Content = "Week " + order.Date.Week + " - " + order.Date.Year + " - " + order.SupplierName + " - " + order.SupplierType + " - " + order.Cost
                        };

                        Orders.Items.Add(item);
                    }

                    orders.Clear();
                }
                ));
        }

        private void CalStoreOrdersButton_Click(object sender, RoutedEventArgs e)
        {
            //check if a store is selected
            if(Shops.SelectedItem != null && TotalCostStore.Text.Length <= 1)
            {
                //get current active store
                ListBoxItem store = (ListBoxItem)Shops.SelectedItem;

                //calculate total of all orders for store and display
                TotalCostStore.Text = String.Format("{0:£#,##0.00;(£#,##0.00);Zero}", CSVLoader.GetStoreTotalOrderCost(store.Content.ToString().Substring(0, 4)));
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            Shops.Items.Clear();
            ShopData.Items.Clear();
            TotalCostStore.Text = "-";
            TotalCostAllOrders.Text = "-";
            LoadStoreNames();
        }

        private void CalOrderCostsAllStoresButton_Click(object sender, RoutedEventArgs e)
        {
            if (TotalCostAllOrders.Text.Length <= 1)
            {
                //calculate total of all orders and display
                TotalCostAllOrders.Text = String.Format("{0:£#,##0.00;(£#,##0.00);Zero}", CSVLoader.GetTotalOrderCost());
            }
        }

        private void CalWeeklyStoreOrderCost_Click(object sender, RoutedEventArgs e)
        {
            if(ShopData.SelectedItem != null)
            {
                ListBoxItem filename = (ListBoxItem)ShopData.SelectedItem;
                WeekOrderCost.Text = String.Format("{0:£#,##0.00;(£#,##0.00);Zero}", CSVLoader.GetStoreWeeklyOrderCost(filename.Content.ToString())).ToString();
            }
        }

        private void ShopData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Orders.Items.Clear();
            WeekOrderCost.Text = " - ";
        }
    }
}
