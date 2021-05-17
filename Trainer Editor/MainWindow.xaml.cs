using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.IO;

namespace Trainer_Editor {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();

            List<Trainer> trainers = new List<Trainer>();

            char cursor;
            string monStruct = "";
            StringBuilder sb = new StringBuilder();
            int open = 0;
            int closed = 0;

            string path = @"C:\Users\Scott\Desktop\trainer_parties.h";
            //FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            try {
                using (StreamReader sr = new StreamReader(path)) {
                    string line = "a";

                    while ((line = sr.ReadLine()) != null) {

                        if (MonRegex.IfContainsTrainerAddTrainer(line, ref trainers)) {

                            while (sr.Peek() != ';') {

                                cursor = (char)sr.Read();

                                if (cursor == '{')
                                    open += 1;
                                else if (cursor == '}')
                                    closed += 1;

                                sb.Append(cursor);

                                if (open == closed && open > 0) {
                                    monStruct = sb.ToString();
                                    trainers.Last().AddMonFromStruct(monStruct);
                                    sb.Clear();
                                    open = 0;
                                    closed = 0;
                                }
                            }
                            open = 0;
                            closed = 0;
                        }

                    }
                    //sr.Dispose();
                    //sr.Close();
                }
            }
            catch (Exception e) {
                Debug.WriteLine(e.Message);
            }

            //fs = new FileStream(@"C:\Users\Scott\Desktop\party_test.h", FileMode.OpenOrCreate, FileAccess.Write);
            path = @"C:\Users\Scott\Desktop\party_test.h";

            try {
                using (StreamWriter sw = new StreamWriter(path)) {

                    foreach (Trainer trainer in trainers) {
                        sw.Write($"{trainer.CreatePartyStruct()}");
                        sw.Write("\n\n");
                    }
                    //sw.Dispose();
                    //sw.Close();

                }

            }
            catch (Exception e) {
                Debug.WriteLine(e.Message);
            }
            //foreach (var trainer in Trainers) {
            //    //if(trainer.Party.Count > 0)
            //    Console.WriteLine("  Num: {0}   Name: {1}", Trainers.IndexOf(trainer), trainer.TrainerPartyName);
            //    trainer.PrintPartySpecies();
            //}
            Debug.WriteLine("Files Saved.");

        }
    }
}
