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
    public GameObject dnmkPrefab;
    public float rotateSpeed;
    public bool rotateEachBurstIndependently;

    private float lastSpawnTime;
    private bool spawnerActive;

    // Use this for initialization
    void Start () {
        lastSpawnTime = 0;
        spawnerActive = true;
        //if (rotateSpeed > 0 && !rotateEachBurstIndependently) StartCoroutine(RotateBulletCenterPivot(transform));
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
            StartCoroutine(SpawnerCleanup());
        }
	}

    private IEnumerator SpawnBullets()
    {
        GameObject bulletCenterPivot = new GameObject("BulletPivot");
        bulletCenterPivot.transform.position = transform.position;
        bulletCenterPivot.transform.SetParent(transform, true);
        GameObject[] bullets = new GameObject[bulletAmount];

        for(int i = 0; i < bulletAmount; i++)
        {
            GameObject bullet = Instantiate(dnmkPrefab, bulletCenterPivot.transform, false);
            bullet.transform.RotateAround(bulletCenterPivot.transform.position, Vector3.forward, -totalAngle/2 + (totalAngle / ((float)bulletAmount-1)) * i); // circle-type spawner
            bullet.transform.Translate(-bulletCenterPivot.transform.up);
            //bullet.transform.Translate(0.0f, Time.deltaTime * bulletSpeed, 0.0f);
            //bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.up * bulletSpeed;
            //bullet.GetComponent<Rigidbody2D>().centerOfMass = bulletCenterPivot.transform.position;
            //bullet.GetComponent<Rigidbody2D>().MovePosition(bullet.transform.forward);
            bullets[i] = bullet;
            Destroy(bullet, bulletLifetime);
        }

        foreach(GameObject bullet in bullets)
        {
            bullet.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(0.0f, -1.0f * bulletSpeed), ForceMode2D.Impulse);
        }

        bulletCenterPivot.transform.localRotation = transform.localRotation;
        //if (rotateSpeed > 0 && rotateEachBurstIndependently) StartCoroutine(RotateBulletCenterPivot(bulletCenterPivot.transform)); 
        //- for individual rotation of each circle
        Destroy(bulletCenterPivot, bulletLifetime);
        yield return null;
    }

    private IEnumerator RotateBulletCenterPivot(Transform pivot)
    {
        Debug.Log("Starting rotation coroutine");
        while(pivot != null)
        {   
            pivot.RotateAround(pivot.transform.position, Vector3.forward, rotateSpeed * Time.deltaTime);
            yield return null;
        }
        yield return null;
    }

    private IEnumerator SpawnerCleanup()
    {
        yield return new WaitUntil(() => transform.childCount == 0);
        Destroy(gameObject);
    }
}
