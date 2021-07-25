using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverPanel;

    // Start is called before the first frame update
    void Start()
    {
        gameOverPanel.SetActive(false);  // 게임오버 패널은 꺼놓기

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
