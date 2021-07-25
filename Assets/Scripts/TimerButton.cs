using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerButton : MonoBehaviour
{
    private Timer timer;
    private TextMeshProUGUI buttonText;

    void Start()
    {
        timer = GameObject.FindObjectOfType<Timer>();   // 타이머 스크립트를 가진 오브젝트에서 타이머 스크립트를 빼와주세요!
        buttonText = GetComponentInChildren<TextMeshProUGUI>(); // 내 자식 오브젝트에서 UI에 사용되는 텍스트메쉬프로를 
    }

    public void StartTimer()
    {
        timer.time = 91f;                        // 타이머 시작 시간을 90초로 맞추기 위해 91부터 카운트 다운
        buttonText.text = "Restart";    // Start버튼의 텍스트를 Restart로 변경
    }
}
