using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaycastIndoorSensor : MonoBehaviour
{
    GameObject targetSensor;

    [Header("- Panel")]
    [SerializeField] GameObject dataGuiPanel;
    [SerializeField] GameObject sensorDataPanel;
    public static string SensorSerial;

    [Header ("- Sensor Data Text")]
    [SerializeField] Text titleText;
    [SerializeField] Text text_1;
    [SerializeField] Text text_2;

    // Start is called before the first frame update
    void Start()
    {
        SensorSerial = null;
        sensorDataPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (dataGuiPanel.activeSelf)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(Camera.main.transform.position, ray.direction * 1000.0f, Color.red, 1.0f);
        LayerMask mask = LayerMask.GetMask("Sensor");
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000.0f, mask))
        {
            if (targetSensor == null)
            {
                targetSensor = hit.transform.gameObject;
                targetSensor.transform.GetComponent<Renderer>().material.color = Color.red;
                sensorDataPanel.SetActive(true);
                titleText.text = targetSensor.transform.GetComponent<SensorInfo>().sensorSerial + " (환경 센서)";
                SensorSerial = targetSensor.transform.GetComponent<SensorInfo>().sensorSerial;
            }

            if (Input.GetMouseButtonDown(0))
            {                
                sensorDataPanel.SetActive(false);
                dataGuiPanel.SetActive(true);
            }

            TemporaryData();
        }
        else
        {
            if (targetSensor != null)
            {
                targetSensor.transform.GetComponent<Renderer>().material.color = Color.green;
                sensorDataPanel.SetActive(false);
                targetSensor = null;
            }
        }
    }

    void TemporaryData()
    {
        switch (targetSensor.transform.GetComponent<SensorInfo>().sensorSerial)
        {
            case "ICL1L2100449":
                text_1.text =
                    $@"- Co2 : {IndoorEnvironmentSensorData.ICL1L2100449.Co2}
- Humi : {IndoorEnvironmentSensorData.ICL1L2100449.Humi}
- Voc : {IndoorEnvironmentSensorData.ICL1L2100449.Voc}
- Pm10 : {IndoorEnvironmentSensorData.ICL1L2100449.Pm10}";
                text_2.text = 
$@"- Pm25 : {IndoorEnvironmentSensorData.ICL1L2100449.Pm25}
- Noise : {IndoorEnvironmentSensorData.ICL1L2100449.Noise}";
                break;
            case "ICL1L2100450":
                text_1.text = 
$@"- Co2 : {IndoorEnvironmentSensorData.ICL1L2100450.Co2}
- Humi : {IndoorEnvironmentSensorData.ICL1L2100450.Humi}
- Voc : {IndoorEnvironmentSensorData.ICL1L2100450.Voc}
- Pm10 : {IndoorEnvironmentSensorData.ICL1L2100450.Pm10}";
                text_2.text = 
$@"- Pm25 : {IndoorEnvironmentSensorData.ICL1L2100450.Pm25}
- Noise : {IndoorEnvironmentSensorData.ICL1L2100450.Noise}";
                break;
            case "ICW0W2001044":
                text_1.text =
$@"- Co2 : {IndoorEnvironmentSensorData.ICW0W2001044.Co2}
- Humi : {IndoorEnvironmentSensorData.ICW0W2001044.Humi}
- Voc : {IndoorEnvironmentSensorData.ICW0W2001044.Voc}
- Pm10 : {IndoorEnvironmentSensorData.ICW0W2001044.Pm10}";
                text_2.text = 
$@"- Pm25 : {IndoorEnvironmentSensorData.ICW0W2001044.Pm25}
- Noise : {IndoorEnvironmentSensorData.ICW0W2001044.Noise}";
                break;
            case "ICL1L2000065":
                text_1.text =
$@"- Co2 : {IndoorEnvironmentSensorData.ICL1L2000065.Co2}
- Humi : {IndoorEnvironmentSensorData.ICL1L2000065.Humi}
- Voc : {IndoorEnvironmentSensorData.ICL1L2000065.Voc}
- Pm10 : {IndoorEnvironmentSensorData.ICL1L2000065.Pm10}";
                text_2.text =
$@"- Pm25 : {IndoorEnvironmentSensorData.ICL1L2000065.Pm25}
- Noise : {IndoorEnvironmentSensorData.ICL1L2000065.Noise}";
                break;
        }
    }
}
