using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyringeController : MonoBehaviour {
    [SerializeField] GameObject handle;
    public JenisObat obatBorrowed = JenisObat.None;

    public void AmbilObat(JenisObat obat) {
        obatBorrowed = obat;

        handle.transform.localPosition = new Vector3(-.035f, handle.transform.localPosition.y, handle.transform.localPosition.z);

        // Animation
        LeanTween.moveLocalX(handle, .035f, .5f).setEaseInOutQuad();
    }

    public void KasihObat() {
        PasienController.Instance.GiveObat(obatBorrowed);
        obatBorrowed = JenisObat.None;

        // Animation
        LeanTween.moveLocalX(handle, -.035f, .5f).setEaseInOutQuad();
    }
}
