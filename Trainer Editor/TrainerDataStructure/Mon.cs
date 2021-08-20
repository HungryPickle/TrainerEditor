using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Trainer_Editor {
    public class Mon : ObservableObject {
        public Mon() { }
        public Mon(string species) {
            Species = species;
            Iv = "0";
            Lvl = "2";
        }
        private string species;
        private string iv;
        private string lvl;
        private string heldItem;
        private List<string> moves = new List<string> { "", "", "", "" };
        private List<Stat> ivs = new List<Stat>(6) { new Stat(), new Stat(), new Stat(), new Stat(), new Stat(), new Stat()};

        public string Species {
            get { return species; }
            set {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException($"Mon.Species null, old value: {species}");
                species = value;
                OnPropertyChanged("Species");
            }
        }

        public string Iv {
            get { return iv; }
            set {
                string match = RegexInput.Digits.Match(value).Value;
                //match = string.IsNullOrWhiteSpace(match) ? "0" : match;
                //iv = int.Parse(match) < 255 ? match : "255";
                iv = match;
                OnPropertyChanged("Iv");
            }
        }
        public string Lvl {
            get { return lvl; }
            set {
                lvl = RegexInput.Digits.Match(value).Value;
                OnPropertyChanged("Lvl");
            }
        }
        public string HeldItem {
            get => heldItem;
            set {
                heldItem = value;
                OnPropertyChanged("HeldItem");
            }
        }
        public List<string> Moves {
            get => moves;
            set {
                moves = value;
                OnPropertyChanged("Moves");
            }
        }
        public List<Stat> IVs {
            get => ivs;
            set => ivs = value;
        }

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
            get => string.IsNullOrEmpty(HeldItem) ? "" : $"\n\t.heldItem = {HeldItem},";
        }
        public string MovesMember {
            get {
                string movesText = "";
                if (Moves.All(m => string.IsNullOrEmpty(m)))
                    return movesText;

                for (int i = 0; i < Moves.Count && !string.IsNullOrEmpty(Moves[i]); i++) {
                    if (i > 0)
                        movesText += ", ";
                    movesText += Moves[i];
                }

                return $"\n\t.moves = {{{movesText}}},";
            }
        }

        public static List<string> MatchMoves(string monStruct) {

            return new List<string>(RegexMon.Moves.Matches(monStruct).Select(m => m.Value));

        }
    }
    public class Stat : ObservableObject {
        public Stat() { }
        public Stat(string text) {
            Text = text;
        }
        private string text = "0";
        public string Text {
            get { return text; }
            set { text = RegexInput.Digits.Match(value).Value; OnPropertyChanged("Text"); }
        }
    }
}
