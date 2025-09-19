using SimpleFileBrowser;
using UnityEngine;

public class Parameters : MonoBehaviour
{
    public GUISkin uiStyle;
    Rect parametersWindowRect = new Rect(10, 10, 300, Screen.height * 0.8f);

    Switch showAuthorsColorsSwitch;
    Switch showBranchColorsSwitch;
    Switch showOwnershipColorsSwitch;
    Switch showHotspotColorsSwitch;
    Switch commitPlacementModeSwitch;
    Switch showFolderRingsSwitch;
    Window window;

    public string commitDistanceMultiplicatorTmp;
    public Vector2 parametersScrollPosition = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        showAuthorsColorsSwitch = new Switch(new Rect(0, 30, 40, 20), uiStyle.GetStyle("switchBackground"), uiStyle.GetStyle("switchKnob"));
        showBranchColorsSwitch = new Switch(new Rect(0, 70, 40, 20), uiStyle.GetStyle("switchBackground"), uiStyle.GetStyle("switchKnob"));
        showOwnershipColorsSwitch = new Switch(new Rect(0, 110, 40, 20), uiStyle.GetStyle("switchBackground"), uiStyle.GetStyle("switchKnob"));
        showHotspotColorsSwitch = new Switch(new Rect(0, 150, 40, 20), uiStyle.GetStyle("switchBackground"), uiStyle.GetStyle("switchKnob"));
        commitPlacementModeSwitch = new Switch(new Rect(0, 70, 40, 20), uiStyle.GetStyle("switchBackground"), uiStyle.GetStyle("switchKnob"));
        showFolderRingsSwitch = new Switch(new Rect(0, 110, 40, 20), uiStyle.GetStyle("switchBackground"), uiStyle.GetStyle("switchKnob"));
        window = new Window("Parameters", uiStyle, 300, Mathf.RoundToInt(Screen.height * 0.8f));

        commitDistanceMultiplicatorTmp = "" + GlobalSettings.commitDistanceMultiplicator;
    }

    // Update is called once per frame
    void Update()
    {
    }


    private void OnGUI()
    {
        GUI.skin = uiStyle;
        if (GlobalSettings.showParameters)
        {
            parametersWindowRect = GUI.Window(0, parametersWindowRect, ParametersWindow, "");
        }
    }

    void ParametersWindow(int windowID)
    {
        window.render();
        parametersScrollPosition = GUI.BeginScrollView(new Rect(0, 30, 300, Screen.height * 0.8f - 30), parametersScrollPosition, new Rect(0, 0, 280, 900));

        if (GUI.Button(new Rect(10, 10, 250, 20), "Open Database"))
        {
            OpenFolder();
        }

        if (GUI.Button(new Rect(10, 35, 250, 20), "Delete Helix"))
        {
            Main.helix.DeleteHelix();
        }

        if (GUI.Button(new Rect(10, 60, 250, 20), "Stop Helix Generation"))
        {
            Main.helix.StopThreads();
        }

        GUI.BeginGroup(new Rect(10, 120, 300, 300));
        GUI.Label(new Rect(0, 0, 300, 20), "Stats", uiStyle.GetStyle("headline"));
        GUI.Label(new Rect(0, 30, 300, 20), "Commits: " + Statistics.commitsDrawn + "/" + (Main.commits == null ? "0" : Main.commits.commits.Length));
        GUI.Label(new Rect(0, 50, 300, 20), "Branches: " + (Main.branches == null ? "0" : Main.branches.branches.Length));
        GUI.Label(new Rect(0, 70, 300, 20), "Files: " + (Main.files == null ? "0" : Main.files.files.Length));
        GUI.Label(new Rect(0, 90, 300, 20), "Stakeholders: " + (Main.stakeholders == null ? "0" : Main.stakeholders.stakeholders.Length));
        GUI.Label(new Rect(0, 110, 300, 20), "Commits-Files: " + (Main.commitsFiles == null ? "0" : Main.commitsFiles.commitsFiles.Length));
        GUI.Label(new Rect(0, 130, 300, 20), "Commits-Commits: " + (Main.commitsCommits == null ? "0" : Main.commitsCommits.commitsCommits.Length));
        GUI.Label(new Rect(0, 150, 300, 20), "Commits-Stakeholders: " + (Main.commitsStakeholders == null ? "0" : Main.commitsStakeholders.commitsStakeholders.Length));
        GUI.Label(new Rect(0, 170, 300, 20), "Commits-Files-Stakeholders: " + (Main.commitsFilesStakeholders == null ? "0" : Main.commitsFilesStakeholders.commitsFilesStakeholders.Length));
        GUI.EndGroup();

        GUI.BeginGroup(new Rect(10, 340, 300, 400));
        GUI.Label(new Rect(0, 0, 200, 20), "Highlighting", uiStyle.GetStyle("headline"));
        GlobalSettings.showAuthorColors = showAuthorsColorsSwitch.render(GlobalSettings.showAuthorColors);
        GUI.Label(new Rect(50, 30, 120, 40), "Show Committer\nColors (c)");

        GlobalSettings.showBranchColors = showBranchColorsSwitch.render(GlobalSettings.showBranchColors);
        GUI.Label(new Rect(50, 70, 120, 40), "Show Branch\nColors (b)");

        GlobalSettings.showOwnershipColors = showOwnershipColorsSwitch.render(GlobalSettings.showOwnershipColors);
        GUI.Label(new Rect(50, 110, 120, 40), "Show Ownership\nColors (o)");

        GlobalSettings.showHotspotColors = showHotspotColorsSwitch.render(GlobalSettings.showHotspotColors);
        GUI.Label(new Rect(50, 150, 120, 40), "Show Hotspot\nColors (h)");

        GUI.Label(new Rect(0, 190, 300, 40), "Hotspot Threshold [" + GlobalSettings.hotspotThreshold + "/" + Main.helix.maxChanges + "]:\n(alt+h+scroll to fine adjust)");
        GlobalSettings.hotspotThreshold = Slider.render(new Rect(0, 230, 180, 20), GlobalSettings.hotspotThreshold, 0, Main.helix.maxChanges, uiStyle.GetStyle("sliderEmpty"), uiStyle.GetStyle("sliderFilled"), uiStyle.GetStyle("sliderKnob"));


        GUI.Label(new Rect(0, 250, 200, 20), "Highlight Path:");
        GlobalSettings.folderSearch = TextField.render(new Rect(0, 270, 200, 20), GlobalSettings.folderSearch, uiStyle.GetStyle("textFieldText"), uiStyle.GetStyle("textFieldBack"), uiStyle.GetStyle("textFieldBorder"));

        GUI.EndGroup();


        GUI.BeginGroup(new Rect(10, 660, 200, 200));
        GUI.Label(new Rect(0, 0, 200, 20), "Visuals", uiStyle.GetStyle("headline"));

        GUI.Label(new Rect(0, 30, 200, 20), "Distance Factor:");
        GlobalSettings.commitDistanceMultiplicator = Slider.render(new Rect(0, 50, 180, 20), GlobalSettings.commitDistanceMultiplicator, 1f, 9f, uiStyle.GetStyle("sliderEmpty"), uiStyle.GetStyle("sliderFilled"), uiStyle.GetStyle("sliderKnob"));

        GlobalSettings.commitPlacementMode = commitPlacementModeSwitch.render(GlobalSettings.commitPlacementMode);
        GUI.Label(new Rect(50, 70, 160, 40), "Commit Distribution\nLinar/By Date");

        GlobalSettings.showFolderRings = showFolderRingsSwitch.render(GlobalSettings.showFolderRings);
        GUI.Label(new Rect(50, 110, 120, 40), "Show Folder\nRings (f)");

        GUI.Label(new Rect(0, 150, 200, 20), "File Size:");
        GlobalSettings.fileSize = Slider.render(new Rect(0, 170, 180, 20), GlobalSettings.fileSize, 0.1f, 15f, uiStyle.GetStyle("sliderEmpty"), uiStyle.GetStyle("sliderFilled"), uiStyle.GetStyle("sliderKnob"));


        GUI.EndGroup();

        GUI.EndScrollView();
    }

    private void OpenFolder()
    {
        FileBrowser.ShowLoadDialog((paths) =>
        {
            RuntimeDebug.Log("Trying to Open: " + paths[0]);
            if (DatabaseLoader.checkFoolderIfValid(paths[0]))
            {
                RuntimeDebug.Log("Path valid");
                if (DatabaseLoader.importDatabase(paths[0]))
                {
                    RuntimeDebug.Log("Database imported Successfull");
                    Main.helix = new Helix(GameObject.Find("Helix"));
                    Main.helix.GenerateHelix();
                }
            }
            else
            {
                RuntimeDebug.Log("Path Invalid. Selected Folder not a Binocular DB");
            }
        }, () => { Debug.Log("Canceled"); }, FileBrowser.PickMode.Folders, false, null, null, "Select Folder", "Select");
    }

}
