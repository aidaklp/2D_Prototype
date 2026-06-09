using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ShopManager : MonoBehaviour
{
    //array for shop items and sets the amount of shop items and how many values with them 
    public int[,] shopItems = new int[4, 2];


    //stores where the coins number will appear for each item
    public TMP_Text CoinsTXT;

    // will track purchased items 
    public bool[] itemPurchased;

    
    [Header("Next Round")]
    //stores reffrence of next round button
    public Button nextRoundButton;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CoinsTXT.text = "Coins:" + GameData.Instance.coins.ToString();

        //setting up the array by making item ID's and it starts as one as i plan to save the system and using the array zero can break in the future
        shopItems[0, 0] = 1;// item 1 iD
        shopItems [0, 1] = 30;// item 1 price

       
        shopItems[1, 0] = 2;// item 2 ID
        shopItems[1, 1] = 50;// Item 2 price

        // NEW
        shopItems[2, 0] = 3; // Echo
        shopItems[2, 1] = 50;

        shopItems[3, 0] = 4; // Pitch shift
        shopItems[3, 1] = 50;


        //Initialises the purchase tracker (one slot per item)
        itemPurchased = new bool[shopItems.GetLength(0)];

    }




    //creating the buying method
    public void Buy()
    {
        //making reffrences to the button and the event system 
        // this is done by creating a variable ButtonRef that will store the GamObject in the scene that has the event tag and will grab every event system component from the object being clicked on
        GameObject ButtonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;


        // stores the Item ID from the button that has been clicked (creates a variable so i dont have to repeat that line of code over and over again)
        int itemIndex = ButtonRef.GetComponent<ItemInfo>().ItemID;


        //Blocks item if already purchased 
        if (itemPurchased[itemIndex])
        {
            Debug.Log("Item already purchased");
            return;
        }


        //checks if the players have enough coins for the item
        if (GameData.Instance.coins >= shopItems[ButtonRef.GetComponent<ItemInfo>().ItemID,1])
        {
            //remove the coins (so subracting the price from our coins
            GameData.Instance.coins -= shopItems[ButtonRef.GetComponent<ItemInfo>().ItemID, 1];

            //updates the coins amount after purchase
            CoinsTXT.text = "Coins:" + GameData.Instance.coins.ToString();


            //saves as bought
            itemPurchased [itemIndex] = true;


            //visually disables the button 
            ButtonRef.GetComponent<Button>().interactable = false;

            Debug.Log("Bought item with ID: " + shopItems[itemIndex, 0]);


            //checks and stores if the metronome item is bought
            ///+new items
            if (itemIndex == 0)
            {
                GameData.Instance.hasMetronome = true;
            }
            else if (itemIndex == 1)
            {
                GameData.Instance.hasRedo = true;
            }
            else if (itemIndex == 2)
            {
                GameData.Instance.hasEcho = true;
            }
            else if (itemIndex == 3)
            {
                GameData.Instance.hasPitchShift = true;
            }
        }
        else
        {
            Debug.Log("Not enough coins.");
        }
    


        Debug.Log("Bought item with ID: " + ButtonRef.GetComponent<ItemInfo>().ItemID);
    }


    public void UpdateCoinsDisplay()
    {
        CoinsTXT.text = "Coins: " + GameData.Instance.coins.ToString();
    }

    
    // next round method
    public void NextRound()
    {
        // store how many items were bought this round
        int itemsBought = 0;

        for (int i = 0; i < itemPurchased.Length; i++)
        {
            if (itemPurchased[i])
            {
                itemsBought++;
            }
        }

        GameData.Instance.previousRoundItemsBought = itemsBought;

        //adds a game round everytime this is calles
        GameData.Instance.currentRound++;

        // calls the reset round data method
        GameData.Instance.ResetRoundData();

        //changes scene back to generation scene
        SceneManager.LoadScene("GeneratingScene");
    }
}
