using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FileController : MonoBehaviour
{
    public string fullFilePath = "";
    public string fileName = "";
    public DBCommitFileRelation commitFileRelation;
    public List<HelixCommitFileStakeholderRelation> commitFileStakeholderRelationList;
    public HelixCommit commit;

    public string userId = "";
    public string owner = "";
    public int linesOwned = 0;
    public int lines = 0;

    public GameObject visual;
    private BoxCollider bc;
    private MeshRenderer mr;
    private Material mat;

    private UnityAction updateFileColorListener;
    private UnityAction updateFileSizeListener;
    private UnityAction updateFolderSearchListener;

    // Start is called before the first frame update
    void Start()
    {
        bc = transform.GetComponent<BoxCollider>();
        mr = visual.GetComponent<MeshRenderer>();
        mat = mr.material;
        updateFileColorListener = new UnityAction(ChangeColor);
        updateFileSizeListener = new UnityAction(ChangeSize);
        updateFolderSearchListener = new UnityAction(ChangeVisibility);
        EventManager.StartListening("updateFileColor", updateFileColorListener);
        EventManager.StartListening("updateFileSize", updateFileSizeListener);
        EventManager.StartListening("updateFolders", updateFolderSearchListener);
        Init();
    }


    public void Init()
    {
        for (int i = 0; i < commitFileStakeholderRelationList.Count; i++)
        {
            int ownedLinesByAuthor = Utils.CalculateOwnedLines(commitFileStakeholderRelationList[i].dBCommitsFilesStakeholderStore.hunks);
            lines += ownedLinesByAuthor;
            if (ownedLinesByAuthor > linesOwned)
            {
                linesOwned = ownedLinesByAuthor;
                owner = commitFileStakeholderRelationList[i].helixStakeholderStore.dBStakeholderStore.gitSignature;
            }
        }
        ChangeColor();
        ChangeSize();
        ChangeVisibility();
    }

    private void ChangeColor()
    {
        if (GlobalSettings.fileIsSelected && fullFilePath != Main.selectedFile.fullFilePath)
        {
            mat.color = Main.fileDeSelectedColor;
        }
        else if (GlobalSettings.showAuthorColors && (GlobalSettings.highlightedAuthor == null || GlobalSettings.highlightedAuthor == commit.signature))
        {
            mat.color = Main.helix.stakeholders[commit.signature].colorStore;
        }
        else if (GlobalSettings.showOwnershipColors && owner != "" && (GlobalSettings.highlightedAuthor == null || GlobalSettings.highlightedAuthor == owner))
        {
            mat.color = Main.helix.stakeholders[owner].colorStore;
        }
        else if (GlobalSettings.showHotspotColors)
        {
            mat.color = Color.Lerp(Main.fileDefaultColor, Main.fileHotspotColor, 1f / GlobalSettings.hotspotThreshold * (commitFileRelation.stats.additions + commitFileRelation.stats.deletions));
        }
        else if (GlobalSettings.showBranchColors && (GlobalSettings.highlightedBranch == null || GlobalSettings.highlightedBranch == commit.dBCommitStore.branch))
        {
            mat.color = Main.helix.branches[commit.dBCommitStore.branch].colorStore;
        }
        else if (GlobalSettings.showAuthorColors || GlobalSettings.showOwnershipColors || GlobalSettings.showBranchColors)
        {
            mat.color = Main.fileDeSelectedColor;
        }
        else
        {
            mat.color = Main.fileDefaultColor;
        }
    }

    private void ChangeSize()
    {
        transform.localScale = new Vector3(GlobalSettings.fileSize, GlobalSettings.fileSize, GlobalSettings.fileSize);
    }

    private void ChangeVisibility()
    {
        if (GlobalSettings.folderSearch.Length == 0 || fullFilePath.StartsWith(GlobalSettings.folderSearch))
        {
            bc.enabled = true;
            mr.enabled = true;
        }
        else
        {
            bc.enabled = false;
            mr.enabled = false;
        }
    }
}
