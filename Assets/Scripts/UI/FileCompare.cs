using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.Networking;

public class FileCompare : MonoBehaviour
{
    public GUISkin uiStyle;

    Window window;

    public Vector2 hunksScrollPosition = Vector2.zero;
    public Vector2 ownershipScrollPosition = Vector2.zero;
    public Vector2 fileInfoScrollPosition = Vector2.zero;

    static int windowWidth = 800;
    static int windowHeight = 800;
    Rect fileInfoWindowRect = new Rect(Screen.width / 2 - windowWidth / 2, Screen.height / 2 - windowHeight / 2, windowWidth, windowHeight);

    string selectedFilePath = "";
    string selectedSha = "";
    string[] currFileContentLines = new string[0];
    string[] parentFileContentLines = new string[0];
    int maxLines = 0;


    public Vector2 contentScrollPosition = Vector2.zero;

    Texture2D hunkTexture;

    // Start is called before the first frame update
    void Start()
    {
        window = new Window("File Compare", uiStyle, windowWidth, windowHeight);
        hunkTexture = new Texture2D(0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalSettings.showFileCompare)
        {
            if (selectedFilePath != Main.selectedFile.fullFilePath)
            {
                StartCoroutine(getFileContent(Main.selectedFile.fullFilePath, Main.selectedFile.commit.webUrl, Main.helix.commits[Main.selectedFile.commit.parents.Split(",")[0]].dBCommitStore.webUrl));
                selectedFilePath = Main.selectedFile.fullFilePath;
            }

            if (selectedSha != Main.selectedFile.commit.sha)
            {
                StartCoroutine(getFileContent(Main.selectedFile.fullFilePath, Main.selectedFile.commit.webUrl, Main.helix.commits[Main.selectedFile.commit.parents.Split(",")[0]].dBCommitStore.webUrl));
                selectedSha = Main.selectedFile.commit.sha;
            }
        }
    }



    private void OnGUI()
    {
        GUI.skin = uiStyle;

        if (GlobalSettings.showFileCompare)
        {
            fileInfoWindowRect = GUI.Window(5, fileInfoWindowRect, FileInfoWindow, "");
        }
    }

    void FileInfoWindow(int windowID)
    {
        window.render();
        GUI.Label(new Rect(new Rect(0, 30, windowWidth / 2 - 20, 20)), selectedSha, uiStyle.GetStyle("headline"));
        GUI.Label(new Rect(new Rect(windowWidth / 2, 30, windowWidth / 2 - 20, 20)), Main.selectedFile.commit.parents.Split(",")[0], uiStyle.GetStyle("headline"));
        contentScrollPosition = GUI.BeginScrollView(new Rect(0, 50, windowWidth, windowHeight - 50), contentScrollPosition, new Rect(0, 0, windowWidth - 20, 20 * maxLines));

        GUI.BeginGroup(new Rect(0, 0, windowWidth / 2 - 20, 20 * maxLines));
        fileContentView(parentFileContentLines, false, Main.selectedFile.commitFileRelation.hunks);
        GUI.EndGroup();
        GUI.DrawTexture(new Rect(windowWidth / 2 - 20, 0, 20, maxLines * 20), hunkTexture);
        GUI.BeginGroup(new Rect(windowWidth / 2, 0, windowWidth / 2 - 20, 20 * maxLines));
        fileContentView(currFileContentLines, true, Main.selectedFile.commitFileRelation.hunks);
        GUI.EndGroup();

        GUI.EndScrollView();
    }

    //mode:  false...oldFile; true...newFile
    void fileContentView(string[] lines, bool mode, DBHunk[] hunks)
    {
        for (int i = 0; i < lines.Length; i++)
        {
            GUI.Label(new Rect(0, i * 20, 1000, 20), "", i % 2 == 0 ? uiStyle.GetStyle("fileCompareEvenLine") : uiStyle.GetStyle("fileCompareOddLine"));
        }

        foreach (DBHunk hunk in hunks)
        {
            GUIStyle changeType;

            if (hunk.newLines > hunk.oldLines)  //Additions
            {
                changeType = uiStyle.GetStyle("fileCompareAddition");
            }
            else if (hunk.newLines < hunk.oldLines) //Deletions
            {
                changeType = uiStyle.GetStyle("fileCompareDeletion");
            }
            else    //Changes
            {
                changeType = uiStyle.GetStyle("fileCompareChange");
            }
            if (mode)
            {
                if (hunk.newLines > 0)
                {
                    for (int i = 0; i < hunk.newLines; i++)
                    {
                        GUI.Label(new Rect(0, (hunk.newStart - 1) * 20 + i * 20, 1000, 20), "", changeType);
                    }
                }
                else
                {
                    GUI.Label(new Rect(0, (hunk.newStart) * 20, 1000, 2), "", changeType);
                }

            }
            else
            {
                if (hunk.oldLines > 0)
                {
                    for (int i = 0; i < hunk.oldLines; i++)
                    {
                        GUI.Label(new Rect(0, (hunk.oldStart - 1) * 20 + i * 20, 1000, 20), "", changeType);
                    }
                }
                else
                {
                    GUI.Label(new Rect(0, (hunk.oldStart) * 20, 1000, 2), "", changeType);
                }
            }
        }

        for (int i = 0; i < lines.Length; i++)
        {
            GUI.Label(new Rect(0, i * 20, 1000, 20), lines[i]);
        }
    }

    IEnumerator getFileContent(string fullFilePath, string weburl, string parentWebUrl)
    {
        string rawUrl = weburl.Replace("github.com", "raw.githubusercontent.com").Replace("/commit/", "/") + "/" + fullFilePath;
        string rawParentUrl = parentWebUrl.Replace("github.com", "raw.githubusercontent.com").Replace("/commit/", "/") + "/" + fullFilePath;

        maxLines = 0;
        UnityWebRequest currentRequest = UnityWebRequest.Get(rawUrl);
        yield return currentRequest.SendWebRequest();

        if (currentRequest.result == UnityWebRequest.Result.ConnectionError)
        {
            currFileContentLines = new string[1] { "Error Loading Content!" };
        }
        else
        {
            currFileContentLines = currentRequest.downloadHandler.text.Split("\n");
            maxLines = currFileContentLines.Length;
        }

        UnityWebRequest parentRequest = UnityWebRequest.Get(rawParentUrl);
        yield return parentRequest.SendWebRequest();

        if (parentRequest.result == UnityWebRequest.Result.ConnectionError)
        {
            parentFileContentLines = new string[1] { "Error Loading Content!" };
        }
        else
        {
            parentFileContentLines = parentRequest.downloadHandler.text.Split("\n");
            if (maxLines < parentFileContentLines.Length)
            {
                maxLines = parentFileContentLines.Length;
            }
        }
        GenerateHunkTexture();
    }

    void GenerateHunkTexture()
    {
        int width = 20;
        hunkTexture = new Texture2D(width, maxLines * 20);
        foreach (DBHunk hunk in Main.selectedFile.commitFileRelation.hunks)
        {
            float y11;
            float y21;
            float y12;
            float y22;
            if (hunk.oldLines > 0)
            {
                y11 = (hunk.oldStart - 1) * 20;
                y21 = (hunk.oldStart - 1) * 20 + hunk.oldLines * 20;
            }
            else
            {
                y11 = hunk.oldStart * 20;
                y21 = hunk.oldStart * 20 + 2;
            }
            if (hunk.newLines > 0)
            {
                y12 = (hunk.newStart - 1) * 20;
                y22 = (hunk.newStart - 1) * 20 + hunk.newLines * 20;
            }
            else
            {
                y12 = hunk.newStart * 20;
                y22 = hunk.newStart * 20 + 2;
            }

            float dx1 = (y12 - y11) / width;
            float dx2 = (y22 - y21) / width;

            float currY1 = y11;
            float currY2 = y21;

            Color color;
            if (hunk.newLines > hunk.oldLines)  //Additions
            {
                color = new Color(52f / 255f, 199f / 255f, 89f / 255f);
            }
            else if (hunk.newLines < hunk.oldLines) //Deletions
            {
                color = new Color(255f, 56f / 255f, 48f / 255f);
            }
            else //Changes
            {
                color = new Color(0f, 122f / 255f, 255f / 255f);
            }
            for (int x = 0; x < width; x++)
            {
                for (int y = (int)currY1; y <= (int)currY2; y++)
                {
                    hunkTexture.SetPixel(x, maxLines * 20 - y, color);
                }
                currY1 += dx1;
                currY2 += dx2;
            }
        }
        hunkTexture.Apply();
    }
}
