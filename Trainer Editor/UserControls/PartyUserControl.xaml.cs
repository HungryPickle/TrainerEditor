using System;
using System.Collections.Generic;
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
            Trainer tr = Data.Instance.SelectedTrainer;

            if (int.Parse((string)cb.Tag) < tr.Party.MonList.Count) {
                tr.Party.MonList[int.Parse((string)cb.Tag)].Species = cb.Items.GetItemAt(0).ToString();
            }
            else if (int.Parse((string)cb.Tag) == tr.Party.MonList.Count) {
                tr.Party.MonList.Add(new Mon(cb.Items.GetItemAt(0).ToString()));
                Data.Instance.SelectedMon = tr.Party.MonList[int.Parse((string)cb.Tag)];
            }
            //else
            //    cb.Text = "";  
            cb.ItemsSource = Data.Instance.Species;
        }

        private void ComboBox_Species_TextChanged(object sender, TextChangedEventArgs e) {
            ComboBox cb = sender as ComboBox;
            if (cb.IsDropDownOpen && cb.Text.Length > 1) {
                cb.ItemsSource = Data.Instance.Species.Where(s => s.Contains(cb.Text, StringComparison.OrdinalIgnoreCase)).OrderBy(s => s);
            }

        }

        private void ComboBox_Species_GotFocus(object sender, RoutedEventArgs e) {
            ComboBox cb = sender as ComboBox;
            Trainer tr = Data.Instance.SelectedTrainer;
            if (int.Parse((string)cb.Tag) < tr.Party.MonList.Count)
                Data.Instance.SelectedMon = tr.Party.MonList[int.Parse((string)cb.Tag)];
        }
    }
}
