using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaycastVentilatorCleaner : MonoBehaviour
{
    GameObject targetVentilatorCleaner;

    [Header("- Panel")]
    [SerializeField] GameObject dataGuiPanel;
    [SerializeField] GameObject VentilatorCleanerDataPanel;
    public static string VentilatorCleanerSerial;

    [Header("- Sensor Data Text")]
    [SerializeField] Text titleText;
    [SerializeField] Text text_1;
    [SerializeField] Text text_2;

    // Start is called before the first frame update
    void Start()
    {
        VentilatorCleanerSerial = null;
        VentilatorCleanerDataPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(Camera.main.transform.position, ray.direction * 1000.0f, Color.red, 1.0f);
        LayerMask mask = LayerMask.GetMask("VentilatorCleaner ");
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000.0f, mask))
        {
            if (targetVentilatorCleaner == null)
            {
                targetVentilatorCleaner = hit.transform.gameObject;
                targetVentilatorCleaner.transform.GetComponent<Renderer>().material.color = Color.red;
                VentilatorCleanerDataPanel.SetActive(true);
                titleText.text = targetVentilatorCleaner.transform.GetComponent<VentilatorCleanerInfo>().VentilatorCleanerInfoSerial + " (환기청정기)";
                VentilatorCleanerSerial = targetVentilatorCleaner.transform.GetComponent<VentilatorCleanerInfo>().VentilatorCleanerInfoSerial;
            }

            if (Input.GetMouseButtonDown(0))
            {
                VentilatorCleanerDataPanel.SetActive(false);
                dataGuiPanel.SetActive(true);
            }

            TemporaryData();
        }
        else
        {
            if (targetVentilatorCleaner != null)
            {
                targetVentilatorCleaner.transform.GetComponent<Renderer>().material.color = Color.blue;
                VentilatorCleanerDataPanel.SetActive(false);
                targetVentilatorCleaner = null;
            }
        }
    }

    void TemporaryData()
    {
        switch (targetVentilatorCleaner.transform.GetComponent<VentilatorCleanerInfo>().VentilatorCleanerInfoSerial)
        {
            case "KWV-ST1_1900579":
                text_1.text =
$@"- Power : {VentilatorCleanerData.KWV_ST1_1900579.Power}
- Air_volume: {VentilatorCleanerData.KWV_ST1_1900579.Air_volume}
- Exh_mode : {VentilatorCleanerData.KWV_ST1_1900579.Exh_mode}
- Auto_mode : {VentilatorCleanerData.KWV_ST1_1900579.Auto_mode}";
                text_2.text =
$@"- Filter_alarm : {VentilatorCleanerData.KWV_ST1_1900579.Filter_alarm}
- Air_mode : {VentilatorCleanerData.KWV_ST1_1900579.Air_mode}";
                break;
            case "KWV-ST1_1900523":
                text_1.text =
$@"- Power : {VentilatorCleanerData.KWV_ST1_1900523.Power}
- Air_volume: {VentilatorCleanerData.KWV_ST1_1900523.Air_volume}
- Exh_mode : {VentilatorCleanerData.KWV_ST1_1900523.Exh_mode}
- Auto_mode : {VentilatorCleanerData.KWV_ST1_1900523.Auto_mode}";
                text_2.text =
$@"- Filter_alarm : {VentilatorCleanerData.KWV_ST1_1900523.Filter_alarm}
- Air_mode : {VentilatorCleanerData.KWV_ST1_1900523.Air_mode}";
                break;
            case "KWV-ST1_1900521":
                text_1.text =
                text_1.text =
$@"- Power : {VentilatorCleanerData.KWV_ST1_1900521.Power}
- Air_volume: {VentilatorCleanerData.KWV_ST1_1900521.Air_volume}
- Exh_mode : {VentilatorCleanerData.KWV_ST1_1900521.Exh_mode}
- Auto_mode : {VentilatorCleanerData.KWV_ST1_1900521.Auto_mode}";
                text_2.text =
$@"- Filter_alarm : {VentilatorCleanerData.KWV_ST1_1900521.Filter_alarm}
- Air_mode : {VentilatorCleanerData.KWV_ST1_1900521.Air_mode}";
                break;
            case "KWV-ST1_1900524":
                text_1.text =
$@"- Power : {VentilatorCleanerData.KWV_ST1_1900524.Power}
- Air_volume: {VentilatorCleanerData.KWV_ST1_1900524.Air_volume}
- Exh_mode : {VentilatorCleanerData.KWV_ST1_1900524.Exh_mode}
- Auto_mode : {VentilatorCleanerData.KWV_ST1_1900524.Auto_mode}";
                text_2.text =
$@"- Filter_alarm : {VentilatorCleanerData.KWV_ST1_1900524.Filter_alarm}
- Air_mode : {VentilatorCleanerData.KWV_ST1_1900524.Air_mode}";
                break;
        }
    }
}
