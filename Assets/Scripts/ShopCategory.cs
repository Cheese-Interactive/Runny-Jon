using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class ShopCategory : ScriptableObject {

    [Header("Information")]
    [SerializeField] private string categoryName;
    [SerializeField] private Image selectedIcon;
    [SerializeField] private Image deselectedIcon;
    [SerializeField] private List<ShopItem> shopItems;

    public string GetCategoryName() {

        return categoryName;

    }

    public Image GetSelectedIcon() {

        return selectedIcon;

    }

    public Image GetUnselectedIcon() {

        return deselectedIcon;

    }

    public List<ShopItem> GetShopItems() {

        return shopItems;

    }
}
