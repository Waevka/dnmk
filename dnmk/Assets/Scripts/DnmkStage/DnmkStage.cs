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
        StartCoroutine(EventStartInvoker());
    }

    IEnumerator EventStartInvoker()
    {
        for(int i = 0; i < dnmkEventList.Length; i++)
        {
            StartCoroutine(EventStarter(dnmkEventList[i], dnmkEventTime[i]));
            yield return null;
        }
    }

    IEnumerator EventStarter(DnmkEvent dnmkEvent, float eventStartTime)
    {
        yield return new WaitForSeconds(eventStartTime);
        Debug.Log("Event started at " + Time.time);
        dnmkEvent.gameObject.SetActive(true);

    }
}
