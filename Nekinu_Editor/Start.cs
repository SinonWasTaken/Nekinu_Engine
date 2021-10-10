using Nekinu.Editor;
using Nekinu.StartUp;

public class Start
{
    public static void Main(string[] args)
    {
        StartEngine.Start_Engine<EditorWindowUpdater>("Editor");
    }
}