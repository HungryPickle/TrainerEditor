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
    public class Mon : ObservableObject {
        public Mon() { }
        public Mon(string species) {
            Species = species;
            Iv = "0";
            Lvl = "2";
        }
        public PartyTypes PartyType { get; set; }
        public Mon (PartyTypes partyType, string monStruct) {

            PartyType = partyType;
            iv = RegexMon.IV.Match(monStruct).Value;
            lvl = RegexMon.Lvl.Match(monStruct).Value;
            species = RegexMon.Species.Match(monStruct).Value;

            switch (partyType) {
                case PartyTypes.TrainerMonNoItemDefaultMoves:
                    break;
                case PartyTypes.TrainerMonItemDefaultMoves:
                    heldItem = RegexMon.HeldItem.Match(monStruct).Value;
                    break;
                case PartyTypes.TrainerMonNoItemCustomMoves:
                    moves = RegexMon.MatchMoves(monStruct);
                    break;
                case PartyTypes.TrainerMonItemCustomMoves:
                    heldItem = RegexMon.HeldItem.Match(monStruct).Value;
                    moves = RegexMon.MatchMoves(monStruct);
                    break;
                case PartyTypes.TrainerMonCustom:
                    heldItem = RegexMon.HeldItem.Match(monStruct).Value;
                    moves = RegexMon.MatchMoves(monStruct);
                    lvlOffset = RegexMon.LvlOffset.Match(monStruct).Value;
                    ivs = RegexMon.MatchStats(monStruct, RegexMon.IVs);
                    evs = RegexMon.MatchStats(monStruct, RegexMon.EVs);
                    gender = RegexMon.Gender.Match(monStruct).Value;
                    nickName = RegexMon.Nickname.Match(monStruct).Value;
                    ball = RegexMon.Ball.Match(monStruct).Value;
                    ability = RegexMon.Ability.Match(monStruct).Value;
                    nature = RegexMon.Nature.Match(monStruct).Value;
                    friendship = RegexMon.Friendship.Match(monStruct).Value;
                    shiny = RegexMon.Shiny.Match(monStruct).Value;
                    monSwaps = new MonSwaps(RegexMon.MatchMonSwaps(monStruct));
                    break;
                default:
                    break;
            }
        
        }

        public string CreateStruct(PartyTypes partyType) {
            string monStruct = $"\n\t{{" +
            IvMember +
            LvlMember +
            SpeciesMember;
            switch (partyType) {
                case PartyTypes.TrainerMonNoItemDefaultMoves:
                    break;
                case PartyTypes.TrainerMonItemDefaultMoves:
                    monStruct += HeldItemMember;
                    break;
                case PartyTypes.TrainerMonNoItemCustomMoves:
                    monStruct += MovesMember;
                    break;
                case PartyTypes.TrainerMonItemCustomMoves:
                    monStruct += HeldItemMember + MovesMember;
                    break;
                case PartyTypes.TrainerMonCustom:
                    monStruct += HeldItemMember 
                        + MovesMember 
                        + IVsMember 
                        + EVsMember 
                        + GenderMember
                        + NatureMember 
                        + NicknameMember 
                        + BallMember
                        + AbilityMember 
                        + FriendshipMember
                        + ShinyMember
                        + MonSwapsMember;
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
        private string lvlOffset;
        private string heldItem;
        private List<string> moves = new List<string> { "", "", "", "" };
        private List<Stat> ivs = new List<Stat>(6) { new Stat(), new Stat(), new Stat(), new Stat(), new Stat(), new Stat() };
        private List<Stat> evs = new List<Stat>(6) { new Stat(), new Stat(), new Stat(), new Stat(), new Stat(), new Stat() };
        private string gender;
        private string nickName;
        private string ball;
        private string ability;
        private string nature;
        private string friendship;
        private string shiny;
        private MonSwaps monSwaps = new MonSwaps(new ObservableCollection<MonSwap>());

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
                string input = RegexInput.Digits.Match(value).Value;
                iv = string.IsNullOrEmpty(input) ? "0" : input;
                OnPropertyChanged("Iv");
            }
        }
        public string Lvl {
            get { return lvl; }
            set {
                string input = RegexInput.Digits.Match(value).Value;
                lvl = string.IsNullOrEmpty(input) ? "2" : input;
                OnPropertyChanged("Lvl");
            }
        }
        public string LvlOffset {
            get { return lvlOffset; }
            set { lvlOffset = value; OnPropertyChanged("LvlOffset"); }
        }
        public static readonly List<string> LvlOffsets = new List<string>{
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
        public List<Stat> IVs { get => ivs; set { ivs = value; OnPropertyChanged("IVs"); } }
        public List<Stat> EVs { get => evs; set { evs = value; OnPropertyChanged("EVs"); } }
        public string Gender { get => gender; set { gender = value; OnPropertyChanged("Gender"); } }
        public static readonly List<string> Genders = new List<string> {
            "MON_FEMALE",
            "MON_MALE_TRAINERMON",
            ""
        };
        public string Nickname { 
            get => nickName; 
            set { 
                nickName = value.Length > 10 ? value.Substring(0, 10) : value;
                OnPropertyChanged("NickName");
            } 
        }
        public string Ball { get => ball; set { ball = value; OnPropertyChanged("Ball"); } }
        public string Ability { get => ability; set { ability = value; OnPropertyChanged("Ability"); } }
        public static readonly List<string> Abilities = new List<string> {
            "ABILITY_SLOT_1",
            "ABILITY_SLOT_2",
            "ABILITY_HIDDEN",
            ""
        };
        public string Nature { get => nature; set { nature = value; OnPropertyChanged("Nature"); } }
        public string Friendship { get => friendship; set { friendship = value; OnPropertyChanged("Friendship"); } }
        public static readonly List<string> FriendshipValues = new List<string> {
            "FRIENDSHIP_FRUSTRATION",
            "FRIENDSHIP_RETURN",
            ""
        };
        public bool ShinyBool {
            get { return shiny == "TRUE" ? true : false; }
            set { shiny = value == true ? "TRUE" : "FALSE"; }
        }
        public MonSwaps MonSwaps { get => monSwaps; set { monSwaps = value; OnPropertyChanged("MonSwaps"); } }

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
            get {
                string comma = PartyType != PartyTypes.TrainerMonItemDefaultMoves ? "," : "";
                return string.IsNullOrEmpty(HeldItem) ? "" : $"\n\t.heldItem = {HeldItem}{comma}"; 
            }
        }
        public string MovesMember {
            get {
                string movesText = "";
                if (Moves.All(m => string.IsNullOrEmpty(m)))
                    return movesText;

                foreach(string move in Moves) {
                    if (string.IsNullOrEmpty(move))
                        continue;
                    movesText += string.IsNullOrEmpty(movesText) ? move : ", " + move;
                }
                string comma = PartyType == PartyTypes.TrainerMonCustom ? "," : "";
                return $"\n\t.moves = {{{movesText}}}{comma}";
            }
        }
        public string IVsMember {
            get { return Stat.IsListEmpty(IVs) ? "" : $"\n\t.ivs = {{{Stat.ListToText(IVs)}}},"; }
        }
        public string EVsMember { 
            get { return Stat.IsListEmpty(EVs) ? "" : $"\n\t.evs = {{{Stat.ListToText(EVs)}}},"; } 
        }
        public string GenderMember {
            get { return string.IsNullOrEmpty(Gender) ? "" : $"\n\t.gender = {Gender},"; }
        }
        public string NicknameMember {
            get { return string.IsNullOrEmpty(Nickname) ? "" : $"\n\t.nickname = _(\"{Nickname}\"),"; }
        }
        public string BallMember {
            get { return string.IsNullOrEmpty(Ball) ? "" : $"\n\t.ball = {Ball},"; }
        }
        public string AbilityMember {
            get { return string.IsNullOrEmpty(Ability) ? "" : $"\n\t.ability = {Ability},"; }
        }
        public string NatureMember {
            get { return string.IsNullOrEmpty(Nature) ? "" : $"\n\t.nature = {Nature},"; }
        }
        public string FriendshipMember {
            get { return string.IsNullOrEmpty(Friendship) ? "" : $"\n\t.friendship = {Friendship},"; }
        }
        public string ShinyMember {
            get { return string.IsNullOrEmpty(shiny) ? "" : $"\n\t.shiny = {shiny},"; }
        }

        public string MonSwapsMember {
            get {
                string monSwapsText = "";
                if (MonSwaps.List.All(m => string.IsNullOrEmpty(m.Species)))
                    return monSwapsText;
                monSwapsText += "\n\t.monSwaps = {";
                foreach(MonSwap monSwap in MonSwaps.List) {
                    if (string.IsNullOrEmpty(monSwap.Species))
                        continue;
                    monSwapsText +=
                        "\n\t\t{" +
                        monSwap.SpeciesMember +
                        monSwap.SwapAtPlayerLvlMember +
                        monSwap.LvlMember +
                        monSwap.MovesMember +
                        "\n\t\t},";
                }
                monSwapsText += "\n\t},";
                return monSwapsText;
            }
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
    public class MonSwap : ObservableObject {
        private string species;
        private string lvl = "";
        private string lvlOffset;
        private string swapAtPlayerLvl;
        private List<string> moves = new List<string> { "", "", "", "" };

        public string Species {
            get { return species; }
            set { species = value; }
        }

        public string Lvl {
            get { return lvl; }
            set { lvl = value; }
        }

        public string LvlOffset {
            get { return lvlOffset; }
            set { lvlOffset = value; }
        }

        public string SwapAtPlayerLvl {
            get { return swapAtPlayerLvl; }
            set { swapAtPlayerLvl = value; }
        }

        public List<string> Moves {
            get { return moves; }
            set { moves = value; }
        }
        public string SpeciesMember { get => string.IsNullOrEmpty(Species) ? "" : $"\n\t\t.swapSpecies = {Species},"; }
        public string SwapAtPlayerLvlMember { get => string.IsNullOrEmpty(SwapAtPlayerLvl) ? "" : $"\n\t\t.swapAtPlayerLvl = {SwapAtPlayerLvl},"; }
        public string LvlMember { get => string.IsNullOrEmpty(Lvl) ? "" : $"\n\t\t.swapLvl = {LvlOffset}{Lvl},"; }
        public string MovesMember {
            get {
                string movesText = "";
                if (Moves.All(m => string.IsNullOrEmpty(m)))
                    return movesText;

                foreach (string move in Moves) {
                    if (string.IsNullOrEmpty(move))
                        continue;
                    movesText += string.IsNullOrEmpty(movesText) ? move : ", " + move;
                }
                return $"\n\t\t.swapMoves = {{{movesText}}}";
            }
        }
    }
    public class MonSwaps : ObservableObject{
        public MonSwaps(ObservableCollection<MonSwap> monSwaps) {
            list = monSwaps;
            list.CollectionChanged += new NotifyCollectionChangedEventHandler(MonSwapsChanged);
        }

        private ObservableCollection<MonSwap> list;
        public ObservableCollection<MonSwap> List { get => list; }
        public MonSwap this[int index] {
            get {
                if (index < list.Count)
                    return list[index];
                else
                    return null;
            }
            set { 
                if (index < list.Count)
                    list[index] = value; 
            }
        }
        public void MonSwapsChanged(object sender, NotifyCollectionChangedEventArgs e) {
            OnPropertyChanged("Item[]");
        }
    }
}
