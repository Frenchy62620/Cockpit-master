namespace Cockpit.Core.Plugins.Plugins.Properties
{
    //[HeliosPropertyEditor("*", "Layout")]
    public partial class LayoutPropertyViewX  /*HeliosPropertyEditor, IDataErrorInfo*/
    {
        public LayoutPropertyViewX()
        {
            InitializeComponent();
        }

        //#region Properties

        //public override string Category
        //{
        //    get
        //    {
        //        return "Layout";
        //    }
        //}

        //public string VisualName
        //{
        //    get { return (string)GetValue(VisualNameProperty); }
        //    set { SetValue(VisualNameProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for VisualName.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty VisualNameProperty =
        //    DependencyProperty.Register("VisualName", typeof(string), typeof(LayoutPropertyEditor), new PropertyMetadata(""));

        //#endregion

        //protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        //{
        //    if (e.Property == ControlProperty)
        //    {
        //        VisualName = Control.Name;
        //    }
        //    if (e.Property == VisualNameProperty)
        //    {
        //        if ((this as IDataErrorInfo)["VisualName"] == null)
        //        {
        //            Control.Name = VisualName;
        //        }
        //    }

        //    base.OnPropertyChanged(e);
        //}

        //string IDataErrorInfo.Error
        //{
        //    get { return null; }
        //}

        //string IDataErrorInfo.this[string columnName]
        //{
        //    get
        //    {
        //        if (columnName.Equals("VisualName"))
        //        {
        //            if (string.IsNullOrWhiteSpace(VisualName))
        //            {
        //                return "Name can not be blank.";
        //            }
        //            if (!System.Text.RegularExpressions.Regex.IsMatch(VisualName, "^[a-zA-Z0-9_ ]*$"))
        //            {
        //                return "Name must not contain special characters.";
        //            }
        //            if (Control != null && Control.Parent != null && !VisualName.Equals(Control.Name) && Control.Parent.Children.ContainsKey(VisualName))
        //            {
        //                return "Name must be unique.";
        //            }
        //        }
        //        return null;
        //    }
        //}
    }
}
