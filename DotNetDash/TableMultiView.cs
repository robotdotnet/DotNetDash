using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace DotNetDash
{
    [TemplatePart(Name = "PART_ViewsMenu", Type = typeof(MenuItem))]
    public class TableMultiView : Selector
    {
        public TableMultiView()
        {
            AllowDrop = true;
        }
        
        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }
        
        public static readonly DependencyProperty HeaderTemplateProperty =
            DependencyProperty.Register(nameof(HeaderTemplate), typeof(DataTemplate), typeof(TableMultiView));
        
        private MenuItem viewMenu;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            viewMenu = (MenuItem)Template.FindName("PART_ViewsMenu", this);
            viewMenu.AddHandler(MenuItem.ClickEvent, (RoutedEventHandler)OnViewMenuItemClicked);
        }

        private void OnViewMenuItemClicked(object sender, RoutedEventArgs args)
        {
            if (args.OriginalSource != viewMenu)
            {
                SelectedItem = ((MenuItem)args.OriginalSource).Header;
            }
        }

        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);
            if(newValue.OfType<object>().Any())
            {
                SelectedItem = newValue.OfType<object>().ElementAt(0);
            }
        }

        private DragDropBehavior behavior = new DragDropBehavior();

        protected override void OnVisualParentChanged(DependencyObject oldParent)
        {
            base.OnVisualParentChanged(oldParent);
            behavior.Detach();
            behavior.Attach(VisualParent);
        }
    }
}
