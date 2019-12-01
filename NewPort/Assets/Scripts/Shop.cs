using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Shop : MonoBehaviour
{


    public enum Child_index
    {
        image = 0,
        name = 1,
        kind = 2,
        info =3,
        price =4
    };

    public delegate void ShopSetting_Dele();
    public static ShopSetting_Dele shopSetting_Dele;

    public delegate void ShopBuy_Dele(int num);
    public static ShopBuy_Dele shopBuy_Dele;

    [SerializeField]
    GameObject Buy_Prefab;
    [SerializeField]
    ScrollRect scrollRect;

    void Awake()
    {
        shopSetting_Dele = ShopScroll_Setting;
        shopBuy_Dele = Shop_Buy;
        
    }

    private void OnEnable()
    {
        
        ShopScroll_Setting();
    }

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ShopScroll_Setting()
    {

        float ini_x = scrollRect.content.transform.localPosition.x + 455f, ini_y = scrollRect.content.transform.localPosition.y;
        float nwidth = 0, nheight = 0;
        int loop = 0;

        nwidth = Buy_Prefab.GetComponent<RectTransform>().rect.width;

        foreach (var i in DataLoader.item_Dic)
        {
            if (!i.Value.Get_IsSell())
                continue;

            nheight += Buy_Prefab.GetComponent<RectTransform>().rect.height;

            GameObject temp_prefab = Instantiate(Buy_Prefab) as GameObject;
            temp_prefab.transform.SetParent(scrollRect.content.transform, true);
            temp_prefab.transform.localPosition = new Vector2(ini_x, ini_y - (loop * Buy_Prefab.GetComponent<RectTransform>().rect.height));

            temp_prefab.transform.GetChild((int)Child_index.image).GetComponent<Image>().sprite = i.Value.Get_Image();
            temp_prefab.transform.GetChild((int)Child_index.name).GetComponent<Text>().text = "이름: " + i.Value.Get_Name();

            if (DataLoader.item_Dic[loop].Get_Kind() == DataLoader.Item_kind.Weapon)
                temp_prefab.transform.GetChild((int)Child_index.kind).GetComponent<Text>().text = "공격력: " + i.Value.Get_Stat().ToString();
            else if(DataLoader.item_Dic[loop].Get_Kind() == DataLoader.Item_kind.Armour)
                temp_prefab.transform.GetChild((int)Child_index.kind).GetComponent<Text>().text = "방어력: " + i.Value.Get_Stat().ToString();

            temp_prefab.transform.GetChild((int)Child_index.info).GetComponent<Text>().text = "설명: " + i.Value.Get_Info();
            temp_prefab.transform.GetChild((int)Child_index.price).GetComponent<Text>().text = "가격: " + i.Value.Get_Price().ToString();

            temp_prefab.name = "ShopItem_" + i.Value.Get_Index();
            loop++;
        }


        scrollRect.content.sizeDelta = new Vector2(nwidth, nheight);

    }

    void Shop_Buy(int num)
    {
        SaveDataIO.SaveTake();

        if (DataLoader.item_Dic[num].Get_Price() > SaveDataIO.save.Gold)
            return;


        //인벤토리에 추가
        Inventory.swapSlotforShop_Dele(num, false);


    }


    public void Click_ShopExit()
    {
        CameraManager.swap_Camera_Dele(CameraManager.Camera_name.main);
        UIManager.uI_Activator(UIManager.UI_name.main);
        UIManager.uI_deActivator(UIManager.UI_name.shop);
    }

    private void OnDisable()
    {

    }
}

