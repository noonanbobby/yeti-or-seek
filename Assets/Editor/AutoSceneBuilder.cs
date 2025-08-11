#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using System.IO;

public class AutoSceneBuilder : IPreprocessBuildWithReport
{
    public int callbackOrder => 0;

    public void PreprocessBuild(BuildReport report)
    {
        const string dir = "Assets/Auto";
        const string scenePath = dir + "/AutoScene.unity";
        if (!AssetDatabase.IsValidFolder(dir)) AssetDatabase.CreateFolder("Assets", "Auto");

        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        // EventSystem
        var es = new GameObject("EventSystem");
        es.AddComponent<EventSystem>();
        es.AddComponent<StandaloneInputModule>();

        // Bootstrap
        var boot = new GameObject("GameBootstrap");
        boot.AddComponent<GameBootstrap>();

        // Camera (will be re-parented by GameBootstrap)
        var cam = new GameObject("MainCamera");
        var camera = cam.AddComponent<Camera>();
        camera.clearFlags = CameraClearFlags.Skybox;
        cam.tag = "MainCamera";

        EditorSceneManager.SaveScene(scene, scenePath);
        AssetDatabase.SaveAssets();

        EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(scenePath, true) };
        Debug.Log("[AutoSceneBuilder] AutoScene created and added to Build Settings: " + scenePath);
    }
}
#endif
