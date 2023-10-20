using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class HelixCommitFileRelation
{
    public DBCommitFileRelation dBCommitsFilesStore;
    public List<HelixCommitFileStakeholderRelation> helixCommitFileStakeholderRelationListStore;

    public HelixCommitFileRelation(DBCommitFileRelation dBCommitsFiles, List<HelixCommitFileStakeholderRelation> helixCommitFileStakeholderRelationList)
    {
        dBCommitsFilesStore = dBCommitsFiles;
        helixCommitFileStakeholderRelationListStore = helixCommitFileStakeholderRelationList;
    }
}

