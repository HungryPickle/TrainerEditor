using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

        private RegexConfig regexConfig = new RegexConfig();
        public RegexConfig RegexConfig {
            get { return regexConfig; }
            set { regexConfig = value; OnPropertyChanged("RegexConfig"); }
        }

        private FilePaths filePaths = new FilePaths();
        public FilePaths FilePaths {
            get {
                if (string.IsNullOrEmpty(filePaths.PokeEmeraldDirectory))
                    return null;
                return filePaths;
            }
            set { filePaths = value; OnPropertyChanged("FilePaths"); }
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

        private List<string> speciesList;
        public List<string> SpeciesList {
            get { return speciesList; }
            set { speciesList = value; OnPropertyChanged("SpeciesList"); }
        }

        private List<string> itemsList;
        public List<string> ItemsList {
            get { return itemsList; }
            set { itemsList = value; OnPropertyChanged("ItemsList"); }
        }

        private List<string> movesList;

        public List<string> MovesList {
            get { return movesList; }
            set { movesList = value; OnPropertyChanged("MovesList"); }
        }

    }
}
