using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace Trainer_Editor {
    public class FilePaths : ObservableObject {
        private string pokeEmeraldDirectory = "";
        public string PokeEmeraldDirectory {
            get => pokeEmeraldDirectory;
            set { pokeEmeraldDirectory = value;
                TrainersHeader = value + @"\src\data\trainers.h";
                PartiesHeader = value + @"\src\data\trainer_parties.h";
                SpeciesHeader = value + @"\include\constants\species.h";
                MovesHeader = value + @"\include\constants\moves.h";
                ItemsHeader = value + @"\include\constants\items.h";
                TrainerClassHeader = value + @"\include\constants\trainers.h";
            }
        }
        public string TrainersHeader { get; set; } = "";
        public string PartiesHeader { get; set; } = "";
        public string SpeciesHeader { get; set; } = "";
        public string MovesHeader { get; set; } = "";
        public string ItemsHeader { get; set; } = "";
        public string TrainerClassHeader { get; set; } = "";
        
        public string GetConstantHeader(Constant constant) {
            switch (constant) {
                case Constant.Species:
                    return SpeciesHeader;
                case Constant.Moves:
                    return MovesHeader;
                case Constant.Items:
                    return ItemsHeader;
                case Constant.TrainerClass:
                    return TrainerClassHeader;
                default:
                    MessageBox.Show("CostantHeader not implemented in FilePaths.GetConstantHeader");
                    return string.Empty;
            }
        }
        [JsonIgnore]
        public Dictionary<Constant, string> ConstantLists { get; set; } = new Dictionary<Constant, string>() {
            {Constant.Species, $"{Directory.GetCurrentDirectory()}\\Constants\\Species.json" },
            {Constant.Moves, $"{Directory.GetCurrentDirectory()}\\Constants\\Moves.json" },
            {Constant.Items, $"{Directory.GetCurrentDirectory()}\\Constants\\Items.json" },
            {Constant.TrainerClass, $"{Directory.GetCurrentDirectory()}\\Constants\\TrainerClass.json" }
        };

        public static readonly string FilePathsJson = $"{Directory.GetCurrentDirectory()}\\Config\\FilePaths.json";
        public static readonly string RegexJson = $"{Directory.GetCurrentDirectory()}\\Config\\RegexConstant.json";
        public static string TrainersHeaderTest { get; set; } = @"C:\TrainerEditor\tests\trainer_test.h";
        public static string PartiesHeaderTest { get; set; } = @"C:\TrainerEditor\tests\party_test.h";

    }
}
