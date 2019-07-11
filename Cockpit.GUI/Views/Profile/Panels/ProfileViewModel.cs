using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading;
using Cockpit.GUI.Views.Main;

namespace Cockpit.GUI.Views.Profile.Panels
{
    public class ProfileViewModel : PanelViewModel
    {

        public ProfileViewModel()
        {
            Title = "Profile";
            IconName = "console-16.png";
        }
    }
}
