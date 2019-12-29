using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;
using System.Xml.Linq;

namespace MergeGridTest
{
    /// <summary>
    /// test1window.xaml 的交互逻辑
    /// </summary>
    public partial class test1window : System.Windows.Window
    {

        bool IsVisible = true;

        DrawingVisual dv = new DrawingVisual();
        public test1window()
        {
            InitializeComponent();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //var app = FlaUI.Core.Application.Launch("notepad.exe");
            //using (var automation = new UIA3Automation())
            //{
            //    var window = app.GetMainWindow(automation);
            //    Console.WriteLine(window.Title);
            //}

            //app = FlaUI.Core.Application.Launch("calc.exe");
            //using (var automation = new UIA3Automation())
            //{
            //    var window = app.GetMainWindow(automation);
            //    var cc = window.FindAllDescendants(/*cf => cf.ByClassName("Button")*/);
            //    var button1 = window.FindFirstDescendant(cf => cf.ByText("1"))?.AsButton();
            //    button1?.Invoke();
            //}
            // var elt =  XElement.Load(@"j:\a1 - copie.xml");

            var mapper = new Mapper(@"j:\a1 - copie.xml");
            //var tree = new TreeView();
            mapper.LoadXml(tree);
            //var Doctors = new List<Doctor>()
            //{
            //    new Doctor(){Name="Zhang",Score=15,Address="Chengdu",Dept="Neike"},
            //    new Doctor(){Name="Zhang",Score=18,Address="Chengdu",Dept="Neike"},
            //    new Doctor(){Name="Zhang",Score=17,Address="Chengdu",Dept="Neike"},
            //    new Doctor(){Name="Liu",Score=15,Address="Chengdu",Dept="Thke" },
            //    new Doctor(){Name="Liu",Score=18,Address="MianYang",Dept="Thke"},
            //    new Doctor(){Name="Liu",Score=17,Address="MianYang",Dept="Thke"}
            //};
            //TestGrid.ItemsSource = Doctors;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IsVisible = !IsVisible;

            //if (IsVisible)
            //{
            //    ImageBrush ib = new ImageBrush();
            //    ib.ImageSource = new BitmapImage(new Uri(@"E:\Téléchargement\C#Samples\demonstration_2.png"));
            //    Mainwindow.Background = ib;
            //    LeftImage.Source = new BitmapImage(new Uri(@"E:\Téléchargement\C#Samples\logo.png"));
            //}
            //else
            //{
            //    ImageBrush ib = new ImageBrush();
            //    ib.ImageSource = new BitmapImage(new Uri(@"E:\Téléchargement\C#Samples\demonstration_2.png"));
            //    Mainwindow.Background = ib;
            //    LeftImage.Source = new BitmapImage(new Uri(@"E:\Téléchargement\C#Samples\yop.png"));
            //}
        }
    }
    class Doctor
    {
        public string Name { get; set; }
        public int Score { get; set; }
        public string Address { get; set; }
        public string Dept { get; set; }
    }
    class Mapper
    {
        private string sourceXmlFile;
        private XDocument xmlData;

        public Mapper(string xmlFilePath)
        {
            sourceXmlFile = xmlFilePath;
        }

        private void BuildNodes(TreeViewItem treeNode, XElement element)
        {

            string attributes = "";
            if (element.HasAttributes && element.Name.LocalName.Equals("anyType"))
            {
                foreach (var att in element.Attributes())
                {
                    attributes = att.Value;
                    //attributes += " " + att.Name.LocalName + " = " + att.Value;
                }
            }

            TreeViewItem childTreeNode = new TreeViewItem
            {
                Header = element.Name.LocalName + attributes,
                IsExpanded = true
            };
            if (element.HasElements)
            {
                foreach (XElement childElement in element.Elements())
                {
                    BuildNodes(childTreeNode, childElement);
                }
            }
            else
            {
                //TreeViewItem childTreeNodeText = new TreeViewItem
                //{
                //    Header = element.Value,
                //    IsExpanded = true
                //};
                //childTreeNode.Items.Add(childTreeNodeText);
                childTreeNode.Header = $"{childTreeNode.Header} = [{element.Value}]";
            
            }

            treeNode.Items.Add(childTreeNode);
        }



        public void LoadXml(TreeView treeview)
        {
            try
            {
                if (sourceXmlFile != null)
                {
                    xmlData = XDocument.Load(sourceXmlFile, LoadOptions.None);
                    if (xmlData == null)
                    {
                        throw new XmlException("Cannot load Xml document from file : " + sourceXmlFile);
                    }
                    else
                    {
                        TreeViewItem treeNode = new TreeViewItem
                        {
                            Header = sourceXmlFile,
                            IsExpanded = true
                        };


                        BuildNodes(treeNode, xmlData.Root);
                        treeview.Items.Add(treeNode);
                    }
                }
                else
                {
                    throw new IOException("Xml file is not set correctly.");
                }
            }
            catch (IOException ioex)
            {
                //log
            }
            catch (XmlException xmlex)
            {
                //log
            }
            catch (Exception ex)
            {
                //log
            }
        }

    }
}
