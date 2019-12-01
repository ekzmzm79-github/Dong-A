using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageMinimap : MonoBehaviour
{
    public delegate void Active_BlinderDele(int num);
    public static Active_BlinderDele active_BlinderDele;

    GameObject Player, PlayerCube;
    Animator Player_Ainmator;

    [SerializeField]
    GameObject[] Map_Blinders = new GameObject[9]; 
    // Start is called before the first frame update
    void Start()
    {
        active_BlinderDele = Active_Blinder;

        Player = PlayerManager.get_playerobj();
        Player_Ainmator = Player.GetComponent<Animator>();
        PlayerCube = this.gameObject;

        Reset_Blinders();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerCube.transform.position = new Vector3(Player.transform.position.x / 10.0f, PlayerCube.transform.position.y, Player.transform.position.z / 10.0f);

    }

    private void Reset_Blinders()
    {
        for(int i=0; i< Map_Blinders.Length; i++)
        {
            Map_Blinders[i].SetActive(false);
        }
    }

    public void Active_Blinder(int num)
    {
        Map_Blinders[num].SetActive(true);
    }

}
