using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using System.Linq;
public class whipCtrl : MonoBehaviour
{
    [Header("Game Object")]
    [SerializeField] Rigidbody2D attackPoint;
    [SerializeField] GameObject hook;
    [SerializeField] GameObject rode;
    [SerializeField] GameObject knob;

    [Header("Property")]
    [SerializeField] int rode_number = 20;
    [SerializeField] bool whipActive;

    GameObject curHook;
    List<GameObject> curRode_List = new List<GameObject>();
    GameObject curKnob;

    // Start is called before the first frame update
    void Start()
    {
        curHook = Instantiate(hook, this.transform.position, Quaternion.identity);
        for (int i = 0; i < rode_number; i++)
        {
            curRode_List.Add(Instantiate(rode, this.transform.position, Quaternion.identity));
            if (i == 0)
                curHook.GetComponent<DistanceJoint2D>().connectedBody = curRode_List[0].GetComponent<Rigidbody2D>();
            else
                curRode_List[i - 1].GetComponent<DistanceJoint2D>().connectedBody = curRode_List[i].GetComponent<Rigidbody2D>();


        }
        curKnob = Instantiate(knob, this.transform.position, Quaternion.identity);
        curRode_List.Last().GetComponent<DistanceJoint2D>().connectedBody = curKnob.GetComponent<Rigidbody2D>();
        curKnob.GetComponent<DistanceJoint2D>().connectedBody = this.GetComponent<Rigidbody2D>();
        //curKnob.GetComponent<DistanceJoint2D>().connectedBody = attackPoint;

        DistanceJoint2D distanceJoint2D = curHook.GetComponent<DistanceJoint2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    whipping();
        //}

    }

    private void whipping()
    {
        if (whipActive == false)
        {
            //Vector2 destiny = Camera.main.ScreenToWorldPoint (Input.mousePosition);

            Vector3 Pos = Camera.main.WorldToScreenPoint(transform.position);
            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Pos.z);
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(mousePos);

            curHook = Instantiate(hook, transform.position, Quaternion.identity);


            whipActive = true;
        }
        else
        {

            //delete rope

            Destroy(curHook);
            curHook = null;

            whipActive = false;

        }
    }
}
