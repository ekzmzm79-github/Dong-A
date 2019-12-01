using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : Slot_Parent
{

    private bool collision_Trigger, drag_Trigger , shopCollision_Trigger;
    private int Sour_num = -1, Des_num = -1;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        

        Sour_num = Convert.ToInt32(Regex.Replace(gameObject.name, @"\D", ""));

        drag_Trigger = true;

        defaultposition = transform.position;
        PositionOffset = transform.position - Input.mousePosition;

        transform.SetAsLastSibling();
        transform.parent.parent.transform.SetAsLastSibling(); //패널
        transform.parent.parent.parent.gameObject.GetComponent<Canvas>().sortingOrder = 1;
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (transform.GetChild(0).name == "empty")
            return;

        Vector2 currentPos = new Vector2(Input.mousePosition.x + PositionOffset.x, Input.mousePosition.y + PositionOffset.y);
        transform.position = currentPos;
    }
    public override void OnEndDrag(PointerEventData eventData)
    {

        drag_Trigger = false;
        transform.position = defaultposition;

        if(shopCollision_Trigger)
        {
            Inventory.swapSlotforShop_Dele(Sour_num, true);
        }
        else if (collision_Trigger && Sour_num + Des_num > 0) // 스왑 실행
        {
            Inventory.swapSlot_Dele(Sour_num, Des_num);
        }

        transform.parent.parent.parent.gameObject.GetComponent<Canvas>().sortingOrder = 0;
        Sour_num = -1;
        Des_num = -1;

    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {

        if(drag_Trigger)
        {
            if(collision.tag == "ItemSlot")
            {
                Des_num = Convert.ToInt32(Regex.Replace(collision.gameObject.name, @"\D", ""));
                collision_Trigger = true;
            }
            else if(collision.tag == "ShopSlot")
            {
                
                shopCollision_Trigger = true;
            }

            
            
        }
        
    }

    public override void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "ItemSlot")
            collision_Trigger = false;
        else if (collision.tag == "ShopSlot")
            shopCollision_Trigger = false;

        Des_num = -1;

    }


    public override void Click_Slot(GameObject onclick_object)
    {
        Inventory.drawInfo_Dele(onclick_object);

    }

    private void OnDisable()
    {
        if (transform.parent.name != "Status")
            Destroy(gameObject);
    }

}
