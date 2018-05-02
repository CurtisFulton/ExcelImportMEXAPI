using ImportTemplateView;
using System.Windows;

namespace ImportTemplateView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainWindowViewModel();
            SetStyles();
        }

        private void SetStyles()
        {
            // Hide the tab item headers
            Style tabStyle = new Style();
            tabStyle.Setters.Add(new Setter(UIElement.VisibilityProperty, Visibility.Collapsed));
            TabControl.ItemContainerStyle = tabStyle;
        }
    }
}
