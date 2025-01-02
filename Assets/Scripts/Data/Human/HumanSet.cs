using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HumanSet : MonoBehaviour
{
    public string id;
    [SerializeField] Material lineMat;
    [SerializeField] Text text;
    [SerializeField] Renderer[] myAllRenderer;
    [SerializeField] Renderer humanRenderer;
    
    TrailRenderer tr;
    
    Color tempColor;

    public void OnEnable()
    {
        Initialize();
        LineStateFumc(true);
    }

    public void OnDisable()
    {
        LineStateFumc(false);
    }

    public void LineStateFumc(bool state)
    {
        if (state)
        {
            tr = this.transform.GetChild(0).gameObject.GetOrAddComponent<TrailRenderer>();
            tr.time = 2.5f;
            tr.startWidth = 0.5f;
            tr.endWidth = 0.0f;
            tr.material = lineMat;
        }
        else
        {
            Destroy(this.transform.GetChild(0).GetComponent<TrailRenderer>());
        }
    }    
    
    public void Initialize()
    {        
        
        foreach (var child_ in myAllRenderer)
        {
            child_.material.SetFloat("_Mode", 3);
            Material[] mats = child_.sharedMaterials;
        
            foreach (var child_mats in mats)
                if (child_mats != null)
                    child_mats.color = Color.black;
        }

    }

    float newDistance;
    public string DistanceStr;
    public void Update()
    {
        ModelColorFunc();
    }

     void ModelColorFunc()
    {
        newDistance = Int32.MaxValue;

        Collider[] colls_1 = Physics.OverlapSphere(transform.position, 10);
        Collider[] colls_2 = Physics.OverlapSphere(transform.position, 20);
        Collider[] colls_3 = Physics.OverlapSphere(transform.position, 30);

        List<string> exceptMyselfColls_1 = new List<string>();
        List<string> exceptMySelfColls_2 = new List<string>();
        List<string> exceptMySelfColls_3 = new List<string>();

        foreach (var child in colls_1)
            if (this.transform.gameObject != child.transform.gameObject)
            {
                if (child.transform.GetComponent<HumanSet>().enabled == true)
                {
                    exceptMyselfColls_1.Add(child.transform.GetComponent<HumanSet>().id);

                    MostShortDistance(child, 10);
                }
            }
        foreach (var child in colls_2)
            if (this.transform.gameObject != child.transform.gameObject)
            {
                if (child.transform.GetComponent<HumanSet>().enabled == true)
                {
                    exceptMySelfColls_2.Add(child.transform.GetComponent<HumanSet>().id);

                    MostShortDistance(child, 20);
                }
            }
        foreach (var child in colls_3)
            if (this.transform.gameObject != child.transform.gameObject)
            {
                if (child.transform.GetComponent<HumanSet>().enabled == true)
                {
                    exceptMySelfColls_3.Add(child.transform.GetComponent<HumanSet>().id);

                    MostShortDistance(child, 30);
                }
            }

        if (exceptMyselfColls_1.Count > 0)
        {
            foreach (var child_ in myAllRenderer)
            {
                tempColor = child_.material.color;
                StartCoroutine("CoModelsColorChange", (int)ColorsState.red);
                child_.material.color = tempColor;
            }
        }
        else if (exceptMySelfColls_2.Count > 0 && exceptMyselfColls_1.Count <= 0)
        {        
           foreach (var child_ in myAllRenderer)
           {
               tempColor = child_.material.color;
               StartCoroutine("CoModelsColorChange", (int)ColorsState.blue);
               child_.material.color = tempColor;
           }
        
        }
        else if (exceptMySelfColls_3.Count > 0 && exceptMySelfColls_2.Count <= 0 && exceptMyselfColls_1.Count <= 0)
        {
            foreach (var child_ in myAllRenderer)
            {
                tempColor = child_.material.color;
                StartCoroutine("CoModelsColorChange", (int)ColorsState.green);
                child_.material.color = tempColor;
            }
        }
        else
        {
            foreach (var child_ in myAllRenderer)
            {
                tempColor = child_.material.color;
                StartCoroutine("CoModelsColorChange", (int)ColorsState.black);
                child_.material.color = tempColor;

                DistanceStr = "--";
            }
        }
    }

    void MostShortDistance(Collider col, int distanceScoop)
    {
        float distance = (col.transform.position - this.transform.position).magnitude;
        if (newDistance > distance)
            newDistance = distance;

        if (newDistance <= distanceScoop)
            DistanceStr = string.Format("{0:0.0#}", (newDistance / 10));
        else
            DistanceStr = "";
    }

    enum ColorsState
    {
        black = 0,
        red = 1,
        green = 2,
        blue = 3
    }

    IEnumerator CoModelsColorChange(int state)
    {
        switch (state)
        {
            case (int)ColorsState.black:
                {
                    for (int i = 0; i < 20; i++)
                    {
                        if (tempColor.r <= 0)
                            tempColor.r = 0;
                        else
                            tempColor.r -= 0.05f;

                        if (tempColor.g <= 0)
                            tempColor.g = 0;
                        else
                            tempColor.g -= 0.05f;

                        if (tempColor.b <= 0)
                            tempColor.b = 0;
                        else
                            tempColor.b -= 0.05f;

                        yield return new WaitForSeconds(0.01f);
                    }
                }
                break;
            case (int)ColorsState.red:
                {
                    for (int i = 0; i < 20; i++)
                    {
                        if (tempColor.r >= 1)
                            tempColor.r = 1;
                        else
                            tempColor.r += 0.05f;

                        if (tempColor.g <= 0)
                            tempColor.g = 0;
                        else
                            tempColor.g -= 0.05f;

                        if (tempColor.b <= 0)
                            tempColor.b = 0;
                        else
                            tempColor.b -= 0.05f;

                        yield return new WaitForSeconds(0.01f);
                    }
                }
                break;
            case (int)ColorsState.green:
                {
                    for (int i = 0; i < 20; i++)
                    {
                        if (tempColor.r <= 0)
                            tempColor.r = 0;
                        else
                            tempColor.r -= 0.05f;

                        if (tempColor.g >= 1)
                            tempColor.g = 1;
                        else
                            tempColor.g += 0.05f;

                        if (tempColor.b <= 0)
                            tempColor.b = 0;
                        else
                            tempColor.b -= 0.05f;

                        yield return new WaitForSeconds(0.01f);
                    }
                }
                break;
            case (int)ColorsState.blue:
                {
                    for (int i = 0; i < 20; i++)
                    {
                        if (tempColor.r <= 0)
                            tempColor.r = 0;
                        else
                            tempColor.r -= 0.05f;

                        if (tempColor.g <= 0)
                            tempColor.g = 0;
                        else
                            tempColor.g -= 0.05f;

                        if (tempColor.b >= 1)
                            tempColor.b = 1;
                        else
                            tempColor.b += 0.05f;

                        yield return new WaitForSeconds(0.01f);
                    }
                }
                break;
        }
    }
}

