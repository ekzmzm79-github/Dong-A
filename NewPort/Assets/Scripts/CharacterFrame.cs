using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterFrame : MonoBehaviour
{
    public delegate void SetTrigger_Dele();
    public static SetTrigger_Dele setTrigger_Dele;

    [SerializeField]
    Image Healthbar, Manabar;
    [SerializeField]
    Text Level;

    GameObject Player;
    PlayerManager.PlayerInfo playerInfo;

    bool Trigger;

    private void Awake()
    {
        setTrigger_Dele = Set_Trigger;
    }

    // Start is called before the first frame update
    void Start()
    {
        Setting_Info();
    }

    // Update is called once per frame
    void Update()
    {
        if(Trigger)
        {
            Trigger = false;
            Setting_Info();
        }
            
    }

    void Setting_Info()
    {
        Player = PlayerManager.get_playerobj();
        playerInfo = Player.transform.GetComponent<PlayerManager>().Get_PlayerInfo();

        Level.text = playerInfo.Level.ToString();

        Healthbar.fillAmount = playerInfo.HP / playerInfo.MAX_HP;
        Manabar.fillAmount = playerInfo.MP / playerInfo.MAX_MP;
    }

    void Set_Trigger()
    {
        Trigger = true;
    }

}
