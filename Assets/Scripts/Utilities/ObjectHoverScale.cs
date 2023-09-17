using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHoverScale : MonoBehaviour
{
    Vector3 originalScale;

    float targetScaleMultiplicator = 1f;

    // Start is called before the first frame update
    void Start()
    {
        originalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.LerpUnclamped(transform.localScale, originalScale * targetScaleMultiplicator, Time.deltaTime * 10);
    }

    private void OnMouseEnter()
    {
        targetScaleMultiplicator = 2f;
    }

    private void OnMouseExit()
    {
        targetScaleMultiplicator = 1f;
    }
}
