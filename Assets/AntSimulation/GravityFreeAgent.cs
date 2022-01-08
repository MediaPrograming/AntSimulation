using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityFreeAgent : MonoBehaviour {

    [SerializeField]
    Transform CenterOfBalance;  // 重心

    public float movableDist = 1.0f;
    public float rayOffset = 0.1f;

    void Start ()
    {
    }
    
    
    void FixedUpdate () {

        Ray ray = new Ray(transform.position,-transform.up + transform.forward * 0.1f);
        
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

        int rayNum = 4;
        Ray[] rays = new Ray[rayNum];
        rays[0] = new Ray(CenterOfBalance.position+transform.forward*rayOffset, transform.forward);
        rays[1] = new Ray(CenterOfBalance.position+transform.forward*rayOffset, transform.forward - transform.up);
        rays[2] = new Ray(CenterOfBalance.position+transform.forward*rayOffset, -transform.up);
        rays[3] = new Ray(CenterOfBalance.position+transform.forward*rayOffset, -transform.forward-transform.up);
        
        RaycastHit[] hitArray = new RaycastHit[rayNum];
        
        for (int i = 0; i < rayNum; i++)
        {
            Physics.Raycast(rays[i], out hitArray[i], float.PositiveInfinity);
            
            Debug.DrawRay(rays[i].origin,rays[i].direction * 100,Color.blue,1,true);
        }

    }

}