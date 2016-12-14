using UnityEngine;
using System.Collections;
using Assets;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;

public class Main : MonoBehaviour
{
    List<WWW> w = new List<WWW>();
	int index;
    List<GameObject> planes = new List<GameObject>();
    int frames = 0;
    PicturesManager pM;
    HashSet<Picture> currentPictures = new HashSet<Picture>();
    bool waitForAnd = false;
    bool waitForOr = false;

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
        w.Clear();
        foreach (GameObject o in planes)
        {
            Destroy(o);
        }
        planes.Clear();
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
	void Update ()
	{

		
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
        }
        else
        {
            w.Clear();
            foreach (GameObject o in planes)
            {
                Destroy(o);
            }
            planes.Clear();
        }
    }

    private void loadPictures(HashSet<Picture> list)
    {
        w.Clear();
        foreach (GameObject o in planes)
        {
            Destroy(o);
        }
        planes.Clear();

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
}

