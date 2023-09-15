using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{

    private float horizontalInput;
    private float verticalInput;
    [SerializeField] private float speed;
    private Rigidbody rb;
    [SerializeField] private float jumpForce;
    private bool isOnGround;
    public Animator animator;
    public static float hp;
    public Vector3 originalPosition;
    public Quaternion originalRotation;

    public TextMeshProUGUI HPText;
    public GameController gameController;
    private AudioSource audioSource;
    [SerializeField] private AudioClip cannonBallSound;
    [SerializeField] private AudioClip extraHPSound;

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        speed = 6;
        jumpForce = 6;
        isOnGround = true;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        gameController = GameObject.FindObjectOfType<GameController>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        if (!GameController.isGameOver && !GameController.isPaused)
        {
            transform.Translate(Vector3.forward * speed * verticalInput * Time.deltaTime);
            transform.Translate(Vector3.right * speed * horizontalInput * Time.deltaTime);
        }
        if (verticalInput != 0)
        {
            animator.SetBool("Running", true);
        }

        if (verticalInput == 0)
        {
            animator.SetBool("Running", false);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isOnGround && !GameController.isGameOver && !GameController.isPaused)
        {
            animator.SetTrigger("Jump_trig");
            //animator.SetBool("Running", false);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            animator.SetTrigger("Punch_trig");
        }

    }

    public void goToOriginalPosition()
    {
        transform.position = originalPosition;
        transform.rotation = originalRotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
        }
        if (collision.gameObject.CompareTag("Tree"))
        {
            hp -= 15;
            HPText.SetText("HP: " + hp);
            audioSource.Play();
            //rb.AddForce(Vector3.up * 11.5f, ForceMode.Impulse);
        }
        /*if (collision.gameObject.CompareTag("SmallMonster"))
        {
            hp -= 10;
            HPText.SetText("HP: " + hp);
        }*/

        if (collision.gameObject.CompareTag("Bomb"))
        {
            hp -= 20;
            HPText.SetText("HP: " + hp);
            rb.AddForce(Vector3.up * 11.5f, ForceMode.Impulse);
            audioSource.Play();
        }

        if (collision.gameObject.name == "Goal")
        {
            gameController.Win();
        }

        if (gameController.IsGameOver())
        {
            GameController.isGameOver = true;
            animator.SetBool("Death", true);
            gameController.GameOver();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("CannonBall"))
        {
            hp -= 20;
            HPText.SetText("HP: " + hp);
            Destroy(other.gameObject);
            audioSource.PlayOneShot(cannonBallSound);
        }

        if (other.gameObject.CompareTag("ExtraHP"))
        {
            hp += 5;
            HPText.SetText("HP: " + hp);
            other.gameObject.SetActive(false);
            audioSource.PlayOneShot(extraHPSound);
        }

        if (gameController.IsGameOver())
        {
            GameController.isGameOver = true;
            animator.SetBool("Death", true);
            gameController.GameOver();
        }
    }
}
