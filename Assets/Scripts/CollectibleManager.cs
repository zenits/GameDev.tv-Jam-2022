using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class CollectibleManager : MonoBehaviour
{

    [SerializeField] List<CollectItem> collectibles;
    [SerializeField] TextMeshProUGUI textCounter;
    [SerializeField] AudioClip CollectSound;
    private int numberOfItemCollected = 0;

    // Start is called before the first frame update
    void Start()
    {
        collectibles = FindObjectsOfType<CollectItem>(true).ToList();

        foreach (var item in collectibles)
        {
            item.onCollect.AddListener(Collect);
        }
        UpdateUIText();
    }
    // Update is called once per frame
    void Update()
    {

    }

    private void Collect(CollectItem item)
    {
        switch (item.itemType)
        {
            case Item.GemBlue:
                numberOfItemCollected++;
                UpdateUIText();

                break;

            case Item.Hearth:

                break;

            default:
                break;
        }
        item.gameObject.SetActive(false);
        AudioManager.Instance.PlayOnce(CollectSound, 1);
        if (item.Respwanable)
            StartCoroutine("RespawnItem", item);
        else
            item.onCollect.RemoveListener(Collect);
    }

    private IEnumerator RespawnItem(object value)
    {
        Debug.Log("RespawnItem");
        CollectItem item = (CollectItem)value;
        yield return new WaitForSeconds(item.RespwanDelayInSeconds);
        item.gameObject.SetActive(true);
        yield return 0;
    }

    private void UpdateUIText()
    {
        if (textCounter != null)
            textCounter.text = $"{numberOfItemCollected}/{collectibles.Where(i => i.itemType == Item.GemBlue).Count()}";
    }
}
