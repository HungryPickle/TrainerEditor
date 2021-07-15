using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Trainer_Editor {
    public class Mon : ObservableObject {
        public Mon() { }
        public Mon(string species) {
            Species = species;
        }
        private string species;
        private string iv;

        public string Iv {
            get { return iv; }
            set {
                string match = RegexInput.Digits.Match(value).Value;
                match = match != "" ? match : "0";
                iv = int.Parse(match) < 255 ? match : "255";
            }
        }
        public string Lvl { get; set; }
        public string Species {
            get { return species; }
            set {
                //if (value.Length < 2) { return; }

                //if (Data.Instance.CulledSpeciesList.Contains(value)) {
                //if (value != null) {
                if (value == null) { throw new ArgumentNullException("Mon.Species"); }
                    species = value;
                    OnPropertyChanged("Species");
                    //Debug.WriteLine(value.ToString());
                //}
                //}
                //else {
                //    Data.Instance.CulledSpeciesList = new ObservableCollection<string>(Constants.Species.Where(s => s.Contains(value, StringComparison.OrdinalIgnoreCase)).OrderBy(s => s));
                //}
                
            }
        }
        public string HeldItem { get; set; }
        public ObservableCollection<string> Moves { get; set; }

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

        public static ObservableCollection<string> MatchMoves(string monStruct) {

            return new ObservableCollection<string>(RegexMon.Moves.Matches(monStruct).Select(m => m.Value));

        }
    }
}
