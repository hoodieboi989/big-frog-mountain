using UnityEngine;  
  
public class EnemyHealth : MonoBehaviour  
{  
    public int maxHealth = 3;  
    private int currentHealth;  
  
    void Start()  
    {  
        currentHealth = maxHealth;  
    }  
  
    public void TakeDamage(int amount)  
    {  
        currentHealth -= amount;  
        if (currentHealth <= 0)  
        {  
            Destroy(gameObject); // Enemy disappears when health is gone  
        }  
    }

    void OnCollisionEnter2D(Collision2D collision)  
    {  
        if (collision.collider.CompareTag("Player"))  
        {  
            TakeDamage(1); // or any amount you want  
        }  
    }    
}  
