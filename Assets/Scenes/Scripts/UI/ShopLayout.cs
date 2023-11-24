using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopLayout : MonoBehaviour {

    [Header("References")]
    [SerializeField] private Transform shopSectionParent;
    [SerializeField] private Transform shopItemsParent;

    public Transform GetShopSectionParent() {

        return shopSectionParent;

    }

    public Transform GetShopItemsParent() {

        return shopItemsParent;

    }
}
