using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleMover : MonoBehaviour
{


    [SerializeField] AnimationCurve moveX;
    [SerializeField] AnimationCurve moveY;


    [SerializeField] bool applyOnX = false;
    [SerializeField] bool applyOnY = true;
    [SerializeField] bool moveLocalPosition = false;

    [Range(0.1f, 120f)]
    [SerializeField]
    float duration = 1f;

    float elapsedTime = 0f;

    Vector2 position;

    private void OnEnable()
    {
        if (moveX.length == 0)
        {
            moveX.AddKey(0, 0);
            moveX.AddKey(0.5f, 0.5f);
            moveX.AddKey(1, 0);
        }
        if (moveY.length == 0)
        {
            moveY.AddKey(0, 0);
            moveY.AddKey(0.5f, 0.5f);
            moveY.AddKey(1, 0);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        var p = position;
        if (applyOnX)
            p.x += moveX.Evaluate(elapsedTime / duration);
        if (applyOnY)
            p.y += moveY.Evaluate(elapsedTime / duration);

        if (moveLocalPosition)
            transform.localPosition = p;
        else
            transform.position = p;

    }
}
