using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using D3Sharp.Demo.Model;

namespace D3Sharp.Demo.Controls
{
    /// <summary>
    /// ForceNodeControl.xaml 的交互逻辑
    /// </summary>
    public partial class ForceNodeControl : UserControl
    {
        private DemoNode node;
        public ForceNodeControl()
        {
            InitializeComponent();
        }

        public DemoNode Node
        {
            get => node;
            set
            {
                this.DataContext = node = value;
                if (node.Type == NodeType.Interface)
                {
                    textBlock.HorizontalAlignment = HorizontalAlignment.Center;
                    textBlock.VerticalAlignment = VerticalAlignment.Center;
                    rect.Visibility = Visibility.Collapsed;
                    ellipse.Visibility = Visibility.Visible;
                }
                else
                {
                    textBlock.HorizontalAlignment = HorizontalAlignment.Left;
                    textBlock.VerticalAlignment = VerticalAlignment.Top;
                    rect.Visibility = Visibility.Visible;
                    ellipse.Visibility = Visibility.Collapsed;
                    if (node.Type == NodeType.Abstract)
                    {
                        rect.Stroke = new SolidColorBrush(Color.FromRgb(0x21, 0x21, 0x2c));
                        rect.StrokeThickness = 2;
                    }
                }
            }
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (node == null)
            {
                //node.Width = this.ActualWidth;
                //node.Height = this.ActualHeight;
            }
        }
    }
}
