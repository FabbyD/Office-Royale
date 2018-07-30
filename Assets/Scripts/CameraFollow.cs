
using UnityEngine;
using UnityEngine.Networking;

public class CameraFollow : NetworkBehaviour {

    private void LateUpdate()
    {
        if (isLocalPlayer)
        {
            Camera.main.transform.position = new Vector3(
                transform.position.x,
                transform.position.y,
                Camera.main.transform.position.z);
        }
    }
}
