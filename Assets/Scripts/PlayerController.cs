using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector2 startPos, endPos, direction;
    //Vector2 startPos, endPos, direction; // touch start position, touch end position, swipe direction
    float touchTimeStart, touchTimeFinish, timeInterval; // to calculate swipe time to control throw force in Z direction

    [SerializeField]
    float throwForceInXandY = 1f; // to control throw force in X and Y directions

    [SerializeField]
    float throwForceInZ = 1f; // to control throw force in Z direction

    Rigidbody rb;

    // パスの効果音を鳴らす定義
    AudioClip getBallSound;
    AudioSource audioSource;

    // パスしたか判定
    public bool isPass = false;

    Dribbling dribblingScript;

    // プレイヤーの配列
    public GameObject[] players;

    // Animatorを入れる変数
    private Animator animator;


    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        getBallSound = Resources.Load<AudioClip>("Audio/ball_hit_rnd_01");
        audioSource = transform.GetComponent<AudioSource>();

        players = GetComponents<GameObject>();



    }


    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            // if you touch the screen
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {

                // getting touch position and working time when you touch the screen
                touchTimeStart = Time.time;
                startPos = Input.GetTouch(0).position;

            }

            // if you release your finger
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {

                // making time when you release it
                touchTimeFinish = Time.time;

                // calculate swipe time interval
                timeInterval = touchTimeFinish - touchTimeStart;

                // getting release finger position
                endPos = Input.GetTouch(0).position;

                // calculating swipe direction in 2D space
                direction = startPos - endPos;

                // 画面右側でスワイプしたらパスorシュートできる
                if (startPos.x > 1100)
                {
                    // add force to ball rigidbody in 3D space depending on swipe time, direction and throw forces
                    rb.isKinematic = false;
                    
                    rb.AddForce(-direction.x * throwForceInXandY, 10f, -direction.y * throwForceInZ, ForceMode.Impulse);
                    //rb.AddForce(-direction.x * throwForceInXandY, throwForceInXandY / timeInterval, -direction.y * throwForceInZ, ForceMode.Impulse);
                    //rb.AddForce(-direction.x * throwForceInXandY, -direction.y * throwForceInXandY, throwForceInZ / timeInterval);

                    foreach (GameObject player in players)
                    {
                        if (player.transform.childCount > 0)
                        {
                            animator = player.GetComponent<Animator>();

                            animator.SetBool("Passing", true);

                            animator.SetBool("Running", false);

                        }
                    }

                    audioSource.PlayOneShot(getBallSound);

                    isPass = true;

                    transform.SetParent(null);

                      

                    // シュートアニメーションを再生
                    //animator.SetBool("Shoot", true);


                    

                }

            }
        }
    }

}

