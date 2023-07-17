using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class HelixCommitFileRelation
{
    public DBCommitFileRelation dBCommitsFilesStore;

    public HelixCommitFileRelation(DBCommitFileRelation dBCommitsFiles)
    {
        dBCommitsFilesStore = dBCommitsFiles;
    }
}

