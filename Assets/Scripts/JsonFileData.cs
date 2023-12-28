using Newtonsoft.Json.Linq;
using System;
using UnityEngine;

namespace AlexisDev.Localization
{
    public class JsonFileData
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
