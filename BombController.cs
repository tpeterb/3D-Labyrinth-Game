using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    [SerializeField] private float speed;
    public GameObject leftBoundary;
    public GameObject rightBoundary;
    public Animator animator;
    public Vector3 originalPosition;
    public Quaternion originalRotation;

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        speed = 6;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == leftBoundary || other.gameObject == rightBoundary)
        {
            transform.Rotate(Vector3.up, 180);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetTrigger("attack01");
            /*StartCoroutine(deactivateBomb());
            StopCoroutine(deactivateBomb());*/
        }
    }

    public void goToOriginalPosition()
    {
        transform.position = originalPosition;
        transform.rotation = originalRotation;
    }

    IEnumerator deactivateBomb()
    {
        yield return new WaitForSeconds(1.5f);
        gameObject.SetActive(false);
    }
}
