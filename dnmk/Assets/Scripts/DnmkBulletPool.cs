using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DnmkBulletPool : MonoBehaviour {

    public int queueSize;
    public GameObject defaultBulletPrefab;

    [SerializeField]
    private Queue<GameObject> bulletPool;
    [SerializeField]
    private int avaliableBullets;

    private void Awake()
    {
        bulletPool = new Queue<GameObject>();

        for(int i = 0; i < queueSize; i++)
        {
            GameObject bullet = Instantiate(defaultBulletPrefab, transform);
            bullet.SetActive(false);
            bulletPool.Enqueue(bullet);
        }

    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        avaliableBullets = bulletPool.Count;
	}

    public GameObject RequestBulletFromPool()
    {
        if (bulletPool.Count > 0)
        {
            GameObject bullet = bulletPool.Dequeue();
            bullet.SetActive(true);
            return bullet;
        } else
        {
            Debug.Log("Error: not enough bullets in queue!");
            Debug.DebugBreak();
            return null;
        }
    }

    public void ReturnBulletToPool(GameObject bullet)
    {
        bullet.SetActive(false);
        bullet.transform.parent = transform;
        bulletPool.Enqueue(bullet);
    }

    public int GetAvialiableBulletsCount()
    {
        return avaliableBullets;
    }
}
