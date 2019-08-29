using Caliburn.Micro;
using Cockpit.RUN.Common;
using Ninject;
using Ninject.Parameters;
using Ninject.Syntax;
using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace Cockpit.RUN.Test
{
    public class MainViewModel
    {
        public BindableCollection<PluginModel> MyCockpitPlugins { get; set; }
        private readonly IResolutionRoot resolutionRoot;
        public MainViewModel(IResolutionRoot resolutionRoot)
        {
            this.resolutionRoot = resolutionRoot;
            MyCockpitPlugins = new BindableCollection<PluginModel>();
            Ninject.Parameters.Parameter[] param = null;
            var FullImage = @"J:\heliosDevices\Images\TypePushButton\red_0.png";
            var groupname = "PushButton";
            var nameUC = "mfd";
            var model = "";

            if (groupname.StartsWith("PushButton"))
            {
                var FullImage1 = FullImage.Replace("_0.png", "_1.png");

                param = new Ninject.Parameters.Parameter[]
                {
                        new ConstructorArgument("settings", new object[]{                                                   //PushButton
                            false, this,                                                                                         //0  is in Mode Editor?
                            $"{nameUC}",                                                                                        //2  name of UC
                            new int[] { 0, 0, 100, 100, 0 },//3  [Left, Top, Width, Height, Angle]

                            new string[]{ FullImage, FullImage1 }, 0,                                                           //4  [images] & startimageposition
                            2d, 0.8d, (int)0, Colors.White,                                                         //6  Glyph: Thickness, Scale, Type, Color
                            "Hello", "1,1", "Franklin Gothic", "Normal", "Normal",                                              //10 Text, TextPushOffset, Family, Style, Weight
                            12d, new double[] { 0d, 0d, 0d, 0d },                                                               //15 Size, [padding L,T,R,B]
                            new int[] { 1, 1 },  Colors.White,                                                                  //17 [TextAlign H,V], TextColor

                            1                                                                                                   //19 Button Type
                                                                        }, true)
                };

                model = "Cockpit.RUN.ViewModels.PushButton_ViewModel, Cockpit.RUN.ViewModels";
            }



            var typeClass = Type.GetType(model);
            //var viewmodel = Activator.CreateInstance(typeClass, param);
            var viewmodel = resolutionRoot.TryGet(typeClass, param);
            //var t = ViewLocator.TransformName("Cockpit.RUN.ViewModels.PushButton_ViewModel", param);
            //var view = ViewLocator.LocateForModel(viewmodel, null, null);
            //ViewModelBinder.Bind(viewmodel, view, null);

            var v = viewmodel as PluginModel;
            var w = (PluginModel)viewmodel;
            MyCockpitPlugins.Add(w);

        }

        public void ContentControlLoaded(ContentControl cc, PluginModel pm)
        {


        }
    }
}
