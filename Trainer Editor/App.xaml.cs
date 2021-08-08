using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Trainer_Editor.UserControls;

namespace Trainer_Editor {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        private void TextBoxCaret_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e) {
            TextBox tb = sender as TextBox;
            tb.CaretIndex = tb.Text.Length;
        }
        private void TextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e) {
            (sender as TextBox).Background = Brushes.LightGreen;
        }

        private void TextBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e) {
            (sender as TextBox).Background = Brushes.White;
        }

    }
}
