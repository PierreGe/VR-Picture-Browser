using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Assets
{
    public class PicturesManager
    {
        //Dictionary containing as key the tag and as value the set with all the pictures associated to that tag
        public static Dictionary<String, HashSet<Picture>> pictureDictionary = new Dictionary<string, HashSet<Picture>>();

        //Initialize a picture and add its reference to the values of each of the tags
        public void addPicture(string path, String[] tags)
        {
            Picture p = new Picture(path);
            foreach(string tag in tags){
                addPictureForTag(p, tag);
            }
            p.setTags(tags);
        }
        //intiialize a picture for one tag
        public void addPictureForTag(string path, string tag)
        {
            Picture pic = new Picture(path);
            addPictureForTag(pic, tag);
        }
        //add an initialized picture to a tag
        public void addPictureForTag(Picture pic,string tag)
        {
            HashSet<Picture> value;
            if (pictureDictionary.TryGetValue(tag, out value))
            {
                value.Add(pic);
            }
            else
            {
                value = new HashSet<Picture>();
                value.Add(pic);
                pictureDictionary.Add(tag, value);
            }
        }
        //get the set of pictures given a tag
        public HashSet<Picture> getPicturesForATag(string tag)
        {
            HashSet<Picture> value = null;
            pictureDictionary.TryGetValue(tag, out value);
            HashSet<Picture> returnValue = new HashSet<Picture>();
            if (value != null)
            {
                foreach (Picture p in value)
                {
                    returnValue.Add(p);
                }
            }
            return returnValue;
        }
    }
}
