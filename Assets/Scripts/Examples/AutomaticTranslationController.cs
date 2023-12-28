using UnityEngine;

/*
 What this script does is ensure that all the names of the gameobjects that contain a type of text component
can be translated as long as the json header matches the name of the gameobject,
unless it has a "textNotIncluding" tag.
 */
namespace AlexisDev.Localization.OtherComponent
{
    public class AutomaticTranslationController : MonoBehaviour
    {
        [Tooltip("If header is empty, then it will automatically search the entire json for the names" +
            " of all the gameobjects in the entire scene, which contains components of type text, otherwise it will only search" +
            "for the header mentioned in the json")]
        [SerializeField] private string headerJson;
        private void Start()
        {
            SetTranslationTexts();
        }
        //It is also invoked from the button component event
        public void SetTranslationTexts()
        {
            Localization.instance.AutomaticTranslationTexts(headerJson);
        }
    }
}
