using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Ookii.Dialogs.Wpf;


namespace Trainer_Editor.UserControls {
    /// <summary>
    /// Interaction logic for PartyUserControl.xaml
    /// </summary>
    public partial class MonSwapUserControl : UserControl {

        //public static List<AutoCompleteTextBox> PartyBoxes { get { return Data.Instance.PartyBoxes; } }
        private static List<AutoCompleteTextBox> MonSwapBoxes { get; set; }
        private static ObservableCollection<MonSwap> SelectedMonSwaps { get => Data.Instance.SelectedMon.MonSwaps.List; }
        public MonSwapUserControl() {
            InitializeComponent();
            MonSwapBoxes = new List<AutoCompleteTextBox> { swap0, swap1, swap2 };
            //Data.Instance.PartyBoxes = new List<AutoCompleteTextBox> { mon0, mon1, mon2, mon3, mon4, mon5 };
        }

        private void AutoCompleteTextBox_GotFocus(object sender, RoutedEventArgs e) {
            AutoCompleteTextBox swapBox = sender as AutoCompleteTextBox;
            Data.Instance.SelectedMonSwap = (MonSwap)swapBox.Tag;

        }

        private void Add_Click(object sender, RoutedEventArgs e) {
            if (SelectedMonSwaps.Count < 3) {
                SelectedMonSwaps.Add(new MonSwap() { Species = "SPECIES_DITTO"} );
                Data.Instance.SelectedMonSwap = (MonSwap)MonSwapBoxes[SelectedMonSwaps.Count - 1].Tag;
                MonSwapBoxes[SelectedMonSwaps.Count - 1].textbox.Focus();
            }
        }
        private void Delete_Click(object sender, RoutedEventArgs e) {
            if (SelectedMonSwaps.Count > 1) {
                int deleted = SelectedMonSwaps.IndexOf(Data.Instance.SelectedMonSwap);
                int last = SelectedMonSwaps.Count - 1;
                SelectedMonSwaps.Remove(Data.Instance.SelectedMonSwap);

                int index = deleted == last ? deleted - 1 : deleted;
                Data.Instance.SelectedMonSwap = (MonSwap)MonSwapBoxes[index].Tag;
                MonSwapBoxes[index].textbox.Focus();

            }
        }

        private void MoveUp_Click(object sender, RoutedEventArgs e) {
            int index = SelectedMonSwaps.IndexOf(Data.Instance.SelectedMonSwap);
            if (index > 0) {
                MonSwap tmp = SelectedMonSwaps[index - 1];
                SelectedMonSwaps[index - 1] = Data.Instance.SelectedMonSwap;
                SelectedMonSwaps[index] = tmp;
                MonSwapBoxes[index - 1].textbox.Focus();
            }
        }

        private void MoveDown_Click(object sender, RoutedEventArgs e) {
            
            int index = SelectedMonSwaps.IndexOf(Data.Instance.SelectedMonSwap);
            if (index + 1 < SelectedMonSwaps.Count) {
                MonSwap tmp = SelectedMonSwaps[index + 1];
                SelectedMonSwaps[index + 1] = Data.Instance.SelectedMonSwap;
                SelectedMonSwaps[index] = tmp;
                MonSwapBoxes[index + 1].textbox.Focus();
            }
        }

        private void PartyUserControl_PreviewKeyDown(object sender, KeyEventArgs e) {
            bool shiftDown = Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);

            switch (e.Key) {
                case Key.Insert:
                    Add_Click(sender, e);
                    e.Handled = true;
                    break;
                case Key.Delete:
                    Delete_Click(sender, e);
                    e.Handled = true;
                    break;
                case Key.Up:
                    if (shiftDown) {
                        MoveUp_Click(sender, e);
                        e.Handled = true;
                    }
                    break;
                case Key.Down:
                    if (shiftDown) {
                        MoveDown_Click(sender, e);
                        e.Handled = true;
                    }
                    break;
                default:
                    break;
            }
        }

    }



}
