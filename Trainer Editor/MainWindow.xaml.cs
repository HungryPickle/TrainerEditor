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
using System.Threading;

namespace Trainer_Editor {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();

            //Trainer_parties.ReCreate();

            //List<Trainer> trainers = FileManager.Trainers.Read();
            //List<Party> parties = FileManager.TrainerParties.Read();
            //
            //for (int i = 0; i < parties.Count(); i++) {
            //    trainers[i + 1].party = parties[i];
            //}
            //
            ////for(int i = 0; i < parties.Count(); i ++) {
            ////    trainers[i+1].party = parties.Find(p => p.TrainerPartyName.Equals(trainers[i+1].partySize));
            ////}
            //
            //Debug.WriteLine("test");

        }

        private async void Button_Click(object sender, RoutedEventArgs e) {

            List<Trainer> trainers = await Test.ReadTrainers();
            Debug.WriteLine("Trainers Read.");

            List<Party> parties = await Test.ReadParties();
            Debug.WriteLine("Parties Read.");

            trainers = await Test.StitchLists(trainers, parties);
            Debug.WriteLine($"Stitch Complete. {trainers[0].PartySize}");

            await Test.WriteTrainers(trainers);
            Debug.WriteLine("Trainers Saved.");

            await Test.WriteParties(trainers);
            Debug.WriteLine("Parties Saved.");

        }
    }

    public class Test {
        public static Task<List<Trainer>> ReadTrainers() {

            return Task.Factory.StartNew(() =>
            {
                return FileManager.ReadTrainers();
            });
        }
        public static Task<List<Party>> ReadParties() {

            return Task.Factory.StartNew(() =>
            {
                return FileManager.ReadParties();
            });
        }
        public static Task WriteTrainers(List<Trainer> trainers) {
            return Task.Factory.StartNew(() =>
            {
                FileManager.WriteTrainers(trainers);
            });
        }
        public static Task WriteParties(List<Trainer> trainers) {
            return Task.Factory.StartNew(() =>
            {
                FileManager.WriteParties(trainers);
            });
        }
        public static Task<List<Trainer>> StitchLists(List<Trainer> trainers, List<Party> parties ) {

            return Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < parties.Count(); i++) {
                    trainers[i + 1].Party = parties[i];
                }
                return trainers;
            });
        }

    }
}
