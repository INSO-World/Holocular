using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public interface IFileStructureElement
{
    string Name { get; set; }
    bool Changed { get; set; }
}

