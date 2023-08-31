using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class GlobalSettings : MonoBehaviour
{
    //Windows
    public static bool debugMode = true;
    public static bool showSettings = true;
    public static bool showFileInfo = false;
    public static bool showAuthorPalette = false;
    public static bool showBranchPalette = false;
    public static bool showFileCompare = false;

    //Coloring/Brushing
    public static bool showAuthorColors = false;
    public static bool showBranchColors = false;
    public static bool showOwnershipColors = false;
    public static bool commitPlacementMode = false; //linear(false)/timeBased(true)
    public static bool showFolderRings = true;

    public static bool fileIsSelected = false;

    public static float commitDistanceMultiplicator = 5f;
    public static float fileSize = 2f;

    public static string highlightedAuthor = null;
    public static string highlightedBranch = null;

    public static string folderSearch = "";

    bool lastShowAuthorColors = showAuthorColors;
    bool lastShowBranchColors = showBranchColors;
    bool lastShowOwnerhshipColors = showOwnershipColors;
    bool lastCommitPlacementMode = commitPlacementMode;
    string lasthighlightedAuthor = highlightedAuthor;
    string lasthighlightedBranch = highlightedBranch;
    float lastCommitDistanceMultiplicator = commitDistanceMultiplicator;
    float lastFileSize = fileSize;
    bool lastShowFolderRings = showFolderRings;
    string lastFolderSearch = folderSearch;
    bool lastFileIsSelected = fileIsSelected;


    // Update is called once per frame
    void Update()
    {
        if (lastShowAuthorColors != showAuthorColors)
        {
            if (showAuthorColors)
            {
                showBranchColors = false;
                showOwnershipColors = false;
            }
            EventManager.TriggerEvent("updateFileColor");
            lastShowAuthorColors = showAuthorColors;
        }

        if (lastShowBranchColors != showBranchColors)
        {
            if (showBranchColors)
            {
                showAuthorColors = false;
                showOwnershipColors = false;
            }
            EventManager.TriggerEvent("updateFileColor");
            lastShowBranchColors = showBranchColors;
        }

        if (lastShowOwnerhshipColors != showOwnershipColors)
        {
            if (showOwnershipColors)
            {
                showBranchColors = false;
                showAuthorColors = false;
            }
            EventManager.TriggerEvent("updateFileColor");
            lastShowOwnerhshipColors = showOwnershipColors;
        }

        if (lasthighlightedAuthor != highlightedAuthor)
        {
            EventManager.TriggerEvent("updateFileColor");
            lasthighlightedAuthor = highlightedAuthor;
        }

        if (lasthighlightedBranch != highlightedBranch)
        {
            EventManager.TriggerEvent("updateFileColor");
            lasthighlightedBranch = highlightedBranch;
        }

        if (lastFileIsSelected != fileIsSelected)
        {
            EventManager.TriggerEvent("updateFileColor");
            lastFileIsSelected = fileIsSelected;
        }

        if (lastCommitPlacementMode != commitPlacementMode)
        {
            EventManager.TriggerEvent("updateCommitDistance");
            Main.helix.UpdateConnectionTreeDistance();
            lastCommitPlacementMode = commitPlacementMode;
        }

        if (lastCommitDistanceMultiplicator != commitDistanceMultiplicator)
        {
            EventManager.TriggerEvent("updateCommitDistance");
            Main.helix.UpdateConnectionTreeDistance();
            lastCommitDistanceMultiplicator = commitDistanceMultiplicator;
        }

        if (lastFileSize != fileSize)
        {
            EventManager.TriggerEvent("updateFileSize");
            lastFileSize = fileSize;
        }

        if (lastShowFolderRings != showFolderRings)
        {
            EventManager.TriggerEvent("updateFolders");
            lastShowFolderRings = showFolderRings;
        }

        if (lastFolderSearch != folderSearch)
        {
            EventManager.TriggerEvent("updateFolders");
            lastFolderSearch = folderSearch;
        }
    }

    public static void SelectAuthor(string signature)
    {
        if (highlightedAuthor != signature)
        {
            highlightedAuthor = signature;
            if (!showAuthorColors && !showOwnershipColors)
            {
                showAuthorColors = true;
                showOwnershipColors = false;
            }
        }
        else if (showAuthorColors == true && showOwnershipColors == false && highlightedAuthor == signature)
        {
            showAuthorColors = false;
            showOwnershipColors = true;
        }
        else if (showAuthorColors == false && showOwnershipColors == true)
        {
            showAuthorColors = false;
            showOwnershipColors = false;
            highlightedAuthor = null;
        }
    }

    public static void SelectBanch(string branch)
    {
        highlightedBranch = branch;
        if (showBranchColors == false)
        {
            showBranchColors = true;
        }
    }

}