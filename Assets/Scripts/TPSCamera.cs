using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSCamera : MonoBehaviour
{
    GameObject player;

    Vector3 offset = new Vector3(0, 1, -1);
    public float distance = 7.0f;   // currentZoom보다 명확한 이름으로 변경
    public float minZoom = 5.0f;
    public float maxZoom = 10.0f;
    public float sensitivity = 100f; // 마우스 감도

    float x;
    float y;

    GameObject transparentObj;
    Renderer ObstacleRenderer;  // 오브젝트를 반투명하게 만들어주는 렌더러
    List<int> ObstaclesID;

    // Start is called before the first frame update
    void Start()
    {
        // 플레이어 태그를 가진 게임오브젝트(=플레이어)를 찾아서 넣기
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        RotateAround();
        CalculateZoom();
        FadeOut();
    }

    // 카메라 확대율 계산
    void CalculateZoom()
    {
        // 마우스 줌 인/아웃
        distance -= Input.GetAxis("Mouse ScrollWheel");

        // 줌 최소/최대 제한
        // Clamp함수 : 최대/최소값을 설정해주고 제한
        distance = Mathf.Clamp(distance, minZoom, maxZoom);
    }

    // 마우스 움직임에 따라 플레이어 주위를 공전하는 카메라
    void RotateAround()
    {
        // 마우스의 위치를 받아오기
        x += Input.GetAxis("Mouse X") * sensitivity * 0.01f; // 마우스 좌우 움직임 감지
        y -= Input.GetAxis("Mouse Y") * sensitivity * 0.01f; // 마우스 상하 움직임 감지

        // 카메라 높이값(끄덕끄덕각도) 제어
        if (y < 0)  // 바닥을 뚫지 않게
            y = 0;
        if (y > 50) // Top View(정수리로 내려보기)로 하고 싶다면 90으로 바꾸기
            y = 50;

        // player.transform을 자주 사용할건데 너무 길어서 치환 => target
        Transform target = player.transform;

        // 카메라가 회전할 각도와 이동할 위치 계산
        Quaternion angle = Quaternion.Euler(y, x, 0);
        Vector3 destination = angle * (Vector3.back * distance) + target.position + Vector3.zero;

        transform.rotation = angle;             // 카메라 각도 조정
        transform.position = destination;   // 카메라 위치 조정
    }

    // 터레인이 아닌 게임오브젝트가 플레이어를 가리지 못하도록 반투명화 하는 메소드
    private void FadeOut()
    {
        // Raycast를 이용하여 플레이어와 카메라 사이에 있는 오브젝트 감지
        // 오브젝트로 감지되지 않으려면 Layer를 Ignor Raycast로 바꿔놓아야 함
        // Ignore Raycast: Player, Terrain, Particles(Steam, DustStorm)
        float distance = Vector3.Distance(transform.position, player.transform.position);
        Vector3 direction = (player.transform.position - transform.position).normalized;
        RaycastHit hit;

        // 카메라에서 플레이어를 향해 레이저를 쏘았을 때 맞은 오브젝트가 있다면
        if (Physics.Raycast(transform.position, direction, out hit, distance))
        {
            // 기존에 투명화한 오브젝트와 다른 오브젝트이면
            if (transparentObj != null && transparentObj != hit.collider.gameObject)
                RestoreMaterial();

            // 맞은 장애물을 저장하고 장애물의 렌더러를 얻어오기
            transparentObj = hit.collider.gameObject;
            ObstacleRenderer = transparentObj.GetComponentInChildren<Renderer>();

            // 그 장애물의 렌더러가 있다면
            if (ObstacleRenderer != null)
            {
                // 오브젝트를 반투명하게 렌더링한다
                Material material = ObstacleRenderer.material;
                Color matColor = material.color;
                matColor.a = 0.5f;
                material.color = matColor;
            }
        }
        else // Ray를 맞은 오브젝트가 없으면 
        {
            // 원상복구할 렌더러가 없으면 끝내기
            if (ObstacleRenderer == null)
                return;

            RestoreMaterial(); // 원상복구
        }
    }

    // 기존 투명화한 오브젝트를 원상복구 하는 메소드
    void RestoreMaterial()
    {
        Material material = ObstacleRenderer.material;
        Color matColor = material.color;
        matColor.a = 1f;
        material.color = matColor;

        ObstacleRenderer = null;
    }
}
