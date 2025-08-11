public static class CurrencyManager
{
    private const string KEY = "flakes";
    public static int Flakes { get; private set; }

    public static void Load() { Flakes = SaveSystem.LoadInt(KEY, 0); }
    public static void AddFlakes(int a) { Flakes += a; }
    public static bool Spend(int a) { if (Flakes < a) return false; Flakes -= a; return true; }
    public static void Save() { SaveSystem.SaveInt(KEY, Flakes); SaveSystem.Flush(); }
}
