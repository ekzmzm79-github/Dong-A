using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public abstract class Slot_Parent : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    protected int slot_num = -1;
    protected static Vector2 defaultposition, PositionOffset;

    public abstract void OnBeginDrag(PointerEventData eventData);
    public abstract void OnDrag(PointerEventData eventData);
    public abstract void OnEndDrag(PointerEventData eventData);
    public abstract void OnTriggerEnter2D(Collider2D collision);
    public abstract void OnTriggerExit2D(Collider2D collision);
    public abstract void Click_Slot(GameObject onclick_objeck);
}

public class Inventory : MonoBehaviour
{
    public enum Equipmen_index
    {
        weapon = 36,
        armour = 37
    };

    private int W_weight = 100;

    public delegate void SwapSlot_Dele(int Sour, int Des);
    public static SwapSlot_Dele swapSlot_Dele;

    public delegate void SwapSlotforShop_Dele(int index, bool IsSell);
    public static SwapSlotforShop_Dele swapSlotforShop_Dele;

    public delegate void DrawInfo_Dele(GameObject obj);
    public static DrawInfo_Dele drawInfo_Dele;

    [SerializeField]
    GameObject first_slot, Slot_Prefab, WeaponSlot, ArmourSlot, InfoPanel;
    
    private Vector3 first_positon;
    private float horizon_Gap = 130f, vertical_Gap = -120f;
    private int X_many = 6, Y_many = 6;

    private Dictionary<int, GameObject> Inventory_Dic = new Dictionary<int, GameObject>();

    private bool showPanel_Trigger;

    void Awake()
    {
        swapSlot_Dele = Swap_Slot;
        swapSlotforShop_Dele = Swap_SlotforShop;
        drawInfo_Dele = Draw_InfoPanel;

        first_positon = first_slot.transform.localPosition;
        
    }
    private void OnEnable()
    {

        Draw_Inventory();

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetMouseButtonDown(0) && showPanel_Trigger)
        {
            showPanel_Trigger = false;

            InfoPanel.gameObject.SetActive(false);

        }

    }

    void Draw_Inventory()
    {
        SaveDataIO.SaveTake();

        

        for (int j = 0; j < Y_many; j++)
        {
            for (int i = 0; i < X_many; i++)
            {
                GameObject slot = Instantiate(Slot_Prefab) as GameObject;
                slot.transform.SetParent(this.gameObject.transform, true);
                slot.transform.localPosition = new Vector3(first_positon.x + (i * horizon_Gap), first_positon.y + (j * vertical_Gap), 0f);
                slot.gameObject.name = "slot_" + (i + j * X_many);

                Inventory_Dic[i + j * Y_many] = slot;

                if (SaveDataIO.save.Inventory.ContainsKey(i + j * Y_many))
                {
                    Inventory_Dic[i + j * Y_many].transform.GetChild(0).GetComponent<Button>().enabled = true;
                    Inventory_Dic[i + j * Y_many].transform.GetChild(0).GetComponent<Image>().sprite = DataLoader.item_Dic[SaveDataIO.save.Inventory[i + j * Y_many]].Get_Image();

                    Color color = Inventory_Dic[i + j * Y_many].transform.GetChild(0).GetComponent<Image>().color;
                    Inventory_Dic[i + j * Y_many].transform.GetChild(0).GetComponent<Image>().color = new Color(color.r, color.g, color.b, 1.0f);
                    Inventory_Dic[i + j * Y_many].transform.GetChild(0).name = SaveDataIO.save.Inventory[i + j * Y_many].ToString();
                    
                }


            }
        }

        //마지막 두 자리는 status-ui의 equipment : weapon, armour
        Inventory_Dic[X_many * Y_many] = WeaponSlot;

        if (SaveDataIO.save.Equipment[DataLoader.Item_kind.Weapon] != -1)
        {
            Inventory_Dic[X_many * Y_many].transform.GetChild(0).GetComponent<Button>().enabled = true;
            Inventory_Dic[X_many * Y_many].transform.GetChild(0).GetComponent<Image>().sprite = DataLoader.item_Dic[SaveDataIO.save.Equipment[DataLoader.Item_kind.Weapon]].Get_Image();


            Color color = Inventory_Dic[X_many * Y_many].transform.GetChild(0).GetComponent<Image>().color;
            Inventory_Dic[X_many * Y_many].transform.GetChild(0).GetComponent<Image>().color = new Color(color.r, color.g, color.b, 1.0f);
            Inventory_Dic[X_many * Y_many].transform.GetChild(0).name = SaveDataIO.save.Equipment[DataLoader.Item_kind.Weapon].ToString();

        }

        Inventory_Dic[X_many * Y_many + 1] = ArmourSlot;

        if (SaveDataIO.save.Equipment[DataLoader.Item_kind.Armour] != -1)
        {
            Inventory_Dic[X_many * Y_many + 1].transform.GetChild(0).GetComponent<Button>().enabled = true;
            Inventory_Dic[X_many * Y_many + 1].transform.GetChild(0).GetComponent<Image>().sprite = DataLoader.item_Dic[SaveDataIO.save.Equipment[DataLoader.Item_kind.Armour]].Get_Image();

            Color color = Inventory_Dic[X_many * Y_many + 1].transform.GetChild(0).GetComponent<Image>().color;
            Inventory_Dic[X_many * Y_many + 1].transform.GetChild(0).GetComponent<Image>().color = new Color(color.r, color.g, color.b, 1.0f);
            Inventory_Dic[X_many * Y_many + 1].transform.GetChild(0).name = SaveDataIO.save.Equipment[DataLoader.Item_kind.Armour].ToString();

        }

    }


    


    void Swap_Slot(int Sour, int Des)
    {
        int Check_sum = Sour + Des;

        if (Check_sum > W_weight * 2) //warehouse 창에서 slot 두개 pick
        {
            Warehouse.swapSlot_Dele(Sour - W_weight, Des - W_weight);
            return;
        }
        else if(Check_sum > W_weight) //warehouse 1 pick, inventory 1 pick
        {
            
            if(Sour >= W_weight)
                Warehouse.changeSlot_Dele(Sour - W_weight, Inventory_Dic[Des]);
            else if(Des >= W_weight)
                Warehouse.changeSlot_Dele(Des - W_weight, Inventory_Dic[Sour]);

            return;
        }
        else if (Check_sum == (int)Equipmen_index.weapon + (int)Equipmen_index.armour)//status 창에서 slot 두개 pick
            return;


        //status 1 pick, inventory 1 pick
        int snum = int.Parse(Inventory_Dic[Sour].transform.GetChild(0).name);
        
        if ((Sour == (int)Equipmen_index.weapon || Sour == (int)Equipmen_index.armour) && Inventory_Dic[Des].transform.GetChild(0).name != "empty")
        {
            
            int dnum = int.Parse(Inventory_Dic[Des].transform.GetChild(0).name);

            if ((DataLoader.item_Dic[snum].Get_Kind() != DataLoader.item_Dic[dnum].Get_Kind()) || DataLoader.item_Dic[dnum].Get_Level() > SaveDataIO.save.Level)
                return;
        }
        else if (Des == (int)Equipmen_index.weapon || Des == (int)Equipmen_index.armour)
        {
            if (SaveDataIO.save.Level < DataLoader.item_Dic[int.Parse(Inventory_Dic[Sour].transform.GetChild(0).name)].Get_Level())
                return;

            if (Des == (int)Equipmen_index.weapon && DataLoader.item_Dic[int.Parse(Inventory_Dic[Sour].transform.GetChild(0).name)].Get_Kind() != DataLoader.Item_kind.Weapon)
                return;
            else if (Des == (int)Equipmen_index.armour && DataLoader.item_Dic[int.Parse(Inventory_Dic[Sour].transform.GetChild(0).name)].Get_Kind() != DataLoader.Item_kind.Armour)
                return;
            
        }


        Vector2 temp = Inventory_Dic[Sour].transform.GetChild(0).position;
        Inventory_Dic[Sour].transform.GetChild(0).position = Inventory_Dic[Des].transform.GetChild(0).position;
        Inventory_Dic[Des].transform.GetChild(0).position = temp;

        Inventory_Dic[Sour].transform.GetChild(0).SetParent(Inventory_Dic[Des].transform);
        Inventory_Dic[Des].transform.GetChild(0).SetParent(Inventory_Dic[Sour].transform);

    }

    void Swap_SlotforShop(int index, bool IsSell)
    {
        Color color = Inventory_Dic[index].transform.GetChild(0).GetComponent<Image>().color;

        if (IsSell) //판매
        {
            int price = DataLoader.item_Dic[int.Parse(Inventory_Dic[index].transform.GetChild(0).name)].Get_Price();
            //골드 더하기
            Gold.addgold_Dele(price);

            Inventory_Dic[index].transform.GetChild(0).GetComponent<Button>().enabled = false;


            Inventory_Dic[index].transform.GetChild(0).GetComponent<Image>().color = new Color(color.r, color.g, color.b, 0);
            Inventory_Dic[index].transform.GetChild(0).name = "empty";

        }
        else //구매
        {
            //비어있는 index를 찾아야한다.
            int empty_index = -1;

            foreach(var e in Inventory_Dic)
            {
                if (e.Value.transform.GetChild(0).name == "empty")
                {
                    empty_index = e.Key;
                    break;
                }
                    
            }

            if (empty_index == -1)
                Debug.Log("인벤토리가 다찼습니다!");
            else
            {
                int price = DataLoader.item_Dic[index].Get_Price();
                //골드 빼기
                Gold.addgold_Dele(price * -1);

                Inventory_Dic[empty_index].transform.GetChild(0).GetComponent<Button>().enabled = true;
                Inventory_Dic[empty_index].transform.GetChild(0).GetComponent<Image>().sprite = DataLoader.item_Dic[index].Get_Image();

                Inventory_Dic[empty_index].transform.GetChild(0).GetComponent<Image>().color = new Color(color.r, color.g, color.b, 1.0f);
                Inventory_Dic[empty_index].transform.GetChild(0).name = DataLoader.item_Dic[index].Get_Index().ToString();

                
            }




        }
    }

    void Draw_InfoPanel(GameObject obj)
    {
        int inum = int.Parse(obj.transform.name);

        if (inum >= W_weight)
            inum -= W_weight;

        InfoPanel.gameObject.SetActive(true);
        InfoPanel.transform.SetParent(obj.transform.parent.parent.parent.transform);
        InfoPanel.transform.SetAsLastSibling();

        
        float width_weight = InfoPanel.GetComponent<RectTransform>().rect.width;
        float height_weight = InfoPanel.GetComponent<RectTransform>().rect.height;

        Vector3 t_vector = new Vector3(Input.mousePosition.x + width_weight / 2.0f, Input.mousePosition.y + height_weight / 2.0f, 0f);

        if (Input.mousePosition.x + width_weight >= Screen.width)
            t_vector = new Vector3(Input.mousePosition.x - width_weight / 2.0f, Input.mousePosition.y + height_weight / 2.0f, 0f);


        InfoPanel.transform.position = t_vector;

        InfoPanel.transform.GetChild((int)Shop.Child_index.image).GetComponent<Image>().sprite = DataLoader.item_Dic[inum].Get_Image();
        InfoPanel.transform.GetChild((int)Shop.Child_index.name).GetComponent<Text>().text = DataLoader.item_Dic[inum].Get_Name();
        InfoPanel.transform.GetChild((int)Shop.Child_index.kind).GetComponent<Text>().text = DataLoader.item_Dic[inum].Get_Kind().ToString();
        InfoPanel.transform.GetChild((int)Shop.Child_index.info).GetComponent<Text>().text = DataLoader.item_Dic[inum].Get_Info();
        InfoPanel.transform.GetChild((int)Shop.Child_index.price).GetComponent<Text>().text = DataLoader.item_Dic[inum].Get_Price().ToString();


        showPanel_Trigger = true;

    }




    void Inventory_Save()
    {
        Dictionary<int, int> new_inventorydic = new Dictionary<int, int>();

        string temp;

        foreach(var a in Inventory_Dic)
        {
            temp = a.Value.transform.GetChild(0).name;

            if (temp != "empty")
            {
                new_inventorydic.Add(a.Key, int.Parse(temp));
            }


        }

        int tweapon = -1, tarmour = -1;


        if(new_inventorydic.ContainsKey((int)Equipmen_index.weapon))
            tweapon = new_inventorydic[(int)Equipmen_index.weapon];
        if (new_inventorydic.ContainsKey((int)Equipmen_index.armour))
            tarmour = new_inventorydic[(int)Equipmen_index.armour];

        SaveDataIO.save.Equipment[DataLoader.Item_kind.Weapon] = tweapon;
        SaveDataIO.save.Equipment[DataLoader.Item_kind.Armour] = tarmour;

        SaveDataIO.save.Inventory = new_inventorydic;


    }


    public void Click_InventoryButton()
    {
        UIManager.uI_Activator(UIManager.UI_name.inventory);
        
    }

    public void Click_InventoryExitButton()
    {
        
        UIManager.uI_deActivator(UIManager.UI_name.inventory);
    }


    private void OnDisable()
    {
        Inventory_Save();
        SaveDataIO.SaveExport();
    }

}
