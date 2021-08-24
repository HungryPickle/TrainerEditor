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
        public Mon (PartyType partyType, string monStruct) {

            iv = RegexMon.IV.Match(monStruct).Value;
            lvl = RegexMon.Lvl.Match(monStruct).Value;
            species = RegexMon.Species.Match(monStruct).Value;

            switch (partyType) {
                case PartyType.TrainerMonNoItemDefaultMoves:
                    break;
                case PartyType.TrainerMonItemDefaultMoves:
                    heldItem = RegexMon.HeldItem.Match(monStruct).Value;
                    break;
                case PartyType.TrainerMonNoItemCustomMoves:
                    moves = RegexMon.MatchMoves(monStruct);
                    break;
                case PartyType.TrainerMonItemCustomMoves:
                    heldItem = RegexMon.HeldItem.Match(monStruct).Value;
                    moves = RegexMon.MatchMoves(monStruct);
                    break;
                case PartyType.TrainerMonCustom:
                    heldItem = RegexMon.HeldItem.Match(monStruct).Value;
                    moves = RegexMon.MatchMoves(monStruct);
                    lvlOffset = RegexMon.LvlOffset.Match(monStruct).Value;
                    ivs = RegexMon.MatchStats(monStruct, RegexMon.IVs);
                    evs = RegexMon.MatchStats(monStruct, RegexMon.EVs);
                    break;
                default:
                    break;
            }
        
        }

        public string CreateStruct(PartyType partyType) {
            string monStruct = $"\n\t{{" +
            IvMember +
            LvlMember +
            SpeciesMember;
            switch (partyType) {
                case PartyType.TrainerMonNoItemDefaultMoves:
                    break;
                case PartyType.TrainerMonItemDefaultMoves:
                    monStruct += HeldItemMember;
                    break;
                case PartyType.TrainerMonNoItemCustomMoves:
                    monStruct += MovesMember;
                    break;
                case PartyType.TrainerMonItemCustomMoves:
                    monStruct += HeldItemMember + MovesMember;
                    break;
                case PartyType.TrainerMonCustom:
                    monStruct += HeldItemMember + MovesMember + IVsMember + EVsMember;
                    break;
                default:
                    break;
            }
            monStruct += $"\n\t}}";

            return monStruct;
        }

        private string species;
        private string iv;
        private string lvl;
        private string lvlOffset = "";
        private string heldItem;
        private List<string> moves = new List<string> { "", "", "", "" };
        private List<Stat> ivs = new List<Stat>(6) { new Stat(), new Stat(), new Stat(), new Stat(), new Stat(), new Stat() };
        private List<Stat> evs = new List<Stat>(6) { new Stat(), new Stat(), new Stat(), new Stat(), new Stat(), new Stat() };

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
                iv = RegexInput.Digits.Match(value).Value;
                OnPropertyChanged("Iv");
            }
        }
        public List<Stat> IVs { get => ivs; set => ivs = value; }
        public List<Stat> EVs { get => evs; set => evs = value; }
        public string Lvl {
            get { return lvl; }
            set {
                string input = RegexInput.Digits.Match(value).Value;
                lvl = string.IsNullOrEmpty(input) ? "1" : input;
                OnPropertyChanged("Lvl");
            }
        }
        public string LvlOffset {
            get { return lvlOffset; }
            set { lvlOffset = value; OnPropertyChanged("LvlOffset"); }
        }
        public static List<string> LevelOffsets { get; set; } = new List<string>{
            "PLAYER_LEVEL_OFFSET + ",
            "PLAYER_LEVEL_OFFSET - ",
            ""
        };

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

        public string SpeciesMember {
            get => string.IsNullOrEmpty(Species) ? "" : $"\n\t.species = {Species},";
        }
        public string LvlMember {
            get => string.IsNullOrEmpty(Lvl) ? "" : $"\n\t.lvl = {LvlOffset}{Lvl},";
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
        public string IVsMember {
            get { return Stat.IsListEmpty(IVs) ? "" : $"\n\t.ivs = {{{Stat.ListToText(IVs)}}},"; }
        }
        public string EVsMember { 
            get { return Stat.IsListEmpty(EVs) ? "" : $"\n\t.evs = {{{Stat.ListToText(EVs)}}},"; } 
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
            set { 
                string input = RegexInput.Digits.Match(value).Value;
                text = string.IsNullOrEmpty(input) ? "0" : input;
                OnPropertyChanged("Text"); }
        }
        public static bool IsListEmpty(List<Stat> stats) {
            return stats.All(s => s.Text == "0") ? true : false;
        }
        public static string ListToText(List<Stat> stats) {
            string statsText = "";
            for (int i = 0; i < stats.Count; i++) {
                if (i > 0)
                    statsText += ", ";
                statsText += stats[i].Text;
            }
            return statsText;
        }
    }
}
