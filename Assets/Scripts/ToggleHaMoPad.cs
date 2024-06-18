using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ToggleHaMoPad : MonoBehaviour {
    [SerializeField] XRSocketInteractor socket;

    public void SetToggleHaMoPad(SelectEnterEventArgs args) {

        Destroy(args.interactableObject.transform.gameObject);
    }
}
