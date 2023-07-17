using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class FileStructureFile : MonoBehaviour, IFileStructureElement
{
    private string name = "";
    public bool changed = false;
    public string fullPath = "";
    public HelixCommitFileRelation helixCommitFileRelation;

    public FileStructureFile()
    {
    }

    string IFileStructureElement.Name { get { return name; } set { name = value; } }
    bool IFileStructureElement.Changed { get { return changed; } set { changed = value; } }
}

