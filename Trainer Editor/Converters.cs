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
        HeldItem, Moves, Custom
    }

    public class PartyTypeRadioButtonConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            //return value?.Equals(parameter);
            return (PartyTypes)value == (PartyTypes)parameter;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            //return (bool)value ? parameter : Binding.DoNothing;
            return (bool)value ? (PartyTypes)parameter : Binding.DoNothing;
        }
    }
    public class PartyTypeEnableInputConverter : IMultiValueConverter {

        public static Dictionary<Input, bool> TrainerMonNoItemDefaultMoves = new Dictionary<Input, bool> { };
        public static Dictionary<Input, bool> TrainerMonItemDefaultMoves = new Dictionary<Input, bool> { { Input.HeldItem, true } };
        public static Dictionary<Input, bool> TrainerMonNoItemCustomMoves = new Dictionary<Input, bool> { { Input.Moves, true } };
        public static Dictionary<Input, bool> TrainerMonItemCustomMoves = new Dictionary<Input, bool> { { Input.HeldItem, true }, { Input.Moves, true } };
        public static Dictionary<Input, bool> TrainerMonCustom = new Dictionary<Input, bool> { { Input.HeldItem, true }, { Input.Moves, true }, { Input.Custom, true } };
        
        public static Dictionary<PartyTypes, Dictionary<Input, bool>> IsInputEnabled = new Dictionary<PartyTypes, Dictionary<Input, bool>> {
            { PartyTypes.TrainerMonNoItemDefaultMoves, TrainerMonNoItemDefaultMoves },
            { PartyTypes.TrainerMonItemDefaultMoves, TrainerMonItemDefaultMoves },
            { PartyTypes.TrainerMonNoItemCustomMoves, TrainerMonNoItemCustomMoves },
            { PartyTypes.TrainerMonItemCustomMoves, TrainerMonItemCustomMoves },
            { PartyTypes.TrainerMonCustom, TrainerMonCustom }
        };

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {

            if (!(values[0] is PartyTypes && values[1] is Input))
                return false;

            PartyTypes partyType = (PartyTypes)values[0];
            Input input = (Input)values[1];

            return IsInputEnabled[partyType].GetValueOrDefault(input);

        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }

    public class DisabledInputOpacityConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return (bool)value ? 1.0 : 0.1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }

    public class DisableControlWithNullBindingConverter : IValueConverter {
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
                return new CroppedBitmap(new BitmapImage(uri), new Int32Rect(0,0,64,64));
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
    public class AiFlagsConverter : IMultiValueConverter {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
            string flag = values[0] as string; 
            List<string> aiFlags = values[1] as List<string>;
            if (flag == null || aiFlags == null)
                return Binding.DoNothing;
            return aiFlags.Contains(flag) ? true : false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {

            return new object[] { Binding.DoNothing, Binding.DoNothing };
        }
    }
}
