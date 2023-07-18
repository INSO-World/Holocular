using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class HelixStakeholder
{

    public DBStakeholder dBStakeholderStore;
    public Color colorStore;

    public HelixStakeholder(DBStakeholder dBStakeholder, Color color)
    {
        dBStakeholderStore = dBStakeholder;
        colorStore = color;
    }
}

