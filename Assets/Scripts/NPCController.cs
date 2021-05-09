using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    public Rigidbody rigidbody;
    public TargetNPC target;
    public float speed = 10f;

    public GameObject player;
    public GameManager gameManager;

    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        gameManager = GameObject.FindGameObjectsWithTag("GameManager")[0].GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        if (target.isDead)
        {
            rigidbody.useGravity = true;
            return;
        }
    }

    void Move()
    {
        transform.Translate(new Vector3(0f, 1f, 0f) * speed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Wall") && CompareTag("Enemy"))
        {
            gameManager.life += -1;
        }

        if (other.gameObject.CompareTag("Wall") && CompareTag("Ally"))
        {
            Debug.Log("score : " + gameManager.score);
            gameManager.score += 1;
        }

        target.Die();
    }
}
