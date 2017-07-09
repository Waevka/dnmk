using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DnmkPlayingField : MonoBehaviour {
    public int Width { get; private set; }
    public int Height { get; private set; }
    public Vector2 Center { get; private set; }

    private void Awake()
    {
        Center = transform.position;
    }

    // Use this for initialization
    void Start () {
		
	}
	
}
