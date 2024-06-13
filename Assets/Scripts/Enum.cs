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
    Normal,
    Vt,
    Asistol
}

public enum JenisObat {
    None,
    Aphinepherin,
    Amiodarone,
}
