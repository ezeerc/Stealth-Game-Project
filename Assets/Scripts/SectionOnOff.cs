using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionOnOff : MonoBehaviour
{
    [SerializeField] GameObject[] sectionsOn;
    [SerializeField] GameObject[] sectionsOff;
    [SerializeField] Enemy enemyToKill;
    [SerializeField] float timeToKill;
    private bool _oneTime;
    
    private void Awake()
    {
        foreach (var element in sectionsOn)
        {
            element.SetActive(false);
        }
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (!_oneTime)
        {
            GameManager.Instance.SaveGame();
            _oneTime = true;
        }
        
        if (other.gameObject.layer == 6)
        {
            foreach (var element in sectionsOn)
            {
                element.SetActive(true);
                KillEnemy();
            }
            
            foreach (var element in sectionsOff)
            {
                element.SetActive(false);
            }
        }
    }

    private void KillEnemy()
    {
        if (enemyToKill)
        {
            StartCoroutine(KillEnemyCoroutine(timeToKill));
        }
    }

    IEnumerator KillEnemyCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        enemyToKill.TakeDamage(100);
    }
}
