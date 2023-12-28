using UnityEngine;
using UnityEngine.UI;

namespace AlexisDev.Localization.OtherComponent
{
    public class LanguageChangeButton : MonoBehaviour
    {
        [SerializeField] private Text txtChangeLanguage;
        private int press;

        public void Start()
        {
            txtChangeLanguage.text = Localization.TranslateDirect("btnChangeLanguage", "TextName", JsonArchiveName.UserInterface);
        }
        //It is invoked from the button component event.
        public void SetTextChange()
        {
            txtChangeLanguage.text = Localization.TranslateDirect("btnChangeLanguage", "TextName", JsonArchiveName.UserInterface);
        }
        //It is invoked from the button component event.
        public void ChangeLanguage()
        {
            press++;
            string detail = Localization.TranslateParts("SelectLanguage", press.ToString(), JsonArchiveName.UserInterface);

            if (string.IsNullOrEmpty(detail))
            {
                press = 0;
                detail = Localization.TranslateParts("SelectLanguage", press.ToString(), JsonArchiveName.UserInterface);
            }

            Localization.instance.languageSettings.SelectedLanguage = detail;
            txtChangeLanguage.text = Localization.TranslateDirect("btnChangeLanguage", "TextName", JsonArchiveName.UserInterface);
        }
    }
}
