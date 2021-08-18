using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Trainer_Editor.UserControls;

namespace Trainer_Editor {

    public class Data : ObservableObject {

        private Data() { }

        private static Data instance = new Data();
        public static Data Instance {
            get { return instance; }
        }

        private List<Trainer> trainers = new List<Trainer>();
        public List<Trainer> Trainers {
            get { return trainers; }
            set { trainers = value; OnPropertyChanged("Trainers"); FilteredTrainers = value; }
        }

        private List<Trainer> filteredTrainers = new List<Trainer>();
        public List<Trainer> FilteredTrainers {
            get { return filteredTrainers; }
            set { filteredTrainers = value; OnPropertyChanged("FilteredTrainers"); }
        }

        private Trainer selectedTrainer;
        public Trainer SelectedTrainer {
            get { return selectedTrainer; }
            set { selectedTrainer = value; OnPropertyChanged("SelectedTrainer"); }
        }

        public ObservableCollection<Mon> SelectedParty {
            get { return SelectedTrainer.Party.MonList; }
            set {
                SelectedTrainer.Party.MonList = value;
            }
        }

        private Mon selectedMon;
        public Mon SelectedMon {
            get { return selectedMon; }
            set {
                selectedMon = value; OnPropertyChanged("SelectedMon");
                OnPropertyChanged("SelectedMonBox");
            }
        }

        public AutoCompleteTextBox SelectedMonBox {
            get { return PartyBoxes.FirstOrDefault(b => (Mon)b.Tag == SelectedMon); }
        }

        private List<AutoCompleteTextBox> partyBoxes;

        public List<AutoCompleteTextBox> PartyBoxes {
            get { return partyBoxes; }
            set { partyBoxes = value; }
        }
        public TextBlock StatusBar { get; set; }

        public Constant Species { get; set; } = new Constant(Constants.Species);
        public Constant Moves { get; set; } = new Constant(Constants.Moves);
        public Constant Items { get; set; } = new Constant(Constants.Items);
        public Constant TrainerClass { get; set; } = new Constant(Constants.TrainerClass);
        public Constant GetConstant(Constants type) {
            switch (type) {
                case Constants.Species:
                    return Species;
                case Constants.Moves:
                    return Moves;
                case Constants.Items:
                    return Items;
                case Constants.TrainerClass:
                    return TrainerClass;
                default:
                    MessageBox.Show("Constant not implemented in Data.GetConstant.");
                    return null;
            }
        }
        public void SetConstant(Constant constant) {
            switch (constant.Type) {
                case Constants.Species:
                    Species = constant;
                    break;
                case Constants.Moves:
                    Moves = constant;
                    break;
                case Constants.Items:
                    Items = constant;
                    break;
                case Constants.TrainerClass:
                    TrainerClass = constant;
                    break;
                default:
                    MessageBox.Show("Constant not implemented in Data.SetConstant.");
                    break;
            }
        }
    }
}
