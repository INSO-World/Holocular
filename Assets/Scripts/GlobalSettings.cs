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

    bool showAuthorUpdate = showAuthorColors;


    // Update is called once per frame
    void Update()
    {
        if (showAuthorColors != showAuthorUpdate)
        {
            EventManager.TriggerEvent("updateFileColor");
            showAuthorUpdate = showAuthorColors;
        }
    }

}