using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DnmkGameManager : MonoBehaviour {

    public static DnmkGameManager Instance { get; private set; }

    [SerializeField]
    private DnmkPlayingField dnmkPlayingField = null;
    public DnmkPlayingField DnmkPlayingField
    {
        get { return dnmkPlayingField; }
    }

    [SerializeField]
    private DnmkParticleSystemPool dnmkParticleSystemPool = null;
    public DnmkParticleSystemPool DnmkParticleSystemPool
    {
        get { return dnmkParticleSystemPool; }
    }

    [SerializeField]
    private DnmkBulletPool dnmkBulletPool = null;
    public DnmkBulletPool DnmkBulletPool
    {
        get { return dnmkBulletPool; }
    }


    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("Multiple instances of DnmkGameManager!", gameObject);
            return;
        }
        Instance = this;
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
