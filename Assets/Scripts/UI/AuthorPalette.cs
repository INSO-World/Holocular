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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!authorBackgroundTexturesInitialized && Helix.stakeholders.Count > 0)
        {
            foreach (KeyValuePair<string, HelixStakeholder> stakeholder in Helix.stakeholders)
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
        GUI.Label(new Rect(0, 0, 200, 20), "Author Palette", uiStyle.GetStyle("windowBar"));

        authorsScrollPosition = GUI.BeginScrollView(new Rect(0, 20, 200, Screen.height / 2 - 20), authorsScrollPosition, new Rect(0, 0, 180, 20 * Helix.stakeholders.Count));
        int i = 0;
        foreach (KeyValuePair<string, HelixStakeholder> stakeholder in Helix.stakeholders)
        {
            GUI.DrawTexture(new Rect(0, 20 * i, 200, 20), authorBackgroundTextures[stakeholder.Key]);
            GUI.Label(new Rect(10, 20 * i, 200, 20), stakeholder.Value.dBStakeholderStore.gitSignature);

            i++;
        }

        GUI.EndScrollView();

        GUI.DragWindow(new Rect(0, 0, 200, 20));
    }
}
