using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Spur_Data_Access
{
    /// <summary>
    /// Interaction logic for SupplierWindow.xaml
    /// </summary>
    public partial class SupplierWindow : Window
    {
        public SupplierWindow()
        {
            InitializeComponent();
            SetupWeeks();
            GetSupplierNames();
            GetSupplierTypes();
        }

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

                           WeekCombo.Items.Add(item);
                       }
                   }
                   ));
        }

        private void GetSupplierNames()
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action
                (
                () =>
                {
                    List<string> supplierNames = CSVLoader.GetSupplierNames();

                    foreach (string name in supplierNames)
                    {
                        ComboBoxItem item = new ComboBoxItem
                        {
                            Content = name
                        };

                        SupplierSelector.Items.Add(item);
                    }
                }
                ));
        }

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

                        ComboBoxItem item2 = new ComboBoxItem
                        {
                            Content = type
                        };

                        SupplierTypeSelector.Items.Add(item);
                        SupplierTypeWeekYearSelector.Items.Add(item2);
                    }
                }
                ));
        }

        private void SupplierSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action
                (
                () =>
                {
                    ComboBoxItem item = (ComboBoxItem)SupplierSelector.SelectedItem;
                    CostOfOrdersToSupplierText.Text = String.Format("{0:£#,##0.00;(£#,##0.00);Zero}", CSVLoader.GetSupplierOrderCost(item.Content.ToString()));
                }
                ));
        }
    }
}
