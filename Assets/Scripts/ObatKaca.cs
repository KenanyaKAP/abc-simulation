using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObatKaca : MonoBehaviour {
    public JenisObat jenisObat;

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Suntik")) {
            other.GetComponent<SyringeController>().AmbilObat(jenisObat);
        }
    }
}
