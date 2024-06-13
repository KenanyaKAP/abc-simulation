using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;

    public GameState gameState;

    void Awake() {
        if (!Instance) {
            Instance = this;
        } else {
            Debug.LogError("GameManager already created!");
            Destroy(gameObject);
        }
    }
}
