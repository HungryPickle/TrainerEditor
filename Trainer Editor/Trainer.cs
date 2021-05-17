using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace Trainer_Editor {

    public abstract class Trainer {
        public Trainer(string partyName) {
            TrainerPartyName = partyName;
            Party = new List<Mon>();
        }
        protected string TrainerPartyName { get; set; }

        protected List<Mon> Party { get; set; }

        abstract public void AddMonFromStruct(string monStruct);
        abstract public string CreateMonMembers(Mon mon);

        public void PrintPartySpecies() {
            foreach (Mon mon in Party) {
                if (!string.IsNullOrEmpty(mon.species))
                    Debug.WriteLine(mon.species);
            }
        }

        private string CreateMonStruct(Mon mon) {
            string monCode = $"\n\t{{";
            monCode += CreateMonMembers(mon);
            monCode += $"\n\t}}";
            return monCode;
        }

        public string CreatePartyStruct() {
            string partyCode = $"static const struct {GetType().Name} {TrainerPartyName}[] = {{";
            for (int i = 0; i < Party.Count; i++) {
                partyCode += CreateMonStruct(Party[i]);
                if (i < Party.Count - 1)
                    partyCode += ",";
            }
            partyCode += $"\n}};";
            return partyCode;
        }

    }
}
