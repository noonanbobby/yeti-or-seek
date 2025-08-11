using System.Collections.Generic;

public static class InventoryManager
{
    public static HashSet<string> Owned = new();
    public static string EquippedSkin = "default";

    public static void Load()
    {
        // Simple stub — extend to JSON later
        EquippedSkin = SaveSystem.LoadString("equippedSkin", "default");
    }

    public static void Save()
    {
        SaveSystem.SaveString("equippedSkin", EquippedSkin);
        SaveSystem.Flush();
    }
}
