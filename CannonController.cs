using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    [SerializeField] private GameObject cannonBall;
    private Vector3 cannonBallDirection;
    public Coroutine ballCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        cannonBallDirection = new Vector3(0, 1, 0);
        ballCoroutine = StartCoroutine(CreateCannonBall());
        //StartCoroutine(CreateCannonBall());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator CreateCannonBall() {
        while (!GameController.isPaused)
        {
            yield return new WaitForSeconds(2);
            if (transform.rotation.y == 0)
            {
                cannonBallDirection.x = 0;
                cannonBallDirection.z = 1;
            }
            if (transform.rotation.y == -90)
            {
                cannonBallDirection.z = 0;
                cannonBallDirection.x = 1;
            }
            if (transform.rotation.y == 90)
            {
                cannonBallDirection.z = 0;
                cannonBallDirection.x = -2;
            }
            Instantiate(cannonBall, transform.position + cannonBallDirection, transform.rotation);
        }
    }
}
