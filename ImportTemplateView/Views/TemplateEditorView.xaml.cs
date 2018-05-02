using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

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

            SetControlProperties();
        }

        private void SetControlProperties()
        {
            // Set the DateGrid Properties
            TemplateDataGrid.AutoGenerateColumns = false;
            TemplateDataGrid.HeadersVisibility = DataGridHeadersVisibility.Column;
            TemplateDataGrid.CanUserAddRows = false;
            TemplateDataGrid.AlternationCount = 2;
            TemplateDataGrid.AlternatingRowBackground = Brushes.LightGray;
            TemplateDataGrid.GridLinesVisibility = DataGridGridLinesVisibility.None;
            TemplateDataGrid.MinRowHeight = 30.0;

            // Set the Column Header Style
            Style headerStyle = new Style();
            headerStyle.Setters.Add(new Setter(DataGrid.BackgroundProperty, Brushes.LightGray));
            headerStyle.Setters.Add(new Setter(DataGrid.BorderBrushProperty, Brushes.Black));
            headerStyle.Setters.Add(new Setter(DataGrid.MinHeightProperty, 30.0));
            headerStyle.Setters.Add(new Setter(DataGrid.PaddingProperty, new Thickness(10, 4, 10, 7)));
            headerStyle.Setters.Add(new Setter(DataGrid.VerticalContentAlignmentProperty, VerticalAlignment.Center));
            headerStyle.Setters.Add(new Setter(DataGrid.BorderThicknessProperty, new Thickness(0, 0, 1, 1)));
            TemplateDataGrid.ColumnHeaderStyle = headerStyle;

            // Set the Cell style
            Style cellStyle = new Style();
            cellStyle.Setters.Add(new Setter(DataGridCell.VerticalAlignmentProperty, VerticalAlignment.Center));
            TemplateDataGrid.CellStyle = cellStyle;
        }

        private void OnBackButtonClicked(object sender, RoutedEventArgs e)
        {
            TabControl tabControl = (TabControl)((TabItem)this.Parent).Parent;

            tabControl.SelectedIndex = 1; 
        }
    }
}
