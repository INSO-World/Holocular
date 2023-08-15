using UnityEngine;

public class FileInfo : MonoBehaviour
{
    public GUISkin uiStyle;
    Rect fileInfoWindowRect = new Rect(700, 10, 400, 800);

    WindowBar windowBar;

    public Vector2 hunksScrollPosition = Vector2.zero;
    public Vector2 ownershipScrollPosition = Vector2.zero;
    public Vector2 fileInfoScrollPosition = Vector2.zero;

    int windowWidth = 400;

    // Start is called before the first frame update
    void Start()
    {
        windowBar = new WindowBar("File Info", uiStyle, windowWidth);
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnGUI()
    {
        GUI.skin = uiStyle;

        if (GlobalSettings.showFileInfo)
        {
            fileInfoWindowRect = GUI.Window(2, fileInfoWindowRect, FileInfoWindow, "");

        }


    }

    void FileInfoWindow(int windowID)
    {
        windowBar.render();
        fileInfoScrollPosition = GUI.BeginScrollView(new Rect(10, 40, windowWidth - 20, 760), fileInfoScrollPosition, new Rect(10, 40, windowWidth - 40, 1200));
        GUI.BeginGroup(new Rect(10, 40, windowWidth - 40, 1200));

        if (Main.selectedFile == null)
        {
            GUI.BeginGroup(new Rect(10, 0, windowWidth - 40, 70));
            GUI.Label(new Rect(0, 0, windowWidth - 40, 20), "No File Selected!", uiStyle.GetStyle("headline"));
            GUI.Label(new Rect(0, 30, windowWidth - 40, 60), "Select a file-node in the tree to view file information");
            GUI.EndGroup();
        }
        else
        {
            GUI.BeginGroup(new Rect(10, 0, windowWidth - 40, 150));
            GUI.Label(new Rect(0, 0, windowWidth - 40, 20), Main.selectedFile.fileName, uiStyle.GetStyle("headline"));
            GUI.Label(new Rect(0, 30, windowWidth - 40, 40), "Path:\n" + Main.selectedFile.fullFilePath);
            GUI.Label(new Rect(0, 70, windowWidth - 40, 40), "Owner:\n" + Main.selectedFile.owner);
            GUI.Label(new Rect(0, 110, windowWidth - 40, 20), "(" + Main.selectedFile.linesOwned + " of " + Main.selectedFile.lines + " lines owned)");
            GUI.EndGroup();

            GUI.BeginGroup(new Rect(10, 150, windowWidth - 40, 300));
            GUI.Label(new Rect(0, 0, windowWidth - 40, 20), "Commit", uiStyle.GetStyle("headline"));
            GUI.Label(new Rect(0, 30, windowWidth - 40, 40), "SHA:\n" + Main.selectedFile.commit.sha);
            GUI.Label(new Rect(0, 70, windowWidth - 40, 60), "Parents:\n" + Main.selectedFile.commit.parents.Replace(",", "\n"));
            GUI.Label(new Rect(0, 130, windowWidth - 40, 20), "Author (Click to show all files edited by this Author):");
            uiStyle.GetStyle("author").normal.textColor = Main.helix.stakeholders[Main.selectedFile.commit.signature].colorStore;
            if (GUI.Button(new Rect(0, 150, windowWidth - 40, 20), Main.selectedFile.commit.signature, uiStyle.GetStyle("author")))
            {
                GlobalSettings.SelectAuthor(Main.selectedFile.commit.signature);
            }
            GUI.Label(new Rect(0, 170, windowWidth - 40, 40), "Date:\n" + Main.selectedFile.commit.date);
            GUI.Label(new Rect(0, 210, windowWidth - 40, 40), "Branch:\n" + Main.selectedFile.commit.branch);
            GUI.Label(new Rect(0, 250, windowWidth - 40, 20), "Url:");
            if (GUI.Button(new Rect(0, 270, windowWidth - 40, 20), Main.selectedFile.commit.webUrl, uiStyle.GetStyle("url")))
            {
                Application.OpenURL(Main.selectedFile.commit.webUrl);
            }
            GUI.EndGroup();

            GUI.BeginGroup(new Rect(10, 470, windowWidth - 40, 300));
            GUI.Label(new Rect(0, 0, windowWidth - 40, 20), "Stats", uiStyle.GetStyle("headline"));
            GUI.Label(new Rect(0, 30, windowWidth - 40, 40), "Additions:\n" + Main.selectedFile.commitFileRelation.stats.additions);
            GUI.Label(new Rect(0, 70, windowWidth - 40, 40), "Deletions:\n" + Main.selectedFile.commitFileRelation.stats.deletions);
            GUI.EndGroup();

            GUI.BeginGroup(new Rect(10, 600, windowWidth - 40, 300));
            GUI.Label(new Rect(0, 0, windowWidth - 40, 20), "Hunks", uiStyle.GetStyle("headline"));
            hunksScrollPosition = GUI.BeginScrollView(new Rect(0, 40, windowWidth - 60, 200), hunksScrollPosition, new Rect(0, 0, windowWidth - 80, 110 * Main.selectedFile.commitFileRelation.hunks.Length));

            for (int i = 0; i < Main.selectedFile.commitFileRelation.hunks.Length; i++)
            {
                DBHunk hunk = Main.selectedFile.commitFileRelation.hunks[i];
                GUI.BeginGroup(new Rect(0, 110 * i, windowWidth - 80, 100), uiStyle.GetStyle("hunk"));

                GUI.Label(new Rect(0, 0, windowWidth - 80, 20), "Hunk " + i, uiStyle.GetStyle("subheadline"));
                GUI.Label(new Rect(0, 20, windowWidth - 80, 20), "Old Start: " + hunk.oldStart);
                GUI.Label(new Rect(0, 40, windowWidth - 80, 20), "Old Lines: " + hunk.oldLines);
                GUI.Label(new Rect(0, 60, windowWidth - 80, 20), "New Start: " + hunk.newStart);
                GUI.Label(new Rect(0, 80, windowWidth - 80, 20), "New Lines: " + hunk.newLines);
                GUI.EndGroup();
            }

            GUI.EndScrollView();
            GUI.EndGroup();

            GUI.BeginGroup(new Rect(10, 840, windowWidth - 40, 300));
            GUI.Label(new Rect(0, 0, windowWidth - 40, 20), "Ownership", uiStyle.GetStyle("headline"));
            ownershipScrollPosition = GUI.BeginScrollView(new Rect(0, 40, windowWidth - 60, 200), ownershipScrollPosition, new Rect(0, 0, windowWidth - 80, 50 * Main.selectedFile.commitFileStakeholderRelationList.Count));
            for (int i = 0; i < Main.selectedFile.commitFileStakeholderRelationList.Count; i++)
            {
                GUI.BeginGroup(new Rect(0, 50 * i, windowWidth - 80, 40), uiStyle.GetStyle("hunk"));

                GUI.Label(new Rect(0, 0, windowWidth - 80, 20), Main.selectedFile.commitFileStakeholderRelationList[i].helixStakeholderStore.dBStakeholderStore.gitSignature, uiStyle.GetStyle("subheadline"));
                GUI.Label(new Rect(0, 20, windowWidth - 80, 20), "OwnedLines: " + Main.selectedFile.commitFileStakeholderRelationList[i].dBCommitsFilesStakeholderStore.ownedLines);
                GUI.EndGroup();
            }

            GUI.EndScrollView();
            GUI.EndGroup();
        }
        GUI.EndGroup();

        GUI.EndScrollView();

    }
}
