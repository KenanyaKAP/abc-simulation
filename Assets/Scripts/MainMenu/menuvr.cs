using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class menuvr : MonoBehaviour
{
    public GameObject MainMenuPanel; // Reference ke panel menu utama
    public GameObject SettPanel; // Reference ke panel setting
    public GameObject CreditPanel;

    // Start is called before the first frame update
    void Start()
    {
        // Pastikan panel setting tidak aktif saat game dimulai
        SettPanel.SetActive(false);
        CreditPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Fungsi untuk menampilkan panel setting dan menyembunyikan panel menu utama
    public void ShowSettings()
    {
        CreditPanel.SetActive(false);
        MainMenuPanel.SetActive(false); // Matikan panel menu utama
        SettPanel.SetActive(true);  // Aktifkan panel setting
    }
    public void ShowCreditMainMenu()
    {
        CreditPanel.SetActive(true);
        SettPanel.SetActive(false); // Matikan panel setting
        MainMenuPanel.SetActive(false);   // Aktifkan panel menu utama
    }


    // Fungsi untuk kembali ke menu utama dari panel setting
    public void ReturnToMainMenu()
    {
        CreditPanel.SetActive(false);
        SettPanel.SetActive(false); // Matikan panel setting
        MainMenuPanel.SetActive(true);   // Aktifkan panel menu utama
    }
    public void MasukKeGame()
    {
        SceneManager.LoadScene(1);
    }

}
