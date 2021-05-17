using System;
using System.Collections.Generic;

namespace Trainer_Editor {
    public class TrainerMonItemDefaultMoves : Trainer {

        public TrainerMonItemDefaultMoves(string partyName) : base(partyName) {

        }

        override public void AddMonFromStruct(string monStruct) {

            Mon mon = new Mon();

            mon.iv = MonRegex.IV.Match(monStruct).Value;
            mon.lvl = MonRegex.Lvl.Match(monStruct).Value;
            mon.species = MonRegex.Species.Match(monStruct).Value;
            mon.heldItem = MonRegex.HeldItem.Match(monStruct).Value;

            Party.Add(mon);
        }

        override public string CreateMonMembers(Mon mon) {
            string monMembers = "";
            monMembers += mon.IvMember();
            monMembers += mon.LvlMember();
            monMembers += mon.SpeciesMember();
            monMembers += mon.HeldItemMember();

            return monMembers;
        }
    }

}
