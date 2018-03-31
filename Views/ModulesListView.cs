using System.Windows.Controls;

namespace CSharpLab5.Views
{
    class ModulesListView : UserControl
    {
        public ModulesListView()
        {
            var listView = new ListView
            {
                ItemTemplate = new System.Windows.DataTemplate(typeof(object))
            };

            listView.SetBinding(ItemsControl.ItemsSourceProperty, "Modules");
        }

    }
}
