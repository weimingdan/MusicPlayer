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
    /// Browser.xaml 的交互逻辑
    /// </summary>
    public partial class Browser : MetroWindow
    {
        public Browser()
        {
            InitializeComponent();
        }

        private void UrlEdit_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                MyBrowser.Address = UrlEdit.Text;
                LoadProgress.Value = 0;
            }
        }
    }
}
