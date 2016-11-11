using UnityEngine;
using System.Collections;
using Assets;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public class Main : MonoBehaviour
{
    List<WWW> w = new List<WWW>();
	int index;
    List<GameObject> planes = new List<GameObject>();
    int frames = 0;
    PicturesManager pM;

    public static Main scriptInstance;
	// Use this for initialization
	void Start ()
	{
		const string jsonPath = "Assets/classification/resultimgobj.json";
		this.pM = PicturesLoader.parseJson (jsonPath);
        scriptInstance = this;
        /*HashSet<Picture> selectedPictures = new HashSet<Picture>();
        var it = PicturesManager.pictures.GetEnumerator();
        for(int i = 0; i < 30; i++)
        {
            selectedPictures.Add(it.Current);
            Debug.Log(it.Current.getPath());
            it.MoveNext();
        }
        loadPictures(selectedPictures);*/
    }

	private void resize(GameObject theGameObject, float newSizex, float newSizey)
	{

		float sizex = theGameObject.GetComponent<Renderer>().bounds.size.x;
		float sizey = theGameObject.GetComponent<Renderer>().bounds.size.y;

		Vector3 rescale = theGameObject.transform.localScale;

		rescale.x = newSizex * rescale.x / sizex;
		rescale.y = newSizey * rescale.y / sizey;

		theGameObject.transform.localScale = rescale;

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
        HashSet<Picture> list = pM.getPicturesForATag(tag);
        if (list != null)
        {
            loadPictures(list);
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
        index = 0;
        float indexx = 0;
        float indexy = 10;
        foreach (Picture picture in list)
        {
            planes.Add(GameObject.CreatePrimitive(PrimitiveType.Plane));
            planes[index].transform.position = new Vector3(indexx, indexy, 0);
            planes[index].transform.Rotate(new Vector3(90, 0, 0));
            loadTexture(picture.getPath());
            indexx += 20;
            index++;
            if (indexx >= 100)
            {
                indexx = 0;
                indexy += 20;
            }
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
}

