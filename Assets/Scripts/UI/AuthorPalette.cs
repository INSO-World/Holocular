using System.Collections;
using System.Collections.Generic;
using System.IO;
using SimpleFileBrowser;
using UnityEditor;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.MessageBox;

public class AuthorPalette : MonoBehaviour
{
    Rect authorPaletteWindowRect = new Rect(330, 10, 200, 800);
    public Vector2 authorsScrollPosition = Vector2.zero;

    Dictionary<string, Texture2D> authorBackgroundTextures = new Dictionary<string, Texture2D>();//key: signature
    bool authorBackgroundTexturesInitialized = false;
    Window window;

    // Start is called before the first frame update
    void Start()
    {
        window = new Window("Author Palette", UiSkinManger.sUiStyle, 200, 800);
    }

    // Update is called once per frame
    void Update()
    {
        if (!authorBackgroundTexturesInitialized && Main.helix.stakeholders.Count > 0)
        {
            foreach (KeyValuePair<string, HelixStakeholder> stakeholder in Main.helix.stakeholders)
            {
                authorBackgroundTextures[stakeholder.Key] = new Texture2D(1, 1);
                authorBackgroundTextures[stakeholder.Key].SetPixel(1, 1, stakeholder.Value.colorStore);
                authorBackgroundTextures[stakeholder.Key].Apply();
            }
            authorBackgroundTexturesInitialized = true;
        }
    }


    private void OnGUI()
    {
        GUI.skin = UiSkinManger.sUiStyle;

        if (GlobalSettings.showAuthorPalette)
        {
            authorPaletteWindowRect = GUI.Window(1, authorPaletteWindowRect, AuthorPaletteWindow, "");

        }


    }

    void AuthorPaletteWindow(int windowID)
    {
        window.render();
        authorsScrollPosition = GUI.BeginScrollView(new Rect(0, 40, 200, 760), authorsScrollPosition, new Rect(0, 0, 180, 20 * Main.helix.stakeholders.Count));
        int i = 0;
        foreach (KeyValuePair<string, HelixStakeholder> stakeholder in Main.helix.stakeholders)
        {
            if (GlobalSettings.highlightedAuthor == null || GlobalSettings.highlightedAuthor == stakeholder.Value.dBStakeholderStore.gitSignature)
            {
                GUI.DrawTexture(new Rect(0, 20 * i, 200, 20), authorBackgroundTextures[stakeholder.Key]);
            }
            if (GUI.Button(new Rect(10, 20 * i, 800, 20), stakeholder.Value.dBStakeholderStore.gitSignature, UiSkinManger.sUiStyle.label))
            {
                GlobalSettings.SelectAuthor(stakeholder.Value.dBStakeholderStore.gitSignature);
            }

            i++;
        }

        GUI.EndScrollView();

    }
}
