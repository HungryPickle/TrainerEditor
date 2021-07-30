using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Ookii.Dialogs.Wpf;


namespace Trainer_Editor.UserControls {
    /// <summary>
    /// Interaction logic for PartyUserControl.xaml
    /// </summary>
    public partial class PartyUserControl : UserControl {
        public PartyUserControl() {
            InitializeComponent();
        }

        private void AutoCompleteTextBox_GotFocus(object sender, RoutedEventArgs e) {
            Data.Instance.SelectedMon = (Mon)(sender as AutoCompleteTextBox).Tag;
        }



        private void Add_Click(object sender, RoutedEventArgs e) {

            if (Data.Instance.SelectedTrainer.Party.MonList.Count < 6) {
                Data.Instance.SelectedTrainer.Party.MonList.Add(new Mon("SPECIES_DITTO"));
            }

            foreach (Mon mon in Data.Instance.SelectedTrainer.Party.MonList) {
                Debug.WriteLine(mon.Species);
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e) { 
            bool prompt = false;
            using (TaskDialog dialog = new TaskDialog()) { 
                dialog.WindowTitle = "Test";
                TaskDialogButton yesButton = new TaskDialogButton(ButtonType.Yes);
                TaskDialogButton noButton = new TaskDialogButton(ButtonType.No);
                dialog.Buttons.Add(yesButton);
                dialog.Buttons.Add(noButton);
                dialog.CenterParent = true;
                TaskDialogButton button = dialog.ShowDialog();
                if (button == yesButton)
                    prompt = true;
                else if (button == noButton)
                    prompt = false;
            };
            
            //MessageBoxResult prompt = MessageBox.Show("Delete Pokemon", "Are you sure?", MessageBoxButton.YesNo, MessageBoxImage.Question);
            
            if(Data.Instance.SelectedTrainer.Party.MonList.Count > 1 && prompt == true) {
                Data.Instance.SelectedTrainer.Party.MonList.Remove(Data.Instance.SelectedMon);
            }
        }

        private void MoveUp_Click(object sender, RoutedEventArgs e) {
            int index = Data.Instance.SelectedTrainer.Party.MonList.IndexOf(Data.Instance.SelectedMon);
            if (index > 0) {
                Mon tmp = Data.Instance.SelectedTrainer.Party.MonList[index - 1];
                Data.Instance.SelectedTrainer.Party.MonList[index - 1] = Data.Instance.SelectedMon;
                Data.Instance.SelectedTrainer.Party.MonList[index] = tmp;
            }
        }

        private void MoveDown_Click(object sender, RoutedEventArgs e) {
            
            int index = Data.Instance.SelectedTrainer.Party.MonList.IndexOf(Data.Instance.SelectedMon);
            if (index + 1 < Data.Instance.SelectedTrainer.Party.MonList.Count) {
                Mon tmp = Data.Instance.SelectedTrainer.Party.MonList[index + 1];
                Data.Instance.SelectedTrainer.Party.MonList[index + 1] = Data.Instance.SelectedMon;
                Data.Instance.SelectedTrainer.Party.MonList[index] = tmp;
            }
        }
    }

    public class HighlightSelectedMonConverter : IMultiValueConverter {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
            bool isntNull = values[0] != null || values[1] != null;
            return values[0] == values[1] && isntNull ? Brushes.LightGreen : Brushes.White;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
            throw new NotSupportedException();
        }
    }

    public class DoesSlotHaveMonConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return value == null ? false : true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotSupportedException();
        }
    }

}
