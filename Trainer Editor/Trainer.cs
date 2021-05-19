using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace Trainer_Editor {
    public class Trainer {
        public string TrainerIndexName { get; set; }
        public List<string> PartyFlags { get; set; }
        public string TrainerClass { get; set; }
        public List<string> EncounterMusic_gender { get; set; }
        public string TrainerPic { get; set; }
        public string TrainerName { get; set; }
        public List<string> Items { get; set; }
        public string DoubleBattle { get; set; }
        public List<string> AiFlags { get; set; }
        public string PartySize { get; set; }
        public Party Party { get; set; }

        public Trainer(string nameLine, string trainerStruct) {

            TrainerIndexName = RegexTrainer.IndexName.Match(nameLine).Value;

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

            MatchCollection matchCollection = member.Matches(trainerStruct);
            List<string> list = new List<string>();
            for (int i = 0; i < matchCollection.Count; i++) {
                list.Add(matchCollection[i].Value);
            }
            return list;

        }
        public string TrainerIndexNameMember {
            get => string.IsNullOrEmpty(TrainerIndexName) ? "" : $"[{TrainerIndexName}] =";
        }
        public string PartyFlagsMember {
            get {
                string partyFlags = "";
                if (PartyFlags == null)
                    return partyFlags;

                for (int i = 0; i < PartyFlags.Count; i++) {
                    partyFlags += PartyFlags[i];
                    if (i < PartyFlags.Count - 1)
                        partyFlags += " | ";
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
                    encounterMusic_gender += EncounterMusic_gender[i];
                    if (i < EncounterMusic_gender.Count - 1)
                        encounterMusic_gender += " | ";
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
                    items += Items[i];
                    if (i < Items.Count - 1)
                        items += ", ";
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
                    aiFlags += AiFlags[i];
                    if (i < AiFlags.Count - 1) {
                        aiFlags += " | ";
                    }
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
                return $"\n\t\t.party = {{.{type} = {Party.PartyName}}},";
            }
        }
        public string CreateTrainerStruct() {
            string partyStruct = $"\n\t[{TrainerIndexName}] =" +
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
