﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Trainer_Editor {
    public enum PartyType {
        TrainerMonNoItemDefaultMoves,
        TrainerMonItemDefaultMoves,
        TrainerMonNoItemCustomMoves,
        TrainerMonItemCustomMoves,
        TrainerMonCustom
    };

    public class Party : ObservableObject {

        private static readonly Dictionary<string, PartyType> TypeStringToType = new Dictionary<string, PartyType>{
            { PartyType.TrainerMonNoItemDefaultMoves.ToString(), PartyType.TrainerMonNoItemDefaultMoves },
            { PartyType.TrainerMonItemDefaultMoves.ToString(), PartyType.TrainerMonItemDefaultMoves },
            { PartyType.TrainerMonNoItemCustomMoves.ToString(), PartyType.TrainerMonNoItemCustomMoves },
            { PartyType.TrainerMonItemCustomMoves.ToString(), PartyType.TrainerMonItemCustomMoves },
            { PartyType.TrainerMonCustom.ToString(), PartyType.TrainerMonCustom }
        };
        public static readonly Dictionary<PartyType, List<string>> TypeToPartyFlags = new Dictionary<PartyType, List<string>> {
            { PartyType.TrainerMonNoItemDefaultMoves, new List<string>{ "0" } },
            { PartyType.TrainerMonItemDefaultMoves, new List<string>{ "F_TRAINER_PARTY_HELD_ITEM" } },
            { PartyType.TrainerMonNoItemCustomMoves, new List<string>{ "F_TRAINER_PARTY_CUSTOM_MOVESET" } },
            { PartyType.TrainerMonItemCustomMoves, new List<string>{ "F_TRAINER_PARTY_HELD_ITEM", "F_TRAINER_PARTY_CUSTOM_MOVESET" } },
            { PartyType.TrainerMonCustom, new List<string>{"F_TRAINER_PARTY_CUSTOM"} }
        };

        private PartyType type;
        public PartyType Type {
            get => type;
            set {
                type = value;
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
            Party party = new Party(PartyType.TrainerMonNoItemDefaultMoves, "DUMMY_PARTY");
            party.MonList.Add(new Mon("SPECIES_NONE"));
            return party;
        }
        public Party(PartyType type, string name) {
            Type = type;
            Name = name;
            MonList = new ObservableCollection<Mon>();
            MonList.CollectionChanged += new NotifyCollectionChangedEventHandler(PartyChanged);
        }

        public void AddMonFromStruct(string monStruct) {

            Mon mon = new Mon();
            mon.Iv = RegexMon.IV.Match(monStruct).Value;
            mon.Lvl = RegexMon.Lvl.Match(monStruct).Value;
            mon.Species = RegexMon.Species.Match(monStruct).Value;

            switch (Type) {
                case PartyType.TrainerMonNoItemDefaultMoves:
                    break;
                case PartyType.TrainerMonItemDefaultMoves:
                    mon.HeldItem = RegexMon.HeldItem.Match(monStruct).Value;
                    break;
                case PartyType.TrainerMonNoItemCustomMoves:
                    mon.Moves = Mon.MatchMoves(monStruct);
                    break;
                case PartyType.TrainerMonItemCustomMoves:
                    mon.HeldItem = RegexMon.HeldItem.Match(monStruct).Value;
                    mon.Moves = Mon.MatchMoves(monStruct);
                    break;
                default:
                    break;
            }

            MonList.Add(mon);
        }

        private string CreateMonStruct(Mon mon) {
            string monStruct = $"\n\t{{" +
            mon.IvMember +
            mon.LvlMember +
            mon.SpeciesMember;
            switch (Type) {
                case PartyType.TrainerMonNoItemDefaultMoves:
                    break;
                case PartyType.TrainerMonItemDefaultMoves:
                    monStruct += mon.HeldItemMember;
                    break;
                case PartyType.TrainerMonNoItemCustomMoves:
                    monStruct += mon.MovesMember;
                    break;
                case PartyType.TrainerMonItemCustomMoves:
                    monStruct += mon.HeldItemMember + mon.MovesMember;
                    break;
                default:
                    break;
            }
            monStruct += $"\n\t}}";

            return monStruct;
        }

        public string CreatePartyStruct() {
            string partyStruct = $"static const struct {Type} {Name}[] = {{";
            for (int i = 0; i < MonList.Count; i++) {
                partyStruct += CreateMonStruct(MonList[i]);
                if (i < MonList.Count - 1)
                    partyStruct += ",";
            }
            partyStruct += $"\n}};";
            return partyStruct;
        }

        public static bool IfContainsPartyAddParty(string line, Dictionary<string, Party> parties) {

            string name = RegexTrainer.PartyName.Match(line).Value;
            PartyType type;

            if (TypeStringToType.TryGetValue(RegexTrainer.PartyType.Match(line).Value, out type)) {
                parties.Add(name, new Party(type, name));
                return true;
            }
            else
                return false;
        }
    }
}
