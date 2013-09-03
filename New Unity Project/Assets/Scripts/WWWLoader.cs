using UnityEngine;
using System.Collections;

public class WWWLoader : MonoBehaviour {

    private static WWWLoader _instance = null;

    public static WWWLoader instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject("_WWWLoader");
                _instance = (WWWLoader)obj.AddComponent(typeof(WWWLoader));
            }
            return _instance;
        }
    }


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public static bool _ready = false;

    public static bool Ready
    {
        get
        {
            if (_ready)
            {
                return true;
                _ready = false;
            }
            return false;
        }
    }

    public delegate void MyDelegate(string s);

    public MyDelegate myDelegate;

    public static string Response = "";

    public static void Load(string url, WWWForm form)
    {
        WWW www = new WWW(url, form);
        instance.StartCoroutine(WWWLoader.instance.WaitForLoad(www));
    }

    public IEnumerator WaitForLoad(WWW www)
    {
        yield return www;

        if (www.error != null)
        {
            Debug.Log("WWWLoader Error - " + www.error);
        }
        else
        {
            if (myDelegate != null)
            {
                myDelegate(www.text);
            }
        }
    }
}
