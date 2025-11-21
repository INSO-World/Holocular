using UnityEngine;
using UnityEngine.Events;

public class CommitController : MonoBehaviour
{
    public Vector3 positionLinear = new Vector3(0, 0, 0);
    public Vector3 positionTime = new Vector3(0, 0, 0);
    public HelixCommit commit;
    public GameObject commitVisual;
    private UnityAction updateCommitDistanceListener;
    private UnityAction updateCommitColorListener;

    // Start is called before the first frame update
    void Start()
    {
        UpdateCommitDistance();
        updateCommitDistanceListener = new UnityAction(UpdateCommitDistance);
        updateCommitColorListener = new UnityAction(ChangeColor);
        EventManager.StartListening("updateCommitDistance", updateCommitDistanceListener);
        EventManager.StartListening("switchDarkLightMode", updateCommitColorListener);
    }

    private void ChangeColor()
    {
        if (Main.darkLightMode)
        {
            commitVisual.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.black;
        }
        else
        {
            commitVisual.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.white;
        }
    }

    private void UpdateCommitDistance()
    {
        if (GlobalSettings.commitPlacementMode)
        {
            transform.position = new Vector3(positionTime.x, positionTime.y, positionTime.z * GlobalSettings.commitDistanceMultiplicator);
        }
        else
        {
            transform.position = new Vector3(positionLinear.x, positionLinear.y, positionLinear.z * GlobalSettings.commitDistanceMultiplicator);
        }
        Debug.Log(commit.dBCommitStore.sha);
        HelixParticleSystemRenderer.UpdateCommitPosition(commit.dBCommitStore.sha,transform.position);
    }
}
