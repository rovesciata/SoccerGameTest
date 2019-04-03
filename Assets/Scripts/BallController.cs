using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{

    Vector3 startingPosition;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = this.transform.position;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Goal" || col.gameObject.tag == "Wall")
        {

            Invoke("BallReStart", 1f);
        }
    }

    void BallReStart()
    {
        this.transform.position = startingPosition;
        rb.isKinematic = true;
        rb.isKinematic = false;


    }



}
