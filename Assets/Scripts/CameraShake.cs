using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CameraShake : MonoBehaviour
{

    Vector3 originalPosition;

    [SerializeField] float shakeDuration = .3f;
    [SerializeField] Vector2 shakeStrength = new Vector2(.2f, .2f);

    [SerializeField] bool isShaking = false;
    [SerializeField] float elapsedTime = 0f;

    // Update is called once per frame
    void Update()
    {
        if (isShaking)
        {
            if (elapsedTime >= shakeDuration)
                StopShake();
            else
            {
                elapsedTime += Time.deltaTime; var x = Random.Range(-shakeStrength.x, shakeStrength.x);
                var y = Random.Range(-shakeStrength.y, shakeStrength.y);
                Vector3 strength = new Vector3(x, y, 0);

                Camera.main.transform.position = originalPosition + strength;
            }
        }
    }

    public void StartShake()
    {
        originalPosition = Camera.main.transform.position;
        elapsedTime = 0f;
        isShaking = true;
    }
    public void StopShake()
    {
        Camera.main.transform.position = originalPosition;
        elapsedTime = 0f;
        isShaking = false;
    }

}
