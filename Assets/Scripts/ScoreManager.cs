using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ScoreManager : NetworkBehaviour {

    [SyncVar(hook = "OnEliminationsChanged")]
    public int eliminations = 0;

    Text eliminationsText;

    void Start()
    {
        if (isLocalPlayer)
        {
            eliminationsText = GameObject.FindGameObjectWithTag("EliminationsText").GetComponent<Text>();
        }
    }

    void OnEliminationsChanged(int eliminations)
    {
        if (isLocalPlayer)
        {
            eliminationsText.text = "Eliminations: " + eliminations;
        }
    }
}
