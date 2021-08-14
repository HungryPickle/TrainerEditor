using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Trainer_Editor {
    public class RegexTrainer {
        public static Regex PartyType = new Regex(@"(?<=const\s+struct\s+)\w+");
        public static Regex PartyName = new Regex(@"\w+(?=\[\])");

        //public static Regex IndexName = new Regex(@"(?<=\[)TRAINER_\w+(?=\])");
        public static Regex IndexName = new Regex(@"(?<=\[)TRAINER_(?!NONE)\w+(?=\])");


        public static Regex PartyFlags = new Regex(@"(?<=\.partyFlags\s+=.+)(F_TRAINER_\w+|0)");
        public static Regex TrainerClass = new Regex(@"(?<=\.trainerClass\s+=\s+)TRAINER_CLASS_\w+(?=,)");
        public static Regex EncounterMusic_Gender = new Regex(@"(?<=\.encounterMusic_gender\s+=.+)\w+");
        public static Regex TrainerPic = new Regex(@"(?<=\.trainerPic\s+=\s+).+(?=,)");
        public static Regex TrainerName = new Regex(@"(?<=\.trainerName\s+=.+"").+(?=""\W,)");
        public static Regex Items = new Regex(@"(?<=\.items\s+=\s+.+)ITEM_\w+");
        public static Regex DoubleBattle = new Regex(@"(?<=\.doubleBattle\s+=\s+).+(?=,)");
        public static Regex AiFlags = new Regex(@"(?<=\.aiFlags\s+=.+)(AI_SCRIPT_\w+|0)");
        public static Regex PartySize = new Regex(@"(?<=\.partySize\s+=.+)\w+(?=\W,)");
        public static Regex Party = new Regex(@"(?<=\.party\s+=.+)sParty_\w+");

        //public static Regex Trainer = new Regex(@"\[TRAINER[\s\S]+?(?=\s+\[TRAINER)|(?<=},\s+)\[TRAINER[\s\S]+(?=};)");
    }
    public class RegexMon {

        public static Regex IV = new Regex(@"(?<=\.iv\s+=\s+).+(?=,)");
        public static Regex Lvl = new Regex(@"(?<=\.lvl\s+=\s+).+(?=,)");
        public static Regex Species = new Regex(@"(?<=\.species\s+=\s+)SPECIES_\w+");
        public static Regex HeldItem = new Regex(@"(?<=\.heldItem\s+=\s+)ITEM_\w+");
        public static Regex Moves = new Regex(@"(?<=\.moves\s+=.+)MOVE_\w+");

    }

    public class RegexConfig : ObservableObject {
        public static Regex SpeciesDefault { get; set; } = new Regex(@"(?<=#define\s+)SPECIES\w+(?=\s)");
        public static Regex MovesDefault { get; set; } = new Regex(@"MOVE_\w+");
        public static Regex ItemsDefault { get; set; } = new Regex(@"(?<=#define\s+)ITEM(\w(?!USE_|B_USE|_COUNT|FIELD_ARROW))+(?=\s)");

        [JsonIgnore]
        public Dictionary<Constant, Regex> ConstantRegexes { get; set; } = new Dictionary<Constant, Regex> {
            { Constant.Species, SpeciesDefault },
            { Constant.Moves, MovesDefault },
            { Constant.Items, ItemsDefault },
        };
        public void SetRegex(Constant constant, Regex value, Regex defaultValue) {
            if (!string.IsNullOrEmpty(value?.ToString()))
                ConstantRegexes[constant] = value;
            //else 
            //    ConstantRegexes[constant] = defaultValue;
        }

        public Regex Species {
            get { return ConstantRegexes[Constant.Species]; }
            set { SetRegex(Constant.Species, value, SpeciesDefault); OnPropertyChanged("Species"); }
        }
        public Regex Moves {
            get { return ConstantRegexes[Constant.Moves]; }
            set { SetRegex(Constant.Moves, value, MovesDefault); OnPropertyChanged("Moves"); }
        }
        public Regex Items {
            get { return ConstantRegexes[Constant.Items]; }
            set { SetRegex(Constant.Items, value, ItemsDefault); OnPropertyChanged("Items"); }
        }

    }
    public class RegexInput {
        //public static Regex Digits = new Regex(@"[1-9][0-9]*");
        public static Regex Digits = new Regex(@"[0-9]+");
    }

}
