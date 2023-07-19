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

    // Update is called once per frame
    void Update()
    {
        /*if (GlobalSettings.showAuthorColors != colorUpdate)
        {
            ChangeColor();

            colorUpdate = GlobalSettings.showAuthorColors;
        }*/
    }

    private void ChangeColor()
    {
        if (GlobalSettings.showAuthorColors)
        {
            mat.color = Helix.stakeholders[authorSighnature].colorStore;
        }
        else
        {
            mat.color = Main.fileDefaultColor;
        }
    }
}
