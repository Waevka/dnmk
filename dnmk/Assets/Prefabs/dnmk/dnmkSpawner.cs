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

    private float lastSpawnTime;
    private bool spawnerActive;

    // Use this for initialization
    void Start () {
        lastSpawnTime = 0;
        spawnerActive = true;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(Time.time > lastSpawnTime + frequency && spawnerActive)
        {
            StartCoroutine(SpawnBullets());
            lastSpawnTime = Time.time;
            repeats -= 1;
        }

        //if (rotateSpeed > 0) transform.RotateAround(transform.position, Vector3.forward, rotateSpeed * Time.deltaTime);

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

        for(int i = 0; i < bulletAmount; i++)
        {
            GameObject bullet = Instantiate(dnmkPrefab, bulletCenterPivot.transform, false);
            //TODO: fix to make it perfect angle specified
            bullet.transform.Rotate(new Vector3(0.0f, 0.0f, totalAngle/2 + (totalAngle / (float)bulletAmount) * i)); // circle-type spawner
            bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.up * bulletSpeed;
            //bullet.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(0.0f, -1.0f * bulletSpeed), ForceMode2D.Impulse);
            Destroy(bullet, bulletLifetime);
        }

        //if (rotateSpeed > 0) StartCoroutine(RotateBulletCenterPivot(bulletCenterPivot.transform)); 
        //- for individual rotation of each circle
        Destroy(bulletCenterPivot, bulletLifetime);
        yield return null;
    }

    private IEnumerator RotateBulletCenterPivot(Transform pivot)
    {
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
