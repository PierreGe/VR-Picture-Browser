using UnityEngine;
using System.Collections;
using Assets;
using System.IO;
using System.Collections.Generic;

public class Main : MonoBehaviour
{
	List<WWW> w;
	int index;
	List<GameObject> planes;

	// Use this for initialization
	void Start ()
	{
		string filename = @"Assets/data/100001.jpg";
		Debug.Log (Path.GetFullPath(filename));
		const string jsonPath = "Assets/classification/resultimgobj.json";
		PicturesManager pM = PicturesLoader.parseJson (jsonPath);
		HashSet<Picture> list = pM.getPicturesForATag ("valley");


		w = new List<WWW>();
		planes = new List<GameObject>();
		index = 0;

		//        var info = new DirectoryInfo("B:\\Images\\Dossier");
		//        FileInfo[] fileInfo = info.GetFiles();
		//        IEnumerable<string> fullNames = fileInfo.Select(file => file.FullName);


		Debug.Log ("List " + pM.pictureDictionary);

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
			if (indexx >= 100) {
				indexx = 0;
				indexy += 20;
			} 
		} 
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
		string fullPath = "file://" + Path.GetFullPath(@"Assets/data/") + filename ;
		w.Add(new WWW(fullPath));
	}
	// Update is called once per frame
	void Update ()
	{
		for (int i = 0; i < w.Count; i++)
		{
			if (w[i].isDone)
			{
				planes[i].GetComponent<Renderer>().material.mainTexture = w[i].texture;
			}

		}
		
	}
}

