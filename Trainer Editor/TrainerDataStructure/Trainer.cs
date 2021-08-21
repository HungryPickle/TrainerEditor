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
        private List<string> encounterMusic_gender;
        private string trainerName;
        private List<string> items;
        private string doubleBattle;
        private List<string> aiFlags;
        private string partySize;

        public string IndexName { get => indexName; set => indexName = value; }
        public List<string> PartyFlags { get => partyFlags; set => partyFlags = value; }
        public string TrainerClass { get => trainerClass; set { trainerClass = value; OnPropertyChanged("TrainerClass"); } }
        public List<string> EncounterMusic_gender { get => encounterMusic_gender; set => encounterMusic_gender = value; }
        public string TrainerPic { get => trainerPic; set { trainerPic = value; OnPropertyChanged("TrainerPic"); } }
        public string TrainerName { get => trainerName; set => trainerName = value; }
        public List<string> Items { get => items; set => items = value; }
        public string DoubleBattle { get => doubleBattle; set => doubleBattle = value; }
        public List<string> AiFlags { get => aiFlags; set => aiFlags = value; }
        public string PartySize { get => partySize; set => partySize = value; }
        public Party Party {
            get => party;
            set { party = value; }
        }

        public PartyType PartyType {
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
                Party = Party.CreateDummy(),
                TrainerClass = "TRAINER_CLASS_HEX_MANIAC",
                EncounterMusic_gender = new List<string>() { "F_TRAINER_FEMALE", "TRAINER_ENCOUNTER_MUSIC_SUSPICIOUS" },
                TrainerPic = "TRAINER_PIC_HEX_MANIAC",
                TrainerName = "TAMMY",
                Items = new List<string>() { "ITEM_HYPER_POTION", "ITEM_FULL_RESTORE" },
                DoubleBattle = "FALSE",
                AiFlags = new List<string>() { "AI_SCRIPT_CHECK_BAD_MOVE", "AI_SCRIPT_TRY_TO_FAINT", "AI_SCRIPT_CHECK_VIABILITY" }
            };
            trainer.PartySize = trainer.Party.Name;
            trainer.PartyFlags = Party.TypeToPartyFlags[trainer.PartyType];

            return trainer;
        }
        private Trainer() { }

        public Trainer(string nameLine, string trainerStruct) {

            indexName = RegexTrainer.IndexName.Match(nameLine).Value;

            partyFlags = MatchList(RegexTrainer.PartyFlags, trainerStruct);
            trainerClass = RegexTrainer.TrainerClass.Match(trainerStruct).Value;
            encounterMusic_gender = MatchList(RegexTrainer.EncounterMusic_Gender, trainerStruct);
            trainerPic = RegexTrainer.TrainerPic.Match(trainerStruct).Value;
            trainerName = RegexTrainer.TrainerName.Match(trainerStruct).Value;
            items = MatchList(RegexTrainer.Items, trainerStruct);
            doubleBattle = RegexTrainer.DoubleBattle.Match(trainerStruct).Value;
            aiFlags = MatchList(RegexTrainer.AiFlags, trainerStruct);
            partySize = RegexTrainer.PartySize.Match(trainerStruct).Value;
        }
        public static List<string> MatchList(Regex member, string trainerStruct) {

            return new List<string>(member.Matches(trainerStruct).Select(m => m.Value));
        }
        public string TrainerIndexNameMember {
            get => string.IsNullOrEmpty(IndexName) ? "" : $"[{IndexName}] =";
        }
        public string PartyFlagsMember {
            get {
                string partyFlags = "";
                if (PartyFlags == null)
                    return partyFlags;

                for (int i = 0; i < PartyFlags.Count; i++) {
                    if (i > 0)
                        partyFlags += " | " + PartyFlags[i];
                    else
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
                string encounterMusic_gender = "";
                if (EncounterMusic_gender == null)
                    return encounterMusic_gender;

                for (int i = 0; i < EncounterMusic_gender.Count; i++) {
                    if (i > 0)
                        encounterMusic_gender += " | " + EncounterMusic_gender[i];
                    else
                        encounterMusic_gender += EncounterMusic_gender[i];
                }
                return $"\n\t\t.encounterMusic_gender = {encounterMusic_gender},";
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
                string items = "";

                for (int i = 0; i < Items.Count; i++) {
                    if (i > 0)
                        items += ", " + Items[i];
                    else
                        items += Items[i];
                }
                return $"\n\t\t.items = {{{items}}},";
            }
        }
        public string DoubleBattleMember {
            get => string.IsNullOrEmpty(DoubleBattle) ? "" : $"\n\t\t.doubleBattle = {DoubleBattle},";
        }
        public string AiFlagsMember {
            get {
                string aiFlags = "";
                if (AiFlags == null)
                    return aiFlags;

                for (int i = 0; i < AiFlags.Count; i++) {
                    if (i > 0)
                        aiFlags += " | " + AiFlags[i];
                    else
                        aiFlags += AiFlags[i];
                }
                return $"\n\t\t.aiFlags = {aiFlags},";
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
    }
}
