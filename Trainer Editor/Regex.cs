using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace Trainer_Editor {
    public class RegexTrainer {
        public static Regex PartyType = new Regex(@"(?<=const\s+struct\s+)\w+");
        public static Regex PartyName = new Regex(@"\w+(?=\[\])");

        //public static Regex IndexName = new Regex(@"(?<=\[)TRAINER_\w+(?=\])");
        public static Regex IndexName = new Regex(@"(?<=\[)TRAINER_(?!NONE)\w+(?=\])");


        public static Regex PartyFlags = new Regex(@"(?<=\.partyFlags\s+=.+)(F_TRAINER_\w+|0)");
        public static Regex TrainerClass = new Regex(@"(?<=\.trainerClass\s+=\s+)TRAINER_CLASS_\w+(?=,)");
        public static Regex EncounterGender = new Regex(@"(?<=\.encounterMusic_gender\s+=\s+)F_TRAINER_\w+");
        public static Regex EncounterMusic = new Regex(@"(?<=\.encounterMusic_gender\s+=.+)TRAINER_ENCOUNTER_MUSIC_\w+");
        public static Regex TrainerPic = new Regex(@"(?<=\.trainerPic\s+=\s+).+(?=,)");
        public static Regex TrainerName = new Regex(@"(?<=\.trainerName\s+=.+"").+(?=""\W,)");
        public static Regex Items = new Regex(@"(?<=\.items\s+=\s+.+)ITEM_\w+");
        public static Regex DoubleBattle = new Regex(@"(?<=\.doubleBattle\s+=\s+).+(?=,)");
        public static Regex AiFlags = new Regex(@"(?<=\.aiFlags\s+=.+)(AI_SCRIPT_\w+|0)");
        public static Regex PartySize = new Regex(@"(?<=\.partySize\s+=.+)\w+(?=\W,)");
        public static Regex Party = new Regex(@"(?<=\.party\s+=.+)sParty_\w+");

        //public static Regex Trainer = new Regex(@"\[TRAINER[\s\S]+?(?=\s+\[TRAINER)|(?<=},\s+)\[TRAINER[\s\S]+(?=};)");
        public static List<string> MatchList(Regex member, string trainerStruct) {

            return new List<string>(member.Matches(trainerStruct).Select(m => m.Value));
        }
        public static List<string> MatchItems(string trainerStruct) {
            List<string> items = new List<string> { "", "", "", "" };
            List<string> matches = new List<string>(RegexTrainer.Items.Matches(trainerStruct).Select(m => m.Value));
            for (int i = 0; i < matches.Count; i++) {
                items[i] = matches[i];
            }
            return items;
        }
    }
    public class RegexMon {

        public static Regex IV = new Regex(@"(?<=\.iv\s+=\s+).+(?=,)");
        public static Regex Lvl = new Regex(@"(?<=\.lvl\s+=.+)[0-9]+(?=,)");
        public static Regex LvlOffset = new Regex(@"(?<=\.lvl\s+=\s+)PLAYER_LEVEL_OFFSET\s(\-|\+)\s");
        public static Regex Species = new Regex(@"(?<=\.species\s+=\s+)SPECIES_\w+");
        public static Regex HeldItem = new Regex(@"(?<=\.heldItem\s+=\s+)ITEM_\w+");
        public static Regex Gender = new Regex(@"(?<=\.gender\s+=\s+)(MON_FEMALE|MON_MALE_TRAINERMON)");
        public static Regex Nickname = new Regex(@"(?<=\.nickname\s+=.+"").+(?=""\W,)");
        public static Regex Ball = new Regex(@"(?<=\.ball\s+=\s+)ITEM_\w+BALL");
        public static Regex Ability = new Regex(@"(?<=\.ability\s+=\s+)ABILITY_\w+");
        public static Regex Nature = new Regex(@"(?<=\.nature\s+=\s+)NATURE_\w+");
        

        private static Regex Moves = new Regex(@"(?<=\.moves\s+=.+)MOVE_\w+");  
        public static Regex IVs = new Regex(@"(?<=\.ivs\s+=.+)[0-9]+");
        public static Regex EVs = new Regex(@"(?<=\.evs\s+=.+)[0-9]+");

        public static Regex MonSwapStructs = new Regex(@"\.swapSpecies(\r|\n|\r\n|.)+?(?=},)");
        public static Regex SwapSpecies = new Regex(@"(?<=\.swapSpecies\s+=\s+)\w+");
        public static Regex SwapAtPlayerLvl = new Regex(@"(?<=\.swapAtPlayerLvl\s+=\s+)[0-9]+");
        public static Regex SwapLvl = new Regex(@"(?<=\.swapLvl\s+=.+)[0-9]+(?=,)");
        public static Regex SwapLvlOffset = new Regex(@"(?<=\.swapLvl\s+=\s+)PLAYER_LEVEL_OFFSET\s(\-|\+)\s");
        private static Regex SwapMoves = new Regex(@"(?<=\.swapMoves\s+=.+)MOVE_\w+");

        public static ObservableCollection<MonSwap> MatchMonSwaps(string monStruct) {
            ObservableCollection<MonSwap> monSwaps = new ObservableCollection<MonSwap>();
            List<string> monSwapStructs = new List<string>(RegexMon.MonSwapStructs.Matches(monStruct).Select(m => m.Value));
            
            for (int i = 0; i < monSwapStructs.Count(); i++) {
                MonSwap monSwap = new MonSwap();
                monSwap.Species = RegexMon.SwapSpecies.Match(monSwapStructs[i]).Value;
                monSwap.SwapAtPlayerLvl = RegexMon.SwapAtPlayerLvl.Match(monSwapStructs[i]).Value;
                monSwap.Lvl = RegexMon.SwapLvl.Match(monSwapStructs[i]).Value;
                monSwap.LvlOffset = RegexMon.SwapLvlOffset.Match(monSwapStructs[i]).Value;
                monSwap.Moves = MatchSwapMoves(monSwapStructs[i]);
                monSwaps.Add(monSwap);
            }
            return monSwaps;
        }
        public static List<string> MatchSwapMoves(string monSwapStruct) {
            return MatchMoves(monSwapStruct, RegexMon.SwapMoves);
        }
        public static List<string> MatchMoves(string monStruct) {
            return MatchMoves(monStruct, RegexMon.Moves);
        }
        private static List<string> MatchMoves(string monStruct, Regex regex) {
            List<string> moves = new List<string> { "", "", "", "" };
            List<string> matches = new List<string>(regex.Matches(monStruct).Select(m => m.Value));
            for (int i = 0; i < matches.Count; i++) {
                moves[i] = matches[i];
            }
            return moves;
        }
        public static List<Stat> MatchStats(string monStruct, Regex stat) {
            List<Stat> stats = new List<Stat> { new Stat(), new Stat(), new Stat(), new Stat(), new Stat(), new Stat() };
            List<string> matches = new List<string>(stat.Matches(monStruct).Select(m => m.Value));
            for (int i = 0; i < matches.Count; i++) {
                stats[i].Text = matches[i];
            }
            return stats;
        }
    }
    public class RegexInput {
        //public static Regex Digits = new Regex(@"[1-9][0-9]*");
        public static Regex Digits = new Regex(@"[0-9]+");
    }

}
