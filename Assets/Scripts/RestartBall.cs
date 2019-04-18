using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartBall : MonoBehaviour
{
    // ボールのキックオフ位置
    Vector3 startingPosition;
    // ボールのRigidbodyを入れる変数
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        // 初期位置を取得
        startingPosition = this.transform.position;
        // Rigidbodyを取得
        rb = GetComponent<Rigidbody>();
    }

    // ボールがゴールに入るか、ラインを越えた場合、ボールをキックオフ位置に戻す
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Goal" || col.gameObject.tag == "Wall")
        {
            // ボールを１秒後に戻す
            Invoke("BallReStart", 1f);
        }
    }

    // ボールをキックオフ位置に戻す関数
    void BallReStart()
    {
        // キックオフ位置を代入
        this.transform.position = startingPosition;
        rb.isKinematic = true;
        rb.isKinematic = false;
    }
}
