using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    float speed;
    int direction;  // 1이면 시계방향 회전, 0이면 시계 반대방향 회전
    float current;

    // Start is called before the first frame update
    void Start()
    {
        // 플랫폼인지 장애물인지 판별
        if(transform.parent.CompareTag("Ground") == true)
        {
            // 속도는 각 플랫폼마다 랜덤
            speed = Random.Range(30f, 51f);
        }
        // 장애물(회전하는 바)
        else
        {
            // 속도는 각 장애물마다 랜덤
            speed = Random.Range(60f, 81f);
        }

        // 움직이는 방향 결정: 시계방향/시계반대방향
        direction = Random.Range(0, 2);
        if (direction == 0)
            direction = -1;
    }

    // Update is called once per frame
    void Update()
    {
        current += Time.deltaTime * speed * direction;
        if (current >= 360 || current <= -360)
            current = 0;

        transform.rotation = Quaternion.Euler(0, current, 0);
    }
}
