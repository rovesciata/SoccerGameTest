using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passing : MonoBehaviour
{
    // タッチ開始位置、指を離した位置、スワイプの方向を入れる変数
    private Vector2 startPos, endPos, direction;

    [SerializeField]
    // X軸方向のスワイプの強さ
    float throwForceInX = 1f;
    [SerializeField]
    // Z軸方向のスワイプの強さ
    float throwForceInZ = 1f;
    // プレイヤーのRigidbodyを入れる変数
    Rigidbody rb;
    // パスの効果音を鳴らす定義
    AudioClip getBallSound;
    AudioSource audioSource;
    // パスしたか判定
    public bool isPass = false;
    // プレイヤーの配列
    public GameObject[] players;
    // Animatorを入れる変数
    private Animator animator;

    // Use this for initialization
    void Start()
    {
        // プレイヤーのRigidbodyを取得
        rb = GetComponent<Rigidbody>();
        // ボールの音を取得
        getBallSound = Resources.Load<AudioClip>("Audio/ball_hit_rnd_01");
        audioSource = transform.GetComponent<AudioSource>();
        // プレイヤーの配列を取得
        players = GetComponents<GameObject>();
    }

    // プレイヤーがボールを保持している時の処理(パスorシュート)
    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            // タッチ開始時
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                // タッチ開始位置を取得
                startPos = Input.GetTouch(0).position;
            }

            // 指を離した時
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                // 指を離した時の位置を取得
                endPos = Input.GetTouch(0).position;

                // スワイプの方向を取得
                direction = startPos - endPos;

                // 画面右側でスワイプしたらパスorシュートできる
                if (startPos.x > 1100)
                {
                    // 物理演算の影響を無効
                    rb.isKinematic = false;
                    // パスorシュート
                    rb.AddForce(-direction.x * throwForceInX, 10f, -direction.y * throwForceInZ, ForceMode.Impulse);

                    // プレイヤー全員を取得
                    foreach (GameObject player in players)
                    {
                        // ボールを保持しているプレイヤーを抽出(=ボールが子要素になっている)
                        if (player.transform.childCount > 0)
                        {
                            // プレイヤーのAnimatorを取得
                            animator = player.GetComponent<Animator>();
                            // パスのアニメーションを有効
                            animator.SetBool("Passing", true);
                            // 走るアニメーションを無効
                            animator.SetBool("Running", false);
                        }
                    }
                    // ボールの音を鳴らす
                    audioSource.PlayOneShot(getBallSound);
                    // パスを有効
                    isPass = true;
                    // ボールとプレイヤーの親子関係を無効
                    transform.SetParent(null);
                }
            }
        }
    }
}