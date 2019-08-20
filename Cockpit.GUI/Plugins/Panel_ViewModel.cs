﻿using Caliburn.Micro;
using Cockpit.Core.Plugins.Plugins;
using Cockpit.Core.Plugins.Plugins.Properties;
using Cockpit.GUI.Events;
using Cockpit.GUI.Plugins.Properties;
using Cockpit.GUI.Views.Profile;
using GongSolutions.Wpf.DragDrop;
using Ninject;
using Ninject.Parameters;
using Ninject.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Cockpit.Core.Plugins.Events;
using IEventAggregator = Cockpit.Core.Common.Events.IEventAggregator;

namespace Cockpit.GUI.Plugins
{
    public class Panel_ViewModel : PluginModel, IDropTarget, Core.Common.Events.IHandle<RemovePanelEvent>,
                                                             Core.Common.Events.IHandle<Core.Plugins.Events.VisibilityPanelEvent>
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IResolutionRoot resolutionRoot;

        public PanelAppearanceViewModel Appearance { get; private set; }
        public LayoutPropertyViewModel Layout { get; private set; }

        public Dictionary<ContentControl, bool> DictContentcontrol = new Dictionary<ContentControl, bool>();

        public string lastNameUC = "";
        private MonitorViewModel mv { get; set; }

        public Panel_ViewModel(IEventAggregator eventAggregator, IResolutionRoot resolutionRoot, params object[] settings)
        {
            this.resolutionRoot = resolutionRoot;
            this.eventAggregator = eventAggregator;
            //this.eventAggregator.Subscribe(this);

            mv = (MonitorViewModel)settings[1];
            Layout = new LayoutPropertyViewModel(eventAggregator, settings);
            Appearance = new PanelAppearanceViewModel(eventAggregator, this, settings);

            MyCockpitViewModels = new BindableCollection<PluginModel>();

            //RenderO = (int)Appearance.SelectedApparition < 2 ? "1.0, 1.0" : "0.0, 0.0";//ToLeft/ToTop or ToRight/ToBottom
            //ScaleXX = (int)Appearance.SelectedApparition % 2 == 0; //ToLeft or ToRight? or ToUp or ToBottom?

            //RenderO = "1.0, 1.0";
            //ScaleXX = true;


            //RenderO = settings.Side < 2 ? "1.0, 1.0" : "0.0, 0.0";//ToLeft/ToTop or ToRight/ToBottom
            //ScaleXX = settings.Side % 2 == 0; //ToLeft or ToRight? or ToUp or ToBottom?
            IsVisible = true;

            NameUC = (string)settings[2];
        }
        ~Panel_ViewModel()
        {
            System.Diagnostics.Debug.WriteLine($"sortie panel {NameUC}");
        }
        public BindableCollection<PluginModel> MyCockpitViewModels { get; set; }

        private bool isvisible;
        public bool IsVisible
        {
            get => isvisible;
            set
            {
                isvisible = value;
                NotifyOfPropertyChange(() => IsVisible);
            }
        }

        //public bool ScaleXX { get; set; }
        private bool _scaleXX;
        public bool ScaleXX
        {
            get { return _scaleXX; }
            set
            {
                _scaleXX = value;
                NotifyOfPropertyChange(() => ScaleXX);
            }
        }


        private string _renderO;
        public string RenderO
        {
            get { return _renderO; }
            set
            {
                _renderO = value;
                NotifyOfPropertyChange(() => RenderO);
            }
        }

        public override double Left
        {
            get => Layout.UCLeft;
            set => Layout.UCLeft = value;
        }
        public override double Top
        {
            get => Layout.UCTop;
            set => Layout.UCTop = value;
        }
        public override double Width
        {
            get => Layout.Width;
            set => Layout.Width = value;
        }
        public override double Height
        {
            get => Layout.Height;
            set => Layout.Height = value;
        }


        public override PluginProperties[] GetProperties()
        {
            return new PluginProperties[] { Layout, Appearance };
        }


        //public void KeyTest(object sender, KeyEventArgs e)
        //{
        //    if (e == null || mv.hash_name_general.Count() == 0) return;

        //    mv.KeyTest(sender, e);

        //    if (e.Key == Key.Delete)
        //    {
        //        foreach (var name in mv.hash_name_general)
        //        {
        //            if (mv.SortedDico[name].pm.ToString().Equals("Cockpit.GUI.Plugins.Panel_ViewModel"))
        //            {
        //                eventAggregator.Publish(new RemovePanelEvent(name));
        //            }
        //            MyCockpitViewModels.Remove(mv.SortedDico[name].pm);
        //            mv.SortedDico.Remove(name);

        //        }
        //        mv.hash_name_general.Clear();
        //    }

        //}

        #region Mouse Events
        //public void MouseLeftButtonDown(IInputElement elem, MouseButtonEventArgs e)
        //{
        //    System.Diagnostics.Debug.WriteLine($"MouseLeftButtonDown {e.ClickCount}");
        //}
        //public void MouseLeftButtonUp()
        //{

        //}
        //public void MouseEnter(MouseEventArgs e)
        //{
        //    //ToolTip = $"({UCLeft}, {UCTop})\n({ScaleX:0.##}, {(ScaleX * ImageSize[0]):0.##}, {(ScaleX * ImageSize[1]):0.##})";
        //}
        public void MouseLeftButtonDownOnContentControl(ContentControl cc, PluginModel pm, MouseEventArgs e)
        {
            e.Handled = true;

            var CtrlDown = (Keyboard.Modifiers & ModifierKeys.Control) != 0;
            if (!CtrlDown || mv.AdornersSelectedList.Count == 0 || !MyCockpitViewModels.Any(t => t.NameUC.Equals(mv.AdornersSelectedList.ElementAt(0))))
            {
                RemoveAdorners();
                AddNewAdorner(cc, pm);
            }
            else
            {
                if (mv.AdornersSelectedList.Contains(pm.NameUC))
                {
                    RemoveAdorner(cc, pm);
                    UpdateFirstAdorner();
                }
                else
                {
                    AddNewAdorner(cc, pm, 2);
                }
            }

            if (mv.AdornersSelectedList.Count() == 0)
                eventAggregator.Publish(new DisplayPropertiesEvent(new[] { (PluginProperties)mv.LayoutMonitor }));
            else
                eventAggregator.Publish(new DisplayPropertiesEvent(mv.SortedDico[mv.AdornersSelectedList.ElementAt(0)].pm.GetProperties()));

        }

        public void MouseLeftButtonDownOnPanelView(IInputElement elem, Point pos, MouseEventArgs e)
        {
            //eventAggregator.Publish(new RemoveAdornerEvent());
            //RemoveAdorners();
            //eventAggregator.Publish(new DisplayPropertiesEvent(new[] { (PluginProperties)Layout, Appearance }));
        }
        #endregion


        #region Scale, rotation, XY translation usercontrol
        private double _scalex;
        public double ScaleX
        {
            get { return _scalex; }
            set
            {
                _scalex = value;
                NotifyOfPropertyChange(() => ScaleX);
            }
        }
        private double _angleRotation;
        public double AngleRotation
        {
            get { return _angleRotation; }
            set
            {
                _angleRotation = value;
                NotifyOfPropertyChange(() => AngleRotation);
            }
        }
        private double _ucleft;
        public double UCLeft
        {
            get { return _ucleft; }
            set
            {
                _ucleft = value;
                NotifyOfPropertyChange(() => UCLeft);
            }
        }
        private double _uctop;
        public double UCTop
        {
            get { return _uctop; }
            set
            {
                _uctop = value;
                NotifyOfPropertyChange(() => UCTop);
            }
        }
        #endregion

        #region ToolTip
        private string _tooltip;
        public string ToolTip
        {
            get { return _tooltip; }
            set
            {
                _tooltip = value;
                NotifyOfPropertyChange(() => ToolTip);
            }
        }
        #endregion


        public void ContentControlLoaded(ContentControl cc, PluginModel pm)
        {
            if (mv.SortedDico.ContainsKey(pm.NameUC))
                return;
            mv.SortedDico[pm.NameUC] = new Elements(cc, pm);

            RemoveAdorners();
            AddNewAdorner(cc, pm);

            eventAggregator.Publish(new DisplayPropertiesEvent(mv.SortedDico[mv.AdornersSelectedList.ElementAt(0)].pm.GetProperties()));
            cc.Focus();

        }

        private void RemoveAdorner(ContentControl cc, PluginModel pm)
        {
            mv.RemoveAdorner(cc, pm);
        }

        private void RemoveAdorners()
        {
            mv.RemoveAdorners();
        }

        private void AddNewAdorner(ContentControl cc, PluginModel pm, int color = 0)
        {
            mv.AddNewAdorner(cc, pm, color);
        }

        public void UpdateFirstAdorner()
        {
            mv.UpdateFirstAdorner();
        }



        #region DragOver & Drop
        void IDropTarget.DragOver(IDropInfo dropInfo)
        {
            if (dropInfo.Data is ToolBoxGroup)
            {
                var tbg = dropInfo.Data as ToolBoxGroup;
                var FullImage = tbg.SelectedToolBoxItem.FullImageName;
                tbg.AnchorMouse = new Point(0.0, 0.0);
                dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
                dropInfo.Effects = DragDropEffects.Move;
            }
        }

        void IDropTarget.Drop(IDropInfo dropInfo)
        {
            var tbg = dropInfo.Data as ToolBoxGroup;
            var selected = tbg.SelectedToolBoxItem;
            int left = (int)dropInfo.DropPosition.X;
            int top = (int)dropInfo.DropPosition.Y;
            var FullImage = (dropInfo.Data as ToolBoxGroup).SelectedToolBoxItem.FullImageName;
            var groupname = (dropInfo.Data as ToolBoxGroup).GroupName;

            var num = MyCockpitViewModels.Count;
            var nameUC = tbg.SelectedToolBoxItem.ShortImageName;

            //var nbr = MyCockpitViewModels.Select(t => t.NameUC.Equals(nameUC)).Count();
            //if (nbr > 0)
            //{
            //    nameUC = $"{nameUC}_{nbr}";
            //}

            if (mv.SortedDico.ContainsKey(nameUC))
            {
                var nbr = mv.SortedDico.Count(t => t.Key.StartsWith(nameUC));
                nameUC = $"{nameUC}_{nbr}";
            }

            Ninject.Parameters.Parameter[] param;

            Ninject.Parameters.Parameter[][] paramproperties = null;
            string[] properties;
            string model;
            var AngleSwitch = 90;
            if (groupname.StartsWith("PushButton"))
            {
                var FullImage1 = FullImage.Replace("_0.png", "_1.png");

                param = new Ninject.Parameters.Parameter[]
                {
                        new ConstructorArgument("settings", new object[]{                                                   //PushButton
                            true, this,                                                                                         //0  is in Mode Editor?
                            $"{nameUC}",                                                                                        //2  name of UC
                            new int[] { left, top, tbg.SelectedToolBoxItem.ImageWidth, tbg.SelectedToolBoxItem.ImageHeight, 0 },//3  [Left, Top, Width, Height, Angle]

                            new string[]{ FullImage, FullImage1 }, 0,                                                           //4  [images] & startimageposition
                            2d, 0.8d, (PushButtonGlyph)0, Colors.White,                                                         //6  Glyph: Thickness, Scale, Type, Color
                            "Hello", "1,1", "Franklin Gothic", "Normal", "Normal",                                              //10 Text, TextPushOffset, Family, Style, Weight
                            12d, new double[] { 0d, 0d, 0d, 0d },                                                               //15 Size, [padding L,T,R,B]
                            new int[] { 1, 1 },  Colors.White,                                                                  //17 [TextAlign H,V], TextColor

                            1                                                                                                   //19 Button Type
                                                                        }, true)
                };

                model = "Cockpit.Core.Plugins.Plugins.PushButton_ViewModel, Cockpit.Core.Plugins";
                //properties = new string[] { "Cockpit.GUI.Plugins.Properties.LayoutPropertyViewModel",
                //                            "Cockpit.GUI.Plugins.Properties.PushButtonAppearanceViewModel",
                //                            "Cockpit.GUI.Plugins.Properties.PushButtonBehaviorViewModel"};


                paramproperties = new Ninject.Parameters.Parameter[][]
                {
                    new Ninject.Parameters.Parameter[]
                    {
                        // Layout
                        new ConstructorArgument("settings", new object[]{
                            true, this,                                                                                         //0  In Mode Editor?
                            $"{nameUC}",                                                                                        //2  name of UC
                            new int[] { left, top, tbg.SelectedToolBoxItem.ImageWidth, tbg.SelectedToolBoxItem.ImageHeight, 0 } //3  [Left, Top, Width, Height, Angle]
                        }, true)
                    },
                        // Appearance
                    new Ninject.Parameters.Parameter[]
                    {
                        new ConstructorArgument("settings", new object[]{
                            true, mv,                                                                                         //0  In Mode Editor?
                            $"{nameUC}",                                                                                        //2  name of UC
                            new string[]{ FullImage, FullImage1 }, 0,                                                           //3  images, start image position
                            2d, 0.8d, (PushButtonGlyph)0, Colors.White,                                                         //5  Glyph: Thickness, Scale, Type, Color
                            "Hello", "1,1", "Franklin Gothic", "Normal", "Normal",                                              //9  Text, TextPushOffset, Family, Style, Weight
                            12d, new double[] { 0d, 0d, 0d, 0d },                                                               //14 Size, [padding L,T,R,B]
                            new int[] { 1, 1 },  Colors.White                                                                   //16 [TextAlign H,V], TextColor
                        }, true)
                    },
                        // Behavior
                    new Ninject.Parameters.Parameter[]
                    {
                        new ConstructorArgument("settings", new object[]{
                            true, this,                                                                                               //0  In Mode Editor?
                            $"{nameUC}",                                                                                        //1  name of UC
                            1                                                                                                   //2 Button Type
                        }, true)}
                    };


            }
            else if (groupname.StartsWith("Panel"))
            {

                param = new Ninject.Parameters.Parameter[]
                {
                        new ConstructorArgument("settings", new object[]{                                                   //Panel Button
                            true, mv,                                                                                         //0 is in Mode Editor?
                            $"{nameUC}",                                                                                        //2 name of UC
                            new int[] { left, top, tbg.SelectedToolBoxItem.ImageWidth, tbg.SelectedToolBoxItem.ImageHeight, 0 },//3 [Left, Top, Width, Height, Angle]

                            FullImage,                                                                                          //4 [images] 

                            2, 1d, 2, 3 }, true)
                };

                model = "Cockpit.GUI.Plugins.Panel_ViewModel";
                //properties = new string[] { "Cockpit.GUI.Plugins.Properties.LayoutPropertyViewModel",
                //                            "Cockpit.GUI.Plugins.Properties.PanelAppearanceViewModel"};
            }
            else
            {
                var FullImage1 = FullImage.Replace("_0.png", "_1.png");
                var FullImage2 = FullImage.Replace("_0.png", "_2.png");
                AngleSwitch = 0;
                param = new Ninject.Parameters.Parameter[]
                {
                        new ConstructorArgument("settings", new object[]{                                                   //Switch Button
                            true, this,                                                                                         //0 is in Mode Editor?
                            $"{nameUC}",                                                                                        //2 name of UC
                            new int[] { left, top, tbg.SelectedToolBoxItem.ImageWidth, tbg.SelectedToolBoxItem.ImageHeight, AngleSwitch },//3 [Left, Top, Width, Height, Angle]

                            new string[]{ FullImage, FullImage1, FullImage2 , "", "", "" }, 0,                                  //4 [images] & startimageposition

                            2, 1d, 2, 3 }, true)
                };

                model = "Cockpit.Core.Plugins.Plugins.Switch_ViewModel, Cockpit.Core.Plugins";
                //properties = new string[] { "", "", "" };
            }

            var typeClass = Type.GetType(model);
            //var viewmodel = Activator.CreateInstance(typeClass);
            var viewmodel = resolutionRoot.TryGet(typeClass, param);
            //var view = ViewLocator.LocateForModel(viewmodel, null, null);
            
            //ViewModelBinder.Bind(viewmodel, view, null);
            //var v = viewmodel as PluginModel;
            //v.ZoomFactorFromPluginModel = ZoomFactorFromMonitorViewModel;
            MyCockpitViewModels.Add((PluginModel)viewmodel);
        }

        #endregion
        public void Handle(Core.Plugins.Events.VisibilityPanelEvent message)
        {
            if (!NameUC.Equals(message.PanelName)) return;

            IsVisible = !IsVisible;
        }

        public void Handle(RemovePanelEvent message)
        {
            //if (MyCockpitViewModels == null) return;

            //if (message.IsPanel)
            //{
            //    if (message.NameUC.Equals(NameUC))
            //    {
            //        foreach (var item in MyCockpitViewModels.ToList())
            //        {
            //            if (item.ToString().Equals("Cockpit.GUI.Plugins.Panel_ViewModel"))
            //            {
            //                eventAggregator.Publish(new RemovePanelEvent(NameUC: item.NameUC, IsPanel: true));
            //                continue;
            //            }
            //            if (MyCockpitViewModels.Count() > 0)
            //            {
            //                MyCockpitViewModels.Remove(MyCockpitViewModels.Where(t => t.NameUC.Equals(item.NameUC)).First());
            //                mv.SortedDico.Remove(item.NameUC);
            //            }
            //        }
            //        MyCockpitViewModels = null;
            //        eventAggregator.Unsubscribe(this);
            //        return;
            //    }
            //}

            //if (MyCockpitViewModels.Any(t => t.NameUC.Equals(message.NameUC)))
            //{
            //    MyCockpitViewModels.Remove(mv.SortedDico[message.NameUC].pm);
            //    mv.SortedDico.Remove(message.NameUC);
            //}
        }
    }
}
