using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DebugView : MonoBehaviour
{
    public GUIStyle logOutputStyle;
    int logOutputLines = 10;

    public Vector2 logScrollPosition = Vector2.zero;

    int debugWindowWidth = 400;
    int margin = 10;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnGUI()
    {
        GUI.skin = UiSkinManger.sUiStyle;

        if (GlobalSettings.debugMode)
        {
            GUI.BeginGroup(new Rect(Screen.width - debugWindowWidth, 0, debugWindowWidth, Screen.height), UiSkinManger.sUiStyle.GetStyle("debugBackground"));
            GUI.BeginGroup(new Rect(margin, margin, debugWindowWidth - 2 * margin, Screen.height));
            GUILayout.BeginVertical();
            GUILayout.Label("General:",UiSkinManger.sUiStyle.GetStyle("headline"));
            GUILayout.Label("Move Speed (Mouse wheel): " + Main.moveSpeed + "m/s");
            GUILayout.Label("Mouse Sensitivity (alt + Mouse wheel): " + Main.mouseSensitivity);
            GUILayout.Label("FPS: " + ((int)(1f / Time.unscaledDeltaTime)));
            GUILayout.Label("View Distance (V + Mouse wheel): " + Main.viewDistance + "m");

            GUILayout.Label("Threads:",UiSkinManger.sUiStyle.GetStyle("headline"));
            GUILayout.Label("Create Structure Thread: " + Main.helix.createStructureThreadState);
            GUILayout.Label("Draw Structure Thread: " + Main.helix.drawStructureThreadState);
            GUILayout.Label("Action Queue Size: " + Main.actionQueue.Count);

            GUILayout.Label("Selections:",UiSkinManger.sUiStyle.GetStyle("headline"));
            GUILayout.Label("Highlighted Author: " + GlobalSettings.highlightedAuthor);
            GUILayout.Label("Highlight Mode: " + (GlobalSettings.showAuthorColors ? "committer" : GlobalSettings.showBranchColors ? "branch" : GlobalSettings.showOwnershipColors ? "ownership" : "none"));
            GUILayout.Label("Last Selected Object: " + (Main.lastSelectedObject == null ? "none" : Main.lastSelectedObject.name));
            GUILayout.EndVertical();
            GUI.EndGroup();
            LogScrollView();
            GUI.EndGroup();
        }
        else
        {
            GUI.Label(new Rect(Screen.width - 200, 0, 200, 20), "F3 for debug info.");
        }
    }


    private void LogScrollView()
    {
        List<Log> logs = RuntimeDebug.getLogs();
        logScrollPosition = GUI.BeginScrollView(new Rect(margin, Screen.height - 20 * logOutputLines, debugWindowWidth - margin, 20 * logOutputLines), logScrollPosition, new Rect(0, 0, debugWindowWidth - margin - 20, 20 * logs.Count));

        for (int i = 0; i < logs.Count; i++)
        {
            GUI.Label(new Rect(0, 20 * (logs.Count - i), debugWindowWidth * 2, 20), "[" + logs[i].time.ToLongTimeString() + "]: " + logs[i].message);
        }
        GUI.EndScrollView();
    }
}
