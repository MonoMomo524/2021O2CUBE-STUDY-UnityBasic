using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    float speed;
    int type; // 0이면 UpDown, 1이면 LeftRight
    int direction;  // 1이면 Up/Right, 0이면 Down/Left
    float max = 3f;
    float min =-3f;
    float current;  // 현재 위치
    

    // Start is called before the first frame update
    void Start()
    {
        // 랜덤 함수 주의!!!

        // 움직이는 타입 결정: 상하/좌우
        type = Random.Range(0, 2);

        // 속도는 각 플랫폼마다 랜덤
        speed = Random.Range(1f, 5f);

        // 어느 방향으로 움직일지
        direction = Random.Range(0, 2);
        if (direction == 0)
            direction = -1;

        // [디버그] 인스턴스마다 다르게 동작함
        //Debug.Log(transform.name + ":" + speed);
        //Debug.Log(transform.name + ":" + type);
    }

    // Update is called once per frame
    void Update()
    {
        // 현재 위치 계산
        current += Time.deltaTime * direction * speed;

        // 최대값보다 조금이라도 커지면 최대값으로 재조정
        if (current >= max)
        {
            current = max;  
            direction *= -1;    // 방향 전환(음수->양수, 양수->음수)
        }
        // 최소값보다 조금이라도 작아지면 최소값으로 재조정
        else if (current <= min)
        {
            current = min;  
            direction *= -1;    // 방향 전환(음수->양수, 양수->음수)
        }

        // 계산된 위치 값을 대입
        if (type == 0)
            transform.position = new Vector3(transform.position.x, current, transform.position.z);  // 플랫폼의 높이값 변화
        else
            transform.position = new Vector3(current, transform.position.y, transform.position.z);  // 플랫폼의 좌우값 변화
    }
}
