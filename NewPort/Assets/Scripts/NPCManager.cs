using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NPC_Parent : MonoBehaviour
{
    
    protected GameObject player;

    protected abstract void OnMouseDown();
}

public class NPCManager : MonoBehaviour
{

    public delegate void Activator_Dele(string name);
    public static Activator_Dele activator_Dele;

    void Awake()
    {
        activator_Dele = Activator;
    }

    // Start is called before the first frame update
    void Start()
    {
        
        



    }

    // Update is called once per frame
    void Update()
    {





    }

    private void Activator(string name)
    {
        if (name == "ShopNPC" && Camera.main.name != "ShopCamera")
        {
            CameraManager.swap_Camera_Dele(CameraManager.Camera_name.shop);
            UIManager.uI_Activator(UIManager.UI_name.shop);
            UIManager.uI_deActivator(UIManager.UI_name.main);
            
            
        }
        else if(name == "WarehouseNPC")
        {
            CameraManager.swap_Camera_Dele(CameraManager.Camera_name.warehouse);
            UIManager.uI_Activator(UIManager.UI_name.warehouse);
            UIManager.uI_deActivator(UIManager.UI_name.main);
        }

    }

}



//Raycast를 통해 NPC 클릭여부 판단 소스
/* 해당 부분은 Updata에 추가
if (Vector3.Distance(transform.position, PlayerTr.position) <= 1.5f)
{
    if (Input.GetMouseButtonDown(0))
    {
        target = GetClickedObject();

        if(target.Equals(gameObject))
        {
            Debug.Log("성공");
        }


    }

}
*/

/* MouseClick으로 Raycast를 쏴서 판별
 * private GameObject target; --> 변수

private GameObject GetClickedObject()
{
    RaycastHit hit;
    GameObject target = null;

    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

    if((Physics.Raycast(ray.origin, ray.direction * 10, out hit)) == true)
    {
        target = hit.collider.gameObject;
    }

    return target;
}
*/
