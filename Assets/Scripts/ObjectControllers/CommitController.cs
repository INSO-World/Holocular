using UnityEngine;
using UnityEngine.Events;

public class CommitController : MonoBehaviour
{
    public Vector3 positionLinear = new Vector3(0, 0, 0);
    public Vector3 positionTime = new Vector3(0, 0, 0);

    private UnityAction updateCommitDistanceListener;


    // Start is called before the first frame update
    void Start()
    {
        UpdateCommitDistance();
        updateCommitDistanceListener = new UnityAction(UpdateCommitDistance);
        EventManager.StartListening("updateCommitDistance", updateCommitDistanceListener);

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
    }
}
