using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
namespace AlexisDev
{
    public static class UnityShortcuts
    {
        #region Initialization
        public static void SetSingleton(GameObject instance, ref GameObject currentInstance)
        {
            if (currentInstance != null && currentInstance != instance)
            {
                Object.Destroy(instance);
                return;
            }

            currentInstance = instance;

            Object.DontDestroyOnLoad(instance);
        }
        #endregion
        #region Listeners

        #endregion
        public static void AddListener(Button button, UnityAction callback)
        {
            button.onClick.AddListener(callback);
        }
        public static void AddListeners(List<Button> button, UnityAction callback)
        {
            button.ForEach(button => button.onClick.AddListener(callback));
        }
        public static void AddListeners<T>(List<Button> buttons, UnityAction<T> callbacks, List<T> elements)
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                var gObject = elements[i];
                buttons[i].onClick.AddListener(() => callbacks(gObject));
                Debug.Log($"AddListeners<T>{i} btn {i} elements {i}");
            }
        }
        public static void AddListeners(List<Button> buttons, UnityAction<int> callbacks)
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                int index = i;
                buttons[i].onClick.AddListener(() => callbacks(index));
            }
        }
        public static void AddListeners(List<Button> button, UnityAction<Button> callback)
        {
            button.ForEach(button => button.onClick.AddListener(()=> callback.Invoke(button)));
        }
        public static void CambiarColorHex(string codigoHex, Image image)
        {
            Color colorNuevo;

            if (ColorUtility.TryParseHtmlString(codigoHex, out colorNuevo))
            {
                image.color = colorNuevo;
            }
        }
    }
}

