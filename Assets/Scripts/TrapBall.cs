using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapBall : MonoBehaviour
{
    // ボールを入れる変数
    public GameObject ball;
    // ボールのRigidbodyを入れる変数
    Rigidbody ballRigidbody;
    // Passingクラスを入れる変数
    Passing passScript;
    // プレイヤーの移動スピード
    public float playerSpeed = 2;
    // Animatorを入れる変数
    private Animator animator;
    // プレイヤーのRigidbodyを入れる変数
    Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        // ボールを取得
        ball = GameObject.Find("Ball");
        // ボールのRigidbodyを取得
        ballRigidbody = ball.GetComponent<Rigidbody>();
        // ボールのPassingクラスを取得
        passScript = ball.GetComponent<Passing>();
        // Animatorを取得
        animator = GetComponent<Animator>();
        // プレイヤーのRigidbodyを取得
        rb = GetComponent<Rigidbody>();
    }

    // トラップ(プレイヤーにボールが近づいたらボールを止める)
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Ball" && passScript.isPass == true)
        {
            // ボールを止める
            Invoke("StopBall", 0f);
            // パスを無効
            passScript.isPass = false;

            // シュートアニメーションを無効化
            animator.SetBool("Shoot", false);
        }
    }

    //　ボールを止める関数
    void StopBall()
    {
        // ボールのスピードを0にする
        ballRigidbody.velocity = Vector3.zero;
        // ボールの回転を0にする
        ballRigidbody.angularVelocity = Vector3.zero;
        // ボールの物理演算の影響を有効
        ballRigidbody.isKinematic = true;
        // ボールを親(=プレイヤー)の位置と同じにする
        ball.transform.SetParent(transform);   
    }
}
