using Caliburn.Micro;
using Cockpit.GUI.Plugins;
using Cockpit.GUI.Views.Main;
using System.IO;
using System.Linq;
using System.Text;
using IEventAggregator = Cockpit.Core.Common.Events.IEventAggregator;

namespace Cockpit.GUI.Views.Profile.Panels
{
    public class ToolBoxViewModel : PanelViewModel
    {
        private readonly IEventAggregator eventAggregator;
        public ToolBoxViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            eventAggregator.Subscribe(this);

            Title = "ToolBox";
            IconName = "console-16.png";

            LoadThumb(@"J:\heliosDevices\Images");


            //MyCockpitViewModels = new BindableCollection<PluginModel>();
        }
        //public BindableCollection<PluginModel> MyCockpitViewModels;

        private BindableCollection<ToolBoxGroup> _toolBoxGroups;
        public BindableCollection<ToolBoxGroup> ToolBoxGroups
        {
            get { return _toolBoxGroups; }
            set
            {
                _toolBoxGroups = value;
                NotifyOfPropertyChange(() => ToolBoxGroups);
            }
        }

        public void LoadThumb(string parentDir)
        {
            ToolBoxGroups = new BindableCollection<ToolBoxGroup>();
            foreach (string subdir in Directory.GetDirectories(parentDir))
            {
                var groupname = Path.GetFileName(subdir);
                bool flag = groupname.StartsWith("Type");
                if (!flag) continue;

                var toolBoxItems = new BindableCollection<ToolBoxItem>();
                groupname = groupname.Replace("Type", "");

                foreach (string file in Directory.GetFiles(subdir))
                {
                    string shortImageName;
                    if (groupname.Equals("RotarySwitch"))
                    {
                        shortImageName = file.Split('\\').Last();
                    }
                    else
                    {
                        var End = "_0.png";
                        if (groupname.Contains("Panel"))
                            End = ".png";
                        else if (!file.EndsWith(End)) continue;

                        shortImageName = file.Split('\\').Last().Replace(End, "");
                    }
                    //System.Drawing.Image img = System.Drawing.Image.FromStream;

                    getSizeOfImage(file, out int width, out int height);

                    toolBoxItems.Add(new ToolBoxItem
                    {
                        FullImageName = file,
                        ShortImageName = shortImageName,
                        ImageHeight = height,
                        ImageWidth = width,
                    });


                    void getSizeOfImage(string filename, out int w, out int h)
                    {
                        //using (var imageStream = File.OpenRead(file))
                        //{
                        //    var decoder = BitmapDecoder.Create(imageStream, BitmapCreateOptions.None, BitmapCacheOption.None);
                        //    h = decoder.Frames[0].PixelHeight;
                        //    w = decoder.Frames[0].PixelWidth;
                        //}

                        using (BinaryReader b = new BinaryReader(File.Open(file, FileMode.Open)))
                        {
                            b.BaseStream.Seek(1, SeekOrigin.Begin);
                            var p = b.ReadBytes(3);
                            string bytesAsString = Encoding.UTF8.GetString(p);
                            b.BaseStream.Seek(16, SeekOrigin.Begin);
                            w = (b.ReadByte() << 24) + (b.ReadByte() << 16) + (b.ReadByte() << 8) + b.ReadByte();
                            h = (b.ReadByte() << 24) + (b.ReadByte() << 16) + (b.ReadByte() << 8) + b.ReadByte();
                        }

                    }

                }

                ToolBoxGroups.Add(new ToolBoxGroup { GroupName = groupname, ToolBoxItems = toolBoxItems });
            }
        }
    }
}
