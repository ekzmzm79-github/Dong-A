using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Status : MonoBehaviour
{
    [SerializeField]
    GameObject WeaponSlot, ArmourSlot;

    //private Dictionary<string, int> equipment_dic = new Dictionary<string, int>();

    // Start is called before the first frame update
    void Start()
    {
        Equipment_Setting();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Equipment_Setting()
    {
        SaveDataIO.SaveTake();
        /*
        if(SaveDataIO.save.Equipment["Weapon"] != -1)
        {
            
            Color color = WeaponSlot.transform.GetChild(0).GetComponent<Image>().color;
            WeaponSlot.transform.GetChild(0).GetComponent<Image>().color = new Color(color.r, color.g, color.b, 1.0f);
            WeaponSlot.transform.GetChild(0).name = SaveDataIO.save.Equipment["Weapon"].ToString();

        }

        if (SaveDataIO.save.Equipment["Armour"] != -1)
        {

            Color color = ArmourSlot.transform.GetChild(0).GetComponent<Image>().color;
            ArmourSlot.transform.GetChild(0).GetComponent<Image>().color = new Color(color.r, color.g, color.b, 1.0f);
            ArmourSlot.transform.GetChild(0).name = SaveDataIO.save.Equipment["Armour"].ToString();

        }
        */

    }

}
