﻿using System.Windows.Controls;

namespace Cockpit.GUI.Plugins
{
    /// <summary>
    /// Logique d'interaction pour SwitchOn_Off_On_View.xaml
    /// </summary>
    public partial class Panel_View : UserControl
    {
        public Panel_View()
        {
            InitializeComponent();
            System.Diagnostics.Debug.WriteLine($"entree {this}");
        }
#if DEBUG
        ~Panel_View()
        {
            System.Diagnostics.Debug.WriteLine($"sortie {this}");
        }
#endif
    }
}
