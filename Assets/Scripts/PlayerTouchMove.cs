using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTouchMove : MonoBehaviour
{

    private Vector2 startPos, endPos;
    private Vector2 direction;
    //Vector2 startPos, endPos, direction; // touch start position, touch end position, swipe direction
    float touchTimeStart, touchTimeFinish, timeInterval; // to calculate swipe time to control throw force in Z direction


    [SerializeField]
    float throwForceInXandY = 1f; // to control throw force in X and Y directions

    [SerializeField]
    float throwForceInZ = 1f; // to control throw force in Z direction

    Rigidbody rb;

    Vector3 playerPos;

    // 移動スピード
    public float speed = 1f;

    // ジャンプ力
    public float thrust = 200;
    // Animatorを入れる変数
    private Animator animator;
    // 地面に接触しているか否か
    bool ground;

    public GameObject ball;

    PlayerController passScript;

    public float radian;


    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        animator = GetComponent<Animator>();

        // Playerの現在より少し前の位置を保存
        playerPos = transform.position;

        ball = GameObject.Find("Ball");

        // Switch to 640 x 480 full-screen
        Screen.SetResolution(640, 480, true);

        passScript = ball.GetComponent<PlayerController>();
        

    }

    void Update()
    {
        if (ground && IHaveBall())
        {
            // 親オブジェクト(プレイヤー)のlocalPositionに移動
            ball.transform.localPosition = new Vector3(0, 0, 0.5f);


            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        startPos = touch.position;

                        break;

                    case TouchPhase.Moved:
                    if (startPos.x <= 1100)
                        {
                            direction = startPos - touch.position;

                            speed = 3f;

                            //ball.transform.position = transform.position;

                            // 斜め移動用の角度を代入
                            float radian = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;

                            if (radian < 0)
                            {
                                radian += 360; // マイナスの物に360を加算
                            }

                            // 方向判定
                            if (radian <= 22.5f || radian > 337.5f)
                            {
                                // 右向きに移動
                                float x = 1 * Time.deltaTime * speed;
                                rb.MovePosition(transform.position + new Vector3(x, 0, 0));

                                ball.transform.localPosition = new Vector3(0, 0, 0.5f);

                            }
                            else if (radian < 67.5f && radian > 22.5f)
                            {
                                // 右上に移動
                                float x = 1 * Time.deltaTime * speed;
                                float z = 1 * Time.deltaTime * speed;
                                rb.MovePosition(transform.position + new Vector3(x, 0, z));

                                ball.transform.localPosition = new Vector3(0, 0, 0.5f);

                            }
                            else if (radian <= 112.5f && radian > 67.5f)
                            {
                                // 上向きに移動
                                float z = 1 * Time.deltaTime * speed;
                                rb.MovePosition(transform.position + new Vector3(0, 0, z));

                                ball.transform.localPosition = new Vector3(0, 0, 0.5f);

                            }
                            else if (radian <= 157.5f && radian > 112.5f)
                            {
                                // 左上に移動
                                float x = -1 * Time.deltaTime * speed;
                                float z = 1 * Time.deltaTime * speed;
                                rb.MovePosition(transform.position + new Vector3(x, 0, z));

                                ball.transform.localPosition = new Vector3(0, 0, 0.5f);

                            }
                            else if (radian <= 202.5f && radian > 157.5f)
                            {
                                // 左向きに移動
                                float x = -1 * Time.deltaTime * speed;
                                rb.MovePosition(transform.position + new Vector3(x, 0, 0));

                                ball.transform.localPosition = new Vector3(0, 0, 0.5f);

                            }
                            else if (radian <= 247.5f && radian > 202.5f)
                            {
                                // 左下に移動
                                float x = -1 * Time.deltaTime * speed;
                                float z = -1 * Time.deltaTime * speed;
                                rb.MovePosition(transform.position + new Vector3(x, 0, z));

                                ball.transform.localPosition = new Vector3(0, 0, 0.5f);

                            }
                            else if (radian <= 292.5f && radian > 247.5f)
                            {
                                // 下向きに移動
                                float z = -1 * Time.deltaTime * speed;
                                rb.MovePosition(transform.position + new Vector3(0, 0, z));

                                ball.transform.localPosition = new Vector3(0, 0, 0.5f);

                            }
                            else if (radian <= 337.5f && radian > 292.5f)
                            {
                                // 右下に移動
                                float x = 1 * Time.deltaTime * speed;
                                float z = -1 * Time.deltaTime * speed;
                                rb.MovePosition(transform.position + new Vector3(x, 0, z));

                                ball.transform.localPosition = new Vector3(0, 0, 0.5f);

                            }


                            // 移動距離が少しでもあった場合に方向転換
                            if (direction.magnitude > 0.01f)
                            {
                                // directionのX軸とZ軸の方向を向かせる
                                transform.rotation = Quaternion.LookRotation(new Vector3(-direction.x, 0, -direction.y));
                                // 走るアニメーションを再生
                                animator.SetBool("Running", true);
                                //Debug.Log(animator);
                            }
                            else
                            {
                                // ベクトルの長さがない=移動していない時は走るアニメーションはオフ
                                animator.SetBool("Running", false);

                                LookBall();
                            }


                            // Playerの位置を更新する
                            playerPos = transform.position;


                            // スペースキーでジャンプ
                            if (Input.GetButton("Jump"))
                            {
                                // thrustの分だけ上方に力がかかる
                                rb.AddForce(transform.up * thrust);
                                // 速度が出ていたら前方と上方に力がかかる
                                if (rb.velocity.magnitude > 0)
                                    rb.AddForce(transform.forward * thrust + transform.up * thrust);
                            }

                        }

                        break;

                    case TouchPhase.Stationary:

                        if (startPos.x <= 1100)
                        {
                            direction = startPos - touch.position;

                            // 斜め移動用の角度を代入
                            float radian = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;

                            if (radian < 0)
                            {
                                radian += 360; // マイナスの物に360を加算
                            }

                            // 方向判定
                            if (radian <= 22.5f || radian > 337.5f)
                            {
                                // 右向きに移動
                                float x = 1 * Time.deltaTime * speed;
                                rb.MovePosition(transform.position + new Vector3(x, 0, 0));

                                ball.transform.localPosition = new Vector3(0, 0, 0.5f);

                            }
                            else if (radian < 67.5f && radian > 22.5f)
                            {
                                // 右上に移動
                                float x = 1 * Time.deltaTime * speed;
                                float z = 1 * Time.deltaTime * speed;
                                rb.MovePosition(transform.position + new Vector3(x, 0, z));

                                ball.transform.localPosition = new Vector3(0, 0, 0.5f);

                            }
                            else if (radian <= 112.5f && radian > 67.5f)
                            {
                                // 上向きに移動
                                float z = 1 * Time.deltaTime * speed;
                                rb.MovePosition(transform.position + new Vector3(0, 0, z));

                                ball.transform.localPosition = new Vector3(0, 0, 0.5f);

                            }
                            else if (radian <= 157.5f && radian > 112.5f)
                            {
                                // 左上に移動
                                float x = -1 * Time.deltaTime * speed;
                                float z = 1 * Time.deltaTime * speed;
                                rb.MovePosition(transform.position + new Vector3(x, 0, z));

                                ball.transform.localPosition = new Vector3(0, 0, 0.5f);

                            }
                            else if (radian <= 202.5f && radian > 157.5f)
                            {
                                // 左向きに移動
                                float x = -1 * Time.deltaTime * speed;
                                rb.MovePosition(transform.position + new Vector3(x, 0, 0));

                                ball.transform.localPosition = new Vector3(0, 0, 0.5f);

                            }
                            else if (radian <= 247.5f && radian > 202.5f)
                            {
                                // 左下に移動
                                float x = -1 * Time.deltaTime * speed;
                                float z = -1 * Time.deltaTime * speed;
                                rb.MovePosition(transform.position + new Vector3(x, 0, z));

                                ball.transform.localPosition = new Vector3(0, 0, 0.5f);

                            }
                            else if (radian <= 292.5f && radian > 247.5f)
                            {
                                // 下向きに移動
                                float z = -1 * Time.deltaTime * speed;
                                rb.MovePosition(transform.position + new Vector3(0, 0, z));

                                ball.transform.localPosition = new Vector3(0, 0, 0.5f);

                            }
                            else if (radian <= 337.5f && radian > 292.5f)
                            {
                                // 右下に移動
                                float x = 1 * Time.deltaTime * speed;
                                float z = -1 * Time.deltaTime * speed;
                                rb.MovePosition(transform.position + new Vector3(x, 0, z));

                                ball.transform.localPosition = new Vector3(0, 0, 0.5f);

                            }

                            // 移動距離が少しでもあった場合に方向転換
                            if (direction.magnitude > 0.01f)
                            {
                                // directionのX軸とZ軸の方向を向かせる
                                transform.rotation = Quaternion.LookRotation(new Vector3(-direction.x, 0, -direction.y));
                                // 走るアニメーションを再生
                                animator.SetBool("Running", true);
                                //Debug.Log(animator);
                            }
                            else
                            {
                                // ベクトルの長さがない=移動していない時は走るアニメーションはオフ
                                animator.SetBool("Running", false);

                                LookBall();
                            }


                            // Playerの位置を更新する
                            playerPos = transform.position;


                            // スペースキーでジャンプ
                            if (Input.GetButton("Jump"))
                            {
                                // thrustの分だけ上方に力がかかる
                                rb.AddForce(transform.up * thrust);
                                // 速度が出ていたら前方と上方に力がかかる
                                if (rb.velocity.magnitude > 0)
                                    rb.AddForce(transform.forward * thrust + transform.up * thrust);
                            }

                        }

                        break;


                    case TouchPhase.Ended:
                        // ベクトルの長さがない=移動していない時は走るアニメーションはオフ
                        animator.SetBool("Running", false);

                        //passScript.isPass = true;

                        LookBall();
                        break;
                }
            }
        }
    }

           

    // Groundの触れている間作動
    void OnCollisionStay(Collision col)
    {
        ground = true;
        // ジャンプのアニメーションをオフにする
        animator.SetBool("Jumping", false);
    }

    // Groundから離れると作動
    void OnCollisionExit(Collision col)
    {
        ground = false;
        // ジャンプのアニメーションをオンにする
        animator.SetBool("Jumping", true);
    }

    // ボールから離れている時はボールの方を見る
    void LookBall()
    {
        var aim = this.ball.transform.position - this.transform.position;
        var look = Quaternion.LookRotation(aim);
        this.transform.localRotation = look;
    }


    // ボールを保持
    private bool IHaveBall()
    {
        return transform.childCount > 7;
    }

    //void OnTriggerStay(Collider col)
    //{
    //    if (col.gameObject.tag == "Ball")
    //    {
    //        if (Input.touchCount > 0)
    //        {
    //            Touch touch = Input.GetTouch(0);

    //            switch (touch.phase)
    //            {
    //                case TouchPhase.Began:
    //                    startPos = touch.position;
    //                    directionChosen = false;
    //                    break;

    //                case TouchPhase.Moved:
    //                    direction = touch.position - startPos;

    //                    // A・Dキー、←→キーで横移動
    //                    float x = Input.GetAxisRaw("Horizontal") * Time.deltaTime * speed;
    //                    // W・Sキー、↑↓キーで縦移動
    //                    float z = Input.GetAxisRaw("Vertical") * Time.deltaTime * speed;

    //                    // 現在の位置+入力した数値の場所に移動する
    //                    rb.MovePosition(transform.position + new Vector3(x, 0, z));

    //                    // Playerの最新の位置から少し前の位置を引いて方向を割り出す
    //                    //Vector3 direction = transform.position - playerPos;

    //                    // 移動距離が少しでもあった場合に方向転換
    //                    if (direction.magnitude > 0.01f)
    //                    {
    //                        // directionのX軸とZ軸の方向を向かせる
    //                        transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, z));
    //                        // 走るアニメーションを再生
    //                        //animator.SetBool("Running", true);
    //                    }
    //                    else
    //                    {
    //                        // ベクトルの長さがない=移動していない時は走るアニメーションはオフ
    //                        //animator.SetBool("Running", false);

    //                        //LookBall();
    //                    }

    //                    // Playerの位置を更新する
    //                    playerPos = transform.position;
    //                    break;

    //                case TouchPhase.Ended:
    //                    directionChosen = true;
    //                    break;
    //            }
    //        }
    //        if (directionChosen)
    //        {

    //        }
    //    }
    //}

    //void OnTriggerStay(Collider col)
    //{
    //    if (col.gameObject.tag == "Ball")
    //    {
    //        // if you touch the screen
    //        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
    //        {

    //            // getting touch position and working time when you touch the screen
    //            touchTimeStart = Time.time;
    //            startPos = Input.GetTouch(0).position;

    //        }

    //        // if you release your finger
    //        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
    //        {

    //            // making time when you release it
    //            touchTimeFinish = Time.time;

    //            // calculate swipe time interval
    //            timeInterval = touchTimeFinish - touchTimeStart;

    //            // getting release finger position
    //            endPos = Input.GetTouch(0).position;

    //            // calculating swipe direction in 2D space
    //            direction = startPos - endPos;

    //            // 画面右側でスワイプしたらパスorシュートできる
    //            if (startPos.x < 1100)
    //            {
    //                // add force to ball rigidbody in 3D space depending on swipe time, direction and throw forces
    //                rb.isKinematic = false;

    //                rb.AddForce(-direction.x * throwForceInXandY, 10f, -direction.y * throwForceInZ, ForceMode.Impulse);
    //                //rb.AddForce(-direction.x * throwForceInXandY, throwForceInXandY / timeInterval, -direction.y * throwForceInZ, ForceMode.Impulse);
    //                //rb.AddForce(-direction.x * throwForceInXandY, -direction.y * throwForceInXandY, throwForceInZ / timeInterval);


    //            }
    //        }
    //    }
    //}
}
