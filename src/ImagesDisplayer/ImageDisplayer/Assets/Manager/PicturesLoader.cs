using System;
using UnityEngine;
using Newtonsoft.Json.Linq;
using System.IO;

namespace Assets
{
    public class PicturesLoader
    {
        private const double LIMIT = 0.35;

        public static PicturesManager parseJson(string path)
        {
            string jsonFile = File.ReadAllText(path);
            JObject obj = JObject.Parse(jsonFile);
            JToken currentObj = obj.First;
            PicturesManager picturesManager = new PicturesManager();
            while (currentObj != null)
            {
                JProperty currentProperty = (JProperty)currentObj;
                string name = currentProperty.Name;
                JToken values = currentProperty.Value;
                foreach (JToken value in values)
                {
                    JProperty score = (JProperty)value.First;
                    JProperty tags = (JProperty)value.Last;
                    if ((double)score.Value > LIMIT)
                    {
                        String selectedTags = (String)tags.First;
                        String[] splitted = selectedTags.Split(',');
                        foreach (String s in splitted)
                        {
                            picturesManager.addPictureForTag(name, s.Trim());
                        }
                    }
                }
                currentObj = currentObj.Next;
            }
            return picturesManager;
        }

    }
}
