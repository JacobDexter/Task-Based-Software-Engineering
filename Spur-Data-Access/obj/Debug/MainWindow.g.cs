﻿#pragma checksum "..\..\MainWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "288D8CA9FA7EE051217A7BE37DE6784BCB9DAC151B497FF2FD8FC6451FCFA7DF"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Spur_Data_Access;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace Spur_Data_Access {
    
    
    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 10 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox Shops;
        
        #line default
        #line hidden
        
        
        #line 11 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox ShopData;
        
        #line default
        #line hidden
        
        
        #line 12 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox Orders;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock TotalCostAllOrders;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock TotalCostStore;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button CalStoreOrdersButton;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button CalOrderCostsAllStoresButton;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button CalWeeklyStoreOrderCost;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock WeekOrderCost;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox WeekCombo;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox YearCombo;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock AllStoreWeeklyText;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox StoreSupplierTypeTotalDropDown;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock StoreSupplierTypeTotalText;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox SuppTypeWeekStoreDrop;
        
        #line default
        #line hidden
        
        
        #line 46 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox SuppTypeYearStoreDrop;
        
        #line default
        #line hidden
        
        
        #line 50 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock TotalOrderSuppTypeStoreWeeklyText;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Spur-Data-Access;component/mainwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\MainWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.Shops = ((System.Windows.Controls.ListBox)(target));
            
            #line 10 "..\..\MainWindow.xaml"
            this.Shops.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.Shops_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.ShopData = ((System.Windows.Controls.ListBox)(target));
            
            #line 11 "..\..\MainWindow.xaml"
            this.ShopData.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.ShopData_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.Orders = ((System.Windows.Controls.ListBox)(target));
            return;
            case 4:
            this.TotalCostAllOrders = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 5:
            this.TotalCostStore = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 6:
            this.CalStoreOrdersButton = ((System.Windows.Controls.Button)(target));
            
            #line 17 "..\..\MainWindow.xaml"
            this.CalStoreOrdersButton.Click += new System.Windows.RoutedEventHandler(this.CalStoreOrdersButton_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.CalOrderCostsAllStoresButton = ((System.Windows.Controls.Button)(target));
            
            #line 18 "..\..\MainWindow.xaml"
            this.CalOrderCostsAllStoresButton.Click += new System.Windows.RoutedEventHandler(this.CalOrderCostsAllStoresButton_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            
            #line 19 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.RefreshButton_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.CalWeeklyStoreOrderCost = ((System.Windows.Controls.Button)(target));
            
            #line 20 "..\..\MainWindow.xaml"
            this.CalWeeklyStoreOrderCost.Click += new System.Windows.RoutedEventHandler(this.CalWeeklyStoreOrderCost_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            this.WeekOrderCost = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 11:
            this.WeekCombo = ((System.Windows.Controls.ComboBox)(target));
            
            #line 27 "..\..\MainWindow.xaml"
            this.WeekCombo.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.WeekCombo_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 12:
            this.YearCombo = ((System.Windows.Controls.ComboBox)(target));
            
            #line 29 "..\..\MainWindow.xaml"
            this.YearCombo.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.YearCombo_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 13:
            this.AllStoreWeeklyText = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 14:
            
            #line 36 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            case 15:
            this.StoreSupplierTypeTotalDropDown = ((System.Windows.Controls.ComboBox)(target));
            
            #line 37 "..\..\MainWindow.xaml"
            this.StoreSupplierTypeTotalDropDown.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.StoreSupplierTypeTotalDropDown_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 16:
            this.StoreSupplierTypeTotalText = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 17:
            this.SuppTypeWeekStoreDrop = ((System.Windows.Controls.ComboBox)(target));
            
            #line 44 "..\..\MainWindow.xaml"
            this.SuppTypeWeekStoreDrop.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.SuppTypeWeekStoreDrop_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 18:
            this.SuppTypeYearStoreDrop = ((System.Windows.Controls.ComboBox)(target));
            
            #line 46 "..\..\MainWindow.xaml"
            this.SuppTypeYearStoreDrop.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.SuppTypeYearStoreDrop_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 19:
            this.TotalOrderSuppTypeStoreWeeklyText = ((System.Windows.Controls.TextBlock)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

