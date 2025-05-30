﻿using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class FileStructure : MonoBehaviour
{
    IFileStructureElement root = new FileStructureFolder();

    int maxDepht = 1;



    public FileStructure()
    {
        root.Name = "root";
    }

    public void AddFilePathToFileStructure(DBCommit commit,
        string path,
        bool changedInThisCommit,
        HelixCommitFileRelation helixCommitFileRelation)
    {
        string[] splittedPath = path.Split('/');
        maxDepht = splittedPath.Length;
        (root as FileStructureFolder).AddElement(commit, path, "", splittedPath, changedInThisCommit, helixCommitFileRelation);
    }

    public void DrawHelixRing(HelixCommit commit,
        string branchName,
        Dictionary<string, HelixConnectionTree> fileHelixConnectiontreeDictionary,
        Dictionary<string, HelixCommit> shaCommitsRelation)
    {
        Transform rootFolderObject = (root as FileStructureFolder).Draw(commit.commitObject.transform,
            maxDepht * Main.helixReferenceRadius * Main.helixeRadiusSpread,
            new Color(0, 1, 1, 0.5f),
            new Color(0, 1, 1, 0.25f),
            0.75f,
            branchName,
            commit,
            new Vector3(0, 0, 0),
            fileHelixConnectiontreeDictionary,
            shaCommitsRelation);
        rootFolderObject.parent = commit.commitObject.transform;
    }

}

