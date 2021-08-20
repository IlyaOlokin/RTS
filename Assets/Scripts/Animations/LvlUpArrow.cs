using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LvlUpArrow : MonoBehaviour
{
    private Image image;
    private float _fillAmount = 0;
    private bool filling;
    private bool unfilling;
    
    void Awake()
    {
        image = GetComponent<Image>();
        

    }

    void Update()
    {
        if (filling)
        {
            image.fillAmount += Time.deltaTime * 2f;
        }
        else if (unfilling)
        {
            image.fillAmount -= Time.deltaTime * 2f;
        }
    }

    public void StartAnim()
    {
        StartCoroutine(Animation());
    }


    public IEnumerator Animation()
    {
        image.fillOrigin = 0;
        image.fillAmount = 0;
        unfilling = false;
        filling = true;
        yield return new WaitForSeconds(2);
        filling = false;
        image.fillOrigin = 1;
        image.fillAmount = 1;
        unfilling = true;
        yield return new WaitForSeconds(2);
        unfilling = false;
        gameObject.SetActive(false);
    }
}
