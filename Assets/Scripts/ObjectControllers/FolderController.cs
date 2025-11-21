using UnityEngine;
using UnityEngine.Events;

public class FolderController : MonoBehaviour
{

    private UnityAction updateFolderListener;
    private UnityAction updateFolderColorListener;
    private LineRenderer lr;
    public string fullPath = "";
    public Color ringColor = Color.clear;
    public Color ringColorLight = Color.clear;

    // Start is called before the first frame update
    void Start()
    {
        lr = gameObject.GetComponent<LineRenderer>();
        UpdateFolder();
        updateFolderListener = new UnityAction(UpdateFolder);
        updateFolderColorListener = new UnityAction(ChangeColor);
        EventManager.StartListening("updateFolders", updateFolderListener);
        EventManager.StartListening("switchDarkLightMode", updateFolderColorListener);
    }

    private void ChangeColor()
    {
        if (Main.darkLightMode)
        {
            lr.SetColors(ringColorLight,ringColorLight);
        }
        else
        {
            lr.SetColors(ringColor,ringColor);
        }
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
