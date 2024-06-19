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
    [SerializeField] int startDelay = 20;
    
    [Header("2. Event Vt/Vf")]
    [SerializeField] int minPcrVtVf = 30;
    [SerializeField] int minShockVtVf = 1;
    [SerializeField] int delayToNextStage = 6;
    public UnityEvent eventVtVf1Start;
    public UnityEvent eventVtVf2CPR;
    public UnityEvent eventVtVf3Cek;
    public UnityEvent eventVtVf4Shock;
    public UnityEvent eventVtVf5CPR;
    public UnityEvent eventVtVf6Epinephrine;
    public UnityEvent eventVtVf7Cek;
    public UnityEvent eventVtVf8Shock;
    public UnityEvent eventVtVf9CPR;
    public UnityEvent eventVtVf10Amiodarone;
    public UnityEvent eventVtVf11Cek;
    
    [Header("3. Event Asistol/PEA And Vt/Vf")]
    [SerializeField] int minPcrAsystole = 30;
    public UnityEvent eventAsystole1Start;
    public UnityEvent eventAsystole2Epinephrine;
    public UnityEvent eventAsystole3Cek;
    public UnityEvent eventAsystole4CPR;
    public UnityEvent eventAsystole5Cek;
    
    [Header("Component")]
    [SerializeField] GameObject spine1;
    [SerializeField] GameObject spine2;
    [SerializeField] GameObject pcrButton;
    [SerializeField] GameObject shockHover;
    [SerializeField] GameObject obatSocket;

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
        if (Input.GetKeyDown(KeyCode.N)) {
            Debug.Log("Hard Shock!");
            ShockPasien();
        }  
        
        if (Input.GetKeyDown(KeyCode.M)) {
            Debug.Log("Hard PCR!");
            PCRPasien();
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
    bool shockDelay = false;
    public void ShockPasien() {
        if (shockDelay) return;

        shockDone += 1;
        shockDelay = true;

        // Animation
        LeanTween.rotateAroundLocal(spine1,Vector3.right, 10f, .2f).setEaseOutQuart()
            .setOnComplete(() => LeanTween.rotateAroundLocal(spine1,Vector3.right, -10f, .35f).setEaseInQuad()
            .setOnComplete(() => shockDelay = false));
        LeanTween.scaleZ(spine1, 1.2f, .1f).setLoopPingPong(1);
    }

    int pcrDone = 0;
    bool pcrDelay = false;
    public void PCRPasien() {
        if (pcrDelay) return;

        pcrDone += 1;
        pcrDelay = true;

        // Animation
        LeanTween.rotateAroundLocal(spine1,Vector3.right, -10f, .12f).setEaseOutQuad().setLoopPingPong(1);
        LeanTween.rotateAroundLocal(spine2,Vector3.right, 10f, .12f).setEaseOutQuad().setLoopPingPong(1)
        .setOnComplete(() => pcrDelay = false);
    }

    JenisObat obatGiven;
    public void GiveObat(JenisObat obat) {
        obatGiven = obat;
    }

    // ============================= Public Pasien Function =============================

    public void setmaskerTerpasang(bool val) {
        maskerTerpasang = val;
    }
    public void setmonitorTerpasang(bool val) {
        monitorTerpasang = val;
    }
    public void setinfusTerpasang(bool val) {
        infusTerpasang = val;
    }

    // ============================= IEnumerator Function =============================
    IEnumerator IEStartPasien() {
        state = PasienState.Normal;

        yield return new WaitForSeconds(startDelay);

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
        ECGRenderer.Instance.ChangeECGLine(ECGLine.VtVf);
        eventVtVf1Start.Invoke();
        shockDone = 0;
        
        //////////////////////////////////////////////// STEP 1 ////////////////////////////////////////////////////////////////////////

        // Phase 1
        shockHover.SetActive(true);
        yield return new WaitUntil(() => shockDone >= minShockVtVf ); // need shock
        shockHover.SetActive(false);
        
        eventVtVf2CPR.Invoke();
        pcrDone = 0;

        pcrButton.SetActive(true);
        yield return new WaitUntil(() => pcrDone >= minPcrVtVf );   // need pcr
        pcrButton.SetActive(false);

        eventVtVf3Cek.Invoke();
        state = PasienState.Normal;

        // Wait Condition in montitor
        yield return new WaitForSeconds(delayToNextStage);

        // Phase 2
        state = PasienState.VtVf;
        shockDone = 0;
        pcrDone   = 0;
        eventVtVf4Shock.Invoke();

        shockHover.SetActive(true);
        yield return new WaitUntil(() => shockDone >= minShockVtVf ); // need shock
        shockHover.SetActive(false);
        eventVtVf5CPR.Invoke();

        pcrButton.SetActive(true);
        yield return new WaitUntil(() => pcrDone >= minPcrVtVf ); // need pcr
        pcrButton.SetActive(false);

        eventVtVf6Epinephrine.Invoke();
        
        obatGiven = JenisObat.None;
        obatSocket.SetActive(true);
        yield return new WaitUntil(() => obatGiven != JenisObat.None); // Need to Give Apinepherin
        obatSocket.SetActive(false);

        // Wrong medicine 
        if (obatGiven != JenisObat.Epinephrine) {
            // Game Over
            GameManager.Instance.GameOver();
            yield break;
        }
        state = PasienState.Normal;

        eventVtVf7Cek.Invoke();
        // Wait Condition in montitor
        yield return new WaitForSeconds(delayToNextStage);

        //Phase 3
        state = PasienState.VtVf;
        shockDone = 0;
        pcrDone   = 0;
        eventVtVf8Shock.Invoke();

        shockHover.SetActive(true);
        yield return new WaitUntil(() => shockDone >= minShockVtVf ); // need shock
        shockHover.SetActive(false);
        eventVtVf9CPR.Invoke();

        pcrButton.SetActive(true);
        yield return new WaitUntil(() => pcrDone >= minPcrVtVf ); // need pcr
        pcrButton.SetActive(false);
        
        eventVtVf10Amiodarone.Invoke(); 
        // Need to Give Amiodarone
        obatGiven = JenisObat.None;
        obatSocket.SetActive(true);
        yield return new WaitUntil(() => obatGiven != JenisObat.None);
        obatSocket.SetActive(false);

        // Wrong medicine 
        if (obatGiven != JenisObat.Amiodarone) {
            // Game Over
            GameManager.Instance.GameOver();
            yield break;
        }

        eventVtVf11Cek.Invoke();

        // Wait Condition in montitor
        yield return new WaitForSeconds(delayToNextStage);
        state = PasienState.Normal;
        ECGRenderer.Instance.ChangeECGLine(ECGLine.Normal);
        GameManager.Instance.GameWin();
    }

    IEnumerator IEStartAsistol() {
        //State Asistol
        //phase 1
        state = PasienState.Asystole;
        ECGRenderer.Instance.ChangeECGLine(ECGLine.Asystole);
        eventAsystole1Start.Invoke();
        pcrDone = 0;

        // Need to pcr
        pcrButton.SetActive(true);
        yield return new WaitUntil(() => pcrDone >= minPcrAsystole);
        pcrButton.SetActive(false);
        
        eventAsystole2Epinephrine.Invoke();

        // Need to Give Epinephrine
        obatGiven = JenisObat.None;
        obatSocket.SetActive(true);
        yield return new WaitUntil(() => obatGiven != JenisObat.None);
        obatSocket.SetActive(false);

        // Wrong medicine 
        if (obatGiven != JenisObat.Epinephrine) {
            // Game Over
            GameManager.Instance.GameOver();
            yield break;
        }

        eventAsystole3Cek.Invoke();
        yield return new WaitForSeconds(delayToNextStage);

        eventAsystole4CPR.Invoke();
        pcrDone = 0;

        // Need to pcr
        pcrButton.SetActive(true);
        yield return new WaitUntil(() => pcrDone >= minPcrAsystole);
        pcrButton.SetActive(false);
        
        eventAsystole5Cek.Invoke();
        
        yield return new WaitForSeconds(delayToNextStage);

        // Pasien survived
        state = PasienState.Normal;
        ECGRenderer.Instance.ChangeECGLine(ECGLine.Normal);
        GameManager.Instance.GameWin();
        yield return null;
    }
}
