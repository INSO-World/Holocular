using UnityEngine;
using UnityEngine.Events;

public class FolderController : MonoBehaviour
{

    private UnityAction updateFolderListener;
    private LineRenderer lr;
    public string fullPath = "";

    // Start is called before the first frame update
    void Start()
    {
        lr = gameObject.GetComponent<LineRenderer>();
        UpdateFolder();
        updateFolderListener = new UnityAction(UpdateFolder);
        EventManager.StartListening("updateFolders", updateFolderListener);
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
