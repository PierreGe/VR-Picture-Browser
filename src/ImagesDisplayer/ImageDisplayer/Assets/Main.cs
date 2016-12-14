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
    GameObject currentSelected = null;

    public static Main scriptInstance;
    

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
		float sizey = theGameObject.GetComponent<Renderer>().bounds.size.y;

		Vector3 rescale = theGameObject.transform.localScale;

		rescale.x = newSizex * rescale.x / sizex;
		rescale.y = newSizey * rescale.y / sizey;

		theGameObject.transform.localScale = rescale;

	}

    public void onAndRecognised()
    {
        waitForAnd = true;
    }

    public void onOrRecognised()
    {
        waitForOr = true;
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
                Debug.Log(fwd);
                Vector3 cameraPos = Camera.main.transform.position;
                if (currentSelected) {
                    currentSelected.transform.position -= Vector3.MoveTowards(currentSelected.transform.position, new Vector3(cameraPos.x, currentSelected.transform.position.y, cameraPos.z), (float)0.01);
                }

                target.transform.position += Vector3.MoveTowards(target.transform.position, new Vector3(cameraPos.x, target.transform.position.y, cameraPos.z), (float)0.01);
                currentSelected = target;

            }
        }
        else
        {
            if (currentSelected)
            {
                Vector3 cameraPos = Camera.main.transform.position;
                currentSelected.transform.position -= Vector3.MoveTowards(currentSelected.transform.position, new Vector3(cameraPos.x, currentSelected.transform.position.y, cameraPos.z), (float)0.01);
                currentSelected = null;
            }
        }

    }

    public void onTagRecognised(string tag)
    {
        if (waitForOr)
        {
            currentPictures.UnionWith(pM.getPicturesForATag(tag));
            waitForOr = false;
        } else if (waitForAnd)
        {
            currentPictures.IntersectWith(pM.getPicturesForATag(tag));
            waitForAnd = false;
        }
        else
        {
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
		float size = 20;
		float[,] indexes = {
			{0,size,90,0,0},
			{size*sqrt2,size*sqrt2,90,45,0},
			{-size*sqrt2,size*sqrt2, 90, -45, 0},
			{size,0, 90, 90, 0},
			{-size,0, 90, -90, 0},
			{size*sqrt2,-size*sqrt2, 90, 135, 0},
			{-size*sqrt2,-size*sqrt2, 90, -135, 0},
			{0,-size,90,180,0}
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
			float indexy = level*12 + 5;
			float indexz = indexes [current, 1];

            planes.Add(GameObject.CreatePrimitive(PrimitiveType.Plane));
			planes[index].transform.position = new Vector3(indexx, indexy, indexz);
			planes [index].transform.Rotate (new Vector3 (indexes [current,2], 180 + indexes [current,3], indexes [current,4]));
            loadTexture(picture.getPath());
            indexx += 20;
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
}

