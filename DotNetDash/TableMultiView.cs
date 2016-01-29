using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace DotNetDash
{
    [TemplatePart(Name = "PART_Presenter", Type = typeof(ContentPresenter))]
    public class TableMultiView : Selector
    {
        public TableMultiView()
        {
            AllowDrop = true;
        }

        private ContentPresenter presenter;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            presenter = (ContentPresenter)Template.FindName("PART_Presenter", this);
            TrySetNewContent(SelectedItem);
        }

        protected override void OnItemTemplateChanged(DataTemplate oldItemTemplate, DataTemplate newItemTemplate)
        {
            base.OnItemTemplateChanged(oldItemTemplate, newItemTemplate);
            TrySetNewContent(SelectedItem);
        }

        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);
            if(newValue.OfType<object>().Any())
            {
                SelectedItem = newValue.OfType<object>().ElementAt(0);
            }
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);
            if (e.AddedItems.Count > 0)
            {
                var newPresenter = e.AddedItems[0];
                TrySetNewContent(newPresenter);
            }
        }

        private void TrySetNewContent(object newPresenter)
        {
            if (ItemTemplate == null || presenter == null) return;
            var newContent = (FrameworkElement)ItemTemplate.LoadContent();
            newContent.DataContext = newPresenter;
            presenter.Content = newContent;
        }

        // The following code is taken from StackOverflow from answers written by Kevin Cruijssen
        private double firstXPos, firstYPos;
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            // In this event, we get the current mouse position on the control to use it in the MouseMove event.
            var canvas = GetContainingPanelAsCanvas();
            if (canvas == null) return;

            firstXPos = e.GetPosition(this).X;
            firstYPos = e.GetPosition(this).Y;

            // Put the image currently being dragged on top of the others
            var top = Panel.GetZIndex(this);
            foreach (UIElement child in canvas.Children)
                if (top < Panel.GetZIndex(child))
                    top = Panel.GetZIndex(child);
            Panel.SetZIndex(this, top + 1);
            Mouse.Capture(this);
        }

        private Canvas GetContainingPanelAsCanvas()
        {
            return VisualTreeHelper.GetParent(TemplatedParent) as Canvas;
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            var canvas = GetContainingPanelAsCanvas();
            if (canvas == null) return;

            // Put the image currently being dragged on top of the others
            var top = Panel.GetZIndex(this);
            foreach (UIElement child in canvas.Children)
                if (top > Panel.GetZIndex(child))
                    top = Panel.GetZIndex(child);
            Panel.SetZIndex(this, top + 1);
            Mouse.Capture(null);
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var canvas = GetContainingPanelAsCanvas();
                if (canvas == null) return;

                var newLeft = e.GetPosition(canvas).X - firstXPos - canvas.Margin.Left;
                // newLeft inside canvas right-border?
                if (newLeft > canvas.Margin.Left + canvas.ActualWidth - ActualWidth)
                    newLeft = canvas.Margin.Left + canvas.ActualWidth - ActualWidth;
                // newLeft inside canvas left-border?
                else if (newLeft < canvas.Margin.Left)
                    newLeft = canvas.Margin.Left;
                SetValue(Canvas.LeftProperty, newLeft);

                var newTop = e.GetPosition(canvas).Y - firstYPos - canvas.Margin.Top;
                // newTop inside canvas bottom-border?
                if (newTop > canvas.Margin.Top + canvas.ActualHeight - ActualHeight)
                    newTop = canvas.Margin.Top + canvas.ActualHeight - ActualHeight;
                // newTop inside canvas top-border?
                else if (newTop < canvas.Margin.Top)
                    newTop = canvas.Margin.Top;
                SetValue(Canvas.TopProperty, newTop);
            }
        }
    }
}
