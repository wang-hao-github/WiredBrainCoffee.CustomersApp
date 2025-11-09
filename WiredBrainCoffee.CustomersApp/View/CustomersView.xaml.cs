using Repository;
using Repository.Model;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WiredBrainCoffee.CustomersApp.Common;
using WiredBrainCoffee.CustomersApp.ViewModel;

namespace WiredBrainCoffee.CustomersApp.View
{
    /// <summary>
    /// CustomersView.xaml 的交互逻辑
    /// </summary>
    public partial class CustomersView : UserControl
    {
        private CustomersViewModel _viewModel;

        public CustomersView()
        {
            InitializeComponent();
        }
    }
}
