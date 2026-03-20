using UnityEngine;  
  
public class CameraFollow : MonoBehaviour  
{  
    public Transform player; // Drag your player here in the inspector  
    public Vector3 offset = new Vector3(0, 0, -10); // Keeps camera behind player  
  
    void LateUpdate()  
    {  
        if (player != null)  
            transform.position = player.position + offset;  
    }  
}  