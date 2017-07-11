using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dnmkSpawner : MonoBehaviour {

    [Range(1,360)]
    public float totalAngle;
    public float frequency;
    public int repeats;
    [Range(1,1000)]
    public int bulletAmount;
    public float bulletSpeed;
    [Range(5, 1000)]
    public float bulletLifetime;
    public float rotateSpeed;
    public bool rotateEachBurstIndependently;
    
    // Prefabs
    //private ParticleSystem dnmkParticleSystem;
    public GameObject dnmkPrefab;

    private float lastSpawnTime;
    private bool spawnerActive;
    private DnmkGameManager GameManager;

    private void Awake()
    {
        lastSpawnTime = 0;
        spawnerActive = true;
    }

    // Use this for initialization
    void Start ()
    {
        GameManager = DnmkGameManager.Instance;
        if (GameManager == null)
        {
            Debug.LogWarning("GameManager is null - possibly not enough time to initialize!", gameObject);
        }

        if (rotateSpeed > 0 && !rotateEachBurstIndependently) StartCoroutine(RotateBulletCenterPivot(transform));
        StartCoroutine(SpawnerCleanup());
    }
	
	// Update is called once per frame
	void FixedUpdate () {
		if(Time.time > lastSpawnTime + frequency && spawnerActive)
        {
            StartCoroutine(SpawnBullets());
            lastSpawnTime = Time.time;
            repeats -= 1;
        }

        if (repeats == 0)
        {
            spawnerActive = false;
        }
	}

    private IEnumerator SpawnBullets()
    {   
        // Create a pivot for attaching Particle System/Systems to it.
        GameObject bulletCenterPivot = new GameObject("BulletPivot");
        bulletCenterPivot.transform.position = transform.position;
        bulletCenterPivot.transform.SetParent(transform, true);
        
        // Request a ParticleSystem from pool and attach it to pivot.
        GameObject subParticleSystem = GameManager.DnmkParticleSystemPool.RequestParticleSystemFromPool();
        subParticleSystem.transform.position = bulletCenterPivot.transform.position;
        subParticleSystem.transform.parent = bulletCenterPivot.transform;

        // Set the parameters of the bullets: speed, color, sprite, etc.
        // Velocity sets the direction of the bullet.
        var emitParameters = new ParticleSystem.EmitParams();
        emitParameters.position = bulletCenterPivot.transform.localPosition;
        emitParameters.velocity = Vector3.down;
        emitParameters.axisOfRotation = Vector3.forward;

        for (int i = 0; i < bulletAmount; i++)
        {   
            // Create a temporary bulletTransform to simulate rotation of the particles, to calculate the velocity.
            Transform bulletTransform = bulletCenterPivot.transform;
            bulletCenterPivot.transform.rotation = transform.rotation; // keep up with the rotating parent

            // Circle type spawner formula:
            // If the total angle is 360, divide it evenly.
            // Else it will be divided so that both angle sides have bullets on them, and the edges of the angle are lined with bullets.
            // Velocity is now calculated.
            bulletTransform.transform.RotateAround(
                bulletCenterPivot.transform.position,
                Vector3.forward,
                totalAngle/2.0f + (totalAngle / ((totalAngle == 360.0f) ? ((float)bulletAmount) : ((float)bulletAmount - 1)) * i)
                );

            // Update more of the parameters
            emitParameters.velocity = bulletTransform.transform.right;
            // color etc.

            subParticleSystem.GetComponent<ParticleSystem>().Emit(emitParameters, 1);
        }
        
        if (rotateSpeed > 0 && rotateEachBurstIndependently)
        {
            StartCoroutine(RotateBulletCenterPivot(bulletCenterPivot.transform));
        }

        // for individual rotation of each inside circle
        //StartCoroutine(MoveBullets(bullets, bulletCenterPivot.transform));
        //StartCoroutine(ParticleSystemCleanup(subParticleSystem, bulletLifetime));
        StartCoroutine(PivotCleanup(bulletCenterPivot, bulletLifetime));
        yield return null;
    }

    // This method rotates the selected pivot around, with their child bullets.
    private IEnumerator RotateBulletCenterPivot(Transform pivot)
    {
        while(pivot != null) // Until the pivot is destroyed by the clean up functions
        {   
            pivot.RotateAround(pivot.transform.position, Vector3.forward, rotateSpeed * Time.deltaTime * 10.0f);
            yield return null;
        }
    }

    // Returns the Particle System back to the pool, when it has no more particles.
    private IEnumerator ParticleSystemCleanup(GameObject particleSystemHolder, float time)
    {
        float spawnerLifeTime = Time.time;
        ParticleSystem particleSystem = particleSystemHolder.GetComponent<ParticleSystem>();
        yield return new WaitUntil(() => particleSystem.particleCount == 0 && Time.time > (spawnerLifeTime + time));
        GameManager.DnmkParticleSystemPool.ReturnParticleSystemToPool(particleSystemHolder);

    }

    // Deletes the pivots after the bullets reach their lifetime or collide with playing field boundary.
    private IEnumerator PivotCleanup(GameObject pivot, float time)
    {
        float rotationStartTime = Time.time;
        yield return new WaitUntil(() => pivot.transform.childCount == 0 && Time.time > (rotationStartTime + time));
        Destroy(pivot, 0.2f);
    }

    // Deletes the spawners after all bullet burst have been shot (repeats == 0), and no more particle systems exist (childcount == 0).
    private IEnumerator SpawnerCleanup()
    {
        yield return new WaitUntil(() => transform.childCount == 0 && repeats == 0);
        Destroy(gameObject);
    }

    [System.Obsolete("Bullets are not GameObjects anymore, this is an old implementation to move bullets.")]
    private IEnumerator MoveBullets(GameObject[] bullets, Transform pivot)
    {
        while (pivot != null)
        {
            foreach(GameObject bullet in bullets)
            {
                if (bullet.activeSelf) bullet.transform.Translate(0.0f, Time.deltaTime * bulletSpeed, 0.0f);
            }
            yield return null;
        }
    }

}
