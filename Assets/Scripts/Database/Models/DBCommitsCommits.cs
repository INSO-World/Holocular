using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DBCommitsCommits
{
    public DBCommitComitRelation[] commitsCommits;

}

[System.Serializable]
public class DBCommitComitRelation
{
    public string _id;
    public string from;
    public string to;
    public string _from;
    public string _to;
}