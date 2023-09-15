using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    public GameObject player;
    public static float timeLeft;
    public TextMeshProUGUI HPText, timeText;
    public GameObject menu, winUI, gameOverUI, restartWholeGameUI;
    public PlayerController playerController;
    public CameraController cameraController;
    public ExtraHPController[] extraHPControllers;
    public CannonController[] cannonControllers;
    public AngryTreeController[] angryTreeControllers;
    private BombController[] bombControllers;
    private CannonBallController[] cannonBallControllers;
    //private SlimeAttackController[] slimeControllers;
    public static bool isPaused;
    public static bool isGameOver;
    public int levelToLoad;

    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
        isGameOver = false;
        PlayerController.hp = 100;
        timeLeft = 180;
        levelToLoad = 1;
        winUI.SetActive(false);
        gameOverUI.SetActive(false);
        menu.SetActive(false);
        restartWholeGameUI.SetActive(false);
        HPText.SetText("HP: " + PlayerController.hp);
        timeText.SetText("Time: " + timeLeft);
        playerController = GameObject.FindObjectOfType<PlayerController>();
        cameraController = GameObject.FindObjectOfType<CameraController>();
        extraHPControllers = GameObject.FindObjectsOfType<ExtraHPController>();
        cannonControllers = GameObject.FindObjectsOfType<CannonController>();
        angryTreeControllers = GameObject.FindObjectsOfType<AngryTreeController>();
        bombControllers = GameObject.FindObjectsOfType<BombController>();
        cannonBallControllers = GameObject.FindObjectsOfType<CannonBallController>();
        //slimeControllers = GameObject.FindObjectsOfType<SlimeAttackController>();
        StartCoroutine(Countdown());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !menu.activeInHierarchy)
        {
            menu.SetActive(true);
            Cursor.visible = true;
            cameraController.enabled = false;
            Cursor.lockState = CursorLockMode.Confined;
        }

        else if (Input.GetKeyDown(KeyCode.Q) && menu.activeInHierarchy && !isPaused)
        {
            menu.SetActive(false);
            Cursor.visible = false;
            cameraController.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public bool IsGameOver()
    {
        return PlayerController.hp <= 0;
    }

    public void Win()
    {
        winUI.SetActive(true);
        StopAllCoroutines();
        Debug.Log("You WON! :D");
        StartCoroutine(LoadNextLevel());
        isGameOver = true;
    }

    public void GameOver()
    {
        isPaused = true;
        HPText.SetText("");
        gameOverUI.SetActive(true);
        Cursor.visible = true;
        cameraController.enabled = false;
        Cursor.lockState = CursorLockMode.Confined;
        StopAllCoroutines();
    }

    public void Restart()
    {
        Enable();
        winUI.SetActive(false);
        gameOverUI.SetActive(false);
        menu.SetActive(false);
        timeLeft = 180;
        timeText.SetText("Time: " + timeLeft);
        PlayerController.hp = 100;
        HPText.SetText("HP: " + PlayerController.hp);
        GameObject[] cannonBalls = GameObject.FindGameObjectsWithTag("CannonBall");
        foreach (var cannonBall in cannonBalls)
        {
            Destroy(cannonBall.gameObject);
        }
        GameObject[] bombs = FindInActiveObjectsByTag("Bomb");
        foreach (var bomb in bombs)
        {
            bomb.gameObject.SetActive(true);
        }
        Cursor.visible = false;
        cameraController.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        resetPositions();
        GameObject[] firstAidKits = FindInActiveObjectsByTag("ExtraHP");
        foreach (var firstAidKit in firstAidKits)
        {
            firstAidKit.SetActive(true);
        }
        if (isPaused)
        {
            StartCoroutine(Countdown());
        }

        if (isPaused)
        {
            isPaused = false;
            Debug.Log("Restart: " + isPaused);
            foreach (var cannonController in cannonControllers)
            {
                cannonController.StartCoroutine(cannonController.CreateCannonBall());
            }
        }
        playerController.animator.SetBool("Death", false);
        isGameOver = false;
    }

    public void RestartWholeGame()
    {
        restartWholeGameUI.SetActive(false);
        SceneManager.LoadScene("FirstLevel");
        Restart();
        restartWholeGameUI.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        cameraController.enabled = true;
        playerController.animator.SetBool("Death", false);
        isGameOver = false;
    }

    public void Continue()
    {
        menu.SetActive(false);
        Cursor.visible = false;
        cameraController.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        if (isPaused)
        {
            StartCoroutine(Countdown());
            Enable();
            isPaused = false;
            foreach (var cannonController in cannonControllers)
            {
                cannonController.StartCoroutine(cannonController.CreateCannonBall());
            }
        }
    }

    public void Pause()
    {
        if (!isPaused)
        {
            isPaused = true;
            StopAllCoroutines();
            Disable();
        }
    }

    public void Enable()
    {
        foreach (var extraHPController in extraHPControllers)
        {
            extraHPController.enabled = true;
        }
        /*foreach (var cannonController in cannonControllers)
        {
            //cannonController.enabled = true;
            if (isPaused)
            {
                cannonController.StartCoroutine(cannonController.CreateCannonBall());
                //cannonController.ballCoroutine = StartCoroutine(cannonController.CreateCannonBall());
            }
        }*/
        foreach (var angryTreeController in angryTreeControllers)
        {
            angryTreeController.enabled = true;
        }
        foreach (var bombController in bombControllers)
        {
            bombController.enabled = true;
        }
    }

    public void Disable()
    {
        foreach (var extraHPController in extraHPControllers)
        {
            extraHPController.enabled = false;
        }
        /*foreach (var cannonController in cannonControllers)
        {
            cannonController.StopCoroutine(cannonController.CreateCannonBall());
        }*/
        foreach (var angryTreeController in angryTreeControllers)
        {
            angryTreeController.enabled = false;
        }
        foreach (var bombController in bombControllers)
        {
            bombController.enabled = false;
        }
    }

    public void resetPositions()
    {
        player.GetComponent<PlayerController>().goToOriginalPosition();
        foreach (var angryTreeController in angryTreeControllers)
        {
            angryTreeController.goToOriginalPosition();
        }
        foreach (var bombController in bombControllers)
        {
            bombController.goToOriginalPosition();

        }
        /*foreach (var slimeController in slimeControllers)
        {
            slimeController.goToOriginalPosition();
        }*/
    }

    IEnumerator Countdown()
    {
        while (timeLeft > 0)
        {
            yield return new WaitForSeconds(1);
            timeLeft--;
            timeText.SetText("Time: " + timeLeft);
        }
        GameOver();
    }

    IEnumerator LoadNextLevel()
    {
        yield return new WaitForSeconds(2f);
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(0))
        {
            SceneManager.LoadScene(levelToLoad);
        } else
        {
            winUI.SetActive(false);
            restartWholeGameUI.SetActive(true);
            Cursor.visible = true;
            cameraController.enabled = false;
            Cursor.lockState = CursorLockMode.Confined;

        }
    }

    private GameObject[] FindInActiveObjectsByTag(string tag)
    {
        List<GameObject> validTransforms = new List<GameObject>();
        Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].hideFlags == HideFlags.None)
            {
                if (objs[i].gameObject.CompareTag(tag))
                {
                    validTransforms.Add(objs[i].gameObject);
                }
            }
        }
        return validTransforms.ToArray();
    }
}
