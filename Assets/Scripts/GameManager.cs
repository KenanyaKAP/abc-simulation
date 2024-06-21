using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;

    [Header("Game Properties (Do Not Change!)")]
    public GameState gameState;
    
    [Header("Game Event")]
    public UnityEvent onWelcome;
    public UnityEvent onGameStart;
    public UnityEvent onGameOver;
    public UnityEvent onGameWin;

    void Awake() {
        if (!Instance) {
            Instance = this;
        } else {
            Debug.LogError("GameManager already created!");
            Destroy(gameObject);
        }
    }

    void Start() {
        onWelcome.Invoke();
        Debug.Log("OnWelcome Invoked");

        StartCoroutine(IECheckPatientConditionToStart());
    }

    // ============================= Public Function =============================

    public void GameOver() {
        gameState = GameState.GameOver;
        onGameOver.Invoke();
        Debug.Log("Game Over!");
    }
    
    public void GameWin() {
        gameState = GameState.Win;
        onGameWin.Invoke();
        Debug.Log("Game Win! Pasien Survived!");

        StartCoroutine(GameWinBackToMainMenu());
    }

    public void RestartLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // ============================= IEnumerator Function =============================

    IEnumerator IECheckPatientConditionToStart() {
        yield return new WaitUntil(() => 
            PasienController.Instance.maskerTerpasang && 
            PasienController.Instance.monitorTerpasang && 
            PasienController.Instance.infusTerpasang
        );

        onGameStart.Invoke();
        Debug.Log("OnGameStart Invoked");
    }

    IEnumerator GameWinBackToMainMenu() {
        yield return new WaitForSeconds(6);

        SceneManager.LoadScene(0);
    }
}
