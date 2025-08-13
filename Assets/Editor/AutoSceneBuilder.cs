#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public sealed class AutoSceneBuilder : IPreprocessBuildWithReport
{
    public int callbackOrder => 0;

    // Unity 2018.1+ signature
    public void OnPreprocessBuild(BuildReport report)
    {
        // Ensure our test scene is included in build settings if it exists
        var desired = new[] { "Assets/Scenes/HideSeek_Test.unity" };

        var existing = EditorBuildSettings.scenes?.Select(s => s.path).ToList() ?? new System.Collections.Generic.List<string>();
        var updated = existing.ToList();

        foreach (var path in desired)
        {
            if (System.IO.File.Exists(path) && !existing.Contains(path))
            {
                updated.Add(path);
                Debug.Log($"AutoSceneBuilder: Added to Build Settings: {path}");
            }
        }

        if (!updated.SequenceEqual(existing))
        {
            EditorBuildSettings.scenes = updated.Select(p => new EditorBuildSettingsScene(p, true)).ToArray();
            AssetDatabase.SaveAssets();
        }
    }
}
#endif
