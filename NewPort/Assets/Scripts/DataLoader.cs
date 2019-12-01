using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using LitJson;
using Newtonsoft.Json;


public class DataLoader : MonoBehaviour
{
    public enum Item_kind { Weapon, Armour, Common };
    public enum Monster_kind { Weak = 0, Nomal = 1, Strong = 2 , Boss = 101 };
    public enum Skill_kind { Nomal, Combo, Buff, Missile, Ultimate };

    public static Dictionary<int, Equipment> item_Dic = new Dictionary<int, Equipment>();
    public static Dictionary<int, Monster> monster_Dic = new Dictionary<int, Monster>();
    public static Dictionary<int, Skill> skill_Dic = new Dictionary<int, Skill>();

    void Awake()
    {
        Ini_Setting();

    }
    


    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void Ini_Setting()
    {
        string JsonStr = File.ReadAllText(DataPathStringClass.DataPathString() + "/JsonData/Item_Data.json");
        JsonData jsondata = JsonMapper.ToObject(JsonStr);

        for (int i = 0; i < jsondata.Count; i++)
        {
            Equipment temp = new Equipment(int.Parse(jsondata[i]["Index"].ToString()), (Item_kind)System.Enum.Parse(typeof(Item_kind), jsondata[i]["Kind"].ToString()), jsondata[i]["Name"].ToString(), int.Parse(jsondata[i]["Level"].ToString()), int.Parse(jsondata[i]["Stat"].ToString()), int.Parse(jsondata[i]["Price"].ToString()), jsondata[i]["Info"].ToString(), bool.Parse(jsondata[i]["IsSell"].ToString()));

            item_Dic[temp.Get_Index()] = temp;

        }

        JsonStr = File.ReadAllText(DataPathStringClass.DataPathString() + "/JsonData/Monster_Data.json");
        jsondata = JsonMapper.ToObject(JsonStr);

        for (int i = 0; i < jsondata.Count; i++)
        {
            Monster temp = new Monster(int.Parse(jsondata[i]["Index"].ToString()), (Monster_kind)System.Enum.Parse(typeof(Monster_kind), jsondata[i]["Kind"].ToString()), jsondata[i]["Name"].ToString(), int.Parse(jsondata[i]["Level"].ToString()), int.Parse(jsondata[i]["Many"].ToString()), float.Parse(jsondata[i]["HP"].ToString()), float.Parse(jsondata[i]["Attack"].ToString()), float.Parse(jsondata[i]["Defence"].ToString()), float.Parse(jsondata[i]["AttackSpeed"].ToString()), float.Parse(jsondata[i]["MoveSpeed"].ToString()), float.Parse(jsondata[i]["Exp"].ToString()), jsondata[i]["Info"].ToString());

            monster_Dic[temp.Get_Index()] = temp;
        }

        JsonStr = File.ReadAllText(DataPathStringClass.DataPathString() + "/JsonData/Skill_Data.json");
        jsondata = JsonMapper.ToObject(JsonStr);

        for (int i = 0; i < jsondata.Count; i++)
        {
            Skill temp = new Skill(int.Parse(jsondata[i]["Index"].ToString()), (Skill_kind)System.Enum.Parse(typeof(Skill_kind), jsondata[i]["Kind"].ToString()), jsondata[i]["Name"].ToString(), int.Parse(jsondata[i]["Count"].ToString()), float.Parse(jsondata[i]["Damage"].ToString()), float.Parse(jsondata[i]["Delay"].ToString()), float.Parse(jsondata[i]["Range"].ToString()), float.Parse(jsondata[i]["Cost"].ToString()), jsondata[i]["Info"].ToString());

            skill_Dic[temp.Get_Index()] = temp;
        }


        //SaveDataIO.SaveReset();
        //temp_savedata();
    }


    private void temp_savedata()
    {
        //SaveDataIO.SaveTake();

        SaveDataIO.save.HP = 300;
        SaveDataIO.save.MP = 100;
        SaveDataIO.save.Level = 1;
        SaveDataIO.save.Gold = 1000;
        
        SaveDataIO.save.Equipment[Item_kind.Weapon] = 0;
        SaveDataIO.save.Equipment[Item_kind.Armour] = 1;

        SaveDataIO.save.Warehouse[0] = 6;
        SaveDataIO.save.Warehouse[1] = 7;
        SaveDataIO.save.Warehouse[2] = 8;
        SaveDataIO.save.Warehouse[3] = 10;

        SaveDataIO.save.Inventory[0] = 50;
        SaveDataIO.save.Inventory[1] = 51;
        SaveDataIO.save.Inventory[5] = 53;

        SaveDataIO.SaveExport();


    }


}

public class Skill
{
    private int Index;
    private DataLoader.Skill_kind Kind;
    private string Name;
    private int Count;
    private float Damage;
    private float Delay;
    private float Range;
    private float Cost;
    private string Info;

    public Skill() { }
    public Skill(int _Index, DataLoader.Skill_kind _Kind, string _Name, int _Count, float _Damage, float _Delay, float _Range, float _Cost, string _Info)
    {
        Index = _Index;
        Kind = _Kind;
        Name = _Name;
        Count = _Count;
        Damage = _Damage;
        Delay = _Delay;
        Range = _Range;
        Cost = _Cost;
        Info = _Info;
    }

    public int Get_Index() { return Index; }
    public DataLoader.Skill_kind Get_Kind() { return Kind; }
    public string Get_Name() { return Name; }
    public int Get_Count() { return Count; }
    public float Get_Damage() { return Damage; }
    public float Get_Delay() { return Delay; }
    public float Get_Range() { return Range; }
    public float Get_Cost() { return Cost; }
    public string Get_Info() { return Info; }


}


public class Monster
{
    private int Index;
    private DataLoader.Monster_kind Kind;
    private string Name;
    private int Level;
    private int Many;
    private float HP;
    private float Attack;
    private float Defence;
    private float AttackSpeed;
    private float MoveSpeed;
    private float Exp;
    private string Info;
    private GameObject Prefab;

    public Monster() { }
    public Monster(int _Index, DataLoader.Monster_kind _Kind, string _Name, int _Level, int _Many, float _HP, float _Attack, float _Defence, float _AttackSpeed, float _MoveSpeed, float _Exp, string _Info)
    {
        Index = _Index;
        Kind = _Kind;
        Name = _Name;
        Level = _Level;
        Many = _Many;
        HP = _HP;
        Attack = _Attack;
        Defence = _Defence;
        AttackSpeed = _AttackSpeed;
        MoveSpeed = _MoveSpeed;
        Exp = _Exp;
        Info = _Info;

        Prefab = Resources.Load<GameObject>("Monsters/" + Name);

    }

    public int Get_Index(){ return Index; }
    public DataLoader.Monster_kind Get_Kind() { return Kind; }
    public string Get_Name() { return Name; }
    public int Get_Level() { return Level; }
    public int Get_Many() { return Many; }
    public float Get_HP() { return HP; }
    public float Get_Attack() { return Attack; }
    public float Get_Defence() { return Defence; }
    public float Get_AttackSpeed() { return AttackSpeed; }
    public float Get_MoveSpeed() { return MoveSpeed; }
    public float Get_Exp() { return Exp; }
    public string Get_Info() { return Info; }
    public GameObject Get_Prefab() { return Prefab; }

}



public class Equipment
{
    private int Index;
    private DataLoader.Item_kind Kind;
    private string Name;
    private int Level;
    private int Stat;
    private int Price;
    private string Info;
    private bool IsSell;
    private Sprite Image;

    public Equipment() { }

    public Equipment(int _Index, DataLoader.Item_kind _Kind, string _Name, int _Level, int _Stat, int _Price, string _Info, bool _IsSell)
    {
        Index = _Index;
        Kind = _Kind;
        Name = _Name;
        Level = _Level;
        Stat = _Stat;
        Price = _Price;
        Info = _Info;
        IsSell = _IsSell;

        Image = Resources.Load<Sprite>("Item_Images/" + Name);

    }
    
    public int Get_Index(){ return Index; }
    public DataLoader.Item_kind Get_Kind(){ return Kind; }
    public string Get_Name(){ return Name; }
    public int Get_Level(){ return Level; }
    public int Get_Stat(){ return Stat; }
    public int Get_Price(){ return Price; }
    public string Get_Info(){ return Info; }
    public bool Get_IsSell(){ return IsSell; }
    public Sprite Get_Image(){ return Image; }
}


public class SaveData
{
    //현재 HP, MP
    public int HP;
    public int MP;
    public int Level;
    public int Gold;

    public Dictionary<DataLoader.Item_kind, int> Equipment = new Dictionary<DataLoader.Item_kind, int>(); //key: "weapon" or "armour", value: item index
    public Dictionary<int, int> Inventory = new Dictionary<int, int>(); //key: index, value:item index
    public Dictionary<int, int> Warehouse = new Dictionary<int, int>();

    /* 
    public List<int> Inventory = new List<int>();
    public List<int> Warehouse = new List<int>();
    
    public int[] Inventory = new int[36];
    public int[] Warehouse = new int[50];
    */

}

public static class SaveDataIO
{
    private static string JsonStr;
    public static SaveData save = new SaveData();

    public static void SaveTake()
    {
        JsonStr = File.ReadAllText(DataPathStringClass.DataPathString() + "/JsonData/SaveData.json");

        save = JsonConvert.DeserializeObject<SaveData>(JsonStr);
    }

    public static void SaveExport()
    {
        JsonStr = JsonConvert.SerializeObject(save);

        File.WriteAllText(DataPathStringClass.DataPathString() + "/JsonData/SaveData.json", JsonStr);
    }

    public static void SaveReset()
    {
        SaveData save = new SaveData();

        JsonStr = JsonConvert.SerializeObject(save);

        File.WriteAllText(DataPathStringClass.DataPathString() + "/JsonData/SaveData.json", JsonStr);
    }
}



public static class DataPathStringClass
{

    public static string DataPathString()
    {
#if UNITY_EDITOR
        string path = Application.dataPath;
        return path;
#elif UNITY_ANDROID
        
        string path = Application.persistentDataPath;

        if (!Directory.Exists(path + "/Save"))
        {
            //Create SaveFolder

        }


        return path;
#endif
    }




}