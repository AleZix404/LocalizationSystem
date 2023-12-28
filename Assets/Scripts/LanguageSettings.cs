using System.Collections.Generic;
using UnityEngine;

namespace AlexisDev.Localization
{
    [CreateAssetMenu(fileName = "LanguageSettings", menuName = "Settings/LanguageSettings")]
    public class LanguageSettings : ScriptableObject
    {
        //Assign the language to this string as it is written as an element in the languages ​​list
        //You can write it from the inspector or from your manager
        [Header("The current language to translate:")]
        [SerializeField] private string selectedLanguage;

        [Header("Type of text:")]
        public bool isTextLegacy;
        public bool isTextMeshProUGUI, isTextMeshPro, isTextMesh, isInputText;

        [Header("Moments of translation:")]
        [Tooltip("Automatic translation of all texts")]
        public bool isLoadAllSceneAwake;

        [Header("Automatic translation of all texts")]
        [Tooltip("You can use the search by type or by tag, whichever seems more convenient, if it is by tag, please add the tag automaticTranslation")]
        public SearchMode searchMode;

        [Header("Scenes that will not translate automatically:")]
        public List<string> nonTranslatableScenes = new List<string>();

        [Header("Add your json here:")]
        public List<TextAsset> TextJson = new List<TextAsset>();

        public string SelectedLanguage { get => selectedLanguage; set => selectedLanguage = value; }

        public enum SearchMode
        {
            Type,
            Tag
        }
    }
}

