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
    Asystole
}

public enum ECGLine {
    Inactive,
    Normal,
    VtVf,
    Asystole
}

public enum JenisObat {
    None,
    Epinephrine,
    Amiodarone,
}
