using TaleWorlds.Library;
namespace UFO.Extension
{
    public static class Message
    {
        public static void Show(string text, Color? color = null)
        {
            Color color2 = color ?? Color.White;
            InformationManager.DisplayMessage(new InformationMessage(text, color2));
        }
    }
}