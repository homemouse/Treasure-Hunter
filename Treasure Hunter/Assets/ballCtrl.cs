using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballCtrl : MonoBehaviour
{
    [SerializeField] Rigidbody2D rigidbody2d;
    [SerializeField] float speed = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        bool key = Input.GetKeyDown(KeyCode.Space);

        if (rigidbody2d != null && (x != 0 || y != 0)) 
        {
            rigidbody2d.MovePosition(new Vector2(rigidbody2d.position.x + (x * Time.deltaTime * speed), rigidbody2d.position.y + (y * Time.deltaTime * speed)));
        }

        if(rigidbody2d != null && key)
        {
            rigidbody2d.MovePosition(new Vector2(rigidbody2d.position.x + +10, rigidbody2d.position.y));

        }

    }
}
