using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class HelixCommitFileStakeholderRelation
{
    public DBCommitFileStakeholderRelation dBCommitsFilesStakeholderStore;
    public HelixStakeholder helixStakeholderStore;

    public HelixCommitFileStakeholderRelation(DBCommitFileStakeholderRelation dBCommitsFilesStakeholder, HelixStakeholder helixStakeholder)
    {
        dBCommitsFilesStakeholderStore = dBCommitsFilesStakeholder;
        helixStakeholderStore = helixStakeholder;
    }
}

