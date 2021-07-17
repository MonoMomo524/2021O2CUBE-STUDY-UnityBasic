using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCamera : MonoBehaviour
{
    GameObject player;

    public float sensitivity = 100f; // 마우스 감도
    float rotX;
    float rotY;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = player.transform.position + new Vector3(0, 1.5f, 0);
        RotateCamera();
    }

    void RotateCamera()
    {
        // 마우스의 위치를 받아오기
        float x = Input.GetAxis("Mouse X"); // 마우스 좌우 움직임 감지
        float y = Input.GetAxis("Mouse Y"); // 마우스 상하 움직임 감지

        // 감도에 맞게 카메라 회전량 계산
        rotX += x * sensitivity * Time.deltaTime;   // 마우스가 좌우: 두리번두리번
        rotY += y * sensitivity * Time.deltaTime;   // 마우스가 위아래 : 끄덕끄덕

        // 카메라 회전 제한
        if (rotY > 50)
            rotY = 50;
        else if (rotY < -50)
            rotY = -50;

        transform.eulerAngles = new Vector3(-rotY, rotX, 0f);
    }
}
