using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PasienController : MonoBehaviour {
    public static PasienController Instance;
    
    [Header("Properties (Do Not Change!)")]
    public PasienState state;
    public bool maskerTerpasang;
    public bool monitorTerpasang;
    public bool infusTerpasang;
    public JenisObat obatDiperlukan;

    void Awake() {
        if (!Instance) {
            Instance = this;
        } else {
            Debug.LogError("PasienController already created!");
            Destroy(gameObject);
        }
    }
}
