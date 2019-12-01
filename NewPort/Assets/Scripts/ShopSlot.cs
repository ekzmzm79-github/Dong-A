using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class ShopSlot : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Click_BuyButton()
    {
        int index = Convert.ToInt32(Regex.Replace(gameObject.name, @"\D", ""));
        Shop.shopBuy_Dele(index);
    }

}
