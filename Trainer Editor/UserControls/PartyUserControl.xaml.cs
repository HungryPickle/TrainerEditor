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
    public partial class PartyUserControl : UserControl {
        public ObservableCollection<Mon> SelectedParty { get { return Data.Instance.SelectedTrainer.Party.MonList; } }
        public static List<AutoCompleteTextBox> PartyBoxes { get { return Data.Instance.PartyBoxes; } }
        public PartyUserControl() {
            InitializeComponent();

            Data.Instance.PartyBoxes = new List<AutoCompleteTextBox> { mon0, mon1, mon2, mon3, mon4, mon5 };
        }

        private void AutoCompleteTextBox_GotFocus(object sender, RoutedEventArgs e) {
            AutoCompleteTextBox monBox = sender as AutoCompleteTextBox;
            Data.Instance.SelectedMon = (Mon)monBox.Tag;

        }

        private void Add_Click(object sender, RoutedEventArgs e) {
            if (SelectedParty.Count < 6) {
                SelectedParty.Add(new Mon("SPECIES_DITTO"));
                Data.Instance.SelectedMon = (Mon)PartyBoxes[SelectedParty.Count - 1].Tag;
                PartyBoxes[SelectedParty.Count - 1].textbox.Focus();
            }
        }
        private void Delete_Click(object sender, RoutedEventArgs e) {
            if (SelectedParty.Count > 1) {
                int deleted = SelectedParty.IndexOf(Data.Instance.SelectedMon);
                int last = SelectedParty.Count - 1;
                SelectedParty.Remove(Data.Instance.SelectedMon);

                int index = deleted == last ? deleted - 1 : deleted;
                Data.Instance.SelectedMon = (Mon)PartyBoxes[index].Tag;
                PartyBoxes[index].textbox.Focus();

            }
        }

        private void MoveUp_Click(object sender, RoutedEventArgs e) {
            int index = SelectedParty.IndexOf(Data.Instance.SelectedMon);
            if (index > 0) {
                Mon tmp = SelectedParty[index - 1];
                SelectedParty[index - 1] = Data.Instance.SelectedMon;
                SelectedParty[index] = tmp;
                PartyBoxes[index - 1].textbox.Focus();
            }
        }

        private void MoveDown_Click(object sender, RoutedEventArgs e) {
            
            int index = SelectedParty.IndexOf(Data.Instance.SelectedMon);
            if (index + 1 < SelectedParty.Count) {
                Mon tmp = SelectedParty[index + 1];
                SelectedParty[index + 1] = Data.Instance.SelectedMon;
                SelectedParty[index] = tmp;
                PartyBoxes[index + 1].textbox.Focus();
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

        private void TextBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e) {
            Data.Instance.PartyBoxes.ForEach(b =>
                BindingOperations.GetMultiBindingExpression(b, AutoCompleteTextBox.TextBoxBackgroundProperty)
                .UpdateTarget()
            );
        }
    }



}
