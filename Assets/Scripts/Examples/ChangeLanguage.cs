using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AlexisDev;

public class ChangeLanguage : MonoBehaviour
{
    [SerializeField] private Button btnChangeLanguage;
    
    private void Start()
    {
        UnityShortcuts.AddListener(btnChangeLanguage, StartChange);
    }

    private void StartChange()
    {
        
    }
}
