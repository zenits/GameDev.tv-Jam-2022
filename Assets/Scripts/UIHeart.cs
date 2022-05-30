using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHeart : MonoBehaviour
{

    [SerializeField] Sprite fullHeart;
    [SerializeField] Sprite emptyHeart;

    [SerializeField] List<UnityEngine.UI.Image> heartList = new List<UnityEngine.UI.Image>();

    public void OnHealthChange(int health)
    {
        for (int i = 0; i < heartList.Count; i++)
        {
            if (health > i)
                heartList[i].sprite = fullHeart;
            else
                heartList[i].sprite = emptyHeart;
        }
    }



}
