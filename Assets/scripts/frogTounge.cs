using UnityEngine;  
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.UI;
using TMPro;
  
public class FrogTongue : MonoBehaviour  
{  
    public float tongueDistance;  
    public LineRenderer lineRenderer;  
    private PlayerControls controls;  
    public Vector2 pullTarget;  
    public bool isPulling = false;  
    public float pullSpeed;
    private Rigidbody2D rb;
    public int maxHealth = 50;
    private int currentHealth;
    public TMP_Text healthText;
    public float invincibilityTime = .25f;
    private bool isInvincible = false;
    public float pushForce = 20;
  
    void Awake()  
    {  
        controls = new PlayerControls();  
        controls.Gameplay.ShootTongue.performed += _ => ShootTongue();
        rb = GetComponent<Rigidbody2D>();
    }  

    void Update()
    {
        if (isPulling)
        {
            Vector2 direction = (pullTarget - (Vector2)transform.position).normalized;
            rb.linearVelocity = direction * pullSpeed;

            //stop pulling if close enough
            if (Vector2.Distance(transform.position, pullTarget) < 0.5f)
            {
                isPulling = false;
                rb.linearVelocity = Vector2.zero;
            }
        }
    }
  
    void OnEnable() { controls.Gameplay.Enable(); }  
    void OnDisable() { controls.Gameplay.Disable(); }  
  
    void Start() 
    { 
        lineRenderer.positionCount = 0;
        currentHealth = maxHealth;
        healthText.text = "Health: " + currentHealth;
    }  

    IEnumerator InvincibilityCoroutine()  
    {  
        isInvincible = true;  
        // Optional: add a visual effect here (like flashing)  
        yield return new WaitForSeconds(invincibilityTime);  
        isInvincible = false;  
    }  


    public void TakeDamage(int amount)  
    {  
        if (isInvincible) return;

        currentHealth -= amount;  
        if (currentHealth <= 0)
        {  
            currentHealth = 0;  
            // Handle death (restart, game over, etc.)  
            Debug.Log("Frog died!");  
        }  
        else  
        {  
            Debug.Log("Frog hurt! Health: " + currentHealth);  
        }  

        healthText.text = "Health: " + currentHealth;
        StartCoroutine(InvincibilityCoroutine());
    }  

    IEnumerator RetractTongue(Vector2 retractStart)  
    {  
        float retractSpeed = 30f;  
        float retractProgress = 0f;  
        while (retractProgress < 1f)  
        {  
            retractProgress += retractSpeed * Time.deltaTime / tongueDistance;  
            Vector2 retractPoint = Vector2.Lerp(retractStart, (Vector2)transform.position, retractProgress);  
            lineRenderer.SetPosition(1, retractPoint);  
            lineRenderer.SetPosition(0, transform.position);  
            yield return null;  
        }  
        lineRenderer.positionCount = 0;  
    }  

    IEnumerator AnimateTongue(Vector2 direction)  
    {  
        float speed = 120f; // How fast the tongue extends  
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
            
                if (hit.collider.CompareTag("Ground"))  
                {  
                    // Apply push  
                    Vector2 pushDir = ((Vector2)transform.position - hit.point).normalized;   
                    rb.AddForce(pushDir * pushForce, ForceMode2D.Impulse);  
            
                    // Retract the tongue  
                    StartCoroutine(RetractTongue(hit.point));  
                }  
                else  
                {  
                    StartCoroutine(GrapplePull(hit.point));  
                }  
                yield break;     
            }  
        }  
        // If we reach here, tongue fully extended without hitting    
        // (We'll add retraction here next!)  
        // Now retract the tongue back to the frog  
        Vector2 retractStart = lineRenderer.GetPosition(1); // The tip where extension stopped  
        float retractProgress = 0f;  
        float retractSpeed = 30f;  
        
        while (retractProgress < 1f)  
        {  
            retractProgress += retractSpeed * Time.deltaTime / tongueDistance;  
            Vector2 retractPoint = Vector2.Lerp(retractStart, (Vector2)transform.position, retractProgress);  
            lineRenderer.SetPosition(1, retractPoint);  
            yield return null;  
        }  
        
        // Hide tongue when done  
        lineRenderer.positionCount = 0;  
    }  

    IEnumerator GrapplePull(Vector2 targetPoint)  
    {   
        // Keep tongue extended  
        lineRenderer.positionCount = 2;  
        lineRenderer.SetPosition(1, targetPoint);  
    
        while (Vector2.Distance(transform.position, targetPoint) > 0.1f)  
        {  
            // Move frog toward target  
            transform.position = Vector2.MoveTowards(transform.position, targetPoint, pullSpeed * Time.deltaTime);  
    
            // Update tongue base to frog's position  
            lineRenderer.SetPosition(0, transform.position);  
    
            yield return null;  
        }  
    
        // Now retract tongue from target to frog  
        float retractTime = 0.2f;  
        float t = 0;  
        Vector2 startTip = targetPoint;  
        while (t < 1f)  
        {  
            t += Time.deltaTime / retractTime;  
            Vector2 tip = Vector2.Lerp(startTip, (Vector2)transform.position, t);  
            lineRenderer.SetPosition(1, tip);  
            lineRenderer.SetPosition(0, transform.position); // still follow frog  
            yield return null;  
        }  
    
        lineRenderer.positionCount = 0; // Hide tongue when done  
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

    void OnCollisionEnter2D(Collision2D collision)  
    {  
        if (collision.collider.CompareTag("Enemy"))  
        {  
            TakeDamage(1);  
        }  
    }  
}  
