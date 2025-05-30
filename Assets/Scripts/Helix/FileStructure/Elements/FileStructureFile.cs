﻿using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class FileStructureFile : MonoBehaviour, IFileStructureElement
{
    private string name = "";
    public bool changed = false;
    public string fullPath = "";
    public string authorSignature;
    public HelixCommitFileRelation helixCommitFileRelation;
    public DBCommit commit;

    public FileStructureFile()
    {
    }

    string IFileStructureElement.Name { get { return name; } set { name = value; } }
    bool IFileStructureElement.Changed { get { return changed; } set { changed = value; } }

}

