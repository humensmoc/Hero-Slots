#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class DevToolUtilityWindow : EditorWindow
{
    private const string WindowTitle = "DevTool Utility";

    [MenuItem("Tools/DevTool/小工具面板")]
    public static void ShowWindow()
    {
        DevToolUtilityWindow window = GetWindow<DevToolUtilityWindow>();
        window.titleContent = new GUIContent(WindowTitle);
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Space(12f);
        EditorGUILayout.LabelField("DevTool 自定义工具", EditorStyles.boldLabel);
        GUILayout.Space(8f);

        if (GUILayout.Button("测试胜利", GUILayout.Height(36f)))
        {
            UISystem.Instance.gameOverPanelView.gameObject.SetActive(true);
        }
        GUILayout.Space(8f);
        if (GUILayout.Button("测试失败", GUILayout.Height(36f)))
        {
            UISystem.Instance.gameOverPanelView.gameObject.SetActive(true);
        }
    }
}
#endif
