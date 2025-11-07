using UnityEngine;

public class Settings : MonoBehaviour
{
    Rect settingsWindowRect = new Rect(50, 50, 300, Screen.height * 0.8f);
    
    Switch proximityModeSwitch;
    Switch darkLightModeSwitch;
    Window window;

    public Vector2 settingsScrollPosition = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        proximityModeSwitch = new Switch(new Rect(0, 30, 40, 20), UiSkinManger.sUiStyle.GetStyle("switchBackground"), UiSkinManger.sUiStyle.GetStyle("switchKnob"));
        darkLightModeSwitch = new Switch(new Rect(0, 60, 40, 20), UiSkinManger.sUiStyle.GetStyle("switchBackground"), UiSkinManger.sUiStyle.GetStyle("switchKnob"));
        window = new Window("Settings", UiSkinManger.sUiStyle, 300, Mathf.RoundToInt(Screen.height * 0.8f));

    }

    // Update is called once per frame
    void Update()
    {
    }


    private void OnGUI()
    {
        GUI.skin = UiSkinManger.sUiStyle;
        if (GlobalSettings.showSettings)
        {
            settingsWindowRect = GUI.Window(6, settingsWindowRect, SettingsWindow, "");
        }
    }

    void SettingsWindow(int windowID)
    {
        window.render();
        settingsScrollPosition = GUI.BeginScrollView(new Rect(0, 30, 300, Screen.height * 0.8f - 30), settingsScrollPosition, new Rect(0, 0, 280, Screen.height * 0.8f));

        GUI.BeginGroup(new Rect(10, 10, 300, 400));
        GUI.Label(new Rect(0, 0, 200, 20), "Rendering Behaviour", UiSkinManger.sUiStyle.GetStyle("headline"));
        Main.proximityMode = proximityModeSwitch.render(Main.proximityMode);
        GUI.Label(new Rect(50, 30, 120, 40), "Proximity Mode");
        Main.darkLightMode = darkLightModeSwitch.render(Main.darkLightMode);
        GUI.Label(new Rect(50, 60, 120, 40), "Dark/Light Mode");
        GUI.EndGroup();

        GUI.EndScrollView();
    }
}
