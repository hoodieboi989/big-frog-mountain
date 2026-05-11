using UnityEngine;  
using UnityEngine.UI;  
using TMPro; 
  
public class CardDisplay : MonoBehaviour  
{  
    public CardData data; // Holds the info for this specific card  
    public TMP_Text nameText; // Drag your Name Text here  
    public TMP_Text descText; // Drag your Description Text here  
    public CardEffectManager effectManager;

    // This method runs when you click the button  
    public void OnCardClick()  
    {  
        if (effectManager != null)  
        {  
            effectManager.ApplyCardEffect(data);  
            Destroy(gameObject); // Remove card after playing  
        }  
        else  
        {  
            Debug.LogWarning("CardEffectManager not assigned!");  
        }  
    }  
}  