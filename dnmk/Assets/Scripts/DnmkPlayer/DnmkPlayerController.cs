using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DnmkPlayerController : MonoBehaviour {

    public ParticleSystem.EmitParams bulletProperties;
    public ParticleSystem playerShooter; // TODO

    private float varX;
    private float varY;
    private float normalSpeed;
    private float focusedSpeed;
    private bool isFocused;
    private bool isShooting;
    private bool canShoot;
    new private Rigidbody2D rigidbody;
    private DnmkGameManager GameManager;
    private Vector3 allocatedPositionVector;
    // Use this for initialization
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        isFocused = false;
        isShooting = false;
        focusedSpeed = 0.0f;
        normalSpeed = 0.0f;
        canShoot = false;
        bulletProperties = new ParticleSystem.EmitParams();
        bulletProperties.velocity = new Vector3(0.0f, 8.0f); //TODO
        allocatedPositionVector = Vector3.zero;
    }
    void Start () {
		
	}

    // Update is called once per frame
    void FixedUpdate() {
        UpdatePosition();
        UpdateFocus();
        UpdateBomb();
        UpdateFire();
    }

    private void UpdatePosition()
    {
        varX = Input.GetAxisRaw("Horizontal");
        varY = Input.GetAxisRaw("Vertical");
        if (varX != 0 || varY != 0)
        {
            if (isFocused)
            {
                allocatedPositionVector.x = varX * Time.fixedDeltaTime * focusedSpeed;
                allocatedPositionVector.y = varY * Time.fixedDeltaTime * focusedSpeed;
                allocatedPositionVector = Vector3.ClampMagnitude(allocatedPositionVector, focusedSpeed);
            }
            else
            {
                allocatedPositionVector.x = varX * Time.fixedDeltaTime * normalSpeed;
                allocatedPositionVector.y = varY * Time.fixedDeltaTime * normalSpeed;
                allocatedPositionVector = Vector3.ClampMagnitude(allocatedPositionVector, normalSpeed);
            }
            rigidbody.MovePosition(transform.position + allocatedPositionVector);
        }
    }

    private void UpdateFocus()
    {
        if (Input.GetButtonDown("Focus") || Input.GetButton("Focus"))
        {
            if (!isFocused)
            {
                Debug.Log("Entering Focus");
                //TODO: send focus start message for special effects
                isFocused = true;
            }
        }
        else if (Input.GetButtonUp("Focus") || !Input.GetButton("Focus"))
        {
            if (isFocused)
            {
                Debug.Log("Leaving focus");
                //TODO: send focus start message for special effects
                isFocused = false;
            }
        }
    }

    private void UpdateBomb()
    {
        //TODO: invoke shooting module
    }

    private void UpdateFire()
    {
        if(Input.GetButtonDown("Fire") || Input.GetButton("Fire"))
        {
            if (!isShooting)
            {
                isShooting = true;
            }
            Shoot();

        } else if (Input.GetButtonUp("Fire") || !Input.GetButton("Fire"))
        {
            isShooting = false;
        }
    }

    private void Shoot()
    {
        if(canShoot)
        {
            playerShooter.Emit(bulletProperties, 1);
        }
    }

    private void OnEnable()
    {
        isFocused = false;
        isShooting = false;
        canShoot = true;
        GameManager = DnmkGameManager.Instance;
        normalSpeed = GameManager.DnmkPlayer.NormalSpeed;
        focusedSpeed = GameManager.DnmkPlayer.FocusedSpeed;
    }

    private void OnDisable()
    {
        /*var temShooterMain = tempShooter.main;
        temShooterMain.simulationSpace = ParticleSystemSimulationSpace.Local;
        if (GameManager.DnmkParticleSystemPool != null)
        {
            GameManager.DnmkParticleSystemPool.ReturnParticleSystemToPool(tempShooter.gameObject);
        }
        tempShooter = null;*/
    }

    private void OnDestroy()
    {
        if (GameManager.DnmkParticleSystemPool != null && playerShooter != null)
        {
            GameManager.DnmkParticleSystemPool.ReturnParticleSystemToPool(playerShooter.gameObject);
        }
    }
}
