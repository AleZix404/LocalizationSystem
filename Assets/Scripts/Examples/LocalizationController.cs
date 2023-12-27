using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AlexisDev.Localization.OtherComponent
{
    public class LocalizationController : MonoBehaviour
    {
        #region Variables
        [Header("By text type")]
        [SerializeField] private Text textLegacy;
        [SerializeField] private TextMeshPro textMeshPro;
        [SerializeField] private TextMeshProUGUI textMeshProUGUI;
        [SerializeField] private TextMesh textMesh;
        [SerializeField] private Text description;

        public bool isGetAllContent;

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
        }
        private void TranslateContent()
        {
            textLegacy.text = Localization.TranslateDirect("TextType", "TextLegacy", JsonArchiveName.UserInterface);
            textMeshPro.text = Localization.TranslateDirect("TextType", "TextMeshPro", JsonArchiveName.UserInterface);
            textMeshProUGUI.text = Localization.TranslateDirect("TextType", "TextMeshProUGUI", JsonArchiveName.UserInterface);
            textMesh.text = Localization.TranslateDirect("TextType", "TextMesh", JsonArchiveName.UserInterface);

            GetAllContent();
        }

        private void GetAllContent()
        {
            if (isGetAllContent)
            {
                description.text = string.Empty;
                StringBuilder descriptionBuilder = new StringBuilder();

                foreach (var text in Localization.GetAllValues("TextType", JsonArchiveName.UserInterface))
                {
                    descriptionBuilder.Append($"<color=yellow>{text}</color>\n");
                }

                description.text = descriptionBuilder.ToString();
            }
        }
        #endregion
    }
}

