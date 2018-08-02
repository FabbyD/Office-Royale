using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameManager : NetworkBehaviour {

    public static GameManager instance;

    public Text remainingText;

    [SyncVar(hook = "OnRemainingChanged")]
    private int remaining;

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

    public void SetupGame()
    {
        remainingText.text = "Remaining: " + GameObject.FindGameObjectsWithTag("Player").Length;
    }

    public void PlayerJoined()
    {
        if (isServer)
        {
            remaining++;
        }
    }
    
    public void PlayerEliminated(GameObject gameObject)
    {
        if (isServer)
        {
            remaining--;
        }
    }

    void OnRemainingChanged(int remaining)
    {
        remainingText.text = "Remaining: " + remaining;
    }

}
