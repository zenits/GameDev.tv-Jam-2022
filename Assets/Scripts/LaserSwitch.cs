using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSwitch : MonoBehaviour
{

    public enum SwitchMode
    {
        OnOff,
        Switch
    }

    [SerializeField] List<GameObject> laserBeams = new List<GameObject>();
    [SerializeField] Sprite switchOn;
    [SerializeField] Sprite switchOff;
    [SerializeField] SwitchMode mode = SwitchMode.OnOff;
    [SerializeField] bool isOn = true;



    private SpriteRenderer _SpriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        _SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            var ghost = other.gameObject.GetComponent<GhostController>();
            ghost.onInteract.AddListener(Switch);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            var ghost = other.gameObject.GetComponent<GhostController>();
            ghost.onInteract.RemoveListener(Switch);
        }
    }

    private void Switch()
    {
        if (mode == SwitchMode.Switch)
            InvertBeams();
        else if (isOn)
            SetBeamsOff();
        else
            SetBeamsOn();
        UpdateSwitch();
    }

    private void UpdateSwitch()
    {
        if (isOn)
        {
            _SpriteRenderer.sprite = switchOff;
            isOn = false;
        }
        else
        {
            isOn = true;
            _SpriteRenderer.sprite = switchOn;
        }
    }

    public void InvertBeams()
    {
        laserBeams.ForEach(beam => beam.SetActive(!beam.activeSelf));
    }

    public void SetBeamsOn()
    {
        laserBeams.ForEach(beam => beam.SetActive(true));
    }
    public void SetBeamsOff()
    {
        laserBeams.ForEach(beam => beam.SetActive(false));
    }

}
