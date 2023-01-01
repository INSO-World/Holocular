using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControll : MonoBehaviour
{

    float rotationX = 0F;
    float rotationY = 0F;
    Quaternion originalRotation;

    // Start is called before the first frame update
    void Start()
    {
        originalRotation = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
        {
            if (Input.GetMouseButtonDown(0))
            {
                transform.LookAt(hit.transform.position);
            }
            if (Main.debugMode)
            {
                RuntimeDebug.DrawLine(transform.position - Vector3.up, hit.transform.position, Color.green); ;
                Debug.Log("hit");
            }
        }

        if (Input.GetMouseButton(1))
        {
   
            rotationX += Input.GetAxis("Mouse X") * Main.mouseSensitivity;
            rotationY += Input.GetAxis("Mouse Y") * Main.mouseSensitivity;
            Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
            Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, -Vector3.right);
            transform.localRotation = originalRotation * xQuaternion * yQuaternion;
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += transform.forward * Main.moveSpeed *Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= transform.forward * Main.moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= transform.right * Main.moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += transform.right * Main.moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            transform.position += Vector3.up * Main.moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            transform.position -= Vector3.up * Main.moveSpeed * Time.deltaTime;
        }
        if(Input.mouseScrollDelta.y != 0)
        {
            if (Input.GetKey(KeyCode.LeftAlt))
            {
                Main.mouseSensitivity -= (int)Input.mouseScrollDelta.y;
                if (Main.mouseSensitivity >= 0)
                {
                    Main.mouseSensitivity -= (int)Input.mouseScrollDelta.y;
                }
                else
                {
                    Main.mouseSensitivity = 0;
                }
            }
            else
            { 
                Main.moveSpeed -= (int)Input.mouseScrollDelta.y;
                if (Main.moveSpeed >= 0)
                {
                    Main.moveSpeed -= (int)Input.mouseScrollDelta.y;
                }
                else
                {
                    Main.moveSpeed = 0;
                }
            }
        }
    }
}
