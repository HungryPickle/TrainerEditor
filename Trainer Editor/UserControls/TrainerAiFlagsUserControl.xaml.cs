using System;
using System.Collections.Generic;
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
    /// Interaction logic for AiFlagsUserControl.xaml
    /// </summary>
    public partial class TrainerAiFlagsUserControl : UserControl {
        public TrainerAiFlagsUserControl() {
            InitializeComponent();
            DataContext = Data.Instance;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e) {
            CheckBox cb = sender as CheckBox;
            string aiFlag = (string)cb.Tag;
            if (!Data.Instance.SelectedTrainer.AiFlags.Contains(aiFlag))
                Data.Instance.SelectedTrainer.AiFlags.Add(aiFlag);
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e) {
            CheckBox cb = sender as CheckBox;
            string aiFlag = (string)cb.Tag;
            if (Data.Instance.SelectedTrainer.AiFlags.Contains(aiFlag))
                Data.Instance.SelectedTrainer.AiFlags.Remove(aiFlag);
        }
    }
}
