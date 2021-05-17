using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Trainer_Editor {
    public class MonRegex {
        private static Regex TrainerType = new Regex(@"(?<=const\s+struct\s+)\w+");
        private static Regex TrainerPartyName = new Regex(@"\w+(?=\[\])");

        public static Regex IV = new Regex(@"(?<=\.iv\s+=\s+).+(?=,)");
        public static Regex Lvl = new Regex(@"(?<=\.lvl\s+=\s+).+(?=,)");
        public static Regex Species = new Regex(@"(?<=\.species\s+=\s+).+(?=,)");
        public static Regex HeldItem = new Regex(@"(?<=\.heldItem\s+=\s+)\w+");
        private static Regex MovesArray = new Regex(@"(?<=\.moves\s+=\s+){.+}");
        private static Regex Move = new Regex(@"MOVE_\w+");

        public static bool IfContainsTrainerAddTrainer(string line, ref List<Trainer> trainers) {

            string partyType = TrainerType.Match(line).Value;
            string partyName = TrainerPartyName.Match(line).Value;
            
            if (partyType.Equals(typeof(TrainerMonNoItemDefaultMoves).Name)) {
                trainers.Add(new TrainerMonNoItemDefaultMoves(partyName));
                return true;
            }
            else if (partyType.Equals(typeof(TrainerMonNoItemCustomMoves).Name)) {
                trainers.Add(new TrainerMonNoItemCustomMoves(partyName));
                return true;
            }
            else if (partyType.Equals(typeof(TrainerMonItemDefaultMoves).Name)) {
                trainers.Add(new TrainerMonItemDefaultMoves(partyName));
                return true;
            }
            else if (partyType.Equals(typeof(TrainerMonItemCustomMoves).Name)) {
                trainers.Add(new TrainerMonItemCustomMoves(partyName));
                return true;
            }
            else {
                return false;
            }
        }

        public static List<string> MatchMoves(string monStruct) {
            if (MonRegex.MovesArray.IsMatch(monStruct)) {

                List<string> moves = new List<string>();
                string movesArray = MonRegex.MovesArray.Match(monStruct).Value;
                MatchCollection moveCollection = MonRegex.Move.Matches(movesArray);
                foreach (Match move in moveCollection) {
                    moves.Add(move.Value);
                }
                return moves;
            }
            else {
                return null;
            }
        }

    }
}
