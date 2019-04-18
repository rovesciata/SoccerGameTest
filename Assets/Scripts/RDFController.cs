using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RDFController : MonoBehaviour
{
    // Animatorを入れる変数
    private Animator animator;
    // ボールを入れる変数
    public GameObject ball;

    // Start is called before the first frame update
    void Start()
    {
        // Animatorを取得
        animator = GetComponent<Animator>();
        // ボールを取得
        ball = GameObject.Find("Ball");
    }

    // Update is called once per frame
    void Update()
    {
        // ボールを持っていない時
        if (IDontHaveBall())
        {
            // ボールの位置がハーフウェイラインを越えた時
            if (ball.transform.position.x > 1 && ball.transform.position.x < 15)
            {
                // プレイヤーがハーフウェイラインからx軸-5の位置まで動く
                if (transform.position.x < -5)
                {
                    // 右に動く
                    transform.position += Vector3.right * Time.deltaTime * 2;

                    // 走るアニメーションを再生
                    animator.SetBool("Running", true);
                }
                else
                {
                    // 走るアニメーションを停止
                    animator.SetBool("Running", false);
                }
            }
            // ボールの位置がハーフウェイライン手前の時
            else if (ball.transform.position.x > -10 && ball.transform.position.x <= 1)
            {
                // プレイヤーがハーフウェイラインからx軸-15の位置まで動く
                if (transform.position.x < -15)
                {
                    transform.position += Vector3.right * Time.deltaTime * 2;

                    // 走るアニメーションを再生
                    animator.SetBool("Running", true);
                }
                else
                {
                    // 走るアニメーションを停止
                    animator.SetBool("Running", false);
                }
            }
        }
    }

    // ボールを持ってない時
    public bool IDontHaveBall()
    {
        return transform.childCount <= 7;
    }
}
