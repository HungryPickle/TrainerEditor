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
            set {
                SetValue(ListSourceProperty, value);
                FilteredSource = value;
            }
        }
        
        private IEnumerable<string> filteredSource;
        public IEnumerable<string> FilteredSource {
            get { return filteredSource; }
            set {
                filteredSource = value;
                OnPropertyChanged("FilteredSource");
            }
        }
        public string SelectedItem {
            get { return (string)GetValue(SelectedItemProperty); }
            set {
                if (string.IsNullOrEmpty((string)GetValue(SelectedItemProperty)))
                    textbox1.Clear();
                else
                    SetValue(SelectedItemProperty, value); 
            }
        }

        private bool firstKey = true;

         // Using a DependencyProperty as the backing store for ListBoxSource.  This enables animation, styling, binding, etc...
         public static readonly DependencyProperty ListSourceProperty =
             DependencyProperty.Register("ListSource", typeof(IEnumerable<string>), typeof(AutoCompleteTextBox), new PropertyMetadata(null));

        // Using a DependencyProperty as the backing store for SelectedItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(string), typeof(AutoCompleteTextBox), new PropertyMetadata(string.Empty));

        public AutoCompleteTextBox() {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e) {
            TextBox tb = sender as TextBox;
            if (tb.Text.Length > 1) {

            }
            if (FilteredSource?.Contains(tb.Text) == true) {
                return;
            }
            //if(ListSource?.Contains(tb.Text) == true) {
            //    FilteredSource = ListSource;
            //    SelectedItem = tb.Text;
            //    return;
            //}

            FilteredSource = new ObservableCollection<string>(ListSource.Where(t => t.Contains(tb.Text, StringComparison.OrdinalIgnoreCase)).OrderBy(t => t));
            
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            ListBox lb = sender as ListBox;

            if (lb.SelectedItem != null){//!string.IsNullOrEmpty((string)lb.SelectedItem)) {
                //Debug.WriteLine(lb.SelectedItem);
                lb.GetBindingExpression(ListBox.SelectedItemProperty).UpdateSource();
            }
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e) {
            TextBox tb = sender as TextBox;
            popup.IsOpen = true;

            if (e.Key == Key.Enter) {
                if(FilteredSource.FirstOrDefault() == null)
                    textbox1.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
                else
                    SelectedItem = FilteredSource.FirstOrDefault();
                popup.IsOpen = false;
                firstKey = true;
            }
            else if (e.Key == Key.Down) {
                if (!listbox1.IsFocused) {
                    listbox1.Focus();
                    listbox1.SelectedIndex = 0;
                }
            }
            else if (firstKey == true) {
                tb.Clear();
                firstKey = false;
            }
        }

        private void PopUp_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            popup.IsOpen = false;
            firstKey = true;
        }

        private void PopUp_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                popup.IsOpen = false;
                firstKey = true;
            }
        }
    }
}
