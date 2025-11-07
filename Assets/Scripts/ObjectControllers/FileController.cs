using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.Events;
using Utilities;

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

    private BoxCollider bc;
    private MeshRenderer mrHD;
    private MeshRenderer mrMD;
    private Material matHD;
    private Material matMD;

    private UnityAction updateFileColorListener;
    private UnityAction updateFileSizeListener;
    private UnityAction updateFolderSearchListener;
    private UnityAction updateLODListener;

    //LoD
    public GameObject visualHighDetail;
    public GameObject visualMediumDetail;
    
    private LevelOfDetail.LevelOfDetailType levelOfDetail;
    
    // Start is called before the first frame update
    void Start()
    {
        levelOfDetail = LevelOfDetail.LevelOfDetailType.Low;
        ChangeVisual(levelOfDetail);
        bc = transform.GetComponent<BoxCollider>();
        mrHD = visualHighDetail.transform.GetChild(0).GetComponent<MeshRenderer>();
        mrMD = visualMediumDetail.transform.GetChild(0).GetComponent<MeshRenderer>();
        matHD = mrHD.material;
        matMD = mrMD.material;
        updateFileColorListener = new UnityAction(ChangeColor);
        updateFileSizeListener = new UnityAction(ChangeSize);
        updateFolderSearchListener = new UnityAction(ChangeVisibility);
        updateLODListener = new UnityAction(UpdateLOD);
        EventManager.StartListening("updateFileColor", updateFileColorListener);
        EventManager.StartListening("updateFileSize", updateFileSizeListener);
        EventManager.StartListening("updateFolders", updateFolderSearchListener);
        EventManager.StartListening("updateLOD", updateLODListener);
        EventManager.StartListening("switchDarkLightMode", updateFileColorListener);

        Init();
    }
    
    
    void UpdateLOD()
    {
        if (Vector3.Distance(CameraControll.mainCamera.position, transform.position) < Main.lodDistanceHeight)
        {
            if (levelOfDetail != LevelOfDetail.LevelOfDetailType.High)
            {
                levelOfDetail = LevelOfDetail.LevelOfDetailType.High;
                ChangeVisual(levelOfDetail);
            }
        }
        else if (Vector3.Distance(CameraControll.mainCamera.position, transform.position) >= Main.lodDistanceHeight
                 && Vector3.Distance(CameraControll.mainCamera.position, transform.position) < Main.lodDistanceMedium)
        {
            if (levelOfDetail != LevelOfDetail.LevelOfDetailType.Medium)
            {
                levelOfDetail = LevelOfDetail.LevelOfDetailType.Medium;
                ChangeVisual(levelOfDetail);
            }
        }
        else
        {
            if (levelOfDetail != LevelOfDetail.LevelOfDetailType.Low)
            {
                levelOfDetail = LevelOfDetail.LevelOfDetailType.Low;
                ChangeVisual(levelOfDetail);
            }
        }

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
        Color color;
        if (GlobalSettings.fileIsSelected && fullFilePath != Main.selectedFile.fullFilePath)
        {
            color = Main.fileDeSelectedColor;
        }
        else if (GlobalSettings.fileIsSelected && this == Main.selectedFile)
        {
            color = Main.fileSelectedColor;
        }
        else if (GlobalSettings.showAuthorColors && (GlobalSettings.highlightedAuthor == null || GlobalSettings.highlightedAuthor == commit.signature))
        {
            color = Main.helix.stakeholders[commit.signature].colorStore;
        }
        else if (GlobalSettings.showOwnershipColors && owner != "" && (GlobalSettings.highlightedAuthor == null || GlobalSettings.highlightedAuthor == owner))
        {
            color = Main.helix.stakeholders[owner].colorStore;
        }
        else if (GlobalSettings.showHotspotColors)
        {
            color = Color.Lerp(Main.darkLightMode? Main.fileDarkDefaultColor: Main.fileDefaultColor, Main.fileHotspotColor, 1f / GlobalSettings.hotspotThreshold * (commitFileRelation.stats.additions + commitFileRelation.stats.deletions));
        }
        else if (GlobalSettings.showBranchColors && (GlobalSettings.highlightedBranch == null || GlobalSettings.highlightedBranch == commit.dBCommitStore.branch))
        {
            color = Main.helix.branches[commit.dBCommitStore.branch].colorStore;
        }
        else if (GlobalSettings.showAuthorColors || GlobalSettings.showOwnershipColors || GlobalSettings.showBranchColors)
        {
            color = Main.fileDeSelectedColor;
        }
        else
        {
            color = Main.darkLightMode? Main.fileDarkDefaultColor: Main.fileDefaultColor;
        }
        HelixParticleSystemRenderer.UpdateElement(commit.dBCommitStore.sha,fullFilePath,new HelixParticleSystemElement(transform.position, color));
        matHD.color = color;
        matMD.color = color;
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
            mrHD.enabled = true;
            mrMD.enabled = true;
            HelixParticleSystemRenderer.UpdateElement(commit.dBCommitStore.sha,fullFilePath,new HelixParticleSystemElement(transform.position, Main.darkLightMode? Main.fileDarkDefaultColor: Main.fileDefaultColor));
        }
        else
        {
            bc.enabled = false;
            mrHD.enabled = false;
            mrMD.enabled = false;
            HelixParticleSystemRenderer.RemoveFile(commit.dBCommitStore.sha,fullFilePath);
        }
    }
    
    private void ChangeVisual(LevelOfDetail.LevelOfDetailType levelOfDetail)
    {
        switch (levelOfDetail)
        {
            case LevelOfDetail.LevelOfDetailType.High:
                visualHighDetail.SetActive(true);
                visualMediumDetail.SetActive(false);
                break;
            case LevelOfDetail.LevelOfDetailType.Medium:
                visualHighDetail.SetActive(false);
                visualMediumDetail.SetActive(true);
                break;
            case LevelOfDetail.LevelOfDetailType.Low:
            default:
                visualHighDetail.SetActive(false);
                visualMediumDetail.SetActive(false);
                break;
        }
    }
}
