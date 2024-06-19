using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    
    [Header("Properties")]
    [SerializeField] float rotateSpeed = 5f;

    int rotateValue;

    void Update() {
        if (rotateValue != 0) {
            transform.eulerAngles += Vector3.up * rotateValue * rotateSpeed * Time.deltaTime;
        }
    }

    public void AddRotateRight(bool value) {
        if (value) {
            rotateValue -= 1;
        } else {
            rotateValue = 0;
        }
    }
    public void AddRotateLeft(bool value) {
        if (value) {
            rotateValue += 1;
        } else {
            rotateValue = 0;
        }
    }
}
