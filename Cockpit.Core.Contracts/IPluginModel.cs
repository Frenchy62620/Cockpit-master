namespace Cockpit.Core.Contracts
{
    public interface IPluginModel
    {
        //double Left { get; set; }
        //double Top { get; set; }
        //double Width { get; set; }
        //double Height { get; set; }

        double ZoomFactorFromPluginModel { get; set; }
        //double ScaleX { get; set; }
        //double ScaleY { get; set; }

        //string NameUC { get; set; }

        IPluginProperty[] GetProperties();
    }
}
