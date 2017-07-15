using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DnmkParticleSystemPool : MonoBehaviour, IDnmkReadyableObject
{
    public int queueSize;
    public GameObject defaultParticleSystemPrefab;
    public bool IsReady { get; set; }

    [SerializeField]
    private Queue<GameObject> particleSystemPool;
    [SerializeField]
    private int avaliableParticleSystems;
    private DnmkGameManager GameManager;

    private void Awake()
    {
        IsReady = false;
        particleSystemPool = new Queue<GameObject>();
    }

    // Use this for initialization
    void Start () {
        GameManager = DnmkGameManager.Instance;

        for (int i = 0; i < queueSize; i++)
        {
            GameObject particleSystem = Instantiate(defaultParticleSystemPrefab, transform);
            SetupBulletPlayingFieldCollider(particleSystem.GetComponent<ParticleSystem>());
            particleSystemPool.Enqueue(particleSystem);
        }
        IsReady = true;
    }
	
	// Update is called once per frame
	void Update () {
        avaliableParticleSystems = particleSystemPool.Count;
    }

    // Assigns a playing field boundary collider (to destroy bullets after they leave the screen),
    // if the collider is not assigned manually in Particle System "Trigger" settings tab
    private void SetupBulletPlayingFieldCollider(ParticleSystem particleSystem)
    {
        if (particleSystem.trigger.GetCollider(0) == null) // [0] - first collider on list (in this case playing field collider)
        {
            particleSystem.trigger.SetCollider(0, GameManager.DnmkPlayingField.GetPlayingFieldCollider());
        }
    }

    public GameObject RequestParticleSystemFromPool()
    {
        if (particleSystemPool.Count > 0)
        {
            GameObject system = particleSystemPool.Dequeue();
            return system;
        }
        else
        {
            Debug.Log("Error: not enough particleSystems in queue!");
            Debug.DebugBreak();
            return null;
        }
    }

    public void ReturnParticleSystemToPool(GameObject system)
    {
        system.transform.parent = transform;
        particleSystemPool.Enqueue(system);
    }
}
