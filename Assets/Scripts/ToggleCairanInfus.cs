using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ToggleCairanInfus : MonoBehaviour {
    [SerializeField] GameObject selangInfus;
    [SerializeField] Transform socket;

    public void attachSelang() {
        Destroy(selangInfus.GetComponent<XRGrabInteractable>());
        selangInfus.AddComponent<HingeJoint>();
        selangInfus.transform.position = socket.position;
        selangInfus.transform.rotation = socket.rotation;
    }
}
