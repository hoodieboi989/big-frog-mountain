using UnityEngine;  
  
public class FlyMovement : MonoBehaviour  
{  
    public float moveSpeed = 3f;  
    public float range = 10f;  
    public float waitTime = 1f;  
  
    private Vector2 startPos;  
    private Vector2 targetPos;  
    private bool moving = false;  
  
    void Start()  
    {  
        startPos = transform.position;  
        PickNewTarget();  
    }  
  
    void Update()  
    {  
        if (moving)  
        {  
            transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);  
            if (Vector2.Distance(transform.position, targetPos) < 0.1f)  
            {  
                moving = false;  
                Invoke(nameof(PickNewTarget), waitTime);  
            }  
        }  
    }  
  
    void PickNewTarget()  
    {  
        Vector2 randomOffset = Random.insideUnitCircle * range;  
        targetPos = startPos + randomOffset;  
        moving = true;  
    }  
}  