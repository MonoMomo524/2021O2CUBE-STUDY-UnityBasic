using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    private Timer timer;

    void Start()
    {
        timer = GameObject.FindObjectOfType<Timer>();   // 타이머 스크립트를 가진 오브젝트에서 타이머 스크립트를 빼와주세요!
    }

    public void Restart()
    {
        timer.time = 121f;                        // 타이머 시작 시간을 n초로 맞추기 위해 n+1부터 카운트 다운
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
