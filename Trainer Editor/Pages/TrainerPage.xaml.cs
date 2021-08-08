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

namespace Trainer_Editor.Pages {
    /// <summary>
    /// Interaction logic for TrainerPage.xaml
    /// </summary>
    public partial class TrainerPage : Page {
        public TrainerPage() {
            InitializeComponent();

            DataContext = Data.Instance;
        }
    }
}
