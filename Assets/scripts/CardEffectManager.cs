using UnityEngine;  
  
public class CardEffectManager : MonoBehaviour  
{  
    // References to your game objects  
    public GameObject player; // Drag your Frog here  
    public GameObject enemy;  // Drag an Enemy here (or we can find them dynamically)  
  
    public void ApplyCardEffect(CardData card)  
    {  
        FrogTongue playerScript = player.GetComponent<FrogTongue>();  
        Debug.Log("Applying effect: " + card.cardName);  
  
        switch (card.type)  
        {  
            case CardData.CardType.Attack:  

                if (playerScript != null)  
                {  
                    playerScript.nextAttackBonus = card.value;  
                    Debug.Log("Next attack buffed by " + card.value);  
                }   
                break;

            case CardData.CardType.Movement:  
                // Boost player speed or jump  
                Debug.Log("Movement! Boosting by " + card.value);  
                // TODO: Add actual movement logic here  
                break;  
  
            case CardData.CardType.Utility:  

            if (playerScript != null)  
            {  
                playerScript.currentHealth += card.value;  
                
                if (playerScript.currentHealth > playerScript.maxHealth)  
                {  
                    playerScript.currentHealth = playerScript.maxHealth;  
                }  
                
                playerScript.healthText.text = "Health: " + playerScript.currentHealth;  
                Debug.Log("Healed for " + card.value);  
            }  
            break;  
        }  
    }  
}  