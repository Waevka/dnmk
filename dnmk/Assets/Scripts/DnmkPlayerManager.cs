using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DnmkPlayerManager : MonoBehaviour, IDnmkReadyableObject {

    public enum MovementType { Normal, Focused };

    public bool IsReady { get; set; }
    public GameObject playerPrefab;
    public Vector3 DefaultPlayerPosition;
    [SerializeField]
    private float normalSpeed;
    public float NormalSpeed
    {
        get { return normalSpeed; }
    }

    [SerializeField]
    private float focusedSpeed;
    public float FocusedSpeed
    {
        get { return focusedSpeed; }
    }

    private DnmkGameManager GameManager;
    private DnmkPlayer PlayerInstance;

    private void Awake()
    {
        IsReady = false;
    }
    // Use this for initialization
    void Start () {
        GameManager = DnmkGameManager.Instance;
        if (PlayerInstance = FindObjectOfType<DnmkPlayer>())
        {
            Debug.Log("Existing Player instance found.");
        } else
        {
            PlayerInstance = Instantiate(playerPrefab).GetComponent<DnmkPlayer>();
            Debug.Log("Creating new Player instance.");
        }
        PlayerInstance.PlayerManager = this;
        IsReady = true;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartPlayer()
    {
        if (!PlayerInstance.DataInitializedOnGameStart)
        {
            switch (PlayerInstance.DifficultyLevel)
            {   //Do some stuff: change amout of lives, score multiplier etc;
                case DnmkGameManager.DifficultyLevel.Easy:
                    break;
                case DnmkGameManager.DifficultyLevel.Normal:
                    break;
                case DnmkGameManager.DifficultyLevel.Hard:
                    break;
                case DnmkGameManager.DifficultyLevel.Lunatic:
                    break;
            }
            PlayerInstance.DataInitializedOnGameStart = true;
        }

        PlayerInstance.transform.parent = GameManager.DnmkPlayingField.GetPlayingFieldTransform();
        PlayerInstance.transform.localPosition = DefaultPlayerPosition; 
        PlayerInstance.StartPlayerControl();
    }
}
