using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindPanel : MonoBehaviour
{
    float power = 25;
    Rigidbody playerRidbody;
    bool isActivated = false;
    ParticleSystem particle;

    private void Start()
    {
        particle = GetComponent<ParticleSystem>();
    }

    private void FixedUpdate()
    {
        if (isActivated)
        {
            playerRidbody.AddForce(Vector3.up * power, ForceMode.Force); // 천천히 위로 날려버리기
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 트리거에 들어온 오브젝트가 플레이어 태그를 가지고 있고 함정이 활성화 되지 않았다면
        if (other.CompareTag("Player") && isActivated == false)
        {
            StartCoroutine(other.GetComponent<PlayerControl>().SturnState());   // 플레이어에게 스턴 걸기

            playerRidbody = other.GetComponent<Rigidbody>();
            isActivated = true;
            StartCoroutine(ActivateWind());
        }
    }

    IEnumerator ActivateWind()
    {
        particle.Play();
        float sec = 0f;
        while (sec < 2f)
        {
            yield return new WaitForSeconds(0.1f);
            sec += 0.1f;
        }

        isActivated = false;
        playerRidbody = null;
        particle.Stop();
    }
}
