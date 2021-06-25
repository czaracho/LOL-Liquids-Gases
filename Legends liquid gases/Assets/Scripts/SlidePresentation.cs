using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidePresentation : MonoBehaviour
{
    public TypeWriter TypeWriter;
    public GameObject[] infoSlides;
    public string firstText = "first";

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(waitforReadFirstTime());
    }

    
    IEnumerator waitforReadFirstTime() {
        yield return new WaitForSeconds(2.0f);
        TypeWriter.WriteTextGameSpace(firstText);

    }
}
