using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DnmkPlayer : MonoBehaviour {

    public DnmkPlayerManager PlayerManager { get; set; }
    public DnmkGameManager.DifficultyLevel DifficultyLevel { get; set; }
    public bool IsInvincible { get; set; }
    //score
    //difficulty

    // We use this bool to check if stats like: score, lives are already initialized,
    // because we want to keep them between levels.
    // Other things (position) - we want to reset between levels.
    public bool DataInitializedOnGameStart;
    public float invincibilityTime;
    private SpriteRenderer playerImage;
    private int invincibilityAnimationFrames;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        DataInitializedOnGameStart = false;
        DifficultyLevel = DnmkGameManager.DifficultyLevel.Easy;
        IsInvincible = false;
        invincibilityAnimationFrames = 10;
        GetComponent<DnmkPlayerController>().enabled = false;
        playerImage = GetComponentInChildren<SpriteRenderer>();
        playerImage.enabled = false;
    }
    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnParticleCollision(GameObject particle)
    {
        PlayerHit();
    }

    private void PlayerHit()
    {
        if (!IsInvincible)
        {
            Debug.Log("Player was hit");
            IsInvincible = true;
            StartCoroutine(InvincibilityPeriod());
        }
        //lives -1;
    }

    IEnumerator InvincibilityPeriod()
    {
        GetComponent<Rigidbody2D>().simulated = false;
        float startTime = Time.time;

        // animation ticks
        for(int i = 0; i < invincibilityAnimationFrames; i++)
        {
            playerImage.enabled = !playerImage.enabled;
            yield return new WaitForSeconds(invincibilityTime / (float)invincibilityAnimationFrames);
        }

        GetComponent<Rigidbody2D>().simulated = true;
        IsInvincible = false;
    }

    public void StartPlayerControl()
    {
        playerImage.enabled = true;
        GetComponent<DnmkPlayerController>().enabled = true;
        GetComponent<Rigidbody2D>().simulated = true;
    }

    public void StopPlayerControl()
    {
        playerImage.enabled = false;
        GetComponent<DnmkPlayerController>().enabled = false;
        GetComponent<Rigidbody2D>().simulated = false;
    }
}
