using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    GameObject Player, PlayerCube;
    Animator Player_Ainmator;


    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        Player = PlayerManager.get_playerobj();
        Player_Ainmator = Player.GetComponent<Animator>();
        PlayerCube = this.gameObject;

    }

    // Update is called once per frame
    void Update()
    {
        
        if (Player_Ainmator.GetBool("Walk_Flag"))
        {
            PlayerCube.transform.position = new Vector3(Player.transform.position.x / 10.0f, PlayerCube.transform.position.y, Player.transform.position.z / 10.0f);

        }
    }
}
