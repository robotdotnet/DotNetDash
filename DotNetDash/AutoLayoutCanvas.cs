using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace DotNetDash
{
    public class AutoLayoutCanvas : Canvas
    {
        private static readonly DependencyProperty GivenInitialPlacementProperty = DependencyProperty.RegisterAttached("GivenInitialPlacement", typeof(bool), typeof(AutoLayoutCanvas), new PropertyMetadata(false));

        protected override void OnVisualParentChanged(DependencyObject oldParent)
        {
            base.OnVisualParentChanged(oldParent);
            FrameworkElement parent = this;
            while (!(parent is Border))
            {
                parent = VisualTreeHelper.GetParent(parent) as FrameworkElement;
            }

            parent.MouseDown += AutoLayoutCanvas_MouseDown;
        }

        private void AutoLayoutCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Keyboard.Focus(this);
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            var pathGeometry = new PathGeometry();
            pathGeometry.FillRule = FillRule.Nonzero;
            foreach (UIElement child in Children.OfType<UIElement>()
                        .Where(element => (bool)element.GetValue(GivenInitialPlacementProperty)))
            {
                var left = (double)child.GetValue(LeftProperty);
                var top = (double)child.GetValue(TopProperty);
                var topLeft = new Point(double.IsNaN(left) ? 0 : left, double.IsNaN(top) ? 0 : top);
                pathGeometry.AddGeometry(new RectangleGeometry(new Rect(topLeft, child.DesiredSize)));
            }
            var bounds = pathGeometry.Bounds == Rect.Empty ? new Rect(): pathGeometry.Bounds;
            foreach (UIElement child in Children.OfType<UIElement>()
                        .Where(element => !(bool)element.GetValue(GivenInitialPlacementProperty)))
            {
                if (bounds.Right < arrangeSize.Width)
                {
                    child.SetValue(LeftProperty, bounds.Right);
                    bounds = new Rect(bounds.TopLeft, new Point(bounds.Right + child.DesiredSize.Width, bounds.Bottom));
                    child.SetValue(GivenInitialPlacementProperty, true); 
                }
                else
                {
                    child.SetValue(TopProperty, bounds.Bottom);
                    bounds = new Rect(bounds.TopLeft, new Point(bounds.Right, bounds.Bottom + child.DesiredSize.Height));
                    child.SetValue(GivenInitialPlacementProperty, true);
                }
            }
            return base.ArrangeOverride(arrangeSize);
        }
    }
}
