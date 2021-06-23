using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetNPC : MonoBehaviour
{
    public float health = 100f;
    public bool isDead = false;
    public AudioSource dieSound;

    GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.FindGameObjectsWithTag("GameManager")[0].GetComponent<GameManager>();
        // gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }

    public void TakeDamage(float amount)
    {
        health += -amount;

        // 총에 맞았는지, 죽어있는지 확인합니다.
        if (health <= 0 && !isDead)
        {
            // 애플민트 소녀가 총맞으면 life가 -1 깎입니다.
            if (CompareTag("Ally"))
            {
                gameManager.life += -1;
            }
            // 죽는 소리를 재생합니다.
            dieSound.Play();
            Die();
        }
    }

    public void Die()
    {
        // 게임이 끝나지 않았거나, 죽지 않았으면 1초뒤에 죽입니다.
        if (!gameManager.isEndedGame && !isDead)
        {
            Destroy(gameObject, 1f);
        }

        isDead = true;
    }
}
