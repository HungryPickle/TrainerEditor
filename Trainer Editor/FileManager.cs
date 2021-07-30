using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Trainer_Editor {

    public class FilePaths {
        private FilePaths() { }

        private static FilePaths instance = new FilePaths();
        public static FilePaths Instance { 
            get { return instance; }
        }

        private string regexConfig = @"C:\TrainerEditor\RegexConfig.slowpoketail";
        public string RegexConfig { get { return regexConfig; } set { regexConfig = value; } }

        private string speciesHeader = @"C:\TrainerEditor\species.h";
        private string speciesConstants = @"C:\TrainerEditor\Species.slowpoketail";
        public string SpeciesHeader { get => speciesHeader; set => speciesHeader = value; }
        public string SpeciesConstants { get => speciesConstants; set => speciesConstants = value; }

        private string itemsHeader = @"C:\TrainerEditor\items.h";
        private string itemsConstants = @"C:\TrainerEditor\Items.slowpoketail";
        public string ItemsHeader { get => itemsHeader; set => itemsHeader = value; }
        public string ItemsConstants { get => itemsConstants; set => itemsConstants = value; }

        private string movesHeader = @"C:\TrainerEditor\moves.h";
        private string movesConstants = @"C:\TrainerEditor\Moves.slowpoketail";
        public string MovesHeader { get => movesHeader; set => movesHeader = value; }
        public string MovesConstants { get => movesConstants; set => movesConstants = value; }


    }

    public class FileManager {

        public static Dictionary<string,Party> ReadParties() {

            //List<Party> parties = new List<Party>();
            Dictionary<string, Party> namedParties = new Dictionary<string, Party>();

            StringBuilder sb = new StringBuilder();
            char cursor;
            string monStruct;
            string line;
            int open = 0;
            int closed = 0;
            
            string path = @"C:\TrainerEditor\trainer_parties.h";
            
            try {
                using StreamReader sr = new StreamReader(path);
                while ((line = sr.ReadLine()) != null) {

                    if (Party.IfContainsPartyAddParty(line, ref namedParties)) {

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
                        //for (; namedParties.Last().Value.MonList.Count < 6; namedParties.Last().Value.MonList.Add(new Mon())) {
                        //
                        //}
                    }

                }
            }
            catch (Exception e) {
                Debug.WriteLine(e.Message);
            }

            return namedParties;
        }

        public static void WriteParties(List<Trainer> trainers) {

            string path = @"C:\TrainerEditor\party_test.h";

            try {
                using StreamWriter sw = new StreamWriter(path);
                for (int i = 0; i < trainers.Count; i++) {
                    sw.Write($"{trainers[i].Party.CreatePartyStruct()}");
                    sw.Write("\n\n");
                }

            }
            catch (Exception e) {
                Debug.WriteLine(e.Message);
            }

        }

        public static List<Trainer> ReadTrainers() {

            List<Trainer> trainers = new List<Trainer>();

            StringBuilder sb = new StringBuilder();
            char cursor;
            string line;
            int open = 0;
            int closed = 0;

            string path = @"C:\TrainerEditor\trainers.h";

            try {
                using StreamReader sr = new StreamReader(path);
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

        public static void WriteTrainers(List<Trainer> trainers) {

            string path = @"C:\TrainerEditor\trainer_test.h";

            try {
                using StreamWriter sw = new StreamWriter(path);
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
        
        private static string GetConstantPath(Constant constant) {
            switch (constant) {
                case Constant.Species:
                    return FilePaths.Instance.SpeciesConstants;
                case Constant.Moves:
                    return FilePaths.Instance.MovesConstants;
                case Constant.Items:
                    return FilePaths.Instance.ItemsConstants;
                default:
                    throw new Exception("Constant for GetConstantPath not implemented");
            }
        }
        private static string GetHeaderPath(Constant constant) {
            switch (constant) {
                case Constant.Species:
                    return FilePaths.Instance.SpeciesHeader;
                case Constant.Moves:
                    return FilePaths.Instance.MovesHeader;
                case Constant.Items:
                    return FilePaths.Instance.ItemsHeader;
                default:
                    throw new Exception("Constant for GetHeaderPath not implemented");
            }
        }

        public static List<string> ParseHeaderWriteConstants(Constant constant) {

            List<string> constants = ParseHeader(constant);
            WriteConstants(constants, constant);
            return ReadConstants(constant);
        }
        public static List<string> ParseHeader(Constant constant) {
            try {
                Regex regex = ReadConstantRegex(constant);

                string line;
                List<string> constants = new List<string>();

                using StreamReader sr = new StreamReader(GetHeaderPath(constant));
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
        public static void WriteConstants(List<string> constants, Constant constant) {
            try {
                using StreamWriter sw = new StreamWriter(GetConstantPath(constant));
                foreach (string con in constants) {
                    sw.WriteLine(con);
                }
            }
            catch (Exception e) {
                throw e;
            }
        }
        public static List<string> ReadConstants(Constant constant) {
            try {
                string line;
                List<string> constants = new List<string>();
                if (constant != Constant.Species)
                    constants.Add(string.Empty);

                using StreamReader sr = new StreamReader(GetConstantPath(constant));
                while ((line = sr.ReadLine()) != null) {
                    constants.Add(line);
                }

                return constants;
            }
            catch (Exception e) {
                throw e;
            }
        }
        public static Regex ReadConstantRegex(Constant constant) {
            try {
                using StreamReader sr = new StreamReader(FilePaths.Instance.RegexConfig);
                string regexConfig = sr.ReadToEnd();
                
                return new Regex(RegexConfig.Constants[constant].Match(regexConfig).Value);
            }
            catch (Exception e) {

                throw e;
            }
        }

    }
}
