using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NPC : NPC_Parent
{

    // Start is called before the first frame update
    void Start()
    {
        player = PlayerManager.get_playerobj();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void OnMouseDown()
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= 1.5f)
        {
            
            NPCManager.activator_Dele(this.gameObject.name);

        }

    }
}
