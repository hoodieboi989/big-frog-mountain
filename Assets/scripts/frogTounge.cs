using UnityEngine;  
using UnityEngine.InputSystem;
using System.Collections;
  
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

    IEnumerator AnimateTongue(Vector2 direction)  
    {  
        float speed = 60f; // How fast the tongue extends  
        lineRenderer.positionCount = 2;    
        lineRenderer.SetPosition(0, transform.position);    
  
    Vector2 start = transform.position;    
    Vector2 end = start;    
  
    Vector2 target = start + direction * tongueDistance;  
    float traveled = 0f;  
    
    while (traveled < tongueDistance)  
    {  
        float step = speed * Time.deltaTime;  
        traveled += step;  
        Vector2 nextPoint = Vector2.Lerp(start, target, traveled / tongueDistance);  
    
        lineRenderer.SetPosition(1, nextPoint);  
    
        RaycastHit2D hit = Physics2D.Raycast(start, direction, traveled);  
        if (hit.collider != null)  
        {  
            lineRenderer.SetPosition(1, hit.point);  
            yield break;  
        }  
        yield return null;  
}  
    // If we reach here, tongue fully extended without hitting    
    // (We'll add retraction here next!)    
    }  

    void ShootTongue()  
    {  
        StopAllCoroutines(); // Stop any current tongue animation  
        
        Vector2 mouseScreen = controls.Gameplay.MousePosition.ReadValue<Vector2>();  
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreen);  
        mouseWorld.z = transform.position.z;  
        Vector2 direction = ((Vector2)mouseWorld - (Vector2)transform.position).normalized;  
        StartCoroutine(AnimateTongue(direction));  
    }  
}  
