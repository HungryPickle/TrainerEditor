using System;
using System.Collections.Generic;
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
        public string PartyName { get; set; }
        protected List<Mon> MonList { get; set; }

        public Party(TYPE partyType, string partyName) {
            Type = partyType;
            PartyName = partyName;
            MonList = new List<Mon>();
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
            string partyStruct = $"static const struct {Type} {PartyName}[] = {{";
            for (int i = 0; i < MonList.Count; i++) {
                partyStruct += CreateMonStruct(MonList[i]);
                if (i < MonList.Count - 1)
                    partyStruct += ",";
            }
            partyStruct += $"\n}};";
            return partyStruct;
        }

        public static bool IfContainsPartyAddParty(string line, ref List<Party> parties) {

            string partyName = RegexTrainer.PartyName.Match(line).Value;
            TYPE partyType;

            if (TypeDict.TryGetValue(RegexTrainer.PartyType.Match(line).Value, out partyType)) {
                parties.Add(new Party(partyType, partyName));
                return true;
            }
            else
                return false;
        }
    }
}
