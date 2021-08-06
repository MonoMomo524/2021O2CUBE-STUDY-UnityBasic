using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerControl : MonoBehaviour
{
    float speed = 5f;
    Rigidbody rb;

    float jumpForce = 10f;
    private bool isGrounded;

    Camera cam;
    public float rotSpeed = 10f;

    bool isMovable;

    public UnityEvent OnPlayerFall;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
        isMovable = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(isMovable == false)
        {
            return;
        }
        //MovePosition();
        //MoveTranslate();

        // 점프 확인
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        Move();
    }

    // 물리 연산은 FixedUpdate에서 실행하자!
    // 왜? why? : FixedUpdate() 호출 간격이 일정 == frame 속도 관계 없다 == 물리적으로 감지(검사)가 일정
    // 과도하게 Update함수를 호출할 필요가 없어진다!
    private void FixedUpdate()
    {
        // 이동
        //MoveForce();
        //MoveVelocity();
    }

    // 플레이어 순간이동
    void MovePosition()
    {
        this.transform.position = new Vector3(0, 5, 0);
    }

    void MoveTranslate()
    {
        //// 이동 입력
        //float h = Input.GetAxisRaw("Horizontal");   // 좌우 이동
        //float v = Input.GetAxisRaw("Vertical");       // 전후 이동

        //// 속도 대입
        //float translateSpeed = speed * Time.deltaTime;

        //// 움직이기
        //this.transform.Translate(h * translateSpeed, 0, v * translateSpeed);

        // WASD로 이동하기 // KeyCode값을 바꾸면 특정 버튼으로 이동 가능!
        if (Input.GetKey(KeyCode.W))
        {
            this.transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            this.transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            this.transform.Translate(Vector3.back * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            this.transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
    }

    void MoveForce()
    {
        // 이동 입력
        float h = Input.GetAxisRaw("Horizontal");   // 좌우 이동
        float v = Input.GetAxisRaw("Vertical");       // 전후 이동

        // 이동 방향
        Vector3 direction = new Vector3(h, 0, v);

        // 이동(힘 가하기)
        rb.AddForce(direction * speed);
    }

    void MoveVelocity()
    {
        // 이동 입력
        float h = Input.GetAxisRaw("Horizontal");   // 좌우 이동
        float v = Input.GetAxisRaw("Vertical");       // 전후 이동

        // 이동 방향
        Vector3 direction = new Vector3(h, 0, v);
        rb.velocity = direction * speed;
    }

    void Move()
    {
        // Scene View에서 플레이어가 바라보는 방향(정면)으로 플레이어 머리에서 레이저를 쏘는 것으로 보여줌
        // 주의) y값을 반영하면 카메라가 보는 각도로 인해 바닥이나 하늘을 바라보게 되고, 이대로 앞으로 가면 하늘로 승천하거나 땅으로 파묻혀 들어갈 수 있어 y값을 반드시 0으로!
        Debug.DrawRay(this.transform.position + new Vector3(0, 1, 0), new Vector3(cam.transform.forward.x, 0f, cam.transform.forward.z).normalized, Color.blue);

        // 이동 입력
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        // 전방 주시
        // 정규화를 통해 1만큼의 길이(방향값) 구함
        Vector3 lookFoward = new Vector3(cam.transform.forward.x, 0f, cam.transform.forward.z).normalized; // 카메라의 전방 확인
        Vector3 lookRight = new Vector3(cam.transform.right.x, 0f, cam.transform.right.z).normalized;              // 카메라의 오른쪽 확인

        // 캐릭터가 움직일 방향 계산
        Vector3 moveDir = lookFoward * moveInput.y + lookRight * moveInput.x;
        Quaternion rotateFoward = Quaternion.LookRotation(lookFoward);  // 플레어가 앞을 바라봄
        rb.rotation = Quaternion.Slerp(rb.rotation, rotateFoward, rotSpeed * Time.deltaTime);   // 부드럽게 회전
        this.transform.position += moveDir * Time.deltaTime * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            isGrounded = true;
            this.transform.parent = collision.transform;
        }
    }

    //private void OnCollisionStay(Collision collision)
    //{
    //    if(collision.transform.CompareTag("Ground"))
    //    {
    //        isGrounded = true;
    //    }
    //}

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            isGrounded = false;
            this.transform.parent = null;
        }
    }

    public void CannotMove()
    {
        isMovable = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Fall"))
        {
            OnPlayerFall.Invoke();  // OnPlayerFall 이벤트 발동!!
        }
    }
}
