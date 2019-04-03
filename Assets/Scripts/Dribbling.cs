using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dribbling : MonoBehaviour
{
    Transform ControllingFeet;

    public bool keeping;
    // Start is called before the first frame update
    void Start()
    {
        keeping = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "EthanRightFoot" && keeping == false)
        {
            ControllingFeet = other.transform;
            keeping = true;
        }
    }

    void FixedUpdate()
    {
        bool isControlled = ControllingFeet != null;

        if (isControlled && keeping == true)
        {

            //Vector3 targetPos = ControllingFeet.transform.position;

            Vector3 targetPos = new Vector3(ControllingFeet.transform.position.x + 1.5f, ControllingFeet.transform.position.y, ControllingFeet.transform.position.z);
            GetComponent<Rigidbody>().MovePosition(targetPos);


        }
    }
}
