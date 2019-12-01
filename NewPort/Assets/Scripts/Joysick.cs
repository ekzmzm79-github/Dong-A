using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Joysick : MonoBehaviour
{

    [SerializeField]
    Transform Joystick;

    GameObject Player;
    private float Player_MoveSpeed = 0.0f;

    private Vector3 Stick_FirstPos;
    private Vector3 Stick_Dir;
    private float Radius;

    // Start is called before the first frame update
    void Start()
    {
        Player = PlayerManager.get_playerobj();
        Radius = GetComponent<RectTransform>().sizeDelta.y * 0.5f;

        Stick_FirstPos = Joystick.transform.position;
        Player_MoveSpeed = Player.GetComponent<Animator>().GetFloat("Walk_Speed");


        float temp = transform.parent.GetComponent<RectTransform>().localScale.x;
        Radius *= temp;

        

    }

    // Update is called once per frame
    void Update()
    {
        if (Player.GetComponent<Animator>().GetBool("Walk_Flag"))
        {
            Player.transform.Translate(Vector3.forward * Time.deltaTime * Player_MoveSpeed);

        }
    }

    public void Drag(BaseEventData Data)
    {
        if (Player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            Player.GetComponent<Animator>().SetBool("Walk_Flag", true);
        }
            

        //드래그 시작 시 마우스의 위치 찾기
        PointerEventData p_Data = Data as PointerEventData;
        Vector3 p_Pos = p_Data.position;

        //드래그 중에 처음 위치와의 거리를 계속 구함
        Stick_Dir = (p_Pos - Stick_FirstPos).normalized;

        float Dis = Vector3.Distance(p_Pos, Stick_FirstPos);

        if (Dis < Radius)
            Joystick.position = Stick_FirstPos + Stick_Dir * Dis;
        else
            Joystick.position = Stick_FirstPos + Stick_Dir * Radius;

        Player.transform.eulerAngles = new Vector3(0, Mathf.Atan2(Stick_Dir.x, Stick_Dir.y) * Mathf.Rad2Deg, 0);
        
    }

    public void DragEnd()
    {
        Joystick.position = Stick_FirstPos;
        Stick_Dir = Vector3.zero;
        Player.GetComponent<Animator>().SetBool("Walk_Flag", false);
    }

}
