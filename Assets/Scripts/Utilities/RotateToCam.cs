using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Utilities
{
    public class RotateToCam :MonoBehaviour
    {
        void Start()
        {
            StartCoroutine(UpdateTimer());
        }

        private IEnumerator UpdateTimer()
        {
            while (true)
            {
                transform.LookAt(Main.cameraDolly);
                yield return new WaitForSeconds(10f);
            }
        }

        /*private void Update()
        {
            transform.LookAt(Main.cameraDolly);
        }*/
    }
}