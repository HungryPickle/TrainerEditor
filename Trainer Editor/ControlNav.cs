using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Trainer_Editor.UserControls;

namespace Trainer_Editor {
    public class ControlNav {
        private Control up;
        private Control down;
        private Control left;
        private Control right;
        private Control enter;
        private Control shiftEnter;
        public Control Up { get => GetTextBox(up); set => up = value; }
        public Control Down { get => GetTextBox(down); set => down = value; }
        public Control Left { get => GetTextBox(left); set => left = value; }
        public Control Right { get => GetTextBox(right); set => right = value; }
        public Control Enter { get => GetTextBox(enter); set => enter = value; }
        public Control ShiftEnter { get => GetTextBox(shiftEnter); set => shiftEnter = value; }
        public static Dictionary<Control, ControlNav> Navs = new Dictionary<Control, ControlNav>();

        public ControlNav(Control shiftEnter = null, Control enter = null, Control up = null, Control down = null, Control left = null, Control right = null) {
            ShiftEnter = shiftEnter;
            Enter = enter;
            Up = up;
            Down = down;
            Left = left;
            Right = right;
        }
        static T AddEvent<T>(T control, KeyEventHandler handler) {
            (control as Control).PreviewKeyDown += handler;
            return control;
        }
        public static void LoadControlNavs() {
            MainWindow m = ((MainWindow)Application.Current.MainWindow);
            KeyEventHandler labelPreview = new KeyEventHandler(LabeledTextBox_PreviewKeyDown);
            KeyEventHandler autoPreview = new KeyEventHandler(AutoCompleteTextBox_PreviewKeyDown);
            KeyEventHandler textPreview = new KeyEventHandler(TextBox_PreviewKeyDown);

            LabeledTextBox lvl = AddEvent(m.partyPage.lvlBox, labelPreview);
            LabeledTextBox iv = AddEvent(m.partyPage.ivBox, labelPreview);

            AutoCompleteTextBox move0 = AddEvent(m.partyPage.moves.move0, autoPreview);
            AutoCompleteTextBox move1 = AddEvent(m.partyPage.moves.move1, autoPreview);
            AutoCompleteTextBox move2 = AddEvent(m.partyPage.moves.move2, autoPreview);
            AutoCompleteTextBox move3 = AddEvent(m.partyPage.moves.move3, autoPreview);

            AutoCompleteTextBox mon0 = AddEvent(m.partyPage.party.mon0, autoPreview);
            AutoCompleteTextBox mon1 = AddEvent(m.partyPage.party.mon1, autoPreview);
            AutoCompleteTextBox mon2 = AddEvent(m.partyPage.party.mon2, autoPreview);
            AutoCompleteTextBox mon3 = AddEvent(m.partyPage.party.mon3, autoPreview);
            AutoCompleteTextBox mon4 = AddEvent(m.partyPage.party.mon4, autoPreview);
            AutoCompleteTextBox mon5 = AddEvent(m.partyPage.party.mon5, autoPreview);

            AutoCompleteTextBox heldItem = AddEvent(m.partyPage.heldItemBox, autoPreview);

            TextBox trainerSearch = AddEvent(m.trainerSearch.textbox, textPreview);


            Navs.Add(trainerSearch, new ControlNav(null, mon0));
            Navs.Add(mon0, new ControlNav(trainerSearch, lvl, null, mon1));
            Navs.Add(mon1, new ControlNav(trainerSearch, lvl, mon0, mon2));
            Navs.Add(mon2, new ControlNav(trainerSearch, lvl, mon1, mon3));
            Navs.Add(mon3, new ControlNav(trainerSearch, lvl, mon2, mon4));
            Navs.Add(mon4, new ControlNav(trainerSearch, lvl, mon3, mon5));
            Navs.Add(mon5, new ControlNav(trainerSearch, lvl, mon4, null));
            Navs.Add(lvl, new ControlNav(mon0, iv));
            Navs.Add(iv, new ControlNav(lvl, heldItem));
            Navs.Add(heldItem, new ControlNav(iv, move0));
            Navs.Add(move0, new ControlNav(heldItem, move1));
            Navs.Add(move1, new ControlNav(move0, move2));
            Navs.Add(move2, new ControlNav(move1, move3));
            Navs.Add(move3, new ControlNav(move2, null));
        }
        void MoveFocus(Control sender, KeyEventArgs e) {
            switch (e.Key) {
                case Key.Up:
                    Up?.Focus();
                    e.Handled = true;
                    break;
                case Key.Down:
                    Down?.Focus();
                    e.Handled = true;
                    break;
                case Key.Right:
                    Right?.Focus();
                    e.Handled = true;
                    break;
                case Key.Left:
                    Left?.Focus();
                    e.Handled = true;
                    break;
                case Key.Enter:
                    if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)) {
                        if (shiftEnter?.Tag is Mon)
                            Data.Instance.SelectedMonBox.textbox.Focus();
                        else
                            ShiftEnter?.Focus();

                    }
                    else {
                        if (enter?.Tag is Mon)
                            Data.Instance.SelectedMonBox.textbox.Focus();
                        else
                            Enter?.Focus();

                    }
                    e.Handled = true;
                    break;
                case Key.LeftAlt:
                case Key.RightAlt:
                case Key.LeftCtrl:
                case Key.RightCtrl:
                    e.Handled = true;
                    break;
                default:
                    break;
            }

        }
        static Control GetTextBox(Control control) {
            if (control is AutoCompleteTextBox)
                return (control as AutoCompleteTextBox)?.textbox;
            else if (control is LabeledTextBox)
                return (control as LabeledTextBox)?.textbox;
            else
                return control;
        }
        static bool IsCaretAtTextBoxEdge(TextBox textbox, Key key) {
            switch (key) {
                case Key.Left:
                    if (textbox.CaretIndex == 0)
                        return true;
                    break;
                case Key.Right:
                    if (textbox.CaretIndex == textbox.Text.Length)
                        return true;
                    break;
                default:
                    return true;
            }
            return false;
        }
        static void AutoCompleteTextBox_PreviewKeyDown(object sender, KeyEventArgs e) {
            AutoCompleteTextBox control = sender as AutoCompleteTextBox;

            if (!control.popup.IsOpen && IsCaretAtTextBoxEdge(control.textbox, e.Key)) {

                Navs[sender as Control].MoveFocus(control, e);
            }
        }
        static void LabeledTextBox_PreviewKeyDown(object sender, KeyEventArgs e) {
            LabeledTextBox control = sender as LabeledTextBox;
            if (IsCaretAtTextBoxEdge(control.textbox, e.Key))
                Navs[sender as Control].MoveFocus(control, e);
        }
        static void ListBox_PreviewKeyDown(object sender, KeyEventArgs e) {
            ListBox control = sender as ListBox;
            if (e.Key != Key.Up && e.Key != Key.Down)
                Navs[sender as Control].MoveFocus(control, e);
        }
        static void TextBox_PreviewKeyDown(object sender, KeyEventArgs e) {
            TextBox tb = sender as TextBox;
                Navs[sender as Control].MoveFocus(tb, e);
        }
    }


}
