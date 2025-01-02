using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class VentilatorCleanerData : MonoBehaviour
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
            url = "http://keti1.energyiotlab.com:10111/v1/sensors/kw-vskp1";
            //"http://io.energyiotlab.com/irmon2/kw-vskp1";
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
        public string DateTime;
        public string Reg_date;
        public string Power;
        public string Air_volume;
        public string Exh_mode;
        public string Auto_mode;
        public string Filter_alarm;
        public string Air_mode;
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
                List<string> reg_dateList = new List<string>();
                List<string> powerList = new List<string>();
                List<string> air_volumeList = new List<string>();
                List<string> exh_modeList = new List<string>();
                List<string> auto_modeList = new List<string>();
                List<string> filter_alarmList = new List<string>();
                List<string> air_modeList = new List<string>();

                foreach (var childF in jObject)
                {
                    var key = childF.Key;
                    //Debug.Log($"Key : {childF.Key}");
                    keyList.Add(key);

                    foreach (var childS in childF.Value)
                    {
                        if (childS.ToString().Contains("service"))
                        {
                            var timestamp = childS.First["timestamp"].ToString();
                            //Debug.Log($"Timestamp : {timestamp}");
                            timestampList.Add(timestamp);

                            var datetime = childS.First["datetime"].ToString();
                            //Debug.Log($"Datetime : {datetime}");
                            datetimeList.Add(datetime);
                        }

                        if (childS.ToString().Contains("data"))
                        {
                            if (childS.First.ToString().Contains("\"reg_date\":"))
                            {
                                var reg_date = childS.First["reg_date"].ToString();
                                //Debug.Log($"reg_date : {reg_date}");
                                reg_dateList.Add(reg_date);
                            }
                            else
                            {
                                reg_dateList.Add(null);
                            }

                            if (childS.First.ToString().Contains("\"power\":"))
                            {
                                var power = childS.First["power"].ToString();
                                //Debug.Log($"power : {power}");
                                powerList.Add(power);
                            }
                            else
                            {
                                powerList.Add(null);
                            }

                            if (childS.First.ToString().Contains("\"air_volume\":"))
                            {
                                var air_volume = childS.First["air_volume"].ToString();
                                //Debug.Log($"air_volume : {air_volume}");
                                air_volumeList.Add(air_volume);
                            }
                            else
                            {
                                air_volumeList.Add(null);
                            }

                            if (childS.First.ToString().Contains("\"exh_mode\":"))
                            {
                                var exh_mode = childS.First["exh_mode"].ToString();
                                //Debug.Log($"exh_mode : {exh_mode}");
                                exh_modeList.Add(exh_mode);
                            }
                            else
                            {
                                exh_modeList.Add(null);
                            }

                            if (childS.First.ToString().Contains("\"auto_mode\":"))
                            {
                                var auto_mode = childS.First["auto_mode"].ToString();
                                //Debug.Log($"auto_mode : {auto_mode}");
                                auto_modeList.Add(auto_mode);
                            }
                            else
                            {
                                auto_modeList.Add(null);
                            }

                            if (childS.First.ToString().Contains("\"filter_alarm\":"))
                            {
                                var filter_alarm = childS.First["filter_alarm"].ToString();
                                //Debug.Log($"filter_alarm : {filter_alarm}");
                                filter_alarmList.Add(filter_alarm);
                            }
                            else
                            {
                                filter_alarmList.Add(null);
                            }

                            if (childS.First.ToString().Contains("\"air_mode\":"))
                            {
                                var air_mode = childS.First["air_mode"].ToString();
                                //Debug.Log($"air_mode : {air_mode}");
                                air_modeList.Add(air_mode);
                            }
                            else
                            {
                                air_modeList.Add(null);
                            }
                        }
                    }

                    data = new Data[keyList.Count];
                    for (int i = 0; i < keyList.Count; i++)
                    {
                        data[i].Key = keyList[i];
                        data[i].Timestamp = timestampList[i];
                        data[i].DateTime = datetimeList[i];
                        data[i].Reg_date = reg_dateList[i];
                        data[i].Power = powerList[i];
                        data[i].Air_volume = air_volumeList[i];
                        data[i].Exh_mode = exh_modeList[i];
                        data[i].Auto_mode = auto_modeList[i];
                        data[i].Filter_alarm = filter_alarmList[i];
                        data[i].Air_mode = air_modeList[i];
                    }
                    //Debug.Log("=========================================");

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
            // 동아 유치원 지하 강당 벽면(우)
            if (child.Key == "KWV-ST1_2100144")
            {
                KWV_ST1_2100144.Key = key_144 = child.Key;
                KWV_ST1_2100144.Timestamp = timestamp_144 = child.Timestamp;
                KWV_ST1_2100144.Datetime = datetime_144 = child.DateTime;
                KWV_ST1_2100144.Reg_date = reg_date_144 = child.Reg_date;
                KWV_ST1_2100144.Power = power_144 = child.Power;
                KWV_ST1_2100144.Air_volume = air_volume_144 = child.Air_volume;
                KWV_ST1_2100144.Exh_mode = exh_mode_144 = child.Exh_mode;
                KWV_ST1_2100144.Auto_mode = auto_mode_144 = child.Auto_mode;
                KWV_ST1_2100144.Filter_alarm = filter_alarm_144 = child.Filter_alarm;
                KWV_ST1_2100144.Air_mode = air_mode_144 = child.Air_mode;
            }
            // 동아 유치원 지하 강당 벽면(좌)
            else if (child.Key == "KWV-ST1_2101208")
            {
                KWV_ST1_2101208.Key = key_208 = child.Key;
                KWV_ST1_2101208.Timestamp = timestamp_208 = child.Timestamp;
                KWV_ST1_2101208.Datetime = datetime_208 = child.DateTime;
                KWV_ST1_2101208.Reg_date = reg_date_208 = child.Reg_date;
                KWV_ST1_2101208.Power = power_208 = child.Power;
                KWV_ST1_2101208.Air_volume = air_volume_208 = child.Air_volume;
                KWV_ST1_2101208.Exh_mode = exh_mode_208 = child.Exh_mode;
                KWV_ST1_2101208.Auto_mode = auto_mode_208 = child.Auto_mode;
                KWV_ST1_2101208.Filter_alarm = filter_alarm_208 = child.Filter_alarm;
                KWV_ST1_2101208.Air_mode = air_mode_208 = child.Air_mode;
            }
            // 동아 유치원 2층 가온반
            else if (child.Key == "KWV-ST1_2100226")
            {
                KWV_ST1_2100226.Key = key_226 = child.Key;
                KWV_ST1_2100226.Timestamp = timestamp_226 = child.Timestamp;
                KWV_ST1_2100226.Datetime = datetime_226 = child.DateTime;
                KWV_ST1_2100226.Reg_date = reg_date_226 = child.Reg_date;
                KWV_ST1_2100226.Power = power_226 = child.Power;
                KWV_ST1_2100226.Air_volume = air_volume_226 = child.Air_volume;
                KWV_ST1_2100226.Exh_mode = exh_mode_226 = child.Exh_mode;
                KWV_ST1_2100226.Auto_mode = auto_mode_226 = child.Auto_mode;
                KWV_ST1_2100226.Filter_alarm = filter_alarm_226 = child.Filter_alarm;
                KWV_ST1_2100226.Air_mode = air_mode_226 = child.Air_mode;
            }
            // 동아 유치원 2층 나래반
            else if (child.Key == "KWV-ST1_2100179")
            {
                KWV_ST1_2100179.Key = key_179 = child.Key;
                KWV_ST1_2100179.Timestamp = timestamp_179 = child.Timestamp;
                KWV_ST1_2100179.Datetime = datetime_179 = child.DateTime;
                KWV_ST1_2100179.Reg_date = reg_date_179 = child.Reg_date;
                KWV_ST1_2100179.Power = power_179 = child.Power;
                KWV_ST1_2100179.Air_volume = air_volume_179 = child.Air_volume;
                KWV_ST1_2100179.Exh_mode = exh_mode_179 = child.Exh_mode;
                KWV_ST1_2100179.Auto_mode = auto_mode_179 = child.Auto_mode;
                KWV_ST1_2100179.Filter_alarm = filter_alarm_179 = child.Filter_alarm;
                KWV_ST1_2100179.Air_mode = air_mode_179 = child.Air_mode;
            }

            // 신도림 초등학교 운동장 강연대 쪽
            else if (child.Key == "KWV-ST1_2100217")
            {
                KWV_ST1_2100217.Key = key_217 = child.Key;
                KWV_ST1_2100217.Timestamp = timestamp_217 = child.Timestamp;
                KWV_ST1_2100217.Datetime = datetime_217 = child.DateTime;
                KWV_ST1_2100217.Reg_date = reg_date_217 = child.Reg_date;
                KWV_ST1_2100217.Power = power_217 = child.Power;
                KWV_ST1_2100217.Air_volume = air_volume_217 = child.Air_volume;
                KWV_ST1_2100217.Exh_mode = exh_mode_217 = child.Exh_mode;
                KWV_ST1_2100217.Auto_mode = auto_mode_217 = child.Auto_mode;
                KWV_ST1_2100217.Filter_alarm = filter_alarm_217 = child.Filter_alarm;
                KWV_ST1_2100217.Air_mode = air_mode_217 = child.Air_mode;
            }
            // 신도림 초등학교 운동장 강연대 쪽
            else if (child.Key == "KWV-ST1_2100154")
            {
                KWV_ST1_2100154.Key = key_154 = child.Key;
                KWV_ST1_2100154.Timestamp = timestamp_154 = child.Timestamp;
                KWV_ST1_2100154.Datetime = datetime_154 = child.DateTime;
                KWV_ST1_2100154.Reg_date = reg_date_154 = child.Reg_date;
                KWV_ST1_2100154.Power = power_154 = child.Power;
                KWV_ST1_2100154.Air_volume = air_volume_154 = child.Air_volume;
                KWV_ST1_2100154.Exh_mode = exh_mode_154 = child.Exh_mode;
                KWV_ST1_2100154.Auto_mode = auto_mode_154 = child.Auto_mode;
                KWV_ST1_2100154.Filter_alarm = filter_alarm_154 = child.Filter_alarm;
                KWV_ST1_2100154.Air_mode = air_mode_154 = child.Air_mode;
            }
            // 신도림 초등학교 뒷면 입구 쪽
            else if (child.Key == "KWV-ST1_2100212")
            {
                KWV_ST1_2100212.Key = key_212 = child.Key;
                KWV_ST1_2100212.Timestamp = timestamp_212 = child.Timestamp;
                KWV_ST1_2100212.Datetime = datetime_212 = child.DateTime;
                KWV_ST1_2100212.Reg_date = reg_date_212 = child.Reg_date;
                KWV_ST1_2100212.Power = power_212 = child.Power;
                KWV_ST1_2100212.Air_volume = air_volume_212 = child.Air_volume;
                KWV_ST1_2100212.Exh_mode = exh_mode_212 = child.Exh_mode;
                KWV_ST1_2100212.Auto_mode = auto_mode_212 = child.Auto_mode;
                KWV_ST1_2100212.Filter_alarm = filter_alarm_212 = child.Filter_alarm;
                KWV_ST1_2100212.Air_mode = air_mode_212 = child.Air_mode;
            }
            // 신도림 초등학교 뒷면 입구 쪽
            else if (child.Key == "KWV-ST1_2100214")
            {
                KWV_ST1_2100214.Key = key_214 = child.Key;
                KWV_ST1_2100214.Timestamp = timestamp_214 = child.Timestamp;
                KWV_ST1_2100214.Datetime = datetime_214 = child.DateTime;
                KWV_ST1_2100214.Reg_date = reg_date_214 = child.Reg_date;
                KWV_ST1_2100214.Power = power_214 = child.Power;
                KWV_ST1_2100214.Air_volume = air_volume_214 = child.Air_volume;
                KWV_ST1_2100214.Exh_mode = exh_mode_214 = child.Exh_mode;
                KWV_ST1_2100214.Auto_mode = auto_mode_214 = child.Auto_mode;
                KWV_ST1_2100214.Filter_alarm = filter_alarm_214 = child.Filter_alarm;
                KWV_ST1_2100214.Air_mode = air_mode_214 = child.Air_mode;
            }
            // 신도림 초등학교 운동장 입구 쪽
            else if (child.Key == "KWV-ST1_2100216")
            {
                KWV_ST1_2100216.Key = key_216 = child.Key;
                KWV_ST1_2100216.Timestamp = timestamp_216 = child.Timestamp;
                KWV_ST1_2100216.Datetime = datetime_216 = child.DateTime;
                KWV_ST1_2100216.Reg_date = reg_date_216 = child.Reg_date;
                KWV_ST1_2100216.Power = power_216 = child.Power;
                KWV_ST1_2100216.Air_volume = air_volume_216 = child.Air_volume;
                KWV_ST1_2100216.Exh_mode = exh_mode_216 = child.Exh_mode;
                KWV_ST1_2100216.Auto_mode = auto_mode_216 = child.Auto_mode;
                KWV_ST1_2100216.Filter_alarm = filter_alarm_216 = child.Filter_alarm;
                KWV_ST1_2100216.Air_mode = air_mode_216 = child.Air_mode;
            }
            // 신도림 초등학교 운동장 입구 쪽
            else if (child.Key == "KWV-ST1_2100220")
            {
                KWV_ST1_2100220.Key = key_220 = child.Key;
                KWV_ST1_2100220.Timestamp = timestamp_220 = child.Timestamp;
                KWV_ST1_2100220.Datetime = datetime_220 = child.DateTime;
                KWV_ST1_2100220.Reg_date = reg_date_220 = child.Reg_date;
                KWV_ST1_2100220.Power = power_220 = child.Power;
                KWV_ST1_2100220.Air_volume = air_volume_220 = child.Air_volume;
                KWV_ST1_2100220.Exh_mode = exh_mode_220 = child.Exh_mode;
                KWV_ST1_2100220.Auto_mode = auto_mode_220 = child.Auto_mode;
                KWV_ST1_2100220.Filter_alarm = filter_alarm_220 = child.Filter_alarm;
                KWV_ST1_2100220.Air_mode = air_mode_220 = child.Air_mode;
            }

            // 목일 중학교 체육관 강연대 쪽
            else if (child.Key == "KWV-ST1_1900579")
            {
                KWV_ST1_1900579.Key = key_579 = child.Key;
                KWV_ST1_1900579.Timestamp = timestamp_579 = child.Timestamp;
                KWV_ST1_1900579.Datetime = datetime_579 = child.DateTime;
                KWV_ST1_1900579.Reg_date = reg_date_579 = child.Reg_date;
                KWV_ST1_1900579.Power = power_579 = child.Power;
                KWV_ST1_1900579.Air_volume = air_volume_579 = child.Air_volume;
                KWV_ST1_1900579.Exh_mode = exh_mode_579 = child.Exh_mode;
                KWV_ST1_1900579.Auto_mode = auto_mode_579 = child.Auto_mode;
                KWV_ST1_1900579.Filter_alarm = filter_alarm_579 = child.Filter_alarm;
                KWV_ST1_1900579.Air_mode = air_mode_579 = child.Air_mode;
            }
            // 목일 중학교 체육관 2층 스탠드
            else if (child.Key == "KWV-ST1_1900523")
            {
                KWV_ST1_1900523.Key = key_523 = child.Key;
                KWV_ST1_1900523.Timestamp = timestamp_523 = child.Timestamp;
                KWV_ST1_1900523.Datetime = datetime_523 = child.DateTime;
                KWV_ST1_1900523.Reg_date = reg_date_523 = child.Reg_date;
                KWV_ST1_1900523.Power = power_523 = child.Power;
                KWV_ST1_1900523.Air_volume = air_volume_523 = child.Air_volume;
                KWV_ST1_1900523.Exh_mode = exh_mode_523 = child.Exh_mode;
                KWV_ST1_1900523.Auto_mode = auto_mode_523 = child.Auto_mode;
                KWV_ST1_1900523.Filter_alarm = filter_alarm_523 = child.Filter_alarm;
                KWV_ST1_1900523.Air_mode = air_mode_523 = child.Air_mode;
            }
            // 목일 중학교 체육관 2층 스탠드
            else if (child.Key == "KWV-ST1_1900521")
            {
                KWV_ST1_1900521.Key = key_521 = child.Key;
                KWV_ST1_1900521.Timestamp = timestamp_521 = child.Timestamp;
                KWV_ST1_1900521.Datetime = datetime_521 = child.DateTime;
                KWV_ST1_1900521.Reg_date = reg_date_521 = child.Reg_date;
                KWV_ST1_1900521.Power = power_521 = child.Power;
                KWV_ST1_1900521.Air_volume = air_volume_521 = child.Air_volume;
                KWV_ST1_1900521.Exh_mode = exh_mode_521 = child.Exh_mode;
                KWV_ST1_1900521.Auto_mode = auto_mode_521 = child.Auto_mode;
                KWV_ST1_1900521.Filter_alarm = filter_alarm_521 = child.Filter_alarm;
                KWV_ST1_1900521.Air_mode = air_mode_521 = child.Air_mode;
            }
            // 목일 중학교 체육관 입구 쪽
            else if (child.Key == "KWV-ST1_1900524")
            {
                KWV_ST1_1900524.Key = key_524 = child.Key;
                KWV_ST1_1900524.Timestamp = timestamp_524 = child.Timestamp;
                KWV_ST1_1900524.Datetime = datetime_524 = child.DateTime;
                KWV_ST1_1900524.Reg_date = reg_date_524 = child.Reg_date;
                KWV_ST1_1900524.Power = power_524 = child.Power;
                KWV_ST1_1900524.Air_volume = air_volume_524 = child.Air_volume;
                KWV_ST1_1900524.Exh_mode = exh_mode_524 = child.Exh_mode;
                KWV_ST1_1900524.Auto_mode = auto_mode_524 = child.Auto_mode;
                KWV_ST1_1900524.Filter_alarm = filter_alarm_524 = child.Filter_alarm;
                KWV_ST1_1900524.Air_mode = air_mode_524 = child.Air_mode;
            }


            else if (child.Key == "KWV-ST1_1900326")
            {
                KWV_ST1_1900326.Key = key_326 = child.Key;
                KWV_ST1_1900326.Timestamp = timestamp_326 = child.Timestamp;
                KWV_ST1_1900326.Datetime = datetime_326 = child.DateTime;
                KWV_ST1_1900326.Reg_date = reg_date_326 = child.Reg_date;
                KWV_ST1_1900326.Power = power_326 = child.Power;
                KWV_ST1_1900326.Air_volume = air_volume_326 = child.Air_volume;
                KWV_ST1_1900326.Exh_mode = exh_mode_326 = child.Exh_mode;
                KWV_ST1_1900326.Auto_mode = auto_mode_326 = child.Auto_mode;
                KWV_ST1_1900326.Filter_alarm = filter_alarm_326 = child.Filter_alarm;
                KWV_ST1_1900326.Air_mode = air_mode_326 = child.Air_mode;
            }
            else if (child.Key == "KWV-ST1_1900268")
            {
                KWV_ST1_1900268.Key = key_268 = child.Key;
                KWV_ST1_1900268.Timestamp = timestamp_268 = child.Timestamp;
                KWV_ST1_1900268.Datetime = datetime_268 = child.DateTime;
                KWV_ST1_1900268.Reg_date = reg_date_268 = child.Reg_date;
                KWV_ST1_1900268.Power = power_268 = child.Power;
                KWV_ST1_1900268.Air_volume = air_volume_268 = child.Air_volume;
                KWV_ST1_1900268.Exh_mode = exh_mode_268 = child.Exh_mode;
                KWV_ST1_1900268.Auto_mode = auto_mode_268 = child.Auto_mode;
                KWV_ST1_1900268.Filter_alarm = filter_alarm_268 = child.Filter_alarm;
                KWV_ST1_1900268.Air_mode = air_mode_268 = child.Air_mode;
            }
            else if (child.Key == "KWV-ST1_1900471")
            {
                KWV_ST1_1900471.Key = key_471 = child.Key;
                KWV_ST1_1900471.Timestamp = timestamp_471 = child.Timestamp;
                KWV_ST1_1900471.Datetime = datetime_471 = child.DateTime;
                KWV_ST1_1900471.Reg_date = reg_date_471 = child.Reg_date;
                KWV_ST1_1900471.Power = power_471 = child.Power;
                KWV_ST1_1900471.Air_volume = air_volume_471 = child.Air_volume;
                KWV_ST1_1900471.Exh_mode = exh_mode_471 = child.Exh_mode;
                KWV_ST1_1900471.Auto_mode = auto_mode_471 = child.Auto_mode;
                KWV_ST1_1900471.Filter_alarm = filter_alarm_471 = child.Filter_alarm;
                KWV_ST1_1900471.Air_mode = air_mode_471 = child.Air_mode;
            }
            else if (child.Key == "KWV-ST1_1900384")
            {
                KWV_ST1_1900384.Key = key_384 = child.Key;
                KWV_ST1_1900384.Timestamp = timestamp_384 = child.Timestamp;
                KWV_ST1_1900384.Datetime = datetime_384 = child.DateTime;
                KWV_ST1_1900384.Reg_date = reg_date_384 = child.Reg_date;
                KWV_ST1_1900384.Power = power_384 = child.Power;
                KWV_ST1_1900384.Air_volume = air_volume_384 = child.Air_volume;
                KWV_ST1_1900384.Exh_mode = exh_mode_384 = child.Exh_mode;
                KWV_ST1_1900384.Auto_mode = auto_mode_384 = child.Auto_mode;
                KWV_ST1_1900384.Filter_alarm = filter_alarm_384 = child.Filter_alarm;
                KWV_ST1_1900384.Air_mode = air_mode_384 = child.Air_mode;
            }
            else if (child.Key == "KWV-ST1_1900340")
            {
                KWV_ST1_1900340.Key = key_340 = child.Key;
                KWV_ST1_1900340.Timestamp = timestamp_340 = child.Timestamp;
                KWV_ST1_1900340.Datetime = datetime_340 = child.DateTime;
                KWV_ST1_1900340.Reg_date = reg_date_340 = child.Reg_date;
                KWV_ST1_1900340.Power = power_340 = child.Power;
                KWV_ST1_1900340.Air_volume = air_volume_340 = child.Air_volume;
                KWV_ST1_1900340.Exh_mode = exh_mode_340 = child.Exh_mode;
                KWV_ST1_1900340.Auto_mode = auto_mode_340 = child.Auto_mode;
                KWV_ST1_1900340.Filter_alarm = filter_alarm_340 = child.Filter_alarm;
                KWV_ST1_1900340.Air_mode = air_mode_340 = child.Air_mode;
            }
            else if (child.Key == "KWV-ST1_1900519")
            {
                KWV_ST1_1900519.Key = key_519 = child.Key;
                KWV_ST1_1900519.Timestamp = timestamp_519 = child.Timestamp;
                KWV_ST1_1900519.Datetime = datetime_519 = child.DateTime;
                KWV_ST1_1900519.Reg_date = reg_date_519 = child.Reg_date;
                KWV_ST1_1900519.Power = power_519 = child.Power;
                KWV_ST1_1900519.Air_volume = air_volume_519 = child.Air_volume;
                KWV_ST1_1900519.Exh_mode = exh_mode_519 = child.Exh_mode;
                KWV_ST1_1900519.Auto_mode = auto_mode_519 = child.Auto_mode;
                KWV_ST1_1900519.Filter_alarm = filter_alarm_519 = child.Filter_alarm;
                KWV_ST1_1900519.Air_mode = air_mode_519 = child.Air_mode;
            }
            else if (child.Key == "KWV-ST1_2100471")
            {
                KWV_ST1_2100471.Key = key_0471 = child.Key;
                KWV_ST1_2100471.Timestamp = timestamp_0471 = child.Timestamp;
                KWV_ST1_2100471.Datetime = datetime_0471 = child.DateTime;
                KWV_ST1_2100471.Reg_date = reg_date_0471 = child.Reg_date;
                KWV_ST1_2100471.Power = power_0471 = child.Power;
                KWV_ST1_2100471.Air_volume = air_volume_0471 = child.Air_volume;
                KWV_ST1_2100471.Exh_mode = exh_mode_0471 = child.Exh_mode;
                KWV_ST1_2100471.Auto_mode = auto_mode_0471 = child.Auto_mode;
                KWV_ST1_2100471.Filter_alarm = filter_alarm_0471 = child.Filter_alarm;
                KWV_ST1_2100471.Air_mode = air_mode_0471 = child.Air_mode;
            }
            else if (child.Key == "KWV_ST1_1900471")
            {
                KWV_ST1_1900471_Ex.Key = key_00471 = child.Key;
                KWV_ST1_1900471_Ex.Timestamp = timestamp_00471 = child.Timestamp;
                KWV_ST1_1900471_Ex.Datetime = datetime_00471 = child.DateTime;
                KWV_ST1_1900471_Ex.Reg_date = reg_date_00471 = child.Reg_date;
                KWV_ST1_1900471_Ex.Power = power_00471 = child.Power;
                KWV_ST1_1900471_Ex.Air_volume = air_volume_00471 = child.Air_volume;
                KWV_ST1_1900471_Ex.Exh_mode = exh_mode_00471 = child.Exh_mode;
                KWV_ST1_1900471_Ex.Auto_mode = auto_mode_00471 = child.Auto_mode;
                KWV_ST1_1900471_Ex.Filter_alarm = filter_alarm_00471 = child.Filter_alarm;
                KWV_ST1_1900471_Ex.Air_mode = air_mode_00471 = child.Air_mode;
            }
        }
    }

    #region UNITY_INSPECTOR
    #region KWV-ST1_2100144 동아 유치원 지하 강당 벽면(우)
    [Space(20f)]
    [Header("< KWV-ST1_2100144 > ")]
    public string key_144;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_144;
    public string datetime_144;

    [Space(2f)]
    [Header("- Data")]
    public string reg_date_144;
    public string power_144;
    public string air_volume_144;
    public string exh_mode_144;
    public string auto_mode_144;
    public string filter_alarm_144;
    public string air_mode_144;
    #endregion

    #region KWV-ST1_2101208 동아 유치원 지하 강당 벽면(좌)
    [Space(20f)]
    [Header("< KWV-ST1_2101208 > ")]
    public string key_208;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_208;
    public string datetime_208;

    [Space(2f)]
    [Header("- Data")]
    public string reg_date_208;
    public string power_208;
    public string air_volume_208;
    public string exh_mode_208;
    public string auto_mode_208;
    public string filter_alarm_208;
    public string air_mode_208;
    #endregion

    #region KWV-ST1_2100226 동아 유치원 2층 가온반
    [Space(20f)]
    [Header("< KWV-ST1_2100226 > ")]
    public string key_226;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_226;
    public string datetime_226;

    [Space(2f)]
    [Header("- Data")]
    public string reg_date_226;
    public string power_226;
    public string air_volume_226;
    public string exh_mode_226;
    public string auto_mode_226;
    public string filter_alarm_226;
    public string air_mode_226;
    #endregion

    #region KWV-ST1_2100179 동아 유치원 2층 나래반
    [Space(20f)]
    [Header("< KWV-ST1_2100179 > ")]
    public string key_179;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_179;
    public string datetime_179;

    [Space(2f)]
    [Header("- Data")]
    public string reg_date_179;
    public string power_179;
    public string air_volume_179;
    public string exh_mode_179;
    public string auto_mode_179;
    public string filter_alarm_179;
    public string air_mode_179;
    #endregion



    #region KWV-ST1_2100217 신도림 초등학교 운동장 강연대 쪽
    [Space(20f)]
    [Header("< KWV-ST1_2100217 > ")]
    public string key_217;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_217;
    public string datetime_217;

    [Space(2f)]
    [Header("- Data")]
    public string reg_date_217;
    public string power_217;
    public string air_volume_217;
    public string exh_mode_217;
    public string auto_mode_217;
    public string filter_alarm_217;
    public string air_mode_217;
    #endregion

    #region KWV-ST1_2100154 신도림 초등학교 운동장 강연대 쪽
    [Space(20f)]
    [Header("< KWV-ST1_2100154 >")]
    public string key_154;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_154;
    public string datetime_154;

    [Space(2f)]
    [Header("- Data")]
    public string reg_date_154;
    public string power_154;
    public string air_volume_154;
    public string exh_mode_154;
    public string auto_mode_154;
    public string filter_alarm_154;
    public string air_mode_154;
    #endregion

    #region KWV-ST1_2100212 신도림 초등학교 뒷면 입구 쪽
    [Space(20f)]
    [Header("< KWV-ST1_2100212 > ")]
    public string key_212;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_212;
    public string datetime_212;

    [Space(2f)]
    [Header("- Data")]
    public string reg_date_212;
    public string power_212;
    public string air_volume_212;
    public string exh_mode_212;
    public string auto_mode_212;
    public string filter_alarm_212;
    public string air_mode_212;
    #endregion

    #region KWV-ST1_2100214 신도림 초등학교 뒷면 입구 쪽
    [Space(20f)]
    [Header("< KWV-ST1_2100214 > ")]
    public string key_214;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_214;
    public string datetime_214;

    [Space(2f)]
    [Header("- Data")]
    public string reg_date_214;
    public string power_214;
    public string air_volume_214;
    public string exh_mode_214;
    public string auto_mode_214;
    public string filter_alarm_214;
    public string air_mode_214;
    #endregion

    #region KWV-ST1_2100216 신도림 초등학교 운동장 입구 쪽
    [Space(20f)]
    [Header("< KWV-ST1_2100216 > ")]
    public string key_216;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_216;
    public string datetime_216;

    [Space(2f)]
    [Header("- Data")]
    public string reg_date_216;
    public string power_216;
    public string air_volume_216;
    public string exh_mode_216;
    public string auto_mode_216;
    public string filter_alarm_216;
    public string air_mode_216;
    #endregion

    #region KWV-ST1_2100220 신도림 초등학교 운동장 입구 쪽
    [Space(20f)]
    [Header("< KWV-ST1_2100220 > ")]
    public string key_220;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_220;
    public string datetime_220;

    [Space(2f)]
    [Header("- Data")]
    public string reg_date_220;
    public string power_220;
    public string air_volume_220;
    public string exh_mode_220;
    public string auto_mode_220;
    public string filter_alarm_220;
    public string air_mode_220;
    #endregion



    #region KWV-ST1_1900579 목일 중학교 체육관 강연대 쪽
    [Space(20f)]
    [Header("< KWV-ST1_1900579 > ")]
    public string key_579;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_579;
    public string datetime_579;

    [Space(2f)]
    [Header("- Data")]
    public string reg_date_579;
    public string power_579;
    public string air_volume_579;
    public string exh_mode_579;
    public string auto_mode_579;
    public string filter_alarm_579;
    public string air_mode_579;
    #endregion

    #region KWV-ST1_1900523 목일 중학교 체육관 2층 스탠드
    [Header("< KWV-ST1_1900523 >")]
    public string key_523;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_523;
    public string datetime_523;

    [Space(2f)]
    [Header("- Data")]
    public string reg_date_523;
    public string power_523;
    public string air_volume_523;
    public string exh_mode_523;
    public string auto_mode_523;
    public string filter_alarm_523;
    public string air_mode_523;
    #endregion

    #region KWV-ST1_1900521 목일 중학교 체육관 2층 스탠드
    [Space(20f)]
    [Header("< KWV-ST1_1900521 >")]
    public string key_521;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_521;
    public string datetime_521;

    [Space(2f)]
    [Header("- Data")]
    public string reg_date_521;
    public string power_521;
    public string air_volume_521;
    public string exh_mode_521;
    public string auto_mode_521;
    public string filter_alarm_521;
    public string air_mode_521;
    #endregion

    #region KWV-ST1_1900524 목일 중학교 체육관 입구 쪽
    [Space(20f)]
    [Header("< KWV-ST1_1900524 >")]
    public string key_524;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_524;
    public string datetime_524;

    [Space(2f)]
    [Header("- Data")]
    public string reg_date_524;
    public string power_524;
    public string air_volume_524;
    public string exh_mode_524;
    public string auto_mode_524;
    public string filter_alarm_524;
    public string air_mode_524;
    #endregion



    #region KWV-ST1_1900326
    [Space(20f)]
    [Header("< KWV-ST1_1900326 >")]
    public string key_326;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_326;
    public string datetime_326;

    [Space(2f)]
    [Header("- Data")]
    public string reg_date_326;
    public string power_326;
    public string air_volume_326;
    public string exh_mode_326;
    public string auto_mode_326;
    public string filter_alarm_326;
    public string air_mode_326;
    #endregion

    #region KWV-ST1_1900268
    [Space(20f)]
    [Header("< KWV-ST1_1900268 >")]
    public string key_268;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_268;
    public string datetime_268;

    [Space(2f)]
    [Header("- Data")]
    public string reg_date_268;
    public string power_268;
    public string air_volume_268;
    public string exh_mode_268;
    public string auto_mode_268;
    public string filter_alarm_268;
    public string air_mode_268;
    #endregion

    #region KWV-ST1_1900471
    [Space(20f)]
    [Header("< KWV-ST1_1900471 >")]
    public string key_471;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_471;
    public string datetime_471;

    [Space(2f)]
    [Header("- Data")]
    public string reg_date_471;
    public string power_471;
    public string air_volume_471;
    public string exh_mode_471;
    public string auto_mode_471;
    public string filter_alarm_471;
    public string air_mode_471;
    #endregion

    #region KWV-ST1_1900384
    [Space(20f)]
    [Header("< KWV-ST1_1900384 >")]
    public string key_384;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_384;
    public string datetime_384;

    [Space(2f)]
    [Header("- Data")]
    public string reg_date_384;
    public string power_384;
    public string air_volume_384;
    public string exh_mode_384;
    public string auto_mode_384;
    public string filter_alarm_384;
    public string air_mode_384;
    #endregion

    #region KWV-ST1_1900340
    [Space(20f)]
    [Header("< KWV-ST1_1900340 >")]
    public string key_340;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_340;
    public string datetime_340;

    [Space(2f)]
    [Header("- Data")]
    public string reg_date_340;
    public string power_340;
    public string air_volume_340;
    public string exh_mode_340;
    public string auto_mode_340;
    public string filter_alarm_340;
    public string air_mode_340;
    #endregion

    #region KWV-ST1_1900519
    [Space(20f)]
    [Header("< KWV-ST1_1900519 >")]
    public string key_519;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_519;
    public string datetime_519;

    [Space(2f)]
    [Header("- Data")]
    public string reg_date_519;
    public string power_519;
    public string air_volume_519;
    public string exh_mode_519;
    public string auto_mode_519;
    public string filter_alarm_519;
    public string air_mode_519;
    #endregion

    #region KWV-ST1_2100471
    [Space(20f)]
    [Header("< KWV-ST1_2100471 > ")]
    public string key_0471;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_0471;
    public string datetime_0471;

    [Space(2f)]
    [Header("- Data")]
    public string reg_date_0471;
    public string power_0471;
    public string air_volume_0471;
    public string exh_mode_0471;
    public string auto_mode_0471;
    public string filter_alarm_0471;
    public string air_mode_0471;
    #endregion

    #region KWV_ST1_1900471
    [Space(20f)]
    [Header("< KWV_ST1_1900471 > ")]
    public string key_00471;

    [Space(2f)]
    [Header("- Service")]
    public string timestamp_00471;
    public string datetime_00471;

    [Space(2f)]
    [Header("- Data")]
    public string reg_date_00471;
    public string power_00471;
    public string air_volume_00471;
    public string exh_mode_00471;
    public string auto_mode_00471;
    public string filter_alarm_00471;
    public string air_mode_00471;
    #endregion
    #endregion

    #region STATIC_CLASS
    // 동아 유치원 지하 강당 벽면(우)
    public static class KWV_ST1_2100144
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Reg_date;
        public static string Power;
        public static string Air_volume;
        public static string Exh_mode;
        public static string Auto_mode;
        public static string Filter_alarm;
        public static string Air_mode;
    }

    // 동아 유치원 지하 강당 벽면(좌)
    public static class KWV_ST1_2101208
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Reg_date;
        public static string Power;
        public static string Air_volume;
        public static string Exh_mode;
        public static string Auto_mode;
        public static string Filter_alarm;
        public static string Air_mode;
    }

    // 동아 유치원 2층 가온반
    public static class KWV_ST1_2100226
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Reg_date;
        public static string Power;
        public static string Air_volume;
        public static string Exh_mode;
        public static string Auto_mode;
        public static string Filter_alarm;
        public static string Air_mode;
    }

    // 동아 유치원 2층 나래반
    public static class KWV_ST1_2100179
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Reg_date;
        public static string Power;
        public static string Air_volume;
        public static string Exh_mode;
        public static string Auto_mode;
        public static string Filter_alarm;
        public static string Air_mode;
    }



    // 신도림 초등학교 운동장 강연대 쪽
    public static class KWV_ST1_2100217
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Reg_date;
        public static string Power;
        public static string Air_volume;
        public static string Exh_mode;
        public static string Auto_mode;
        public static string Filter_alarm;
        public static string Air_mode;
    }

    // 신도림 초등학교 운동장 강연대 쪽
    public static class KWV_ST1_2100154
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Reg_date;
        public static string Power;
        public static string Air_volume;
        public static string Exh_mode;
        public static string Auto_mode;
        public static string Filter_alarm;
        public static string Air_mode;
    }

    // 신도림 초등학교 뒷면 입구 쪽
    public static class KWV_ST1_2100212
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Reg_date;
        public static string Power;
        public static string Air_volume;
        public static string Exh_mode;
        public static string Auto_mode;
        public static string Filter_alarm;
        public static string Air_mode;
    }

    // 신도림 초등학교 뒷면 입구 쪽
    public static class KWV_ST1_2100214
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Reg_date;
        public static string Power;
        public static string Air_volume;
        public static string Exh_mode;
        public static string Auto_mode;
        public static string Filter_alarm;
        public static string Air_mode;
    }

    // 신도림 초등학교 운동장 입구 쪽
    public static class KWV_ST1_2100216
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Reg_date;
        public static string Power;
        public static string Air_volume;
        public static string Exh_mode;
        public static string Auto_mode;
        public static string Filter_alarm;
        public static string Air_mode;
    }

    // 신도림 초등학교 운동장 입구 쪽
    public static class KWV_ST1_2100220
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Reg_date;
        public static string Power;
        public static string Air_volume;
        public static string Exh_mode;
        public static string Auto_mode;
        public static string Filter_alarm;
        public static string Air_mode;
    }



    // 목일 중학교 체육관 강연대 쪽
    public static class KWV_ST1_1900579
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Reg_date;
        public static string Power;
        public static string Air_volume;
        public static string Exh_mode;
        public static string Auto_mode;
        public static string Filter_alarm;
        public static string Air_mode;
    }

    // 목일 중학교 체육관 2층 스탠드
    public static class KWV_ST1_1900523
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Reg_date;
        public static string Power;
        public static string Air_volume;
        public static string Exh_mode;
        public static string Auto_mode;
        public static string Filter_alarm;
        public static string Air_mode;
    }

    // 목일 중학교 체육관 2층 스탠드
    public static class KWV_ST1_1900521
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Reg_date;
        public static string Power;
        public static string Air_volume;
        public static string Exh_mode;
        public static string Auto_mode;
        public static string Filter_alarm;
        public static string Air_mode;
    }

    // 목일 중학교 체육관 입구 쪽
    public static class KWV_ST1_1900524
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Reg_date;
        public static string Power;
        public static string Air_volume;
        public static string Exh_mode;
        public static string Auto_mode;
        public static string Filter_alarm;
        public static string Air_mode;
    }



    public static class KWV_ST1_1900268
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Reg_date;
        public static string Power;
        public static string Air_volume;
        public static string Exh_mode;
        public static string Auto_mode;
        public static string Filter_alarm;
        public static string Air_mode;
    }

    public static class KWV_ST1_1900326
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Reg_date;
        public static string Power;
        public static string Air_volume;
        public static string Exh_mode;
        public static string Auto_mode;
        public static string Filter_alarm;
        public static string Air_mode;
    }

    public static class KWV_ST1_1900471
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Reg_date;
        public static string Power;
        public static string Air_volume;
        public static string Exh_mode;
        public static string Auto_mode;
        public static string Filter_alarm;
        public static string Air_mode;
    }

    public static class KWV_ST1_1900384
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Reg_date;
        public static string Power;
        public static string Air_volume;
        public static string Exh_mode;
        public static string Auto_mode;
        public static string Filter_alarm;
        public static string Air_mode;
    }

    public static class KWV_ST1_1900340
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Reg_date;
        public static string Power;
        public static string Air_volume;
        public static string Exh_mode;
        public static string Auto_mode;
        public static string Filter_alarm;
        public static string Air_mode;
    }

    public static class KWV_ST1_1900519
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Reg_date;
        public static string Power;
        public static string Air_volume;
        public static string Exh_mode;
        public static string Auto_mode;
        public static string Filter_alarm;
        public static string Air_mode;
    }

    public static class KWV_ST1_2100471
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Reg_date;
        public static string Power;
        public static string Air_volume;
        public static string Exh_mode;
        public static string Auto_mode;
        public static string Filter_alarm;
        public static string Air_mode;
    }

    public static class KWV_ST1_1900471_Ex
    {
        public static string Key;
        public static string Timestamp;
        public static string Datetime;
        public static string Reg_date;
        public static string Power;
        public static string Air_volume;
        public static string Exh_mode;
        public static string Auto_mode;
        public static string Filter_alarm;
        public static string Air_mode;
    }
    #endregion
}
