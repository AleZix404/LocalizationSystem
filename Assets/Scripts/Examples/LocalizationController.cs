using AlexisDev;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LocalizationController : MonoBehaviour
{
    #region Variables
    [Header("By text type")]
    [SerializeField] private Text textLegacy;
    [SerializeField] private TextMeshPro textMeshPro;
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    [SerializeField] private TextMesh textMesh;
    [SerializeField] private Text description;

    [SerializeField] private Button btnChangeLanguage;
    [SerializeField] private Text txtChangeLanguage;

    public bool isGetAllContent;
    private int press;
    public enum SearchType
    {
        reference,
        component,
        
    }
    #endregion

    #region Methods
    void Start()
    {
        TranslateContent();

        UnityShortcuts.AddListener(btnChangeLanguage, ChangeLanguage);
    }
    private void TranslateContent()
    {
        textLegacy.text = Localization.TranslateDirect("TextType", "TextLegacy", JsonArchiveName.UserInterface);
        textMeshPro.text = Localization.TranslateDirect("TextType", "TextMeshPro", JsonArchiveName.UserInterface);
        textMeshProUGUI.text = Localization.TranslateDirect("TextType", "TextMeshProUGUI", JsonArchiveName.UserInterface);
        textMesh.text = Localization.TranslateDirect("TextType", "TextMesh", JsonArchiveName.UserInterface);

        txtChangeLanguage.text = Localization.TranslateDirect("btnChangeLanguage", "TextName", JsonArchiveName.UserInterface);
        GetAllContent();
    }

    private void GetAllContent()
    {
        if (isGetAllContent)
        {
            StringBuilder descriptionBuilder = new StringBuilder();

            foreach (var text in Localization.GetAllValues("TextType", JsonArchiveName.UserInterface))
            {
                descriptionBuilder.Append($"<color=yellow>{text}</color>\n");
            }

            description.text += descriptionBuilder.ToString();
        }
    }

    private void ChangeLanguage()
    {
        press++;
        string detail = Localization.TranslateParts("SelectLanguage", press.ToString(), JsonArchiveName.UserInterface);

        if (string.IsNullOrEmpty(detail))
        {
            press = 0;
            detail = Localization.TranslateParts("SelectLanguage", press.ToString(), JsonArchiveName.UserInterface);
        }

        Localization.instance.languageSettings.SelectedLanguage = detail;
        TranslateContent();
    }
    #endregion
}
