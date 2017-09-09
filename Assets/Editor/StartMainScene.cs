using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class StartMainScene : EditorWindow
{

    [MenuItem("Play/PlayMe _%h")]
    public static void RunMainScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Main.unity");
        EditorApplication.isPlaying = true;
    }
}
