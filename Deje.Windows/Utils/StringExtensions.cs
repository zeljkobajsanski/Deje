namespace Deje.Windows.Utils
{
    public static class StringExtensions
    {
        public static string ConvertFromCyrilic(this string stringToConvert)
        {
            stringToConvert = stringToConvert.Replace("а", "a");
            stringToConvert = stringToConvert.Replace("А", "A");
            stringToConvert = stringToConvert.Replace("б", "b");
            stringToConvert = stringToConvert.Replace("Б", "B");
            stringToConvert = stringToConvert.Replace("в", "v");
            stringToConvert = stringToConvert.Replace("В", "V");
            stringToConvert = stringToConvert.Replace("г", "g");
            stringToConvert = stringToConvert.Replace("Г", "G");
            stringToConvert = stringToConvert.Replace("д", "d");
            stringToConvert = stringToConvert.Replace("Д", "D");
            stringToConvert = stringToConvert.Replace("ђ", "đ");
            stringToConvert = stringToConvert.Replace("Ђ", "Đ");
            stringToConvert = stringToConvert.Replace("Џ", "Dž");
            stringToConvert = stringToConvert.Replace("џ", "dž");
            stringToConvert = stringToConvert.Replace("е", "e");
            stringToConvert = stringToConvert.Replace("Е", "E");
            stringToConvert = stringToConvert.Replace("Ж", "Ž");
            stringToConvert = stringToConvert.Replace("ж", "ž");
            stringToConvert = stringToConvert.Replace("З", "Z");
            stringToConvert = stringToConvert.Replace("з", "z");
            stringToConvert = stringToConvert.Replace("И", "I");
            stringToConvert = stringToConvert.Replace("и", "i");
            stringToConvert = stringToConvert.Replace("Ј", "J");
            stringToConvert = stringToConvert.Replace("ј", "j");
            stringToConvert = stringToConvert.Replace("К", "K");
            stringToConvert = stringToConvert.Replace("к", "k");
            stringToConvert = stringToConvert.Replace("Л", "L");
            stringToConvert = stringToConvert.Replace("л", "l");
            stringToConvert = stringToConvert.Replace("Љ", "Lj");
            stringToConvert = stringToConvert.Replace("љ", "lj");
            stringToConvert = stringToConvert.Replace("М", "M");
            stringToConvert = stringToConvert.Replace("м", "m");
            stringToConvert = stringToConvert.Replace("Н", "N");
            stringToConvert = stringToConvert.Replace("н", "n");
            stringToConvert = stringToConvert.Replace("Њ", "Nj");
            stringToConvert = stringToConvert.Replace("њ", "nj");
            stringToConvert = stringToConvert.Replace("О", "O");
            stringToConvert = stringToConvert.Replace("о", "o");
            stringToConvert = stringToConvert.Replace("П", "P");
            stringToConvert = stringToConvert.Replace("п", "p");
            stringToConvert = stringToConvert.Replace("Р", "R");
            stringToConvert = stringToConvert.Replace("р", "r");
            stringToConvert = stringToConvert.Replace("С", "S");
            stringToConvert = stringToConvert.Replace("с", "s");
            stringToConvert = stringToConvert.Replace("Т", "T");
            stringToConvert = stringToConvert.Replace("т", "t");
            stringToConvert = stringToConvert.Replace("Ћ", "Ć");
            stringToConvert = stringToConvert.Replace("ћ", "ć");
            stringToConvert = stringToConvert.Replace("У", "U");
            stringToConvert = stringToConvert.Replace("у", "u");
            stringToConvert = stringToConvert.Replace("Ф", "F");
            stringToConvert = stringToConvert.Replace("ф", "f");
            stringToConvert = stringToConvert.Replace("Х", "H");
            stringToConvert = stringToConvert.Replace("х", "h");
            stringToConvert = stringToConvert.Replace("Ц", "C");
            stringToConvert = stringToConvert.Replace("ц", "c");
            stringToConvert = stringToConvert.Replace("Ч", "Č");
            stringToConvert = stringToConvert.Replace("ч", "č");
            stringToConvert = stringToConvert.Replace("Ђ", "Đ");
            stringToConvert = stringToConvert.Replace("ђ", "đ");
            stringToConvert = stringToConvert.Replace("Ш", "Š");
            stringToConvert = stringToConvert.Replace("ш", "š");
            return stringToConvert;
        }
    }
}