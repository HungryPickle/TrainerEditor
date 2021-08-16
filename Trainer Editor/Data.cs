using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using Trainer_Editor.UserControls;

namespace Trainer_Editor {
    public enum Constant {
        Species, Moves, Items
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
        public List<AutoCompleteTextBox> PartyBoxes {
            get { return partyBoxes; }
            set { partyBoxes = value; }
        }
        public TextBlock StatusBar { get; set; }
        public Dictionary<Constant, List<string>> ConstantLists { get; set; } = new Dictionary<Constant, List<string>>{
            { Constant.Species, new List<string>() },
            { Constant.Items, new List<string>() },
            { Constant.Moves, new List<string>() }
        };
        public List<string> SpeciesList {
            get { return ConstantLists[Constant.Species]; }
            set { ConstantLists[Constant.Species] = value; OnPropertyChanged("SpeciesList"); }
        }
        public List<string> ItemsList {
            get { return ConstantLists[Constant.Items]; }
            set { ConstantLists[Constant.Items] = value; OnPropertyChanged("ItemsList"); }
        }
        public List<string> MovesList {
            get { return ConstantLists[Constant.Moves]; }
            set { ConstantLists[Constant.Moves] = value; OnPropertyChanged("MovesList"); }
        }

    }
}
