using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSCamera : MonoBehaviour
{
    GameObject player;

    Vector3 offset = new Vector3(0, 1, -1);
    public float currentZoom = 7.0f;
    public float minZoom = 5.0f;
    public float maxZoom = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // 마우스 줌 인/아웃
        currentZoom -= Input.GetAxis("Mouse ScrollWheel");
        // 줌 최소/최대 제한
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom); // Clamp함수 : 최대/최소값을 설정해주고 제한한다.
    }

    private void LateUpdate()
    {
        // 변경된 카메라 위치를 적용
        this.transform.position = player.transform.position + offset * currentZoom;
        this.transform.LookAt(player.transform);
    }
}
