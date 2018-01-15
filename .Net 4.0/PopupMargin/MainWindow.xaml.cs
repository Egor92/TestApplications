using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PopupMargin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void PopupPeple_OnLoaded(object sender, EventArgs e)
        {
            PopupPeple.CustomPopupPlacementCallback = (popupSize, targetSize, offset) =>
            {
                return new[]
                {
                    new CustomPopupPlacement(new Point(0, targetSize.Height - 1), PopupPrimaryAxis.None),
                };
            };
        }
    }
}