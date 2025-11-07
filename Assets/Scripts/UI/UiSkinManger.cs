using System;
using UnityEngine;
using UnityEngine.Events;

public class UiSkinManger : MonoBehaviour
{
    public GUISkin uiStyle;
    public GUISkin uiStyleLight;
    static public GUISkin sUiStyle;

    private UnityAction updateUiStyleListener;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sUiStyle = uiStyle;
        updateUiStyleListener = new UnityAction(UpdateUiStyle);

        EventManager.StartListening("switchDarkLightMode", updateUiStyleListener);

    }

    private void UpdateUiStyle()
    {
        if (Main.darkLightMode)
        {
            sUiStyle = uiStyleLight;
        }
        else
        {
            sUiStyle = uiStyle;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
