using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace MergeGridTest
{
    /// <summary>
    /// test1window.xaml 的交互逻辑
    /// </summary>
    public partial class test1window : Window
    {

        DrawingVisual dv = new DrawingVisual();
        public test1window()
        {
            InitializeComponent();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var Doctors = new List<Doctor>()
            {
                new Doctor(){Name="Zhang",Score=15,Address="Chengdu",Dept="Neike"},
                new Doctor(){Name="Zhang",Score=18,Address="Chengdu",Dept="Neike"},
                new Doctor(){Name="Zhang",Score=17,Address="Chengdu",Dept="Neike"},
                new Doctor(){Name="Liu",Score=15,Address="Chengdu",Dept="Thke" },
                new Doctor(){Name="Liu",Score=18,Address="MianYang",Dept="Thke"},
                new Doctor(){Name="Liu",Score=17,Address="MianYang",Dept="Thke"}
            };
            TestGrid.ItemsSource = Doctors;
        }
    }
    class Doctor
    {
        public string Name { get; set; }
        public int Score { get; set; }
        public string Address { get; set; }
        public string Dept { get; set; }
    }
}
