﻿using System;
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

            FileManager.Instance.ParseAllConstants();
            FileManager.Instance.SerializeAllConstants();

        }

        private void defaultRegex_Click(object sender, RoutedEventArgs e) {
            foreach (Constants type in Enum.GetValues(typeof(Constants))) {
                Data.Instance.GetConstant(type).SetRegexToDefault();
            }
        }
    }
}
