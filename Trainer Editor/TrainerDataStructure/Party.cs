using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Trainer_Editor {
    public enum PartyTypes {
        TrainerMonNoItemDefaultMoves,
        TrainerMonItemDefaultMoves,
        TrainerMonNoItemCustomMoves,
        TrainerMonItemCustomMoves,
        TrainerMonCustom
    };

    public class Party : ObservableObject {

        private static readonly Dictionary<string, PartyTypes> TypeStringToType = new Dictionary<string, PartyTypes>{
            { PartyTypes.TrainerMonNoItemDefaultMoves.ToString(), PartyTypes.TrainerMonNoItemDefaultMoves },
            { PartyTypes.TrainerMonItemDefaultMoves.ToString(), PartyTypes.TrainerMonItemDefaultMoves },
            { PartyTypes.TrainerMonNoItemCustomMoves.ToString(), PartyTypes.TrainerMonNoItemCustomMoves },
            { PartyTypes.TrainerMonItemCustomMoves.ToString(), PartyTypes.TrainerMonItemCustomMoves },
            { PartyTypes.TrainerMonCustom.ToString(), PartyTypes.TrainerMonCustom }
        };
        public static readonly Dictionary<PartyTypes, List<string>> TypeToPartyFlags = new Dictionary<PartyTypes, List<string>> {
            { PartyTypes.TrainerMonNoItemDefaultMoves, new List<string>{ "0" } },
            { PartyTypes.TrainerMonItemDefaultMoves, new List<string>{ "F_TRAINER_PARTY_HELD_ITEM" } },
            { PartyTypes.TrainerMonNoItemCustomMoves, new List<string>{ "F_TRAINER_PARTY_CUSTOM_MOVESET" } },
            { PartyTypes.TrainerMonItemCustomMoves, new List<string>{ "F_TRAINER_PARTY_HELD_ITEM", "F_TRAINER_PARTY_CUSTOM_MOVESET" } },
            { PartyTypes.TrainerMonCustom, new List<string>{"F_TRAINER_PARTY_CUSTOM"} }
        };

        private PartyTypes type;
        public PartyTypes Type {
            get => type;
            set {
                type = value;
                foreach(Mon mon in MonList) {
                    mon.PartyType = value;
                }
                OnPropertyChanged("Type");
            }
        }
        public string Name { get; set; }

        private ObservableCollection<Mon> monList;

        public ObservableCollection<Mon> MonList {
            get => monList;
            set { monList = value;
                OnPropertyChanged("MonList");
            }
        }

        public void PartyChanged(object sender, NotifyCollectionChangedEventArgs e) {
            OnPropertyChanged("Item[]");
        }

        public Mon this[int index] {
            get {
                if (index < monList.Count)
                    return monList[index];
                else
                    return null;
            }
            set {
                if (index < monList.Count)
                    monList[index] = value;
            }
        }

        public static Party CreateDummy() {
            Party party = new Party(PartyTypes.TrainerMonNoItemDefaultMoves, "DUMMY_PARTY");
            party.MonList.Add(new Mon("SPECIES_NONE"));
            return party;
        }
        public Party(PartyTypes type, string name) {
            MonList = new ObservableCollection<Mon>();
            Type = type;
            Name = name;
            MonList.CollectionChanged += new NotifyCollectionChangedEventHandler(PartyChanged);
        }

        public void AddMonFromStruct(string monStruct) {
            MonList.Add(new Mon(Type, monStruct));
        }

        public string CreatePartyStruct() {
            string partyStruct = $"static const struct {Type} {Name}[] = {{";
            for (int i = 0; i < MonList.Count; i++) {
                partyStruct += MonList[i].CreateStruct(Type);
                if (i < MonList.Count - 1)
                    partyStruct += ",";
            }
            partyStruct += $"\n}};";
            return partyStruct;
        }

        public static bool IfContainsPartyAddParty(string line, Dictionary<string, Party> parties) {

            string name = RegexTrainer.PartyName.Match(line).Value;
            PartyTypes type;

            if (TypeStringToType.TryGetValue(RegexTrainer.PartyType.Match(line).Value, out type)) {
                parties.Add(name, new Party(type, name));
                return true;
            }
            else
                return false;
        }
    }
}
