using System;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class ShopItem : ScriptableObject {

    [Header("References")]
    public Type objectScript;

    [Header("Settings")]
    public string itemName;
    public Image icon;
    public int price;
    public bool selected;
    public bool purchased;

    public Type GetObjectScript() {

        return objectScript;

    }

    public void SetSelected(bool selected) {

        this.selected = selected;

    }

    public bool IsSelected() {

        return selected;

    }

    public void SetPurchased(bool purchased) {

        this.purchased = purchased;

    }

    public bool IsPurchased() {

        return purchased;

    }

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
