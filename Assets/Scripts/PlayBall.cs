using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayBall : MonoBehaviour
{
    NavMeshAgent agent;
    public Transform ball;
    public Transform goal;

    // Start is called before the first frame update
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        agent.speed = Random.Range(5.0f, 15.0f);
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 ballToGoal = goal.position - ball.position;

        Vector3 desireDestination = ball.position - ballToGoal.normalized;

        agent.SetDestination(desireDestination);
        if (agent.remainingDistance <= 1)
        {
            if (Vector3.Angle(ballToGoal, this.transform.forward) < 10)
            {
                ball.GetComponent<Rigidbody>().AddForce(this.transform.forward * 30);

            }
            else
            {
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation,
                Quaternion.LookRotation(ballToGoal),
                    Time.deltaTime * agent.angularSpeed);
            }
        }

    }

    }
