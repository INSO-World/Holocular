using UnityEngine;

public class FileInfo : MonoBehaviour
{

    Window window;

    public Vector2 hunksScrollPosition = Vector2.zero;
    public Vector2 ownershipScrollPosition = Vector2.zero;
    public Vector2 fileInfoScrollPosition = Vector2.zero;

    static int windowWidth = 400;
    static int windowHeight = 800;
    Rect fileInfoWindowRect = new Rect(770, 10, windowWidth, windowHeight);

    // Start is called before the first frame update
    void Start()
    {
        window = new Window("File Info", UiSkinManger.sUiStyle, windowWidth, windowHeight);
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnGUI()
    {
        GUI.skin = UiSkinManger.sUiStyle;

        if (GlobalSettings.showFileInfo)
        {
            fileInfoWindowRect = GUI.Window(2, fileInfoWindowRect, FileInfoWindow, "");

        }


    }

    void FileInfoWindow(int windowID)
    {
        window.render();

        GUI.BeginGroup(new Rect(10, 40, windowWidth - 10, 1200));
        if (Main.selectedFile == null)
        {
            GUILayout.BeginVertical();
            GUILayout.Label("No File Selected!", UiSkinManger.sUiStyle.GetStyle("headline"));
            GUILayout.Label("Select a file-node in the tree to view file information");
            GUILayout.EndVertical();
        }
        else
        {
            fileInfoScrollPosition = GUILayout.BeginScrollView(fileInfoScrollPosition, false, true, GUILayout.Height(windowHeight - 40), GUILayout.Width(windowWidth - 20));
            GUILayout.BeginVertical();
            //Main File Info
            GUILayout.BeginVertical();
            GUILayout.Label(Main.selectedFile.fileName, UiSkinManger.sUiStyle.GetStyle("headline"));
            GUILayout.Label("Path:\n" + Main.selectedFile.fullFilePath);
            GUILayout.Label("Owner:\n" + Main.selectedFile.owner);
            GUILayout.Label("(" + Main.selectedFile.linesOwned + " of " + Main.selectedFile.lines + " lines owned)");
            GUILayout.EndVertical();
            GUILayout.Space(20);

            //Commit Info
            GUILayout.BeginVertical();
            GUILayout.Label("Commit", UiSkinManger.sUiStyle.GetStyle("headline"));
            GUILayout.Label("SHA:\n" + Main.selectedFile.commit.dBCommitStore.sha);
            GUILayout.Label("Parents:\n" + string.Join(",", Main.selectedFile.commit.parents));
            if (GUILayout.Button("Compare With Parent", GUILayout.Width(windowWidth - 30)))
            {
                GlobalSettings.showFileCompare = true;
            }
            GUILayout.Label("Author (Click to show all files edited by this Author):");
            UiSkinManger.sUiStyle.GetStyle("author").normal.textColor = Main.helix.stakeholders[Main.selectedFile.commit.signature].colorStore;
            if (GUILayout.Button(Main.selectedFile.commit.signature, UiSkinManger.sUiStyle.GetStyle("author"), GUILayout.Width(windowWidth - 30)))
            {
                GlobalSettings.SelectAuthor(Main.selectedFile.commit.signature);
            }
            GUILayout.Label("Date:\n" + Main.selectedFile.commit.dBCommitStore.date);
            GUILayout.Label("Branch:\n" + Main.selectedFile.commit.dBCommitStore.branch);
            GUILayout.Label("Url:");
            if (GUILayout.Button(Main.selectedFile.commit.dBCommitStore.webUrl, UiSkinManger.sUiStyle.GetStyle("url"), GUILayout.Width(windowWidth - 30)))
            {
                Application.OpenURL(Main.selectedFile.commit.dBCommitStore.webUrl);
            }
            GUILayout.EndVertical();
            GUILayout.Space(20);

            //Stats
            GUILayout.BeginVertical();
            GUILayout.Label("Stats", UiSkinManger.sUiStyle.GetStyle("headline"));
            GUILayout.Label("Additions:\n" + Main.selectedFile.commitFileRelation.stats.additions);
            GUILayout.Label("Deletions:\n" + Main.selectedFile.commitFileRelation.stats.deletions);
            GUILayout.EndVertical();
            GUILayout.Space(20);

            //Hunks
            GUILayout.BeginVertical();
            GUILayout.Label("Hunks", UiSkinManger.sUiStyle.GetStyle("headline"));
            hunksScrollPosition = GUILayout.BeginScrollView(hunksScrollPosition, false, true, GUILayout.Height(200));

            for (int i = 0; i < Main.selectedFile.commitFileRelation.hunks.Length; i++)
            {
                DBHunk hunk = Main.selectedFile.commitFileRelation.hunks[i];
                GUILayout.BeginVertical(UiSkinManger.sUiStyle.GetStyle("hunk"));

                GUILayout.Label("Hunk " + i, UiSkinManger.sUiStyle.GetStyle("subheadline"));
                GUILayout.Label("Old Start: " + hunk.oldStart);
                GUILayout.Label("Old Lines: " + hunk.oldLines);
                GUILayout.Label("New Start: " + hunk.newStart);
                GUILayout.Label("New Lines: " + hunk.newLines);
                GUILayout.EndVertical();
                GUILayout.Space(5);
            }

            GUILayout.EndScrollView();
            GUILayout.EndVertical();
            GUILayout.Space(20);

            //OwnerShip Hunks
            GUILayout.BeginVertical();
            GUILayout.Label("Ownership", UiSkinManger.sUiStyle.GetStyle("headline"));
            ownershipScrollPosition = GUILayout.BeginScrollView(ownershipScrollPosition, false, true, GUILayout.Height(200));
            for (int i = 0; i < Main.selectedFile.commitFileStakeholderRelationList.Count; i++)
            {
                GUILayout.BeginVertical(UiSkinManger.sUiStyle.GetStyle("hunk"));
                GUILayout.Label(Main.selectedFile.commitFileStakeholderRelationList[i].helixStakeholderStore.dBStakeholderStore.gitSignature, UiSkinManger.sUiStyle.GetStyle("subheadline"));
                GUILayout.Label("OwnedLines: " + Utils.CalculateOwnedLines(Main.selectedFile.commitFileStakeholderRelationList[i].dBCommitsFilesStakeholderStore.hunks));
                GUILayout.EndVertical();
                GUILayout.Space(5);
            }

            GUI.EndScrollView();
            GUILayout.EndVertical();
            GUILayout.Space(20);

            GUILayout.EndVertical();
            GUILayout.EndVertical();
            GUILayout.EndScrollView();
        }
        GUI.EndGroup();


    }
}
