using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class IndoorEnvironmentSensorData : MonoBehaviour
{
    string url;
    string jsonText;

    // 실외 환경센서 데이터
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("GetRequest");
        StartCoroutine("CoGetJsonData");
    }

    IEnumerator GetRequest()
    {
        while (true)
        {   
            url = "http://keti1.energyiotlab.com:10111/v1/sensors/kw-iske1";
            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                // Request and wait for the desired page.
                yield return webRequest.SendWebRequest();

                string[] pages = url.Split('/');
                int page = pages.Length - 1;

                switch (webRequest.result)
                {
                    case UnityWebRequest.Result.ConnectionError:
                    case UnityWebRequest.Result.DataProcessingError:
                        //UnityEngine.Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                        break;
                    case UnityWebRequest.Result.ProtocolError:
                        //UnityEngine.Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                        break;
                    case UnityWebRequest.Result.Success:
                        //Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                        jsonText = webRequest.downloadHandler.text;
                        break;
                }
            }
        }
    }

    struct Data
    {
        public string Key;
        public string Timestamp;
        public string Datetime;
        public string Tm;
        public string Pm01_raw;
        public string Pm25_raw;
        public string Pm10_raw;
        public string Temp;
        public string Humi;
        public string Co2;
        public string Voc;
        public string Noise;
        public string Pm10;
        public string Pm25;
        public string Pm01;
        public string Cici_pm10;
        public string Cici_pm25;
        public string Cici_co2;
        public string Cici_voc;
        public string Ciai;
        public string Cici_temp;
        public string Cici_humi;
        public string Cici_noise;
        public string Cici;
    }

    JObject jObject;
    Data[] data;
    Queue<Data[]> dataQueue = new Queue<Data[]>();
    IEnumerator CoGetJsonData()
    {
        while (true)
        {
            if (jsonText != null)
            {
                jObject = JObject.Parse(jsonText);

                List<string> keyList = new List<string>();
                List<string> timestampList = new List<string>();
                List<string> datetimeList = new List<string>();
                List<string> tmList = new List<string>();
                List<string> pm01_rawList = new List<string>();
                List<string> pm25_rawList = new List<string>();
                List<string> pm10_rawList = new List<string>();
                List<string> tempList = new List<string>();
                List<string> humiList = new List<string>();
                List<string> co2List = new List<string>();
                List<string> vocList = new List<string>();
                List<string> noiseList = new List<string>();
                List<string> pm10List = new List<string>();
                List<string> pm25List = new List<string>();
                List<string> pm01List = new List<string>();
                List<string> cici_pm10List = new List<string>();
                List<string> cici_pm25List = new List<string>();
                List<string> cici_co2List = new List<string>();
                List<string> cici_vocList = new List<string>();
                List<string> ciaiList = new List<string>();
                List<string> cici_tempList = new List<string>();
                List<string> cici_humiList = new List<string>();
                List<string> cici_noiseList = new List<string>();
                List<string> ciciList = new List<string>();

                foreach (var childF in jObject)
                {
                    var key = childF.Key;
                    //Debug.Log($"Key: {childF.Key}");
                    keyList.Add(key);

                    foreach (var childS in childF.Value)
                    {
                        if (childS.ToString().Contains("service"))
                        {
                            var timestamp = childS.First["timestamp"].ToString();
                            timestampList.Add(timestamp);

                            var datetime = childS.First["datetime"].ToString();
                            datetimeList.Add(datetime);
                        }

                        if (childS.ToString().Contains("data"))
                        {
                            if (childS.First.ToString().Contains("\"tm\":"))
                            {
                                var tm = childS.First["tm"].ToString();
                                tmList.Add(tm);
                            }
                            else
                            {
                                tmList.Add(null);
                            }

                            if (childS.First.ToString().Contains("\"pm01_raw\":"))
                            {
                                var pm01_raw = childS.First["pm01_raw"].ToString();
                                pm01_rawList.Add(pm01_raw);
                            }
                            else
                            {
                                pm01_rawList.Add(null);
                            }

                            if (childS.First.ToString().Contains("\"pm25_raw\":"))
                            {
                                var pm25_raw = childS.First["pm25_raw"].ToString();
                                pm25_rawList.Add(pm25_raw);
                            }
                            else
                            {
                                pm25_rawList.Add(null);
                            }

                            if (childS.First.ToString().Contains("\"pm10_raw\":"))
                            {
                                var pm10_raw = childS.First["pm10_raw"].ToString();
                                pm10_rawList.Add(pm10_raw);
                            }
                            else
                            {
                                pm10_rawList.Add(null);
                            }

                            if (childS.First.ToString().Contains("\"temp\":"))
                            {
                                var temp = childS.First["temp"].ToString();
                                tempList.Add(temp);
                            }
                            else
                            {
                                tempList.Add(null);
                            }

                            if (childS.First.ToString().Contains("\"humi\":"))
                            {
                                var humi = childS.First["humi"].ToString();
                                humiList.Add(humi);
                            }
                            else
                            {
                                humiList.Add(null);
                            }

                            if (childS.First.ToString().Contains("\"co2\":"))
                            {
                                var co2 = childS.First["co2"].ToString();
                                co2List.Add(co2);
                            }
                            else
                            {
                                co2List.Add(null);
                            }

                            if (childS.First.ToString().Contains("\"voc\":"))
                            {
                                var voc = childS.First["voc"].ToString();
                                vocList.Add(voc);
                            }
                            else
                            {
                                vocList.Add(null);
                            }

                            if (childS.First.ToString().Contains("\"noise\":"))
                            {
                                var noise = childS.First["noise"].ToString();
                                noiseList.Add(noise);
                            }
                            else
                            {
                                noiseList.Add(null);
                            }

                            if (childS.First.ToString().Contains("\"pm10\":"))
                            {
                                var pm10 = childS.First["pm10"].ToString();
                                pm10List.Add(pm10);
                            }
                            else
                            {
                                pm10List.Add(null);
                            }

                            if (childS.First.ToString().Contains("\"pm25\":"))
                            {
                                var pm25 = childS.First["pm25"].ToString();
                                pm25List.Add(pm25);
                            }
                            else
                            {
                                pm25List.Add(null);
                            }

                            if (childS.First.ToString().Contains("\"pm01\":"))
                            {
                                var pm01 = childS.First["pm01"].ToString();
                                pm01List.Add(pm01);
                            }
                            else
                            {
                                pm01List.Add(null);
                            }

                            if (childS.First.ToString().Contains("\"cici_pm10\":"))
                            {
                                var cici_pm10 = childS.First["cici_pm10"].ToString();
                                cici_pm10List.Add(cici_pm10);
                            }
                            else
                            {
                                cici_pm10List.Add(null);
                            }

                            if (childS.First.ToString().Contains("\"cici_pm25\":"))
                            {
                                var cici_pm25 = childS.First["cici_pm25"].ToString();
                                cici_pm25List.Add(cici_pm25);
                            }
                            else
                            {
                                cici_pm25List.Add(null);
                            }

                            if (childS.First.ToString().Contains("\"cici_co2\":"))
                            {
                                var cici_co2 = childS.First["cici_co2"].ToString();
                                cici_co2List.Add(cici_co2);
                            }
                            else
                            {
                                cici_co2List.Add(null);
                            }

                            if (childS.First.ToString().Contains("\"cici_voc\":"))
                            {
                                var cici_voc = childS.First["cici_voc"].ToString();
                                cici_vocList.Add(cici_voc);
                            }
                            else
                            {
                                cici_vocList.Add(null);
                            }

                            if (childS.First.ToString().Contains("\"ciai\":"))
                            {
                                var ciai = childS.First["ciai"].ToString();
                                ciaiList.Add(ciai);
                            }
                            else
                            {
                                ciaiList.Add(null);
                            }

                            if (childS.First.ToString().Contains("\"cici_temp\":"))
                            {
                                var cici_temp = childS.First["cici_temp"].ToString();
                                cici_tempList.Add(cici_temp);
                            }
                            else
                            {
                                cici_tempList.Add(null);
                            }

                            if (childS.First.ToString().Contains("\"cici_humi\":"))
                            {
                                var cici_humi = childS.First["cici_humi"].ToString();
                                cici_humiList.Add(cici_humi);
                            }
                            else
                            {
                                cici_humiList.Add(null);
                            }

                            if (childS.First.ToString().Contains("\"cici_noise\":"))
                            {
                                var cici_noise = childS.First["cici_noise"].ToString();
                                cici_noiseList.Add(cici_noise);
                            }
                            else
                            {
                                cici_noiseList.Add(null);
                            }

                            if (childS.First.ToString().Contains("\"cici\":"))
                            {
                                var cici = childS.First["cici"].ToString();
                                ciciList.Add(cici);
                            }
                            else
                            {
                                ciciList.Add(null);
                            }
                        }
                    }

                    data = new Data[keyList.Count];
                    for (int i = 0; i < keyList.Count; i++)
                    {
                        data[i].Key = keyList[i];
                        data[i].Timestamp = timestampList[i];
                        data[i].Datetime = datetimeList[i];
                        data[i].Tm = tmList[i];
                        data[i].Pm01_raw = pm01_rawList[i];
                        data[i].Pm25_raw = pm25_rawList[i];
                        data[i].Pm10_raw = pm10_rawList[i];
                        data[i].Temp = tempList[i];
                        data[i].Humi = humiList[i];
                        data[i].Co2 = co2List[i];
                        data[i].Voc = vocList[i];
                        data[i].Noise = noiseList[i];
                        data[i].Pm10 = pm10List[i];
                        data[i].Pm25 = pm25List[i];
                        data[i].Pm01 = pm01List[i];
                        data[i].Cici_pm10 = cici_pm10List[i];
                        data[i].Cici_pm25 = cici_pm25List[i];
                        data[i].Cici_co2 = cici_co2List[i];
                        data[i].Cici_voc = cici_vocList[i];
                        data[i].Ciai = ciaiList[i];
                        data[i].Cici_temp = cici_tempList[i];
                        data[i].Cici_humi = cici_humiList[i];
                        data[i].Cici_noise = cici_noiseList[i];
                        data[i].Cici = ciciList[i];
                    }
                }
                dataQueue.Enqueue(data);
                UpdateSensorData();
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void UpdateSensorData()
    {
        Data[] _data = dataQueue.Dequeue();

        foreach (var child in _data)
        {
            // 동아 유치원 2층 라온반
            if (child.Key == "ICW0W2001040")
            {
                ICW0W2001040.Key = key_040 = child.Key;
                ICW0W2001040.Timestamp = timestamp_040 = child.Timestamp;
                ICW0W2001040.Datetime = datetime_040 = child.Datetime;
                ICW0W2001040.Tm = tm_040 = child.Tm;
                ICW0W2001040.Pm01_raw = pm01_raw_040 = child.Pm01_raw;
                ICW0W2001040.Pm25_raw = pm25_raw_040 = child.Pm25_raw;
                ICW0W2001040.Pm10_raw = pm10_raw_040 = child.Pm10_raw;
                ICW0W2001040.Temp = temp_040 = child.Temp;
                ICW0W2001040.Humi = humi_040 = child.Humi;
                ICW0W2001040.Co2 = co2_040 = child.Co2;
                ICW0W2001040.Voc = voc_040 = child.Voc;
                ICW0W2001040.Noise = noise_040 = child.Noise;
                ICW0W2001040.Pm10 = pm10_040 = child.Pm10;
                ICW0W2001040.Pm25 = pm25_040 = child.Pm25;
                ICW0W2001040.Pm01 = pm01_040 = child.Pm01;
                ICW0W2001040.Cici_pm10 = cici_pm10_040 = child.Cici_pm10;
                ICW0W2001040.Cici_pm25 = cici_pm25_040 = child.Cici_pm25;
                ICW0W2001040.Cici_co2 = cici_co2_040 = child.Cici_co2;
                ICW0W2001040.Cici_voc = cici_voc_040 = child.Cici_voc;
                ICW0W2001040.Ciai = ciai_040 = child.Ciai;
                ICW0W2001040.Cici_temp = cici_temp_040 = child.Cici_temp;
                ICW0W2001040.Cici_humi = cici_humi_040 = child.Cici_humi;
                ICW0W2001040.Cici_noise = cici_noise_040 = child.Cici_noise;
                ICW0W2001040.Cici = cici_040 = child.Cici;
            }
            // 동아 유치원 2층 나래반
            else if (child.Key == "ICW0W2001039")
            {
                ICW0W2001039.Key = key_039 = child.Key;
                ICW0W2001039.Timestamp = timestamp_039 = child.Timestamp;
                ICW0W2001039.Datetime = datetime_039 = child.Datetime;
                ICW0W2001039.Tm = tm_039 = child.Tm;
                ICW0W2001039.Pm01_raw = pm01_raw_039 = child.Pm01_raw;
                ICW0W2001039.Pm25_raw = pm25_raw_039 = child.Pm25_raw;
                ICW0W2001039.Pm10_raw = pm10_raw_039 = child.Pm10_raw;
                ICW0W2001039.Temp = temp_039 = child.Temp;
                ICW0W2001039.Humi = humi_039 = child.Humi;
                ICW0W2001039.Co2 = co2_039 = child.Co2;
                ICW0W2001039.Voc = voc_039 = child.Voc;
                ICW0W2001039.Noise = noise_039 = child.Noise;
                ICW0W2001039.Pm10 = pm10_039 = child.Pm10;
                ICW0W2001039.Pm25 = pm25_039 = child.Pm25;
                ICW0W2001039.Pm01 = pm01_039 = child.Pm01;
                ICW0W2001039.Cici_pm10 = cici_pm10_039 = child.Cici_pm10;
                ICW0W2001039.Cici_pm25 = cici_pm25_039 = child.Cici_pm25;
                ICW0W2001039.Cici_co2 = cici_co2_039 = child.Cici_co2;
                ICW0W2001039.Cici_voc = cici_voc_039 = child.Cici_voc;
                ICW0W2001039.Ciai = ciai_039 = child.Ciai;
                ICW0W2001039.Cici_temp = cici_temp_039 = child.Cici_temp;
                ICW0W2001039.Cici_humi = cici_humi_039 = child.Cici_humi;
                ICW0W2001039.Cici_noise = cici_noise_039 = child.Cici_noise;
                ICW0W2001039.Cici = cici_039 = child.Cici;
            }
            // 동아 유치원 2층 가온반
            else if (child.Key == "ICW0W2001038")
            {
                ICW0W2001038.Key = key_038 = child.Key;
                ICW0W2001038.Timestamp = timestamp_038 = child.Timestamp;
                ICW0W2001038.Datetime = datetime_038 = child.Datetime;
                ICW0W2001038.Tm = tm_038 = child.Tm;
                ICW0W2001038.Pm01_raw = pm01_raw_038 = child.Pm01_raw;
                ICW0W2001038.Pm25_raw = pm25_raw_038 = child.Pm25_raw;
                ICW0W2001038.Pm10_raw = pm10_raw_038 = child.Pm10_raw;
                ICW0W2001038.Temp = temp_038 = child.Temp;
                ICW0W2001038.Humi = humi_038 = child.Humi;
                ICW0W2001038.Co2 = co2_038 = child.Co2;
                ICW0W2001038.Voc = voc_038 = child.Voc;
                ICW0W2001038.Noise = noise_038 = child.Noise;
                ICW0W2001038.Pm10 = pm10_038 = child.Pm10;
                ICW0W2001038.Pm25 = pm25_038 = child.Pm25;
                ICW0W2001038.Pm01 = pm01_038 = child.Pm01;
                ICW0W2001038.Cici_pm10 = cici_pm10_038 = child.Cici_pm10;
                ICW0W2001038.Cici_pm25 = cici_pm25_038 = child.Cici_pm25;
                ICW0W2001038.Cici_co2 = cici_co2_038 = child.Cici_co2;
                ICW0W2001038.Cici_voc = cici_voc_038 = child.Cici_voc;
                ICW0W2001038.Ciai = ciai_038 = child.Ciai;
                ICW0W2001038.Cici_temp = cici_temp_038 = child.Cici_temp;
                ICW0W2001038.Cici_humi = cici_humi_038 = child.Cici_humi;
                ICW0W2001038.Cici_noise = cici_noise_038 = child.Cici_noise;
                ICW0W2001038.Cici = cici_038 = child.Cici;
            }
            // 동아 유치원 지하 강당 벽면
            else if (child.Key == "ICW0W2001036")
            {
                ICW0W2001036.Key = key_036 = child.Key;
                ICW0W2001036.Timestamp = timestamp_036 = child.Timestamp;
                ICW0W2001036.Datetime = datetime_036 = child.Datetime;
                ICW0W2001036.Tm = tm_036 = child.Tm;
                ICW0W2001036.Pm01_raw = pm01_raw_036 = child.Pm01_raw;
                ICW0W2001036.Pm25_raw = pm25_raw_036 = child.Pm25_raw;
                ICW0W2001036.Pm10_raw = pm10_raw_036 = child.Pm10_raw;
                ICW0W2001036.Temp = temp_036 = child.Temp;
                ICW0W2001036.Humi = humi_036 = child.Humi;
                ICW0W2001036.Co2 = co2_036 = child.Co2;
                ICW0W2001036.Voc = voc_036 = child.Voc;
                ICW0W2001036.Noise = noise_036 = child.Noise;
                ICW0W2001036.Pm10 = pm10_036 = child.Pm10;
                ICW0W2001036.Pm25 = pm25_036 = child.Pm25;
                ICW0W2001036.Pm01 = pm01_036 = child.Pm01;
                ICW0W2001036.Cici_pm10 = cici_pm10_036 = child.Cici_pm10;
                ICW0W2001036.Cici_pm25 = cici_pm25_036 = child.Cici_pm25;
                ICW0W2001036.Cici_co2 = cici_co2_036 = child.Cici_co2;
                ICW0W2001036.Cici_voc = cici_voc_036 = child.Cici_voc;
                ICW0W2001036.Ciai = ciai_036 = child.Ciai;
                ICW0W2001036.Cici_temp = cici_temp_036 = child.Cici_temp;
                ICW0W2001036.Cici_humi = cici_humi_036 = child.Cici_humi;
                ICW0W2001036.Cici_noise = cici_noise_036 = child.Cici_noise;
                ICW0W2001036.Cici = cici_036 = child.Cici;
            }
            // 동아 유치원 지하 강당 놀이터
            else if (child.Key == "ICW0W2001037")
            {
                ICW0W2001037.Key = key_037 = child.Key;
                ICW0W2001037.Timestamp = timestamp_037 = child.Timestamp;
                ICW0W2001037.Datetime = datetime_037 = child.Datetime;
                ICW0W2001037.Tm = tm_037 = child.Tm;
                ICW0W2001037.Pm01_raw = pm01_raw_037 = child.Pm01_raw;
                ICW0W2001037.Pm25_raw = pm25_raw_037 = child.Pm25_raw;
                ICW0W2001037.Pm10_raw = pm10_raw_037 = child.Pm10_raw;
                ICW0W2001037.Temp = temp_037 = child.Temp;
                ICW0W2001037.Humi = humi_037 = child.Humi;
                ICW0W2001037.Co2 = co2_037 = child.Co2;
                ICW0W2001037.Voc = voc_037 = child.Voc;
                ICW0W2001037.Noise = noise_037 = child.Noise;
                ICW0W2001037.Pm10 = pm10_037 = child.Pm10;
                ICW0W2001037.Pm25 = pm25_037 = child.Pm25;
                ICW0W2001037.Pm01 = pm01_037 = child.Pm01;
                ICW0W2001037.Cici_pm10 = cici_pm10_037 = child.Cici_pm10;
                ICW0W2001037.Cici_pm25 = cici_pm25_037 = child.Cici_pm25;
                ICW0W2001037.Cici_co2 = cici_co2_037 = child.Cici_co2;
                ICW0W2001037.Cici_voc = cici_voc_037 = child.Cici_voc;
                ICW0W2001037.Ciai = ciai_037 = child.Ciai;
                ICW0W2001037.Cici_temp = cici_temp_037 = child.Cici_temp;
                ICW0W2001037.Cici_humi = cici_humi_037 = child.Cici_humi;
                ICW0W2001037.Cici_noise = cici_noise_037 = child.Cici_noise;
                ICW0W2001037.Cici = cici_037 = child.Cici;
            }



            // 신도림 초등학교 체육관 강연대
            else if (child.Key == "ICL1L2000064")
            {
                ICL1L2000064.Key = key_064 = child.Key;
                ICL1L2000064.Timestamp = timestamp_064 = child.Timestamp;
                ICL1L2000064.Datetime = datetime_064 = child.Datetime;
                ICL1L2000064.Tm = tm_064 = child.Tm;
                ICL1L2000064.Pm01_raw = pm01_raw_064 = child.Pm01_raw;
                ICL1L2000064.Pm25_raw = pm25_raw_064 = child.Pm25_raw;
                ICL1L2000064.Pm10_raw = pm10_raw_064 = child.Pm10_raw;
                ICL1L2000064.Temp = temp_064 = child.Temp;
                ICL1L2000064.Humi = humi_064 = child.Humi;
                ICL1L2000064.Co2 = co2_064 = child.Co2;
                ICL1L2000064.Voc = voc_064 = child.Voc;
                ICL1L2000064.Noise = noise_064 = child.Noise;
                ICL1L2000064.Pm10 = pm10_064 = child.Pm10;
                ICL1L2000064.Pm25 = pm25_064 = child.Pm25;
                ICL1L2000064.Pm01 = pm01_064 = child.Pm01;
                ICL1L2000064.Cici_pm10 = cici_pm10_064 = child.Cici_pm10;
                ICL1L2000064.Cici_pm25 = cici_pm25_064 = child.Cici_pm25;
                ICL1L2000064.Cici_co2 = cici_co2_064 = child.Cici_co2;
                ICL1L2000064.Cici_voc = cici_voc_064 = child.Cici_voc;
                ICL1L2000064.Ciai = ciai_064 = child.Ciai;
                ICL1L2000064.Cici_temp = cici_temp_064 = child.Cici_temp;
                ICL1L2000064.Cici_humi = cici_humi_064 = child.Cici_humi;
                ICL1L2000064.Cici_noise = cici_noise_064 = child.Cici_noise;
                ICL1L2000064.Cici = cici_064 = child.Cici;
            }
            // 신도림 초등학교 뒷면 입구 쪽(교체)
            else if (child.Key == "ISCKL2200001")
            {
                ISCKL2200001.Key = key_001 = child.Key;
                ISCKL2200001.Timestamp = timestamp_001 = child.Timestamp;
                ISCKL2200001.Datetime = datetime_001 = child.Datetime;
                ISCKL2200001.Tm = tm_001 = child.Tm;
                ISCKL2200001.Pm01_raw = pm01_raw_001 = child.Pm01_raw;
                ISCKL2200001.Pm25_raw = pm25_raw_001 = child.Pm25_raw;
                ISCKL2200001.Pm10_raw = pm10_raw_001 = child.Pm10_raw;
                ISCKL2200001.Temp = temp_001 = child.Temp;
                ISCKL2200001.Humi = humi_001 = child.Humi;
                ISCKL2200001.Co2 = co2_001 = child.Co2;
                ISCKL2200001.Voc = voc_001 = child.Voc;
                ISCKL2200001.Noise = noise_001 = child.Noise;
                ISCKL2200001.Pm10 = pm10_001 = child.Pm10;
                ISCKL2200001.Pm25 = pm25_001 = child.Pm25;
                ISCKL2200001.Pm01 = pm01_001 = child.Pm01;
                ISCKL2200001.Cici_pm10 = cici_pm10_001 = child.Cici_pm10;
                ISCKL2200001.Cici_pm25 = cici_pm25_001 = child.Cici_pm25;
                ISCKL2200001.Cici_co2 = cici_co2_001 = child.Cici_co2;
                ISCKL2200001.Cici_voc = cici_voc_001 = child.Cici_voc;
                ISCKL2200001.Ciai = ciai_001 = child.Ciai;
                ISCKL2200001.Cici_temp = cici_temp_001 = child.Cici_temp;
                ISCKL2200001.Cici_humi = cici_humi_001 = child.Cici_humi;
                ISCKL2200001.Cici_noise = cici_noise_001 = child.Cici_noise;
                ISCKL2200001.Cici = cici_001 = child.Cici;
            }
            // 신도림 초등학교 운동장 입구 쪽
            else if (child.Key == "ISC0W2000003")
            {
                ISC0W2000003.Key = key_003 = child.Key;
                ISC0W2000003.Timestamp = timestamp_003 = child.Timestamp;
                ISC0W2000003.Datetime = datetime_003 = child.Datetime;
                ISC0W2000003.Tm = tm_003 = child.Tm;
                ISC0W2000003.Pm01_raw = pm01_raw_003 = child.Pm01_raw;
                ISC0W2000003.Pm25_raw = pm25_raw_003 = child.Pm25_raw;
                ISC0W2000003.Pm10_raw = pm10_raw_003 = child.Pm10_raw;
                ISC0W2000003.Temp = temp_003 = child.Temp;
                ISC0W2000003.Humi = humi_003 = child.Humi;
                ISC0W2000003.Co2 = co2_003 = child.Co2;
                ISC0W2000003.Voc = voc_003 = child.Voc;
                ISC0W2000003.Noise = noise_003 = child.Noise;
                ISC0W2000003.Pm10 = pm10_003 = child.Pm10;
                ISC0W2000003.Pm25 = pm25_003 = child.Pm25;
                ISC0W2000003.Pm01 = pm01_003 = child.Pm01;
                ISC0W2000003.Cici_pm10 = cici_pm10_003 = child.Cici_pm10;
                ISC0W2000003.Cici_pm25 = cici_pm25_003 = child.Cici_pm25;
                ISC0W2000003.Cici_co2 = cici_co2_003 = child.Cici_co2;
                ISC0W2000003.Cici_voc = cici_voc_003 = child.Cici_voc;
                ISC0W2000003.Ciai = ciai_003 = child.Ciai;
                ISC0W2000003.Cici_temp = cici_temp_003 = child.Cici_temp;
                ISC0W2000003.Cici_humi = cici_humi_003 = child.Cici_humi;
                ISC0W2000003.Cici_noise = cici_noise_003 = child.Cici_noise;
                ISC0W2000003.Cici = cici_003 = child.Cici;
            }
            // 신도림 초등학교 운동장 강연대 쪽
            else if (child.Key == "ISC0W2000002")
            {
                ISC0W2000002.Key = key_002 = child.Key;
                ISC0W2000002.Timestamp = timestamp_002 = child.Timestamp;
                ISC0W2000002.Datetime = datetime_002 = child.Datetime;
                ISC0W2000002.Tm = tm_002 = child.Tm;
                ISC0W2000002.Pm01_raw = pm01_raw_002 = child.Pm01_raw;
                ISC0W2000002.Pm25_raw = pm25_raw_002 = child.Pm25_raw;
                ISC0W2000002.Pm10_raw = pm10_raw_002 = child.Pm10_raw;
                ISC0W2000002.Temp = temp_002 = child.Temp;
                ISC0W2000002.Humi = humi_002 = child.Humi;
                ISC0W2000002.Co2 = co2_002 = child.Co2;
                ISC0W2000002.Voc = voc_002 = child.Voc;
                ISC0W2000002.Noise = noise_002 = child.Noise;
                ISC0W2000002.Pm10 = pm10_002 = child.Pm10;
                ISC0W2000002.Pm25 = pm25_002 = child.Pm25;
                ISC0W2000002.Pm01 = pm01_002 = child.Pm01;
                ISC0W2000002.Cici_pm10 = cici_pm10_002 = child.Cici_pm10;
                ISC0W2000002.Cici_pm25 = cici_pm25_002 = child.Cici_pm25;
                ISC0W2000002.Cici_co2 = cici_co2_002 = child.Cici_co2;
                ISC0W2000002.Cici_voc = cici_voc_002 = child.Cici_voc;
                ISC0W2000002.Ciai = ciai_002 = child.Ciai;
                ISC0W2000002.Cici_temp = cici_temp_002 = child.Cici_temp;
                ISC0W2000002.Cici_humi = cici_humi_002 = child.Cici_humi;
                ISC0W2000002.Cici_noise = cici_noise_002 = child.Cici_noise;
                ISC0W2000002.Cici = cici_002 = child.Cici;
            }

            // 목일 중학교 체육관 입구 쪽(3차교체)
            else if (child.Key == "ICL1L2100450")
            {
                ICL1L2100450.Key = key_450 = child.Key;
                ICL1L2100450.Timestamp = timestamp_450 = child.Timestamp;
                ICL1L2100450.Datetime = datetime_450 = child.Datetime;
                ICL1L2100450.Tm = tm_450 = child.Tm;
                ICL1L2100450.Pm01_raw = pm01_raw_450 = child.Pm01_raw;
                ICL1L2100450.Pm25_raw = pm25_raw_450 = child.Pm25_raw;
                ICL1L2100450.Pm10_raw = pm10_raw_450 = child.Pm10_raw;
                ICL1L2100450.Temp = temp_450 = child.Temp;
                ICL1L2100450.Humi = humi_450 = child.Humi;
                ICL1L2100450.Co2 = co2_450 = child.Co2;
                ICL1L2100450.Voc = voc_450 = child.Voc;
                ICL1L2100450.Noise = noise_450 = child.Noise;
                ICL1L2100450.Pm10 = pm10_450 = child.Pm10;
                ICL1L2100450.Pm25 = pm25_450 = child.Pm25;
                ICL1L2100450.Pm01 = pm01_450 = child.Pm01;
                ICL1L2100450.Cici_pm10 = cici_pm10_450 = child.Cici_pm10;
                ICL1L2100450.Cici_pm25 = cici_pm25_450 = child.Cici_pm25;
                ICL1L2100450.Cici_co2 = cici_co2_450 = child.Cici_co2;
                ICL1L2100450.Cici_voc = cici_voc_450 = child.Cici_voc;
                ICL1L2100450.Ciai = ciai_450 = child.Ciai;
                ICL1L2100450.Cici_temp = cici_temp_450 = child.Cici_temp;
                ICL1L2100450.Cici_humi = cici_humi_450 = child.Cici_humi;
                ICL1L2100450.Cici_noise = cici_noise_450 = child.Cici_noise;
                ICL1L2100450.Cici = cici_450 = child.Cici;
            }
            // 목일 중학교 체육관 강연대 쪽 (2차교체)
            else if (child.Key == "ICL1L2100449")
            {
                ICL1L2100449.Key = key_449 = child.Key;
                ICL1L2100449.Timestamp = timestamp_449 = child.Timestamp;
                ICL1L2100449.Datetime = datetime_449 = child.Datetime;
                ICL1L2100449.Tm = tm_449 = child.Tm;
                ICL1L2100449.Pm01_raw = pm01_raw_449 = child.Pm01_raw;
                ICL1L2100449.Pm25_raw = pm25_raw_449 = child.Pm25_raw;
                ICL1L2100449.Pm10_raw = pm10_raw_449 = child.Pm10_raw;
                ICL1L2100449.Temp = temp_449 = child.Temp;
                ICL1L2100449.Humi = humi_449 = child.Humi;
                ICL1L2100449.Co2 = co2_449 = child.Co2;
                ICL1L2100449.Voc = voc_449 = child.Voc;
                ICL1L2100449.Noise = noise_449 = child.Noise;
                ICL1L2100449.Pm10 = pm10_449 = child.Pm10;
                ICL1L2100449.Pm25 = pm25_449 = child.Pm25;
                ICL1L2100449.Pm01 = pm01_449 = child.Pm01;
                ICL1L2100449.Cici_pm10 = cici_pm10_449 = child.Cici_pm10;
                ICL1L2100449.Cici_pm25 = cici_pm25_449 = child.Cici_pm25;
                ICL1L2100449.Cici_co2 = cici_co2_449 = child.Cici_co2;
                ICL1L2100449.Cici_voc = cici_voc_449 = child.Cici_voc;
                ICL1L2100449.Ciai = ciai_449 = child.Ciai;
                ICL1L2100449.Cici_temp = cici_temp_449 = child.Cici_temp;
                ICL1L2100449.Cici_humi = cici_humi_449 = child.Cici_humi;
                ICL1L2100449.Cici_noise = cici_noise_449 = child.Cici_noise;
                ICL1L2100449.Cici = cici_449 = child.Cici;
            }
            // 목일 중학교 체육관 운동장 입구 쪽
            else if (child.Key == "ICW0W2001044")
            {
                ICW0W2001044.Key = key_044 = child.Key;
                ICW0W2001044.Timestamp = timestamp_044 = child.Timestamp;
                ICW0W2001044.Datetime = datetime_044 = child.Datetime;
                ICW0W2001044.Tm = tm_044 = child.Tm;
                ICW0W2001044.Pm01_raw = pm01_raw_044 = child.Pm01_raw;
                ICW0W2001044.Pm25_raw = pm25_raw_044 = child.Pm25_raw;
                ICW0W2001044.Pm10_raw = pm10_raw_044 = child.Pm10_raw;
                ICW0W2001044.Temp = temp_044 = child.Temp;
                ICW0W2001044.Humi = humi_044 = child.Humi;
                ICW0W2001044.Co2 = co2_044 = child.Co2;
                ICW0W2001044.Voc = voc_044 = child.Voc;
                ICW0W2001044.Noise = noise_044 = child.Noise;
                ICW0W2001044.Pm10 = pm10_044 = child.Pm10;
                ICW0W2001044.Pm25 = pm25_044 = child.Pm25;
                ICW0W2001044.Pm01 = pm01_044 = child.Pm01;
                ICW0W2001044.Cici_pm10 = cici_pm10_044 = child.Cici_pm10;
                ICW0W2001044.Cici_pm25 = cici_pm25_044 = child.Cici_pm25;
                ICW0W2001044.Cici_co2 = cici_co2_044 = child.Cici_co2;
                ICW0W2001044.Cici_voc = cici_voc_044 = child.Cici_voc;
                ICW0W2001044.Ciai = ciai_044 = child.Ciai;
                ICW0W2001044.Cici_temp = cici_temp_044 = child.Cici_temp;
                ICW0W2001044.Cici_humi = cici_humi_044 = child.Cici_humi;
                ICW0W2001044.Cici_noise = cici_noise_044 = child.Cici_noise;
                ICW0W2001044.Cici = cici_044 = child.Cici;
            }
            // 목일 중학교 체육관 운동장 강연대 쪽
            else if (child.Key == "ICL1L2000065")
            {
                ICL1L2000065.Key = key_065 = child.Key;
                ICL1L2000065.Timestamp = timestamp_065 = child.Timestamp;
                ICL1L2000065.Datetime = datetime_065 = child.Datetime;
                ICL1L2000065.Tm = tm_065 = child.Tm;
                ICL1L2000065.Pm01_raw = pm01_raw_065 = child.Pm01_raw;
                ICL1L2000065.Pm25_raw = pm25_raw_065 = child.Pm25_raw;
                ICL1L2000065.Pm10_raw = pm10_raw_065 = child.Pm10_raw;
                ICL1L2000065.Temp = temp_065 = child.Temp;
                ICL1L2000065.Humi = humi_065 = child.Humi;
                ICL1L2000065.Co2 = co2_065 = child.Co2;
                ICL1L2000065.Voc = voc_065 = child.Voc;
                ICL1L2000065.Noise = noise_065 = child.Noise;
                ICL1L2000065.Pm10 = pm10_065 = child.Pm10;
                ICL1L2000065.Pm25 = pm25_065 = child.Pm25;
                ICL1L2000065.Pm01 = pm01_065 = child.Pm01;
                ICL1L2000065.Cici_pm10 = cici_pm10_065 = child.Cici_pm10;
                ICL1L2000065.Cici_pm25 = cici_pm25_065 = child.Cici_pm25;
                ICL1L2000065.Cici_co2 = cici_co2_065 = child.Cici_co2;
                ICL1L2000065.Cici_voc = cici_voc_065 = child.Cici_voc;
                ICL1L2000065.Ciai = ciai_065 = child.Ciai;
                ICL1L2000065.Cici_temp = cici_temp_065 = child.Cici_temp;
                ICL1L2000065.Cici_humi = cici_humi_065 = child.Cici_humi;
                ICL1L2000065.Cici_noise = cici_noise_065 = child.Cici_noise;
                ICL1L2000065.Cici = cici_065 = child.Cici;
            }



            else if (child.Key == "ICW0W2001046")
            {
                ICW0W2001046.Key = key_046 = child.Key;
                ICW0W2001046.Timestamp = timestamp_046 = child.Timestamp;
                ICW0W2001046.Datetime = datetime_046 = child.Datetime;
                ICW0W2001046.Tm = tm_046 = child.Tm;
                ICW0W2001046.Pm01_raw = pm01_raw_046 = child.Pm01_raw;
                ICW0W2001046.Pm25_raw = pm25_raw_046 = child.Pm25_raw;
                ICW0W2001046.Pm10_raw = pm10_raw_046 = child.Pm10_raw;
                ICW0W2001046.Temp = temp_046 = child.Temp;
                ICW0W2001046.Humi = humi_046 = child.Humi;
                ICW0W2001046.Co2 = co2_046 = child.Co2;
                ICW0W2001046.Voc = voc_046 = child.Voc;
                ICW0W2001046.Noise = noise_046 = child.Noise;
                ICW0W2001046.Pm10 = pm10_046 = child.Pm10;
                ICW0W2001046.Pm25 = pm25_046 = child.Pm25;
                ICW0W2001046.Pm01 = pm01_046 = child.Pm01;
                ICW0W2001046.Cici_pm10 = cici_pm10_046 = child.Cici_pm10;
                ICW0W2001046.Cici_pm25 = cici_pm25_046 = child.Cici_pm25;
                ICW0W2001046.Cici_co2 = cici_co2_046 = child.Cici_co2;
                ICW0W2001046.Cici_voc = cici_voc_046 = child.Cici_voc;
                ICW0W2001046.Ciai = ciai_046 = child.Ciai;
                ICW0W2001046.Cici_temp = cici_temp_046 = child.Cici_temp;
                ICW0W2001046.Cici_humi = cici_humi_046 = child.Cici_humi;
                ICW0W2001046.Cici_noise = cici_noise_046 = child.Cici_noise;
                ICW0W2001046.Cici = cici_046 = child.Cici;
            }
            else if (child.Key == "ICW0W2001045")
            {
                ICW0W2001045.Key = key_045 = child.Key;
                ICW0W2001045.Timestamp = timestamp_045 = child.Timestamp;
                ICW0W2001045.Datetime = datetime_045 = child.Datetime;
                ICW0W2001045.Tm = tm_045 = child.Tm;
                ICW0W2001045.Pm01_raw = pm01_raw_045 = child.Pm01_raw;
                ICW0W2001045.Pm25_raw = pm25_raw_045 = child.Pm25_raw;
                ICW0W2001045.Pm10_raw = pm10_raw_045 = child.Pm10_raw;
                ICW0W2001045.Temp = temp_045 = child.Temp;
                ICW0W2001045.Humi = humi_045 = child.Humi;
                ICW0W2001045.Co2 = co2_045 = child.Co2;
                ICW0W2001045.Voc = voc_045 = child.Voc;
                ICW0W2001045.Noise = noise_045 = child.Noise;
                ICW0W2001045.Pm10 = pm10_045 = child.Pm10;
                ICW0W2001045.Pm25 = pm25_045 = child.Pm25;
                ICW0W2001045.Pm01 = pm01_045 = child.Pm01;
                ICW0W2001045.Cici_pm10 = cici_pm10_045 = child.Cici_pm10;
                ICW0W2001045.Cici_pm25 = cici_pm25_045 = child.Cici_pm25;
                ICW0W2001045.Cici_co2 = cici_co2_045 = child.Cici_co2;
                ICW0W2001045.Cici_voc = cici_voc_045 = child.Cici_voc;
                ICW0W2001045.Ciai = ciai_045 = child.Ciai;
                ICW0W2001045.Cici_temp = cici_temp_045 = child.Cici_temp;
                ICW0W2001045.Cici_humi = cici_humi_045 = child.Cici_humi;
                ICW0W2001045.Cici_noise = cici_noise_045 = child.Cici_noise;
                ICW0W2001045.Cici = cici_045 = child.Cici;
            }
            else if (child.Key == "ICW0W2001043")
            {
                ICW0W2001043.Key = key_043 = child.Key;
                ICW0W2001043.Timestamp = timestamp_043 = child.Timestamp;
                ICW0W2001043.Datetime = datetime_043 = child.Datetime;
                ICW0W2001043.Tm = tm_043 = child.Tm;
                ICW0W2001043.Pm01_raw = pm01_raw_043 = child.Pm01_raw;
                ICW0W2001043.Pm25_raw = pm25_raw_043 = child.Pm25_raw;
                ICW0W2001043.Pm10_raw = pm10_raw_043 = child.Pm10_raw;
                ICW0W2001043.Temp = temp_043 = child.Temp;
                ICW0W2001043.Humi = humi_043 = child.Humi;
                ICW0W2001043.Co2 = co2_043 = child.Co2;
                ICW0W2001043.Voc = voc_043 = child.Voc;
                ICW0W2001043.Noise = noise_043 = child.Noise;
                ICW0W2001043.Pm10 = pm10_043 = child.Pm10;
                ICW0W2001043.Pm25 = pm25_043 = child.Pm25;
                ICW0W2001043.Pm01 = pm01_043 = child.Pm01;
                ICW0W2001043.Cici_pm10 = cici_pm10_043 = child.Cici_pm10;
                ICW0W2001043.Cici_pm25 = cici_pm25_043 = child.Cici_pm25;
                ICW0W2001043.Cici_co2 = cici_co2_043 = child.Cici_co2;
                ICW0W2001043.Cici_voc = cici_voc_043 = child.Cici_voc;
                ICW0W2001043.Ciai = ciai_043 = child.Ciai;
                ICW0W2001043.Cici_temp = cici_temp_043 = child.Cici_temp;
                ICW0W2001043.Cici_humi = cici_humi_043 = child.Cici_humi;
                ICW0W2001043.Cici_noise = cici_noise_043 = child.Cici_noise;
                ICW0W2001043.Cici = cici_043 = child.Cici;
            }
            else if (child.Key == "ICW0W2001042")
            {
                ICW0W2001042.Key = key_042 = child.Key;
                ICW0W2001042.Timestamp = timestamp_042 = child.Timestamp;
                ICW0W2001042.Datetime = datetime_042 = child.Datetime;
                ICW0W2001042.Tm = tm_042 = child.Tm;
                ICW0W2001042.Pm01_raw = pm01_raw_042 = child.Pm01_raw;
                ICW0W2001042.Pm25_raw = pm25_raw_042 = child.Pm25_raw;
                ICW0W2001042.Pm10_raw = pm10_raw_042 = child.Pm10_raw;
                ICW0W2001042.Temp = temp_042 = child.Temp;
                ICW0W2001042.Humi = humi_042 = child.Humi;
                ICW0W2001042.Co2 = co2_042 = child.Co2;
                ICW0W2001042.Voc = voc_042 = child.Voc;
                ICW0W2001042.Noise = noise_042 = child.Noise;
                ICW0W2001042.Pm10 = pm10_042 = child.Pm10;
                ICW0W2001042.Pm25 = pm25_042 = child.Pm25;
                ICW0W2001042.Pm01 = pm01_042 = child.Pm01;
                ICW0W2001042.Cici_pm10 = cici_pm10_042 = child.Cici_pm10;
                ICW0W2001042.Cici_pm25 = cici_pm25_042 = child.Cici_pm25;
                ICW0W2001042.Cici_co2 = cici_co2_042 = child.Cici_co2;
                ICW0W2001042.Cici_voc = cici_voc_042 = child.Cici_voc;
                ICW0W2001042.Ciai = ciai_042 = child.Ciai;
                ICW0W2001042.Cici_temp = cici_temp_042 = child.Cici_temp;
                ICW0W2001042.Cici_humi = cici_humi_042 = child.Cici_humi;
                ICW0W2001042.Cici_noise = cici_noise_042 = child.Cici_noise;
                ICW0W2001042.Cici = cici_042 = child.Cici;
            }
            else if (child.Key == "ISC0W2000006")
            {
                ISC0W2000006.Key = key_006 = child.Key;
                ISC0W2000006.Timestamp = timestamp_006 = child.Timestamp;
                ISC0W2000006.Datetime = datetime_006 = child.Datetime;
                ISC0W2000006.Tm = tm_006 = child.Tm;
                ISC0W2000006.Pm01_raw = pm01_raw_006 = child.Pm01_raw;
                ISC0W2000006.Pm25_raw = pm25_raw_006 = child.Pm25_raw;
                ISC0W2000006.Pm10_raw = pm10_raw_006 = child.Pm10_raw;
                ISC0W2000006.Temp = temp_006 = child.Temp;
                ISC0W2000006.Humi = humi_006 = child.Humi;
                ISC0W2000006.Co2 = co2_006 = child.Co2;
                ISC0W2000006.Voc = voc_006 = child.Voc;
                ISC0W2000006.Noise = noise_006 = child.Noise;
                ISC0W2000006.Pm10 = pm10_006 = child.Pm10;
                ISC0W2000006.Pm25 = pm25_006 = child.Pm25;
                ISC0W2000006.Pm01 = pm01_006 = child.Pm01;
                ISC0W2000006.Cici_pm10 = cici_pm10_006 = child.Cici_pm10;
                ISC0W2000006.Cici_pm25 = cici_pm25_006 = child.Cici_pm25;
                ISC0W2000006.Cici_co2 = cici_co2_006 = child.Cici_co2;
                ISC0W2000006.Cici_voc = cici_voc_006 = child.Cici_voc;
                ISC0W2000006.Ciai = ciai_006 = child.Ciai;
                ISC0W2000006.Cici_temp = cici_temp_006 = child.Cici_temp;
                ISC0W2000006.Cici_humi = cici_humi_006 = child.Cici_humi;
                ISC0W2000006.Cici_noise = cici_noise_006 = child.Cici_noise;
                ISC0W2000006.Cici = cici_006 = child.Cici;
            }
            else if (child.Key == "ICW0W2001041")
            {
                ICW0W2001041.Key = key_041 = child.Key;
                ICW0W2001041.Timestamp = timestamp_041 = child.Timestamp;
                ICW0W2001041.Datetime = datetime_041 = child.Datetime;
                ICW0W2001041.Tm = tm_041 = child.Tm;
                ICW0W2001041.Pm01_raw = pm01_raw_041 = child.Pm01_raw;
                ICW0W2001041.Pm25_raw = pm25_raw_041 = child.Pm25_raw;
                ICW0W2001041.Pm10_raw = pm10_raw_041 = child.Pm10_raw;
                ICW0W2001041.Temp = temp_041 = child.Temp;
                ICW0W2001041.Humi = humi_041 = child.Humi;
                ICW0W2001041.Co2 = co2_041 = child.Co2;
                ICW0W2001041.Voc = voc_041 = child.Voc;
                ICW0W2001041.Noise = noise_041 = child.Noise;
                ICW0W2001041.Pm10 = pm10_041 = child.Pm10;
                ICW0W2001041.Pm25 = pm25_041 = child.Pm25;
                ICW0W2001041.Pm01 = pm01_041 = child.Pm01;
                ICW0W2001041.Cici_pm10 = cici_pm10_041 = child.Cici_pm10;
                ICW0W2001041.Cici_pm25 = cici_pm25_041 = child.Cici_pm25;
                ICW0W2001041.Cici_co2 = cici_co2_041 = child.Cici_co2;
                ICW0W2001041.Cici_voc = cici_voc_041 = child.Cici_voc;
                ICW0W2001041.Ciai = ciai_041 = child.Ciai;
                ICW0W2001041.Cici_temp = cici_temp_041 = child.Cici_temp;
                ICW0W2001041.Cici_humi = cici_humi_041 = child.Cici_humi;
                ICW0W2001041.Cici_noise = cici_noise_041 = child.Cici_noise;
                ICW0W2001041.Cici = cici_041 = child.Cici;
            }
            else if (child.Key == "ISC0W2000007")
            {
                ISC0W2000007.Key = key_007 = child.Key;
                ISC0W2000007.Timestamp = timestamp_007 = child.Timestamp;
                ISC0W2000007.Datetime = datetime_007 = child.Datetime;
                ISC0W2000007.Tm = tm_007 = child.Tm;
                ISC0W2000007.Pm01_raw = pm01_raw_007 = child.Pm01_raw;
                ISC0W2000007.Pm25_raw = pm25_raw_007 = child.Pm25_raw;
                ISC0W2000007.Pm10_raw = pm10_raw_007 = child.Pm10_raw;
                ISC0W2000007.Temp = temp_007 = child.Temp;
                ISC0W2000007.Humi = humi_007 = child.Humi;
                ISC0W2000007.Co2 = co2_007 = child.Co2;
                ISC0W2000007.Voc = voc_007 = child.Voc;
                ISC0W2000007.Noise = noise_007 = child.Noise;
                ISC0W2000007.Pm10 = pm10_007 = child.Pm10;
                ISC0W2000007.Pm25 = pm25_007 = child.Pm25;
                ISC0W2000007.Pm01 = pm01_007 = child.Pm01;
                ISC0W2000007.Cici_pm10 = cici_pm10_007 = child.Cici_pm10;
                ISC0W2000007.Cici_pm25 = cici_pm25_007 = child.Cici_pm25;
                ISC0W2000007.Cici_co2 = cici_co2_007 = child.Cici_co2;
                ISC0W2000007.Cici_voc = cici_voc_007 = child.Cici_voc;
                ISC0W2000007.Ciai = ciai_007 = child.Ciai;
                ISC0W2000007.Cici_temp = cici_temp_007 = child.Cici_temp;
                ISC0W2000007.Cici_humi = cici_humi_007 = child.Cici_humi;
                ISC0W2000007.Cici_noise = cici_noise_007 = child.Cici_noise;
                ISC0W2000007.Cici = cici_007 = child.Cici;
            }
            else if (child.Key == "ISC0W2000004")
            {
                ISC0W2000004.Key = key_004 = child.Key;
                ISC0W2000004.Timestamp = timestamp_004 = child.Timestamp;
                ISC0W2000004.Datetime = datetime_004 = child.Datetime;
                ISC0W2000004.Tm = tm_004 = child.Tm;
                ISC0W2000004.Pm01_raw = pm01_raw_004 = child.Pm01_raw;
                ISC0W2000004.Pm25_raw = pm25_raw_004 = child.Pm25_raw;
                ISC0W2000004.Pm10_raw = pm10_raw_004 = child.Pm10_raw;
                ISC0W2000004.Temp = temp_004 = child.Temp;
                ISC0W2000004.Humi = humi_004 = child.Humi;
                ISC0W2000004.Co2 = co2_004 = child.Co2;
                ISC0W2000004.Voc = voc_004 = child.Voc;
                ISC0W2000004.Noise = noise_004 = child.Noise;
                ISC0W2000004.Pm10 = pm10_004 = child.Pm10;
                ISC0W2000004.Pm25 = pm25_004 = child.Pm25;
                ISC0W2000004.Pm01 = pm01_004 = child.Pm01;
                ISC0W2000004.Cici_pm10 = cici_pm10_004 = child.Cici_pm10;
                ISC0W2000004.Cici_pm25 = cici_pm25_004 = child.Cici_pm25;
                ISC0W2000004.Cici_co2 = cici_co2_004 = child.Cici_co2;
                ISC0W2000004.Cici_voc = cici_voc_004 = child.Cici_voc;
                ISC0W2000004.Ciai = ciai_004 = child.Ciai;
                ISC0W2000004.Cici_temp = cici_temp_004 = child.Cici_temp;
                ISC0W2000004.Cici_humi = cici_humi_004 = child.Cici_humi;
                ISC0W2000004.Cici_noise = cici_noise_004 = child.Cici_noise;
                ISC0W2000004.Cici = cici_004 = child.Cici;
            }
            else if (child.Key == "ISC0W2000005")
            {
                ISC0W2000005.Key = key_005 = child.Key;
                ISC0W2000005.Timestamp = timestamp_005 = child.Timestamp;
                ISC0W2000005.Datetime = datetime_005 = child.Datetime;
                ISC0W2000005.Tm = tm_005 = child.Tm;
                ISC0W2000005.Pm01_raw = pm01_raw_005 = child.Pm01_raw;
                ISC0W2000005.Pm25_raw = pm25_raw_005 = child.Pm25_raw;
                ISC0W2000005.Pm10_raw = pm10_raw_005 = child.Pm10_raw;
                ISC0W2000005.Temp = temp_005 = child.Temp;
                ISC0W2000005.Humi = humi_005 = child.Humi;
                ISC0W2000005.Co2 = co2_005 = child.Co2;
                ISC0W2000005.Voc = voc_005 = child.Voc;
                ISC0W2000005.Noise = noise_005 = child.Noise;
                ISC0W2000005.Pm10 = pm10_005 = child.Pm10;
                ISC0W2000005.Pm25 = pm25_005 = child.Pm25;
                ISC0W2000005.Pm01 = pm01_005 = child.Pm01;
                ISC0W2000005.Cici_pm10 = cici_pm10_005 = child.Cici_pm10;
                ISC0W2000005.Cici_pm25 = cici_pm25_005 = child.Cici_pm25;
                ISC0W2000005.Cici_co2 = cici_co2_005 = child.Cici_co2;
                ISC0W2000005.Cici_voc = cici_voc_005 = child.Cici_voc;
                ISC0W2000005.Ciai = ciai_005 = child.Ciai;
                ISC0W2000005.Cici_temp = cici_temp_005 = child.Cici_temp;
                ISC0W2000005.Cici_humi = cici_humi_005 = child.Cici_humi;
                ISC0W2000005.Cici_noise = cici_noise_005 = child.Cici_noise;
                ISC0W2000005.Cici = cici_005 = child.Cici;
            }
            else if (child.Key == "ISC0W2000020")
            {
                ISC0W2000020.Key = key_020 = child.Key;
                ISC0W2000020.Timestamp = timestamp_020 = child.Timestamp;
                ISC0W2000020.Datetime = datetime_020 = child.Datetime;
                ISC0W2000020.Tm = tm_020 = child.Tm;
                ISC0W2000020.Pm01_raw = pm01_raw_020 = child.Pm01_raw;
                ISC0W2000020.Pm25_raw = pm25_raw_020 = child.Pm25_raw;
                ISC0W2000020.Pm10_raw = pm10_raw_020 = child.Pm10_raw;
                ISC0W2000020.Temp = temp_020 = child.Temp;
                ISC0W2000020.Humi = humi_020 = child.Humi;
                ISC0W2000020.Co2 = co2_020 = child.Co2;
                ISC0W2000020.Voc = voc_020 = child.Voc;
                ISC0W2000020.Noise = noise_020 = child.Noise;
                ISC0W2000020.Pm10 = pm10_020 = child.Pm10;
                ISC0W2000020.Pm25 = pm25_020 = child.Pm25;
                ISC0W2000020.Pm01 = pm01_020 = child.Pm01;
                ISC0W2000020.Cici_pm10 = cici_pm10_020 = child.Cici_pm10;
                ISC0W2000020.Cici_pm25 = cici_pm25_020 = child.Cici_pm25;
                ISC0W2000020.Cici_co2 = cici_co2_020 = child.Cici_co2;
                ISC0W2000020.Cici_voc = cici_voc_020 = child.Cici_voc;
                ISC0W2000020.Ciai = ciai_020 = child.Ciai;
                ISC0W2000020.Cici_temp = cici_temp_020 = child.Cici_temp;
                ISC0W2000020.Cici_humi = cici_humi_020 = child.Cici_humi;
                ISC0W2000020.Cici_noise = cici_noise_020 = child.Cici_noise;
                ISC0W2000020.Cici = cici_020 = child.Cici;
            }
            else if (child.Key == "000000000000")
            {
                O00000000000.Key = key_000 = child.Key;
                O00000000000.Timestamp = timestamp_000 = child.Timestamp;
                O00000000000.Datetime = datetime_000 = child.Datetime;
                O00000000000.Tm = tm_000 = child.Tm;
                O00000000000.Pm01_raw = pm01_raw_000 = child.Pm01_raw;
                O00000000000.Pm25_raw = pm25_raw_000 = child.Pm25_raw;
                O00000000000.Pm10_raw = pm10_raw_000 = child.Pm10_raw;
                O00000000000.Temp = temp_000 = child.Temp;
                O00000000000.Humi = humi_000 = child.Humi;
                O00000000000.Co2 = co2_000 = child.Co2;
                O00000000000.Voc = voc_000 = child.Voc;
                O00000000000.Noise = noise_000 = child.Noise;
                O00000000000.Pm10 = pm10_000 = child.Pm10;
                O00000000000.Pm25 = pm25_000 = child.Pm25;
                O00000000000.Pm01 = pm01_000 = child.Pm01;
                O00000000000.Cici_pm10 = cici_pm10_000 = child.Cici_pm10;
                O00000000000.Cici_pm25 = cici_pm25_000 = child.Cici_pm25;
                O00000000000.Cici_co2 = cici_co2_000 = child.Cici_co2;
                O00000000000.Cici_voc = cici_voc_000 = child.Cici_voc;
                O00000000000.Ciai = ciai_000 = child.Ciai;
                O00000000000.Cici_temp = cici_temp_000 = child.Cici_temp;
                O00000000000.Cici_humi = cici_humi_000 = child.Cici_humi;
                O00000000000.Cici_noise = cici_noise_000 = child.Cici_noise;
                O00000000000.Cici = cici_000 = child.Cici;
            }
            else if (child.Key == "ISC0W1800005")
            {
                ISC0W1800005.Key = key_0005 = child.Key;
                ISC0W1800005.Timestamp = timestamp_0005 = child.Timestamp;
                ISC0W1800005.Datetime = datetime_0005 = child.Datetime;
                ISC0W1800005.Tm = tm_0005 = child.Tm;
                ISC0W1800005.Pm01_raw = pm01_raw_0005 = child.Pm01_raw;
                ISC0W1800005.Pm25_raw = pm25_raw_0005 = child.Pm25_raw;
                ISC0W1800005.Pm10_raw = pm10_raw_0005 = child.Pm10_raw;
                ISC0W1800005.Temp = temp_0005 = child.Temp;
                ISC0W1800005.Humi = humi_0005 = child.Humi;
                ISC0W1800005.Co2 = co2_0005 = child.Co2;
                ISC0W1800005.Voc = voc_0005 = child.Voc;
                ISC0W1800005.Noise = noise_0005 = child.Noise;
                ISC0W1800005.Pm10 = pm10_0005 = child.Pm10;
                ISC0W1800005.Pm25 = pm25_0005 = child.Pm25;
                ISC0W1800005.Pm01 = pm01_0005 = child.Pm01;
                ISC0W1800005.Cici_pm10 = cici_pm10_0005 = child.Cici_pm10;
                ISC0W1800005.Cici_pm25 = cici_pm25_0005 = child.Cici_pm25;
                ISC0W1800005.Cici_co2 = cici_co2_0005 = child.Cici_co2;
                ISC0W1800005.Cici_voc = cici_voc_0005 = child.Cici_voc;
                ISC0W1800005.Ciai = ciai_0005 = child.Ciai;
                ISC0W1800005.Cici_temp = cici_temp_0005 = child.Cici_temp;
                ISC0W1800005.Cici_humi = cici_humi_0005 = child.Cici_humi;
                ISC0W1800005.Cici_noise = cici_noise_0005 = child.Cici_noise;
                ISC0W1800005.Cici = cici_0005 = child.Cici;
            }
            else if (child.Key == "ISC0W1800000")
            {
                ISC0W1800000.Key = key_0000 = child.Key;
                ISC0W1800000.Timestamp = timestamp_0000 = child.Timestamp;
                ISC0W1800000.Datetime = datetime_0000 = child.Datetime;
                ISC0W1800000.Tm = tm_0000 = child.Tm;
                ISC0W1800000.Pm01_raw = pm01_raw_0000 = child.Pm01_raw;
                ISC0W1800000.Pm25_raw = pm25_raw_0000 = child.Pm25_raw;
                ISC0W1800000.Pm10_raw = pm10_raw_0000 = child.Pm10_raw;
                ISC0W1800000.Temp = temp_0000 = child.Temp;
                ISC0W1800000.Humi = humi_0000 = child.Humi;
                ISC0W1800000.Co2 = co2_0000 = child.Co2;
                ISC0W1800000.Voc = voc_0000 = child.Voc;
                ISC0W1800000.Noise = noise_0000 = child.Noise;
                ISC0W1800000.Pm10 = pm10_0000 = child.Pm10;
                ISC0W1800000.Pm25 = pm25_0000 = child.Pm25;
                ISC0W1800000.Pm01 = pm01_0000 = child.Pm01;
                ISC0W1800000.Cici_pm10 = cici_pm10_0000 = child.Cici_pm10;
                ISC0W1800000.Cici_pm25 = cici_pm25_0000 = child.Cici_pm25;
                ISC0W1800000.Cici_co2 = cici_co2_0000 = child.Cici_co2;
                ISC0W1800000.Cici_voc = cici_voc_0000 = child.Cici_voc;
                ISC0W1800000.Ciai = ciai_0000 = child.Ciai;
                ISC0W1800000.Cici_temp = cici_temp_0000 = child.Cici_temp;
                ISC0W1800000.Cici_humi = cici_humi_0000 = child.Cici_humi;
                ISC0W1800000.Cici_noise = cici_noise_0000 = child.Cici_noise;
                ISC0W1800000.Cici = cici_0000 = child.Cici;
            }
            else if (child.Key == "ICW0W2100604")
            {
                ISC0W1800000.Key = key_604 = child.Key;
                ISC0W1800000.Timestamp = timestamp_604 = child.Timestamp;
                ISC0W1800000.Datetime = datetime_604 = child.Datetime;
                ISC0W1800000.Tm = tm_604 = child.Tm;
                ISC0W1800000.Pm01_raw = pm01_raw_604 = child.Pm01_raw;
                ISC0W1800000.Pm25_raw = pm25_raw_604 = child.Pm25_raw;
                ISC0W1800000.Pm10_raw = pm10_raw_604 = child.Pm10_raw;
                ISC0W1800000.Temp = temp_604 = child.Temp;
                ISC0W1800000.Humi = humi_604 = child.Humi;
                ISC0W1800000.Co2 = co2_604 = child.Co2;
                ISC0W1800000.Voc = voc_604 = child.Voc;
                ISC0W1800000.Noise = noise_604 = child.Noise;
                ISC0W1800000.Pm10 = pm10_604 = child.Pm10;
                ISC0W1800000.Pm25 = pm25_604 = child.Pm25;
                ISC0W1800000.Pm01 = pm01_604 = child.Pm01;
                ISC0W1800000.Cici_pm10 = cici_pm10_604 = child.Cici_pm10;
                ISC0W1800000.Cici_pm25 = cici_pm25_604 = child.Cici_pm25;
                ISC0W1800000.Cici_co2 = cici_co2_604 = child.Cici_co2;
                ISC0W1800000.Cici_voc = cici_voc_604 = child.Cici_voc;
                ISC0W1800000.Ciai = ciai_604 = child.Ciai;
                ISC0W1800000.Cici_temp = cici_temp_604 = child.Cici_temp;
                ISC0W1800000.Cici_humi = cici_humi_604 = child.Cici_humi;
                ISC0W1800000.Cici_noise = cici_noise_604 = child.Cici_noise;
                ISC0W1800000.Cici = cici_604 = child.Cici;
            }
            else if (child.Key == "ISC0W1800002")
            {
                ISC0W1800002.Key = key_0002 = child.Key;
                ISC0W1800002.Timestamp = timestamp_0002 = child.Timestamp;
                ISC0W1800002.Datetime = datetime_0002 = child.Datetime;
                ISC0W1800002.Tm = tm_0002 = child.Tm;
                ISC0W1800002.Pm01_raw = pm01_raw_0002 = child.Pm01_raw;
                ISC0W1800002.Pm25_raw = pm25_raw_0002 = child.Pm25_raw;
                ISC0W1800002.Pm10_raw = pm10_raw_0002 = child.Pm10_raw;
                ISC0W1800002.Temp = temp_0002 = child.Temp;
                ISC0W1800002.Humi = humi_0002 = child.Humi;
                ISC0W1800002.Co2 = co2_0002 = child.Co2;
                ISC0W1800002.Voc = voc_0002 = child.Voc;
                ISC0W1800002.Noise = noise_0002 = child.Noise;
                ISC0W1800002.Pm10 = pm10_0002 = child.Pm10;
                ISC0W1800002.Pm25 = pm25_0002 = child.Pm25;
                ISC0W1800002.Pm01 = pm01_0002 = child.Pm01;
                ISC0W1800002.Cici_pm10 = cici_pm10_0002 = child.Cici_pm10;
                ISC0W1800002.Cici_pm25 = cici_pm25_0002 = child.Cici_pm25;
                ISC0W1800002.Cici_co2 = cici_co2_0002 = child.Cici_co2;
                ISC0W1800002.Cici_voc = cici_voc_0002 = child.Cici_voc;
                ISC0W1800002.Ciai = ciai_0002 = child.Ciai;
                ISC0W1800002.Cici_temp = cici_temp_0002 = child.Cici_temp;
                ISC0W1800002.Cici_humi = cici_humi_0002 = child.Cici_humi;
                ISC0W1800002.Cici_noise = cici_noise_0002 = child.Cici_noise;
                ISC0W1800002.Cici = cici_0002 = child.Cici;
            }
            else if (child.Key == "ISC0W1800001")
            {
                ISC0W1800001.Key = key_0001 = child.Key;
                ISC0W1800001.Timestamp = timestamp_0001 = child.Timestamp;
                ISC0W1800001.Datetime = datetime_0001 = child.Datetime;
                ISC0W1800001.Tm = tm_0001 = child.Tm;
                ISC0W1800001.Pm01_raw = pm01_raw_0001 = child.Pm01_raw;
                ISC0W1800001.Pm25_raw = pm25_raw_0001 = child.Pm25_raw;
                ISC0W1800001.Pm10_raw = pm10_raw_0001 = child.Pm10_raw;
                ISC0W1800001.Temp = temp_0001 = child.Temp;
                ISC0W1800001.Humi = humi_0001 = child.Humi;
                ISC0W1800001.Co2 = co2_0001 = child.Co2;
                ISC0W1800001.Voc = voc_0001 = child.Voc;
                ISC0W1800001.Noise = noise_0001 = child.Noise;
                ISC0W1800001.Pm10 = pm10_0001 = child.Pm10;
                ISC0W1800001.Pm25 = pm25_0001 = child.Pm25;
                ISC0W1800001.Pm01 = pm01_0001 = child.Pm01;
                ISC0W1800001.Cici_pm10 = cici_pm10_0001 = child.Cici_pm10;
                ISC0W1800001.Cici_pm25 = cici_pm25_0001 = child.Cici_pm25;
                ISC0W1800001.Cici_co2 = cici_co2_0001 = child.Cici_co2;
                ISC0W1800001.Cici_voc = cici_voc_0001 = child.Cici_voc;
                ISC0W1800001.Ciai = ciai_0001 = child.Ciai;
                ISC0W1800001.Cici_temp = cici_temp_0001 = child.Cici_temp;
                ISC0W1800001.Cici_humi = cici_humi_0001 = child.Cici_humi;
                ISC0W1800001.Cici_noise = cici_noise_0001 = child.Cici_noise;
                ISC0W1800001.Cici = cici_0001 = child.Cici;
            }
            else if (child.Key == "ICW1W2000011")
            {
                ICW1W2000011.Key = key_011 = child.Key;
                ICW1W2000011.Timestamp = timestamp_011 = child.Timestamp;
                ICW1W2000011.Datetime = datetime_011 = child.Datetime;
                ICW1W2000011.Tm = tm_011 = child.Tm;
                ICW1W2000011.Pm01_raw = pm01_raw_011 = child.Pm01_raw;
                ICW1W2000011.Pm25_raw = pm25_raw_011 = child.Pm25_raw;
                ICW1W2000011.Pm10_raw = pm10_raw_011 = child.Pm10_raw;
                ICW1W2000011.Temp = temp_011 = child.Temp;
                ICW1W2000011.Humi = humi_011 = child.Humi;
                ICW1W2000011.Co2 = co2_011 = child.Co2;
                ICW1W2000011.Voc = voc_011 = child.Voc;
                ICW1W2000011.Noise = noise_011 = child.Noise;
                ICW1W2000011.Pm10 = pm10_011 = child.Pm10;
                ICW1W2000011.Pm25 = pm25_011 = child.Pm25;
                ICW1W2000011.Pm01 = pm01_011 = child.Pm01;
                ICW1W2000011.Cici_pm10 = cici_pm10_011 = child.Cici_pm10;
                ICW1W2000011.Cici_pm25 = cici_pm25_011 = child.Cici_pm25;
                ICW1W2000011.Cici_co2 = cici_co2_011 = child.Cici_co2;
                ICW1W2000011.Cici_voc = cici_voc_011 = child.Cici_voc;
                ICW1W2000011.Ciai = ciai_011 = child.Ciai;
                ICW1W2000011.Cici_temp = cici_temp_011 = child.Cici_temp;
                ICW1W2000011.Cici_humi = cici_humi_011 = child.Cici_humi;
                ICW1W2000011.Cici_noise = cici_noise_011 = child.Cici_noise;
                ICW1W2000011.Cici = cici_011 = child.Cici;
            }
            else if (child.Key == "ICW0W2001092")
            {
                ICW0W2001092.Key = key_092 = child.Key;
                ICW0W2001092.Timestamp = timestamp_092 = child.Timestamp;
                ICW0W2001092.Datetime = datetime_092 = child.Datetime;
                ICW0W2001092.Tm = tm_092 = child.Tm;
                ICW0W2001092.Pm01_raw = pm01_raw_092 = child.Pm01_raw;
                ICW0W2001092.Pm25_raw = pm25_raw_092 = child.Pm25_raw;
                ICW0W2001092.Pm10_raw = pm10_raw_092 = child.Pm10_raw;
                ICW0W2001092.Temp = temp_092 = child.Temp;
                ICW0W2001092.Humi = humi_092 = child.Humi;
                ICW0W2001092.Co2 = co2_092 = child.Co2;
                ICW0W2001092.Voc = voc_092 = child.Voc;
                ICW0W2001092.Noise = noise_092 = child.Noise;
                ICW0W2001092.Pm10 = pm10_092 = child.Pm10;
                ICW0W2001092.Pm25 = pm25_092 = child.Pm25;
                ICW0W2001092.Pm01 = pm01_092 = child.Pm01;
                ICW0W2001092.Cici_pm10 = cici_pm10_092 = child.Cici_pm10;
                ICW0W2001092.Cici_pm25 = cici_pm25_092 = child.Cici_pm25;
                ICW0W2001092.Cici_co2 = cici_co2_092 = child.Cici_co2;
                ICW0W2001092.Cici_voc = cici_voc_092 = child.Cici_voc;
                ICW0W2001092.Ciai = ciai_092 = child.Ciai;
                ICW0W2001092.Cici_temp = cici_temp_092 = child.Cici_temp;
                ICW0W2001092.Cici_humi = cici_humi_092 = child.Cici_humi;
                ICW0W2001092.Cici_noise = cici_noise_092 = child.Cici_noise;
                ICW0W2001092.Cici = cici_092 = child.Cici;
            }
            else if (child.Key == "ICW0W1800001")
            {
                ICW0W1800001.Key = key_00001 = child.Key;
                ICW0W1800001.Timestamp = timestamp_00001 = child.Timestamp;
                ICW0W1800001.Datetime = datetime_00001 = child.Datetime;
                ICW0W1800001.Tm = tm_00001 = child.Tm;
                ICW0W1800001.Pm01_raw = pm01_raw_00001 = child.Pm01_raw;
                ICW0W1800001.Pm25_raw = pm25_raw_00001 = child.Pm25_raw;
                ICW0W1800001.Pm10_raw = pm10_raw_00001 = child.Pm10_raw;
                ICW0W1800001.Temp = temp_00001 = child.Temp;
                ICW0W1800001.Humi = humi_00001 = child.Humi;
                ICW0W1800001.Co2 = co2_00001 = child.Co2;
                ICW0W1800001.Voc = voc_00001 = child.Voc;
                ICW0W1800001.Noise = noise_00001 = child.Noise;
                ICW0W1800001.Pm10 = pm10_00001 = child.Pm10;
                ICW0W1800001.Pm25 = pm25_00001 = child.Pm25;
                ICW0W1800001.Pm01 = pm01_00001 = child.Pm01;
                ICW0W1800001.Cici_pm10 = cici_pm10_00001 = child.Cici_pm10;
                ICW0W1800001.Cici_pm25 = cici_pm25_00001 = child.Cici_pm25;
                ICW0W1800001.Cici_co2 = cici_co2_00001 = child.Cici_co2;
                ICW0W1800001.Cici_voc = cici_voc_00001 = child.Cici_voc;
                ICW0W1800001.Ciai = ciai_00001 = child.Ciai;
                ICW0W1800001.Cici_temp = cici_temp_00001 = child.Cici_temp;
                ICW0W1800001.Cici_humi = cici_humi_00001 = child.Cici_humi;
                ICW0W1800001.Cici_noise = cici_noise_00001 = child.Cici_noise;
                ICW0W1800001.Cici = cici_00001 = child.Cici;
            }
            else if (child.Key == "ICW0W1800000")
            {
                ICW0W1800000.Key = key_00000 = child.Key;
                ICW0W1800000.Timestamp = timestamp_00000 = child.Timestamp;
                ICW0W1800000.Datetime = datetime_00000 = child.Datetime;
                ICW0W1800000.Tm = tm_00000 = child.Tm;
                ICW0W1800000.Pm01_raw = pm01_raw_00000 = child.Pm01_raw;
                ICW0W1800000.Pm25_raw = pm25_raw_00000 = child.Pm25_raw;
                ICW0W1800000.Pm10_raw = pm10_raw_00000 = child.Pm10_raw;
                ICW0W1800000.Temp = temp_00000 = child.Temp;
                ICW0W1800000.Humi = humi_00000 = child.Humi;
                ICW0W1800000.Co2 = co2_00000 = child.Co2;
                ICW0W1800000.Voc = voc_00000 = child.Voc;
                ICW0W1800000.Noise = noise_00000 = child.Noise;
                ICW0W1800000.Pm10 = pm10_00000 = child.Pm10;
                ICW0W1800000.Pm25 = pm25_00000 = child.Pm25;
                ICW0W1800000.Pm01 = pm01_00000 = child.Pm01;
                ICW0W1800000.Cici_pm10 = cici_pm10_00000 = child.Cici_pm10;
                ICW0W1800000.Cici_pm25 = cici_pm25_00000 = child.Cici_pm25;
                ICW0W1800000.Cici_co2 = cici_co2_00000 = child.Cici_co2;
                ICW0W1800000.Cici_voc = cici_voc_00000 = child.Cici_voc;
                ICW0W1800000.Ciai = ciai_00000 = child.Ciai;
                ICW0W1800000.Cici_temp = cici_temp_00000 = child.Cici_temp;
                ICW0W1800000.Cici_humi = cici_humi_00000 = child.Cici_humi;
                ICW0W1800000.Cici_noise = cici_noise_00000 = child.Cici_noise;
                ICW0W1800000.Cici = cici_00000 = child.Cici;
            }
            else if (child.Key == "ISC0W2000016")
            {
                ISC0W2000016.Key = key_016 = child.Key;
                ISC0W2000016.Timestamp = timestamp_016 = child.Timestamp;
                ISC0W2000016.Datetime = datetime_016 = child.Datetime;
                ISC0W2000016.Tm = tm_016 = child.Tm;
                ISC0W2000016.Pm01_raw = pm01_raw_016 = child.Pm01_raw;
                ISC0W2000016.Pm25_raw = pm25_raw_016 = child.Pm25_raw;
                ISC0W2000016.Pm10_raw = pm10_raw_016 = child.Pm10_raw;
                ISC0W2000016.Temp = temp_016 = child.Temp;
                ISC0W2000016.Humi = humi_016 = child.Humi;
                ISC0W2000016.Co2 = co2_016 = child.Co2;
                ISC0W2000016.Voc = voc_016 = child.Voc;
                ISC0W2000016.Noise = noise_016 = child.Noise;
                ISC0W2000016.Pm10 = pm10_016 = child.Pm10;
                ISC0W2000016.Pm25 = pm25_016 = child.Pm25;
                ISC0W2000016.Pm01 = pm01_016 = child.Pm01;
                ISC0W2000016.Cici_pm10 = cici_pm10_016 = child.Cici_pm10;
                ISC0W2000016.Cici_pm25 = cici_pm25_016 = child.Cici_pm25;
                ISC0W2000016.Cici_co2 = cici_co2_016 = child.Cici_co2;
                ISC0W2000016.Cici_voc = cici_voc_016 = child.Cici_voc;
                ISC0W2000016.Ciai = ciai_016 = child.Ciai;
                ISC0W2000016.Cici_temp = cici_temp_016 = child.Cici_temp;
                ISC0W2000016.Cici_humi = cici_humi_016 = child.Cici_humi;
                ISC0W2000016.Cici_noise = cici_noise_016 = child.Cici_noise;
                ISC0W2000016.Cici = cici_016 = child.Cici;
            }
            else if (child.Key == "ISC0W1800015")
            {
                ISC0W1800015.Key = key_015 = child.Key;
                ISC0W1800015.Timestamp = timestamp_015 = child.Timestamp;
                ISC0W1800015.Datetime = datetime_015 = child.Datetime;
                ISC0W1800015.Tm = tm_015 = child.Tm;
                ISC0W1800015.Pm01_raw = pm01_raw_015 = child.Pm01_raw;
                ISC0W1800015.Pm25_raw = pm25_raw_015 = child.Pm25_raw;
                ISC0W1800015.Pm10_raw = pm10_raw_015 = child.Pm10_raw;
                ISC0W1800015.Temp = temp_015 = child.Temp;
                ISC0W1800015.Humi = humi_015 = child.Humi;
                ISC0W1800015.Co2 = co2_015 = child.Co2;
                ISC0W1800015.Voc = voc_015 = child.Voc;
                ISC0W1800015.Noise = noise_015 = child.Noise;
                ISC0W1800015.Pm10 = pm10_015 = child.Pm10;
                ISC0W1800015.Pm25 = pm25_015 = child.Pm25;
                ISC0W1800015.Pm01 = pm01_015 = child.Pm01;
                ISC0W1800015.Cici_pm10 = cici_pm10_015 = child.Cici_pm10;
                ISC0W1800015.Cici_pm25 = cici_pm25_015 = child.Cici_pm25;
                ISC0W1800015.Cici_co2 = cici_co2_015 = child.Cici_co2;
                ISC0W1800015.Cici_voc = cici_voc_015 = child.Cici_voc;
                ISC0W1800015.Ciai = ciai_015 = child.Ciai;
                ISC0W1800015.Cici_temp = cici_temp_015 = child.Cici_temp;
                ISC0W1800015.Cici_humi = cici_humi_015 = child.Cici_humi;
                ISC0W1800015.Cici_noise = cici_noise_015 = child.Cici_noise;
                ISC0W1800015.Cici = cici_015 = child.Cici;
            }
            else if (child.Key == "ISC0W1800014")
            {
                ISC0W1800014.Key = key_014 = child.Key;
                ISC0W1800014.Timestamp = timestamp_014 = child.Timestamp;
                ISC0W1800014.Datetime = datetime_014 = child.Datetime;
                ISC0W1800014.Tm = tm_014 = child.Tm;
                ISC0W1800014.Pm01_raw = pm01_raw_014 = child.Pm01_raw;
                ISC0W1800014.Pm25_raw = pm25_raw_014 = child.Pm25_raw;
                ISC0W1800014.Pm10_raw = pm10_raw_014 = child.Pm10_raw;
                ISC0W1800014.Temp = temp_014 = child.Temp;
                ISC0W1800014.Humi = humi_014 = child.Humi;
                ISC0W1800014.Co2 = co2_014 = child.Co2;
                ISC0W1800014.Voc = voc_014 = child.Voc;
                ISC0W1800014.Noise = noise_014 = child.Noise;
                ISC0W1800014.Pm10 = pm10_014 = child.Pm10;
                ISC0W1800014.Pm25 = pm25_014 = child.Pm25;
                ISC0W1800014.Pm01 = pm01_014 = child.Pm01;
                ISC0W1800014.Cici_pm10 = cici_pm10_014 = child.Cici_pm10;
                ISC0W1800014.Cici_pm25 = cici_pm25_014 = child.Cici_pm25;
                ISC0W1800014.Cici_co2 = cici_co2_014 = child.Cici_co2;
                ISC0W1800014.Cici_voc = cici_voc_014 = child.Cici_voc;
                ISC0W1800014.Ciai = ciai_014 = child.Ciai;
                ISC0W1800014.Cici_temp = cici_temp_014 = child.Cici_temp;
                ISC0W1800014.Cici_humi = cici_humi_014 = child.Cici_humi;
                ISC0W1800014.Cici_noise = cici_noise_014 = child.Cici_noise;
                ISC0W1800014.Cici = cici_014 = child.Cici;
            }
            else if (child.Key == "ISC0W1800011")
            {
                ISC0W1800011.Key = key_0011 = child.Key;
                ISC0W1800011.Timestamp = timestamp_0011 = child.Timestamp;
                ISC0W1800011.Datetime = datetime_0011 = child.Datetime;
                ISC0W1800011.Tm = tm_0011 = child.Tm;
                ISC0W1800011.Pm01_raw = pm01_raw_0011 = child.Pm01_raw;
                ISC0W1800011.Pm25_raw = pm25_raw_0011 = child.Pm25_raw;
                ISC0W1800011.Pm10_raw = pm10_raw_0011 = child.Pm10_raw;
                ISC0W1800011.Temp = temp_0011 = child.Temp;
                ISC0W1800011.Humi = humi_0011 = child.Humi;
                ISC0W1800011.Co2 = co2_0011 = child.Co2;
                ISC0W1800011.Voc = voc_0011 = child.Voc;
                ISC0W1800011.Noise = noise_0011 = child.Noise;
                ISC0W1800011.Pm10 = pm10_0011 = child.Pm10;
                ISC0W1800011.Pm25 = pm25_0011 = child.Pm25;
                ISC0W1800011.Pm01 = pm01_0011 = child.Pm01;
                ISC0W1800011.Cici_pm10 = cici_pm10_0011 = child.Cici_pm10;
                ISC0W1800011.Cici_pm25 = cici_pm25_0011 = child.Cici_pm25;
                ISC0W1800011.Cici_co2 = cici_co2_0011 = child.Cici_co2;
                ISC0W1800011.Cici_voc = cici_voc_0011 = child.Cici_voc;
                ISC0W1800011.Ciai = ciai_0011 = child.Ciai;
                ISC0W1800011.Cici_temp = cici_temp_0011 = child.Cici_temp;
                ISC0W1800011.Cici_humi = cici_humi_0011 = child.Cici_humi;
                ISC0W1800011.Cici_noise = cici_noise_0011 = child.Cici_noise;
                ISC0W1800011.Cici = cici_0011 = child.Cici;
            }
            else if (child.Key == "ISC0W1800010")
            {
                ISC0W1800010.Key = key_0010 = child.Key;
                ISC0W1800010.Timestamp = timestamp_0010 = child.Timestamp;
                ISC0W1800010.Datetime = datetime_0010 = child.Datetime;
                ISC0W1800010.Tm = tm_0010 = child.Tm;
                ISC0W1800010.Pm01_raw = pm01_raw_0010 = child.Pm01_raw;
                ISC0W1800010.Pm25_raw = pm25_raw_0010 = child.Pm25_raw;
                ISC0W1800010.Pm10_raw = pm10_raw_0010 = child.Pm10_raw;
                ISC0W1800010.Temp = temp_0010 = child.Temp;
                ISC0W1800010.Humi = humi_0010 = child.Humi;
                ISC0W1800010.Co2 = co2_0010 = child.Co2;
                ISC0W1800010.Voc = voc_0010 = child.Voc;
                ISC0W1800010.Noise = noise_0010 = child.Noise;
                ISC0W1800010.Pm10 = pm10_0010 = child.Pm10;
                ISC0W1800010.Pm25 = pm25_0010 = child.Pm25;
                ISC0W1800010.Pm01 = pm01_0010 = child.Pm01;
                ISC0W1800010.Cici_pm10 = cici_pm10_0010 = child.Cici_pm10;
                ISC0W1800010.Cici_pm25 = cici_pm25_0010 = child.Cici_pm25;
                ISC0W1800010.Cici_co2 = cici_co2_0010 = child.Cici_co2;
                ISC0W1800010.Cici_voc = cici_voc_0010 = child.Cici_voc;
                ISC0W1800010.Ciai = ciai_0010 = child.Ciai;
                ISC0W1800010.Cici_temp = cici_temp_0010 = child.Cici_temp;
                ISC0W1800010.Cici_humi = cici_humi_0010 = child.Cici_humi;
                ISC0W1800010.Cici_noise = cici_noise_0010 = child.Cici_noise;
                ISC0W1800010.Cici = cici_0010 = child.Cici;
            }
            else if (child.Key == "ISC0W1800013")
            {
                ISC0W1800013.Key = key_0013 = child.Key;
                ISC0W1800013.Timestamp = timestamp_0013 = child.Timestamp;
                ISC0W1800013.Datetime = datetime_0013 = child.Datetime;
                ISC0W1800013.Tm = tm_0013 = child.Tm;
                ISC0W1800013.Pm01_raw = pm01_raw_0013 = child.Pm01_raw;
                ISC0W1800013.Pm25_raw = pm25_raw_0013 = child.Pm25_raw;
                ISC0W1800013.Pm10_raw = pm10_raw_0013 = child.Pm10_raw;
                ISC0W1800013.Temp = temp_0013 = child.Temp;
                ISC0W1800013.Humi = humi_0013 = child.Humi;
                ISC0W1800013.Co2 = co2_0013 = child.Co2;
                ISC0W1800013.Voc = voc_0013 = child.Voc;
                ISC0W1800013.Noise = noise_0013 = child.Noise;
                ISC0W1800013.Pm10 = pm10_0013 = child.Pm10;
                ISC0W1800013.Pm25 = pm25_0013 = child.Pm25;
                ISC0W1800013.Pm01 = pm01_0013 = child.Pm01;
                ISC0W1800013.Cici_pm10 = cici_pm10_0013 = child.Cici_pm10;
                ISC0W1800013.Cici_pm25 = cici_pm25_0013 = child.Cici_pm25;
                ISC0W1800013.Cici_co2 = cici_co2_0013 = child.Cici_co2;
                ISC0W1800013.Cici_voc = cici_voc_0013 = child.Cici_voc;
                ISC0W1800013.Ciai = ciai_0013 = child.Ciai;
                ISC0W1800013.Cici_temp = cici_temp_0013 = child.Cici_temp;
                ISC0W1800013.Cici_humi = cici_humi_0013 = child.Cici_humi;
                ISC0W1800013.Cici_noise = cici_noise_0013 = child.Cici_noise;
                ISC0W1800013.Cici = cici_0013 = child.Cici;
            }
            else if (child.Key == "ISC0W1800012")
            {
                ISC0W1800012.Key = key_0012 = child.Key;
                ISC0W1800012.Timestamp = timestamp_0012 = child.Timestamp;
                ISC0W1800012.Datetime = datetime_0012 = child.Datetime;
                ISC0W1800012.Tm = tm_0012 = child.Tm;
                ISC0W1800012.Pm01_raw = pm01_raw_0012 = child.Pm01_raw;
                ISC0W1800012.Pm25_raw = pm25_raw_0012 = child.Pm25_raw;
                ISC0W1800012.Pm10_raw = pm10_raw_0012 = child.Pm10_raw;
                ISC0W1800012.Temp = temp_0012 = child.Temp;
                ISC0W1800012.Humi = humi_0012 = child.Humi;
                ISC0W1800012.Co2 = co2_0012 = child.Co2;
                ISC0W1800012.Voc = voc_0012 = child.Voc;
                ISC0W1800012.Noise = noise_0012 = child.Noise;
                ISC0W1800012.Pm10 = pm10_0012 = child.Pm10;
                ISC0W1800012.Pm25 = pm25_0012 = child.Pm25;
                ISC0W1800012.Pm01 = pm01_0012 = child.Pm01;
                ISC0W1800012.Cici_pm10 = cici_pm10_0012 = child.Cici_pm10;
                ISC0W1800012.Cici_pm25 = cici_pm25_0012 = child.Cici_pm25;
                ISC0W1800012.Cici_co2 = cici_co2_0012 = child.Cici_co2;
                ISC0W1800012.Cici_voc = cici_voc_0012 = child.Cici_voc;
                ISC0W1800012.Ciai = ciai_0012 = child.Ciai;
                ISC0W1800012.Cici_temp = cici_temp_0012 = child.Cici_temp;
                ISC0W1800012.Cici_humi = cici_humi_0012 = child.Cici_humi;
                ISC0W1800012.Cici_noise = cici_noise_0012 = child.Cici_noise;
                ISC0W1800012.Cici = cici_0012 = child.Cici;
            }
            else if (child.Key == "ISC0W18000010")
            {
                ISC0W18000010.Key = key_00010 = child.Key;
                ISC0W18000010.Timestamp = timestamp_00010 = child.Timestamp;
                ISC0W18000010.Datetime = datetime_00010 = child.Datetime;
                ISC0W18000010.Tm = tm_00010 = child.Tm;
                ISC0W18000010.Pm01_raw = pm01_raw_00010 = child.Pm01_raw;
                ISC0W18000010.Pm25_raw = pm25_raw_00010 = child.Pm25_raw;
                ISC0W18000010.Pm10_raw = pm10_raw_00010 = child.Pm10_raw;
                ISC0W18000010.Temp = temp_00010 = child.Temp;
                ISC0W18000010.Humi = humi_00010 = child.Humi;
                ISC0W18000010.Co2 = co2_00010 = child.Co2;
                ISC0W18000010.Voc = voc_00010 = child.Voc;
                ISC0W18000010.Noise = noise_00010 = child.Noise;
                ISC0W18000010.Pm10 = pm10_00010 = child.Pm10;
                ISC0W18000010.Pm25 = pm25_00010 = child.Pm25;
                ISC0W18000010.Pm01 = pm01_00010 = child.Pm01;
                ISC0W18000010.Cici_pm10 = cici_pm10_00010 = child.Cici_pm10;
                ISC0W18000010.Cici_pm25 = cici_pm25_00010 = child.Cici_pm25;
                ISC0W18000010.Cici_co2 = cici_co2_00010 = child.Cici_co2;
                ISC0W18000010.Cici_voc = cici_voc_00010 = child.Cici_voc;
                ISC0W18000010.Ciai = ciai_00010 = child.Ciai;
                ISC0W18000010.Cici_temp = cici_temp_00010 = child.Cici_temp;
                ISC0W18000010.Cici_humi = cici_humi_00010 = child.Cici_humi;
                ISC0W18000010.Cici_noise = cici_noise_00010 = child.Cici_noise;
                ISC0W18000010.Cici = cici_00010 = child.Cici;
            }
            else if (child.Key == "ICW0W2100006")
            {
                ICW0W2100006.Key = key_0006 = child.Key;
                ICW0W2100006.Timestamp = timestamp_0006 = child.Timestamp;
                ICW0W2100006.Datetime = datetime_0006 = child.Datetime;
                ICW0W2100006.Tm = tm_0006 = child.Tm;
                ICW0W2100006.Pm01_raw = pm01_raw_0006 = child.Pm01_raw;
                ICW0W2100006.Pm25_raw = pm25_raw_0006 = child.Pm25_raw;
                ICW0W2100006.Pm10_raw = pm10_raw_0006 = child.Pm10_raw;
                ICW0W2100006.Temp = temp_0006 = child.Temp;
                ICW0W2100006.Humi = humi_0006 = child.Humi;
                ICW0W2100006.Co2 = co2_0006 = child.Co2;
                ICW0W2100006.Voc = voc_0006 = child.Voc;
                ICW0W2100006.Noise = noise_0006 = child.Noise;
                ICW0W2100006.Pm10 = pm10_0006 = child.Pm10;
                ICW0W2100006.Pm25 = pm25_0006 = child.Pm25;
                ICW0W2100006.Pm01 = pm01_0006 = child.Pm01;
                ICW0W2100006.Cici_pm10 = cici_pm10_0006 = child.Cici_pm10;
                ICW0W2100006.Cici_pm25 = cici_pm25_0006 = child.Cici_pm25;
                ICW0W2100006.Cici_co2 = cici_co2_0006 = child.Cici_co2;
                ICW0W2100006.Cici_voc = cici_voc_0006 = child.Cici_voc;
                ICW0W2100006.Ciai = ciai_0006 = child.Ciai;
                ICW0W2100006.Cici_temp = cici_temp_0006 = child.Cici_temp;
                ICW0W2100006.Cici_humi = cici_humi_0006 = child.Cici_humi;
                ICW0W2100006.Cici_noise = cici_noise_0006 = child.Cici_noise;
                ICW0W2100006.Cici = cici_0006 = child.Cici;
            }
            else if (child.Key == "ICW0W2200121")
            {
                ICW0W2200121.Key = key_121 = child.Key;
                ICW0W2200121.Timestamp = timestamp_121 = child.Timestamp;
                ICW0W2200121.Datetime = datetime_121 = child.Datetime;
                ICW0W2200121.Tm = tm_121 = child.Tm;
                ICW0W2200121.Pm01_raw = pm01_raw_121 = child.Pm01_raw;
                ICW0W2200121.Pm25_raw = pm25_raw_121 = child.Pm25_raw;
                ICW0W2200121.Pm10_raw = pm10_raw_121 = child.Pm10_raw;
                ICW0W2200121.Temp = temp_121 = child.Temp;
                ICW0W2200121.Humi = humi_121 = child.Humi;
                ICW0W2200121.Co2 = co2_121 = child.Co2;
                ICW0W2200121.Voc = voc_121 = child.Voc;
                ICW0W2200121.Noise = noise_121 = child.Noise;
                ICW0W2200121.Pm10 = pm10_121 = child.Pm10;
                ICW0W2200121.Pm25 = pm25_121 = child.Pm25;
                ICW0W2200121.Pm01 = pm01_121 = child.Pm01;
                ICW0W2200121.Cici_pm10 = cici_pm10_121 = child.Cici_pm10;
                ICW0W2200121.Cici_pm25 = cici_pm25_121 = child.Cici_pm25;
                ICW0W2200121.Cici_co2 = cici_co2_121 = child.Cici_co2;
                ICW0W2200121.Cici_voc = cici_voc_121 = child.Cici_voc;
                ICW0W2200121.Ciai = ciai_121 = child.Ciai;
                ICW0W2200121.Cici_temp = cici_temp_121 = child.Cici_temp;
                ICW0W2200121.Cici_humi = cici_humi_121 = child.Cici_humi;
                ICW0W2200121.Cici_noise = cici_noise_121 = child.Cici_noise;
                ICW0W2200121.Cici = cici_121 = child.Cici;
            }
            else if (child.Key == "IBOKW2200028")
            {
                IBOKW2200028.Key = key_028 = child.Key;
                IBOKW2200028.Timestamp = timestamp_028 = child.Timestamp;
                IBOKW2200028.Datetime = datetime_028 = child.Datetime;
                IBOKW2200028.Tm = tm_028 = child.Tm;
                IBOKW2200028.Pm01_raw = pm01_raw_028 = child.Pm01_raw;
                IBOKW2200028.Pm25_raw = pm25_raw_028 = child.Pm25_raw;
                IBOKW2200028.Pm10_raw = pm10_raw_028 = child.Pm10_raw;
                IBOKW2200028.Temp = temp_028 = child.Temp;
                IBOKW2200028.Humi = humi_028 = child.Humi;
                IBOKW2200028.Co2 = co2_028 = child.Co2;
                IBOKW2200028.Voc = voc_028 = child.Voc;
                IBOKW2200028.Noise = noise_028 = child.Noise;
                IBOKW2200028.Pm10 = pm10_028 = child.Pm10;
                IBOKW2200028.Pm25 = pm25_028 = child.Pm25;
                IBOKW2200028.Pm01 = pm01_028 = child.Pm01;
                IBOKW2200028.Cici_pm10 = cici_pm10_028 = child.Cici_pm10;
                IBOKW2200028.Cici_pm25 = cici_pm25_028 = child.Cici_pm25;
                IBOKW2200028.Cici_co2 = cici_co2_028 = child.Cici_co2;
                IBOKW2200028.Cici_voc = cici_voc_028 = child.Cici_voc;
                IBOKW2200028.Ciai = ciai_028 = child.Ciai;
                IBOKW2200028.Cici_temp = cici_temp_028 = child.Cici_temp;
                IBOKW2200028.Cici_humi = cici_humi_028 = child.Cici_humi;
                IBOKW2200028.Cici_noise = cici_noise_028 = child.Cici_noise;
                IBOKW2200028.Cici = cici_028 = child.Cici;
            }
            else if (child.Key == "IBOKW2200033")
            {
                IBOKW2200033.Key = key_033 = child.Key;
                IBOKW2200033.Timestamp = timestamp_033 = child.Timestamp;
                IBOKW2200033.Datetime = datetime_033 = child.Datetime;
                IBOKW2200033.Tm = tm_033 = child.Tm;
                IBOKW2200033.Pm01_raw = pm01_raw_033 = child.Pm01_raw;
                IBOKW2200033.Pm25_raw = pm25_raw_033 = child.Pm25_raw;
                IBOKW2200033.Pm10_raw = pm10_raw_033 = child.Pm10_raw;
                IBOKW2200033.Temp = temp_033 = child.Temp;
                IBOKW2200033.Humi = humi_033 = child.Humi;
                IBOKW2200033.Co2 = co2_033 = child.Co2;
                IBOKW2200033.Voc = voc_033 = child.Voc;
                IBOKW2200033.Noise = noise_033 = child.Noise;
                IBOKW2200033.Pm10 = pm10_033 = child.Pm10;
                IBOKW2200033.Pm25 = pm25_033 = child.Pm25;
                IBOKW2200033.Pm01 = pm01_033 = child.Pm01;
                IBOKW2200033.Cici_pm10 = cici_pm10_033 = child.Cici_pm10;
                IBOKW2200033.Cici_pm25 = cici_pm25_033 = child.Cici_pm25;
                IBOKW2200033.Cici_co2 = cici_co2_033 = child.Cici_co2;
                IBOKW2200033.Cici_voc = cici_voc_033 = child.Cici_voc;
                IBOKW2200033.Ciai = ciai_033 = child.Ciai;
                IBOKW2200033.Cici_temp = cici_temp_033 = child.Cici_temp;
                IBOKW2200033.Cici_humi = cici_humi_033 = child.Cici_humi;
                IBOKW2200033.Cici_noise = cici_noise_033 = child.Cici_noise;
                IBOKW2200033.Cici = cici_033 = child.Cici;
            }
            else if (child.Key == "IBOKW2200024")
            {
                IBOKW2200024.Key = key_024 = child.Key;
                IBOKW2200024.Timestamp = timestamp_024 = child.Timestamp;
                IBOKW2200024.Datetime = datetime_024 = child.Datetime;
                IBOKW2200024.Tm = tm_024 = child.Tm;
                IBOKW2200024.Pm01_raw = pm01_raw_024 = child.Pm01_raw;
                IBOKW2200024.Pm25_raw = pm25_raw_024 = child.Pm25_raw;
                IBOKW2200024.Pm10_raw = pm10_raw_024 = child.Pm10_raw;
                IBOKW2200024.Temp = temp_024 = child.Temp;
                IBOKW2200024.Humi = humi_024 = child.Humi;
                IBOKW2200024.Co2 = co2_024 = child.Co2;
                IBOKW2200024.Voc = voc_024 = child.Voc;
                IBOKW2200024.Noise = noise_024 = child.Noise;
                IBOKW2200024.Pm10 = pm10_024 = child.Pm10;
                IBOKW2200024.Pm25 = pm25_024 = child.Pm25;
                IBOKW2200024.Pm01 = pm01_024 = child.Pm01;
                IBOKW2200024.Cici_pm10 = cici_pm10_024 = child.Cici_pm10;
                IBOKW2200024.Cici_pm25 = cici_pm25_024 = child.Cici_pm25;
                IBOKW2200024.Cici_co2 = cici_co2_024 = child.Cici_co2;
                IBOKW2200024.Cici_voc = cici_voc_024 = child.Cici_voc;
                IBOKW2200024.Ciai = ciai_024 = child.Ciai;
                IBOKW2200024.Cici_temp = cici_temp_024 = child.Cici_temp;
                IBOKW2200024.Cici_humi = cici_humi_024 = child.Cici_humi;
                IBOKW2200024.Cici_noise = cici_noise_024 = child.Cici_noise;
                IBOKW2200024.Cici = cici_024 = child.Cici;
            }
            else if (child.Key == "IBOKW2200025") 
            {
                IBOKW2200025.Key = key_025 = child.Key;
                IBOKW2200025.Timestamp = timestamp_025 = child.Timestamp;
                IBOKW2200025.Datetime = datetime_025 = child.Datetime;
                IBOKW2200025.Tm = tm_025 = child.Tm;
                IBOKW2200025.Pm01_raw = pm01_raw_025 = child.Pm01_raw;
                IBOKW2200025.Pm25_raw = pm25_raw_025 = child.Pm25_raw;
                IBOKW2200025.Pm10_raw = pm10_raw_025 = child.Pm10_raw;
                IBOKW2200025.Temp = temp_025 = child.Temp;
                IBOKW2200025.Humi = humi_025 = child.Humi;
                IBOKW2200025.Co2 = co2_025 = child.Co2;
                IBOKW2200025.Voc = voc_025 = child.Voc;
                IBOKW2200025.Noise = noise_025 = child.Noise;
                IBOKW2200025.Pm10 = pm10_025 = child.Pm10;
                IBOKW2200025.Pm25 = pm25_025 = child.Pm25;
                IBOKW2200025.Pm01 = pm01_025 = child.Pm01;
                IBOKW2200025.Cici_pm10 = cici_pm10_025 = child.Cici_pm10;
                IBOKW2200025.Cici_pm25 = cici_pm25_025 = child.Cici_pm25;
                IBOKW2200025.Cici_co2 = cici_co2_025 = child.Cici_co2;
                IBOKW2200025.Cici_voc = cici_voc_025 = child.Cici_voc;
                IBOKW2200025.Ciai = ciai_025 = child.Ciai;
                IBOKW2200025.Cici_temp = cici_temp_025 = child.Cici_temp;
                IBOKW2200025.Cici_humi = cici_humi_025 = child.Cici_humi;
                IBOKW2200025.Cici_noise = cici_noise_025 = child.Cici_noise;
                IBOKW2200025.Cici = cici_025 = child.Cici;
            }
        }
    }

    #region UNITY_INSPECTOR
    #region ICW0W2001040 동아 유치원 2층 라온반
    [Space(20f)]
    [Header("< ICW0W2001040 >")]
    public string key_040;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_040;
    public string datetime_040;

    [Space(2f)]
    [Header("- Data")]
    public string tm_040;
    public string pm01_raw_040;
    public string pm25_raw_040;
    public string pm10_raw_040;
    public string temp_040;
    public string humi_040;
    public string co2_040;
    public string voc_040;
    public string noise_040;
    public string pm10_040;
    public string pm25_040;
    public string pm01_040;
    public string cici_pm10_040;
    public string cici_pm25_040;
    public string cici_co2_040;
    public string cici_voc_040;
    public string ciai_040;
    public string cici_temp_040;
    public string cici_humi_040;
    public string cici_noise_040;
    public string cici_040;
    #endregion

    #region ICW0W2001039 동아 유치원 2층 나래반
    [Space(20f)]
    [Header("< ICW0W2001039 >")]
    public string key_039;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_039;
    public string datetime_039;

    [Space(2f)]
    [Header("- Data")]
    public string tm_039;
    public string pm01_raw_039;
    public string pm25_raw_039;
    public string pm10_raw_039;
    public string temp_039;
    public string humi_039;
    public string co2_039;
    public string voc_039;
    public string noise_039;
    public string pm10_039;
    public string pm25_039;
    public string pm01_039;
    public string cici_pm10_039;
    public string cici_pm25_039;
    public string cici_co2_039;
    public string cici_voc_039;
    public string ciai_039;
    public string cici_temp_039;
    public string cici_humi_039;
    public string cici_noise_039;
    public string cici_039;
    #endregion

    #region ICW0W2001038 동아 유치원 2층 가온반
    [Space(20f)]
    [Header("< ICW0W2001038 >")]
    public string key_038;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_038;
    public string datetime_038;

    [Space(2f)]
    [Header("- Data")]
    public string tm_038;
    public string pm01_raw_038;
    public string pm25_raw_038;
    public string pm10_raw_038;
    public string temp_038;
    public string humi_038;
    public string co2_038;
    public string voc_038;
    public string noise_038;
    public string pm10_038;
    public string pm25_038;
    public string pm01_038;
    public string cici_pm10_038;
    public string cici_pm25_038;
    public string cici_co2_038;
    public string cici_voc_038;
    public string ciai_038;
    public string cici_temp_038;
    public string cici_humi_038;
    public string cici_noise_038;
    public string cici_038;
    #endregion

    #region ICW0W2001036 동아 유치원 지하 강당 벽면
    [Space(20f)]
    [Header("< ICW0W2001036 >")]
    public string key_036;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_036;
    public string datetime_036;

    [Space(2f)]
    [Header("- Data")]
    public string tm_036;
    public string pm01_raw_036;
    public string pm25_raw_036;
    public string pm10_raw_036;
    public string temp_036;
    public string humi_036;
    public string co2_036;
    public string voc_036;
    public string noise_036;
    public string pm10_036;
    public string pm25_036;
    public string pm01_036;
    public string cici_pm10_036;
    public string cici_pm25_036;
    public string cici_co2_036;
    public string cici_voc_036;
    public string ciai_036;
    public string cici_temp_036;
    public string cici_humi_036;
    public string cici_noise_036;
    public string cici_036;
    #endregion

    #region ICW0W2001037 동아 유치원 지하 강당 놀이터
    [Space(20f)]
    [Header("< ICW0W2001037 >")]
    public string key_037;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_037;
    public string datetime_037;

    [Space(2f)]
    [Header("- Data")]
    public string tm_037;
    public string pm01_raw_037;
    public string pm25_raw_037;
    public string pm10_raw_037;
    public string temp_037;
    public string humi_037;
    public string co2_037;
    public string voc_037;
    public string noise_037;
    public string pm10_037;
    public string pm25_037;
    public string pm01_037;
    public string cici_pm10_037;
    public string cici_pm25_037;
    public string cici_co2_037;
    public string cici_voc_037;
    public string ciai_037;
    public string cici_temp_037;
    public string cici_humi_037;
    public string cici_noise_037;
    public string cici_037;
    #endregion



    #region ICL1L2000064 신도림 초등학교 체육관 강연대
    [Space(20f)]
    [Header("< ICL1L2000064 >")]
    public string key_064;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_064;
    public string datetime_064;

    [Space(2f)]
    [Header("- Data")]
    public string tm_064;
    public string pm01_raw_064;
    public string pm25_raw_064;
    public string pm10_raw_064;
    public string temp_064;
    public string humi_064;
    public string co2_064;
    public string voc_064;
    public string noise_064;
    public string pm10_064;
    public string pm25_064;
    public string pm01_064;
    public string cici_pm10_064;
    public string cici_pm25_064;
    public string cici_co2_064;
    public string cici_voc_064;
    public string ciai_064;
    public string cici_temp_064;
    public string cici_humi_064;
    public string cici_noise_064;
    public string cici_064;
    #endregion

    #region ISCKL2200001 신도림초등학교 뒷면 입구 쪽(교체)
    [Space(20f)]
    [Header("< ISCKL2200001 >")]
    public string key_001;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_001;
    public string datetime_001;

    [Space(2f)]
    [Header("- Data")]
    public string tm_001;
    public string pm01_raw_001;
    public string pm25_raw_001;
    public string pm10_raw_001;
    public string temp_001;
    public string humi_001;
    public string co2_001;
    public string voc_001;
    public string noise_001;
    public string pm10_001;
    public string pm25_001;
    public string pm01_001;
    public string cici_pm10_001;
    public string cici_pm25_001;
    public string cici_co2_001;
    public string cici_voc_001;
    public string ciai_001;
    public string cici_temp_001;
    public string cici_humi_001;
    public string cici_noise_001;
    public string cici_001;
    #endregion

    #region ISC0W2000003 신도림 초등학교 운동장 입구 쪽
    [Space(20f)]
    [Header("< ISC0W2000003 >")]
    public string key_003;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_003;
    public string datetime_003;

    [Space(2f)]
    [Header("- Data")]
    public string tm_003;
    public string pm01_raw_003;
    public string pm25_raw_003;
    public string pm10_raw_003;
    public string temp_003;
    public string humi_003;
    public string co2_003;
    public string voc_003;
    public string noise_003;
    public string pm10_003;
    public string pm25_003;
    public string pm01_003;
    public string cici_pm10_003;
    public string cici_pm25_003;
    public string cici_co2_003;
    public string cici_voc_003;
    public string ciai_003;
    public string cici_temp_003;
    public string cici_humi_003;
    public string cici_noise_003;
    public string cici_003;
    #endregion

    #region ISC0W2000002 신도림 초등학교 운동장 강연대 쪽
    [Space(20f)]
    [Header("< ISC0W2000002 >")]
    public string key_002;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_002;
    public string datetime_002;

    [Space(2f)]
    [Header("- Data")]
    public string tm_002;
    public string pm01_raw_002;
    public string pm25_raw_002;
    public string pm10_raw_002;
    public string temp_002;
    public string humi_002;
    public string co2_002;
    public string voc_002;
    public string noise_002;
    public string pm10_002;
    public string pm25_002;
    public string pm01_002;
    public string cici_pm10_002;
    public string cici_pm25_002;
    public string cici_co2_002;
    public string cici_voc_002;
    public string ciai_002;
    public string cici_temp_002;
    public string cici_humi_002;
    public string cici_noise_002;
    public string cici_002;
    #endregion



    #region ICL1L2100450 목일 중학교 체육관 입구 쪽(3차교체)
    [Space(20f)]
    [Header("< ICL1L2100450 >")]
    public string key_450;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_450;
    public string datetime_450;

    [Space(2f)]
    [Header("- Data")]
    public string tm_450;
    public string pm01_raw_450;
    public string pm25_raw_450;
    public string pm10_raw_450;
    public string temp_450;
    public string humi_450;
    public string co2_450;
    public string voc_450;
    public string noise_450;
    public string pm10_450;
    public string pm25_450;
    public string pm01_450;
    public string cici_pm10_450;
    public string cici_pm25_450;
    public string cici_co2_450;
    public string cici_voc_450;
    public string ciai_450;
    public string cici_temp_450;
    public string cici_humi_450;
    public string cici_noise_450;
    public string cici_450;
    #endregion

    #region ICL1L2100449 목일 중학교 체육관 강연대 쪽 (2차교체)
    [Space(20f)]
    [Header("< ICL1L2100449 >")]
    public string key_449;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_449;
    public string datetime_449;

    [Space(2f)]
    [Header("- Data")]
    public string tm_449;
    public string pm01_raw_449;
    public string pm25_raw_449;
    public string pm10_raw_449;
    public string temp_449;
    public string humi_449;
    public string co2_449;
    public string voc_449;
    public string noise_449;
    public string pm10_449;
    public string pm25_449;
    public string pm01_449;
    public string cici_pm10_449;
    public string cici_pm25_449;
    public string cici_co2_449;
    public string cici_voc_449;
    public string ciai_449;
    public string cici_temp_449;
    public string cici_humi_449;
    public string cici_noise_449;
    public string cici_449;
    #endregion

    #region ICW0W2001044 목일 중학교 체육관 운동장 입구 쪽
    [Space(20f)]
    [Header("< ICW0W2001044 >")]
    public string key_044;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_044;
    public string datetime_044;

    [Space(2f)]
    [Header("- Data")]
    public string tm_044;
    public string pm01_raw_044;
    public string pm25_raw_044;
    public string pm10_raw_044;
    public string temp_044;
    public string humi_044;
    public string co2_044;
    public string voc_044;
    public string noise_044;
    public string pm10_044;
    public string pm25_044;
    public string pm01_044;
    public string cici_pm10_044;
    public string cici_pm25_044;
    public string cici_co2_044;
    public string cici_voc_044;
    public string ciai_044;
    public string cici_temp_044;
    public string cici_humi_044;
    public string cici_noise_044;
    public string cici_044;
    #endregion

    #region ICL1L2000065 목일 중학교 체육관 운동장 강연대 쪽
    [Space(20f)]
    [Header("< ICL1L2000065 >")]
    public string key_065;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_065;
    public string datetime_065;

    [Space(2f)]
    [Header("- Data")]
    public string tm_065;
    public string pm01_raw_065;
    public string pm25_raw_065;
    public string pm10_raw_065;
    public string temp_065;
    public string humi_065;
    public string co2_065;
    public string voc_065;
    public string noise_065;
    public string pm10_065;
    public string pm25_065;
    public string pm01_065;
    public string cici_pm10_065;
    public string cici_pm25_065;
    public string cici_co2_065;
    public string cici_voc_065;
    public string ciai_065;
    public string cici_temp_065;
    public string cici_humi_065;
    public string cici_noise_065;
    public string cici_065;
    #endregion



    #region ICW0W2001046
    [Space(2f)]
    [Header("< ICW0W2001046 >")]
    public string key_046;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_046;
    public string datetime_046;

    [Space(2f)]
    [Header("- Data")]
    public string tm_046;
    public string pm01_raw_046;
    public string pm25_raw_046;
    public string pm10_raw_046;
    public string temp_046;
    public string humi_046;
    public string co2_046;
    public string voc_046;
    public string noise_046;
    public string pm10_046;
    public string pm25_046;
    public string pm01_046;
    public string cici_pm10_046;
    public string cici_pm25_046;
    public string cici_co2_046;
    public string cici_voc_046;
    public string ciai_046;
    public string cici_temp_046;
    public string cici_humi_046;
    public string cici_noise_046;
    public string cici_046;
    #endregion

    #region ICW0W2001045
    [Space(20f)]
    [Header("< ICW0W2001045 >")]
    public string key_045;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_045;
    public string datetime_045;

    [Space(2f)]
    [Header("- Data")]
    public string tm_045;
    public string pm01_raw_045;
    public string pm25_raw_045;
    public string pm10_raw_045;
    public string temp_045;
    public string humi_045;
    public string co2_045;
    public string voc_045;
    public string noise_045;
    public string pm10_045;
    public string pm25_045;
    public string pm01_045;
    public string cici_pm10_045;
    public string cici_pm25_045;
    public string cici_co2_045;
    public string cici_voc_045;
    public string ciai_045;
    public string cici_temp_045;
    public string cici_humi_045;
    public string cici_noise_045;
    public string cici_045;
    #endregion

    #region ICW0W2001043
    [Space(20f)]
    [Header("< ICW0W2001043 >")]
    public string key_043;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_043;
    public string datetime_043;

    [Space(2f)]
    [Header("- Data")]
    public string tm_043;
    public string pm01_raw_043;
    public string pm25_raw_043;
    public string pm10_raw_043;
    public string temp_043;
    public string humi_043;
    public string co2_043;
    public string voc_043;
    public string noise_043;
    public string pm10_043;
    public string pm25_043;
    public string pm01_043;
    public string cici_pm10_043;
    public string cici_pm25_043;
    public string cici_co2_043;
    public string cici_voc_043;
    public string ciai_043;
    public string cici_temp_043;
    public string cici_humi_043;
    public string cici_noise_043;
    public string cici_043;
    #endregion

    #region ICW0W2001042
    [Space(20f)]
    [Header("< ICW0W2001042 >")]
    public string key_042;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_042;
    public string datetime_042;

    [Space(2f)]
    [Header("- Data")]
    public string tm_042;
    public string pm01_raw_042;
    public string pm25_raw_042;
    public string pm10_raw_042;
    public string temp_042;
    public string humi_042;
    public string co2_042;
    public string voc_042;
    public string noise_042;
    public string pm10_042;
    public string pm25_042;
    public string pm01_042;
    public string cici_pm10_042;
    public string cici_pm25_042;
    public string cici_co2_042;
    public string cici_voc_042;
    public string ciai_042;
    public string cici_temp_042;
    public string cici_humi_042;
    public string cici_noise_042;
    public string cici_042;
    #endregion

    #region ISC0W2000006
    [Space(20f)]
    [Header("< ISC0W2000006 >")]
    public string key_006;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_006;
    public string datetime_006;

    [Space(2f)]
    [Header("- Data")]
    public string tm_006;
    public string pm01_raw_006;
    public string pm25_raw_006;
    public string pm10_raw_006;
    public string temp_006;
    public string humi_006;
    public string co2_006;
    public string voc_006;
    public string noise_006;
    public string pm10_006;
    public string pm25_006;
    public string pm01_006;
    public string cici_pm10_006;
    public string cici_pm25_006;
    public string cici_co2_006;
    public string cici_voc_006;
    public string ciai_006;
    public string cici_temp_006;
    public string cici_humi_006;
    public string cici_noise_006;
    public string cici_006;
    #endregion

    #region ICW0W2001041
    [Space(20f)]
    [Header("< ICW0W2001041 >")]
    public string key_041;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_041;
    public string datetime_041;

    [Space(2f)]
    [Header("- Data")]
    public string tm_041;
    public string pm01_raw_041;
    public string pm25_raw_041;
    public string pm10_raw_041;
    public string temp_041;
    public string humi_041;
    public string co2_041;
    public string voc_041;
    public string noise_041;
    public string pm10_041;
    public string pm25_041;
    public string pm01_041;
    public string cici_pm10_041;
    public string cici_pm25_041;
    public string cici_co2_041;
    public string cici_voc_041;
    public string ciai_041;
    public string cici_temp_041;
    public string cici_humi_041;
    public string cici_noise_041;
    public string cici_041;
    #endregion

    #region ISC0W2000007
    [Space(20f)]
    [Header("< ISC0W2000007 >")]
    public string key_007;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_007;
    public string datetime_007;

    [Space(2f)]
    [Header("- Data")]
    public string tm_007;
    public string pm01_raw_007;
    public string pm25_raw_007;
    public string pm10_raw_007;
    public string temp_007;
    public string humi_007;
    public string co2_007;
    public string voc_007;
    public string noise_007;
    public string pm10_007;
    public string pm25_007;
    public string pm01_007;
    public string cici_pm10_007;
    public string cici_pm25_007;
    public string cici_co2_007;
    public string cici_voc_007;
    public string ciai_007;
    public string cici_temp_007;
    public string cici_humi_007;
    public string cici_noise_007;
    public string cici_007;
    #endregion

    #region ISC0W2000004
    [Space(20f)]
    [Header("< ISC0W2000004 >")]
    public string key_004;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_004;
    public string datetime_004;

    [Space(2f)]
    [Header("- Data")]
    public string tm_004;
    public string pm01_raw_004;
    public string pm25_raw_004;
    public string pm10_raw_004;
    public string temp_004;
    public string humi_004;
    public string co2_004;
    public string voc_004;
    public string noise_004;
    public string pm10_004;
    public string pm25_004;
    public string pm01_004;
    public string cici_pm10_004;
    public string cici_pm25_004;
    public string cici_co2_004;
    public string cici_voc_004;
    public string ciai_004;
    public string cici_temp_004;
    public string cici_humi_004;
    public string cici_noise_004;
    public string cici_004;
    #endregion

    #region ISC0W2000005
    [Space(20f)]
    [Header("< ISC0W2000005 >")]
    public string key_005;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_005;
    public string datetime_005;

    [Space(2f)]
    [Header("- Data")]
    public string tm_005;
    public string pm01_raw_005;
    public string pm25_raw_005;
    public string pm10_raw_005;
    public string temp_005;
    public string humi_005;
    public string co2_005;
    public string voc_005;
    public string noise_005;
    public string pm10_005;
    public string pm25_005;
    public string pm01_005;
    public string cici_pm10_005;
    public string cici_pm25_005;
    public string cici_co2_005;
    public string cici_voc_005;
    public string ciai_005;
    public string cici_temp_005;
    public string cici_humi_005;
    public string cici_noise_005;
    public string cici_005;
    #endregion

    #region ISC0W2000020
    [Space(20f)]
    [Header("< ISC0W2000020 >")]
    public string key_020;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_020;
    public string datetime_020;

    [Space(2f)]
    [Header("- Data")]
    public string tm_020;
    public string pm01_raw_020;
    public string pm25_raw_020;
    public string pm10_raw_020;
    public string temp_020;
    public string humi_020;
    public string co2_020;
    public string voc_020;
    public string noise_020;
    public string pm10_020;
    public string pm25_020;
    public string pm01_020;
    public string cici_pm10_020;
    public string cici_pm25_020;
    public string cici_co2_020;
    public string cici_voc_020;
    public string ciai_020;
    public string cici_temp_020;
    public string cici_humi_020;
    public string cici_noise_020;
    public string cici_020;
    #endregion

    #region 000000000000
    [Space(20f)]
    [Header("< 000000000000 >")]
    public string key_000;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_000;
    public string datetime_000;

    [Space(2f)]
    [Header("- Data")]
    public string tm_000;
    public string pm01_raw_000;
    public string pm25_raw_000;
    public string pm10_raw_000;
    public string temp_000;
    public string humi_000;
    public string co2_000;
    public string voc_000;
    public string noise_000;
    public string pm10_000;
    public string pm25_000;
    public string pm01_000;
    public string cici_pm10_000;
    public string cici_pm25_000;
    public string cici_co2_000;
    public string cici_voc_000;
    public string ciai_000;
    public string cici_temp_000;
    public string cici_humi_000;
    public string cici_noise_000;
    public string cici_000;
    #endregion

    #region ISC0W1800005
    [Space(20f)]
    [Header("< ISC0W1800005 >")]
    public string key_0005;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_0005;
    public string datetime_0005;

    [Space(2f)]
    [Header("- Data")]
    public string tm_0005;
    public string pm01_raw_0005;
    public string pm25_raw_0005;
    public string pm10_raw_0005;
    public string temp_0005;
    public string humi_0005;
    public string co2_0005;
    public string voc_0005;
    public string noise_0005;
    public string pm10_0005;
    public string pm25_0005;
    public string pm01_0005;
    public string cici_pm10_0005;
    public string cici_pm25_0005;
    public string cici_co2_0005;
    public string cici_voc_0005;
    public string ciai_0005;
    public string cici_temp_0005;
    public string cici_humi_0005;
    public string cici_noise_0005;
    public string cici_0005;
    #endregion

    #region ISC0W1800000
    [Space(20f)]
    [Header("< ISC0W1800000 >")]
    public string key_0000;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_0000;
    public string datetime_0000;

    [Space(2f)]
    [Header("- Data")]
    public string tm_0000;
    public string pm01_raw_0000;
    public string pm25_raw_0000;
    public string pm10_raw_0000;
    public string temp_0000;
    public string humi_0000;
    public string co2_0000;
    public string voc_0000;
    public string noise_0000;
    public string pm10_0000;
    public string pm25_0000;
    public string pm01_0000;
    public string cici_pm10_0000;
    public string cici_pm25_0000;
    public string cici_co2_0000;
    public string cici_voc_0000;
    public string ciai_0000;
    public string cici_temp_0000;
    public string cici_humi_0000;
    public string cici_noise_0000;
    public string cici_0000;
    #endregion

    #region ICW0W2100604
    [Space(20f)]
    [Header("< ICW0W2100604 >")]
    public string key_604;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_604;
    public string datetime_604;

    [Space(2f)]
    [Header("- Data")]
    public string tm_604;
    public string pm01_raw_604;
    public string pm25_raw_604;
    public string pm10_raw_604;
    public string temp_604;
    public string humi_604;
    public string co2_604;
    public string voc_604;
    public string noise_604;
    public string pm10_604;
    public string pm25_604;
    public string pm01_604;
    public string cici_pm10_604;
    public string cici_pm25_604;
    public string cici_co2_604;
    public string cici_voc_604;
    public string ciai_604;
    public string cici_temp_604;
    public string cici_humi_604;
    public string cici_noise_604;
    public string cici_604;
    #endregion

    #region ISC0W1800002
    [Space(20f)]
    [Header("< ISC0W1800002 >")]
    public string key_0002;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_0002;
    public string datetime_0002;

    [Space(2f)]
    [Header("- Data")]
    public string tm_0002;
    public string pm01_raw_0002;
    public string pm25_raw_0002;
    public string pm10_raw_0002;
    public string temp_0002;
    public string humi_0002;
    public string co2_0002;
    public string voc_0002;
    public string noise_0002;
    public string pm10_0002;
    public string pm25_0002;
    public string pm01_0002;
    public string cici_pm10_0002;
    public string cici_pm25_0002;
    public string cici_co2_0002;
    public string cici_voc_0002;
    public string ciai_0002;
    public string cici_temp_0002;
    public string cici_humi_0002;
    public string cici_noise_0002;
    public string cici_0002;
    #endregion

    #region ISC0W1800001
    [Space(20f)]
    [Header("< ISC0W1800001 >")]
    public string key_0001;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_0001;
    public string datetime_0001;

    [Space(2f)]
    [Header("- Data")]
    public string tm_0001;
    public string pm01_raw_0001;
    public string pm25_raw_0001;
    public string pm10_raw_0001;
    public string temp_0001;
    public string humi_0001;
    public string co2_0001;
    public string voc_0001;
    public string noise_0001;
    public string pm10_0001;
    public string pm25_0001;
    public string pm01_0001;
    public string cici_pm10_0001;
    public string cici_pm25_0001;
    public string cici_co2_0001;
    public string cici_voc_0001;
    public string ciai_0001;
    public string cici_temp_0001;
    public string cici_humi_0001;
    public string cici_noise_0001;
    public string cici_0001;
    #endregion

    #region ICW1W2000011
    [Space(20f)]
    [Header("< ISC0W1800001 >")]
    public string key_011;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_011;
    public string datetime_011;

    [Space(2f)]
    [Header("- Data")]
    public string tm_011;
    public string pm01_raw_011;
    public string pm25_raw_011;
    public string pm10_raw_011;
    public string temp_011;
    public string humi_011;
    public string co2_011;
    public string voc_011;
    public string noise_011;
    public string pm10_011;
    public string pm25_011;
    public string pm01_011;
    public string cici_pm10_011;
    public string cici_pm25_011;
    public string cici_co2_011;
    public string cici_voc_011;
    public string ciai_011;
    public string cici_temp_011;
    public string cici_humi_011;
    public string cici_noise_011;
    public string cici_011;
    #endregion

    #region ICW0W2001092
    [Space(20f)]
    [Header("< ICW0W2001092 >")]
    public string key_092;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_092;
    public string datetime_092;

    [Space(2f)]
    [Header("- Data")]
    public string tm_092;
    public string pm01_raw_092;
    public string pm25_raw_092;
    public string pm10_raw_092;
    public string temp_092;
    public string humi_092;
    public string co2_092;
    public string voc_092;
    public string noise_092;
    public string pm10_092;
    public string pm25_092;
    public string pm01_092;
    public string cici_pm10_092;
    public string cici_pm25_092;
    public string cici_co2_092;
    public string cici_voc_092;
    public string ciai_092;
    public string cici_temp_092;
    public string cici_humi_092;
    public string cici_noise_092;
    public string cici_092;
    #endregion

    #region ICW0W1800001
    [Space(20f)]
    [Header("< ICW0W1800001 >")]
    public string key_00001;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_00001;
    public string datetime_00001;

    [Space(2f)]
    [Header("- Data")]
    public string tm_00001;
    public string pm01_raw_00001;
    public string pm25_raw_00001;
    public string pm10_raw_00001;
    public string temp_00001;
    public string humi_00001;
    public string co2_00001;
    public string voc_00001;
    public string noise_00001;
    public string pm10_00001;
    public string pm25_00001;
    public string pm01_00001;
    public string cici_pm10_00001;
    public string cici_pm25_00001;
    public string cici_co2_00001;
    public string cici_voc_00001;
    public string ciai_00001;
    public string cici_temp_00001;
    public string cici_humi_00001;
    public string cici_noise_00001;
    public string cici_00001;
    #endregion

    #region ICW0W1800000
    [Space(20f)]
    [Header("< ICW0W1800000 >")]
    public string key_00000;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_00000;
    public string datetime_00000;

    [Space(2f)]
    [Header("- Data")]
    public string tm_00000;
    public string pm01_raw_00000;
    public string pm25_raw_00000;
    public string pm10_raw_00000;
    public string temp_00000;
    public string humi_00000;
    public string co2_00000;
    public string voc_00000;
    public string noise_00000;
    public string pm10_00000;
    public string pm25_00000;
    public string pm01_00000;
    public string cici_pm10_00000;
    public string cici_pm25_00000;
    public string cici_co2_00000;
    public string cici_voc_00000;
    public string ciai_00000;
    public string cici_temp_00000;
    public string cici_humi_00000;
    public string cici_noise_00000;
    public string cici_00000;
    #endregion

    #region ISC0W2000016
    [Space(20f)]
    [Header("< ISC0W2000016 >")]
    public string key_016;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_016;
    public string datetime_016;

    [Space(2f)]
    [Header("- Data")]
    public string tm_016;
    public string pm01_raw_016;
    public string pm25_raw_016;
    public string pm10_raw_016;
    public string temp_016;
    public string humi_016;
    public string co2_016;
    public string voc_016;
    public string noise_016;
    public string pm10_016;
    public string pm25_016;
    public string pm01_016;
    public string cici_pm10_016;
    public string cici_pm25_016;
    public string cici_co2_016;
    public string cici_voc_016;
    public string ciai_016;
    public string cici_temp_016;
    public string cici_humi_016;
    public string cici_noise_016;
    public string cici_016;
    #endregion

    #region ISC0W1800015
    [Space(20f)]
    [Header("< ISC0W1800015 >")]
    public string key_015;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_015;
    public string datetime_015;

    [Space(2f)]
    [Header("- Data")]
    public string tm_015;
    public string pm01_raw_015;
    public string pm25_raw_015;
    public string pm10_raw_015;
    public string temp_015;
    public string humi_015;
    public string co2_015;
    public string voc_015;
    public string noise_015;
    public string pm10_015;
    public string pm25_015;
    public string pm01_015;
    public string cici_pm10_015;
    public string cici_pm25_015;
    public string cici_co2_015;
    public string cici_voc_015;
    public string ciai_015;
    public string cici_temp_015;
    public string cici_humi_015;
    public string cici_noise_015;
    public string cici_015;
    #endregion

    #region ISC0W1800014
    [Space(20f)]
    [Header("< ISC0W1800014 >")]
    public string key_014;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_014;
    public string datetime_014;

    [Space(2f)]
    [Header("- Data")]
    public string tm_014;
    public string pm01_raw_014;
    public string pm25_raw_014;
    public string pm10_raw_014;
    public string temp_014;
    public string humi_014;
    public string co2_014;
    public string voc_014;
    public string noise_014;
    public string pm10_014;
    public string pm25_014;
    public string pm01_014;
    public string cici_pm10_014;
    public string cici_pm25_014;
    public string cici_co2_014;
    public string cici_voc_014;
    public string ciai_014;
    public string cici_temp_014;
    public string cici_humi_014;
    public string cici_noise_014;
    public string cici_014;
    #endregion

    #region ISC0W1800011
    [Space(20f)]
    [Header("< ISC0W1800011 >")]
    public string key_0011;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_0011;
    public string datetime_0011;

    [Space(2f)]
    [Header("- Data")]
    public string tm_0011;
    public string pm01_raw_0011;
    public string pm25_raw_0011;
    public string pm10_raw_0011;
    public string temp_0011;
    public string humi_0011;
    public string co2_0011;
    public string voc_0011;
    public string noise_0011;
    public string pm10_0011;
    public string pm25_0011;
    public string pm01_0011;
    public string cici_pm10_0011;
    public string cici_pm25_0011;
    public string cici_co2_0011;
    public string cici_voc_0011;
    public string ciai_0011;
    public string cici_temp_0011;
    public string cici_humi_0011;
    public string cici_noise_0011;
    public string cici_0011;
    #endregion

    #region ISC0W1800010
    [Space(20f)]
    [Header("< ISC0W1800010 >")]
    public string key_0010;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_0010;
    public string datetime_0010;

    [Space(2f)]
    [Header("- Data")]
    public string tm_0010;
    public string pm01_raw_0010;
    public string pm25_raw_0010;
    public string pm10_raw_0010;
    public string temp_0010;
    public string humi_0010;
    public string co2_0010;
    public string voc_0010;
    public string noise_0010;
    public string pm10_0010;
    public string pm25_0010;
    public string pm01_0010;
    public string cici_pm10_0010;
    public string cici_pm25_0010;
    public string cici_co2_0010;
    public string cici_voc_0010;
    public string ciai_0010;
    public string cici_temp_0010;
    public string cici_humi_0010;
    public string cici_noise_0010;
    public string cici_0010;
    #endregion

    #region ISC0W1800013
    [Space(20f)]
    [Header("< ISC0W1800013 >")]
    public string key_0013;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_0013;
    public string datetime_0013;

    [Space(2f)]
    [Header("- Data")]
    public string tm_0013;
    public string pm01_raw_0013;
    public string pm25_raw_0013;
    public string pm10_raw_0013;
    public string temp_0013;
    public string humi_0013;
    public string co2_0013;
    public string voc_0013;
    public string noise_0013;
    public string pm10_0013;
    public string pm25_0013;
    public string pm01_0013;
    public string cici_pm10_0013;
    public string cici_pm25_0013;
    public string cici_co2_0013;
    public string cici_voc_0013;
    public string ciai_0013;
    public string cici_temp_0013;
    public string cici_humi_0013;
    public string cici_noise_0013;
    public string cici_0013;
    #endregion

    #region ISC0W1800012
    [Space(20f)]
    [Header("< ISC0W1800012 >")]
    public string key_0012;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_0012;
    public string datetime_0012;

    [Space(2f)]
    [Header("- Data")]
    public string tm_0012;
    public string pm01_raw_0012;
    public string pm25_raw_0012;
    public string pm10_raw_0012;
    public string temp_0012;
    public string humi_0012;
    public string co2_0012;
    public string voc_0012;
    public string noise_0012;
    public string pm10_0012;
    public string pm25_0012;
    public string pm01_0012;
    public string cici_pm10_0012;
    public string cici_pm25_0012;
    public string cici_co2_0012;
    public string cici_voc_0012;
    public string ciai_0012;
    public string cici_temp_0012;
    public string cici_humi_0012;
    public string cici_noise_0012;
    public string cici_0012;
    #endregion

    #region ISC0W18000010
    [Space(20f)]
    [Header("< ISC0W18000010 >")]
    public string key_00010;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_00010;
    public string datetime_00010;

    [Space(2f)]
    [Header("- Data")]
    public string tm_00010;
    public string pm01_raw_00010;
    public string pm25_raw_00010;
    public string pm10_raw_00010;
    public string temp_00010;
    public string humi_00010;
    public string co2_00010;
    public string voc_00010;
    public string noise_00010;
    public string pm10_00010;
    public string pm25_00010;
    public string pm01_00010;
    public string cici_pm10_00010;
    public string cici_pm25_00010;
    public string cici_co2_00010;
    public string cici_voc_00010;
    public string ciai_00010;
    public string cici_temp_00010;
    public string cici_humi_00010;
    public string cici_noise_00010;
    public string cici_00010;
    #endregion

    #region ICW0W2100006
    [Space(20f)]
    [Header("< ICW0W2100006 >")]
    public string key_0006;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_0006;
    public string datetime_0006;

    [Space(2f)]
    [Header("- Data")]
    public string tm_0006;
    public string pm01_raw_0006;
    public string pm25_raw_0006;
    public string pm10_raw_0006;
    public string temp_0006;
    public string humi_0006;
    public string co2_0006;
    public string voc_0006;
    public string noise_0006;
    public string pm10_0006;
    public string pm25_0006;
    public string pm01_0006;
    public string cici_pm10_0006;
    public string cici_pm25_0006;
    public string cici_co2_0006;
    public string cici_voc_0006;
    public string ciai_0006;
    public string cici_temp_0006;
    public string cici_humi_0006;
    public string cici_noise_0006;
    public string cici_0006;
    #endregion

    #region ICW0W2200121
    [Space(20f)]
    [Header("< ICW0W2100006 >")]
    public string key_121;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_121;
    public string datetime_121;

    [Space(2f)]
    [Header("- Data")]
    public string tm_121;
    public string pm01_raw_121;
    public string pm25_raw_121;
    public string pm10_raw_121;
    public string temp_121;
    public string humi_121;
    public string co2_121;
    public string voc_121;
    public string noise_121;
    public string pm10_121;
    public string pm25_121;
    public string pm01_121;
    public string cici_pm10_121;
    public string cici_pm25_121;
    public string cici_co2_121;
    public string cici_voc_121;
    public string ciai_121;
    public string cici_temp_121;
    public string cici_humi_121;
    public string cici_noise_121;
    public string cici_121;
    #endregion

    #region IBOKW2200025
    [Space(20f)]
    [Header("< IBOKW2200025 >")]
    public string key_025;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_025;
    public string datetime_025;

    [Space(2f)]
    [Header("- Data")]
    public string tm_025;
    public string pm01_raw_025;
    public string pm25_raw_025;
    public string pm10_raw_025;
    public string temp_025;
    public string humi_025;
    public string co2_025;
    public string voc_025;
    public string noise_025;
    public string pm10_025;
    public string pm25_025;
    public string pm01_025;
    public string cici_pm10_025;
    public string cici_pm25_025;
    public string cici_co2_025;
    public string cici_voc_025;
    public string ciai_025;
    public string cici_temp_025;
    public string cici_humi_025;
    public string cici_noise_025;
    public string cici_025;
    #endregion

    #region IBOKW2200028
    [Space(20f)]
    [Header("< IBOKW2200028 >")]
    public string key_028;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_028;
    public string datetime_028;

    [Space(2f)]
    [Header("- Data")]
    public string tm_028;
    public string pm01_raw_028;
    public string pm25_raw_028;
    public string pm10_raw_028;
    public string temp_028;
    public string humi_028;
    public string co2_028;
    public string voc_028;
    public string noise_028;
    public string pm10_028;
    public string pm25_028;
    public string pm01_028;
    public string cici_pm10_028;
    public string cici_pm25_028;
    public string cici_co2_028;
    public string cici_voc_028;
    public string ciai_028;
    public string cici_temp_028;
    public string cici_humi_028;
    public string cici_noise_028;
    public string cici_028;
    #endregion

    #region IBOKW2200033
    [Space(20f)]
    [Header("< IBOKW2200033 >")]
    public string key_033;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_033;
    public string datetime_033;

    [Space(2f)]
    [Header("- Data")]
    public string tm_033;
    public string pm01_raw_033;
    public string pm25_raw_033;
    public string pm10_raw_033;
    public string temp_033;
    public string humi_033;
    public string co2_033;
    public string voc_033;
    public string noise_033;
    public string pm10_033;
    public string pm25_033;
    public string pm01_033;
    public string cici_pm10_033;
    public string cici_pm25_033;
    public string cici_co2_033;
    public string cici_voc_033;
    public string ciai_033;
    public string cici_temp_033;
    public string cici_humi_033;
    public string cici_noise_033;
    public string cici_033;
    #endregion

    #region IBOKW2200024
    [Space(20f)]
    [Header("< IBOKW2200024 >")]
    public string key_024;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_024;
    public string datetime_024;

    [Space(2f)]
    [Header("- Data")]
    public string tm_024;
    public string pm01_raw_024;
    public string pm25_raw_024;
    public string pm10_raw_024;
    public string temp_024;
    public string humi_024;
    public string co2_024;
    public string voc_024;
    public string noise_024;
    public string pm10_024;
    public string pm25_024;
    public string pm01_024;
    public string cici_pm10_024;
    public string cici_pm25_024;
    public string cici_co2_024;
    public string cici_voc_024;
    public string ciai_024;
    public string cici_temp_024;
    public string cici_humi_024;
    public string cici_noise_024;
    public string cici_024;
    #endregion
    #endregion


    #region STATIC_CLASS
    // 동아 유치원 2층 라온반
    public static class ICW0W2001040
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Tm;
        public static string Pm01_raw;
        public static string Pm25_raw;
        public static string Pm10_raw;
        public static string Temp;
        public static string Humi;
        public static string Co2;
        public static string Voc;
        public static string Noise;
        public static string Pm10;
        public static string Pm25;
        public static string Pm01;
        public static string Cici_pm10;
        public static string Cici_pm25;
        public static string Cici_co2;
        public static string Cici_voc;
        public static string Ciai;
        public static string Cici_temp;
        public static string Cici_humi;
        public static string Cici_noise;
        public static string Cici;
    }

    // 동아 유치원 2층 나래반
    public static class ICW0W2001039
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Tm;
        public static string Pm01_raw;
        public static string Pm25_raw;
        public static string Pm10_raw;
        public static string Temp;
        public static string Humi;
        public static string Co2;
        public static string Voc;
        public static string Noise;
        public static string Pm10;
        public static string Pm25;
        public static string Pm01;
        public static string Cici_pm10;
        public static string Cici_pm25;
        public static string Cici_co2;
        public static string Cici_voc;
        public static string Ciai;
        public static string Cici_temp;
        public static string Cici_humi;
        public static string Cici_noise;
        public static string Cici;
    }

    // 동아 유치원 2층 가온반
    public static class ICW0W2001038
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Tm;
        public static string Pm01_raw;
        public static string Pm25_raw;
        public static string Pm10_raw;
        public static string Temp;
        public static string Humi;
        public static string Co2;
        public static string Voc;
        public static string Noise;
        public static string Pm10;
        public static string Pm25;
        public static string Pm01;
        public static string Cici_pm10;
        public static string Cici_pm25;
        public static string Cici_co2;
        public static string Cici_voc;
        public static string Ciai;
        public static string Cici_temp;
        public static string Cici_humi;
        public static string Cici_noise;
        public static string Cici;
    }

    // 동아 유치원 지하 강당 벽면
    public static class ICW0W2001036
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Tm;
        public static string Pm01_raw;
        public static string Pm25_raw;
        public static string Pm10_raw;
        public static string Temp;
        public static string Humi;
        public static string Co2;
        public static string Voc;
        public static string Noise;
        public static string Pm10;
        public static string Pm25;
        public static string Pm01;
        public static string Cici_pm10;
        public static string Cici_pm25;
        public static string Cici_co2;
        public static string Cici_voc;
        public static string Ciai;
        public static string Cici_temp;
        public static string Cici_humi;
        public static string Cici_noise;
        public static string Cici;
    }

    // 동아 유치원 지하 강당 놀이터
    public static class ICW0W2001037
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Tm;
        public static string Pm01_raw;
        public static string Pm25_raw;
        public static string Pm10_raw;
        public static string Temp;
        public static string Humi;
        public static string Co2;
        public static string Voc;
        public static string Noise;
        public static string Pm10;
        public static string Pm25;
        public static string Pm01;
        public static string Cici_pm10;
        public static string Cici_pm25;
        public static string Cici_co2;
        public static string Cici_voc;
        public static string Ciai;
        public static string Cici_temp;
        public static string Cici_humi;
        public static string Cici_noise;
        public static string Cici;
    }



    // 신도림 초등학교 체육관 강연대
    public static class ICL1L2000064
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Tm;
        public static string Pm01_raw;
        public static string Pm25_raw;
        public static string Pm10_raw;
        public static string Temp;
        public static string Humi;
        public static string Co2;
        public static string Voc;
        public static string Noise;
        public static string Pm10;
        public static string Pm25;
        public static string Pm01;
        public static string Cici_pm10;
        public static string Cici_pm25;
        public static string Cici_co2;
        public static string Cici_voc;
        public static string Ciai;
        public static string Cici_temp;
        public static string Cici_humi;
        public static string Cici_noise;
        public static string Cici;
    }

    // 신도림초등학교 뒷면 입구 쪽(교체)
    public static class ISCKL2200001
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Tm;
        public static string Pm01_raw;
        public static string Pm25_raw;
        public static string Pm10_raw;
        public static string Temp;
        public static string Humi;
        public static string Co2;
        public static string Voc;
        public static string Noise;
        public static string Pm10;
        public static string Pm25;
        public static string Pm01;
        public static string Cici_pm10;
        public static string Cici_pm25;
        public static string Cici_co2;
        public static string Cici_voc;
        public static string Ciai;
        public static string Cici_temp;
        public static string Cici_humi;
        public static string Cici_noise;
        public static string Cici;
    }

    // 신도림 초등학교 운동장 강연대 쪽
    public static class ISC0W2000002
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Tm;
        public static string Pm01_raw;
        public static string Pm25_raw;
        public static string Pm10_raw;
        public static string Temp;
        public static string Humi;
        public static string Co2;
        public static string Voc;
        public static string Noise;
        public static string Pm10;
        public static string Pm25;
        public static string Pm01;
        public static string Cici_pm10;
        public static string Cici_pm25;
        public static string Cici_co2;
        public static string Cici_voc;
        public static string Ciai;
        public static string Cici_temp;
        public static string Cici_humi;
        public static string Cici_noise;
        public static string Cici;
    }

    // 신도림 초등학교 운동장 입구 쪽
    public static class ISC0W2000003
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Tm;
        public static string Pm01_raw;
        public static string Pm25_raw;
        public static string Pm10_raw;
        public static string Temp;
        public static string Humi;
        public static string Co2;
        public static string Voc;
        public static string Noise;
        public static string Pm10;
        public static string Pm25;
        public static string Pm01;
        public static string Cici_pm10;
        public static string Cici_pm25;
        public static string Cici_co2;
        public static string Cici_voc;
        public static string Ciai;
        public static string Cici_temp;
        public static string Cici_humi;
        public static string Cici_noise;
        public static string Cici;
    }




    // 목일 중학교 체육관 운동장 강연대 쪽
    public static class ICL1L2000065
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Tm;
        public static string Pm01_raw;
        public static string Pm25_raw;
        public static string Pm10_raw;
        public static string Temp;
        public static string Humi;
        public static string Co2;
        public static string Voc;
        public static string Noise;
        public static string Pm10;
        public static string Pm25;
        public static string Pm01;
        public static string Cici_pm10;
        public static string Cici_pm25;
        public static string Cici_co2;
        public static string Cici_voc;
        public static string Ciai;
        public static string Cici_temp;
        public static string Cici_humi;
        public static string Cici_noise;
        public static string Cici;
    }

    // 목일 중학교 체육관 운동장 입구 쪽
    public static class ICW0W2001044
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Tm;
        public static string Pm01_raw;
        public static string Pm25_raw;
        public static string Pm10_raw;
        public static string Temp;
        public static string Humi;
        public static string Co2;
        public static string Voc;
        public static string Noise;
        public static string Pm10;
        public static string Pm25;
        public static string Pm01;
        public static string Cici_pm10;
        public static string Cici_pm25;
        public static string Cici_co2;
        public static string Cici_voc;
        public static string Ciai;
        public static string Cici_temp;
        public static string Cici_humi;
        public static string Cici_noise;
        public static string Cici;
    }

    // 목일 중학교 체육관 입구 쪽(3차교체)
    public static class ICL1L2100450
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Tm;
        public static string Pm01_raw;
        public static string Pm25_raw;
        public static string Pm10_raw;
        public static string Temp;
        public static string Humi;
        public static string Co2;
        public static string Voc;
        public static string Noise;
        public static string Pm10;
        public static string Pm25;
        public static string Pm01;
        public static string Cici_pm10;
        public static string Cici_pm25;
        public static string Cici_co2;
        public static string Cici_voc;
        public static string Ciai;
        public static string Cici_temp;
        public static string Cici_humi;
        public static string Cici_noise;
        public static string Cici;
    }

    // 목일 중학교 체육관 강연대 쪽 (2차교체)
    public static class ICL1L2100449
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Tm;
        public static string Pm01_raw;
        public static string Pm25_raw;
        public static string Pm10_raw;
        public static string Temp;
        public static string Humi;
        public static string Co2;
        public static string Voc;
        public static string Noise;
        public static string Pm10;
        public static string Pm25;
        public static string Pm01;
        public static string Cici_pm10;
        public static string Cici_pm25;
        public static string Cici_co2;
        public static string Cici_voc;
        public static string Ciai;
        public static string Cici_temp;
        public static string Cici_humi;
        public static string Cici_noise;
        public static string Cici;
    }





    public static class ICW0W2001043
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Tm;
        public static string Pm01_raw;
        public static string Pm25_raw;
        public static string Pm10_raw;
        public static string Temp;
        public static string Humi;
        public static string Co2;
        public static string Voc;
        public static string Noise;
        public static string Pm10;
        public static string Pm25;
        public static string Pm01;
        public static string Cici_pm10;
        public static string Cici_pm25;
        public static string Cici_co2;
        public static string Cici_voc;
        public static string Ciai;
        public static string Cici_temp;
        public static string Cici_humi;
        public static string Cici_noise;
        public static string Cici;
    }

    public static class ICW0W2001042
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Tm;
        public static string Pm01_raw;
        public static string Pm25_raw;
        public static string Pm10_raw;
        public static string Temp;
        public static string Humi;
        public static string Co2;
        public static string Voc;
        public static string Noise;
        public static string Pm10;
        public static string Pm25;
        public static string Pm01;
        public static string Cici_pm10;
        public static string Cici_pm25;
        public static string Cici_co2;
        public static string Cici_voc;
        public static string Ciai;
        public static string Cici_temp;
        public static string Cici_humi;
        public static string Cici_noise;
        public static string Cici;
    }

    public static class ISC0W2000006
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Tm;
        public static string Pm01_raw;
        public static string Pm25_raw;
        public static string Pm10_raw;
        public static string Temp;
        public static string Humi;
        public static string Co2;
        public static string Voc;
        public static string Noise;
        public static string Pm10;
        public static string Pm25;
        public static string Pm01;
        public static string Cici_pm10;
        public static string Cici_pm25;
        public static string Cici_co2;
        public static string Cici_voc;
        public static string Ciai;
        public static string Cici_temp;
        public static string Cici_humi;
        public static string Cici_noise;
        public static string Cici;
    }

    public static class ICW0W2001041
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Tm;
        public static string Pm01_raw;
        public static string Pm25_raw;
        public static string Pm10_raw;
        public static string Temp;
        public static string Humi;
        public static string Co2;
        public static string Voc;
        public static string Noise;
        public static string Pm10;
        public static string Pm25;
        public static string Pm01;
        public static string Cici_pm10;
        public static string Cici_pm25;
        public static string Cici_co2;
        public static string Cici_voc;
        public static string Ciai;
        public static string Cici_temp;
        public static string Cici_humi;
        public static string Cici_noise;
        public static string Cici;
    }

    public static class ISC0W2000007
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Tm;
        public static string Pm01_raw;
        public static string Pm25_raw;
        public static string Pm10_raw;
        public static string Temp;
        public static string Humi;
        public static string Co2;
        public static string Voc;
        public static string Noise;
        public static string Pm10;
        public static string Pm25;
        public static string Pm01;
        public static string Cici_pm10;
        public static string Cici_pm25;
        public static string Cici_co2;
        public static string Cici_voc;
        public static string Ciai;
        public static string Cici_temp;
        public static string Cici_humi;
        public static string Cici_noise;
        public static string Cici;
    }

    public static class ISC0W2000004
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Tm;
        public static string Pm01_raw;
        public static string Pm25_raw;
        public static string Pm10_raw;
        public static string Temp;
        public static string Humi;
        public static string Co2;
        public static string Voc;
        public static string Noise;
        public static string Pm10;
        public static string Pm25;
        public static string Pm01;
        public static string Cici_pm10;
        public static string Cici_pm25;
        public static string Cici_co2;
        public static string Cici_voc;
        public static string Ciai;
        public static string Cici_temp;
        public static string Cici_humi;
        public static string Cici_noise;
        public static string Cici;
    }

    public static class ISC0W2000005
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Tm;
        public static string Pm01_raw;
        public static string Pm25_raw;
        public static string Pm10_raw;
        public static string Temp;
        public static string Humi;
        public static string Co2;
        public static string Voc;
        public static string Noise;
        public static string Pm10;
        public static string Pm25;
        public static string Pm01;
        public static string Cici_pm10;
        public static string Cici_pm25;
        public static string Cici_co2;
        public static string Cici_voc;
        public static string Ciai;
        public static string Cici_temp;
        public static string Cici_humi;
        public static string Cici_noise;
        public static string Cici;
    }

    public static class ISC0W2000020
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Tm;
        public static string Pm01_raw;
        public static string Pm25_raw;
        public static string Pm10_raw;
        public static string Temp;
        public static string Humi;
        public static string Co2;
        public static string Voc;
        public static string Noise;
        public static string Pm10;
        public static string Pm25;
        public static string Pm01;
        public static string Cici_pm10;
        public static string Cici_pm25;
        public static string Cici_co2;
        public static string Cici_voc;
        public static string Ciai;
        public static string Cici_temp;
        public static string Cici_humi;
        public static string Cici_noise;
        public static string Cici;
    }

    public static class O00000000000
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Tm;
        public static string Pm01_raw;
        public static string Pm25_raw;
        public static string Pm10_raw;
        public static string Temp;
        public static string Humi;
        public static string Co2;
        public static string Voc;
        public static string Noise;
        public static string Pm10;
        public static string Pm25;
        public static string Pm01;
        public static string Cici_pm10;
        public static string Cici_pm25;
        public static string Cici_co2;
        public static string Cici_voc;
        public static string Ciai;
        public static string Cici_temp;
        public static string Cici_humi;
        public static string Cici_noise;
        public static string Cici;
    }

    public static class ISC0W1800005
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Tm;
        public static string Pm01_raw;
        public static string Pm25_raw;
        public static string Pm10_raw;
        public static string Temp;
        public static string Humi;
        public static string Co2;
        public static string Voc;
        public static string Noise;
        public static string Pm10;
        public static string Pm25;
        public static string Pm01;
        public static string Cici_pm10;
        public static string Cici_pm25;
        public static string Cici_co2;
        public static string Cici_voc;
        public static string Ciai;
        public static string Cici_temp;
        public static string Cici_humi;
        public static string Cici_noise;
        public static string Cici;
    }

    public static class ISC0W1800000
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Tm;
        public static string Pm01_raw;
        public static string Pm25_raw;
        public static string Pm10_raw;
        public static string Temp;
        public static string Humi;
        public static string Co2;
        public static string Voc;
        public static string Noise;
        public static string Pm10;
        public static string Pm25;
        public static string Pm01;
        public static string Cici_pm10;
        public static string Cici_pm25;
        public static string Cici_co2;
        public static string Cici_voc;
        public static string Ciai;
        public static string Cici_temp;
        public static string Cici_humi;
        public static string Cici_noise;
        public static string Cici;
    }

    public static class ICW0W2100604
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Tm;
        public static string Pm01_raw;
        public static string Pm25_raw;
        public static string Pm10_raw;
        public static string Temp;
        public static string Humi;
        public static string Co2;
        public static string Voc;
        public static string Noise;
        public static string Pm10;
        public static string Pm25;
        public static string Pm01;
        public static string Cici_pm10;
        public static string Cici_pm25;
        public static string Cici_co2;
        public static string Cici_voc;
        public static string Ciai;
        public static string Cici_temp;
        public static string Cici_humi;
        public static string Cici_noise;
        public static string Cici;
    }

    public static class ISC0W1800002
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Tm;
        public static string Pm01_raw;
        public static string Pm25_raw;
        public static string Pm10_raw;
        public static string Temp;
        public static string Humi;
        public static string Co2;
        public static string Voc;
        public static string Noise;
        public static string Pm10;
        public static string Pm25;
        public static string Pm01;
        public static string Cici_pm10;
        public static string Cici_pm25;
        public static string Cici_co2;
        public static string Cici_voc;
        public static string Ciai;
        public static string Cici_temp;
        public static string Cici_humi;
        public static string Cici_noise;
        public static string Cici;
    }

    public static class ISC0W1800001
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Tm;
        public static string Pm01_raw;
        public static string Pm25_raw;
        public static string Pm10_raw;
        public static string Temp;
        public static string Humi;
        public static string Co2;
        public static string Voc;
        public static string Noise;
        public static string Pm10;
        public static string Pm25;
        public static string Pm01;
        public static string Cici_pm10;
        public static string Cici_pm25;
        public static string Cici_co2;
        public static string Cici_voc;
        public static string Ciai;
        public static string Cici_temp;
        public static string Cici_humi;
        public static string Cici_noise;
        public static string Cici;
    }

    public static class ICW1W2000011
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Tm;
        public static string Pm01_raw;
        public static string Pm25_raw;
        public static string Pm10_raw;
        public static string Temp;
        public static string Humi;
        public static string Co2;
        public static string Voc;
        public static string Noise;
        public static string Pm10;
        public static string Pm25;
        public static string Pm01;
        public static string Cici_pm10;
        public static string Cici_pm25;
        public static string Cici_co2;
        public static string Cici_voc;
        public static string Ciai;
        public static string Cici_temp;
        public static string Cici_humi;
        public static string Cici_noise;
        public static string Cici;
    }

    public static class ICW0W2001092
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Tm;
        public static string Pm01_raw;
        public static string Pm25_raw;
        public static string Pm10_raw;
        public static string Temp;
        public static string Humi;
        public static string Co2;
        public static string Voc;
        public static string Noise;
        public static string Pm10;
        public static string Pm25;
        public static string Pm01;
        public static string Cici_pm10;
        public static string Cici_pm25;
        public static string Cici_co2;
        public static string Cici_voc;
        public static string Ciai;
        public static string Cici_temp;
        public static string Cici_humi;
        public static string Cici_noise;
        public static string Cici;
    }

    public static class ICW0W1800001
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Tm;
        public static string Pm01_raw;
        public static string Pm25_raw;
        public static string Pm10_raw;
        public static string Temp;
        public static string Humi;
        public static string Co2;
        public static string Voc;
        public static string Noise;
        public static string Pm10;
        public static string Pm25;
        public static string Pm01;
        public static string Cici_pm10;
        public static string Cici_pm25;
        public static string Cici_co2;
        public static string Cici_voc;
        public static string Ciai;
        public static string Cici_temp;
        public static string Cici_humi;
        public static string Cici_noise;
        public static string Cici;
    }

    public static class ICW0W1800000
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Tm;
        public static string Pm01_raw;
        public static string Pm25_raw;
        public static string Pm10_raw;
        public static string Temp;
        public static string Humi;
        public static string Co2;
        public static string Voc;
        public static string Noise;
        public static string Pm10;
        public static string Pm25;
        public static string Pm01;
        public static string Cici_pm10;
        public static string Cici_pm25;
        public static string Cici_co2;
        public static string Cici_voc;
        public static string Ciai;
        public static string Cici_temp;
        public static string Cici_humi;
        public static string Cici_noise;
        public static string Cici;
    }

    public static class ISC0W2000016
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Tm;
        public static string Pm01_raw;
        public static string Pm25_raw;
        public static string Pm10_raw;
        public static string Temp;
        public static string Humi;
        public static string Co2;
        public static string Voc;
        public static string Noise;
        public static string Pm10;
        public static string Pm25;
        public static string Pm01;
        public static string Cici_pm10;
        public static string Cici_pm25;
        public static string Cici_co2;
        public static string Cici_voc;
        public static string Ciai;
        public static string Cici_temp;
        public static string Cici_humi;
        public static string Cici_noise;
        public static string Cici;
    }

    public static class ISC0W1800015
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Tm;
        public static string Pm01_raw;
        public static string Pm25_raw;
        public static string Pm10_raw;
        public static string Temp;
        public static string Humi;
        public static string Co2;
        public static string Voc;
        public static string Noise;
        public static string Pm10;
        public static string Pm25;
        public static string Pm01;
        public static string Cici_pm10;
        public static string Cici_pm25;
        public static string Cici_co2;
        public static string Cici_voc;
        public static string Ciai;
        public static string Cici_temp;
        public static string Cici_humi;
        public static string Cici_noise;
        public static string Cici;
    }

    public static class ISC0W1800014
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Tm;
        public static string Pm01_raw;
        public static string Pm25_raw;
        public static string Pm10_raw;
        public static string Temp;
        public static string Humi;
        public static string Co2;
        public static string Voc;
        public static string Noise;
        public static string Pm10;
        public static string Pm25;
        public static string Pm01;
        public static string Cici_pm10;
        public static string Cici_pm25;
        public static string Cici_co2;
        public static string Cici_voc;
        public static string Ciai;
        public static string Cici_temp;
        public static string Cici_humi;
        public static string Cici_noise;
        public static string Cici;
    }

    public static class ISC0W1800011
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Tm;
        public static string Pm01_raw;
        public static string Pm25_raw;
        public static string Pm10_raw;
        public static string Temp;
        public static string Humi;
        public static string Co2;
        public static string Voc;
        public static string Noise;
        public static string Pm10;
        public static string Pm25;
        public static string Pm01;
        public static string Cici_pm10;
        public static string Cici_pm25;
        public static string Cici_co2;
        public static string Cici_voc;
        public static string Ciai;
        public static string Cici_temp;
        public static string Cici_humi;
        public static string Cici_noise;
        public static string Cici;
    }

    public static class ISC0W1800010
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Tm;
        public static string Pm01_raw;
        public static string Pm25_raw;
        public static string Pm10_raw;
        public static string Temp;
        public static string Humi;
        public static string Co2;
        public static string Voc;
        public static string Noise;
        public static string Pm10;
        public static string Pm25;
        public static string Pm01;
        public static string Cici_pm10;
        public static string Cici_pm25;
        public static string Cici_co2;
        public static string Cici_voc;
        public static string Ciai;
        public static string Cici_temp;
        public static string Cici_humi;
        public static string Cici_noise;
        public static string Cici;
    }

    public static class ISC0W1800013
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Tm;
        public static string Pm01_raw;
        public static string Pm25_raw;
        public static string Pm10_raw;
        public static string Temp;
        public static string Humi;
        public static string Co2;
        public static string Voc;
        public static string Noise;
        public static string Pm10;
        public static string Pm25;
        public static string Pm01;
        public static string Cici_pm10;
        public static string Cici_pm25;
        public static string Cici_co2;
        public static string Cici_voc;
        public static string Ciai;
        public static string Cici_temp;
        public static string Cici_humi;
        public static string Cici_noise;
        public static string Cici;
    }

    public static class ISC0W1800012
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Tm;
        public static string Pm01_raw;
        public static string Pm25_raw;
        public static string Pm10_raw;
        public static string Temp;
        public static string Humi;
        public static string Co2;
        public static string Voc;
        public static string Noise;
        public static string Pm10;
        public static string Pm25;
        public static string Pm01;
        public static string Cici_pm10;
        public static string Cici_pm25;
        public static string Cici_co2;
        public static string Cici_voc;
        public static string Ciai;
        public static string Cici_temp;
        public static string Cici_humi;
        public static string Cici_noise;
        public static string Cici;
    }

    public static class ISC0W18000010
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Tm;
        public static string Pm01_raw;
        public static string Pm25_raw;
        public static string Pm10_raw;
        public static string Temp;
        public static string Humi;
        public static string Co2;
        public static string Voc;
        public static string Noise;
        public static string Pm10;
        public static string Pm25;
        public static string Pm01;
        public static string Cici_pm10;
        public static string Cici_pm25;
        public static string Cici_co2;
        public static string Cici_voc;
        public static string Ciai;
        public static string Cici_temp;
        public static string Cici_humi;
        public static string Cici_noise;
        public static string Cici;
    }

    public static class ICW0W2200121
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Tm;
        public static string Pm01_raw;
        public static string Pm25_raw;
        public static string Pm10_raw;
        public static string Temp;
        public static string Humi;
        public static string Co2;
        public static string Voc;
        public static string Noise;
        public static string Pm10;
        public static string Pm25;
        public static string Pm01;
        public static string Cici_pm10;
        public static string Cici_pm25;
        public static string Cici_co2;
        public static string Cici_voc;
        public static string Ciai;
        public static string Cici_temp;
        public static string Cici_humi;
        public static string Cici_noise;
        public static string Cici;
    }

    public static class ICW0W2100006
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Tm;
        public static string Pm01_raw;
        public static string Pm25_raw;
        public static string Pm10_raw;
        public static string Temp;
        public static string Humi;
        public static string Co2;
        public static string Voc;
        public static string Noise;
        public static string Pm10;
        public static string Pm25;
        public static string Pm01;
        public static string Cici_pm10;
        public static string Cici_pm25;
        public static string Cici_co2;
        public static string Cici_voc;
        public static string Ciai;
        public static string Cici_temp;
        public static string Cici_humi;
        public static string Cici_noise;
        public static string Cici;
    }

    public static class ICW0W2001046
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Tm;
        public static string Pm01_raw;
        public static string Pm25_raw;
        public static string Pm10_raw;
        public static string Temp;
        public static string Humi;
        public static string Co2;
        public static string Voc;
        public static string Noise;
        public static string Pm10;
        public static string Pm25;
        public static string Pm01;
        public static string Cici_pm10;
        public static string Cici_pm25;
        public static string Cici_co2;
        public static string Cici_voc;
        public static string Ciai;
        public static string Cici_temp;
        public static string Cici_humi;
        public static string Cici_noise;
        public static string Cici;
    }

    public static class ICW0W2001045
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Tm;
        public static string Pm01_raw;
        public static string Pm25_raw;
        public static string Pm10_raw;
        public static string Temp;
        public static string Humi;
        public static string Co2;
        public static string Voc;
        public static string Noise;
        public static string Pm10;
        public static string Pm25;
        public static string Pm01;
        public static string Cici_pm10;
        public static string Cici_pm25;
        public static string Cici_co2;
        public static string Cici_voc;
        public static string Ciai;
        public static string Cici_temp;
        public static string Cici_humi;
        public static string Cici_noise;
        public static string Cici;
    }

    public static class IBOKW2200025
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Tm;
        public static string Pm01_raw;
        public static string Pm25_raw;
        public static string Pm10_raw;
        public static string Temp;
        public static string Humi;
        public static string Co2;
        public static string Voc;
        public static string Noise;
        public static string Pm10;
        public static string Pm25;
        public static string Pm01;
        public static string Cici_pm10;
        public static string Cici_pm25;
        public static string Cici_co2;
        public static string Cici_voc;
        public static string Ciai;
        public static string Cici_temp;
        public static string Cici_humi;
        public static string Cici_noise;
        public static string Cici;
    }

    public static class IBOKW2200028
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Tm;
        public static string Pm01_raw;
        public static string Pm25_raw;
        public static string Pm10_raw;
        public static string Temp;
        public static string Humi;
        public static string Co2;
        public static string Voc;
        public static string Noise;
        public static string Pm10;
        public static string Pm25;
        public static string Pm01;
        public static string Cici_pm10;
        public static string Cici_pm25;
        public static string Cici_co2;
        public static string Cici_voc;
        public static string Ciai;
        public static string Cici_temp;
        public static string Cici_humi;
        public static string Cici_noise;
        public static string Cici;
    }

    public static class IBOKW2200033
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Tm;
        public static string Pm01_raw;
        public static string Pm25_raw;
        public static string Pm10_raw;
        public static string Temp;
        public static string Humi;
        public static string Co2;
        public static string Voc;
        public static string Noise;
        public static string Pm10;
        public static string Pm25;
        public static string Pm01;
        public static string Cici_pm10;
        public static string Cici_pm25;
        public static string Cici_co2;
        public static string Cici_voc;
        public static string Ciai;
        public static string Cici_temp;
        public static string Cici_humi;
        public static string Cici_noise;
        public static string Cici;
    }

    public static class IBOKW2200024
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Tm;
        public static string Pm01_raw;
        public static string Pm25_raw;
        public static string Pm10_raw;
        public static string Temp;
        public static string Humi;
        public static string Co2;
        public static string Voc;
        public static string Noise;
        public static string Pm10;
        public static string Pm25;
        public static string Pm01;
        public static string Cici_pm10;
        public static string Cici_pm25;
        public static string Cici_co2;
        public static string Cici_voc;
        public static string Ciai;
        public static string Cici_temp;
        public static string Cici_humi;
        public static string Cici_noise;
        public static string Cici;
    }
    #endregion
}