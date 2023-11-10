using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class ShopItem : ScriptableObject {

    public string itemName;

    public Image icon;

    public int price;

    public string GetItemName() {

        return itemName;

    }

    public Image GetIcon() {

        return icon;

    }

    public int GetPrice() {

        return price;

    }

    public bool Equals(ShopItem other) {

        if (itemName == other.GetItemName() && icon == other.GetIcon() && price == other.GetPrice())
            return true;

        return false;

    }
}
