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

namespace Trainer_Editor.UserControls {
    /// <summary>
    /// Interaction logic for StatsUserControl.xaml
    /// </summary>
    public partial class StatsUserControl : UserControl {
        public List<Stat> Stats {
            get { return (List<Stat>)GetValue(StatsProperty); }
            set { SetValue(StatsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Stats.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StatsProperty =
            DependencyProperty.Register("Stats", typeof(List<Stat>), typeof(StatsUserControl), new PropertyMetadata(null));

        public StatsUserControl() {
            InitializeComponent();
        }
    }
}
