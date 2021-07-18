using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSCamera : MonoBehaviour
{
    GameObject player;

    Vector3 offset = new Vector3(0, 1, -1);
    public float distance = 7.0f;   // currentZoom보다 명확한 이름으로 변경
    public float minZoom = 5.0f;
    public float maxZoom = 10.0f;
    public float sensitivity = 100f; // 마우스 감도

    float x;
    float y;

    // Start is called before the first frame update
    void Start()
    {
        // 플레이어 태그를 가진 게임오브젝트(=플레이어)를 찾아서 넣기
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        RotateAround();
        CalculateZoom();
    }

    // 카메라 확대율 계산
    void CalculateZoom()
    {
        // 마우스 줌 인/아웃
        distance -= Input.GetAxis("Mouse ScrollWheel");

        // 줌 최소/최대 제한
        // Clamp함수 : 최대/최소값을 설정해주고 제한
        distance = Mathf.Clamp(distance, minZoom, maxZoom);
    }

    // 마우스 움직임에 따라 플레이어 주위를 공전하는 카메라
    void RotateAround()
    {
        // 마우스의 위치를 받아오기
        x += Input.GetAxis("Mouse X") * sensitivity * 0.01f; // 마우스 좌우 움직임 감지
        y -= Input.GetAxis("Mouse Y") * sensitivity * 0.01f; // 마우스 상하 움직임 감지

        // 카메라 높이값(끄덕끄덕각도) 제어
        if (y < 0)  // 바닥을 뚫지 않게
            y = 0;
        if (y > 50) // Top View(정수리로 내려보기)로 하고 싶다면 90으로 바꾸기
            y = 50;

        // player.transform을 자주 사용할건데 너무 길어서 치환 => target
        Transform target = player.transform;

        // 카메라가 회전할 각도와 이동할 위치 계산
        Quaternion angle = Quaternion.Euler(y, x, 0);
        Vector3 destination = angle * (Vector3.back * distance) + target.position + Vector3.zero;

        transform.rotation = angle;             // 카메라 각도 조정
        transform.position = destination;   // 카메라 위치 조정
    }
}
