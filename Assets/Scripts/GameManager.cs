using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameManager : NetworkBehaviour {

    // Singleton instance
    public static GameManager instance;

    // The shrinking safe zone
    public SafeZone safeZone;

    // Text displaying the amount of players left in the game
    public Text remainingText;

    // The UI overlay that appears when the player wins the game
    public GameObject winnerOverlay;

    // The UI overlay that appears when the player dies
    public GameObject loserOverlay;

    // Amount of time before the first zone starts shrinking
    public int firstZone = 10;

    // The time the game started
    private float startTime;

    // Amount of players left in the game
    [SyncVar(hook = "OnRemainingChanged")]
    private int remaining = 0;

    // Boolean indicating if the player is still alive
    private bool isAlive = true;

    void Awake()
    {
        //If we don't currently have a game manager...
        if (instance == null)
            //...set this one to be it...
            instance = this;
        //...otherwise...
        else if (instance != this)
            //...destroy this one because it is a duplicate.
            Destroy(gameObject);
    }

    void Start()
    {
        // TODO Move to the place where the 
    }

    void Update()
    {
        
    }

    public void SetupGame()
    {
        remainingText.text = GameObject.FindGameObjectsWithTag("Player").Length.ToString();
    }

    public void PlayerJoined()
    {
        if (isServer)
        {
            remaining++;
        }
    }
    
    public void PlayerEliminated(GameObject gameObject, bool isLocal)
    {
        if (isServer)
        {
            remaining--;
        }

        if (isLocal)
        {
            isAlive = false;
            loserOverlay.SetActive(true);
        }

        if (remaining == 1 && isAlive)
        {
            winnerOverlay.SetActive(true);
        }
    }

    void OnRemainingChanged(int remaining)
    {
        remainingText.text = remaining.ToString();
    }


}
