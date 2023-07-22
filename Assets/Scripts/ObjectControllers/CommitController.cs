using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.Handles;

public class CommitController : MonoBehaviour
{
    public Vector3 positionLinear = new Vector3(0, 0, 0);

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
        transform.position = new Vector3(positionLinear.x, positionLinear.y, positionLinear.z * GlobalSettings.commitDistanceMultiplicator);
    }
}
