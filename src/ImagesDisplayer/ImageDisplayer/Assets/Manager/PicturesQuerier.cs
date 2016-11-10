using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Assets
{
    public class PicturesQuerier
    {
        private const string SHOW_PICTURES_COMMAND = "show me pictures with";
        private PicturesManager picturesManager;
        public PicturesQuerier(PicturesManager pm)
        {
            picturesManager = pm;
        }

        public HashSet<Picture> parseRequest(string request)
        {
            // tag request
            if(request.StartsWith(SHOW_PICTURES_COMMAND))
            {
                string tagsRaw = request.Substring(SHOW_PICTURES_COMMAND.Length);
                string[] tags = tagsRaw.Split(' ');
                HashSet<Picture> result = new HashSet<Picture>();
                if (tags.Length > 0)
                {

                }
                return result;
            }
            return null;
        }
    }
}
