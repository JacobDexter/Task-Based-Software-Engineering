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

namespace Spur_Data_Access
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static Dictionary<string, CSVLoader.Store> stores = new Dictionary<string, CSVLoader.Store>();

        public MainWindow()
        {
            InitializeComponent();
            LoadStoreNames();
        }

        void LoadStoreNames()
        {
            stores = CSVLoader.LoadStores();
            foreach(string index in stores.Keys)
            {
                ListBoxItem item = new ListBoxItem
                {
                    Content = stores[index].StoreCode + " - " + stores[index].StoreLocation
                };

                item.Selected += new RoutedEventHandler(GetStoreData);

                Shops.Items.Add(item);
            }
        }

        void GetStoreData(object sender, RoutedEventArgs e)
        {
            ListBoxItem temp = (ListBoxItem)sender;
            List<string> fileNames = CSVLoader.StoreData(temp.Content.ToString().Substring(0, 4));

            ShopData.Items.Clear(); //clear any remaining items in list box

            foreach (string file in fileNames)
            {
                ListBoxItem item = new ListBoxItem
                {
                    Content = file
                };

                ShopData.Items.Add(item);
            }
        }
    }
}
