using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraControll : MonoBehaviour
{
    Transform selectPoint;
    public static Transform mainCamera;
    float selectFocusSpeed = 10f;

    float zoomDistanceToFile = 20f;

    Quaternion targetRotation;
    Vector3 targetPosition;

    // Start is called before the first frame update
    void Start()
    {
        targetRotation = transform.rotation;
        targetPosition = transform.position;
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
                    selectPoint.position = hit.transform.position + -dir * zoomDistanceToFile * GlobalSettings.fileSize;
                    selectPoint.LookAt(hit.transform.position);
                    Main.lastSelectedObject = hit.transform.gameObject;
                    Main.selectedFile = hit.transform.gameObject.GetComponent<FileController>();
                    GlobalSettings.fileIsSelected = true;
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
                    RuntimeDebug.DrawLine(mainCamera.position - Vector3.up, hit.transform.position, Color.green); ;
                }
            }
            else if (hit.transform.name == "Z-Axis +" && Input.GetMouseButtonDown(0))
            {
                targetRotation.eulerAngles = new Vector3(0, 0, 0);
                GlobalSettings.fileIsSelected = false;
            }
            else if (hit.transform.name == "Z-Axis -" && Input.GetMouseButtonDown(0))
            {
                targetRotation.eulerAngles = new Vector3(0, 180, 0);
                GlobalSettings.fileIsSelected = false;
            }
            else if (hit.transform.name == "X-Axis +" && Input.GetMouseButtonDown(0))
            {
                targetRotation.eulerAngles = new Vector3(0, 90, 0);
                GlobalSettings.fileIsSelected = false;
            }
            else if (hit.transform.name == "X-Axis -" && Input.GetMouseButtonDown(0))
            {
                targetRotation.eulerAngles = new Vector3(0, 270, 0);
                GlobalSettings.fileIsSelected = false;
            }
            else if (hit.transform.name == "Y-Axis +" && Input.GetMouseButtonDown(0))
            {
                targetRotation.eulerAngles = new Vector3(270, 0, 270);
                GlobalSettings.fileIsSelected = false;
            }
            else if (hit.transform.name == "Y-Axis -" && Input.GetMouseButtonDown(0))
            {
                targetRotation.eulerAngles = new Vector3(90, 0, 270);
                GlobalSettings.fileIsSelected = false;
            }
            else if (hit.transform.name == "Anchor" && Input.GetMouseButtonDown(0))
            {
                if (Main.commits != null && Main.commits.commits.Length > 0)
                {
                    HelixCommit lastCommit = Main.helix.commits[Main.commits.commits[Main.commits.commits.Length - 1].sha];
                    targetPosition = new Vector3(0, 0, lastCommit.GetCommitPosition().z * GlobalSettings.commitDistanceMultiplicator / 2);
                    targetPosition -= transform.forward * lastCommit.GetCommitPosition().z * GlobalSettings.commitDistanceMultiplicator * 0.6f;
                }
                GlobalSettings.fileIsSelected = false;
            }
        }
        else
        {
            Main.fileHover = false;
        }



        if (Input.GetMouseButton(1))
        {
            targetRotation = Quaternion.Euler(targetRotation.eulerAngles.x - Input.GetAxis("Mouse Y") * Main.mouseSensitivity, targetRotation.eulerAngles.y + Input.GetAxis("Mouse X") * Main.mouseSensitivity, targetRotation.eulerAngles.z);
            GlobalSettings.fileIsSelected = false;
        }
        if (Input.GetKey(KeyCode.W))
        {
            targetPosition += transform.forward * Main.moveSpeed * Time.deltaTime;
            GlobalSettings.fileIsSelected = false;
        }
        if (Input.GetKey(KeyCode.S))
        {
            targetPosition -= transform.forward * Main.moveSpeed * Time.deltaTime;
            GlobalSettings.fileIsSelected = false;
        }
        if (Input.GetKey(KeyCode.A))
        {
            targetPosition -= transform.right * Main.moveSpeed * Time.deltaTime;
            GlobalSettings.fileIsSelected = false;
        }
        if (Input.GetKey(KeyCode.D))
        {
            targetPosition += transform.right * Main.moveSpeed * Time.deltaTime;
            GlobalSettings.fileIsSelected = false;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            targetPosition += Vector3.up * Main.moveSpeed * Time.deltaTime;
            GlobalSettings.fileIsSelected = false;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            targetPosition -= Vector3.up * Main.moveSpeed * Time.deltaTime;
            GlobalSettings.fileIsSelected = false;
        }
        if (Input.mouseScrollDelta.y != 0)
        {
            if (!(Input.GetKey(KeyCode.H) && (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) || Input.GetKey(KeyCode.V)))
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
            if (Input.GetKey(KeyCode.V))
            {
                Main.viewDistance -= (int)Input.mouseScrollDelta.y * 100;
                if (Main.viewDistance >= 100)
                {
                    Main.viewDistance -= (int)Input.mouseScrollDelta.y;
                }
                else
                {
                    Main.viewDistance = 100;
                }
                mainCamera.GetComponent<Camera>().farClipPlane = Main.viewDistance;
            }
        }

        if (GlobalSettings.fileIsSelected)
        {
            mainCamera.position = Vector3.MoveTowards(mainCamera.position, selectPoint.position, selectFocusSpeed * Time.deltaTime * Vector3.Distance(mainCamera.position, selectPoint.position));
            mainCamera.rotation = Quaternion.Lerp(mainCamera.rotation, selectPoint.rotation, selectFocusSpeed * Time.deltaTime);

        }
        else
        {
            mainCamera.localPosition = Vector3.MoveTowards(mainCamera.localPosition, new Vector3(0, 0, 0), selectFocusSpeed * Time.deltaTime * Vector3.Distance(mainCamera.localPosition, new Vector3(0, 0, 0)));
            mainCamera.rotation = Quaternion.Lerp(mainCamera.rotation, transform.rotation, selectFocusSpeed * Time.deltaTime);
        }

        transform.rotation = Quaternion.LerpUnclamped(transform.rotation, targetRotation, Time.deltaTime * 10);
        transform.position = Vector3.LerpUnclamped(transform.position, targetPosition, Time.deltaTime * 10);
    }
}
