using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AlexisDev.Localization
{
    public class FileManager
    {
        public static void LoadFiles(FileManagerSettings settings)
        {
            var localization = Localization.instance;
            foreach (var textAsset in localization.languageSettings.TextJson)
            {
                settings.JsonFiles.Add(JsonFileData.FromTextAsset(textAsset));
                settings.ContnJson[textAsset.name] = textAsset;

                var jsonFileData = JsonFileData.FromTextAsset(textAsset);
                settings.JsonFileDataMap[jsonFileData.FileName] = jsonFileData;
                settings.JsonFileDataMap[jsonFileData.ArchiveName.ToString() + localization.languageSettings.SelectedLanguage] = jsonFileData;
            }
        }
    }
}

