using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DBCommitsStakeholders
{
    public DBCommitStakeholderRelation[] commitsStakeholders;

}

[System.Serializable]
public class DBCommitStakeholderRelation
{
    public string _id;
    public string from;
    public string to;
    public string _from;
    public string _to;
}