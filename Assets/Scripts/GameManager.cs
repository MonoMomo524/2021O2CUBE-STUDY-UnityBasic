using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        gameOverPanel.SetActive(false);  // 게임오버 패널은 꺼놓기
        startPos = GameObject.Find("Player").transform.position;    // 플레이어의 시작위치 기억
    }

    public void SetPlayerPosition()
    {
        GameObject.Find("Player").transform.position = startPos;
    }
}
