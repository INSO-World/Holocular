using System.Collections;
using System.Collections.Generic;
using System.IO;
using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using SimpleFileBrowser;
using UnityEditor;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.MessageBox;

public class BranchPalette : MonoBehaviour
{
    public GUISkin uiStyle;
    Rect branchPaletteWindowRect = new Rect(550, 10, 200, 800);
    public Vector2 branchesScrollPosition = Vector2.zero;

    Dictionary<string, Texture2D> branchBackgroundTextures = new Dictionary<string, Texture2D>();//key: signature
    bool branchBackgroundTexturesInitialized = false;
    Window window;

    // Start is called before the first frame update
    void Start()
    {
        window = new Window("Branch Palette", uiStyle, 200, 800);
    }

    // Update is called once per frame
    void Update()
    {
        if (!branchBackgroundTexturesInitialized && Main.helix.branches.Count > 0)
        {
            Color[] palette = ColorPalette.Generate(Main.helix.branches.Count);
            int i = 0;
            foreach (KeyValuePair<string, HelixBranch> branch in Main.helix.branches)
            {
                branch.Value.colorStore = palette[i];
                branchBackgroundTextures[branch.Value.dBBranchStore.branch] = new Texture2D(1, 1);
                branchBackgroundTextures[branch.Value.dBBranchStore.branch].SetPixel(1, 1, palette[i]);
                branchBackgroundTextures[branch.Value.dBBranchStore.branch].Apply();
                i++;
            }
            branchBackgroundTexturesInitialized = true;
        }
    }


    private void OnGUI()
    {
        GUI.skin = uiStyle;

        if (GlobalSettings.showBranchPalette)
        {
            branchPaletteWindowRect = GUI.Window(4, branchPaletteWindowRect, BranchPaletteWindow, "");

        }


    }

    void BranchPaletteWindow(int windowID)
    {
        window.render();
        branchesScrollPosition = GUI.BeginScrollView(new Rect(0, 40, 200, 760), branchesScrollPosition, new Rect(0, 0, 180, 20 * Main.branches.branches.Length));
        int i = 0;
        foreach (KeyValuePair<string, HelixBranch> branch in Main.helix.branches)
        {
            if (GlobalSettings.highlightedBranch == null || GlobalSettings.highlightedBranch == branch.Value.dBBranchStore.branch)
            {
                GUI.DrawTexture(new Rect(0, 20 * i, 200, 20), branchBackgroundTextures[branch.Value.dBBranchStore.branch]);
            }
            if (GUI.Button(new Rect(10, 20 * i, 800, 20), branch.Value.dBBranchStore.branch, uiStyle.label))
            {
                if (GlobalSettings.highlightedBranch == branch.Value.dBBranchStore.branch)
                {
                    GlobalSettings.highlightedBranch = null;
                }
                else
                {
                    GlobalSettings.highlightedBranch = branch.Value.dBBranchStore.branch;
                }
            }

            i++;
        }

        GUI.EndScrollView();

    }
}
