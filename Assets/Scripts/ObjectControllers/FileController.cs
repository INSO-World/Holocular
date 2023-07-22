using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.Handles;

public class FileController : MonoBehaviour
{
    public string authorSighnature = "";

    public GameObject visual;

    private Material mat;

    private UnityAction updateFileColorListener;



    // Start is called before the first frame update
    void Start()
    {
        mat = visual.GetComponent<Renderer>().material;
        ChangeColor();
        updateFileColorListener = new UnityAction(ChangeColor);
        EventManager.StartListening("updateFileColor", updateFileColorListener);

    }

    private void ChangeColor()
    {
        if (GlobalSettings.showAuthorColors)
        {
            if (GlobalSettings.highlightedAuthor == null || GlobalSettings.highlightedAuthor == authorSighnature)
            {
                mat.color = Helix.stakeholders[authorSighnature].colorStore;
            }
            else
            {
                mat.color = Main.fileDeSelectedColor;
            }
        }
        else
        {
            mat.color = Main.fileDefaultColor;
        }
    }
}
