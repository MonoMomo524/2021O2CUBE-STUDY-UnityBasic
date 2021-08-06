using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    public Slider HPSlider;
    private bool isGameOver = false;
    public bool IsGameOver  {   get { return isGameOver; }    }

    // 플레이어 사망 시 발동되는 유니티 이벤트
    /// <summary> UnityEvent </summary>
    /// Delegate와 Event를 사용했을 때에는 같은 형태의 메소드만 등록할 수 있지만,
    /// UnityEvent의 경우, 다른 형태의 메소드도 등록이 가능하며 에디터에서 가시적으로 메소드를 지정할 수 있다.
    public UnityEvent OnPlayerDead;

    // Use this for initialization
    void Start()
    {
        HPSlider.value = 100;             // 플레이어 HP는 100으로 설정
        isGameOver = false;               // 게임오버 상태가 아님
    }

    private void Update()
    {
        // 플레이어 HP가 0이면 게임오버
        if (HPSlider.value <= 0)
        {
            HPSlider.transform.Find("Fill Area").transform.Find("Fill").gameObject.SetActive(false);
            isGameOver = true;
            Timer.IsOver = true;
            Dead(); // 플레이어 사망 시 다른 클래스에서도 사망 관련 처리 실행
        }
    }

    // 트리거가 지속적으로 감지되는지 확인
    void OnTriggerStay(Collider other)
    {
        // 대소문자까지 모두 일치해야 판정됨
        if (other.gameObject.name == "Fire" && HPSlider.value > 0)
        {
            HPSlider.value -= .25f;
        }
        else if(other.gameObject.name == "Water" && HPSlider.value < 100)
        {
            HPSlider.value += .1f;
        }
    }

    private void Dead()
    {
        OnPlayerDead.Invoke();
    }
}
