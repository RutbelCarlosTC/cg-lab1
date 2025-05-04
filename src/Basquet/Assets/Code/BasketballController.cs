using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketballController : MonoBehaviour {

    public float MoveSpeed = 10;
    public float launchDistance = 10f;
    

    public Transform Ball;
    public Transform PosDribble;
    public Transform PosOverHead;
    public Transform Arms;
    public Transform Target;

    // variables
    private bool IsBallInHands = true;
    private bool IsBallFlying = false;
    private float T = 0;


    // Update is called once per frame
    void Update() {

        // walking
        Vector3 direction = new(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        transform.position += MoveSpeed * Time.deltaTime * direction;
        transform.LookAt(transform.position + direction);

        // ball in hands
        if (IsBallInHands) {

            // hold over head
            if (Input.GetKey(KeyCode.Space)) {
                Ball.position = PosOverHead.position;
                Arms.localEulerAngles = Vector3.right * 180;

                // look towards the target
                //transform.LookAt(Target.parent.position);
            }

            // dribbling
            else {
                Ball.position = PosDribble.position + Vector3.up * Mathf.Abs(Mathf.Sin(Time.time * 10));
                Arms.localEulerAngles = Vector3.right * 0;
            }

            // throw ball
            if (Input.GetKeyUp(KeyCode.Space)) {
                IsBallInHands = false;
                IsBallFlying = true;
                T = 0;
            }
        }

        // ball in the air
        if (IsBallFlying) {
            T += Time.deltaTime;
            float duration = 0.66f;
            float t01 = T / duration;

            // move to target
            
            Vector3 launchDirection = transform.forward; // o basado en mouse/stick input
            Vector3 A = PosOverHead.position;
            Vector3 B = A + launchDirection * launchDistance;
            //Vector3 B = Target.position;
            Vector3 pos = Vector3.Lerp(A, B, t01);

            // move in arc
            Vector3 arc = 5 * Mathf.Sin(t01 * 3.14f) * Vector3.up;

            Ball.position = pos + arc;

            // moment when ball arrives at the target
            if (t01 >= 1f) {
                IsBallFlying = false;
                Rigidbody rb = Ball.GetComponent<Rigidbody>();
                rb.isKinematic = false;

                // Estimar velocidad final como derivada de la posición
                Vector3 finalVelocity = (B - A) / duration;
                //Vector3 arcVelocity = 5f * Mathf.PI * Mathf.Cos(Mathf.PI) * Vector3.up / duration; // derivada del arco
                Vector3 arcVelocity = -5f * Mathf.PI / duration * Vector3.up; 

                Vector3 totalVelocity = finalVelocity + arcVelocity;
                Debug.Log("velocidad final: " + totalVelocity );
                rb.velocity = totalVelocity; // velocidad total

            }
        }
    }

    private void OnTriggerEnter(Collider other) {

        if (!IsBallInHands && !IsBallFlying) {

            IsBallInHands = true;
            Ball.GetComponent<Rigidbody>().isKinematic = true;
        }
    }
}
