using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>점프 패널의 경우, 위에서 밟아야지만 위로 튀어오르도록 동작</summary>
/// <param name="power">힘의 크기</param>
/// <param name="isStay">일정 시간동안 플레이어가 감지되는지</param>
/// <param name="isStay">타이머가 작동 중인지</param>

public class JumpingPanel : MonoBehaviour
{
    float power = 10f;
    bool isStay = false;
    bool isTimerOn = false;

    // Start is called before the first frame update
    void Start()
    {
        //점프 패널마다 힘을 다르게 하고싶다면 주석을 풀기
        //power = Random.Range(5, 21);
    }

    private void OnTriggerEnter(Collider other)
    {
        // 트리거에 들어온 오브젝트가 플레이어 태그를 가지고 있다면
        if(other.CompareTag("Player"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>(); // Player의 Rigidbody를 가져오기

            // 즉시 감지
            //rb.AddForce(Vector3.up * power, ForceMode.Impulse); // 튀어오르도록 위쪽을 향해 힘을 가하기

            // 시간차 감지
            isStay = true;
            if (isTimerOn == false)
            {
                isTimerOn = true;   // 코루틴 하나만 돌리도록 타이머 진행 기점(flag,깃발) 설정
                StartCoroutine(PushTimer(rb));  // [방법1]플레이어의 Rigidbody를 전달해주며 코루틴 시작하기
                //StartCoroutine("PushTimer", rb);  // [방법2]
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 트리거를 빠져나간 오브젝트가 플레이어 태그를 가지고 있다면
        if (other.CompareTag("Player"))
        {
            isStay = false;
            
            // 타이머가 돌고 있다면 강제 종료
            if (isTimerOn == true)
            {
                isTimerOn = false;   // 코루틴 하나만 돌리도록 타이머 진행 기점(flag,깃발) 설정
                StopCoroutine("PushTimer");
            }
        }
    }
    /// <summary>코루틴(Coroutine)</summary>
    /// 다수의 프레임을 오가며 특정 작업을 수행할 때
    /// 특정 시간동안 특정 작업을 수행해야할 때 주로 사용

    /// <summary>열거자(IEnumerator)</summary>
    /// while문으로 묶으면 타이머처럼 사용 가능
    /// 그렇지 않으면, yield return에서 반환해버리고 다시 호출했을 때 yield return이 마지막으로 일어난 곳 다음줄에서 실행
    IEnumerator PushTimer(Rigidbody rb)
    {
        float sec = 0;
        while(sec < 0.5f)
        {
            yield return new WaitForSeconds(0.1f);  // 유니티 시간의 지정 시간만큼 멈추기
            //yield return new WaitForSecondsRealtime(0.1f);    // 현실 시간의 지정 시간만큼 멈추기
            sec += 0.1f;    // 대기한 시간만큼 추가
        }

        // 아직도 감지가 되면
        if(isStay)
            rb.AddForce(Vector3.up * power, ForceMode.Impulse); // 튀어오르도록 위쪽을 향해 힘을 가하기

        isTimerOn = false;  // 타이머 작동 끝
    }
}
