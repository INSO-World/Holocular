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

        int branchRing = (branchCount - int.Parse(dBBranch.id)) / 3; 
        float positionOnBranchRing = (branchCount - int.Parse(dBBranch.id)) % 3 + (1.0f * branchRing * 2 / branchCount);

        float x = Main.helixBranchOffset * branchRing * Mathf.Cos(positionOnBranchRing * (2 * Mathf.PI) / 3);
        float y = Main.helixBranchOffset * branchRing * Mathf.Sin(positionOnBranchRing * (2 * Mathf.PI) / 3);


        position = new Vector2(x, y);
    }
}
