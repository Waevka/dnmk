using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DnmkPlayingField : MonoBehaviour {
    public float Width;
    public float Height;
    public float BulletDeactivationMargin;
    public Vector2 Center { get; private set; }

    private DnmkGameManager GameManager;

    private void Awake()
    {
        Center = transform.position;
        GetComponent<BoxCollider2D>().size = new Vector2(Width + 2 * BulletDeactivationMargin, Height + 2 * BulletDeactivationMargin);
    }

    // Use this for initialization
    void Start ()
    {
        GameManager = DnmkGameManager.Instance;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameManager.DnmkBulletPool.ReturnBulletToPool(collision.gameObject);
    }

}
