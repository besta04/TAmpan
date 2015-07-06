using UnityEngine;
using System.Collections;

public class Hills : MonoBehaviour 
{
    public float speed = 1f;
    public float offSetTime = 0f;
    public float maxTime = 3f;
    public GameObject maskObject;

    private float startTime;
    private Material meshMaterial;

	// Use this for initialization
	void Start () 
    {
        GetComponent<Renderer>().enabled = false;
        meshMaterial = GetComponent<Renderer>().material;
        ResetStartTime();
        StartCoroutine(RevealChildAfterSecs(0.7f));
	}

    private void ResetStartTime()
    {
        GetComponent<Renderer>().enabled = true;
        startTime = Time.time + offSetTime;
    }

    private IEnumerator RevealChildAfterSecs(float secs)
    {
        if(maskObject != null)
        {
            maskObject.SetActive(false);
            yield return new WaitForSeconds(secs);
            maskObject.SetActive(true);
            maskObject.GetComponent<Renderer>().enabled = true;
        }
        yield return null;
    }
	
	// Update is called once per frame
	void Update () 
    {
        float delta = (Time.time - startTime) * speed;
	    if(maxTime != 0f)
        {
            delta = Mathf.Min(delta, maxTime);
        }
        meshMaterial.SetFloat("DeltaTime", delta);
	}
}
