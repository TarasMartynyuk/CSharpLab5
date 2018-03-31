using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CSharpLab5.Views
{
    class ModuleCellView : UserControl
    {
        public ModuleCellView()
        {
            Background = Brushes.DarkGreen;
            VerticalAlignment = VerticalAlignment.Stretch;
            Height = 50;
            var panel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                VerticalAlignment = VerticalAlignment.Stretch
            };

            var nameLabel = new Label()
            {
                Content = "foo",
                VerticalAlignment = VerticalAlignment.Stretch
            };
            //nameLabel.SetBinding(ContentProperty, "ModuleName");

            var filenameLabel = new Label()
            {
                Content = "bar",
                VerticalAlignment = VerticalAlignment.Stretch,
            };
            //filenameLabel.SetBinding(ContentProperty, "FileName");


            panel.Children.Add(nameLabel);
            panel.Children.Add(filenameLabel);

            Content = panel;
        }
    }
}
