using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using D3Sharp.Demo.Controls;
using D3Sharp.Demo.Helps;
using D3Sharp.Demo.ViewModel;

namespace D3Sharp.Demo.Force
{
    /// <summary>
    /// ForceDemoPage.xaml 的交互逻辑
    /// </summary>
    public partial class ForceDemoPage : Page
    {
        ForceDemoViewModel viewModel;
        CenterXyConverter xyConverter;

        public ForceDemoPage()
        {
            xyConverter = new CenterXyConverter();
            InitializeComponent();
            animation = (Storyboard)Resources["copyButtonStoryboard"];
            DataContext = viewModel = new ForceDemoViewModel();
            loadComponents();
        }

        private void loadComponents()
        {
            mark.SetBinding(Canvas.LeftProperty, new Binding
            {
                Path = new PropertyPath("Cx"),
                UpdateSourceTrigger=UpdateSourceTrigger.PropertyChanged,
                Converter = xyConverter,
                ConverterParameter = new ControlXy { Control = mark, Target = "Width" }
            });
            mark.SetBinding(Canvas.TopProperty, new Binding
            {
                Path = new PropertyPath("Cy"),
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                Converter = xyConverter,
                ConverterParameter = new ControlXy { Control = mark, Target = "Height" }
            });

            foreach (var item in viewModel.Links)
            {
                var line = new Line();
                line.DataContext = item;
                line.ToolTip = item.Relationship.ToString();
                line.StrokeThickness = 1.5;
                switch (item.Relationship)
                {
                    case Model.Relationship.Realization:
                        line.Stroke = Brushes.Red;
                        break;
                    case Model.Relationship.Generalization:
                        line.Stroke = Brushes.Orange;
                        break;
                    case Model.Relationship.Association:
                        line.Stroke = Brushes.Yellow;
                        break;
                    case Model.Relationship.Aggregation:
                        line.Stroke = Brushes.Green;
                        break;
                    case Model.Relationship.Composition:
                        line.Stroke = Brushes.Blue;
                        break;
                    case Model.Relationship.Dependency:
                        line.Stroke = Brushes.Purple;
                        break;
                    default:
                        line.Stroke = Brushes.Gray;
                        break;
                }
                line.SetBinding(Line.X1Property, new Binding { Path = new PropertyPath("Source.X") });
                line.SetBinding(Line.X2Property, new Binding { Path = new PropertyPath("Target.X") });
                line.SetBinding(Line.Y1Property, new Binding { Path = new PropertyPath("Source.Y") });
                line.SetBinding(Line.Y2Property, new Binding { Path = new PropertyPath("Target.Y") });
                canvas.Children.Add(line);

                //var block = new TextBlock();
                //block.DataContext = item;
                //block.FontSize = 9; block.FontFamily = new FontFamily("consloas");
                //block.SetBinding(TextBlock.TextProperty, new Binding { Path = new PropertyPath("Relationship") });
                //block.SetBinding(Canvas.LeftProperty, new Binding
                //{
                //    Path = new PropertyPath("MiddleX"),
                //    Converter = xyConverter,
                //    ConverterParameter = new ControlXy { Control = block, Target = "X" }
                //});
                //block.SetBinding(Canvas.TopProperty, new Binding
                //{
                //    Path = new PropertyPath("MiddleY"),
                //    Converter = xyConverter,
                //    ConverterParameter = new ControlXy { Control = block, Target = "Y" }
                //});
                //canvas.Children.Add(block);
            }

            var nodes = viewModel.Nodes;
            for (int i = nodes.Count - 1; i > -1; i--)
            {
                var item = nodes[i];
                ForceNodeControl ele = new ForceNodeControl();
                ele.Node = item;

                //ele.Width = ele.Height = shapeWidth;

                ele.SetBinding(Canvas.LeftProperty, new Binding
                {
                    Path = new PropertyPath("X"),
                    Converter = xyConverter,
                    ConverterParameter = new ControlXy { Control = ele, Target = "X" }
                });
                ele.SetBinding(Canvas.TopProperty, new Binding
                {
                    Path = new PropertyPath("Y"),
                    Converter = xyConverter,
                    ConverterParameter = new ControlXy { Control = ele, Target = "Y" }
                });
                ele.MouseDoubleClick += Ele_MouseDoubleClick;

                this.canvas.Children.Add(ele);
            }
        }
        private void Start_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Excute();
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            var cx = this.canvas.ActualWidth / 2;
            var cy = this.canvas.ActualHeight / 2;
            viewModel.Reset(cx, cy);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            initPostions();
            viewModel.Excute();
        }

        private void initPostions()
        {
            var cx = this.canvas.ActualWidth / 2;
            var cy = this.canvas.ActualHeight / 2;
            viewModel.Cx = cx;
            viewModel.Cy = cy;
            viewModel.XX = cx;
            viewModel.YY = cy;
            viewModel.BoundedX2 = canvas.ActualWidth - viewModel.BoundedX1;
            viewModel.BoundedY2 = canvas.ActualHeight - viewModel.BoundedY1;
            foreach (var item in viewModel.Nodes)
            {
                item.X = cx;
                item.Y = cy;//start at center view
            }
            viewModel.FixedItem = viewModel.Nodes[0];
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            if (viewModel == null) return;
            viewModel.RestoreFixedItem();
            viewModel.Excute();
        }

        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            if (viewModel == null) return;
            viewModel.FixedItem = null;
            viewModel.Excute();
        }

        private void Ele_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ForceNodeControl control = sender as ForceNodeControl;
            //System.Diagnostics.Debug.WriteLine($"{control.ActualWidth}, {control.ActualHeight}");

            viewModel.FixedItem = control.Node;
            if (fixedButton.IsChecked == false)
                fixedButton.IsChecked = true;
            else
                viewModel.Excute();
        }

        Storyboard animation;
        private void copy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.Clear();
            animation.Begin();
            Clipboard.SetText(viewModel.FinalExcuteString);
        }
    }
}
