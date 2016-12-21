using System;
using UnityEngine;
using Newtonsoft.Json.Linq;
using System.IO;

namespace Assets
{
    public class PicturesLoader
    {
        //lower confidence level accepted
        private const double LIMIT = 0.35;

        public static PicturesManager parseJson(string path)
        {
            /*
             * Read the Json file and create it in the object model of the Library.
             * The file contains a list of pairs:
             * {image_path, [{score,tag}]}
             * */
            string jsonFile = File.ReadAllText(path);
            JObject obj = JObject.Parse(jsonFile);
            JToken currentObj = obj.First;
            //initialize a manager
            PicturesManager picturesManager = new PicturesManager();
            //iterate the picture
            while (currentObj != null)
            {
                JProperty currentProperty = (JProperty)currentObj;
                string name = currentProperty.Name;
                JToken values = currentProperty.Value;
                //iterate the {score,tag} list
                foreach (JToken value in values)
                {
                    JProperty score = (JProperty)value.First;
                    JProperty tags = (JProperty)value.Last;
                    //filter the tags for their score
                    if ((double)score.Value > LIMIT)
                    {
                        //separate the tags
                        String selectedTags = (String)tags.First;
                        String[] splitted = selectedTags.Split(',');
                        for(int i = 0; i < splitted.Length; i++)
                        {
                            splitted[i] = splitted[i].Trim();
                        }
                        //add the picture in the manager
                        picturesManager.addPicture(name, splitted);
                        
                    }
                }
                //update the currentobject
                currentObj = currentObj.Next;
            }
            //return the manager
            return picturesManager;
        }

    }
}
