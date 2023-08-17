using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.Handles;

public class FileController : MonoBehaviour
{
    public string fullFilePath = "";
    public string fileName = "";
    public DBCommitFileRelation commitFileRelation;
    public List<HelixCommitFileStakeholderRelation> commitFileStakeholderRelationList;
    public DBCommit commit;

    public string owner = "";
    public int linesOwned = 0;
    public int lines = 0;

    public GameObject visual;

    private Material mat;

    private UnityAction updateFileColorListener;
    private UnityAction updateFileSizeListener;

    // Start is called before the first frame update
    void Start()
    {
        mat = visual.GetComponent<MeshRenderer>().material;
        updateFileColorListener = new UnityAction(ChangeColor);
        updateFileSizeListener = new UnityAction(ChangeSize);
        EventManager.StartListening("updateFileColor", updateFileColorListener);
        EventManager.StartListening("updateFileSize", updateFileSizeListener);
        Init();
    }


    public void Init()
    {
        for (int i = 0; i < commitFileStakeholderRelationList.Count; i++)
        {
            lines += commitFileStakeholderRelationList[i].dBCommitsFilesStakeholderStore.ownedLines;
            if (commitFileStakeholderRelationList[i].dBCommitsFilesStakeholderStore.ownedLines > linesOwned)
            {
                linesOwned = commitFileStakeholderRelationList[i].dBCommitsFilesStakeholderStore.ownedLines;
                owner = commitFileStakeholderRelationList[i].helixStakeholderStore.dBStakeholderStore.gitSignature;
            }
        }
        ChangeColor();
        ChangeSize();
    }

    private void ChangeColor()
    {
        if (GlobalSettings.showAuthorColors && (GlobalSettings.highlightedAuthor == null || GlobalSettings.highlightedAuthor == commit.signature))
        {
            mat.color = Main.helix.stakeholders[commit.signature].colorStore;
        }
        else if (GlobalSettings.showOwnershipColors && owner != "" && (GlobalSettings.highlightedAuthor == null || GlobalSettings.highlightedAuthor == owner))
        {
            Debug.Log("Test: " + mat);
            mat.color = Main.helix.stakeholders[owner].colorStore;
        }
        else if (GlobalSettings.showBranchColors && (GlobalSettings.highlightedBranch == null || GlobalSettings.highlightedBranch == commit.branch))
        {
            mat.color = Main.helix.branches[commit.branch].colorStore;
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
}
