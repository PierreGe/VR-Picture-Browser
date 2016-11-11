using UnityEngine;
using System.Collections;
using UnityEngine.Windows.Speech;
using UnityEngine.UI;
using System.Collections.Generic;

public class DictationScript : MonoBehaviour
{
    KeywordRecognizer keywordRecognizer;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();
    

    void Start()
    {
        keywords.Add("hello", () =>
        {
            testCalled();
        });
        string[] keys = new string[keywords.Keys.Count];
        keywords.Keys.CopyTo(keys, 0);
        print(keys[0]);
        keywordRecognizer = new KeywordRecognizer(keys);
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizedOnPhraseRecognized;
        keywordRecognizer.Start();
        print(keywordRecognizer.IsRunning);
    }

    void KeywordRecognizedOnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        print("recognized");
        System.Action keywordAction;

        if(keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }

    void testCalled()
    {
        print("it works!");
    }
}
