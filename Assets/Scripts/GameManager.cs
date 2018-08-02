using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;         //A reference to our game manager script so we can access it statically

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
    


}
