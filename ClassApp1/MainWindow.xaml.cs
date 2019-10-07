//MainWindow.xaml.cs
using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows;
using System.Xml;

namespace ClassApp1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var p = new Plugins();

            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).Where(a => a.ToString().StartsWith("ClassP") || a.ToString().StartsWith("ClassL")).ToArray();

            var type = p.GetType();
            var dcs = new DataContractSerializer(type, types);

            var settings = new XmlWriterSettings()
            {
                Indent = true,
                IndentChars = "    "
            };

            using (XmlWriter wr = XmlWriter.Create(@"j:\message.xml", settings))
            {
                dcs.WriteObject(wr,  p);
            }

            //var settingsR = new XmlReaderSettings()
            // {
            //     Indent = true,
            //     IndentChars = "    "
            // };
            ClassApp1.Plugins plg;
            using (XmlReader wr = XmlReader.Create(@"j:\message.xml"))
            {
               plg =(Plugins) dcs.ReadObject(wr);
            }

            
        }
    }
}
