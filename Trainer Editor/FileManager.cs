using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace Trainer_Editor {

    public class FilePaths : ObservableObject {

        public static string TrainersHeader { get; set; } = @"C:\TrainerEditor\trainers.h";
        public static string TrainersHeaderTest { get; set; } = @"C:\TrainerEditor\tests\trainer_test.h";
        public static string PartiesHeader { get; set; } = @"C:\TrainerEditor\trainer_parties.h";
        public static string PartiesHeaderTest { get; set; } = @"C:\TrainerEditor\tests\party_test.h";
        public Dictionary<Constant, string> ConstantHeaders { get; set; } = new Dictionary<Constant, string>() {
            {Constant.Species, @"C:\TrainerEditor\headers\species.h" },
            {Constant.Moves, @"C:\TrainerEditor\headers\moves.h" },
            {Constant.Items, @"C:\TrainerEditor\headers\items.h" }
        };
        public Dictionary<Constant, string> ConstantLists { get; set; } = new Dictionary<Constant, string>() {
            {Constant.Species, @"C:\TrainerEditor\Species.json" },
            {Constant.Moves, @"C:\TrainerEditor\Moves.json" },
            {Constant.Items, @"C:\TrainerEditor\Items.json" }
        };
        
        public string SpeciesHeader {
            get => ConstantHeaders[Constant.Species];
            set { ConstantHeaders[Constant.Species] = value; OnPropertyChanged("SpeciesHeader"); }
        }
        public string MovesHeader {
            get => ConstantHeaders[Constant.Moves];
            set { ConstantHeaders[Constant.Moves] = value; OnPropertyChanged("MovesHeader"); }
        }
        public string ItemsHeader {
            get => ConstantHeaders[Constant.Items];
            set { ConstantHeaders[Constant.Items] = value; OnPropertyChanged("ItemsHeader"); }
        }
        //public string SpeciesConstants {
        //    get => ConstantListPaths[Constant.Species];
        //    set { ConstantListPaths[Constant.Species] = value; OnPropertyChanged("SpeciesConstants"); }
        //}
        //public string MovesConstants {
        //    get => ConstantListPaths[Constant.Moves];
        //    set { ConstantListPaths[Constant.Moves] = value; OnPropertyChanged("MovesConstants"); }
        //}
        //public string ItemsConstants {
        //    get => ConstantListPaths[Constant.Items];
        //    set { ConstantListPaths[Constant.Items] = value; OnPropertyChanged("ItemsConstants"); }
        //}

    }

    public class FileManager {

        public static Dictionary<string,Party> ParsePartiesHeader() {

            Dictionary<string, Party> namedParties = new Dictionary<string, Party>();

            StringBuilder sb = new StringBuilder();
            char cursor;
            string monStruct;
            string line;
            int open = 0;
            int closed = 0;
            
            try {
                using StreamReader sr = new StreamReader(FilePaths.PartiesHeader);
                while ((line = sr.ReadLine()) != null) {

                    if (Party.IfContainsPartyAddParty(line, namedParties)) {

                        while (sr.Peek() != ';' && sr.Peek() != -1) {

                            cursor = (char)sr.Read();
                            sb.Append(cursor);

                            if (cursor == '{')
                                open += 1;
                            else if (cursor == '}')
                                closed += 1;

                            if (open == closed && open > 0) {
                                monStruct = sb.ToString();
                                namedParties.Last().Value.AddMonFromStruct(monStruct);
                                sb.Clear();
                                open = 0;
                                closed = 0;
                            }
                        }
                        closed = 0;
                    }

                }
            }
            catch (Exception e) {
                Debug.WriteLine(e.Message);
            }

            return namedParties;
        }

        public static void WritePartiesHeader(List<Trainer> trainers) {
            if (trainers == null || trainers.Count == 0 || trainers[0]?.Party?.MonList?[0] == null) {
                MessageBox.Show("No Parties to Save.", "caption", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try {
                using StreamWriter sw = new StreamWriter(FilePaths.PartiesHeaderTest);
                for (int i = 0; i < trainers.Count; i++) {
                    sw.Write($"{trainers[i].Party.CreatePartyStruct()}");
                    sw.Write("\n\n");
                }
            }
            catch (Exception e) {
                Debug.WriteLine(e.Message);
            }
        }

        public static List<Trainer> ParseTrainersHeader() {

            List<Trainer> trainers = new List<Trainer>();

            StringBuilder sb = new StringBuilder();
            char cursor;
            string line;
            int open = 0;
            int closed = 0;

            try {
                using StreamReader sr = new StreamReader(FilePaths.TrainersHeader);
                while ((line = sr.ReadLine()) != null) {
                    if (RegexTrainer.IndexName.IsMatch(line)) {

                        while (sr.Peek() != '[' && sr.Peek() != -1) {

                            cursor = (char)sr.Read();
                            sb.Append(cursor);

                            if (cursor == '{')
                                open += 1;
                            else if (cursor == '}')
                                closed += 1;

                            if (open == closed && open > 0) {
                                trainers.Add(new Trainer(line, sb.ToString()));
                                sb.Clear();
                                open = 0;
                                closed = 0;

                            }

                        }
                    }
                }
            }
            catch (Exception e) {
                Debug.WriteLine(e.Message);
            }

            return trainers;
        }

        public static void WriteTrainersHeader(List<Trainer> trainers) {
            if (trainers == null || trainers.Count == 0) {
                MessageBox.Show("No Trainers to Save.", "caption", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try {
                using StreamWriter sw = new StreamWriter(FilePaths.TrainersHeaderTest);
                sw.Write(
                "const struct Trainer gTrainers[] = {" +
                "\n\t[TRAINER_NONE] =" +
                "\n\t{" +
                "\n\t\t.partyFlags = 0," +
                "\n\t\t.trainerClass = TRAINER_CLASS_PKMN_TRAINER_1," +
                "\n\t\t.encounterMusic_gender = TRAINER_ENCOUNTER_MUSIC_MALE," +
                "\n\t\t.trainerPic = TRAINER_PIC_HIKER," +
                "\n\t\t.trainerName = _(\"\")," +
                "\n\t\t.items = {}," +
                "\n\t\t.doubleBattle = FALSE," +
                "\n\t\t.aiFlags = 0," +
                "\n\t\t.partySize = 0," +
                "\n\t\t.party = {.NoItemDefaultMoves = NULL}," +
                "\n\t}," +
                "\n");

                foreach (Trainer trainer in trainers) {
                    sw.Write($"{trainer.CreateTrainerStruct()}");
                    sw.Write("\n");
                }

                sw.WriteLine("};");

            }
            catch (Exception e) {
                Debug.WriteLine(e.Message);
            }

        }

        public static List<Trainer> AttachPartiesToTrainers(List<Trainer> trainers, Dictionary<string, Party> parties) {

            for (int i = 0; i < parties.Count(); i++) {
                trainers[i].Party = parties.GetValueOrDefault(trainers[i].PartySize);
            }
            return trainers;

        }
        public static void DeserializeAllConstantLists(Data data) {
            data.SpeciesList = DeserializeConstantList(Constant.Species);
            data.MovesList = DeserializeConstantList(Constant.Moves);
            data.ItemsList = DeserializeConstantList(Constant.Items);
        }
        public static void ParseAllConstantHeadersToJson() {
            foreach (Constant constant in Enum.GetValues(typeof(Constant))) {
                ParseConstantHeaderToJson(constant);
            }
        }
        public static void ParseConstantHeaderToJson(Constant constant) {
            SerializeConstantList(constant, ParseConstantHeader(constant));
        }
        private static List<string> ParseConstantHeader(Constant constant) {
            try {
                Regex regex = Data.Instance.RegexConfig.ConstantRegexes[constant];

                string line;
                List<string> constants = new List<string>();

                using StreamReader sr = new StreamReader(Data.Instance.FilePaths.ConstantHeaders[constant]);
                while ((line = sr.ReadLine()) != null) {
                    if (regex.IsMatch(line))
                        constants.Add(regex.Match(line).Value);
                }

                return constants;
            }
            catch (Exception e) {
                throw e;
            }
        }
        private static void SerializeConstantList(Constant constant, List<string> constants) {
            try {
                constants.RemoveAll(s => s == string.Empty);
                Stream stream = new FileStream(Data.Instance.FilePaths.ConstantLists[constant], FileMode.Create, FileAccess.Write);
                using StreamWriter writer = new StreamWriter(stream);
                JsonSerializer json = new JsonSerializer { Formatting = Formatting.Indented };
                json.Serialize(writer, constants);
            }
            catch (Exception e) {
                throw e;
            }
        }
        private static List<string> DeserializeConstantList(Constant constant) {
            try {
                Stream stream = new FileStream(Data.Instance.FilePaths.ConstantLists[constant], FileMode.Open, FileAccess.Read);
                using StreamReader reader = new StreamReader(stream);
                JsonSerializer json = new JsonSerializer { Formatting = Formatting.Indented };
                List<string> constants = (List<string>)json.Deserialize(reader, typeof(List<string>));

                if (constant != Constant.Species)
                    constants.Add(string.Empty);
                return constants;
            }
            catch (Exception e) {
                throw e;
            }
        }
        public static void SerializeRegexConfig(RegexConfig regexConfig) {
            Stream stream = new FileStream(@"C:\TrainerEditor\RegexConfig.json", FileMode.Create, FileAccess.Write);
            using StreamWriter writer = new StreamWriter(stream);
            JsonSerializer json = new JsonSerializer { Formatting = Formatting.Indented };
            json.Serialize(writer, regexConfig);
        }

        public static RegexConfig DeserializeRegexConfig() {
            Stream stream = new FileStream(@"C:\TrainerEditor\RegexConfig.json", FileMode.Open, FileAccess.Read);
            using StreamReader reader = new StreamReader(stream);
            JsonSerializer json = new JsonSerializer { Formatting = Formatting.Indented };
            return (RegexConfig)json.Deserialize(reader, typeof(RegexConfig));
        }

    }
}
