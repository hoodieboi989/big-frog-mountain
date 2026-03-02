using UnityEngine;  
using UnityEngine.InputSystem; // Import the new Input System  
  
public class FrogTongue : MonoBehaviour  
{  
    public float tongueDistance = 20f;  
    public LineRenderer lineRenderer;  
    private PlayerControls controls;  
  
    void Awake()  
    {  
        controls = new PlayerControls();  
        controls.Gameplay.ShootTongue.performed += _ => ShootTongue();
    }  
  
    void OnEnable() { controls.Gameplay.Enable(); }  
    void OnDisable() { controls.Gameplay.Disable(); }  
  
    void Start() { lineRenderer.positionCount = 0; }  
  
    void ShootTongue()  
    {  
        Vector2 mouseScreen = controls.Gameplay.MousePosition.ReadValue<Vector2>();  
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreen);  
        mouseWorld.z = transform.position.z;  
    
        Vector2 direction = ((Vector2)mouseWorld - (Vector2)transform.position).normalized;  
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, tongueDistance);  
    
        lineRenderer.positionCount = 2;  
        lineRenderer.SetPosition(0, transform.position);  
    
        if (hit.collider != null)  
        {  
            lineRenderer.SetPosition(1, hit.point);  
        }  
        else  
        {  
            // Draw to max tongue distance in the aimed direction  
            Vector2 endPoint = (Vector2)transform.position + direction * tongueDistance;  
            lineRenderer.SetPosition(1, endPoint);  
        }  
    }  
}  
