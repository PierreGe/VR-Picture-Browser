using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets
{
    public class Picture
    {
        //the path of the picture
        private string path;
        //a list of tags
        private string[] tags;
        
        /* getters and setters */

        public Picture(string path)
        {
            this.path = path;
        }

        public string getPath()
        {
            return path;
        }

        public void setTags(string[] tags)
        {
            this.tags = tags;
        }

        public string[] getTags()
        {
            return tags;
        }

        // Equals method 
        override
        public bool Equals(Object obj)
        {
            if (obj is Picture)
            {
                Picture aPicture = (Picture)obj;
                return this.path.Equals(aPicture.getPath());
            } else
            {
                return false;
            }
        }
    }
}
