using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace D3Sharp.Demo.Controls
{
    //https://github.com/xamarin/Xamarin.Forms/blob/4048fab83b78c83786cb8dbbbd1380fa49b1a73f/Xamarin.Forms.Platform.WPF/Controls/FormsProgressRing.xaml.cs
    /// <summary>
    /// MiniProgressRing.xaml 的交互逻辑
    /// </summary>
    public partial class MiniProgressRing : UserControl
    {
        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.Register("IsActive", typeof(bool), typeof(MiniProgressRing),
                new PropertyMetadata(false, new PropertyChangedCallback(IsActiveChanged)));

        Storyboard animation;

        public MiniProgressRing()
        {
            InitializeComponent();

            animation = (Storyboard)Resources["ProgressRingStoryboard"];
        }

        public bool IsActive
        {
            get
            {
                return (bool)GetValue(IsActiveProperty);
            }

            set
            {
                SetValue(IsActiveProperty, value);
            }
        }

        static void IsActiveChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((MiniProgressRing)sender).OnIsActiveChanged(Convert.ToBoolean(e.NewValue));
        }

        void OnIsActiveChanged(bool newValue)
        {
            if (newValue)
            {
                animation.Begin();
            }
            else
            {
                animation.Stop();
            }
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            // force the ring to the largest square which is fully visible in the control
            Ring.Width = Math.Min(ActualWidth, ActualHeight);
            Ring.Height = Math.Min(ActualWidth, ActualHeight);
            base.OnRenderSizeChanged(sizeInfo);
        }
    }
}
