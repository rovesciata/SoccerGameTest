using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    private Vector2 startPos, endPos, direction;
    //Vector2 startPos, endPos, direction; // touch start position, touch end position, swipe direction
    float touchTimeStart, touchTimeFinish, timeInterval; // to calculate swipe time to control throw force in Z direction


    // Rigidbodyを変数に入れる
    Rigidbody rb;
    // 移動スピード
    public float speed = 3f;
    // ジャンプ力
    public float thrust = 200;
    // Animatorを入れる変数
    private Animator animator;
    // Playerの位置を入れる
    Vector3 playerPos;
    // 地面に接触しているか否か
    bool ground;

    GameObject ball;

    // Start is called before the first frame update
    void Start()
    {
        // Rigidbodyを取得
        rb = GetComponent<Rigidbody>();
        // PlayerのAnimatorにアクセスする
        animator = GetComponent<Animator>();
        // Playerの現在より少し前の位置を保存
        playerPos = transform.position;

        ball = GameObject.Find("Ball");

    }





            // Update is called once per frame
            void Update()
            {
                if (ground)
                {
                    // A・Dキー、←→キーで横移動
                    float x = Input.GetAxisRaw("Horizontal") * Time.deltaTime * speed;
                    // W・Sキー、↑↓キーで縦移動
                    float z = Input.GetAxisRaw("Vertical") * Time.deltaTime * speed;

                    // 現在の位置+入力した数値の場所に移動する
                    rb.MovePosition(transform.position + new Vector3(x, 0, z));

                    // Playerの最新の位置から少し前の位置を引いて方向を割り出す
                    Vector3 direction = transform.position - playerPos;

                    // 移動距離が少しでもあった場合に方向転換
                    if (direction.magnitude > 0.01f)
                    {
                        // directionのX軸とZ軸の方向を向かせる
                        transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, z));
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
                    if (Input.GetButton("Jump"))
                    {
                        // thrustの分だけ上方に力がかかる
                        rb.AddForce(transform.up * thrust);
                        // 速度が出ていたら前方と上方に力がかかる
                        if (rb.velocity.magnitude > 0)
                            rb.AddForce(transform.forward * thrust + transform.up * thrust);
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
}
