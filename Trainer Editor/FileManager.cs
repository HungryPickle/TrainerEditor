using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

namespace Trainer_Editor {

    public class FileManager {
        private static FileManager instance = new FileManager();
        public static FileManager Instance {
            get { return instance; }
            set { instance = value; }
        }
        private FilePaths filePaths = new FilePaths();
        public FilePaths FilePaths {
            get { return filePaths; }
            set { filePaths = value; }
        }
        private RegexConfig regexConfig = new RegexConfig();
        public RegexConfig RegexConfig {
            get { return regexConfig; }
            set { regexConfig = value; }
        }
        private bool DirectoryNotSet { 
            get {
                if (string.IsNullOrEmpty(FilePaths.PokeEmeraldDirectory)) {
                    Data.Instance.StatusBar.Text = "Directory not set!";
                    return true;
                }
                else
                    return false;
            } 
        }

        public void ParseTrainersAndParties() {
            if (DirectoryNotSet) return;
            List<Trainer> trainers = ParseTrainersHeader();
            Dictionary<string,Party> parties = ParsePartiesHeader();
            for (int i = 0; i < parties.Count(); i++) {
                trainers[i].Party = parties.GetValueOrDefault(trainers[i].PartySize);
            }
            Data.Instance.Trainers = trainers;
        }
        private List<Trainer> ParseTrainersHeader() {

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

        private Dictionary<string,Party> ParsePartiesHeader() {

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
        public void WriteTrainersAndPartiesHeaders() {
            if (DirectoryNotSet) return;
            WriteTrainersHeader();
            WritePartiesHeader();
        }
        private void WriteTrainersHeader() {
            if (Data.Instance.Trainers == null || Data.Instance.Trainers.Count == 0) {
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

                foreach (Trainer trainer in Data.Instance.Trainers) {
                    sw.Write($"{trainer.CreateTrainerStruct()}");
                    sw.Write("\n");
                }

                sw.WriteLine("};");
            }
            catch (Exception e) {
                Debug.WriteLine(e.Message);
            }

        }

        private void WritePartiesHeader() {
            if (Data.Instance.Trainers == null || Data.Instance.Trainers.Count == 0 || Data.Instance.Trainers[0]?.Party?.MonList?[0] == null) {
                MessageBox.Show("No Parties to Save.", "caption", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try {
                using StreamWriter sw = new StreamWriter(FilePaths.PartiesHeaderTest);
                for (int i = 0; i < Data.Instance.Trainers.Count; i++) {
                    sw.Write($"{Data.Instance.Trainers[i].Party.CreatePartyStruct()}");
                    sw.Write("\n\n");
                }
            }
            catch (Exception e) {
                Debug.WriteLine(e.Message);
            }
        }

        public void ParseAllConstants() {
            if (DirectoryNotSet) return;
            foreach (Constant constant in Enum.GetValues(typeof(Constant))) {
                Data.Instance.SetConstantList(constant, ParseConstantHeader(constant));
            }
        }
        public void SerializeAllConstants() {
            foreach (Constant constant in Enum.GetValues(typeof(Constant))) {
                SerializeConstantList(constant, Data.Instance.GetConstantList(constant));
            }
        }
        public void DeserializeAllConstants() {
            foreach (Constant constant in Enum.GetValues(typeof(Constant))) {
                Data.Instance.SetConstantList(constant, DeserializeConstantList(constant));
            }
        }  

        private List<string> ParseConstantHeader(Constant constant) {
            try {
                Regex regex = RegexConfig.GetConstantRegex(constant);
                string line;
                List<string> constants = new List<string>();

                using StreamReader sr = new StreamReader(FilePaths.GetConstantHeader(constant));
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

        private void SerializeConstantList(Constant constant, List<string> constants) {
            constants.RemoveAll(s => s == string.Empty);
            Serialize(FilePaths.ConstantLists[constant], constants);
        }
        private List<string> DeserializeConstantList(Constant constant) {
            List<string> constants = Deserialize<List<string>>(FilePaths.ConstantLists[constant]);
            if (constant != Constant.Species)
                constants.Add(string.Empty);
            return constants;
        }
        public void SerializeRegexConfig() {
            Serialize(FilePaths.RegexJson, RegexConfig);
        }
        public void DeserializeRegexConfig() {
            RegexConfig = Deserialize<RegexConfig>(FilePaths.RegexJson);
        }

        public void SerializeFilePaths() {
            Serialize(FilePaths.FilePathsJson, FilePaths);
        }
        public void DeserializeFilePaths() {
            FilePaths = Deserialize<FilePaths>(FilePaths.FilePathsJson);
        }

        private void Serialize<T>(string path, T obj) {
            try {
                Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write);
                using StreamWriter writer = new StreamWriter(stream);
                JsonSerializer json = new JsonSerializer { Formatting = Formatting.Indented };
                json.Serialize(writer, obj);
            }
            catch (Exception e) {
                throw e;
            }
        }
        private T Deserialize<T>(string path) {
            try {
                Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
                using StreamReader reader = new StreamReader(stream);
                JsonSerializer json = new JsonSerializer { Formatting = Formatting.Indented };
                return (T)json.Deserialize(reader, typeof(T));
            }
            catch (Exception e) {
                throw e;
            }
        }
    }
}
