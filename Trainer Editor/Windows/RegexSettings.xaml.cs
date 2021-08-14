using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Trainer_Editor.UserControls;

namespace Trainer_Editor.Windows {
    /// <summary>
    /// Interaction logic for RegexSettings.xaml
    /// </summary>
    public partial class RegexSettings : Window {
        public RegexSettings() {
            InitializeComponent();
            DataContext = Data.Instance;
        }

        private void saveRegex_Click(object sender, RoutedEventArgs e) {

            FileManager.SerializeRegexConfig(Data.Instance.RegexConfig);
            Data.Instance.RegexConfig = FileManager.DeserializeRegexConfig();

        }

        private void defaultRegex_Click(object sender, RoutedEventArgs e) {
            Data.Instance.RegexConfig.Species = RegexConfig.SpeciesDefault;
            Data.Instance.RegexConfig.Moves = RegexConfig.MovesDefault;
            Data.Instance.RegexConfig.Items = RegexConfig.ItemsDefault;
        }
    }
}
