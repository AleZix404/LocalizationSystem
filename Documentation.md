# Json Translator - Game Localization

This is a powerful tool for game developers who work with JSON format files for translations. Here are some key features available:

- Support for various text types: Text Mesh Pro, Text Mesh Pro UI, Text Mesh, Input Text, Text Legacy, and UI Toolkit.
- Easily add or remove languages.
- Multiple JSON files for the same language are supported. The order doesn't matter.
- Automatic translation for all text-based components based on their type or tag.
- Option to ignore translating specific texts by assigning a tag.
- Change the language with a single line of code!
- Translate entire blocks of texts and store them in lists.
- Obtain interactive text translations for inputs, timers, and more.

Let's keep it simple! Here's a quick guide:

## Setup Instructions

1. Create a JSON file or use existing ones.
2. Add the language to the file name by appending an underscore followed by the language name, for example: `User Interface_Spanish`.
3. Update the `JsonArchiveName` Enum with your JSON file names.
4. Configure the `LanguageSettings` scriptable object by setting the default language.
5. Add your JSON files to the `TextJson` list within the `LanguageSettings`.
6. Select the text types for translation in the "Text Type" section.
7. Enable automatic scene-based translation on Awake if needed.
8. Optionally, exclude specific scenes from translation by adding them to `nonTranslatableScenes`.
9. Choose the search mode for automatic translation: by type or by tag.

## Deployment after Basic Configurations

1. JSON format follows the standard structure:

   ```json
   {
     "MenuOptions": {
       "Continue": { "msm": "Continue" },
       "NewGame": { "msm": "New Game" },
       "Options": { "msm": "Options" },
       "Exit": { "msm": "Exit Game" }
     }
   }

To assign the translation is as follows:
```csharp
textContinue.text = Localization.TranslateDirect("MenuOptions", "Continue", JsonArchiveName.UserInterface);
```
The last parameter is the enum that we must select, this will facilitate the search for the file that contains the translated text

2. To translate automatically you can do it with the following line of code:
```csharp
Localization.instance.AutomaticTranslationTexts();
```
This implies that the header must have the name of your scene:

```json
{
"SceneName":
    {
      "Continue":
      {
        "msm": "Continue"
      }
    }
}
```
3. In case you want a personalized name, just send the name as a parameter:
```csharp
string customName = "MyCustomName";
Localization.instance.AutomaticTranslationTexts(customName);
```
Note: If you do not want to translate a text in automatic mode you can add a tag and assign it, the tag is: "textNotIncluding"

4. If you want to translate something in sequence, you can do so by sending an int as a parameter that increases as you want to expose its textual content.
```csharp
press++;
string detail = Localization.TranslateParts("SelectLanguage", press.ToString(), JsonArchiveName.UserInterface);
```
The json format is as follows:
```json
{
"SelectLanguage":
    {
      "press0":
      {
        "msm": "Spanish"
      },
      "press1":
      {
        "msm": "English"
      }
    }
}
```
Note: In case you keep adding your integer and you no longer have content to display, don't worry about the length, perform a check if it is null or not to print its content:
```csharp
if (!string.IsNullOrEmpty(detail))
```
You can see a clear example in the "LanguageChangeButton" class.
5) If you get the entire translation for a header just call it with:
```csharp
Localization.GetAllValues("TextType", JsonArchiveName.UserInterface)
```
It will return it in the form of a list

# About the package:

You are free to use this package as a complement, I would love for you to contribute to expanding and improving it, it is true that it needs to be refined and performance evaluated further to get the most out of it. All criticism is welcome, if possible include me in your thank you list in any credit that uses this package.

# Future updates:

- Automatic translation for UI Toolkit(All other properties work with UI Toolkit)
- Support for downloading json format files
- (Add your suggestions)
