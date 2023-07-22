using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class GlobalSettings : MonoBehaviour
{
    public static bool debugMode = true;
    public static bool showSettings = true;
    public static bool showAuthorColors = false;
    public static bool showAuthorPalette = false;

    public static int commitDistanceMultiplicator = 5;

    public static string highlightedAuthor = null;

    bool showAuthorUpdate = showAuthorColors;
    string lasthighlightedAuthor = highlightedAuthor;
    int lastCommitDistanceMultiplicator = commitDistanceMultiplicator;



    // Update is called once per frame
    void Update()
    {
        if (showAuthorUpdate != showAuthorColors)
        {
            EventManager.TriggerEvent("updateFileColor");
            showAuthorUpdate = showAuthorColors;
        }

        if (lasthighlightedAuthor != highlightedAuthor)
        {
            EventManager.TriggerEvent("updateFileColor");
            lasthighlightedAuthor = highlightedAuthor;
        }

        if (lastCommitDistanceMultiplicator != commitDistanceMultiplicator)
        {
            EventManager.TriggerEvent("updateCommitDistance");
            Main.helix.UpdateConnectionTreeDistance();
            lastCommitDistanceMultiplicator = commitDistanceMultiplicator;
        }
    }

}