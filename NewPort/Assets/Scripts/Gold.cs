using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gold : MonoBehaviour
{
    public delegate void Addgold_Dele(int value);
    public static Addgold_Dele addgold_Dele;

    Text gold;


    private void Awake()
    {
        gold = GetComponent<Text>();
        addgold_Dele = Add_gold;
    }

    private void OnEnable()
    {
        Update_gold();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Update_gold()
    {
        SaveDataIO.SaveTake();
        gold.text = SaveDataIO.save.Gold.ToString();
    }

    void Add_gold(int value)
    {
        SaveDataIO.SaveTake();
        int p_gold = int.Parse(gold.text);
        SaveDataIO.save.Gold += value;

        SaveDataIO.SaveExport();
        Update_gold();
    }

}
