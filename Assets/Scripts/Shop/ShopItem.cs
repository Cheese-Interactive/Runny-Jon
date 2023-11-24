using System;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class ShopItem : ScriptableObject {

    [Header("References")]
    [SerializeField] private GameObject cosmeticPrefab;

    [Header("Settings")]
    [SerializeField] private string itemName;
    [SerializeField] private Image icon;
    [SerializeField] private int price;
    [SerializeField] private bool selected;
    [SerializeField] private bool purchased;

    public Cosmetic GetCosmeticScript() {

        return cosmeticPrefab.GetComponent<Cosmetic>();

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
