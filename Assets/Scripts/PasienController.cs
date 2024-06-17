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
    [SerializeField] int checkMonitorDelayTime = 5;
    
    [Header("2. Event Vt/Vf")]
    [SerializeField] int minShock = 1;
    public UnityEvent on1stShockDone;
    public UnityEvent on2stShockDone;
    public UnityEvent on3stShockDone;
    
    [Header("3. Event Asistol/PEA And Vt/Vf")]
    [SerializeField] int minPcr = 30;
    public UnityEvent on1stpcrDone;
    public UnityEvent on2stpcrDone;
    public UnityEvent on3stpcrDone;

    void Awake() {
        if (!Instance) {
            Instance = this;
        } else {
            Debug.LogError("PasienController already created!");
            Destroy(gameObject);
        }
    }

    void Update() {
        // Debug
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

        yield return new WaitForSeconds(checkMonitorDelayTime);

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
        
    //////////////////////////////////////////////// STEP 1 ////////////////////////////////////////////////////////////////////////
        // Phase 1

        yield return new WaitUntil(() => shockDone >= minShock ); // need shock
        on1stShockDone.Invoke();
        yield return new WaitUntil(() => pcrDone >= minPcr );   // need pcr
        on1stpcrDone.Invoke();
        state = PasienState.Normal;

        // Wait Condition in montitor
        yield return new WaitForSeconds(checkMonitorDelayTime);

        // Phase 2
        state = PasienState.VtVf;
        shockDone = 0;
        pcrDone   = 0;

        yield return new WaitUntil(() => shockDone >= minShock ); // need shock
        on2stShockDone.Invoke();
        yield return new WaitUntil(() => pcrDone >= minPcr ); // need pcr
        on2stpcrDone.Invoke(); 
        
        obatGiven = JenisObat.None;
        yield return new WaitUntil(() => obatGiven != JenisObat.None); // Need to Give Apinepherin

        // Wrong medicine 
        if (obatGiven != JenisObat.Epinephrine) {
            // Game Over
            GameManager.Instance.GameOver();
            yield break;
        }
        state = PasienState.Normal;

        // Wait Condition in montitor
        yield return new WaitForSeconds(checkMonitorDelayTime);

        //Phase 3
        state = PasienState.VtVf;
        shockDone = 0;
        pcrDone   = 0;

        yield return new WaitUntil(() => shockDone >= minShock ); // need shock
        on3stShockDone.Invoke();
        yield return new WaitUntil(() => pcrDone >= minPcr ); // need pcr
        on3stpcrDone.Invoke(); 

        // Need to Give Amiodarone
        obatGiven = JenisObat.None;
        yield return new WaitUntil(() => obatGiven != JenisObat.None);

        // Wrong medicine 
        if (obatGiven != JenisObat.Amiodarone) {
            // Game Over
            GameManager.Instance.GameOver();
            yield break;
        }

        // Wait Condition in montitor
        yield return new WaitForSeconds(checkMonitorDelayTime);
        state = PasienState.Normal;
        yield return new WaitForSeconds(checkMonitorDelayTime);
        // Pasien survived
        GameManager.Instance.GameWin();
    }

    IEnumerator IEStartAsistol() {
        //State Asistol
        //phase 1
        state = PasienState.Asystole;
        pcrDone = 0;

        // Need to pcr
        yield return new WaitUntil(() => pcrDone >= minPcr);
        state = PasienState.Normal;
        on1stpcrDone.Invoke();

        // Wait Condition in montitor
        yield return new WaitForSeconds(checkMonitorDelayTime);

        //phase 2
        state = PasienState.Asystole;
        pcrDone = 0;

        // Need to pcr
        yield return new WaitUntil(() => pcrDone >= minPcr);
        on2stpcrDone.Invoke();

        // Need to Give Epinephrine
        obatGiven = JenisObat.None;
        yield return new WaitUntil(() => obatGiven != JenisObat.None);

        // Wrong medicine 
        if (obatGiven != JenisObat.Epinephrine) {
            // Game Over
            GameManager.Instance.GameOver();
            yield break;
        }

        yield return new WaitForSeconds(checkMonitorDelayTime);
        state = PasienState.Normal;
        yield return new WaitForSeconds(checkMonitorDelayTime);
        // Pasien survived
        GameManager.Instance.GameWin();
    }
}
