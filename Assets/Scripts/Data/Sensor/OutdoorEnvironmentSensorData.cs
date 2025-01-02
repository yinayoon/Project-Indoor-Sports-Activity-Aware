using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class OutdoorEnvironmentSensorData : MonoBehaviour
{
    string url;
    string jsonText;

    // 실외 환경센서 데이터
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("GetRequest"); // 단국대 진리관
        StartCoroutine("CoGetJsonData");
    }

    IEnumerator GetRequest()
    {
        while (true)
        {
            url = "http://keti1.energyiotlab.com:10111/v1/sensors/kw-oske1";
                //"http://io.energyiotlab.com/irmon2/kw-oske1";
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
        public string No2;
        public string So2;
        public string Atm;
        public string Rain;
        public string Pm10;
        public string Pm25;
        public string Pm01;
        public string Coci_pm10;
        public string Coci_pm25;
        public string Coai;
        public string Coci_temp;
        public string Coci_humi;
        public string Coci;
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
                //Debug.Log(jObject.ToString());

                List<string> keyList = new List<string>();
                List<string> timestampList = new List<string>();
                List<string> datetimeList = new List<string>();
                List<string> tmList = new List<string>();
                List<string> pm01_rawList = new List<string>();
                List<string> pm25_rawList = new List<string>();
                List<string> pm10_rawList = new List<string>();
                List<string> tempList = new List<string>();
                List<string> humiList = new List<string>();
                List<string> no2List = new List<string>();
                List<string> so2List = new List<string>();
                List<string> atmList = new List<string>();
                List<string> rainList = new List<string>();
                List<string> pm10List = new List<string>();
                List<string> pm25List = new List<string>();
                List<string> pm01List = new List<string>();
                List<string> coci_pm10List = new List<string>();
                List<string> coci_pm25List = new List<string>();
                List<string> coaiList = new List<string>();
                List<string> coci_tempList = new List<string>();
                List<string> coci_humiList = new List<string>();
                List<string> cociList = new List<string>();

                foreach (var childF in jObject)
                {
                    var key = childF.Key;
                    //Debug.Log($"Key : {childF.Key}");
                    keyList.Add(key);

                    foreach (var childS in childF.Value)
                    {
                        if (childS.ToString().Contains("service"))
                        {
                            //Debug.Log($"Service : {childS.First.ToString()}");

                            var timestamp = childS.First["timestamp"].ToString();
                            //Debug.Log($"Timestamp : {timestamp}");
                            timestampList.Add(timestamp);

                            var datetime = childS.First["datetime"].ToString();
                            //Debug.Log($"Datetime : {datetime}");
                            datetimeList.Add(datetime);
                        }

                        if (childS.ToString().Contains("data"))
                        {
                            //Debug.Log($"Data : {childS.First.ToString()}");

                            if (childS.First.ToString().Contains("\"tm\":"))
                            {
                                var tm = childS.First["tm"].ToString();
                                //Debug.Log($"tm : {tm}");
                                tmList.Add(tm);
                            }
                            else
                            {
                                tmList.Add(null);
                            }

                            if (childS.First.ToString().Contains("\"pm01_raw\":"))
                            {
                                var pm01_raw = childS.First["pm01_raw"].ToString();
                                //Debug.Log($"pm01_raw : {pm01_raw}");
                                pm01_rawList.Add(pm01_raw);
                            }
                            else
                            {
                                pm01_rawList.Add(null);
                            }

                            if (childS.First.ToString().Contains("\"pm25_raw\":"))
                            {
                                var pm25_raw = childS.First["pm25_raw"].ToString();
                                //Debug.Log($"pm25_raw : {pm25_raw}");
                                pm25_rawList.Add(pm25_raw);
                            }
                            else
                            {
                                pm25_rawList.Add(null);
                            }

                            if (childS.First.ToString().Contains("\"pm10_raw\":"))
                            {
                                var pm10_raw = childS.First["pm10_raw"].ToString();
                                //Debug.Log($"pm10_raw : {pm10_raw}");
                                pm10_rawList.Add(pm10_raw);
                            }
                            else
                            {
                                pm10_rawList.Add(null);
                            }

                            if (childS.First.ToString().Contains("\"temp\":"))
                            {
                                var temp = childS.First["temp"].ToString();
                                //Debug.Log($"temp : {temp}");
                                tempList.Add(temp);
                            }
                            else
                            {
                                tempList.Add(null);
                            }

                            if (childS.First.ToString().Contains("\"humi\":"))
                            {
                                var humi = childS.First["humi"].ToString();
                                //Debug.Log($"humi : {humi}");
                                humiList.Add(humi);
                            }
                            else
                            {
                                humiList.Add(null);
                            }

                            if (childS.First.ToString().Contains("\"no2\":"))
                            {
                                var no2 = childS.First["no2"].ToString();
                                //Debug.Log($"no2 : {no2}");
                                no2List.Add(no2);
                            }
                            else
                            {
                                no2List.Add(null);
                            }

                            if (childS.First.ToString().Contains("\"so2\":"))
                            {
                                var so2 = childS.First["so2"].ToString();
                                //Debug.Log($"so2 : {so2}");
                                so2List.Add(so2);
                            }
                            else
                            {
                                so2List.Add(null);
                            }

                            if (childS.First.ToString().Contains("\"atm\":"))
                            {
                                var atm = childS.First["atm"].ToString();
                                //Debug.Log($"atm : {atm}");
                                atmList.Add(atm);
                            }
                            else
                            {
                                atmList.Add(null);
                            }

                            if (childS.First.ToString().Contains("\"rain\":"))
                            {
                                var rain = childS.First["rain"].ToString();
                                //Debug.Log($"rain : {rain}");
                                rainList.Add(rain);
                            }
                            else
                            {
                                rainList.Add(null);
                            }

                            if (childS.First.ToString().Contains("\"pm10\":"))
                            {
                                var pm10 = childS.First["pm10"].ToString();
                                //Debug.Log($"pm10 : {pm10}");
                                pm10List.Add(pm10);
                            }
                            else
                            {
                                pm10List.Add(null);
                            }

                            if (childS.First.ToString().Contains("\"pm25\":"))
                            {
                                var pm25 = childS.First["pm25"].ToString();
                                //Debug.Log($"pm25 : {pm25}");
                                pm25List.Add(pm25);
                            }
                            else
                            {
                                pm25List.Add(null);
                            }

                            if (childS.First.ToString().Contains("\"pm01\":"))
                            {
                                var pm01 = childS.First["pm01"].ToString();
                                //Debug.Log($"pm01 : {pm01}");
                                pm01List.Add(pm01);
                            }
                            else
                            {
                                pm01List.Add(null);
                            }

                            if (childS.First.ToString().Contains("\"coci_pm10\":"))
                            {
                                var coci_pm10 = childS.First["coci_pm10"].ToString();
                                //Debug.Log($"coci_pm10 : {coci_pm10}");
                                coci_pm10List.Add(coci_pm10);
                            }
                            else
                            {
                                coci_pm10List.Add(null);
                            }

                            if (childS.First.ToString().Contains("\"coci_pm25\":"))
                            {
                                var coci_pm25 = childS.First["coci_pm25"].ToString();
                                //Debug.Log($"coci_pm25 : {coci_pm25}");
                                coci_pm25List.Add(coci_pm25);
                            }
                            else
                            {
                                coci_pm25List.Add(null);
                            }

                            if (childS.First.ToString().Contains("\"coai\":"))
                            {
                                var coai = childS.First["coai"].ToString();
                                //Debug.Log($"coai : {coai}");
                                coaiList.Add(coai);
                            }
                            else
                            {
                                coaiList.Add(null);
                            }

                            if (childS.First.ToString().Contains("\"coci_temp\":"))
                            {
                                var coci_temp = childS.First["coci_temp"].ToString();
                                //Debug.Log($"coci_temp : {coci_temp}");
                                coci_tempList.Add(coci_temp);
                            }
                            else
                            {
                                coci_tempList.Add(null);
                            }

                            if (childS.First.ToString().Contains("\"coci_humi\":"))
                            {
                                var coci_humi = childS.First["coci_humi"].ToString();
                                //Debug.Log($"coci_humi : {coci_humi}");
                                coci_humiList.Add(coci_humi);
                            }
                            else
                            {
                                coci_humiList.Add(null);
                            }

                            if (childS.First.ToString().Contains("\"coci\":"))
                            {
                                var coci = childS.First["coci"].ToString();
                                //Debug.Log($"coci : {coci}");
                                cociList.Add(coci);
                            }
                            else
                            {
                                cociList.Add(null);
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
                        data[i].Pm01_raw = pm01List[i];
                        data[i].Pm25_raw = pm25List[i];
                        data[i].Pm10_raw = pm10List[i];
                        data[i].Temp = tempList[i];
                        data[i].Humi = humiList[i];
                        data[i].No2 = no2List[i];
                        data[i].So2 = so2List[i];
                        data[i].Atm = atmList[i];
                        data[i].Rain = rainList[i];
                        data[i].Pm10 = pm10List[i];
                        data[i].Pm25 = pm25List[i];
                        data[i].Pm01 = pm01List[i];
                        data[i].Coci_pm10 = coci_pm10List[i];
                        data[i].Coci_pm25 = coci_pm25List[i];
                        data[i].Coai = coaiList[i];
                        data[i].Coci_temp = coci_tempList[i];
                        data[i].Coci_humi = coci_humiList[i];
                        data[i].Coci = cociList[i];
                    }
                    //Debug.Log("==============================");

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
            if (child.Key == "OT3CL1800000")
            {
                //Debug.Log($"OT3CL1800000");
                OT3CL1800000.Key = key_00 = child.Key;
                OT3CL1800000.Timestamp = timestamp_00 = child.Timestamp;
                OT3CL1800000.Datetime = datetime_00 = child.Datetime;
                OT3CL1800000.Tm = tm_00 = child.Tm;
                OT3CL1800000.Pm01_raw = pm01_raw_00 = child.Pm01_raw;
                OT3CL1800000.Pm25_raw = pm25_raw_00 = child.Pm25_raw;
                OT3CL1800000.Pm10_raw = pm10_raw_00 = child.Pm10_raw;
                OT3CL1800000.Temp = temp_00 = child.Temp;
                OT3CL1800000.Humi = humi_00 = child.Humi;
                OT3CL1800000.No2 = no2_00 = child.No2;
                OT3CL1800000.So2 = so2_00 = child.So2;
                OT3CL1800000.Atm = atm_00 = child.Atm;
                OT3CL1800000.Rain = rain_00 = child.Rain;
                OT3CL1800000.Pm10 = pm10_00 = child.Pm10;
                OT3CL1800000.Pm25 = pm25_00 = child.Pm25;
                OT3CL1800000.Pm01 = pm01_00 = child.Pm01;
                OT3CL1800000.Coci_pm10 = coci_pm10_00 = child.Coci_pm10;
                OT3CL1800000.Coci_pm25 = coci_pm25_00 = child.Coci_pm25;
                OT3CL1800000.Coai = coai_00 = child.Coai;
                OT3CL1800000.Coci_temp = coci_temp_00 = child.Coci_temp;
                OT3CL1800000.Coci_humi = coci_humi_00 = child.Coci_humi;
                OT3CL1800000.Coci = coci_00 = child.Coci;
            }
            else if (child.Key == "OCS1L2000010")
            {
                //Debug.Log($"OCS1L2000010");
                OCS1L2000010.Key = key_10 = child.Key;
                OCS1L2000010.Timestamp = timestamp_10 = child.Timestamp;
                OCS1L2000010.Datetime = datetime_10 = child.Datetime;
                OCS1L2000010.Tm = tm_00 = tm_10 = child.Tm;
                OCS1L2000010.Pm01_raw = pm01_raw_10 = child.Pm01_raw;
                OCS1L2000010.Pm25_raw = pm25_raw_10 = child.Pm25_raw;
                OCS1L2000010.Pm10_raw = pm10_raw_10 = child.Pm10_raw;
                OCS1L2000010.Temp = temp_10 = child.Temp;
                OCS1L2000010.Humi = humi_10 = child.Humi;
                OCS1L2000010.No2 = no2_10 = child.No2;
                OCS1L2000010.So2 = so2_10 = child.So2;
                OCS1L2000010.Atm = atm_10 = child.Atm;
                OCS1L2000010.Rain = rain_10 = child.Rain;
                OCS1L2000010.Pm10 = pm10_10 = child.Pm10;
                OCS1L2000010.Pm25 = pm25_10 = child.Pm25;
                OCS1L2000010.Pm01 = pm01_10 = child.Pm01;
                OCS1L2000010.Coci_pm10 = coci_pm10_10 = child.Coci_pm10;
                OCS1L2000010.Coci_pm25 = coci_pm25_10 = child.Coci_pm25;
                OCS1L2000010.Coai = coai_10 = child.Coai;
                OCS1L2000010.Coci_temp = coci_temp_10 = child.Coci_temp;
                OCS1L2000010.Coci_humi = coci_humi_10 = child.Coci_humi;
                OCS1L2000010.Coci = coci_10 = child.Coci;
            }
            else if (child.Key == "OCS1L2000011")
            {
                //Debug.Log($"OCS1L2000011");
                OCS1L2000011.Key = key_11 = child.Key;
                OCS1L2000011.Timestamp = timestamp_11 = child.Timestamp;
                OCS1L2000011.Datetime = datetime_11 = child.Datetime;
                OCS1L2000011.Tm = tm_11 = child.Tm;
                OCS1L2000011.Pm01_raw = pm01_raw_11 = child.Pm01_raw;
                OCS1L2000011.Pm25_raw = pm25_raw_11 = child.Pm25_raw;
                OCS1L2000011.Pm10_raw = pm10_raw_11 = child.Pm10_raw;
                OCS1L2000011.Temp = temp_11 = child.Temp;
                OCS1L2000011.Humi = humi_11 = child.Humi;
                OCS1L2000011.No2 = no2_11 = child.No2;
                OCS1L2000011.So2 = so2_11 = child.So2;
                OCS1L2000011.Atm = atm_11 = child.Atm;
                OCS1L2000011.Rain = rain_11 = child.Rain;
                OCS1L2000011.Pm10 = pm10_11 = child.Pm10;
                OCS1L2000011.Pm25 = pm25_11 = child.Pm25;
                OCS1L2000011.Pm01 = pm01_11 = child.Pm01;
                OCS1L2000011.Coci_pm10 = coci_pm10_11 = child.Coci_pm10;
                OCS1L2000011.Coci_pm25 = coci_pm25_11 = child.Coci_pm25;
                OCS1L2000011.Coci = coai_11 = child.Coai;
                OCS1L2000011.Coci_temp = coci_temp_11 = child.Coci_temp;
                OCS1L2000011.Coci_humi = coci_humi_11 = child.Coci_humi;
                OCS1L2000011.Coci = coci_11 = child.Coci;
            }
            else if (child.Key == "OCS1L2000012")
            {
                //Debug.Log($"OCS1L2000012");
                OCS1L2000012.Key = key_12 = child.Key;
                OCS1L2000012.Timestamp = timestamp_12 = child.Timestamp;
                OCS1L2000012.Datetime = datetime_12 = child.Datetime;
                OCS1L2000012.Tm = tm_12 = child.Tm;
                OCS1L2000012.Pm01_raw = pm01_raw_12 = child.Pm01_raw;
                OCS1L2000012.Pm25_raw = pm25_raw_12 = child.Pm25_raw;
                OCS1L2000012.Pm10_raw = pm10_raw_12 = child.Pm10_raw;
                OCS1L2000012.Temp = temp_12 = child.Temp;
                OCS1L2000012.Humi = humi_12 = child.Humi;
                OCS1L2000012.No2 = no2_12 = child.No2;
                OCS1L2000012.So2 = so2_12 = child.So2;
                OCS1L2000012.Atm = atm_12 = child.Atm;
                OCS1L2000012.Rain = rain_12 = child.Rain;
                OCS1L2000012.Pm10 = pm10_12 = child.Pm10;
                OCS1L2000012.Pm25 = pm25_12 = child.Pm25;
                OCS1L2000012.Pm01 = pm01_12 = child.Pm01;
                OCS1L2000012.Coci_pm10 = coci_pm10_12 = child.Coci_pm10;
                OCS1L2000012.Coci_pm25 = coci_pm25_12 = child.Coci_pm25;
                OCS1L2000012.Coai = coai_12 = child.Coai;
                OCS1L2000012.Coci_temp = coci_temp_12 = child.Coci_temp;
                OCS1L2000012.Coci_humi = coci_humi_12 = child.Coci_humi;
                OCS1L2000012.Coci = coci_12 = child.Coci;
            }
        }
    }

    #region UNITY_INSPECTOR
    #region OT3CL1800000
    [Header("< OT3CL1800000 >")]
    public string key_00;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_00;
    public string datetime_00;

    [Space(2f)]
    [Header("- Data")]
    public string tm_00;
    public string pm01_raw_00;
    public string pm25_raw_00;
    public string pm10_raw_00;
    public string temp_00;
    public string humi_00;
    public string no2_00;
    public string so2_00;
    public string atm_00;
    public string rain_00;
    public string pm10_00;
    public string pm25_00;
    public string pm01_00;
    public string coci_pm10_00;
    public string coci_pm25_00;
    public string coai_00;
    public string coci_temp_00;
    public string coci_humi_00;
    public string coci_00;
    #endregion

    #region OCS1L2000010
    [Space(10f)]
    [Header("< OCS1L2000010 >")]
    public string key_10;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_10;
    public string datetime_10;

    [Space(2f)]
    [Header("- Data")]
    public string tm_10;
    public string pm01_raw_10;
    public string pm25_raw_10;
    public string pm10_raw_10;
    public string temp_10;
    public string humi_10;
    public string no2_10;
    public string so2_10;
    public string atm_10;
    public string rain_10;
    public string pm10_10;
    public string pm25_10;
    public string pm01_10;
    public string coci_pm10_10;
    public string coci_pm25_10;
    public string coai_10;
    public string coci_temp_10;
    public string coci_humi_10;
    public string coci_10;
    #endregion

    #region OCS1L2000011
    [Space(10f)]
    [Header("< OCS1L2000011 >")]
    public string key_11;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_11;
    public string datetime_11;

    [Space(2f)]
    [Header("- Data")]
    public string tm_11;
    public string pm01_raw_11;
    public string pm25_raw_11;
    public string pm10_raw_11;
    public string temp_11;
    public string humi_11;
    public string no2_11;
    public string so2_11;
    public string atm_11;
    public string rain_11;
    public string pm10_11;
    public string pm25_11;
    public string pm01_11;
    public string coci_pm10_11;
    public string coci_pm25_11;
    public string coai_11;
    public string coci_temp_11;
    public string coci_humi_11;
    public string coci_11;
    #endregion

    #region OCS1L2000012
    [Space(10f)]
    [Header("< OCS1L2000012 >")]
    public string key_12;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_12;
    public string datetime_12;

    [Space(2f)]
    [Header("- Data")]
    public string tm_12;
    public string pm01_raw_12;
    public string pm25_raw_12;
    public string pm10_raw_12;
    public string temp_12;
    public string humi_12;
    public string no2_12;
    public string so2_12;
    public string atm_12;
    public string rain_12;
    public string pm10_12;
    public string pm25_12;
    public string pm01_12;
    public string coci_pm10_12;
    public string coci_pm25_12;
    public string coai_12;
    public string coci_temp_12;
    public string coci_humi_12;
    public string coci_12;
    #endregion
    #endregion

    #region STATIC_CLASS
    public static class OT3CL1800000
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
        public static string No2;
        public static string So2;
        public static string Atm;
        public static string Rain;
        public static string Pm10;
        public static string Pm25;
        public static string Pm01;
        public static string Coci_pm10;
        public static string Coci_pm25;
        public static string Coai;
        public static string Coci_temp;
        public static string Coci_humi;
        public static string Coci;
    }

    public static class OCS1L2000010
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
        public static string No2;
        public static string So2;
        public static string Atm;
        public static string Rain;
        public static string Pm10;
        public static string Pm25;
        public static string Pm01;
        public static string Coci_pm10;
        public static string Coci_pm25;
        public static string Coai;
        public static string Coci_temp;
        public static string Coci_humi;
        public static string Coci;
    }

    public static class OCS1L2000011
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
        public static string No2;
        public static string So2;
        public static string Atm;
        public static string Rain;
        public static string Pm10;
        public static string Pm25;
        public static string Pm01;
        public static string Coci_pm10;
        public static string Coci_pm25;
        public static string Coai;
        public static string Coci_temp;
        public static string Coci_humi;
        public static string Coci;
    }

    public static class OCS1L2000012
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
        public static string No2;
        public static string So2;
        public static string Atm;
        public static string Rain;
        public static string Pm10;
        public static string Pm25;
        public static string Pm01;
        public static string Coci_pm10;
        public static string Coci_pm25;
        public static string Coai;
        public static string Coci_temp;
        public static string Coci_humi;
        public static string Coci;
    }
    #endregion
}