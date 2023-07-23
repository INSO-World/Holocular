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
    public static bool commitPlacementMode = false; //linear(false)/timeBased(true)

    public static float commitDistanceMultiplicator = 5f;

    public static string highlightedAuthor = null;

    bool showAuthorUpdate = showAuthorColors;
    bool commitPlacementModeUpdate = commitPlacementMode;
    string lasthighlightedAuthor = highlightedAuthor;
    float lastCommitDistanceMultiplicator = commitDistanceMultiplicator;



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

        if (commitPlacementModeUpdate != commitPlacementMode)
        {
            EventManager.TriggerEvent("updateCommitDistance");
            Main.helix.UpdateConnectionTreeDistance();
            commitPlacementModeUpdate = commitPlacementMode;
        }

        if (lastCommitDistanceMultiplicator != commitDistanceMultiplicator)
        {
            EventManager.TriggerEvent("updateCommitDistance");
            Main.helix.UpdateConnectionTreeDistance();
            lastCommitDistanceMultiplicator = commitDistanceMultiplicator;
        }
    }

}