using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
            Task task = Task.Factory.StartNew(() =>
            {
                InitializeComponent();
                SetupWeeks();
                GetSupplierNames();
                GetSupplierTypes();
            }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void SetupWeeks()
        {
            Task task = Task.Factory.StartNew(() =>
            {
                for (int i = 1; i <= 52; i++)
                {
                    ComboBoxItem item = new ComboBoxItem
                    {
                        Content = i
                    };

                    WeekCombo.Items.Add(item);
                }
            }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void GetSupplierNames()
        {
            Task task = Task.Factory.StartNew(() =>
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
            }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void GetSupplierTypes()
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

                    ComboBoxItem item2 = new ComboBoxItem
                    {
                        Content = type
                    };

                    SupplierTypeSelector.Items.Add(item);
                    SupplierTypeWeekYearSelector.Items.Add(item2);
                }

                types.Clear();
            }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void CalculateSuppTypeWeekly()
        {
            Task task = Task.Factory.StartNew(() =>
            {
                if (SupplierTypeWeekYearSelector.SelectedIndex != -1 && WeekCombo.SelectedIndex != -1 && YearCombo.SelectedIndex != -1)
                {
                    ComboBoxItem item = (ComboBoxItem)SupplierTypeWeekYearSelector.SelectedItem;
                    CostOrdersTypePerWeek.Text = String.Format("{0:£#,##0.00;(£#,##0.00);Zero}", CSVLoader.GetSupplierTypeWeeklyCost(item.Content.ToString(), WeekCombo.Text, YearCombo.Text));
                }
            }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void CalculateSuppTypeTotalCost()
        {
            Task task = Task.Factory.StartNew(() =>
            {
                if (SupplierTypeSelector.SelectedIndex != -1)
                {
                    ComboBoxItem item = (ComboBoxItem)SupplierTypeSelector.SelectedItem;
                    CostOfOrdersToSuppType.Text = String.Format("{0:£#,##0.00;(£#,##0.00);Zero}", CSVLoader.GetSupplierTypeTotalCost(item.Content.ToString()));
                }
            }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void SupplierSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Task task = Task.Factory.StartNew(() =>
            {
                ComboBoxItem item = (ComboBoxItem)SupplierSelector.SelectedItem;
                CostOfOrdersToSupplierText.Text = String.Format("{0:£#,##0.00;(£#,##0.00);Zero}", CSVLoader.GetSupplierOrderCost(item.Content.ToString()));
            }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void SupplierTypeWeekYearSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Task task = Task.Factory.StartNew(() =>
            {
                CalculateSuppTypeWeekly();
            }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void WeekCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Task task = Task.Factory.StartNew(() =>
            {
                CalculateSuppTypeWeekly();
            }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void YearCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Task task = Task.Factory.StartNew(() =>
            {
                CalculateSuppTypeWeekly();
            }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void SupplierTypeSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Task task = Task.Factory.StartNew(() =>
            {
                CalculateSuppTypeTotalCost();
            }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}
