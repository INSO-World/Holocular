using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientaionIndicatorAlignment : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, 100, 10));
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = new Vector3(0, 0, 0);
    }
}
