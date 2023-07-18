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
    public static DBFiles files;
    public static DBStakeholders stakeholders;

    public static Queue<Action> actionQueue = new Queue<Action>();

    public static Color fileDefaultColor = Color.white;

    // Start is called before the first frame update
    void Start()
    {
        sFile = file;
        sChangedFile = changedFile;
        sCommit = commit;
        sBranchTreeMaterial = branchTreeMaterial;
        sCommitTreeMaterial = commitTreeMaterial;
        helix = new Helix();
    }

    // Update is called once per frame
    void Update()
    {
        ShortCuts();

        helix.CheckUpdate();


        lock (actionQueue)
        {
            while (actionQueue.Count != 0) actionQueue.Dequeue().Invoke();
        }

    }

    private static void ShortCuts()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            GlobalSettings.showAuthorColors = !GlobalSettings.showAuthorColors;
            RuntimeDebug.Log("Show Author Colors: " + GlobalSettings.showAuthorColors);
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            GlobalSettings.debugMode = !GlobalSettings.debugMode;
            RuntimeDebug.Log("Debug Mode: " + GlobalSettings.debugMode);
        }
    }
}
