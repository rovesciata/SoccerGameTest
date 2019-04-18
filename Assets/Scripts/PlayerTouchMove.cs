using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTouchMove : MonoBehaviour
{
    // タッチ開始位置、指を離した位置、スワイプの方向を入れる変数
    private Vector2 startPos, endPos, direction;
    // プレイヤーのRigidbodyを入れる変数
    Rigidbody rb;
    // プレイヤーの位置を取得
    Vector3 playerPos;
    // 移動スピード
    public float speed = 1f;
    // ジャンプ力
    //public float thrust = 200;
    // Animatorを入れる変数
    private Animator animator;
    // 地面に接触しているか否か
    bool ground;
    // ボールを入れる変数
    public GameObject ball;
    // Passingクラスを入れる変数
    Passing passScript;
    // ８方向移動用の変数
    public float radian;


    // Use this for initialization
    void Start()
    {
        // Rigidbodyを取得
        rb = GetComponent<Rigidbody>();
        // Animatorを取得
        animator = GetComponent<Animator>();
        // Playerの現在より少し前の位置を保存
        playerPos = transform.position;
        // ボールを取得
        ball = GameObject.Find("Ball");
        // ボールのPassingクラスを取得
        passScript = ball.GetComponent<Passing>();
    }

    // ドリブル
    void Update()
    {
        // 地面に接触していて、ボールを保持している場合
        if (ground && IHaveBall())
        {
            // 親オブジェクト(プレイヤー)のlocalPositionに移動
            ball.transform.localPosition = new Vector3(0, 0, 0.5f);
            // タッチした時
            if (Input.touchCount > 0)
            {
                // タッチ操作を取得
                Touch touch = Input.GetTouch(0);

                // タッチの状態による操作
                switch (touch.phase)
                {
                    // タッチ開始時
                    case TouchPhase.Began:
                        // タッチ開始時の位置を取得
                        startPos = touch.position;

                        break;

                 　 // 指を動かしている時
                    case TouchPhase.Moved:

                    // 画面左をスワイプした時
                    if (startPos.x <= 1100)
                        {
                            // スワイプ方向を取得
                            direction = startPos - touch.position;
                            
                            speed = 3f;

                            // ドリブル
                            Dribbling();
                        }

                        break;

                        // 指を押したままの時
                    case TouchPhase.Stationary:

                        if (startPos.x <= 1100)
                        {
                            direction = startPos - touch.position;

                            //ドリブル
                            Dribbling();
                        }

                        break;

                        // 指を離した時
                    case TouchPhase.Ended:

                        // ベクトルの長さがない=移動していない時は走るアニメーションはオフ
                        animator.SetBool("Running", false);
                        // ボールの方を見る
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
        //animator.SetBool("Jumping", false);
    }

    // Groundから離れると作動
    void OnCollisionExit(Collision col)
    {
        ground = false;
        // ジャンプのアニメーションをオンにする
        //animator.SetBool("Jumping", true);
    }

    // ボールから離れている時はボールの方を見る
    void LookBall()
    {
        // プレイヤーの位置とボールの位置の差分から方向を取得
        var aim = this.ball.transform.position - this.transform.position;
        var look = Quaternion.LookRotation(aim);
        // プレイヤーの向きをボールの方へ向ける
        this.transform.localRotation = look;
    }

    // ボールを保持
    public bool IHaveBall()
    {
        return transform.childCount > 7;
    }

    // ボールを保持していない時
    public bool IDontHaveBall()
    {
        return transform.childCount <= 7;
    }

    // ドリブル
    void Dribbling()
    {
        // ８方向移動用の角度を代入
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
            // ボールをプレイヤーの少し手前にする
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
        //if (Input.GetButton("Jump"))
        //{
        //    // thrustの分だけ上方に力がかかる
        //    rb.AddForce(transform.up * thrust);
        //    // 速度が出ていたら前方と上方に力がかかる
        //    if (rb.velocity.magnitude > 0)
        //        rb.AddForce(transform.forward * thrust + transform.up * thrust);
        //}
}
}
