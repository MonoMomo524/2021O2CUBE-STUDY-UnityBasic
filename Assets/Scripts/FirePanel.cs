using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePanel : MonoBehaviour
{
    bool isActivated = false;
    ParticleSystem particle;

    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponentInChildren<ParticleSystem>();
        StartCoroutine(ActivateFire());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator ActivateFire()
    {
        float sec;
        while (true)
        {
            // ON(3.5초)
            isActivated = true;
            particle.Play();

            sec = 0f;
            while (sec < 3.5f)
            {
                yield return new WaitForSeconds(0.1f);
                sec += 0.1f;
            }

            // OFF(3초)
            isActivated = false;
            particle.Stop();

            sec = 0f;
            while (sec < 3f)
            {
                yield return new WaitForSeconds(0.1f);
                sec += 0.1f;
            }
        }
    }

    public bool IsActivated()
    {
        return isActivated;
    }
}
