using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PasienController : MonoBehaviour {
    public static PasienController Instance;
        
    [Header("Properties")]
    // Must added to 1
    [SerializeField] float vtvfChance = .5f;
    [SerializeField] float asistolChance = .5f;

    [Header("Properties (Do Not Change!)")]
    public PasienState state;
    public bool maskerTerpasang;
    public bool monitorTerpasang;
    public bool infusTerpasang;

    [Header("1. Event Start")]
    [SerializeField] int startDelayTime = 5;
    
    [Header("2. Event Vt-Vf")]
    [SerializeField] int minShock = 3;
    public UnityEvent on1stShockDone;
    [SerializeField] int secondShockDelayTime = 10;
    public UnityEvent on2ndShockDone;
    

    [Header("3. Event Asistol")]
    [SerializeField] int minPCR = 10;
    public UnityEvent on1stpcrDone;

    void Awake() {
        if (!Instance) {
            Instance = this;
        } else {
            Debug.LogError("PasienController already created!");
            Destroy(gameObject);
        }
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Debug.Log("Hard Shock!");
            ShockPasien();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            Debug.Log("Hard Give Epinephrine!");
            GiveObat(JenisObat.Epinephrine);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            Debug.Log("Hard Give Amiodarone!");
            GiveObat(JenisObat.Amiodarone);
        }
    }

    // ============================= Public Event =============================


    public void StartPasien() {
        StartCoroutine(IEStartPasien());
    }

    // ============================= Public Function =============================
    
    int shockDone = 0;
    public void ShockPasien() {
        shockDone += 1;

        // Animation
    }

    int pcrDone = 0;
    public void PCRPasien() {
        pcrDone += 1;

        // Animation
    }

    JenisObat obatGiven;
    public void GiveObat(JenisObat obat) {
        obatGiven = obat;
    }

    // ============================= IEnumerator Function =============================

    IEnumerator IEStartPasien() {
        state = PasienState.Normal;

        yield return new WaitForSeconds(startDelayTime);

        float randomEvent = Random.Range(0f, vtvfChance + asistolChance);
        if (randomEvent < vtvfChance) {
            // Vt-Vf
            StartCoroutine(IEStartVTVF());
            Debug.Log("Pasien VtVf");
        } else {
            // Asistol
            StartCoroutine(IEStartAsistol());
            Debug.Log("Pasien Asistol");
        }
    }

    IEnumerator IEStartVTVF() {
        // Initial State VtVf
        state = PasienState.VtVf;
        shockDone = 0;

        // Need to shock
        yield return new WaitUntil(() => shockDone >= minShock);
        state = PasienState.Normal;
        on1stShockDone.Invoke();

        // Need to Give Epinephrine
        obatGiven = JenisObat.None;
        yield return new WaitUntil(() => obatGiven != JenisObat.None);

        // Wrong medicine 
        if (obatGiven != JenisObat.Epinephrine) {
            // Game Over
            GameManager.Instance.GameOver();
            yield break;
        }
        
        // Wait for second Vtvf
        yield return new WaitForSeconds(secondShockDelayTime);
        
        // Vtvf again
        state = PasienState.VtVf;
        shockDone = 0;

        // Need to shock
        yield return new WaitUntil(() => shockDone >= minShock);
        state = PasienState.Normal;
        on2ndShockDone.Invoke();

        // Need to Give Amiodarone
        obatGiven = JenisObat.None;
        yield return new WaitUntil(() => obatGiven != JenisObat.None);

        // Wrong medicine 
        if (obatGiven != JenisObat.Amiodarone) {
            // Game Over
            GameManager.Instance.GameOver();
            yield break;
        }

        // Pasien survived
        GameManager.Instance.GameWin();
    }

    IEnumerator IEStartAsistol() {
        // Initial State Asistol
        state = PasienState.Asistol;
        pcrDone = 0;

        // Need to shock
        yield return new WaitUntil(() => pcrDone >= minPCR);
        state = PasienState.Normal;
        on1stpcrDone.Invoke();

        // Need to Give Epinephrine
        obatGiven = JenisObat.None;
        yield return new WaitUntil(() => obatGiven != JenisObat.None);

        // Wrong medicine 
        if (obatGiven != JenisObat.Epinephrine) {
            // Game Over
            GameManager.Instance.GameOver();
            yield break;
        }

        // Pasien survived
        GameManager.Instance.GameWin();
    }
}
