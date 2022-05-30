using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeMaterial : MonoBehaviour
{
    Color c;

    Color originalColor;

    [SerializeField] Material material;
    [SerializeField] float duration = 1f;
    [SerializeField] float step = 0.05f;


    private void OnEnable() => originalColor = material.color;
    private void OnDisable() => material.color = originalColor;



    // Start is called before the first frame update
    void Start()
    {
        c = material.color;

    }

    public void FadeIn()
    {
        StartCoroutine("ProcessFadeIn");
    }
    public void FadeOut()
    {
        StartCoroutine("ProcessFadeOut");
    }

    IEnumerator ProcessFadeOut()
    {
        for (float f = 1f; f >= -0.05f; f -= 0.05f)
        {
            c.a = f;
            material.color = c;
            yield return new WaitForSeconds(step / duration);
        }
    }
    IEnumerator ProcessFadeIn()
    {
        for (float f = 0f; f <= 1f; f += 0.05f)
        {
            c.a = f;
            material.color = c;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
