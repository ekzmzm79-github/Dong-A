using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour
{
    Text FloatTextPrint;

    private float moveSpeed, destroyTime;

    private void Awake()
    {
       FloatTextPrint = GetComponent<Text>();
    }

    // Start is called before the first frame update
    void Start()
    {
        

        moveSpeed = 5f;
        destroyTime = 3f;

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(FloatTextPrint.transform.position.x, FloatTextPrint.transform.position.y + (moveSpeed + Time.deltaTime));

        destroyTime -= Time.deltaTime;

        if (destroyTime <= 0)
            Destroy(gameObject);

    }

    public void print(string Text)
    {
        FloatTextPrint.text = string.Format("{0}", Text);
    }

}
