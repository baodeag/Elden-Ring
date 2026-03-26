using UnityEngine;

namespace baodeag
{
    public class Item : ScriptableObject
    {
        [Header("Item Information")]
        public string itemName;
        public Sprite itemIcon;

        //decides if this item can have a stackable amount
        public int maxItemAmount = 1;
        public int currentItemAmount = 1;

        [TextArea] public string itemDescription;
        public int itemID;

        [Header("Shop")]
        public bool canBePurchased = true;
        public bool canBeSold = true;
        public int purchasePrice = 100;
        public int sellPrice = 50;
    }
}
