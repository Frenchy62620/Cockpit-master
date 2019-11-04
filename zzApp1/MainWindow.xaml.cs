using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        public double sizeofleft = 0d;
        public double sizeofright = 0d;

        public int a = 10;
        public ConsoleColor b = ConsoleColor.Green;
        public string c ;
        public MainWindow()
        {
            InitializeComponent();

        }

        public int count_avg = 0;
        public double sum = 0;
        public double temp_avg = 0;
        public int id = 0;
        double[] array = new double[50];
        private string Average(double analog_temperature)
        {


            if (count_avg < array.Length)
            {
                array[count_avg++] = analog_temperature;
                temp_avg = analog_temperature;
                sum += analog_temperature;
                if (count_avg == array.Length)
                    temp_avg = sum / array.Length;
            }
            else
            {
                temp_avg = temp_avg - array[id] / array.Length;
                array[id++] = analog_temperature;
                temp_avg = temp_avg + analog_temperature / array.Length;
                if (id == array.Length) id = 0;
            }

            return Math.Round(temp_avg, 1).ToString();
        }

        private void Window_Loaded(object view, RoutedEventArgs e)
        {

            var ListOfType =
    Assembly.GetExecutingAssembly()
        .GetTypes().Where(p => p == typeof(MainWindow) || p == typeof(MyClass)).ToArray();

            var fields = ListOfType[0].GetFields().ToDictionary(f => f.Name, f =>
            {
                return (f.GetValue(this), f.FieldType);
            });

            var pp = ListOfType[1].GetConstructors()[0].GetParameters().ToDictionary(p => p.Name, p => 
            {
                return (p.HasDefaultValue ? p.DefaultValue : null, p.ParameterType);
            });

            var xx = pp.Select(k =>
            {
                if (fields.ContainsKey(k.Key) && fields[k.Key].Item1 != null)
                {
                    return (k.Key, fields[k.Key].Item1);
                }
                else
                    return (k.Key, pp[k.Key].Item1);
            }).ToDictionary(x => x.Key, x => x.Item2);

            //.FirstOrDefault();

            //var instance = MyFactory.MyCreateInstance(someType);

            //var values = someType.GetFields().Select(f => f.GetValue(instance)).ToArray();

            //foreach (var value in values)
            //{
            //    Console.WriteLine(value);
            //}

            var pxp = new object[] { xx["a"], xx["b"], xx["c"] };

            var ttt = Activator.CreateInstance(typeof(MyClass), pxp);
            ProcessElement((DependencyObject)view);

            sizeofleft += 4; //add margin 2 + 2

            return;

            //void ProcessElement(DependencyObject element)
            //{
            //    for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
            //    {

            //        Visual childVisual = (Visual)VisualTreeHelper.GetChild(element, i);
            //        var t = childVisual.GetType().Name;
            //        //System.Diagnostics.Debug.WriteLine($"{this}:{i} -> {t} ");

            //        if (t.Contains("StackPanel") || t.Contains("Border") || t.Contains("Label"))
            //        {
            //            double h;
            //            if (childVisual is StackPanel)
            //            {
            //                if (((StackPanel)childVisual).Name.Equals("buttons"))
            //                {
            //                    sizeofright += (childVisual as StackPanel).ActualHeight;
            //                }
            //            }
            //            else if (childVisual is Label)
            //                sizeofright += (childVisual as Label).ActualHeight;
            //            else
            //            {
            //                var border = childVisual as Border;
            //                if (border.Name.StartsWith("borderleft"))
            //                    sizeofleft += border.ActualHeight;
            //            }
            //        }


            //        ////System.Diagnostics.Debug.WriteLine($"{i} -> {t}");
            //        //var childContentVisual = childVisual as ContentControl;
            //        //if (childContentVisual != null)
            //        //{
            //        //    var content = childContentVisual.Content;
            //        //}
            //        ProcessElement(childVisual);
            //    }

            //}

        }

        private void ProcessElement(DependencyObject element)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
            {

                Visual childVisual = (Visual)VisualTreeHelper.GetChild(element, i);
                if (childVisual == null) continue;
                var t = childVisual.GetType().Name;
                //System.Diagnostics.Debug.WriteLine($"{this}:{i} -> {t} ");

                if (t.Contains("StackPanel") || t.Contains("Border") || t.Contains("Label"))
                {
                    double h;
                    if (childVisual is StackPanel)
                    {
                        if (((StackPanel)childVisual).Name.Equals("buttons"))
                        {
                            sizeofright += (childVisual as StackPanel).ActualHeight;
                        }
                    }
                    else if (childVisual is Label)
                        sizeofright += (childVisual as Label).ActualHeight;
                    else
                    {
                        var border = childVisual as Border;
                        if (border.Name.StartsWith("borderleft"))
                            sizeofleft += border.ActualHeight;
                    }
                }
                
                ProcessElement(childVisual);
            }

        }

    }

    public class MyClass
    {
        public int A = -1;
        public ConsoleColor B = ConsoleColor.Green;
        public string C = "TESTTEST";

        public MyClass(int a, ConsoleColor b, string c = "I am optional parameter value")
        {
            this.A = a;
            this.B = b;
            this.C = c;
        }
    }

    public static class MyFactory
    {
        public static T MyCreateInstance<T>()
            where T : class
        {
            return (T)MyCreateInstance(typeof(T));
        }

        public static object MyCreateInstance(Type type)
        {
            var ctor = type
                .GetConstructors()
                .FirstOrDefault(c => c.GetParameters().Length > 0);

            if (ctor == null)
            {
                return Activator.CreateInstance(type);
            }

            var result =
                ctor.Invoke
                    (ctor.GetParameters()
                        .Select(p =>
                            p.HasDefaultValue ? p.DefaultValue :
                            p.ParameterType.IsValueType && Nullable.GetUnderlyingType(p.ParameterType) == null
                                ? Activator.CreateInstance(p.ParameterType)
                                : null
                        ).ToArray()
                    );

            return result;
        }
    }
}
