using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float sensitivityX = 2f;
    public float sensitivityY = 2f;
    public float smoothing = 2f; 

    private float rotationX = 0f;
    private float rotationY = 0f;

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        mouseX *= sensitivityX;
        mouseY *= sensitivityY;

        rotationX += mouseX;
        rotationY -= mouseY;

        rotationY = Mathf.Clamp(rotationY, -80f, 80f);

        transform.localRotation = Quaternion.Euler(rotationY, rotationX, 0);

        Camera.main.transform.localRotation = Quaternion.Euler(rotationY, rotationX, 0);
    }
}
