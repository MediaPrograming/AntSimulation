using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityFreeAgent : MonoBehaviour {

    [SerializeField]
    Transform CenterOfBalance;  // 重心

    void Start ()
    {
        Application.targetFrameRate = 60;
    }
    
    void Update () {

        // キーボード入力で移動、回転
        // if (Input.GetKey(KeyCode.LeftArrow)) {
        //     transform.Rotate(
        //         new Vector3(0, -3f, 0),
        //         Space.Self
        //     );
        // } else if (Input.GetKey(KeyCode.RightArrow)) {
        //     transform.Rotate(
        //         new Vector3(0, 3f, 0),
        //         Space.Self
        //     );
        // } else if (Input.GetKey(KeyCode.UpArrow)) {
        //     transform.position = 
        //         transform.position + 
        //         (transform.forward * 3 * Time.fixedDeltaTime);
        // } else if (Input.GetKey(KeyCode.DownArrow)) {
        //     transform.position = 
        //         transform.position + 
        //         (transform.forward * 3 * Time.fixedDeltaTime);
        // }
        
        // transform.position = 
        //     transform.position + 
        //     (transform.forward * 3 * Time.fixedDeltaTime);

        RaycastHit hit;

        // Transformの真下の地形の法線を調べる
        if (Physics.Raycast(
                CenterOfBalance.position,
                -transform.up + transform.forward * 0.1f,
                out hit,
                float.PositiveInfinity))
        {
            // 傾きの差を求める
            Quaternion q = Quaternion.FromToRotation(
                transform.up,
                hit.normal);

            
            transform.position = transform.position  + (transform.forward * Time.fixedDeltaTime);
            // 自分を回転させる
            transform.rotation *= q;

            // 地面から一定距離離れていたら落下
            if (hit.distance > 0.05f) {
                transform.position =
                    transform.position +
                    (-transform.up * Physics.gravity.magnitude * Time.fixedDeltaTime);
            }
        }

    }

}