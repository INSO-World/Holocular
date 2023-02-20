using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DBFiles
{
    public DBFile[] files;

}

[System.Serializable]
public class DBFile
{
    public string _id;
    public string webUrl;
    public string path;
    public int maxLength;
}
