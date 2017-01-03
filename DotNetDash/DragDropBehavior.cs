using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using Serilog;

namespace DotNetDash
{
    class DragDropBehavior : Behavior<ContentPresenter>
    {
        private ILogger logger;

        protected override void OnAttached()
        {
            base.OnAttached();
            if (AssociatedObject != null)
            {
                AssociatedObject.AllowDrop = true;
                AssociatedObject.MouseLeftButtonDown += OnMouseLeftButtonDown;
                AssociatedObject.MouseLeftButtonUp += OnMouseLeftButtonUp;
                AssociatedObject.MouseMove += OnMouseMove;
                logger = Log.ForContext(AssociatedObject.GetType());
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            if (AssociatedObject != null)
            {
                AssociatedObject.PreviewMouseLeftButtonDown -= OnMouseLeftButtonDown;
                AssociatedObject.PreviewMouseLeftButtonUp -= OnMouseLeftButtonUp;
                AssociatedObject.PreviewMouseMove -= OnMouseMove; 
            }
        }

        // The following code is taken from StackOverflow from answers written by Kevin Cruijssen
        private bool isDragging;
        private double firstXPos, firstYPos;
        protected void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // In AssociatedObject event, we get the current mouse position on the control to use it in the MouseMove event.
            var canvas = GetContainingPanelAsCanvas();
            if (canvas == null) return;

            firstXPos = e.GetPosition(AssociatedObject).X;
            firstYPos = e.GetPosition(AssociatedObject).Y;

            // Put the image currently being dragged on top of the others
            var top = Panel.GetZIndex(AssociatedObject);
            foreach (UIElement child in canvas.Children)
                if (top < Panel.GetZIndex(child))
                    top = Panel.GetZIndex(child);
            Panel.SetZIndex(AssociatedObject, top + 1);
            isDragging = true;
            Mouse.Capture(AssociatedObject);
            e.Handled = true;
            logger.Debug("Beginning drag operation");
        }

        private Canvas GetContainingPanelAsCanvas()
        {
            return VisualTreeHelper.GetParent(AssociatedObject) as Canvas;
        }

        protected void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var canvas = GetContainingPanelAsCanvas();
            if (canvas == null) return;

            // Put the image currently being dragged on top of the others
            var top = Panel.GetZIndex(AssociatedObject);
            foreach (UIElement child in canvas.Children)
                if (top > Panel.GetZIndex(child))
                    top = Panel.GetZIndex(child);
            Panel.SetZIndex(AssociatedObject, top + 1);
            isDragging = false;
            Mouse.Capture(null);
            e.Handled = true;
            logger.Debug("Dropping element");
        }
        protected void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && isDragging)
            {
                var canvas = GetContainingPanelAsCanvas();
                if (canvas == null) return;

                var newLeft = e.GetPosition(canvas).X - firstXPos - canvas.Margin.Left;
                // newLeft inside canvas right-border?
                if (newLeft > canvas.Margin.Left + canvas.ActualWidth - AssociatedObject.ActualWidth)
                    newLeft = canvas.Margin.Left + canvas.ActualWidth - AssociatedObject.ActualWidth;
                // newLeft inside canvas left-border?
                else if (newLeft < canvas.Margin.Left)
                    newLeft = canvas.Margin.Left;
                AssociatedObject.SetValue(Canvas.LeftProperty, newLeft);

                var newTop = e.GetPosition(canvas).Y - firstYPos - canvas.Margin.Top;
                // newTop inside canvas bottom-border?
                if (newTop > canvas.Margin.Top + canvas.ActualHeight - AssociatedObject.ActualHeight)
                    newTop = canvas.Margin.Top + canvas.ActualHeight - AssociatedObject.ActualHeight;
                // newTop inside canvas top-border?
                else if (newTop < canvas.Margin.Top)
                    newTop = canvas.Margin.Top;
                AssociatedObject.SetValue(Canvas.TopProperty, newTop);
            }
            e.Handled = true;
        }
    }
}
