using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DnmkPlayerController : MonoBehaviour {

    private float varX;
    private float varY;
    private float normalSpeed;
    private float focusedSpeed;
    private bool isFocused;
    new private Rigidbody2D rigidbody;
    private DnmkGameManager GameManager;
    // Use this for initialization
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        focusedSpeed = 0.0f;
        normalSpeed = 0.0f;
    }
    void Start () {
		
	}

    // Update is called once per frame
    void Update() {
        varX = Input.GetAxis("Horizontal");
        varY = Input.GetAxis("Vertical");
        if (varX != 0 || varY != 0)
        {
            if (isFocused)
            {
                varX = varX * focusedSpeed * Time.deltaTime;
                varY = varY * focusedSpeed * Time.deltaTime;
            }
            else
            {
                varX = varX * normalSpeed * Time.deltaTime;
                varY = varY * normalSpeed * Time.deltaTime;
            }

            Debug.Log(varX);
            rigidbody.MovePosition(new Vector2(transform.localPosition.x + varX, transform.localPosition.y + varY));
        }
    }

    private void OnEnable()
    {
        isFocused = false;
        GameManager = DnmkGameManager.Instance;
        normalSpeed = GameManager.DnmkPlayer.NormalSpeed;
        focusedSpeed = GameManager.DnmkPlayer.FocusedSpeed;
    }
}
