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
                SetupWeeks();
                GetSupplierTypes();
        }

        //set weeks dropdown
        async private void SetupWeeks()
        {
            Task task = Task.Factory.StartNew(() =>
            {
                for (int i = 1; i <= 52; i++)
                {
                    ComboBoxItem item = new ComboBoxItem
                    {
                        Content = i
                    };

                    ComboBoxItem item2 = new ComboBoxItem
                    {
                        Content = i
                    };

                    WeekCombo.Items.Add(item);
                    SuppTypeWeekStoreDrop.Items.Add(item2);
                }
            }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());

            await task;
        }

        //get all store names
        async void LoadStoreNames()
        {
            Task task = Task.Factory.StartNew(() =>
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
            }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());

            await task;
        }

        //get all supplier types
        async private void GetSupplierTypes()
        {
            Task task = Task.Factory.StartNew(() =>
            {
                List<string> types = CSVLoader.GetSupplierTypes();

                foreach (string type in types)
                {
                    ComboBoxItem item = new ComboBoxItem
                    {
                        Content = type
                    };

                    StoreSupplierTypeTotalDropDown.Items.Add(item);
                }

                types.Clear();
            }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());

            await task;
        }

        //get all files related to store
        async void GetStoreData(object sender, RoutedEventArgs e)
        {
            Task task = Task.Factory.StartNew(() =>
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
            }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());

            await task;
        }

        //get all orders from file
        async void LoadOrdersFromFile(object sender, RoutedEventArgs e)
        {
            Task task = Task.Factory.StartNew(() =>
            {
                ListBoxItem temp = (ListBoxItem)sender;
                List<CSVLoader.Order> orders = CSVLoader.GetFileOrders(temp.Content.ToString()).OrderBy(x => x.SupplierType).ToList();

                if (Orders.Items.Count != -1)
                    Orders.Items.Clear();

                foreach (CSVLoader.Order order in orders)
                {
                    ListBoxItem item = new ListBoxItem
                    {
                        Content = "Week " + order.Date.Week + " - " + order.Date.Year + " - " + order.SupplierName + " - " + order.SupplierType + " - £" + order.Cost
                    };

                    Orders.Items.Add(item);
                }

                orders.Clear();
            }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());

            await task;
        }

        async private void CalculateStoreWeeklyOrderTotal()
        {
            Task task = Task.Factory.StartNew(() =>
            {
                if (SuppTypeWeekStoreDrop.SelectedIndex != -1 && SuppTypeYearStoreDrop.SelectedIndex != -1 && Shops.SelectedIndex != -1)
                {
                    ListBoxItem item = (ListBoxItem)Shops.SelectedItem;
                    TotalOrderSuppTypeStoreWeeklyText.Text = String.Format("{0:£#,##0.00;(£#,##0.00);Zero}", CSVLoader.GetStoreWeekTotalOrderCost(item.Content.ToString().Substring(0, 4), SuppTypeWeekStoreDrop.Text, SuppTypeYearStoreDrop.Text));
                }
            }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());

            await task;
        }

        async private void CalStoreOrdersButton_Click(object sender, RoutedEventArgs e)
        {
            Task task = Task.Factory.StartNew(() =>
            {
                //check if a store is selected
                if (Shops.SelectedItem != null && TotalCostStore.Text.Length <= 1)
                {
                    //get current active store
                    ListBoxItem store = (ListBoxItem)Shops.SelectedItem;

                    //calculate total of all orders for store and display
                    TotalCostStore.Text = String.Format("{0:£#,##0.00;(£#,##0.00);Zero}", CSVLoader.GetStoreTotalOrderCost(store.Content.ToString().Substring(0, 4)));
                }
            }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());

            await task;
        }

        async private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            Task task = Task.Factory.StartNew(() =>
            {
                Shops.Items.Clear();
                ShopData.Items.Clear();
                TotalCostStore.Text = "-";
                TotalCostAllOrders.Text = "-";
                StoreSupplierTypeTotalDropDown.SelectedIndex = -1;
                WeekCombo.SelectedIndex = -1;
                YearCombo.SelectedIndex = -1;
                StoreSupplierTypeTotalText.Text = " - ";
                AllStoreWeeklyText.Text = " - ";
                SuppTypeWeekStoreDrop.SelectedIndex = -1;
                SuppTypeYearStoreDrop.SelectedIndex = -1;
                TotalOrderSuppTypeStoreWeeklyText.Text = " - ";
                Orders.Items.Clear();
                LoadStoreNames();
            }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());

            await task;
        }

        async private void CalOrderCostsAllStoresButton_Click(object sender, RoutedEventArgs e)
        {
            Task task = Task.Factory.StartNew(() =>
            {
                if (TotalCostAllOrders.Text.Length <= 1)
                {
                    //calculate total of all orders and display
                    TotalCostAllOrders.Text = String.Format("{0:£#,##0.00;(£#,##0.00);Zero}", CSVLoader.GetTotalOrderCost());
                }
            }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());

            await task;
        }

        async private void CalWeeklyStoreOrderCost_Click(object sender, RoutedEventArgs e)
        {
            Task task = Task.Factory.StartNew(() =>
            {
                if (ShopData.SelectedItem != null)
                {
                    ListBoxItem filename = (ListBoxItem)ShopData.SelectedItem;
                    WeekOrderCost.Text = String.Format("{0:£#,##0.00;(£#,##0.00);Zero}", CSVLoader.GetStoreWeeklyOrderCost(filename.Content.ToString())).ToString();
                }
            }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());

            await task;
        }

        private void ShopData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WeekOrderCost.Text = " - ";
        }

        async private void Button_Click(object sender, RoutedEventArgs e)
        {
            Task task = Task.Factory.StartNew(() =>
            {
                Window supp = new SupplierWindow();
                supp.Show();
            }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());

            await task;
        }

        async private void YearCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Task task = Task.Factory.StartNew(() =>
            {
                if (YearCombo.SelectedIndex != -1 && WeekCombo.SelectedIndex != -1)
                {
                    ListBoxItem item = (ListBoxItem)Shops.SelectedItem;
                    AllStoreWeeklyText.Text = String.Format("{0:£#,##0.00;(£#,##0.00);Zero}", CSVLoader.GetWeeklyOrderCost(WeekCombo.Text, YearCombo.Text));
                }
            }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());

            await task;
        }

        async private void WeekCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Task task = Task.Factory.StartNew(() =>
            {
                if (YearCombo.SelectedIndex != -1 && WeekCombo.SelectedIndex != -1)
                {
                    ListBoxItem item = (ListBoxItem)Shops.SelectedItem;
                    AllStoreWeeklyText.Text = String.Format("{0:£#,##0.00;(£#,##0.00);Zero}", CSVLoader.GetWeeklyOrderCost(WeekCombo.Text, YearCombo.Text));
                }
            }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());

            await task;
        }

        async private void SuppTypeWeekStoreDrop_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Task task = Task.Factory.StartNew(() =>
            {
                CalculateStoreWeeklyOrderTotal();
            }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());

            await task;
        }

        async private void SuppTypeYearStoreDrop_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Task task = Task.Factory.StartNew(() =>
            {
                CalculateStoreWeeklyOrderTotal();
            }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());

            await task;
        }

        async private void StoreSupplierTypeTotalDropDown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Task task = Task.Factory.StartNew(() =>
            {
                if (Shops.SelectedIndex != -1 && StoreSupplierTypeTotalDropDown.SelectedIndex != -1)
                {
                    ListBoxItem item = (ListBoxItem)Shops.SelectedItem;
                    StoreSupplierTypeTotalText.Text = String.Format("{0:£#,##0.00;(£#,##0.00);Zero}", CSVLoader.GetTotalCostOfSupplierToStore(item.Content.ToString().Substring(0, 4), StoreSupplierTypeTotalDropDown.Text));
                }
            }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());

            await task;
        }

        async private void Shops_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Task task = Task.Factory.StartNew(() =>
            {
                StoreSupplierTypeTotalDropDown.SelectedIndex = -1;
                StoreSupplierTypeTotalText.Text = " - ";
                SuppTypeWeekStoreDrop.SelectedIndex = -1;
                SuppTypeYearStoreDrop.SelectedIndex = -1;
                TotalOrderSuppTypeStoreWeeklyText.Text = " - ";
            }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
            
            await task;
        }
    }
}
