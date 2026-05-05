using UnityEngine;  
  
[CreateAssetMenu(menuName = "Card/New Card")] // Allows you to create cards in Unity  
public class CardData : ScriptableObject  
{  
    public string cardName;  
    [TextArea]  
    public string description;  
    public Sprite artwork;  
  
    public enum CardType { Attack, Movement, Utility }  
    public CardType type;  
  
    // Stats for the card  
    public int value; // e.g., Damage amount, Heal amount, or Speed boost  
    public float duration; // How long effects last (if applicable)  
}  
