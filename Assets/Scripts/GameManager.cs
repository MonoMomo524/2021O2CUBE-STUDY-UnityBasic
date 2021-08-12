using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    Vector3 startPos;
    Vector3 endPos;

    bool isClear = false;
    public bool IsClear { get { return isClear; } }

    // Start is called before the first frame update
    void Start()
    {
        gameOverPanel.SetActive(false);  // 게임오버 패널은 꺼놓기
        startPos = GameObject.Find("Player").transform.position;    // 플레이어의 시작위치 기억
        endPos = GameObject.FindGameObjectWithTag("Goal").transform.position;
    }

    // 플레이어를 특정 위치로 이동
    public void SetPlayerPosition()
    {
        if(isClear==false)
            GameObject.Find("Player").transform.position = startPos;
        else
            GameObject.Find("Player").transform.position = endPos;
    }
}
