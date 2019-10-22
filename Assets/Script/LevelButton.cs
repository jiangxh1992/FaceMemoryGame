using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour {

    public int level = 0;
	// Use this for initialization
	void Start () {
        GetComponent<Button>().onClick.AddListener(Click);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void Click()
    {
        SceneManager.LoadSceneAsync(string.Format("level{0}",level));
    }
}
