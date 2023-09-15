using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] private float mouseSensitivity;
    private float mouseX, mouseY;
    private float xRotation;
    [SerializeField] private Transform playerBody;
    private AudioSource audioSource;
    //private AudioClip backgroundMusic;

    // Start is called before the first frame update
    void Start()
    {
        mouseSensitivity = 50;
        Cursor.lockState = CursorLockMode.Locked;
        //audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        xRotation -= mouseY * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -80, 80);
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        playerBody.Rotate(Vector3.up * mouseX * Time.deltaTime);
        //audioSource.PlayOneShot(backgroundMusic);
    }
}
