using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GravityFreeAgent : MonoBehaviour {

    [SerializeField]
    Transform CenterOfBalance;  // 重心

    public float movableDist = 1.0f;
    public float rayOffset = 0.02f;

    void Start ()
    {
        
    }
    
    
    void FixedUpdate () {

        Ray ray = new Ray(CenterOfBalance.position + transform.up * 0.01f,-transform.up + transform.forward);
        
        RaycastHit hit;
        
        // Transformの少し前方の地形を調べる
        if (Physics.Raycast(
                ray,
                out hit,
                movableDist))
        {
            // 傾きの差を求める
            Quaternion q = Quaternion.FromToRotation(
                transform.up,
                hit.normal);
            transform.rotation *= q;
            

            transform.position = hit.point + (transform.position - CenterOfBalance.position);
        
            Debug.DrawRay(ray.origin,ray.direction * hit.distance, Color.red, 1,true);

            return;
        }
        //
        // int rayNum = 4;
        // Ray[] rays = new Ray[rayNum];
        // rays[0] = new Ray(CenterOfBalance.position+transform.forward*rayOffset, transform.forward);
        // rays[1] = new Ray(CenterOfBalance.position+transform.forward*rayOffset, transform.forward - transform.up);
        // rays[2] = new Ray(CenterOfBalance.position+transform.forward*rayOffset, -transform.up);
        // rays[3] = new Ray(CenterOfBalance.position+transform.forward*rayOffset, -transform.forward-transform.up);
        // // rays[4] = new Ray(CenterOfBalance.position+transform.forward*rayOffset, transform.up);
        //
        // RaycastHit[] hitArray = new RaycastHit[rayNum];
        //
        // float minDist = float.PositiveInfinity;
        // int minIndex = -1;
        // for (int i = 0; i < rayNum; i++)
        // {
        //     Physics.Raycast(rays[i], out hitArray[i], float.PositiveInfinity);
        //     if (hitArray[i].distance > 0 && minDist > hitArray[i].distance)
        //     {
        //         minDist = hitArray[i].distance;
        //         minIndex = i;
        //     }
        //     Debug.DrawRay(rays[i].origin,rays[i].direction * 100,Color.blue,1,true);
        // }
        //
        // //近くに地形があったら
        // if (minIndex != -1 && minDist < movableDist)
        // {
        //     Quaternion q = Quaternion.FromToRotation(
        //         transform.up,
        //         hitArray[minIndex].normal);
        //
        //     transform.rotation *= q;
        //     
        //     Debug.Log("minDist="+minDist);
        //
        //     transform.position = hitArray[minIndex].point + (transform.position - CenterOfBalance.position);
        //     
        //     Debug.DrawRay(rays[minIndex].origin,rays[minIndex].direction * 100, Color.yellow, 1,true);
        // }
        // else
        // {
        //     transform.Rotate(10,10,10);
        //     transform.position = transform.position +new Vector3(0,0.01f,0);
        // }

    }

}