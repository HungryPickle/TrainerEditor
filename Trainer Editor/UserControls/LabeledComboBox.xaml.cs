using System;
using System.Collections;
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
    /// Interaction logic for LabeledComboBox.xaml
    /// </summary>
    public partial class LabeledComboBox : UserControl {

        public IEnumerable ItemsSource {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
        // Using a DependencyProperty as the backing store for ItemsSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(LabeledComboBox), new PropertyMetadata(null));

        public object SelectedItem {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }
        // Using a DependencyProperty as the backing store for SelectedItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(object), typeof(LabeledComboBox), new PropertyMetadata(null));

        public object LabelContent {
            get { return (object)GetValue(LabelContentProperty); }
            set { SetValue(LabelContentProperty, value); }
        }
        // Using a DependencyProperty as the backing store for LabelContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelContentProperty =
            DependencyProperty.Register("LabelContent", typeof(object), typeof(LabeledComboBox), new PropertyMetadata(null));

        public LabeledComboBox() {
            InitializeComponent();
        }
    }
}
