using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class HelixCommitStakeholderRelation
{
    public DBCommitStakeholderRelation dBCommitStakeholderStore;

    public HelixCommitStakeholderRelation(DBCommitStakeholderRelation dBCommitStakeholder)
    {
        dBCommitStakeholderStore = dBCommitStakeholder;
    }
}

