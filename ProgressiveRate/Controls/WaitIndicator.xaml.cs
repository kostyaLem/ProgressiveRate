using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ProgressiveRate.Controls
{
    public partial class WaitIndicator : UserControl
    {
        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public ICommand CloseCommand
        {
            get { return (ICommand)GetValue(CloseCommandProperty); }
            set { SetValue(CloseCommandProperty, value); }
        }

        public static DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(double), typeof(WaitIndicator));

        public static DependencyProperty CloseCommandProperty =
            DependencyProperty.Register(nameof(CloseCommand), typeof(ICommand), typeof(WaitIndicator));

        public WaitIndicator()
        {
            InitializeComponent();
        }
    }
}
