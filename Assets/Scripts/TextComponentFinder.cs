using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AlexisDev.Localization
{
    public class TextComponentFinder
    {
        private static readonly string tagTextTranslate = "automaticTranslation";
        private static List<Component> textComponents = new List<Component>();

        public static Component[] GetTextComponents()
        {
            /*Beta version
            Right now the improvement is little or it is just a matter of taste in terms of implementation,
            I hope to be able to improve with the intention of obtaining good performance in comparison.*/

            var languageSettings = Localization.instance.languageSettings;
            Localization.instance.searchByType = languageSettings.searchMode == LanguageSettings.SearchMode.Type;

            //Support for UI Toolkit in the future
            SearchAndAddTextComponents<Text>(languageSettings.isTextLegacy);
            SearchAndAddTextComponents<TextMeshPro>(languageSettings.isTextMeshPro);
            SearchAndAddTextComponents<TextMeshProUGUI>(languageSettings.isTextMeshProUGUI);
            SearchAndAddTextComponents<InputField>(languageSettings.isInputText);
            SearchAndAddTextComponents<TextMesh>(languageSettings.isTextMesh);

            // Returns the list of components that meet the conditions.
            return textComponents.ToArray();
        }

        private static void SearchAndAddTextComponents<T>(bool shouldSearch) where T : Component
        {
            if (shouldSearch)
            {
                if (Localization.instance.searchByType)
                    AddComponentsOfType<T>();
                else
                    AddComponentsByTag();
            }
        }

        private static void AddComponentsOfType<T>() where T : Component
        {
            textComponents.AddRange(GameObject.FindObjectsOfType<T>(true));
        }

        private static void AddComponentsByTag()
        {
            GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(tagTextTranslate);
            foreach (var obj in objectsWithTag)
            {
                textComponents.AddRange(obj.GetComponents<Component>());
            }
        }
    }
}

