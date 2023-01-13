using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DBBranches
{
    public DBBranch[] branches;

}

[System.Serializable]
public class DBBranch
{
    public string id;
    public string branch;
    public string active;
}