using UnityEngine;
using System.Collections;

public class throwhook : MonoBehaviour {


	public GameObject hook;


	public bool ropeActive;

	GameObject curHook;

	// Use this for initialization
	void Start () {
	
	}
    private void FixedUpdate()
    {
        
    }
    // Update is called once per frame
    void Update () {
	


		if (Input.GetMouseButtonDown (1)) {


			if (ropeActive == false) {
				//Vector2 destiny = Camera.main.ScreenToWorldPoint (Input.mousePosition);

                Vector3 Pos = Camera.main.WorldToScreenPoint(transform.position);
                Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Pos.z);
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(mousePos);

                curHook = (GameObject)Instantiate (hook, transform.position, Quaternion.identity);

				//curHook.GetComponent<RopeScript> ().destiny = destiny;
				curHook.GetComponent<RopeScript> ().destiny = mousePosition;


                ropeActive = true;
			} else {

				//delete rope

				Destroy (curHook);


				ropeActive = false;

			}
		}


	}
}
