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
        private void SetupWeeks()
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action
                   (
                   () =>
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
                   }
                   ));
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

        //get all supplier types
        private void GetSupplierTypes()
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action
                (
                () =>
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

        private void CalculateStoreWeeklyOrderTotal()
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action
                (
                () =>
                {
                    if (SuppTypeWeekStoreDrop.SelectedIndex != -1 && SuppTypeYearStoreDrop.SelectedIndex != -1 && Shops.SelectedIndex != -1)
                    {
                        ListBoxItem item = (ListBoxItem)Shops.SelectedItem;
                        TotalOrderSuppTypeStoreWeeklyText.Text = String.Format("{0:£#,##0.00;(£#,##0.00);Zero}", CSVLoader.GetStoreWeekTotalOrderCost(item.Content.ToString().Substring(0, 4), SuppTypeWeekStoreDrop.Text, SuppTypeYearStoreDrop.Text));
                    }
                }
                ));
        }

        private void CalStoreOrdersButton_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action
                (
                () =>
                {
                    //check if a store is selected
                    if (Shops.SelectedItem != null && TotalCostStore.Text.Length <= 1)
                    {
                        //get current active store
                        ListBoxItem store = (ListBoxItem)Shops.SelectedItem;

                        //calculate total of all orders for store and display
                        TotalCostStore.Text = String.Format("{0:£#,##0.00;(£#,##0.00);Zero}", CSVLoader.GetStoreTotalOrderCost(store.Content.ToString().Substring(0, 4)));
                    }
                }
                ));
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action
                (
                () =>
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
                    LoadStoreNames();
                }
                ));
        }

        private void CalOrderCostsAllStoresButton_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action
                (
                () =>
                {
                    if (TotalCostAllOrders.Text.Length <= 1)
                    {
                        //calculate total of all orders and display
                        TotalCostAllOrders.Text = String.Format("{0:£#,##0.00;(£#,##0.00);Zero}", CSVLoader.GetTotalOrderCost());
                    }
                }
                ));
        }

        private void CalWeeklyStoreOrderCost_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action
                (
                () =>
                {
                    if (ShopData.SelectedItem != null)
                    {
                        ListBoxItem filename = (ListBoxItem)ShopData.SelectedItem;
                        WeekOrderCost.Text = String.Format("{0:£#,##0.00;(£#,##0.00);Zero}", CSVLoader.GetStoreWeeklyOrderCost(filename.Content.ToString())).ToString();
                    }
                }
                ));
        }

        private void ShopData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Orders.Items.Clear();
            WeekOrderCost.Text = " - ";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action
                (
                () =>
                {
                    Window supp = new SupplierWindow();
                    supp.Show();
                }
                ));
        }

        private void YearCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action
                (
                () =>
                {
                    if(YearCombo.SelectedIndex != -1 && WeekCombo.SelectedIndex != -1)
                    {
                        ListBoxItem item = (ListBoxItem)Shops.SelectedItem;
                        AllStoreWeeklyText.Text = String.Format("{0:£#,##0.00;(£#,##0.00);Zero}", CSVLoader.GetWeeklyOrderCost(WeekCombo.Text, YearCombo.Text));
                    }
                }
                ));
        }

        private void WeekCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action
                (
                () =>
                {
                    if (YearCombo.SelectedIndex != -1 && WeekCombo.SelectedIndex != -1)
                    {
                        ListBoxItem item = (ListBoxItem)Shops.SelectedItem;
                        AllStoreWeeklyText.Text = String.Format("{0:£#,##0.00;(£#,##0.00);Zero}", CSVLoader.GetWeeklyOrderCost(WeekCombo.Text, YearCombo.Text));
                    }
                }
                ));
        }

        private void SuppTypeWeekStoreDrop_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action
                (
                () =>
                {
                    CalculateStoreWeeklyOrderTotal();
                }
                ));
        }

        private void SuppTypeYearStoreDrop_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action
                (
                () =>
                {
                    CalculateStoreWeeklyOrderTotal();
                }
                ));
        }

        private void StoreSupplierTypeTotalDropDown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action
                (
                () =>
                {
                    if (Shops.SelectedIndex != -1 && StoreSupplierTypeTotalDropDown.SelectedIndex != -1)
                    {
                        ListBoxItem item = (ListBoxItem)Shops.SelectedItem;
                        StoreSupplierTypeTotalText.Text = String.Format("{0:£#,##0.00;(£#,##0.00);Zero}", CSVLoader.GetTotalCostOfSupplierToStore(item.Content.ToString().Substring(0, 4), StoreSupplierTypeTotalDropDown.Text));
                    }
                }
                ));
        }

        private void Shops_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action
                (
                () =>
                {
                    StoreSupplierTypeTotalDropDown.SelectedIndex = -1;
                    StoreSupplierTypeTotalText.Text = " - ";
                    SuppTypeWeekStoreDrop.SelectedIndex = -1;
                    SuppTypeYearStoreDrop.SelectedIndex = -1;
                    TotalOrderSuppTypeStoreWeeklyText.Text = " - ";
                }
                ));
        }
    }
}
