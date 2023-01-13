using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DBCommits
{
	public DBCommit[] commits;
}

[System.Serializable]
public class DBCommit
{
    public string _id;
    public string sha;
    public string signature;
    public string date;
    public string message;
    public string webUrl;
    public string branch;
    public string parents;
    public CommitStats stats;
}

[System.Serializable]
public class CommitStats
{
    public int additions;
    public int deletions;
}
