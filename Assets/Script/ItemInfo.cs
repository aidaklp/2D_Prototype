using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemInfo : MonoBehaviour
{
    //setting up the variables that each item will inherit and make them public so they can be chnaged within the inspector
    public int ItemID;

    public TMP_Text PriceTXT;
    public GameObject ShopManager; 

    // Update is called once per frame
    void Update()
    {
        //reffrences the shopmanager script attached to the game manager object
        ShopManager sm = ShopManager.GetComponent<ShopManager>();



        //checks for the item id of the item and sets the price to the right one (from the second column)
        PriceTXT.text = "Price: $" + ShopManager.GetComponent<ShopManager>().shopItems[ItemID, 1].ToString();

        //changes the UI to purchased
        if (sm.itemPurchased != null && sm.itemPurchased[ItemID])
        {
            PriceTXT.text = "Purchased";
        }
    }
}
