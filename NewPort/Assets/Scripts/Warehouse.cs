using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Warehouse : MonoBehaviour
{
    public delegate void SwapSlot_Dele(int Sour, int Des);
    public static SwapSlot_Dele swapSlot_Dele;

    public delegate void ChangeSlot_Dele(int W_index, GameObject target);
    public static ChangeSlot_Dele changeSlot_Dele;

    [SerializeField]
    GameObject first_slot, Slot_Prefab;

    private int W_weight = 100;

    private Vector3 first_positon;
    private float horizon_Gap = 130f, vertical_Gap = -120f;
    private int X_many = 6, Y_many = 2;

    private Dictionary<int, GameObject> Warehouse_Dic = new Dictionary<int, GameObject>();

    private void Awake()
    {
        //Delegate 할당
        swapSlot_Dele = Swap_Slot;
        changeSlot_Dele = Change_Slot;

        first_positon = first_slot.transform.localPosition;
        
    }

    private void OnEnable()
    {
        Draw_Warehouse();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Draw_Warehouse()
    {
        SaveDataIO.SaveTake();

        for (int j = 0; j < Y_many; j++)
        {
            for (int i = 0; i < X_many; i++)
            {
                GameObject slot = Instantiate(Slot_Prefab) as GameObject;
                slot.transform.SetParent(this.gameObject.transform, true);
                slot.transform.localPosition = new Vector3(first_positon.x + (i * horizon_Gap), first_positon.y + (j * vertical_Gap), 0f);
                slot.gameObject.name = "Wslot_" + (i + j * X_many + W_weight);

                Warehouse_Dic[i + j * X_many] = slot;

                if (SaveDataIO.save.Warehouse.ContainsKey(i + j * X_many))
                {
                    Warehouse_Dic[i + j * Y_many].transform.GetChild(0).GetComponent<Button>().enabled = true;
                    Warehouse_Dic[i + j * Y_many].transform.GetChild(0).GetComponent<Image>().sprite = DataLoader.item_Dic[SaveDataIO.save.Warehouse[i + j * X_many]].Get_Image();

                    Color color = Warehouse_Dic[i + j * X_many].transform.GetChild(0).GetComponent<Image>().color;
                    Warehouse_Dic[i + j * X_many].transform.GetChild(0).GetComponent<Image>().color = new Color(color.r, color.g, color.b, 1.0f);
                    Warehouse_Dic[i + j * X_many].transform.GetChild(0).name = SaveDataIO.save.Warehouse[i + j * X_many].ToString();
                }


            }
        }
    }



    void Swap_Slot(int Sour, int Des)
    {
        Vector2 temp = Warehouse_Dic[Sour].transform.GetChild(0).position;
        Warehouse_Dic[Sour].transform.GetChild(0).position = Warehouse_Dic[Des].transform.GetChild(0).position;
        Warehouse_Dic[Des].transform.GetChild(0).position = temp;

        Warehouse_Dic[Sour].transform.GetChild(0).SetParent(Warehouse_Dic[Des].transform);
        Warehouse_Dic[Des].transform.GetChild(0).SetParent(Warehouse_Dic[Sour].transform);
    }

    void Change_Slot(int W_index, GameObject target)
    {
        Vector2 temp = Warehouse_Dic[W_index].transform.GetChild(0).position;
        Warehouse_Dic[W_index].transform.GetChild(0).position = target.transform.GetChild(0).position;
        target.transform.GetChild(0).position = temp;

        Warehouse_Dic[W_index].transform.GetChild(0).SetParent(target.transform);
        target.transform.GetChild(0).SetParent(Warehouse_Dic[W_index].transform);
    }


    void Warehouse_Save()
    {
        Dictionary<int, int> new_warehousedic = new Dictionary<int, int>();

        string temp;

        foreach (var a in Warehouse_Dic)
        {
            temp = a.Value.transform.GetChild(0).name;

            if (temp != "empty")
            {
                new_warehousedic.Add(a.Key, int.Parse(temp));
            }


        }


        SaveDataIO.save.Warehouse = new_warehousedic;


    }



    public void Click_WarehouseExitButton()
    {

        CameraManager.swap_Camera_Dele(CameraManager.Camera_name.main);
        UIManager.uI_Activator(UIManager.UI_name.main);
        UIManager.uI_deActivator(UIManager.UI_name.warehouse);
    }

    private void OnDisable()
    {
        Warehouse_Save();
        SaveDataIO.SaveExport();
    }

}
