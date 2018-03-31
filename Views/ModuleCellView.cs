
using System.Net.Mime;
using System.Windows.Controls;

namespace CSharpLab5.Views
{
    class ModuleCellView : UserControl
    {
        public ModuleCellView()
        {
            var panel = new StackPanel
            {
                Orientation = Orientation.Horizontal
            };

            var nameLabel = new Label();
            nameLabel.SetBinding(ContentProperty, "ModuleName");

            var filenameLabel = new Label();
            nameLabel.SetBinding(ContentProperty, "FileName");
        }
    }
}
