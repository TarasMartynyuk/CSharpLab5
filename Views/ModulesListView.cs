using System.Windows.Controls;
using System.Windows.Data;

namespace CSharpLab5.Views
{
    class ModulesInfoView : UserControl
    {
        public ModulesInfoView()
        {
            HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;

            var grid = new Grid();

            var dataGrid = new DataGrid()
            {
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                AutoGenerateColumns = false
            };

            dataGrid.Columns.Add(new DataGridTextColumn()
            {
                Header = "ModuleName",
                Binding = new Binding("ModuleName"),
            });

            dataGrid.Columns.Add(new DataGridTextColumn()
            {
                Header = "FileName",
                Binding = new Binding("FileName"),
            });

            dataGrid.SetBinding(ItemsControl.ItemsSourceProperty, "Modules");

            grid.Children.Add(dataGrid);

            Content = grid;
        }

    }
}
