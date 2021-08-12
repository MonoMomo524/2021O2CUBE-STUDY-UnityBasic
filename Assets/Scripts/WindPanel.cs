using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindPanel : MonoBehaviour
{
    float power = 30f;
    bool isActivated = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // 트리거에 들어온 오브젝트가 플레이어 태그를 가지고 있고 함정이 활성화 되지 않았다면
        if (other.CompareTag("Player") && isActivated == false)
        {
            StartCoroutine(other.GetComponent<PlayerControl>().SturnState());   // 플레이어에게 스턴 걸기

            isActivated = true;
            Rigidbody rb = other.GetComponent<Rigidbody>();
            StartCoroutine(ActivateWind(rb));
        }
    }

    IEnumerator ActivateWind(Rigidbody rb)
    {
        float sec = 0f;
        while (sec < 3.5f)
        {
            yield return new WaitForSeconds(0.1f);
            sec += 0.1f;
            rb.AddForce(Vector3.up * power, ForceMode.Acceleration); // 천천히 위로 날려버리기
        }

        isActivated = false;
    }
}
