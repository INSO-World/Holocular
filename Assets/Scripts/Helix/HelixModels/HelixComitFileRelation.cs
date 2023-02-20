using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class HelixComitFileRelation
{
	public DBCommitFileRelation dBCommitsFilesStore;

	public HelixComitFileRelation(DBCommitFileRelation dBCommitsFiles)
	{
		dBCommitsFilesStore = dBCommitsFiles;
	}
}

