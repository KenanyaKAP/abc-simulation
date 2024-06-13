using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuSurgery : MonoBehaviour
{
    public GameObject MainMenuPanel; // Reference ke panel menu utama
    public GameObject SettingPanel; // Reference ke panel setting


    // Start is called before the first frame update
    void Start()
    {
        // Pastikan panel setting tidak aktif saat game dimulai
        SettingPanel.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {

    }

    // Fungsi untuk menampilkan panel setting dan menyembunyikan panel menu utama
    public void ShowSettings()
    {
     
        MainMenuPanel.SetActive(false); // Matikan panel menu utama
        SettingPanel.SetActive(true);  // Aktifkan panel setting
    }



    // Fungsi untuk kembali ke menu utama dari panel setting
    public void ReturnToMainMenu()
    {
        
        SettingPanel.SetActive(false); // Matikan panel setting
        MainMenuPanel.SetActive(true);   // Aktifkan panel menu utama
    }

    public void BalikKeMainMenu()
    {
        SceneManager.LoadScene(2);
    }
    public void BalikKeGameSurgeryRoom()
    {
        SceneManager.LoadScene(1);
    }

}
