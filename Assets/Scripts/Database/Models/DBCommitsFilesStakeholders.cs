using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DBCommitsFilesStakeholders
{
    public DBCommitFileStakeholderRelation[] commitsFilesStakeholders;

}

[System.Serializable]
public class DBCommitFileStakeholderRelation
{
    public string _id;
    public string _from;
    public string _to;
    public string from;
    public string to;
    public int ownedLines;
    public DBOwnershipHunk[] hunks;
}

[System.Serializable]
public class DBOwnershipHunk
{
    public string signature;
    public int linesChanged;
    public int startLine;
    public int endLine;
}
