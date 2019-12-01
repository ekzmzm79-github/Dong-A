using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIManager : MonoBehaviour
{
    

    public enum UI_name {main, shop, inventory, warehouse, portal};

    public delegate void UI_delegate(UI_name name);
    public static UI_delegate uI_Activator;
    public static UI_delegate uI_deActivator;

    [SerializeField]
    GameObject main_ui, portal_ui, inventory_ui, shop_ui, warehouse_ui;

    [SerializeField]
    GameObject inventory_ui_ExitButton;

    void Awake()
    {
        uI_Activator = UI_Activator;
        uI_deActivator = UI_deActivator;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Active_main()
    {

    }

    void Active_shop()
    {

    }
    
    void UI_Activator(UI_name name)
    {
        if(name == UI_name.main)
        {
            main_ui.SetActive(true);
        }
        else if(name == UI_name.shop)
        {
            shop_ui.SetActive(true);
            inventory_ui.SetActive(true);
            Only_inventory(true);
        }
        else if(name == UI_name.inventory)
        {
            inventory_ui.SetActive(true);
        }
        else if(name == UI_name.warehouse)
        {
            warehouse_ui.SetActive(true);
            inventory_ui.SetActive(true);
            Only_inventory(true);

        }
        else if(name == UI_name.portal)
        {
            portal_ui.SetActive(true);
        }
    }

    void UI_deActivator(UI_name name)
    {
        if (name == UI_name.main)
        {
            main_ui.SetActive(false);
        }
        else if (name == UI_name.shop)
        {
            shop_ui.SetActive(false);
            Only_inventory(false);
            inventory_ui.SetActive(false);
        }
        else if (name == UI_name.inventory)
        {
            inventory_ui.SetActive(false);
        }
        else if (name == UI_name.warehouse)
        {
            warehouse_ui.SetActive(false);
            Only_inventory(false);
            inventory_ui.SetActive(false);
        }
        else if (name == UI_name.portal)
        {
            portal_ui.SetActive(false);
        }
    }


    void Only_inventory(bool trigger)
    {
        GameObject temp;

        for (int i = 0; i < inventory_ui.transform.childCount; i++)
        {
            temp = inventory_ui.transform.GetChild(i).gameObject;

            if (temp.name != "InventoryPanel")
                temp.SetActive(!trigger);

        }

        inventory_ui_ExitButton.gameObject.SetActive(!trigger);
    }






    




}
