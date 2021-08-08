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
    /// Interaction logic for TrainerSearchUserControl.xaml
    /// </summary>
    public partial class TrainerSearchUserControl : UserControl {
        public TrainerSearchUserControl() {
            InitializeComponent();
        }
        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                Trainer firstTrainer = Data.Instance.FilteredTrainers.FirstOrDefault();
                if (firstTrainer != null) {
                    listbox.SelectedItem = firstTrainer;
                }
            }
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e) {
            TextBox textbox = sender as TextBox;
            Data.Instance.FilteredTrainers = Data.Instance.Trainers.Where(t => t.IndexName.Contains(textbox.Text, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            ListBox lb = sender as ListBox;
            if (lb.SelectedItem != null) {
                Data.Instance.SelectedTrainer = (Trainer)lb.SelectedItem;
                Data.Instance.SelectedMon = Data.Instance.SelectedTrainer.Party[0];
            }
        }
    }
}
