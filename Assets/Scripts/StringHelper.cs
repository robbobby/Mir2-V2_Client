using System.Text.RegularExpressions;
namespace DefaultNamespace {
    public static class StringHelper {
        public static string PascalToSentence(string str) {
            return Regex.Replace(str, "[a-z][A-Z]", m => $"{m.Value[0]} {char.ToLower(m.Value[1])}");
        }
    }
}
