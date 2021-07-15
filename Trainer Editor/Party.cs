using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Trainer_Editor {
    public enum TYPE {
        TrainerMonNoItemDefaultMoves,
        TrainerMonItemDefaultMoves,
        TrainerMonNoItemCustomMoves,
        TrainerMonItemCustomMoves
    };

    public class Party {

        private static readonly Dictionary<string, TYPE> TypeDict = new Dictionary<string, TYPE>{
            { TYPE.TrainerMonNoItemDefaultMoves.ToString(), TYPE.TrainerMonNoItemDefaultMoves },
            { TYPE.TrainerMonItemDefaultMoves.ToString(), TYPE.TrainerMonItemDefaultMoves },
            { TYPE.TrainerMonNoItemCustomMoves.ToString(), TYPE.TrainerMonNoItemCustomMoves },
            { TYPE.TrainerMonItemCustomMoves.ToString(), TYPE.TrainerMonItemCustomMoves }
        };

        public TYPE Type { get; set; }
        public string Name { get; set; }

        private ObservableCollection<Mon> monList;
        public ObservableCollection<Mon> MonList {
            get => monList;
            set {
                monList = value;
            }
        }
        public Mon this[int index] {
            get {
                if (index < monList.Count) {
                    return monList[index]; 
                }
                else {
                    return null;
                }
            }
            set {
                if (index < monList.Count) {
                    monList[index] = value; 
                }
            }
        }
        

        public Party(TYPE type, string name) {
            Type = type;
            Name = name;
            MonList = new ObservableCollection<Mon>();
        }

        public void AddMonFromStruct(string monStruct) {

            Mon mon = new Mon();
            mon.Iv = RegexMon.IV.Match(monStruct).Value;
            mon.Lvl = RegexMon.Lvl.Match(monStruct).Value;
            mon.Species = RegexMon.Species.Match(monStruct).Value;

            switch (Type) {
                case TYPE.TrainerMonNoItemDefaultMoves:
                    break;
                case TYPE.TrainerMonItemDefaultMoves:
                    mon.HeldItem = RegexMon.HeldItem.Match(monStruct).Value;
                    break;
                case TYPE.TrainerMonNoItemCustomMoves:
                    mon.Moves = Mon.MatchMoves(monStruct);
                    break;
                case TYPE.TrainerMonItemCustomMoves:
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
                case TYPE.TrainerMonNoItemDefaultMoves:
                    break;
                case TYPE.TrainerMonItemDefaultMoves:
                    monStruct += mon.HeldItemMember;
                    break;
                case TYPE.TrainerMonNoItemCustomMoves:
                    monStruct += mon.MovesMember;
                    break;
                case TYPE.TrainerMonItemCustomMoves:
                    monStruct += mon.HeldItemMember + ',' +
                    mon.MovesMember;
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

        public static bool IfContainsPartyAddParty(string line, ref Dictionary<string, Party> parties) {

            string name = RegexTrainer.PartyName.Match(line).Value;
            TYPE type;

            if (TypeDict.TryGetValue(RegexTrainer.PartyType.Match(line).Value, out type)) {
                parties.Add(name, new Party(type, name));
                return true;
            }
            else
                return false;
        }
    }
}
