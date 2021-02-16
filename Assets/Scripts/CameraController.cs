using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraState { Follow, Aim };
public class CameraController : MonoBehaviour
{
    public Transform targetCharacter;
    public Transform cameraTarget;
    public float rotationPower;
    public float minClamp;
    public float maxClamp;
    public GameObject followCamera;
    public GameObject aimCamera;

    private Quaternion targetRotation;
    void Start() {
        targetRotation = cameraTarget.localRotation;
    }

    void LateUpdate() {
        // Handle camera movement
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        targetRotation.x -= mouseY * rotationPower;
        targetRotation.y += mouseX * rotationPower;
        targetRotation.x = Mathf.Clamp(targetRotation.x, minClamp, maxClamp);
        cameraTarget.localRotation = Quaternion.Euler(targetRotation.x, targetRotation.y, targetRotation.z);

        targetCharacter.rotation = Quaternion.Euler(0, targetRotation.y, 0);
        cameraTarget.localEulerAngles = new Vector3(targetRotation.x, 0, 0);

        // Temporary
        if (Input.GetMouseButtonDown(1)) {
            aimCamera.SetActive(true);
            followCamera.SetActive(false);
        }
        if (Input.GetMouseButtonUp(1)) {
            aimCamera.SetActive(false);
            followCamera.SetActive(true);
        }
    }
}
