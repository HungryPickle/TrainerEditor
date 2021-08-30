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
    /// Interaction logic for MovesUserControl.xaml
    /// </summary>
    public partial class MovesUserControl : UserControl {

        public List<string> Moves {
            get { return (List<string>)GetValue(MovesProperty); }
            set { SetValue(MovesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Moves.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MovesProperty =
            DependencyProperty.Register("Moves", typeof(List<string>), typeof(MovesUserControl), new PropertyMetadata(null));



        public MovesUserControl() {
            InitializeComponent();
            
        }
    }
}
