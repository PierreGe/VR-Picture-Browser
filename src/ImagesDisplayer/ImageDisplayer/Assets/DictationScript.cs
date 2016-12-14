using UnityEngine;
using System.Collections;
using UnityEngine.Windows.Speech;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public class DictationScript : MonoBehaviour
{
    KeywordRecognizer keywordRecognizer;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();
    

    void Start()
    {
        string[] tags = new string[(Assets.PicturesManager.pictureDictionary).Keys.Count+6];
        tags[tags.Length-1] = "and";
        tags[tags.Length - 2] = "or";
        tags[tags.Length - 3] = "closer";
        tags[tags.Length - 4] = "farther";
        tags[tags.Length - 5] = "further";
        tags[tags.Length - 6] = "reset";
        (Assets.PicturesManager.pictureDictionary).Keys.CopyTo(tags, 0);
        for (int i = 0; i < tags.Length; i++)
        {
            keywords.Add(tags[i], () =>
            {
                testCalled();
            });
        }
        keywordRecognizer = new KeywordRecognizer(tags, ConfidenceLevel.Low);
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizedOnPhraseRecognized;
        keywordRecognizer.Start();
        print(keywordRecognizer.IsRunning);
    }

    void KeywordRecognizedOnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        print("recognized " + args.text);
        if (args.text.Equals("and"))
        {
            Main.scriptInstance.onAndRecognised();
        } else if (args.text.Equals("or"))
        {
            Main.scriptInstance.onOrRecognised();
        }
        else if (args.text.Equals("closer"))
        {
            Main.scriptInstance.onSmallerRecognised();
        }
        else if (args.text.Equals("farther") || args.text.Equals("further") )
        {
            Main.scriptInstance.onBiggerRecognised();
        }
        else if (args.text.Equals("reset"))
        {
            Main.scriptInstance.onResetRecognised();
        }

        else
        {
            Main.scriptInstance.onTagRecognised(args.text);
        }
    }

    void testCalled()
    {
        print("it works!");
    }
}
