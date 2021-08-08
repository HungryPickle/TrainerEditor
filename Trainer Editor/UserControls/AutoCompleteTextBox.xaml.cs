using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
    /// Interaction logic for AutoCompleteTextBox.xaml
    /// </summary>
    public partial class AutoCompleteTextBox : UserControl, INotifyPropertyChanged{

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public IEnumerable<string> ListSource {
            get { return (IEnumerable<string>)GetValue(ListSourceProperty); }
            set { SetValue(ListSourceProperty, value); }
        }
        public static readonly DependencyProperty ListSourceProperty =
             DependencyProperty.Register("ListSource", typeof(IEnumerable<string>), typeof(AutoCompleteTextBox), new PropertyMetadata(null, ListSourceChanged));
        private static void ListSourceChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e) {
            obj.SetValue(FilteredSourceProperty, e.NewValue);
        }

        public IEnumerable<string> FilteredSource {
            get { return (IEnumerable<string>)GetValue(FilteredSourceProperty); }
            set { SetValue(FilteredSourceProperty, value); }
        }
        public static readonly DependencyProperty FilteredSourceProperty =
            DependencyProperty.Register("FilteredSource", typeof(IEnumerable<string>), typeof(AutoCompleteTextBox), new PropertyMetadata(null));

        public string ListItem {
            get { return (string)GetValue(ListItemProperty); }
            set { SetValue(ListItemProperty, value); }
        }
        public static readonly DependencyProperty ListItemProperty =
            DependencyProperty.Register("ListItem", typeof(string), typeof(AutoCompleteTextBox), new PropertyMetadata(string.Empty));

        public double TextBoxFontSize {
            get { return (double)GetValue(TextBoxFontSizeProperty); }
            set { SetValue(TextBoxFontSizeProperty, value); }
        }
        public static readonly DependencyProperty TextBoxFontSizeProperty =
            DependencyProperty.Register("TextBoxFontSize", typeof(double), typeof(AutoCompleteTextBox), new PropertyMetadata(12.0));
        
        public Brush TextBoxBackground {
            get { return (Brush)GetValue(TextBoxBackgroundProperty); }
            set { SetValue(TextBoxBackgroundProperty, value); }
        }
        public static readonly DependencyProperty TextBoxBackgroundProperty =
            DependencyProperty.Register("TextBoxBackground", typeof(Brush), typeof(AutoCompleteTextBox), new PropertyMetadata(Brushes.White));
        
        public string LabelContent {
            get { return (string)GetValue(LabelContentProperty); }
            set { SetValue(LabelContentProperty, value); }
        }
        public static readonly DependencyProperty LabelContentProperty =
            DependencyProperty.Register("LabelContent", typeof(string), typeof(AutoCompleteTextBox), new PropertyMetadata(string.Empty));

        public double LabelFontSize {
            get { return (double)GetValue(LabelFontSizeProperty); }
            set { SetValue(LabelFontSizeProperty, value); }
        }
        public static readonly DependencyProperty LabelFontSizeProperty =
            DependencyProperty.Register("LabelFontSize", typeof(double), typeof(AutoCompleteTextBox), new PropertyMetadata(12.0));


        public Visibility LabelVisibility {
            get { return (Visibility)GetValue(LabelVisibilityProperty); }
            set { SetValue(LabelVisibilityProperty, value); }
        }
        public static readonly DependencyProperty LabelVisibilityProperty =
            DependencyProperty.Register("LabelVisibility", typeof(Visibility), typeof(AutoCompleteTextBox), new PropertyMetadata(Visibility.Visible));


        private bool isFirstKey = true;
        public AutoCompleteTextBox() {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e) {

            if (FilteredSource == null || ListSource == null)
                return;

            if (FilteredSource.Contains(textbox.Text) || ListSource.Contains(textbox.Text)) {
                textbox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                isFirstKey = true;
            }
            else {
                FilteredSource = new ObservableCollection<string>(ListSource
                    .Where(t => t.Contains(textbox.Text, StringComparison.OrdinalIgnoreCase))
                    .OrderBy(t => t));
            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (((string)listbox1.SelectedItem) != null) {
                listbox1.GetBindingExpression(ListBox.SelectedItemProperty).UpdateSource();
            }
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e) {
            switch (e.Key) {
                case Key.Delete:
                    if (ListSource.Contains(string.Empty)) {
                        textbox.Clear();
                    }
                    e.Handled = true;
                    break;
                case Key.Escape:
                    popup.IsOpen = false;
                    e.Handled = true;
                    break;
                case Key.Enter:
                    if (FilteredSource?.FirstOrDefault() != null) {
                        textbox.Text = FilteredSource.FirstOrDefault();
                    }
                    popup.IsOpen = false;
                    e.Handled = true;
                    break;
                case Key.Down:
                    if (popup.IsOpen) {
                        listbox1.Focus();
                    }
                    break;
                case Key.Up:
                case Key.Left:
                case Key.Right:
                case Key.LeftShift:
                case Key.RightShift:
                case Key.LeftCtrl:
                case Key.RightCtrl:
                    break;
                default:
                    if (isFirstKey) {
                        textbox.Clear();
                        isFirstKey = false;
                    }
                    popup.IsOpen = true;
                    break;
            }
        }

        private void PopUp_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            popup.IsOpen = false;
            textbox.Focus();
        }

        private void PopUp_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter || e.Key == Key.Escape) {
                popup.IsOpen = false;
                textbox.Focus();
            }
        }

        private void PopUp_Closed(object sender, EventArgs e) {
            textbox.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
        }
    }

}
