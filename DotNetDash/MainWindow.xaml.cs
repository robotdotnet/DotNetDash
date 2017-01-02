using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DotNetDash
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [Export]
    public partial class MainWindow : Window
    {
        private MainWindow()
        {
            InitializeComponent();
        }

        [ImportingConstructor]
        public MainWindow(NetworkTablesRoot viewModel)
            :this()
        {
            DataContext = viewModel;
        }
    }
}
