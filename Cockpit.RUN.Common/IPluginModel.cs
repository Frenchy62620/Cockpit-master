using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cockpit.RUN.Common
{
    public interface IPluginModel
    {

        string NameUC { get; set; }

        double Width { get; set; }
        double Height { get; set; }
        double Left { get; set; }
        double Top { get; set; }

        double ZoomFactorFromPluginModel { get; set; }

    }
}
