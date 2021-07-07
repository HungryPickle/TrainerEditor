using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Trainer_Editor {
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
            
            string path = @"C:\Users\Scott\Desktop\trainer_parties.h";
            
            try {
                using (StreamReader sr = new StreamReader(path)) {

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
            }
            catch (Exception e) {
                Debug.WriteLine(e.Message);
            }

            return namedParties;
        }

        public static void WriteParties(List<Trainer> trainers) {

            string path = @"C:\Users\Scott\Desktop\party_test.h";

            try {
                using (StreamWriter sw = new StreamWriter(path)) {

                    for (int i = 0; i < trainers.Count; i++) {
                        sw.Write($"{trainers[i].Party.CreatePartyStruct()}");
                        sw.Write("\n\n");
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

            }
            catch (Exception e) {
                Debug.WriteLine(e.Message);
            }

        }
        
    }
}
