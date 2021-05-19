using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Trainer_Editor {
    public class Mon {
        public string Iv { get; set; }
        public string Lvl { get; set; }
        public string Species { get; set; }
        public string HeldItem { get; set; }
        public List<string> Moves { get; set; }

        public string SpeciesMember {
            get => string.IsNullOrEmpty(Species) ? "" : $"\n\t.species = {Species},";
        }
        public string LvlMember {
            get => string.IsNullOrEmpty(Lvl) ? "" : $"\n\t.lvl = {Lvl},";
        }
        public string IvMember {
            get => string.IsNullOrEmpty(Iv) ? "" : $"\n\t.iv = {Iv},";
        }
        public string HeldItemMember {
            get => string.IsNullOrEmpty(HeldItem) ? "" : $"\n\t.heldItem = {HeldItem}";
        }
        public string MovesMember {
            get {
                string movesText = "";
                if (Moves == null)
                    return movesText;

                for (int i = 0; i < Moves.Count; i++) {
                    movesText += Moves[i];
                    if (i < Moves.Count - 1)
                        movesText += ", ";
                }
            
                return $"\n\t.moves = {{{movesText}}}";
            }
        }

        public static List<string> MatchMoves(string monStruct) {
            
            List<string> moves = new List<string>();
            MatchCollection moveCollection = RegexMon.Moves.Matches(monStruct);
            for (int i = 0; i < moveCollection.Count; i++) {
                moves.Add(moveCollection[i].Value);
            }
            return moves;

        }
    }
}
