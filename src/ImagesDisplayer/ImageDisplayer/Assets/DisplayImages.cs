using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;

public class DisplayImages : MonoBehaviour
{
    List<WWW> w;
    int index;
    List<GameObject> planes;
    // Use this for initialization
    void Start()
    {
        w = new List<WWW>();
        planes = new List<GameObject>();
        index = 0;

        var info = new DirectoryInfo("B:\\Images\\Dossier");
        FileInfo[] fileInfo = info.GetFiles();
        IEnumerable<string> fullNames = fileInfo.Select(file => file.FullName);

        float indexx = 0;
        float indexy = 10;
        foreach (string file in fullNames)
        {
            planes.Add(GameObject.CreatePrimitive(PrimitiveType.Plane));
            planes[index].transform.position = new Vector3(indexx, indexy, 0);
            planes[index].transform.Rotate(new Vector3(90, 0, 0));
            Debug.Log(planes);
            loadTexture(file);
            indexx += 20;
            index++;
            Debug.Log(planes);
        } 
        

    }

    private void resize(GameObject theGameObject, float newSizex, float newSizey)
    {

        float sizex = theGameObject.GetComponent<Renderer>().bounds.size.x;
        float sizey = theGameObject.GetComponent<Renderer>().bounds.size.y;

        Vector3 rescale = theGameObject.transform.localScale;

        rescale.x = newSizex * rescale.x / sizex;
        rescale.y = newSizey * rescale.y / sizey;
        Debug.Log(rescale);

        theGameObject.transform.localScale = rescale;

    }

    private void loadTexture(string filename)
    {
        string fullPath = "file://" + filename;
        w.Add(new WWW(fullPath));
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(w.Count);
        for (int i = 0; i < w.Count; i++)
        {
            if (w[i].isDone)
            {
                planes[i].GetComponent<Renderer>().material.mainTexture = w[i].texture;
            }

        }
    }
}