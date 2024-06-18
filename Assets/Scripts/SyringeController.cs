using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyringeController : MonoBehaviour {
    [SerializeField] GameObject handle;
    public JenisObat obatBorrowed;

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Obat")) {
            AmbilObat(other.GetComponent<ObatKaca>().jenisObat);
        }
    }

    public void AmbilObat(JenisObat obat) {
        obatBorrowed = obat;

        // Animation
        LeanTween.moveLocalX(gameObject, .035f, .5f).setEaseInOutQuad();
    }

    public void KasihObat() {
        PasienController.Instance.GiveObat(obatBorrowed);

        // Animation
        LeanTween.moveLocalX(gameObject, -.035f, .5f).setEaseInOutQuad();
    }
}
