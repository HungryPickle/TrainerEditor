using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Trainer_Editor.UserControls;

namespace Trainer_Editor {

    public enum Input {
        HeldItem, Moves, IVs
    }

    public class PartyTypeRadioButtonConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            //return value?.Equals(parameter);
            return (PartyType)value == (PartyType)parameter;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            //return (bool)value ? parameter : Binding.DoNothing;
            return (bool)value ? (PartyType)parameter : Binding.DoNothing;
        }
    }
    public class PartyTypeEnableInputConverter : IMultiValueConverter {

        public static Dictionary<Input, bool> TrainerMonNoItemDefaultMoves = new Dictionary<Input, bool> { { Input.HeldItem, false }, { Input.Moves, false } };
        public static Dictionary<Input, bool> TrainerMonItemDefaultMoves = new Dictionary<Input, bool> { { Input.HeldItem, true }, { Input.Moves, false } };
        public static Dictionary<Input, bool> TrainerMonNoItemCustomMoves = new Dictionary<Input, bool> { { Input.HeldItem, false }, { Input.Moves, true } };
        public static Dictionary<Input, bool> TrainerMonItemCustomMoves = new Dictionary<Input, bool> { { Input.HeldItem, true }, { Input.Moves, true } };
        public static Dictionary<Input, bool> TrainerMonCustom = new Dictionary<Input, bool> { { Input.HeldItem, true }, { Input.Moves, true }, { Input.IVs, true } };
        
        public static Dictionary<PartyType, Dictionary<Input, bool>> IsInputEnabled = new Dictionary<PartyType, Dictionary<Input, bool>> {
            { PartyType.TrainerMonNoItemDefaultMoves, TrainerMonNoItemDefaultMoves },
            { PartyType.TrainerMonItemDefaultMoves, TrainerMonItemDefaultMoves },
            { PartyType.TrainerMonNoItemCustomMoves, TrainerMonNoItemCustomMoves },
            { PartyType.TrainerMonItemCustomMoves, TrainerMonItemCustomMoves },
            { PartyType.TrainerMonCustom, TrainerMonCustom }
        };

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {

            if (!(values[0] is PartyType && values[1] is Input))
                return false;

            PartyType partyType = (PartyType)values[0];
            Input input = (Input)values[1];

            return IsInputEnabled[partyType].GetValueOrDefault(input);

        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }

    public class DisabledInputOpacityConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return (bool)value ? 1.0 : 0.3;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }

    public class DisableEmptyPartySlotConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return value == null ? false : true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotSupportedException();
        }
    }
    public class HighlightSelectedMonConverter : IMultiValueConverter {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
            bool notNull = values[0] != null && values[1] != null;
            if (notNull && values[0] == values[1])
                return ((AutoCompleteTextBox)values[2])?.textbox.IsKeyboardFocused == true ? Brushes.LightGreen : Brushes.LightPink;
            else
                return Brushes.White;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }

    public class PokemonSpriteConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            string species = (string)value;

            if (species == "SPECIES_NONE") {
                return new BitmapImage(new Uri($"{Directory.GetCurrentDirectory()}\\Graphics\\ghost.png"));
            }
            try {
                Regex trim = new Regex(@"(?<=SPECIES_)\w+");
                species = trim.Match(species).Value.ToLower();
                Uri uri = new Uri($"C:\\Users\\Scott\\Decomps\\pokeemerald\\graphics\\pokemon\\{species}\\front.png");
                return new BitmapImage(uri);
            }
            catch (Exception ) {
                //MessageBox.Show(e.Message, "PokemonSpriteConverter", MessageBoxButton.OK, MessageBoxImage.Error);
                //return new BitmapImage(new Uri($"{Directory.GetCurrentDirectory()}\\Graphics\\ghost.png"));
                return Binding.DoNothing;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
    public class TrainerSpriteConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            string trainerPic = (string)value;

            if (trainerPic == null) {
                return new BitmapImage(new Uri($"{Directory.GetCurrentDirectory()}\\Graphics\\rocket.png"));
            }
            try {
                Regex trim = new Regex(@"(?<=TRAINER_PIC_)\w+");
                trainerPic = trim.Match(trainerPic).Value.ToLower();
                Uri uri = new Uri($"C:\\Users\\Scott\\Decomps\\pokeemerald\\graphics\\trainers\\front_pics\\{trainerPic}_front_pic.png");
                return new BitmapImage(uri);
            }
            catch (Exception) {
                return Binding.DoNothing;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
    public class RegexStringConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return ((Regex)value)?.ToString() ?? "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            try {
                return new Regex((string)value);
            }
            catch (Exception e) {
                MessageBox.Show($"Regex: {(string)value}\nError: {e.Message}", "Invalid Regex");
                return new Regex("");
            }
        }
    }
}
