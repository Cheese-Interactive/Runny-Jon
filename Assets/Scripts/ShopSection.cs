
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class ShopSection : ScriptableObject {

    [Header("Information")]
    [SerializeField] private string sectionName;
    [SerializeField] private Image selectedIcon;
    [SerializeField] private Image deselectedIcon;
    [SerializeField] private List<ShopItem> shopItems;

    public string GetSectionName() {

        return sectionName;

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
