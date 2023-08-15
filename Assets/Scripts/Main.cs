using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{

    public GameObject file;
    public static GameObject sFile;

    public GameObject changedFile;
    public static GameObject sChangedFile;

    public GameObject commit;
    public static GameObject sCommit;

    public Material branchTreeMaterial;
    public static Material sBranchTreeMaterial;

    public Material commitTreeMaterial;
    public static Material sCommitTreeMaterial;

    public static int mouseSensitivity = 5;

    public static int moveSpeed = 100;


    public static Helix helix;


    public static float helixReferenceRadius = 5f;
    public static float helixeRadiusSpread = 4f;
    public static float helixBranchOffset = 100f;

    public static DBCommits commits;
    public static DBBranches branches;
    public static DBCommitsFiles commitsFiles;
    public static DBCommitsFilesStakeholders commitsFilesStakeholders;
    public static DBFiles files;
    public static DBStakeholders stakeholders;

    public static Queue<Action> actionQueue = new Queue<Action>();

    public static Color fileDefaultColor = Color.white;
    public static Color fileDeSelectedColor = Color.gray;

    public static GameObject lastSelectedObject;

    public static FileController selectedFile;


    // Start is called before the first frame update
    void Start()
    {
        sFile = file;
        sChangedFile = changedFile;
        sCommit = commit;
        sBranchTreeMaterial = branchTreeMaterial;
        sCommitTreeMaterial = commitTreeMaterial;
        helix = new Helix(GameObject.Find("Helix"));
    }

    // Update is called once per frame
    void Update()
    {
        CheckShortcuts();

        helix.CheckUpdate();


        lock (actionQueue)
        {
            while (actionQueue.Count != 0) actionQueue.Dequeue().Invoke();
        }

    }

    private static void CheckShortcuts()
    {
        if (Input.GetKeyDown(KeyCode.C) && !(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
        {
            GlobalSettings.showAuthorColors = !GlobalSettings.showAuthorColors;
            RuntimeDebug.Log("Show Author Colors: " + GlobalSettings.showAuthorColors);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            GlobalSettings.showOwnershipColors = !GlobalSettings.showOwnershipColors;
            RuntimeDebug.Log("Show Ownership Colors: " + GlobalSettings.showOwnershipColors);
        }

        if (Input.GetKeyDown(KeyCode.B) && !(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
        {
            GlobalSettings.showBranchColors = !GlobalSettings.showBranchColors;
            RuntimeDebug.Log("Show Branch Colors: " + GlobalSettings.showBranchColors);
        }

        if (Input.GetKeyDown(KeyCode.C) && Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            GlobalSettings.showAuthorPalette = !GlobalSettings.showAuthorPalette;
            RuntimeDebug.Log("Show Author Palette: " + GlobalSettings.showAuthorPalette);
        }

        if (Input.GetKeyDown(KeyCode.B) && Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            GlobalSettings.showBranchPalette = !GlobalSettings.showBranchPalette;
            RuntimeDebug.Log("Show Branch Palette: " + GlobalSettings.showBranchPalette);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            GlobalSettings.showSettings = !GlobalSettings.showSettings;
            RuntimeDebug.Log("Show Settings: " + GlobalSettings.showSettings);
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            GlobalSettings.debugMode = !GlobalSettings.debugMode;
            RuntimeDebug.Log("Debug Mode: " + GlobalSettings.debugMode);
        }

    }
}
