using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObatSocket : MonoBehaviour {
    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Suntik")) {
            SyringeController syr = other.GetComponent<SyringeController>();

            if (syr.obatBorrowed != JenisObat.None) {
                syr.KasihObat();
            }
        }
    }
}
