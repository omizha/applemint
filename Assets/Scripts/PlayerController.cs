using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public Rigidbody playerRigidbody;
    public float speed = 8f;
    public float jumpForce = 300f;
    public int jumpLimit = 2;

    int jumpCount = 0;
    float jumpTimer = 0;

    public Camera camera;
    public float mouseSensitivity;

    float xRotation = 0f;

    public float stepRate = 3f;
    float nextTimeToStep;

    public AudioSource footstepSound;
    Vector3 prevPosition;

    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        prevPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float inputHorizontal = Input.GetAxis("Horizontal");
        float inputVertical = Input.GetAxis("Vertical");

        float inputMouseHorizontal = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float inputMouseVertical = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation += -inputMouseVertical;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Quaternion
        // 회전을 나타내는 데 사용
        // Eular
        // z축을 중심으로 z도, x축을 중심으로 x도,
        // y축을 중심으로 y도 회전하는 회전을 반환합니다.
        // 그 순서대로 적용됩니다.
        // localRotation
        // 부모의 변환 회전을 기준으로 한 변환의 회전입니다.
        camera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(0f, inputMouseHorizontal, 0f);

        Vector3 moveDirection = new Vector3(inputHorizontal, 0f, inputVertical);

        if (!gameManager.isEndedGame)
        {
            // Smoothing 필터링을 적용하지 않고 axisName으로 식별되는 가상 축의 값을 반환합니다.
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                transform.Translate(moveDirection * speed * Time.deltaTime);
            }

            jumpTimer += Time.deltaTime;

            if (IsOnGround())
            {
                JumpInit();
            }

            if (Input.GetButtonDown("Jump") && IsJumpable())
            {
                JumpAction();
            }

            if (Time.time >= nextTimeToStep && IsOnGround() && IsMoving())
            {
                nextTimeToStep = Time.time + 1f / stepRate;
                footstepSound.PlayOneShot(footstepSound.clip);
            }

            prevPosition = transform.position;
        }
    }

    bool IsMoving()
    {
        float distance = Vector3.Distance(transform.position, prevPosition);

        return distance > 0.01f;
    }

    bool IsJumpable()
    {
        return jumpCount < jumpLimit && (jumpTimer == 0 || jumpTimer > 0.3f);
    }

    bool IsOnGround()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }

    void JumpInit()
    {
        jumpTimer = 0;
        jumpCount = 0;
    }

    void JumpAction()
    {
        jumpTimer = 0;
        jumpCount++;
        playerRigidbody.AddForce(Vector3.up * jumpForce);
    }
}
