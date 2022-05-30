using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyAwy : MonoBehaviour
{

    Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
            _animator.SetBool("IsFlying", true);
    }
}
