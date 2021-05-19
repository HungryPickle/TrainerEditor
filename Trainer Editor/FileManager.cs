using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Trainer_Editor {
    public class FileManager {
        public static List<Party> ReadParties() {

            List<Party> parties = new List<Party>();

            StringBuilder sb = new StringBuilder();
            char cursor;
            string monStruct;
            string line;
            int open = 0;
            int closed = 0;
            
            string path = @"C:\Users\Scott\Desktop\trainer_parties.h";
            
            try {
                using (StreamReader sr = new StreamReader(path)) {

                    while ((line = sr.ReadLine()) != null) {

                        if (Party.IfContainsPartyAddParty(line, ref parties)) {

                            while (sr.Peek() != ';' && sr.Peek() != -1) {

                                cursor = (char)sr.Read();
                                sb.Append(cursor);

                                if (cursor == '{')
                                    open += 1;
                                else if (cursor == '}')
                                    closed += 1;

                                if (open == closed && open > 0) {
                                    monStruct = sb.ToString();
                                    parties.Last().AddMonFromStruct(monStruct);
                                    sb.Clear();
                                    open = 0;
                                    closed = 0;
                                }
                            }
                            closed = 0;
                        }

                    }
                }
            }
            catch (Exception e) {
                Debug.WriteLine(e.Message);
            }

            return parties;
        }

        public static void WriteParties(List<Trainer> trainers) {

            string path = @"C:\Users\Scott\Desktop\party_test.h";

            try {
                using (StreamWriter sw = new StreamWriter(path)) {

                    for (int i = 1; i < trainers.Count; i++) {
                        if (i > 1) {
                            sw.Write("\n");
                        }
                        sw.Write($"{trainers[i].Party.CreatePartyStruct()}");
                        sw.Write("\n");
                        
                    }

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

            string path = @"C:\Users\Scott\Desktop\trainers.h";

            try {
                using (StreamReader sr = new StreamReader(path)) {
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
            }
            catch (Exception e) {
                Debug.WriteLine(e.Message);
            }

            //try {
            //    using (StreamReader sr = new StreamReader(path)) {
            //        //line = sr.ReadToEnd();
            //        MatchCollection trainerMatches = TrainerRegex.Trainer.Matches(sr.ReadToEnd());
            //        //foreach(Match m in trainerMatches) {
            //        //    trainers.Add(new Trainer("", m.Value));
            //        //}
            //        for(int i = 0; i < trainerMatches.Count; i++) {
            //            trainers.Add(new Trainer("", trainerMatches[i].Value));
            //        }
            //    }
            //}
            //catch (Exception e) {
            //    Debug.WriteLine(e.Message);
            //}

            return trainers;
        }

        public static void WriteTrainers(List<Trainer> trainers) {

            string path = @"C:\Users\Scott\Desktop\trainer_test.h";

            try {
                using (StreamWriter sw = new StreamWriter(path)) {

                    sw.Write("const struct Trainer gTrainers[] = {");

                    foreach (Trainer trainer in trainers) {
                        sw.Write($"{trainer.CreateTrainerStruct()}");
                        sw.Write("\n");
                    }

                    sw.WriteLine("};");
                }

            }
            catch (Exception e) {
                Debug.WriteLine(e.Message);
            }

        }
        
    }
}
