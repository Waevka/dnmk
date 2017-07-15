using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DnmkStage : MonoBehaviour, IDnmkReadyableObject
{
    public bool IsReady { get; set; }

    [SerializeField]
    private DnmkEvent[] dnmkEventList;
    [SerializeField]
    private float[] dnmkEventTime;
    [SerializeField]
    private float gameStartTime;

    private void Awake()
    {
        IsReady = false;
    }
    // Use this for initialization
    void Start () {
        IsReady = true;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
	}

    public void StartEvents()
    {
        gameStartTime = Time.time;
    }
}
