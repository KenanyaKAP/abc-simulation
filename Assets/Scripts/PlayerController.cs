using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [SerializeField] CharacterController controller;
    
    [Header("Properties")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float rotateSpeed = 5f;

    int moveDir;
    int rotateValue;

    void Update() {
        if (rotateValue != 0) {
            transform.eulerAngles += Vector3.up * rotateValue * rotateSpeed * Time.deltaTime;
        }

        if (controller.enabled && moveDir == 1) {
            controller.Move(transform.forward * moveDir * Time.deltaTime * moveSpeed);
        }
    }

    public void AddRotateRight(bool value) {
        if (value) {
            rotateValue = -1;
        } else {
            rotateValue = 0;
        }
    }
    public void AddRotateLeft(bool value) {
        if (value) {
            rotateValue = 1;
        } else {
            rotateValue = 0;
        }
    }


    public void MoveForward(bool value) {
        if (value) {
            moveDir = 1;
        } else {
            moveDir = 0;
        }
    }
}
