using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class CollectibleManager : MonoBehaviour
{

    [SerializeField] List<CollectItem> collectibles;
    [SerializeField] TextMeshProUGUI textCounter;
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
                item.onCollect.RemoveListener(Collect);
                item.gameObject.SetActive(false);
                UpdateUIText();
                break;

            case Item.Hearth:
                item.onCollect.RemoveListener(Collect);
                item.gameObject.SetActive(false);
                break;

            default:
                break;
        }
    }

    private void UpdateUIText()
    {
        if (textCounter != null)
            textCounter.text = $"{numberOfItemCollected}/{collectibles.Where(i => i.itemType == Item.GemBlue).Count()}";
    }
}
