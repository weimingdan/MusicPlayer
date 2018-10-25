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
using MahApps.Metro.Controls;

namespace MusicPlayer
{
    /// <summary>
    /// CheckWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CheckWindow : MetroWindow
    {
        public CheckWindow()
        {
            InitializeComponent();
            cbAllCHeck.IsChecked = null;
        }

        private void CbAllCHeck_OnChecked(object sender, RoutedEventArgs e)
        {
        }

        private void CbAllCHeck_OnUnchecked(object sender, RoutedEventArgs e)
        {
        }
    }
}
