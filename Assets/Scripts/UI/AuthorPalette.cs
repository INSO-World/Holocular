using System.Collections;
using System.Collections.Generic;
using System.IO;
using SimpleFileBrowser;
using UnityEditor;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.MessageBox;

public class AuthorPalette : MonoBehaviour
{
    public GUISkin uiStyle;
    Rect authorPaletteWindowRect = new Rect(40, 40, 200, Screen.height / 2);
    public Vector2 authorsScrollPosition = Vector2.zero;

    Dictionary<string, Texture2D> authorBackgroundTextures = new Dictionary<string, Texture2D>();//key: signature
    bool authorBackgroundTexturesInitialized = false;
    WindowBar windowBar;

    // Start is called before the first frame update
    void Start()
    {
        windowBar = new WindowBar("Author Palette", uiStyle);
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
        GUI.skin = uiStyle;

        if (GlobalSettings.showAuthorPalette)
        {
            authorPaletteWindowRect = GUI.Window(1, authorPaletteWindowRect, AuthorPaletteWindow, "");

        }


    }

    void AuthorPaletteWindow(int windowID)
    {
        windowBar.render();
        authorsScrollPosition = GUI.BeginScrollView(new Rect(0, 30, 200, Screen.height / 2 - 30), authorsScrollPosition, new Rect(0, 0, 180, 20 * Main.helix.stakeholders.Count));
        int i = 0;
        foreach (KeyValuePair<string, HelixStakeholder> stakeholder in Main.helix.stakeholders)
        {
            if (GlobalSettings.highlightedAuthor == null || GlobalSettings.highlightedAuthor == stakeholder.Value.dBStakeholderStore.gitSignature)
            {
                GUI.DrawTexture(new Rect(0, 20 * i, 200, 20), authorBackgroundTextures[stakeholder.Key]);
            }
            if (GUI.Button(new Rect(10, 20 * i, 800, 20), stakeholder.Value.dBStakeholderStore.gitSignature, uiStyle.label))
            {
                if (GlobalSettings.highlightedAuthor == stakeholder.Value.dBStakeholderStore.gitSignature)
                {
                    GlobalSettings.highlightedAuthor = null;
                }
                else
                {
                    GlobalSettings.highlightedAuthor = stakeholder.Value.dBStakeholderStore.gitSignature;
                }
            }

            i++;
        }

        GUI.EndScrollView();

    }
}
