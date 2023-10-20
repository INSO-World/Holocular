using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DBStakeholders
{
    public DBStakeholder[] stakeholders;
}

[System.Serializable]
public class DBStakeholder
{
    public string _id;
    public string gitSignature;
}