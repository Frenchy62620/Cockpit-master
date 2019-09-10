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

namespace zzApp1
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var d = 1;
            var a = 1.2d;
            var b = 1.5d;
            var c = 1.6d;

            var e = d / a;
            var a1 = Math.Round(a, 0, MidpointRounding.ToEven);
            var b1 = Math.Round(b, 0, MidpointRounding.ToEven);
            var c1 = Math.Round(c, 0, MidpointRounding.ToEven);

        }
    }
}
