using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
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
    /// Interaction logic for PartyUserControl.xaml
    /// </summary>
    public partial class PartyUserControl : UserControl {
        public PartyUserControl() {
            InitializeComponent();
        }

        private void ComboBox_Species_PreviewTextInput(object sender, TextCompositionEventArgs e) {
            ComboBox cb = sender as ComboBox;
            cb.IsDropDownOpen = true;
        }

        private void ComboBox_Species_DropDownClosed(object sender, EventArgs e) {
            ComboBox cb = sender as ComboBox;
            Mon mon = cb.Tag as Mon;
            
            if (mon != null) {
                mon.Species = Data.Instance.CulledSpeciesList.FirstOrDefault() ?? "SPECIES_NONE";
            }

        }

        private void ComboBox_Species_GotFocus(object sender, RoutedEventArgs e) {
            ComboBox cb = sender as ComboBox;
            Data.Instance.SelectedMon = (Mon)cb.Tag;

            TextBox TxtBox = (TextBox)cb.Template.FindName("PART_EditableTextBox", cb);
            TxtBox.SelectionLength = 0;
            
        }



    }
    public class SelectedBoxColorConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {

            if (Data.Instance.SelectedTrainer?.Party.MonList.IndexOf((Mon)value) == int.Parse((string)parameter)) {
                return 0.8;
            }
            else {
                return 1;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return Binding.DoNothing;
        }
    }
    public class PartyIndexConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return value == null ? false : true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return Binding.DoNothing;
        }
    }
}
