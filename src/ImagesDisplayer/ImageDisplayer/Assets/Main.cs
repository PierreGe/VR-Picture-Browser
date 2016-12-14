using UnityEngine;
using System.Collections;
using Assets;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using System.Linq;

public class Main : MonoBehaviour
{
    List<WWW> w = new List<WWW>();
    int index;
    List<GameObject> planes = new List<GameObject>();
    List<GameObject> suggestions = new List<GameObject>();
    int frames = 0;
    PicturesManager pM;
    HashSet<Picture> currentPictures = new HashSet<Picture>();
    bool waitForAnd = false;
    bool waitForOr = false;

    public GameObject currentSelected = null;



    float scale = 15;

    public static Main scriptInstance;

    String shownTags = "";
	// Use this for initialization
	void Start ()
	{


        const string jsonPath = "Assets/classification/resultimgobj.json";
		this.pM = PicturesLoader.parseJson (jsonPath);
        scriptInstance = this;
        //        HashSet<Picture> selectedPictures = new HashSet<Picture>();
        //        var it = PicturesManager.pictures.GetEnumerator();
        //        for(int i = 0; i < 30; i++)
        //        {
        //            selectedPictures.Add(it.Current);
        //            Debug.Log(it.Current.getPath());
        //            it.MoveNext();
        //        }
        //		loadPictures(pM.getPicturesForATag("valley"));
        shownTags = "Say a tag to start. \n Look down for Suggestions!";
        showTag();
        List<KeyValuePair<String, HashSet<Picture>>> dic = PicturesManager.pictureDictionary.ToList();
        dic.Sort((pair1, pair2) => -(pair1.Value.Count.CompareTo(pair2.Value.Count)));
        showSuggestions(dic.Take(5).Select(s => s.Key).ToArray());

    }

	private void resize(GameObject theGameObject, float newSizex, float newSizey)
	{

		float sizex = theGameObject.GetComponent<Renderer>().bounds.size.x;
        Debug.Log(sizex);
        float sizey = theGameObject.GetComponent<Renderer>().bounds.size.y;
        Debug.Log(sizey);

        Vector3 rescale = theGameObject.transform.localScale;

		rescale.x = newSizex * rescale.x / sizex;
		rescale.y = newSizey * rescale.y / sizey;

		theGameObject.transform.localScale = rescale;

	}

    private void onClick(object sender, ClickedEventArgs e) {
        if (currentSelected)
        {
            Debug.Log("CLICK CLICK MOFO");
        }

    }

    public void onAndRecognised()
    {
        waitForAnd = true;
        shownTags += " AND ";
        showTag();
    }

    public void onOrRecognised()
    {
        waitForOr = true;
        shownTags += " OR ";
        showTag();
    }

    public void onSmallerRecognised()
    {
        scale = (float) 0.85 * scale;
        loadPictures(currentPictures);
    }

    public void onBiggerRecognised()
    {
        scale = (float)1.15 * scale;
        loadPictures(currentPictures);
    }

    public void onResetRecognised()
    {
        this.destroyPlanes();
        scale = 15;
        waitForAnd = false;
        waitForOr = false;
    }

    private void loadTexture(string filename)
	{
		string fullPath = "file://" + Path.GetFullPath(@"data/") + filename ;
		w.Add(new WWW(fullPath));
	}
    // Update is called once per frame
    void Update()
    {
        Vector3 fwd = Camera.main.transform.TransformDirection(Vector3.forward);
        Ray ray = new Ray(transform.position, fwd);
        RaycastHit hit = default(RaycastHit);
        if (Physics.Raycast(ray, out hit)) {
            GameObject target = hit.collider.gameObject;
            if (target != currentSelected)
            {
                Vector3 cameraPos = Camera.main.transform.position;
                if (currentSelected) {
                    currentSelected.transform.localScale -= new Vector3((float) 0.2,0, (float)0.2); 
                }

                target.transform.localScale += new Vector3((float)0.2, 0, (float)0.2);
     
                currentSelected = target;

            }
        }
        else
        {
            if (currentSelected)
            {
                Vector3 cameraPos = Camera.main.transform.position;
                currentSelected.transform.localScale -= new Vector3((float)0.2, 0, (float)0.2   );
                currentSelected = null;
            }
        }


    }

    public void onTagRecognised(string tag)
    {
        if (waitForOr)
        {
            shownTags += tag;
            showTag();
            currentPictures.UnionWith(pM.getPicturesForATag(tag));
            waitForOr = false;
        } else if (waitForAnd)
        {
            shownTags += tag;
            showTag();
            currentPictures.IntersectWith(pM.getPicturesForATag(tag));
            waitForAnd = false;
        }
        else
        {
            shownTags = tag;
            this.showTag();
            currentPictures = pM.getPicturesForATag(tag);
        }
        if (currentPictures != null)
        {
            loadPictures(currentPictures);
            showSuggestions(selectSuggestions(currentPictures));
        }
        else
        {
            destroyPlanes();
        }
    }

    private void loadPictures(HashSet<Picture> list)
    {
        destroyPlanes();

		float sqrt2 = Mathf.Sqrt (2)/2; 
		float size = scale + (float) 0.4; // changed from 20
        int magicnumber = 90;
        int magicnumber2 = 45;
        int magicnumber3 = 135;
        float[,] indexes = {
			{0,size,magicnumber,0,0},
			{size*sqrt2,size*sqrt2,magicnumber,magicnumber2,0},
			{-size*sqrt2,size*sqrt2, magicnumber, -magicnumber2, 0},
			{size,0, magicnumber, magicnumber, 0},
			{-size,0, magicnumber, -magicnumber, 0},
			{size*sqrt2,-size*sqrt2, magicnumber, magicnumber3, 0},
			{-size*sqrt2,-size*sqrt2, magicnumber, -magicnumber3, 0},
			{0,-size,magicnumber,180,0}
		};
		index = 0;
		int current = 0;
		int level = 0;
        foreach (Picture picture in list)
		{
			if (current == 8)
			{
				current = 0;
				level ++;
			}
			float indexx = indexes [current,0];
			float indexy = level* (scale - (float) 0.5) + (float) 5; // changed from *12 +5
			float indexz = indexes [current, 1];

            planes.Add(GameObject.CreatePrimitive(PrimitiveType.Plane));
			planes[index].transform.position = new Vector3(indexx, indexy, indexz);
			planes [index].transform.Rotate (new Vector3 (indexes [current,2], 180 + indexes [current,3], indexes [current,4]));
            loadTexture(picture.getPath());
            indexx += size;
			current++;
			index++;
        }
        int i = 0;
        while (i < w.Count)
        {
            if (w[i].isDone)
            {
                planes[i].GetComponent<Renderer>().material.mainTexture = w[i].texture;
                i++;
            }

        }
    }

    private void showTag()
    {
        GameObject go = GameObject.Find("TagText");
        TextMesh text = go.GetComponent<TextMesh>();
        text.text = shownTags;
    }

    private string[] selectSuggestions(HashSet<Picture> pictures)
    {
        Dictionary<string, int> words = new Dictionary<string, int>();
        foreach (Picture p in pictures)
        {
            string[] tags = p.getTags();
            foreach(string tag in tags)
            {
                int val;
                if (words.TryGetValue(tag, out val))
                {
                    val++;
                } else
                {
                    words.Add(tag, 1);
                }
            }
        }
        var list = words.ToList();
        list.Sort((p1, p2) => -p1.Value.CompareTo(p2.Value));
        return list.Take(5).Select(p => p.Key).ToArray();
    }

    private void showSuggestions(String[] words)
    {
        foreach(GameObject g in suggestions)
        {
            Destroy(g);
        }
        suggestions.Clear();
        for (int i = 0; i < words.Length; i++)
        {
            GameObject go = new GameObject();
            suggestions.Add(go);
            TextMesh tm = go.AddComponent<TextMesh>();
            tm.text = words[i];
            tm.color = Color.black;
            tm.transform.Rotate(new Vector3(90, 0, 0));
            tm.transform.position = new Vector3(-5, i - 2, 0);
        }
    }

    private void destroyPlanes()
    {
        w.Clear();
        foreach (GameObject o in planes)
        {
            //Destroy(o.GetComponent<Renderer>().material.mainTexture);
            Destroy(o);
        }
        planes.Clear();
    }
}

