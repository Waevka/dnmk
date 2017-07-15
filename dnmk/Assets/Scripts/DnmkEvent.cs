using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DnmkEvent : MonoBehaviour {

    private bool eventActive;

    private void Awake()
    {
        eventActive = false;
        gameObject.SetActive(false);
        //In case we forget to set it in Inspector
    }

    private void Update()
    {
        if(transform.childCount == 0)
        {
            Destroy(gameObject, 0.2f);
        }
    }

    private void OnEnable()
    {
        Debug.Log("Event enabled", gameObject);
    }
}
