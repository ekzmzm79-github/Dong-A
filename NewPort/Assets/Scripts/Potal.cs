using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Potal : MonoBehaviour
{

    GameObject Player;

    // Start is called before the first frame update

    private bool Flag;
    void Start()
    {
        Player = PlayerManager.get_playerobj();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, Player.transform.position) < 3.0f)
        {
            if (!Flag)
            {
                Flag = true;
                UIManager.uI_Activator(UIManager.UI_name.portal);
                Debug.Log("근접");
            }
        }
        else
        {
            //Debug.Log("멀어짐");
            Flag = false;
            UIManager.uI_deActivator(UIManager.UI_name.portal);
        }


    }

    public void Exit_Button()
    {
        UIManager.uI_deActivator(UIManager.UI_name.portal);
    }

    public void Enter_Button()
    {
        LoadingSceneManager.LoadScene("Stage");
        
    }

}
