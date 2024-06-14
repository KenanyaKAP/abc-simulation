using UnityEngine;

[System.Serializable]

public enum GameState {
    Play,
    Pause,
    Win,
    GameOver,
}

public enum PlayerState {
    Idle,
}

public enum PasienState {
    Idle,
    Normal,
    VtVf,
    Asistol
}

public enum JenisObat {
    None,
    Epinephrine,
    Amiodarone,
}
