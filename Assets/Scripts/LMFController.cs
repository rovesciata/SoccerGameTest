using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LMFController : MonoBehaviour
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
            // ボールの位置がハーフウェイライン近くの時
            if (ball.transform.position.x > -5 && ball.transform.position.x < 15)
            {
                // プレイヤーがハーフウェイラインからx軸5の位置まで動く
                if (transform.position.x < 5)
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
        }
    }

    // ボールを持ってない時
    public bool IDontHaveBall()
    {
        return transform.childCount <= 7;
    }
}
