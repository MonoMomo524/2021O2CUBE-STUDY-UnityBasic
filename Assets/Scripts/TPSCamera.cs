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
    List<GameObject> Obstacles;

    // Start is called before the first frame update
    void Start()
    {
        // 플레이어 태그를 가진 게임오브젝트(=플레이어)를 찾아서 넣기
        player = GameObject.FindGameObjectWithTag("Player");
        Obstacles = new List<GameObject>(); // 새 리스트 생성
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
        float distance = Vector3.Distance(transform.position, player.transform.position) - 1;
        Vector3 direction = (player.transform.position - transform.position).normalized;
        RaycastHit[] hits;

        // 카메라에서 플레이어를 향해 레이저를 쏘았을 때 맞은 오브젝트가 있다면
        hits = Physics.RaycastAll(transform.position, direction, distance);

        bool remove = true;
        if (Obstacles.Count != 0 && hits != null)
        {
            for (int i = 0; i <Obstacles.Count; i++)
            {
                foreach (var hit in hits)
                {
                    // hit된 오브젝트가 리스트에 저장되지 않았은 것이면 계속 탐색
                    if (Obstacles[i] != hit.collider.gameObject)
                        continue;
                    // 저장된 오브젝트면 유지
                    else
                    {
                        remove = false;
                        break;
                    }
                }

                // 삭제 대상이면
                if(remove== true)
                {
                    ObstacleRenderer = Obstacles[i].GetComponent<MeshRenderer>();
                    RestoreMaterial();

                    Obstacles.Remove(Obstacles[i]);
                }
            }
        }

        if (hits.Length > 0)
        {
            // 이미 저장된 오브젝트인지 확인
            for (int i = 0; i < hits.Length; i++)
            {
                Debug.DrawRay(transform.position, direction * distance, Color.red);

                transparentObj = hits[i].collider.gameObject;

                // 이미 저장된 오브젝트이면 다음 오브젝트 검사
                if (Obstacles!=null && Obstacles.Contains(transparentObj))
                    continue;

                // 저장되지 않은 오브젝트면 투명화 후 리스트에 추가
                if (transparentObj.layer == 9)
                    ObstacleRenderer = transparentObj.GetComponent<Renderer>();
                if (ObstacleRenderer != null && transparentObj != null)
                {
                    // 오브젝트를 반투명하게 렌더링한다
                    Material material = ObstacleRenderer.material;
                    Color matColor = material.color;
                    matColor.a = 0.5f;
                    material.color = matColor;

                    // 리스트에 추가
                    Obstacles.Add(transparentObj);
                    ObstacleRenderer = null;
                    transparentObj = null;
                }
            }
        }
    }

    // 기존 투명화한 오브젝트를 원상복구 하는 메소드
    void RestoreMaterial()
    {
        Material material = ObstacleRenderer.material;
        Color matColor = material.color;
        matColor.a = 1f;    // 알파값 1:불투명(원상복구)
        material.color = matColor;

        ObstacleRenderer = null;
    }
}
