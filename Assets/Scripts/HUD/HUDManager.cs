using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HUDManager : MonoBehaviour

{
    [SerializeField] private Image _ammoUI;
    [SerializeField] private Image _hidden;
    [SerializeField] private Image _alerted;
    [SerializeField] private Image _detected;

    private WeaponController weaponController;
    private GameObject player;

    [SerializeField] private GameManager gameManager;





    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        weaponController = player.GetComponent<WeaponController>();

        gameManager = FindAnyObjectByType<GameManager>(); // Va a optimizarse cuando cambiemos el GM por singleton

    }


    private void Update()
    {
        if (weaponController.shotCooldown < 1)
        {
            weaponController.shotCooldown += weaponController.weaponActiveScript.shotRecharge * Time.deltaTime;
            _ammoUI.fillAmount = weaponController.shotCooldown;

            if (weaponController.shotCooldown >= 1)
            {
                weaponController.isShotReady = true;
                weaponController.shotCooldown = 1;
            }
        }

        // Detection - Eventualmente sacar del update
       
        switch (gameManager.detectionState)
        {
            case GameManager.DetectionState.Hidden:
                _hidden.gameObject.SetActive(true);
                _alerted.gameObject.SetActive(false);
                _detected.gameObject.SetActive(false);
                break;
            case GameManager.DetectionState.Alerted:
                _alerted.gameObject.SetActive(true);
                _hidden.gameObject.SetActive(false);
                _detected.gameObject.SetActive(false);
                break;
            case GameManager.DetectionState.Detected:
                _detected.gameObject.SetActive(true);
                _hidden.gameObject.SetActive(false);
                _alerted.gameObject.SetActive(false);
                break;
        }
    }

}
