using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitLoadingAnim : MonoBehaviour
{
    Text loadingText;

    // Start is called before the first frame update
    void Start()
    {
        loadingText = gameObject.GetComponent<Text>();
        StartCoroutine(loadingTextAnimator());

    }

    IEnumerator loadingTextAnimator() {

        while (true)
        {
            int counter = 1;

            for (int i = 0; i < 4; i++)
            {
                if (counter == 1) {
                    loadingText.text = "Loading";
                }
                else if (counter == 2) {
                    loadingText.text = "Loading.";
                }
                else if (counter == 3) {
                    loadingText.text = "Loading..";
                }
                else if (counter == 4) {
                    loadingText.text = "Loading...";
                }

                counter++;

                yield return new WaitForSeconds(0.75f);
            }
        }
    }
}
