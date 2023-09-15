using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngryTreeController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject rightBoundary;
    [SerializeField] private GameObject leftBoundary;
    public Vector3 originalPosition;
    public Quaternion originalRotation;

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        speed = 5;
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    public void goToOriginalPosition()
    {
        transform.position = originalPosition;
        transform.rotation = originalRotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == rightBoundary || other.gameObject == leftBoundary)
        {
            transform.Rotate(Vector3.up, 180);
        }
    }
}
