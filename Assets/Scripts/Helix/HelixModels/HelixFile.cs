using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class HelixFile
{

	public DBFile dBFileStore;
	public GameObject fileObject;

	public HelixFile(DBFile dBFile)
	{
		dBFileStore = dBFile;
	}
}

