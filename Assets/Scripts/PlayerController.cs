using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public Rigidbody playerRigidbody;

    // 플레이어의 속도
    public float speed = 8f;
    // 점프력
    public float jumpForce = 300f;
    // 점프 횟수 제한입니다. (값-1)로 적용됩니다.
    public int jumpLimit = 2;

    // 점프 횟수
    int jumpCount = 0;

    // 점프한지 경과된 시간입니다.
    float jumpTimer = 0;

    public Camera camera;

    // 마우스 민감도
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
        // 플레이어 이동을 위한 키 입력 받기
        float inputHorizontal = Input.GetAxis("Horizontal");
        float inputVertical = Input.GetAxis("Vertical");

        // ----------- 마우스에 따른 플레이어 기준 카메라 회전 구현 ---------------
        float inputMouseHorizontal = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float inputMouseVertical = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation += -inputMouseVertical;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Quaternion : 회전을 나타내는 데 사용
        // Eular
        // z축을 중심으로 z도, x축을 중심으로 x도,
        // y축을 중심으로 y도 회전하는 회전을 반환합니다.
        // 그 순서대로 적용됩니다.
        // localRotation : 부모의 변환 회전을 기준으로 한 변환의 회전입니다.
        camera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(0f, inputMouseHorizontal, 0f);
        // ---------------------------------------------------------------------

        Vector3 moveDirection = new Vector3(inputHorizontal, 0f, inputVertical);

        if (!gameManager.isEndedGame)
        {
            // Smoothing 필터링을 적용하지 않고 axisName으로 식별되는 가상 축의 값을 반환합니다.
            // 플레이어 이동 키 입력을 받았는지 확인합니다.
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                // 플레이어를 이동시킵니다.
                transform.Translate(moveDirection * speed * Time.deltaTime);
            }

            jumpTimer += Time.deltaTime;

            // 땅에 있으면 점프를 할 수 있도록 합니다.
            if (IsOnGround())
            {
                JumpInit();
            }

            // 점프가 가능한 상태일 때 점프키를 누르면 점프합니다.
            if (Input.GetButtonDown("Jump") && IsJumpable())
            {
                JumpAction();
            }

            // 땅에서 이동할 시, 발걸음 소리를 재생합니다.
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
