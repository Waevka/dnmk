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
    public ParticleSystem dnmkParticleSystem;
    public GameObject dnmkPrefab;

    private float lastSpawnTime;
    private bool spawnerActive;
    private DnmkGameManager GameManager;

    private void Awake()
    {
        lastSpawnTime = 0;
        spawnerActive = true;
        GameManager = DnmkGameManager.Instance;
        if(dnmkParticleSystem != null && GameManager != null)
        {   
            if(dnmkParticleSystem.trigger.GetCollider(0) == null)
            {
                SetupBulletPlayingFieldCollider();
            }
        }
    }

    // Use this for initialization
    void Start () {
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
        GameObject bulletCenterPivot = new GameObject("BulletPivot");
        bulletCenterPivot.transform.position = transform.position;
        bulletCenterPivot.transform.SetParent(transform, true);
        GameObject[] bullets = new GameObject[bulletAmount];
        
        var emitParams = new ParticleSystem.EmitParams();
        emitParams.position = bulletCenterPivot.transform.localPosition;
        emitParams.velocity = Vector3.down;

        for (int i = 0; i < bulletAmount; i++)
        {
            Transform bulletTransform = bulletCenterPivot.transform;
            //GameObject bullet = GameManager.DnmkBulletPool.RequestBulletFromPool();
            //bullet.transform.position   = bulletCenterPivot.transform.position;
            //bullet.transform.parent     = bulletCenterPivot.transform;
            //bullet.transform.rotation   = bulletCenterPivot.transform.rotation;

            bulletCenterPivot.transform.rotation = transform.rotation;

            /* Circle type spawner
             * If the total angle is 360, divide it evenly.
             * Else it will be divided so that both angle sides have bullets on them, and the edges of the angle are shown.
             */
            bulletTransform.transform.RotateAround(
                bulletCenterPivot.transform.position,
                Vector3.forward,
                totalAngle/2.0f + (totalAngle / ((totalAngle == 360.0f) ? ((float)bulletAmount) : ((float)bulletAmount - 1)) * i)
                );

            if(dnmkParticleSystem != null)
            {
                emitParams.velocity = bulletTransform.transform.right;
                emitParams.axisOfRotation = Vector3.forward;
                emitParams.rotation = 45.0f;
               // emitParams.angularVelocity = 30.0f;
               if(dnmkParticleSystem.subEmitters.subEmittersCount > 0 && repeats - 1 < dnmkParticleSystem.subEmitters.subEmittersCount)
                {
                    dnmkParticleSystem.subEmitters.GetSubEmitterSystem(repeats - 1).Emit(emitParams, 1);
                } else
                {
                    dnmkParticleSystem.Emit(emitParams, 1);
                }
            }
            //Unused transformations:
            //bullet.transform.Translate(-bulletCenterPivot.transform.up); - move bullet X units forward from spawn point
            //bullet.transform.Translate(0.0f, Time.deltaTime * bulletSpeed, 0.0f); - same as ^, but with realtive path length
            //bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.up * bulletSpeed; - add velocity immediateli instead of AddRelativeForce
            //bullet.GetComponent<Rigidbody2D>().centerOfMass = bulletCenterPivot.transform.position; - change center of rigidbody mass
            //bullet.GetComponent<Rigidbody2D>().MovePosition(bullet.transform.forward); - move rigidbody without using force
            //bullet.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(0.0f, -1.0f * bulletSpeed), ForceMode2D.Impulse); - moves rigidbody using force forward
            //bullets[i] = bullet;
        }

        if (rotateSpeed > 0 && rotateEachBurstIndependently && repeats-1 < dnmkParticleSystem.subEmitters.subEmittersCount)
        {
            Debug.Log(repeats);
            StartCoroutine(RotateBulletCenterPivot(dnmkParticleSystem.subEmitters.GetSubEmitterSystem(repeats-1).gameObject.transform));
        } 
        // for individual rotation of each inside circle
        //StartCoroutine(MoveBullets(bullets, bulletCenterPivot.transform));
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

    // Deletes the pivots after the bullets reach their lifetime or collide with playing field boundary.
    private IEnumerator PivotCleanup(GameObject pivot, float time)
    {
        float rotationStartTime = Time.time;
        yield return new WaitUntil(() => pivot.transform.childCount == 0 && Time.time > (rotationStartTime + time));
        Destroy(pivot, 0.2f);
    }

    // Deletes the spawners after all bullet burst have been shot, and no more bullets exist.
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

    // Assigns a playing field boundary collider (to destroy bullets after they leave the screen),
    // if the collider is not assigned manually in Particle System "Trigger" settings tab
    private void SetupBulletPlayingFieldCollider()
    {
        dnmkParticleSystem.trigger.SetCollider(0, GameManager.DnmkPlayingField.GetPlayingFieldCollider());
    }
}
