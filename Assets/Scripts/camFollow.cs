﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class camFollow : MonoBehaviour
{
    public Transform camPos;
    public Vector3 velocity = Vector3.one;
    Quaternion angle = Quaternion.identity;
   // public NormalMovement moveScript;
    Vector3 previousPos;
    Vector3 currentPos;
    Vector3 lerpPos;
   

    private void Start()
    {

        previousPos = transform.position;
    }



    private void LateUpdate()
    {
        if(!GameManager.Instance.isPaused)
        {
            /*float alpha = (Time.time - Time.fixedTime) / Time.fixedDeltaTime;
            currentPos = moveScript.getTargetPosition();
            lerpPos = Vector3.Lerp(previousPos, currentPos, alpha);*/
            transform.position = Vector3.SmoothDamp(transform.position, camPos.position, ref velocity, .025f);
    
            // previousPos = lerpPos;
            //transform.position = camPos.position;
            transform.rotation = camPos.rotation;

            //transform.rotation = SmoothDamp(transform.rotation, camPos.rotation, ref angle, .05f);
        }
     
      


    }



    public static Quaternion SmoothDamp(Quaternion rot, Quaternion target, ref Quaternion deriv, float time)
    {
        // account for double-cover
        var Dot = Quaternion.Dot(rot, target);
        var Multi = Dot > 0f ? 1f : -1f;
        target.x *= Multi;
        target.y *= Multi;
        target.z *= Multi;
        target.w *= Multi;
        // smooth damp (nlerp approx)
        var Result = new Vector4(
            Mathf.SmoothDamp(rot.x, target.x, ref deriv.x, time),
            Mathf.SmoothDamp(rot.y, target.y, ref deriv.y, time),
            Mathf.SmoothDamp(rot.z, target.z, ref deriv.z, time),
            Mathf.SmoothDamp(rot.w, target.w, ref deriv.w, time)
        ).normalized;
        // compute deriv
        var dtInv = 1f / Time.deltaTime;
        deriv.x = (Result.x - rot.x) * dtInv;
        deriv.y = (Result.y - rot.y) * dtInv;
        deriv.z = (Result.z - rot.z) * dtInv;
        deriv.w = (Result.w - rot.w) * dtInv;
        return new Quaternion(Result.x, Result.y, Result.z, Result.w);
    }



}
