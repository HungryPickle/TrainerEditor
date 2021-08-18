using Newtonsoft.Json;
using System;
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
            }
        }
        public string TrainersHeader { get; set; } = "";
        public string PartiesHeader { get; set; } = "";

        public static readonly string JsonPath = $"{Directory.GetCurrentDirectory()}\\Config\\FilePaths.json";
        public static string TrainersHeaderTest { get; set; } = @"C:\TrainerEditor\tests\trainer_test.h";
        public static string PartiesHeaderTest { get; set; } = @"C:\TrainerEditor\tests\party_test.h";

    }
}
