using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.ObjectModel;

namespace Trainer_Editor {
    public class Trainer : ObservableObject {
        private Party party;

        public string IndexName { get; set; }
        public List<string> PartyFlags { get; set; }
        public string TrainerClass { get; set; }
        public List<string> EncounterMusic_gender { get; set; }
        public string TrainerPic { get; set; }
        public string TrainerName { get; set; }
        public List<string> Items { get; set; }
        public string DoubleBattle { get; set; }
        public List<string> AiFlags { get; set; }
        public string PartySize { get; set; }
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
            Trainer trainer = new Trainer();
            trainer.Party = Party.CreateDummy();
            trainer.PartyFlags = Party.TypeToPartyFlags[trainer.PartyType];
            return trainer;
        }
        private Trainer() { }

        public Trainer(string nameLine, string trainerStruct) {

            IndexName = RegexTrainer.IndexName.Match(nameLine).Value;

            PartyFlags = MatchList(RegexTrainer.PartyFlags, trainerStruct);
            TrainerClass = RegexTrainer.TrainerClass.Match(trainerStruct).Value;
            EncounterMusic_gender = MatchList(RegexTrainer.EncounterMusic_Gender, trainerStruct);
            TrainerPic = RegexTrainer.TrainerPic.Match(trainerStruct).Value;
            TrainerName = RegexTrainer.TrainerName.Match(trainerStruct).Value;
            Items = MatchList(RegexTrainer.Items, trainerStruct);
            DoubleBattle = RegexTrainer.DoubleBattle.Match(trainerStruct).Value;
            AiFlags = MatchList(RegexTrainer.AiFlags, trainerStruct);
            PartySize = RegexTrainer.PartySize.Match(trainerStruct).Value;
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
