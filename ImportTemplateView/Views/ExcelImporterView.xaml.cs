using System.Windows.Controls;

namespace ImportTemplateView
{
    /// <summary>
    /// Interaction logic for ExcelImporterView.xaml
    /// </summary>
    public partial class ExcelImporterView : UserControl
    {
        public ExcelImporterView()
        {
            InitializeComponent();
        }

        private void OnButtonClicked(object sender, System.Windows.RoutedEventArgs e)
        {
            TabControl tabControl = (TabControl)((TabItem)this.Parent).Parent;

            tabControl.SelectedIndex = 0;
        }
    }
}
