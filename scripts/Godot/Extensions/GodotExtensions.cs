using System.Text;
using Godot;

namespace TurnBattle.Godot.Extensions
{
    public static class GodotExtensions
    {
        public static string ReadAllText(this File file, string path)
        {
            file.Open(path, File.ModeFlags.Read);
            var stringBuilder = new StringBuilder();
            while (!file.EofReached())
            {
                var line = file.GetLine();
                stringBuilder.AppendLine(line);
            }
            file.Close();
            return stringBuilder.ToString();
        }
    }
}