using UnityEngine;
using UnityEngine.Events;

public class FolderController : MonoBehaviour
{

    private UnityAction updateFolderListener;
    private UnityAction updateFolderColorListener;
    private LineRenderer lr;
    public string fullPath = "";
    public float ringColorFactor = 0f;

    // Start is called before the first frame update
    void Start()
    {
        lr = gameObject.GetComponent<LineRenderer>();
        UpdateFolder();
        ChangeColor();
        updateFolderListener = new UnityAction(UpdateFolder);
        updateFolderColorListener = new UnityAction(ChangeColor);
        EventManager.StartListening("updateFolders", updateFolderListener);
        EventManager.StartListening("switchDarkLightMode", updateFolderColorListener);
        EventManager.StartListening("updateFolderOpacity", updateFolderColorListener);
    }

    private void ChangeColor()
    {
        Color ringColor = Color.clear;
        if (Main.darkLightMode)
        {
            ringColor = Color.Lerp(Main.ringColorLight, Main.ringColorLightEnd, ringColorFactor);
        }
        else
        {
            ringColor = Color.Lerp(Main.ringColorDefault, Main.ringColorDefaultEnd, ringColorFactor);
        }
        lr.SetColors(ringColor,ringColor);
    }

    private void UpdateFolder()
    {
        if (GlobalSettings.showFolderRings && (GlobalSettings.folderSearch.Length == 0 || fullPath.StartsWith(GlobalSettings.folderSearch)))
        {
            lr.enabled = true;
        }
        else
        {
            lr.enabled = false;
        }
    }
}
