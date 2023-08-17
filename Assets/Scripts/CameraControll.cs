using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControll : MonoBehaviour
{

    float rotationX = 0F;
    float rotationY = 0F;
    Quaternion originalRotation;
    Transform selectPoint;
    Transform mainCamera;
    bool selected = false;
    float selectFocusSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        originalRotation = transform.localRotation;
        selectPoint = transform.Find("SelectPoint");
        mainCamera = transform.Find("Main Camera");
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
        {
            if (hit.transform.tag == "File")
            {
                if (Input.GetMouseButtonDown(0))
                {


                    Vector3 dir = (hit.transform.position - mainCamera.position).normalized;
                    selectPoint.position = hit.transform.position + -dir * 5 * GlobalSettings.fileSize;
                    selectPoint.LookAt(hit.transform.position);
                    Main.lastSelectedObject = hit.transform.gameObject;
                    Main.selectedFile = hit.transform.gameObject.GetComponent<FileController>();
                    selected = true;
                    if (!GlobalSettings.showFileInfo)
                    {
                        GlobalSettings.showFileInfo = true;
                    }
                    RuntimeDebug.Log(hit.transform.name);
                }
                else
                {
                    Main.hoveredFile = hit.transform.gameObject.GetComponent<FileController>();
                    Main.fileHover = true;
                }
                if (GlobalSettings.debugMode)
                {
                    RuntimeDebug.DrawLine(transform.position - Vector3.up, hit.transform.position, Color.green); ;
                }
            }
        }
        else
        {
            Main.fileHover = false;
        }



        if (Input.GetMouseButton(1))
        {

            rotationX += Input.GetAxis("Mouse X") * Main.mouseSensitivity;
            rotationY += Input.GetAxis("Mouse Y") * Main.mouseSensitivity;
            Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
            Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, -Vector3.right);
            transform.localRotation = originalRotation * xQuaternion * yQuaternion;
            selected = false;
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += transform.forward * Main.moveSpeed * Time.deltaTime;
            selected = false;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= transform.forward * Main.moveSpeed * Time.deltaTime;
            selected = false;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= transform.right * Main.moveSpeed * Time.deltaTime;
            selected = false;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += transform.right * Main.moveSpeed * Time.deltaTime;
            selected = false;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            transform.position += Vector3.up * Main.moveSpeed * Time.deltaTime;
            selected = false;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            transform.position -= Vector3.up * Main.moveSpeed * Time.deltaTime;
            selected = false;
        }
        if (Input.mouseScrollDelta.y != 0)
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

        if (selected)
        {
            mainCamera.position = Vector3.MoveTowards(mainCamera.position, selectPoint.position, selectFocusSpeed * Time.deltaTime * Vector3.Distance(mainCamera.position, selectPoint.position));
            mainCamera.rotation = Quaternion.Lerp(mainCamera.rotation, selectPoint.rotation, selectFocusSpeed * Time.deltaTime);

        }
        else
        {
            mainCamera.localPosition = Vector3.MoveTowards(mainCamera.localPosition, new Vector3(0, 0, 0), selectFocusSpeed * Time.deltaTime * Vector3.Distance(mainCamera.localPosition, new Vector3(0, 0, 0)));
            mainCamera.rotation = Quaternion.Lerp(mainCamera.rotation, transform.rotation, selectFocusSpeed * Time.deltaTime);
        }
    }
}
