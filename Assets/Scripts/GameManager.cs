using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    public int score;

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

        if (SceneManager.GetActiveScene().name == "Level2")
        {
            Invoke("StopNoiseTransition", 1f);
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

        switch (SceneManager.GetActiveScene().name)
        {
            case "Level1":
                if (score == 50)
                {
                    SceneManager.LoadScene("Level2");
                }
                break;
            case "Level2":
                GameObject noiseSprite = GameObject.FindWithTag("NoiseSprite");
                Color color = noiseSprite.GetComponent<Image>().color;
                color.a = (score - 50) / 100f;
                noiseSprite.GetComponent<Image>().color = color;

                GameObject noiseSound = GameObject.FindGameObjectsWithTag("NoiseSound")[0];
                noiseSound.GetComponent<AudioSource>().volume = (score - 50) * 3 / 100f > 1f ? 1f : (score - 50) * 3 / 100f;

                noiseSound = GameObject.FindGameObjectsWithTag("NoiseSound")[1];
                noiseSound.GetComponent<AudioSource>().volume = (score - 50) * 3 / 100f > 1f ? 1f : (score - 50) * 3 / 100f;

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

    void StopNoiseTransition()
    {
        GameObject.FindWithTag("NoiseTransition").SetActive(false);
    }

    public void RestartGame(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
