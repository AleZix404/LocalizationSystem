using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace AlexisDev.Localization
{
    public class Localization : MonoBehaviour
    {
        public static Localization instance;
        public static Dictionary<string, TextAsset> ContnJson = new Dictionary<string, TextAsset>();
        public LanguageSettings languageSettings;

        private List<JsonFileData> JsonFiles = new List<JsonFileData>();
        private Dictionary<string, JsonFileData> JsonFileDataMap = new Dictionary<string, JsonFileData>();
        [Tooltip("Uncheck this if you want to do a search by tag and add the tag: automaticTranslation, to the texts you want to translate")]
        public bool isTypeSearch;

        private List<Component> textComponents = new List<Component>();

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;

            DontDestroyOnLoad(gameObject);

            LoadFiles();
            if (languageSettings.isLoadAllScene)
            {
                SceneManager.sceneLoaded += OnSceneLoaded;
            }
        }
        private void LoadFiles()
        {
            foreach (var textAsset in languageSettings.TextJson)
            {
                JsonFiles.Add(JsonFileData.FromTextAsset(textAsset));
                ContnJson[textAsset.name] = textAsset;

                var jsonFileData = JsonFileData.FromTextAsset(textAsset);
                JsonFileDataMap[jsonFileData.FileName] = jsonFileData;
                JsonFileDataMap[jsonFileData.ArchiveName.ToString() + languageSettings.SelectedLanguage] = jsonFileData;
            }
            if (languageSettings.isLoadAllSceneAwake &&
                !ShouldTranslateScene(SceneManager.GetActiveScene()))
            {
                AutomaticTranslationTexts();
            }
        }
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (ShouldTranslateScene(scene))
                return;

            AutomaticTranslationTexts();
        }
        private bool ShouldTranslateScene(Scene scene)
        {
            var skippableScene = languageSettings.nonTranslatableScenes.FirstOrDefault(m => m == scene.name);
            return languageSettings.nonTranslatableScenes.Count > 0 && skippableScene != null;
        }
        public void AutomaticTranslationTexts(string header = "")
        {
            header = string.IsNullOrEmpty(header) ? SceneManager.GetActiveScene().name : header;
            Component[] textComponents = GetTextComponents();
            if (textComponents?.Length > 0)
            {
                foreach (Component component in textComponents)
                {
                    if (component != null)
                    {
                        string textObjectName = component.tag.Contains("textNotIncluding") ? string.Empty : component.gameObject.name;
                        string translation = TranslateDirect(header, textObjectName, JsonArchiveName.UserInterface);
                        if (!string.IsNullOrEmpty(translation))
                        {
                            switch (component)
                            {
                                case Text textComponent:
                                    textComponent.text = translation;
                                    break;

                                case TextMeshPro tmpComponent:
                                    tmpComponent.text = translation;
                                    break;

                                case TextMeshProUGUI tmpUGUIComponent:
                                    tmpUGUIComponent.text = translation;
                                    break;

                                case InputField inputField:
                                    inputField.text = translation;
                                    break;

                                case TextMesh textMesh:
                                    textMesh.text = translation;
                                    break;
                                // Add more cases for other component types if necessary
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
        }
        private Component[] GetTextComponents()
        {
            /*Beta version
            Right now the improvement is little or it is just a matter of taste in terms of implementation,
            I hope to be able to improve with the intention of obtaining good performance in comparison.*/

            isTypeSearch = languageSettings.searchMode == LanguageSettings.SearchMode.Type;

            //Support for UI Toolkit in the future
            SearchAndAddTextComponents<Text>(languageSettings.isTextLegacy);
            SearchAndAddTextComponents<TextMeshPro>(languageSettings.isTextMeshPro);
            SearchAndAddTextComponents<TextMeshProUGUI>(languageSettings.isTextMeshProUGUI);
            SearchAndAddTextComponents<InputField>(languageSettings.isInputText);
            SearchAndAddTextComponents<TextMesh>(languageSettings.isTextMesh);

            // Returns the list of components that meet the conditions.
            return textComponents.ToArray();
        }

        private void SearchAndAddTextComponents<T>(bool shouldSearch) where T : Component
        {
            if (shouldSearch)
            {
                if (isTypeSearch)
                    AddComponentsOfType<T>();
                else
                    AddComponentsByTag();
            }
        }

        private void AddComponentsOfType<T>() where T : Component
        {
            textComponents.AddRange(FindObjectsOfType<T>(true));
        }

        private void AddComponentsByTag()
        {
            GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("automaticTranslation");
            foreach (var obj in objectsWithTag)
            {
                textComponents.AddRange(obj.GetComponents<Component>());
            }
        }

        public void AutomaticTranslationTexts(JsonArchiveName indexJson)
        {
            Text[] textComponents = FindObjectsOfType<Text>(true);

            for (int i = 0; i < textComponents.Length; i++)
            {
                Text textComponent = textComponents[i];
                string textObjectName = textComponent.tag.Contains("textNotIncluding") ? string.Empty : textComponent.name;
                string translation = GetAllValues(indexJson)[i];
                Debug.Log("AutomaticTranslationTexts - translation: " + translation + " textObjectName: " + textObjectName);

                if (!string.IsNullOrEmpty(translation))
                {
                    textComponent.text = translation;
                }
            }
        }

        /// <summary>
        /// part refers to the number of times the continue button has been pressed
        /// </summary>
        /// <param name="header"></param>
        /// <param name="part"></param>
        /// <param name="indexJson"></param>
        /// <returns></returns>
        public static string TranslateParts(string header, string part, JsonArchiveName indexJson)
        {
            string fileName = $"{indexJson}_{instance.languageSettings.SelectedLanguage}";
            Debug.Log($"ContentTranslate: {header}");

            Debug.Log($"FileName: {fileName}");

            if (!ContnJson.TryGetValue(fileName, out TextAsset textAsset))
            {
                Debug.Log($"Text not found for: {fileName}");
                return null;
            }

            JObject jsonObject = JObject.Parse(textAsset.text);

            if (jsonObject[header] == null)
            {
                Debug.Log($"Text not found for: {header}");
                return null;
            }

            JObject innerObject;
            while ((innerObject = (JObject)jsonObject[header]["press" + part]) != null)
            {
                if (innerObject["msm"] != null)
                {
                    return innerObject["msm"].ToString();
                }
            }

            Debug.Log($"Content not found for: {header}");
            return null;
        }
        /// <summary>
        /// El contenido del texto no es sucesivo, es directo, por tanto requiere el nombre del sub header, en el parm content
        /// </summary>
        /// <param name="header"></param>
        /// <param name="content"></param>
        /// <param name="indexJson"></param>
        /// <returns></returns>
        public static string TranslateDirect(string header, string content, JsonArchiveName indexJson)
        {
            string fileName = $"{indexJson}_{instance.languageSettings.SelectedLanguage}";
            //Debug.Log($"ContentTranslate: {header}");

            if (!ContnJson.TryGetValue(fileName, out TextAsset textAsset))
            {
                Debug.Log($"Text not found for: {fileName}");
                return null;
            }

            JObject jsonObject = JObject.Parse(textAsset.text);

            if (jsonObject[header] == null)
            {
                Debug.Log($"Text not found for: {header}");
                return null;
            }

            JObject innerObject;
            if ((innerObject = (JObject)jsonObject[header][content]) != null)
            {
                if (innerObject["msm"] != null)
                {
                    return innerObject["msm"].ToString();
                }
            }

            //Debug.Log($"Content not found for: {header}");
            return null;
        }
        public List<string> GetAllValues(JsonArchiveName indexJson)
        {
            string fileName = $"{indexJson}_{instance.languageSettings.SelectedLanguage}";
            //Debug.Log($"GetAllValues - indexJson: {indexJson}");

            List<string> values = new List<string>();

            if (!ContnJson.TryGetValue(fileName, out TextAsset textAsset))
            {
                Debug.LogError($"Text not found for: {fileName}");
            }

            JObject jsonObject = JObject.Parse(textAsset.text);

            if (jsonObject == null)
            {
                Debug.LogError($"Text not found for: {jsonObject}");
            }

            foreach (var subHeader in jsonObject.Children<JProperty>())
            {
                if (subHeader.Value["msm"] != null)
                {
                    values.Add(subHeader.Value["msm"].ToString());
                }
            }

            return values;
        }
        public static List<string> GetAllValues(string header, JsonArchiveName indexJson)
        {
            string fileName = $"{indexJson}_{instance.languageSettings.SelectedLanguage}";
            //Debug.Log($"GetAllValues: {header}");

            List<string> values = new List<string>();

            if (!ContnJson.TryGetValue(fileName, out TextAsset textAsset))
            {
                Debug.Log($"Text not found for: {fileName}");
                return values;
            }

            JObject jsonObject = JObject.Parse(textAsset.text);

            if (jsonObject[header] == null)
            {
                Debug.Log($"Text not found for: {header}");
                return values;
            }

            foreach (var subHeader in jsonObject[header].Children<JProperty>())
            {
                if (subHeader.Value["msm"] != null)
                {
                    values.Add(subHeader.Value["msm"].ToString());
                }
            }

            return values;
        }

        private class JsonFileData
        {
            public string FileName { get; set; }
            public JsonArchiveName ArchiveName { get; set; }
            public JObject Data { get; set; }

            public static JsonFileData FromTextAsset(TextAsset textAsset)
            {
                var json = JObject.Parse(textAsset.text);
                return new JsonFileData
                {
                    FileName = textAsset.name,
                    ArchiveName = (JsonArchiveName)Enum.Parse(typeof(JsonArchiveName), textAsset.name.Split('_')[0], true),
                    Data = json,
                };
            }

            public JObject this[string header] => Data[header] as JObject;
        }
    }
}