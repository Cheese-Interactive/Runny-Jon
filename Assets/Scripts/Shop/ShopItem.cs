using System;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class ShopItem : ScriptableObject {

    [Header("References")]
    public Type objectScript;

    [Header("Settings")]
    public bool defaultItem;
    public string itemName;
    public Image icon;
    public int price;
    [HideInInspector] public bool selected;

    public Type GetObjectScript() {

        return objectScript;

    }

    public bool IsDefaultItem() {

        return defaultItem;

    }

    public bool IsSelected() {

        return selected;

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
