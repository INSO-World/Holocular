using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class HelixCommitCommitRelation
{
    public DBCommitComitRelation dBCommitsCommitsStore;
    public DBCommit dbCommitStore;

    public HelixCommitCommitRelation(DBCommitComitRelation dBCommitsCommits, HelixCommit commit)
    {
        dBCommitsCommitsStore = dBCommitsCommits;
        dbCommitStore = commit.dBCommitStore;
    }
}

