using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public enum Item
{
    GemBlue,
    GemGreen,
    GemRed,
    CoinGold,
    CoinSilver,
    CoinBronze,
    Hearth,
}

[System.Serializable]
public class OnCollect : UnityEvent<CollectItem> { }


public class CollectItem : MonoBehaviour
{

    public Item itemType;

    public OnCollect onCollect;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            onCollect.Invoke(this);
        }
    }


}
