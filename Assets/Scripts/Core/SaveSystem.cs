using UnityEngine;

public static class SaveSystem
{
    public static void SaveString(string key, string value) => PlayerPrefs.SetString(key, value);
    public static string LoadString(string key, string def = "") => PlayerPrefs.GetString(key, def);
    public static void SaveInt(string key, int value) => PlayerPrefs.SetInt(key, value);
    public static int LoadInt(string key, int def = 0) => PlayerPrefs.GetInt(key, def);
    public static void Flush() => PlayerPrefs.Save();
}
