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
    public partial class IVsUserControl : UserControl {

        public IVsUserControl() {
            InitializeComponent();
            combobox.DataContext = this;
        }

        private void combobox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            ComboBox cb = sender as ComboBox;
            for(int i = 0; i < Data.Instance.SelectedMon.IVs.Count; i++) {
                Data.Instance.SelectedMon.IVs[i].Text = ((HPSpread)cb.SelectedItem).IVs[i];
            }

        }
    }
    public class HPSpread {
        public HPSpread(string type, string hp, string atk, string def, string spe, string spAtk, string spDef) {
            Type = type;
            IVs = new List<string> { hp, atk, def, spe, spAtk, spDef };
        }
        public string Type { get; set; }
        public List<string> IVs { get; set; }
        public static List<HPSpread> Spreads { get; set; } = new List<HPSpread> {
            new HPSpread("Max","31","31","31","31","31","31"),
            new HPSpread("Zero","0","0","0","0","0","0"),
            new HPSpread("Bug","31","31","31","30","31","30"),
            new HPSpread("Dark","31","31","31","31","31","31"),
            new HPSpread("Dragon","30","31","31","31","31","31"),
            new HPSpread("Electric","31","31","31","31","30","31"),
            new HPSpread("Fighting","31","31","30","30","30","30"),
            new HPSpread("Fire","31","30","31","30","30","31"),
            new HPSpread("Flying","31","31","31","30","30","30"),
            new HPSpread("Ghost","31","30","31","31","31","30"),
            new HPSpread("Grass","30","31","31","31","30","31"),
            new HPSpread("Ground","31","31","31","31","30","30"),
            new HPSpread("Ice","31","31","31","30","31","31"),
            new HPSpread("Poison","31","31","30","31","30","30"),
            new HPSpread("Psychic","30","31","31","30","31","31"),
            new HPSpread("Rock","31","31","30","30","31","30"),
            new HPSpread("Steel","31","31","31","31","31","30"),
            new HPSpread("Water","31","31","31","30","30","31")
        };
    }
}
