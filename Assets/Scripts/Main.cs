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


    public static int mouseSensitivity = 5;

    public static int moveSpeed = 10;

    public static bool debugMode = true;

    public static Helix helix;

    public static float helixReferenceRadius = 5f;

    public static DBCommits commits;
    public static DBBranches branches;
    public static DBCommitsFiles commitsFiles;
    public static DBFiles files;


    // Start is called before the first frame update
    void Start()
    {
        sFile = file;
        sChangedFile = changedFile;
        sCommit = commit;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            debugMode = !debugMode;
            RuntimeDebug.Log("Debug Mode: " + debugMode);
        }
    }

}
