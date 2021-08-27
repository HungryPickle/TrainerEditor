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
    /// Interaction logic for IVsUserControl.xaml
    /// </summary>
    public partial class EVsUserControl : UserControl {
        public EVsUserControl() {
            InitializeComponent();
            combobox.DataContext = this;
        }

        private void combobox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            ComboBox cb = sender as ComboBox;
            for(int i = 0; i < Data.Instance.SelectedMon.EVs.Count; i++) {
                Data.Instance.SelectedMon.EVs[i].Text = ((EVSpread)cb.SelectedItem).EVs[i];
            }

        }
    }
    public class EVSpread {
        public EVSpread(string type, string hp, string atk, string def, string spe, string spAtk, string spDef) {
            Type = type;
            EVs = new List<string> { hp, atk, def, spe, spAtk, spDef };
        }
        public string Type { get; set; }
        public List<string> EVs { get; set; }

        public static readonly List<EVSpread> Spreads = new List<EVSpread> {
            new EVSpread("Max","255","255","255","255","255","255"),
            new EVSpread("Zero","0","0","0","0","0","0"),
            new EVSpread("Atk & Spe","4","252","0","252","0","0"),
            new EVSpread("SpAtk & Spe","4","0","0","252","252","0"),
            new EVSpread("HP & Def","252","0","252","4","0","0"),
            new EVSpread("HP & SpDef","252","0","0","4","0","252"),
        };
    }
}
