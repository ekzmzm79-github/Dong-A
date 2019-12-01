using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{

    GameObject player;

    public float dist = 3.0f;
    public float height = 3.0f;

    private Transform Camera;
    private Animator Player_Animator;
    private Transform Player_Tr;


    private int RayObj_LayerNum;
    private int IgnRay_LayerNum;

    //레이캐스트를 통해 투명화 처리된 오브젝트들을 모아둔 큐
    static private Queue<GameObject> Raycated_ObjsQueue = new Queue<GameObject>();

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        player = PlayerManager.get_playerobj();

        Camera = GetComponent<Transform>();
        Player_Animator = player.GetComponent<Animator>();
        Player_Tr = player.GetComponent<Transform>();

        RayObj_LayerNum = LayerMask.NameToLayer("RaycastObj");
        IgnRay_LayerNum = LayerMask.NameToLayer("Ignore Raycast");

    }

    // Update is called once per frame
    void Update()
    {
        RayCheck();
    }

    void LateUpdate()
    {

        if (Player_Animator.GetBool("Walk_Flag") || Player_Animator.GetCurrentAnimatorStateInfo(0).IsName("Hurt"))
        {
            Camera.position = player.transform.position - (1 * Vector3.forward * dist) + (Vector3.up * height);
            Camera.LookAt(player.transform);
        }

    }
    private void RayCheck()
    {
        GameObject target = null;

        //레이캐스트 1차 체크
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 10.0f, (1<<RayObj_LayerNum)) == true)
        {
            target = hit.collider.gameObject;
            //Debug.Log("부딪침:" + target.name);

            if (target.tag == "Player")
            {
                //레이캐스트 2차 체크
                if (Physics.Raycast(transform.position, transform.forward, out RaycastHit ig_hit, 10.0f, (1 << IgnRay_LayerNum)) == true)
                    return;

                
                while (Raycated_ObjsQueue.Count != 0)
                {
                    GameObject temp = Raycated_ObjsQueue.Dequeue();
                    temp.layer = RayObj_LayerNum;
                    Renderer dequeue_renderer = temp.GetComponent<Renderer>();
                    dequeue_renderer.material.color = new Color(dequeue_renderer.material.color.r, dequeue_renderer.material.color.g, dequeue_renderer.material.color.b, 1.0f);

                }
            }
            else
            {
                Raycated_ObjsQueue.Enqueue(target);
                //Debug.Log("인큐: " + target.name);

                target.layer = IgnRay_LayerNum;
                
                Renderer target_renderer = target.GetComponent<Renderer>();
                target_renderer.material.color = new Color(target_renderer.material.color.r, target_renderer.material.color.g, target_renderer.material.color.b, 0.5f);

            }


        }

    }

}
