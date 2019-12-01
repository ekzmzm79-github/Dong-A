using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{

    

    public float Damage;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * 0.1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Wall")
            Destroy(gameObject);
    }

}
