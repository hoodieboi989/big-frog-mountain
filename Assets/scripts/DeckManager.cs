using System.Collections.Generic;  
using UnityEngine;  
using UnityEngine.UI; 
  
public class DeckManager : MonoBehaviour  
{  
    public List<CardData> deck = new List<CardData>(); // All cards the player owns  
    public List<CardData> hand = new List<CardData>(); // Cards currently in hand  
    public int handSize = 4; // How many cards to draw
    public GameObject cardPrefab;
    public Transform handPanel;

    void Start()  
    {  
        InitializeDeck();  
  
        Debug.Log("Hand contains:");  
        foreach (CardData card in hand)  
        {  
            Debug.Log(card.cardName);  
        }  
    }   
  
    // Call this at the start of the game or level  
    public void InitializeDeck()  
    {  
        hand.Clear();  
        ShuffleDeck();  
        DrawHand();  
    }  
  
    // Randomize the deck  
    void ShuffleDeck()  
    {  
        for (int i = 0; i < deck.Count; i++)  
        {  
            CardData temp = deck[i];  
            int randomIndex = Random.Range(i, deck.Count);  
            deck[i] = deck[randomIndex];  
            deck[randomIndex] = temp;  
        }  
    }  
  
    // Move cards from Deck to Hand  
    public void DrawHand()  
    {  
        // Clear old UI cards first  
        foreach (Transform child in handPanel)  
        {  
            Destroy(child.gameObject);  
        }  

        for (int i = 0; i < handSize; i++)  
        {  
            if (deck.Count > 0)  
            {  
                // Take the last card from the deck and add to hand  
                CardData drawnCard = deck[deck.Count - 1];  
                deck.RemoveAt(deck.Count - 1);  
                hand.Add(drawnCard);  

                 // Create the UI  
                GameObject newCardUI = Instantiate(cardPrefab, handPanel);  

                CardDisplay display = newCardUI.GetComponent<CardDisplay>();  
                display.data = drawnCard;  
  
                // Update the text using the script's references  
                display.nameText.text = drawnCard.cardName;  
                display.descText.text = drawnCard.description;
            }  
        }  
    }  
}  