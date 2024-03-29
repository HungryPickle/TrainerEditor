﻿using System.Collections.Generic;
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

        private Mon selectedMon;
        public Mon SelectedMon {
            get { return selectedMon; }
            set {
                selectedMon = value; OnPropertyChanged("SelectedMon");
                OnPropertyChanged("SelectedMonBox");
            }
        }

        public AutoCompleteTextBox SelectedMonBox {
            get { return PartyBoxes?.FirstOrDefault(b => (Mon)b.Tag == SelectedMon); }
        }

        private List<AutoCompleteTextBox> partyBoxes;

        public List<AutoCompleteTextBox> PartyBoxes {
            get { return partyBoxes; }
            set { partyBoxes = value; }
        }
        private MonSwap selectedMonSwap;
        public MonSwap SelectedMonSwap { get { return selectedMonSwap; } set { selectedMonSwap = value; OnPropertyChanged("SelectedMonSwap"); } }
        public TextBlock StatusBar { get; set; }

        private Constant species = new Constant(Constants.Species);
        private Constant moves = new Constant(Constants.Moves);
        private Constant items = new Constant(Constants.Items);
        private Constant trainerClass = new Constant(Constants.TrainerClass);
        private Constant trainerPic = new Constant(Constants.TrainerPic);
        private Constant trainerEncounterMusic = new Constant(Constants.TrainerEncounterMusic);
        private Constant trainerAiFlag = new Constant(Constants.TrainerAiFlag);
        private Constant balls = new Constant(Constants.Balls);
        private Constant natures = new Constant(Constants.Natures);
        public Constant Species { get => species; set { species = value; OnPropertyChanged("Species"); } }
        public Constant Moves { get => moves; set { moves = value; OnPropertyChanged("Moves"); } }
        public Constant Items { get => items; set { items = value; OnPropertyChanged("Items"); } }
        public Constant TrainerClass { get => trainerClass; set { trainerClass = value; OnPropertyChanged("TrainerClass"); } }
        public Constant TrainerPic { get => trainerPic; set { trainerPic = value; OnPropertyChanged("TrainerPic"); } }
        public Constant TrainerEncounterMusic { get => trainerEncounterMusic; set { trainerEncounterMusic = value; OnPropertyChanged("TrainerEncounterMusic"); } }
        public Constant TrainerAiFlag { get => trainerAiFlag; set { trainerAiFlag = value; OnPropertyChanged("TrainerAiFlag"); } }
        public Constant Balls { get => balls; set { balls = value; OnPropertyChanged("Balls"); } }
        public Constant Natures { get => natures; set { natures = value; OnPropertyChanged("Natures"); } }
        
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
                case Constants.TrainerPic:
                    return TrainerPic;
                case Constants.TrainerEncounterMusic:
                    return TrainerEncounterMusic;
                case Constants.TrainerAiFlag:
                    return TrainerAiFlag;
                case Constants.Balls:
                    return Balls;
                case Constants.Natures:
                    return Natures;
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
                case Constants.TrainerPic:
                    TrainerPic = constant; 
                    break;
                case Constants.TrainerEncounterMusic:
                    TrainerEncounterMusic = constant; 
                    break;
                case Constants.TrainerAiFlag:
                    TrainerAiFlag = constant;
                    break;
                case Constants.Balls:
                    Balls = constant;
                    break;
                case Constants.Natures:
                    Natures = constant;
                    break;
                default:
                    MessageBox.Show("Constant not implemented in Data.SetConstant.");
                    break;
            }
        }
    }
}
