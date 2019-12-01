using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public enum Camera_name { main, shop, warehouse };

    public delegate void Swap_Camera_Dele(Camera_name name);
    public static Swap_Camera_Dele swap_Camera_Dele;

    [SerializeField]
    Camera main_camera, shop_camera, warehouse_camera;
    // Training_camera;

    void Awake()
    {
        swap_Camera_Dele = Swap_Camera;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void Swap_Camera(Camera_name name)
    {
        
        Camera.main.gameObject.SetActive(false);

        if (name == Camera_name.main)
        {
            main_camera.gameObject.SetActive(true);
            PlayerManager.trans_BodyDele(false);
        }
        else if (name == Camera_name.shop)
        {
            shop_camera.gameObject.SetActive(true);
            PlayerManager.trans_BodyDele(true);
        }
        else if(name == Camera_name.warehouse)
        {
            warehouse_camera.gameObject.SetActive(true);
            PlayerManager.trans_BodyDele(true);
        }
        
    }

}
