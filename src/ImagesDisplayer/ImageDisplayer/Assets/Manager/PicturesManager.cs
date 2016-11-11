using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets
{
    public class PicturesManager
    {
        //Dictionary containing as key the tag and as value the set with all the pictures associated to that tag
        public Dictionary<String, HashSet<Picture>> pictureDictionary = new Dictionary<string, HashSet<Picture>>();

        public void addPicture(string path, HashSet<string> tags)
        {
            foreach(string tag in tags){
                addPictureForTag(path, tag);
            }
        }

        public void addPictureForTag(string path,string tag)
        {
            HashSet<Picture> value;
            if (pictureDictionary.TryGetValue(tag, out value))
            {
                value.Add(new Picture(path));
            }
            else
            {
                value = new HashSet<Picture>();
                value.Add(new Picture(path));
                pictureDictionary.Add(tag, value);
            }
        }

        public HashSet<Picture> getPicturesForATag(string tag)
        {
            HashSet<Picture> value = null;
            pictureDictionary.TryGetValue(tag, out value);
            return value;
        }
    }
}
