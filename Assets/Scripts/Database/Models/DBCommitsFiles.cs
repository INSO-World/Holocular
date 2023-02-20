using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DBCommitsFiles
{
    public DBCommitFileRelation[] commitsFiles;

}

[System.Serializable]
public class DBCommitFileRelation
{
    public string _id;
    public string from;
    public string to;
    public string _from;
    public string _to;
    public int lineCount;
    public DBHunk[] hunks;
    public DBStats stats;
}

[System.Serializable]
public class DBHunk
{
    public string webUrl;
    public int newLines;
    public int newStart;
    public int oldLines;
    public int oldStart;
}

[System.Serializable]
public class DBStats
{
    public int additions;
    public int deletions;
}