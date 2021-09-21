using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Trainer_Editor {
    public class Trainer : ObservableObject {
        private Party party;
        private string trainerPic;
        private string trainerClass;
        private List<string> partyFlags;
        private string indexName;
        private string encounterGender = "";
        private string encounterMusic;
        private string trainerName;
        private List<string> items = new List<string>{ "", "", "", "" };
        private string doubleBattle;
        private List<string> aiFlags;
        private string partySize;

        public string IndexName { get => indexName; set => indexName = value; }
        public List<string> PartyFlags { get => partyFlags; set => partyFlags = value; }
        public string TrainerClass { get => trainerClass; set { trainerClass = value; OnPropertyChanged("TrainerClass"); } }

        public string EncounterMusic { get => encounterMusic; set { encounterMusic = value; OnPropertyChanged("EncounterMusic"); } }
        public static readonly List<string> EncounterGenders = new List<string> { "F_TRAINER_FEMALE", "" };
        public string EncounterGender { get => encounterGender; set { encounterGender = value; OnPropertyChanged("EncounterGender"); } }
        public string TrainerPic { get => trainerPic; set { trainerPic = value; OnPropertyChanged("TrainerPic"); } }
        public string TrainerName { 
            get => trainerName; 
            set { 
                trainerName = value.Length > 11 ? value.Substring(0, 11) : value; 
                OnPropertyChanged("TrainerName"); 
            }
        }
        public List<string> Items { get => items; set => items = value; }
        public bool DoubleBattleBool {
            get { return doubleBattle == "TRUE" ? true : false; }
            set { doubleBattle = value == true ? "TRUE" : "FALSE"; }
        }
        public List<string> AiFlags { get => aiFlags; set => aiFlags = value; }
        public string PartySize { get => partySize; set => partySize = value; }
        public Party Party {
            get => party;
            set { party = value; }
        }

        public PartyTypes PartyType {
            get {
                return Party.Type;
            }
            set {
                Party.Type = value;
                OnPropertyChanged("PartyType");
                PartyFlags = Party.TypeToPartyFlags[value];
            }
        }
        public static Trainer CreateDummy() {
            Trainer trainer = new Trainer {
                party = Party.CreateDummy(),
                trainerClass = "TRAINER_CLASS_HEX_MANIAC",
                encounterGender = "F_TRAINER_FEMALE",
                encounterMusic = "TRAINER_ENCOUNTER_MUSIC_SUSPICIOUS",
                trainerPic = "TRAINER_PIC_HEX_MANIAC",
                trainerName = "TAMMY",
                items = new List<string>() { "ITEM_HYPER_POTION", "ITEM_FULL_RESTORE", "", "" },
                doubleBattle = "FALSE",
                aiFlags = new List<string>() { "AI_SCRIPT_CHECK_BAD_MOVE", "AI_SCRIPT_TRY_TO_FAINT", "AI_SCRIPT_CHECK_VIABILITY" }
            };
            trainer.partySize = trainer.Party.Name;
            trainer.partyFlags = Party.TypeToPartyFlags[trainer.PartyType];

            return trainer;
        }
        private Trainer() { }

        public Trainer(string nameLine, string trainerStruct) {

            indexName = RegexTrainer.IndexName.Match(nameLine).Value;

            partyFlags = RegexTrainer.MatchList(RegexTrainer.PartyFlags, trainerStruct);
            trainerClass = RegexTrainer.TrainerClass.Match(trainerStruct).Value;
            encounterGender = RegexTrainer.EncounterGender.Match(trainerStruct).Value;
            encounterMusic = RegexTrainer.EncounterMusic.Match(trainerStruct).Value;
            trainerPic = RegexTrainer.TrainerPic.Match(trainerStruct).Value;
            trainerName = RegexTrainer.TrainerName.Match(trainerStruct).Value;
            items = RegexTrainer.MatchItems(trainerStruct);
            doubleBattle = RegexTrainer.DoubleBattle.Match(trainerStruct).Value;
            aiFlags = RegexTrainer.MatchList(RegexTrainer.AiFlags, trainerStruct);
            partySize = RegexTrainer.PartySize.Match(trainerStruct).Value;
        }
        public string CreateTrainerStruct() {
            string partyStruct = $"\n\t[{IndexName}] =" +
                "\n\t{" +
                PartyFlagsMember +
                TrainerClassMember +
                EncounterMusic_genderMember +
                TrainerPicMember +
                TrainerNameMember +
                ItemsMember +
                DoubleBattleMember +
                AiFlagsMember +
                PartySizeMember +
                PartyMember +
                "\n\t},";

            return partyStruct;
        }
        
        public string TrainerIndexNameMember {
            get => string.IsNullOrEmpty(IndexName) ? "" : $"[{IndexName}] =";
        }
        public string PartyFlagsMember {
            get {
                string partyFlags = "";
                for (int i = 0; i < PartyFlags.Count; i++) {
                    if (i > 0)
                        partyFlags += " | ";
                    partyFlags += PartyFlags[i];
                }
                return $"\n\t\t.partyFlags = {partyFlags},";
            }
        }
        public string TrainerClassMember {
            get => string.IsNullOrEmpty(TrainerClass) ? "" : $"\n\t\t.trainerClass = {TrainerClass},";
        }

        public string EncounterMusic_genderMember {
            get {
                string gender = string.IsNullOrEmpty(EncounterGender) ? "" : EncounterGender + " | ";
                return $"\n\t\t.encounterMusic_gender = {gender}{EncounterMusic},";
            }
        }
        public string TrainerPicMember {
            get => string.IsNullOrEmpty(TrainerPic) ? "" : $"\n\t\t.trainerPic = {TrainerPic},";
        }
        public string TrainerNameMember {
            get => $"\n\t\t.trainerName = _(\"{TrainerName}\"),";
        }
        public string ItemsMember {
            get {
                string itemsText = "";
                foreach (string item in Items) {
                    if (string.IsNullOrEmpty(item))
                        continue;
                    itemsText += string.IsNullOrEmpty(itemsText) ? item : ", " + item;
                }
                return $"\n\t\t.items = {{{itemsText}}},";
            }
        }
        public string DoubleBattleMember {
            get => $"\n\t\t.doubleBattle = {doubleBattle},"; 
        }
        public string AiFlagsMember {
            get {
                string flagsText = "";

                foreach (string flag in AiFlags) {
                    if (string.IsNullOrEmpty(flag))
                        continue;
                    flagsText += string.IsNullOrEmpty(flagsText) ? flag : " | " + flag;
                }
                if (string.IsNullOrEmpty(flagsText))
                    flagsText = "0";
                return $"\n\t\t.aiFlags = {flagsText},";
            }
        }
        public string PartySizeMember {
            get => string.IsNullOrEmpty(PartySize) ?
                "\n\t\t.partySize = 0," :
                $"\n\t\t.partySize = ARRAY_COUNT({PartySize}),";
        }
        public string PartyMember {
            get {
                if (Party == null)
                    return "\n\t\t.party = {.NoItemDefaultMoves = NULL},";
                string type = Party.Type.ToString().Replace("TrainerMon", "");
                return $"\n\t\t.party = {{.{type} = {Party.Name}}},";
            }
        }
    }

}
