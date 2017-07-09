using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DnmkPlayingField : MonoBehaviour {
    public float Width;
    public float Height;
    public float BulletDeactivationMargin;
    public Vector2 Center { get; private set; }

    private void Awake()
    {
        Center = transform.position;
    }

    // Use this for initialization
    void Start () {
		
	}
	
}
