using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopLayout : MonoBehaviour {

    [Header("References")]
    [SerializeField] private Transform shopCategoryParent;
    [SerializeField] private Transform shopItemsParent;

    public Transform GetShopCategoryParent() {

        return shopCategoryParent;

    }

    public Transform GetShopItemsParent() {

        return shopItemsParent;

    }
}
