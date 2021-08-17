using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Trainer_Editor.UserControls;

namespace Trainer_Editor {
    public enum Constant {
        Species, Moves, Items, TrainerClass
    };
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
        private List<string> speciesList;
        private List<string> itemsList;
        private List<string> movesList;

        public List<AutoCompleteTextBox> PartyBoxes {
            get { return partyBoxes; }
            set { partyBoxes = value; }
        }
        public TextBlock StatusBar { get; set; }
        public List<string> GetConstantList(Constant constant) {
            switch (constant) {
                case Constant.Species:
                    return SpeciesList;
                case Constant.Moves:
                    return MovesList;
                case Constant.Items:
                    return ItemsList;
                case Constant.TrainerClass:
                    return TrainerClassList;
                default:
                    MessageBox.Show("Constant not implemented in Data.GetConstantList");
                    return null;
            }
        }
        public void SetConstantList(Constant constant, List<string> constantList) {
            switch (constant) {
                case Constant.Species:
                    SpeciesList = constantList;
                    break;
                case Constant.Moves:
                    MovesList = constantList;
                    break;
                case Constant.Items:
                    ItemsList = constantList;
                    break;
                case Constant.TrainerClass:
                    TrainerClassList = constantList;
                    break;
                default:
                    MessageBox.Show("Constant not implemented in Data.GetConstantList");
                    break;
            }
        }
        public List<string> SpeciesList {
            get => speciesList;
            set { speciesList = value; OnPropertyChanged("SpeciesList"); }
        }
        public List<string> ItemsList {
            get => itemsList;
            set { itemsList = value; OnPropertyChanged("ItemsList"); }
        }
        public List<string> MovesList {
            get => movesList;
            set { movesList = value; OnPropertyChanged("MovesList"); }
        }
        private List<string> trainerClassList;

        public List<string> TrainerClassList {
            get => trainerClassList;
            set { trainerClassList = value; OnPropertyChanged("TrainerClassList"); }
        }

    }
}
