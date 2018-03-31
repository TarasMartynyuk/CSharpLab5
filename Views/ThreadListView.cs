using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace CSharpLab5.Views
{
    class ThreadListView : UserControl
    {
        public ThreadListView()
        {
            HorizontalAlignment = HorizontalAlignment.Stretch;

            var grid = new Grid();

            var dataGrid = new DataGrid()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                AutoGenerateColumns = false
            };

            dataGrid.Columns.Add(new DataGridTextColumn()
            {
                Header = "Id",
                Binding = new Binding("Id"),
            });

            dataGrid.Columns.Add(new DataGridTextColumn()
            {
                Header = "StartTime",
                Binding = new Binding("StartTime"),
            });

            dataGrid.Columns.Add(new DataGridTextColumn()
            {
                Header = "ThreadState",
                Binding = new Binding("ThreadState"),
            });

            dataGrid.SetBinding(ItemsControl.ItemsSourceProperty, "Threads");

            grid.Children.Add(dataGrid);

            Content = grid;
        }
    }
}
