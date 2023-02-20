using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelixBranch
{
    public Vector2 position;
    public DBBranch dBBranchStore;

    public HelixBranch(DBBranch dBBranch,int branchCount)
    {
        dBBranchStore = dBBranch;
        position = new Vector2((branchCount - int.Parse(dBBranch.id)) * 2, 0);
    }
}
