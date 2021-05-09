using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool isEndedGame = false;
    public float playTime;

    public GameObject crossHairGUI;
    public GameObject endGUI;

    public GameObject[] lifeObject;
    public int life = 3;

    public GameObject[] number0x;
    public GameObject[] numberx0;
    public int score = 0;

    public GameObject textWin;
    public GameObject textLose;

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name != "Intro")
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (isEndedGame)
        {
            case true:
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                crossHairGUI.SetActive(false);
                endGUI.SetActive(true);

                CheckResult();
                return;

            case false:
                playTime += Time.deltaTime;
                break;
        }

        CheckLife();
        DrawLife();

        CheckScore();
        DrawScore();
    }

    void DrawLife()
    {
        for (int i = 0; i < lifeObject.Length; i++)
        {
            lifeObject[i].SetActive(i + 1 <= life);
        }
    }

    void DrawScore()
    {
        for (int i = 0; i < number0x.Length; i++)
        {
            number0x[i].SetActive(score / 10 == i);
        }

        for (int i = 0; i < numberx0.Length; i++)
        {
            numberx0[i].SetActive(score % 10 == i);
        }
    }

    void CheckLife()
    {
        if (life <= 0)
        {
            isEndedGame = true;
        }
    }

    void CheckScore()
    {
        if (score >= 99)
        {
            score = 99;
            isEndedGame = true;
        }
    }

    void CheckResult()
    {
        switch (score >= 99)
        {
            case true:
                textWin.SetActive(true);
                break;
            case false:
                textLose.SetActive(true);
                break;
        }
    }

    public void RestartGame(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
