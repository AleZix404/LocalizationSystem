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
        public static Dictionary<string, TextAsset> contnJson = new Dictionary<string, TextAsset>();
        public LanguageSettings languageSettings;

        private List<JsonFileData> jsonFiles = new List<JsonFileData>();
        private Dictionary<string, JsonFileData> jsonFileDataMap = new Dictionary<string, JsonFileData>();
        [Tooltip("Uncheck this if you want to do a search by tag and add the tag: automaticTranslation, to the texts you want to translate")]
        public bool searchByType;

        private void Awake()
        {
            CheckDuplicateInstance();
            DontDestroyOnLoad(gameObject);

            LoadFilesOnAwake();
            SubscribeToSceneLoadedEvent();
        }

        private void CheckDuplicateInstance()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
        }

        private void LoadFilesOnAwake()
        {
            FileManagerSettings fileManagerSettings = new(jsonFiles, jsonFileDataMap, contnJson);
            FileManager.LoadFiles(fileManagerSettings);

            if (languageSettings.isLoadAllSceneAwake && !ShouldTranslateScene(SceneManager.GetActiveScene()))
            {
                AutomaticTranslationTexts();
            }
        }

        private void SubscribeToSceneLoadedEvent()
        {
            if (languageSettings.isLoadAllSceneAwake)
            {
                SceneManager.sceneLoaded += OnSceneLoaded;
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (ShouldTranslateScene(scene))
                return;

            AutomaticTranslationTexts();
        }
        public bool ShouldTranslateScene(Scene scene)
        {
            var skippableScene = languageSettings.nonTranslatableScenes.FirstOrDefault(m => m == scene.name);
            return languageSettings.nonTranslatableScenes.Count > 0 && skippableScene != null;
        }
        public void AutomaticTranslationTexts(string header = "")
        {
            header = string.IsNullOrEmpty(header) ? SceneManager.GetActiveScene().name : header;
            Component[] textComponents = TextComponentFinder.GetTextComponents();
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
        
        public void AutomaticTranslationTexts(JsonArchiveName indexJson)
        {
            Text[] textComponents = FindObjectsOfType<Text>(true);

            for (int i = 0; i < textComponents.Length; i++)
            {
                Text textComponent = textComponents[i];
                string translation = GetAllValues(indexJson)[i];
                //string textObjectName = textComponent.tag.Contains("textNotIncluding") ? string.Empty : textComponent.name;
                //Debug.Log("AutomaticTranslationTexts - translation: " + translation + " textObjectName: " + textObjectName);

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
            //Debug.Log($"ContentTranslate: {header}");

            //Debug.Log($"FileName: {fileName}");

            if (!contnJson.TryGetValue(fileName, out TextAsset textAsset))
            {
                Debug.Log($"Text not found for: {fileName} - header: {header}");
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
        /// The content of the text is not successive, it is direct, therefore it requires the name of the sub header, in the content parm
        /// </summary>
        /// <param name="header"></param>
        /// <param name="content"></param>
        /// <param name="indexJson"></param>
        /// <returns></returns>
        public static string TranslateDirect(string header, string content, JsonArchiveName indexJson)
        {
            string fileName = $"{indexJson}_{instance.languageSettings.SelectedLanguage}";
            //Debug.Log($"ContentTranslate: {header}");

            if (!contnJson.TryGetValue(fileName, out TextAsset textAsset))
            {
                Debug.Log($"Text not found for: {fileName} - header: {header}");
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
        public static List<string> GetAllValues(JsonArchiveName indexJson)
        {
            string fileName = $"{indexJson}_{instance.languageSettings.SelectedLanguage}";
            //Debug.Log($"GetAllValues - indexJson: {indexJson}");

            List<string> values = new List<string>();

            if (!contnJson.TryGetValue(fileName, out TextAsset textAsset))
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

            if (!contnJson.TryGetValue(fileName, out TextAsset textAsset))
            {
                Debug.Log($"Text not found for: {fileName} - header: {header}");
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
    }
}