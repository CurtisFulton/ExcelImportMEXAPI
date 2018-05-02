using Microsoft.Win32;
using System.IO;
using System.Windows.Controls;

namespace ImportTemplateView
{
    /// <summary>
    /// Interaction logic for FileOpenerView.xaml
    /// </summary>
    public partial class FileOpenerView : UserControl
    {
        public FileOpenerView()
        {
            InitializeComponent();
            DataContext = this;
        }
        
        private void SelectFileClicked(object sender, System.Windows.RoutedEventArgs e)
        {
            OpenFileDialog fileDiag = new OpenFileDialog();
            fileDiag.Filter = "Excel Files |*.csv;*.xlsx";
            fileDiag.Multiselect = false;
            fileDiag.InitialDirectory = string.IsNullOrWhiteSpace(SelectedFileString.Text) ? @"C:\" : Path.GetDirectoryName(SelectedFileString.Text);
            
            if (!fileDiag.ShowDialog() ?? false)
                return;

            SelectedFileString.Text = fileDiag.FileName;
        }
    }
}
