using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trainer_Editor {
     public class Mon {
        public string iv;
        public string lvl;
        public string species;
        public string heldItem;
        public List<string> moves;

        public string SpeciesMember() {
            return string.IsNullOrEmpty(species) ? "" : $"\n\t.species = {species},";
        }
        public string LvlMember() {
            return string.IsNullOrEmpty(lvl) ? "" : $"\n\t.lvl = {lvl},";
        }
        public string IvMember() {
            return string.IsNullOrEmpty(iv) ? "" : $"\n\t.iv = {iv},";
        }
        public string HeldItemMember() {
            return string.IsNullOrEmpty(heldItem) ? "" : $"\n\t.heldItem = {heldItem},";
        }
         public string MovesMember() {
            string movesMember = "";
            if (moves == null)
                return movesMember;
            for (int i = 0; i < moves.Count; i++) {
                movesMember += moves[i];
                if (i < moves.Count - 1)
                    movesMember += ", ";
            }

            return $"\n\t.moves = {{{movesMember}}}";
        }
        
    }
}
