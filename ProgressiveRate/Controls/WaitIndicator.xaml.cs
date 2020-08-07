using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ProgressiveRate.Controls
{
    public partial class WaitIndicator : UserControl
    {
        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public ICommand ReloadCommand
        {
            get { return (ICommand)GetValue(ReloadCommandProperty); }
            set { SetValue(ReloadCommandProperty, value); }
        }

        public static DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(int), typeof(WaitIndicator), new PropertyMetadata(0, null, (d, e) => (int)e > 100 ? 100 : e));

        public static DependencyProperty ReloadCommandProperty =
            DependencyProperty.Register(nameof(ReloadCommand), typeof(ICommand), typeof(WaitIndicator), new PropertyMetadata(null));

        public WaitIndicator()
        {
            InitializeComponent();
        }
    }
}
