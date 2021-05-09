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

        if (health <= 0 && !isDead)
        {
            if (CompareTag("Ally"))
            {
                gameManager.life += -1;
            }
            dieSound.Play();
            Die();
        }
    }

    public void Die()
    {
        if (!gameManager.isEndedGame && !isDead)
        {
            Destroy(gameObject, 1f);
        }

        isDead = true;
    }
}
