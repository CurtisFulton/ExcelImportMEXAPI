using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ImportTemplateView
{
    /// <summary>
    /// Interaction logic for TemplateEditorView.xaml
    /// </summary>
    public partial class TemplateEditorView : UserControl
    {
        public TemplateEditorView()
        {
            InitializeComponent();
            Binding bind = new Binding();
            bind.Path = new PropertyPath("");
        }

        private void OnBackButtonClicked(object sender, RoutedEventArgs e)
        {
            TabControl tabControl = (TabControl)((TabItem)this.Parent).Parent;

            tabControl.SelectedIndex = 1; 
        }
    }
}
