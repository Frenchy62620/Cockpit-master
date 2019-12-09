using System;
using System.Windows;
using System.Windows.Controls;

namespace Cockpit.Core.Plugins.Plugins
{
    /// <summary>
    /// Logique d'interaction pour SwitchOn_Off_On_View.xaml
    /// </summary>
    public partial class PushButton_View : UserControl
    {
        public PushButton_View()
        {
            InitializeComponent();
            System.Diagnostics.Debug.WriteLine($"entree {this}");
        }

#if DEBUG
        ~PushButton_View()
        {
            System.Diagnostics.Debug.WriteLine($"sortie {this}");
        }
#endif
    }
}
