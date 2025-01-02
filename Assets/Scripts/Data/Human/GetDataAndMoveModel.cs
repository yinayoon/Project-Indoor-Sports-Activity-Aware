using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

using UnityEngine.UI;
using System.Linq;
using TMPro;

public class GetDataAndMoveModel : MonoBehaviour
{
    #region 필드
    public bool RunTestVersion;

    public static string jsonText;

    TextAsset textAsset;

    Queue<List<Dictionary<long, List<ValueData>>>> humanTempDataListQueue = new Queue<List<Dictionary<long, List<ValueData>>>>();

    [Header("- For Human")]
    [SerializeField] GameObject humanGroup;

    List<GameObject> humanList = new List<GameObject>();

    // 시간 지연 용도 변수
    float timer;
    float waitingTime;

    JObject jObject;
    JArray jArray;

    [Header("- Debuging Text")]
    [SerializeField] Text timeStampText;
    [SerializeField] Text dataBufferText;
    [SerializeField] Text bufferScreenText;
    [SerializeField] Text humanCountText;

    [Header("- GUI")]
    [SerializeField] Toggle idToggle;

    [Header("- Acting Text")]
    [SerializeField] Text unknown;
    [SerializeField] Text walk;
    [SerializeField] Text run;
    [SerializeField] Text sitdown;
    [SerializeField] Text stand;
    [SerializeField] Text falldown;

    protected bool queueBool = false;

    Vector3 vecVel;
    #endregion

    void Start()
    {
        StartCoroutine(GetRequest("http://keti1.energyiotlab.com:10111/v1/sensors/mokil-ms.camera.context"));

        if (RunTestVersion)
        {
            bufferScreenText.transform.gameObject.SetActive(false);

            StartCoroutine("CoGetJsonDataTest");

            StartCoroutine("CoCreateHumanTest");
        }
        else
        {
            StartCoroutine("CoGetJsonData");

            queueBool = false;

            StartCoroutine("CoCreateHuman");
        }
    }

    IEnumerator GetRequest(string uri)
    {
        while (true)
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                // Request and wait for the desired page.
                yield return webRequest.SendWebRequest();

                string[] pages = uri.Split('/');
                int page = pages.Length - 1;

                switch (webRequest.result)
                {
                    case UnityWebRequest.Result.ConnectionError:
                    case UnityWebRequest.Result.DataProcessingError:
                        UnityEngine.Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                        break;
                    case UnityWebRequest.Result.ProtocolError:
                        UnityEngine.Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                        break;
                    case UnityWebRequest.Result.Success:
                        //Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                        jsonText = webRequest.downloadHandler.text;
                        break;
                }
            }
            sw.Stop();
        }
    }

    public class humanData
    {
        public long timestamp;
        public int id;
        public float x, y;
    }

    #region 접근용 클레스
    public class humanTempData
    {
        public long timestamp;
        public ValueData valueData = new ValueData();
    }

    public class ValueData
    {
        public long timestamp;
        public int id;
        public Pos pos = new Pos();
        public string type;
    }

    public class Pos
    {
        public float x, y;
    }
    #endregion

    int tempIdx = 1;
    string str;

    string timestamp_Compare;

    bool bufferTextOnSign = false;
    float timeSeconds;

    void WriteTxt(string filePath, string message)
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(Path.GetDirectoryName(filePath));

        if (!directoryInfo.Exists)
        {
            directoryInfo.Create();
        }

        FileStream fileStream
            = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);

        StreamWriter writer = new StreamWriter(fileStream, System.Text.Encoding.Unicode);

        writer.WriteLine(message);
        writer.Close();
    }

    IEnumerator CoGetJsonData()
    {
        while (true)
        {
            if (jsonText != null)
            {
                if (JObject.Parse(jsonText) != null)
                {
                    if (str == jsonText)
                    {
                        timeSeconds += Time.deltaTime;

                        if (timeSeconds >= 2)
                        {
                            DestroyAllHuman();
                            unknown.text = $"0 명";
                            walk.text = $"0 명"; ;
                            run.text = $"0 명"; ;
                            sitdown.text = $"0 명"; ;
                            stand.text = $"0 명"; ;
                            falldown.text = $"0 명"; ;
                            humanCountText.text = $"Human Count : 0";
                            bufferScreenText.gameObject.SetActive(false);
                            timeSeconds = 2;
                        }

                        yield return new WaitForSeconds(0.01f);
                    }
                    else if (str != jsonText)
                    {
                        timeSeconds = 0;

                        List<humanTempData> humanTempList = new List<humanTempData>();
                        List<long> _timestamp = new List<long>();
                        List<Dictionary<long, List<ValueData>>> valueDataDicList = new List<Dictionary<long, List<ValueData>>>();

                        str = jsonText;
                        jObject = JObject.Parse(str);

                        foreach (var jChild in jObject)
                        {
                            foreach (JToken child in jChild.Value.Last.Children())
                            {
                                Dictionary<long, List<ValueData>> valueDataDic = new Dictionary<long, List<ValueData>>();

                                foreach (JToken child_0 in child.Children())
                                {
                                    List<ValueData> _valueData = new List<ValueData>();
                                    humanTempData humanObj = new humanTempData();

                                    bufferTextOnSign = true;

                                    _timestamp.Add(long.Parse(child_0["service"]["timestamp"].ToString()));

                                    foreach (JToken objects in child_0["data"]["objects"])
                                    {
                                        humanObj.timestamp = long.Parse(child_0["service"]["timestamp"].ToString());
                                        humanObj.valueData.id = int.Parse(objects["id"].ToString());
                                        humanObj.valueData.pos.x = float.Parse(objects["pos2dMap"]["x"].ToString());
                                        humanObj.valueData.pos.y = float.Parse(objects["pos2dMap"]["y"].ToString());

                                        foreach (var status in objects["status"])
                                        {
                                            humanObj.valueData.type = status["type"].ToString();
                                        }

                                        _valueData.Add(new ValueData
                                        {
                                            timestamp = humanObj.timestamp,
                                            id = humanObj.valueData.id,
                                            pos = new Pos { x = humanObj.valueData.pos.x, y = humanObj.valueData.pos.y },
                                            type = humanObj.valueData.type
                                        });
                                    }

                                    if (valueDataDic.ContainsKey(humanObj.timestamp))
                                        valueDataDic[humanObj.timestamp] = _valueData;
                                    else
                                        valueDataDic.Add(humanObj.timestamp, _valueData);
                                }
                                valueDataDicList.Add(valueDataDic);
                            }

                            humanTempDataListQueue.Enqueue(valueDataDicList);
                        }
                    }
                }
                else
                    Debug.Log("Failed to get Data");
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    Dictionary<long, List<ValueData>> valueDataDic = new Dictionary<long, List<ValueData>>();

    IEnumerator CoGetJsonDataTest()
    {
        while (true)
        {
            #region 작성되어 있는 Json 데이터 활용 코드
            textAsset = Managers.Resource.Load<TextAsset>($"Data/file{tempIdx}");
            
            if (tempIdx < 456)
            {
                tempIdx++;
            }
            else
            {
                tempIdx = 456;
            }
            #endregion
            
            if (str != textAsset.text)
            {
                timeSeconds = 0;

                List<humanTempData> humanTempList = new List<humanTempData>();
                List<long> _timestamp = new List<long>();
                List<Dictionary<long, List<ValueData>>> valueDataDicList = new List<Dictionary<long, List<ValueData>>>();

                str = textAsset.text;
                jObject = JObject.Parse(str);

                foreach (var jChild in jObject)
                {
                    foreach (JToken child in jChild.Value.Last.Children())
                    {
                        Dictionary<long, List<ValueData>> valueDataDic = new Dictionary<long, List<ValueData>>();

                        foreach (JToken child_0 in child.Children())
                        {
                            List<ValueData> _valueData = new List<ValueData>();
                            humanTempData humanObj = new humanTempData();

                            bufferTextOnSign = true;

                            _timestamp.Add(long.Parse(child_0["service"]["timestamp"].ToString()));

                            foreach (JToken objects in child_0["data"]["objects"])
                            {
                                humanObj.timestamp = long.Parse(child_0["service"]["timestamp"].ToString());
                                humanObj.valueData.id = int.Parse(objects["id"].ToString());
                                humanObj.valueData.pos.x = float.Parse(objects["pos2dMap"]["x"].ToString());
                                humanObj.valueData.pos.y = float.Parse(objects["pos2dMap"]["y"].ToString());

                                foreach (var status in objects["status"])
                                {
                                    humanObj.valueData.type = status["type"].ToString();
                                }

                                _valueData.Add(new ValueData
                                {
                                    timestamp = humanObj.timestamp,
                                    id = humanObj.valueData.id,
                                    pos = new Pos { x = humanObj.valueData.pos.x, y = humanObj.valueData.pos.y },
                                    type = humanObj.valueData.type
                                });
                        }

                        if (valueDataDic.ContainsKey(humanObj.timestamp))
                                valueDataDic[humanObj.timestamp] = _valueData;
                            else
                                valueDataDic.Add(humanObj.timestamp, _valueData);
                        }
                        valueDataDicList.Add(valueDataDic);
                    }

                    humanTempDataListQueue.Enqueue(valueDataDicList);
                }
            }
            
            yield return new WaitForSeconds(0.01f);
        }
    }            

    #region 움직임 (애니메이션 코드)
    Dictionary<string, Vector3> dic = new Dictionary<string, Vector3>();
    Dictionary<string, Vector3> dicVec = new Dictionary<string, Vector3>();
    Dictionary<string, string> dicType = new Dictionary<string, string>();
    Dictionary<string, Vector3> dicVel = new Dictionary<string, Vector3>();
    List<List<long>> idTotal = new List<List<long>>();
    List<List<string>> typeTotal = new List<List<string>>();
    Vector3 vec = new Vector3();

    long time = 0;

    int idxForAnim = 0; // 초당 움직임 좌표를 받기 위한 인덱스
    int totalIdx = 0;

    int currentSecond;
    int beforeSecond;

    float timerForAnim = 0;
    float waitingTimeForAnim;
    
    private void Update()
    {
        timerForAnim += Time.deltaTime;
        waitingTimeForAnim = waitingTime / totalIdx;

        if (timerForAnim > waitingTimeForAnim)
        {
            if (RunTestVersion)
                UpdateMoving(idTotal, vec);
            else
                UpdateMoving(idTotal, vec, queueBool);

            timerForAnim = 0;
        }
    }

    int bufferNum = 0;
    IEnumerator CoCreateHuman()
    {
        while (true)
        {
            List<Dictionary<long, List<ValueData>>> _valueDataDicList = new List<Dictionary<long, List<ValueData>>>();

            dataBufferText.text = $"Buffer Count : {humanTempDataListQueue.Count}";
            //버퍼 코드 : 버퍼가 30개 쌓이면 불리언을 true로 0이면 30개 쌓일 때까지 대기
            if (humanTempDataListQueue.Count > bufferNum || bufferTextOnSign == false)
            {
                bufferScreenText.gameObject.SetActive(false);
                queueBool = true;
            }
            else if (humanTempDataListQueue.Count <= 0)
            {
                bufferScreenText.gameObject.SetActive(true);

                queueBool = false;
            }

            // 만약 queueBool가 true라면 안쪽 코드 실행
            if (queueBool == true)// && bufferTextOnSign == true)
            {
                #region 좌표 저장 코드
                List<List<int>> idListGroup = new List<List<int>>();
                List<List<float>> xListGroup = new List<List<float>>();
                List<List<float>> yListGroup = new List<List<float>>();
                List<List<long>> timestampGroup = new List<List<long>>();

                List<List<string>> typeListGroup = new List<List<string>>();

                if (bufferTextOnSign == false)
                {
                    yield return new WaitForSeconds(0.01f);
                    humanCountText.text = $"Human Count : 0";
                    continue;
                }
                // TODO : 해당 부분에 이전 humanTempDataListQueue의 값과 TempQueue 값이 다르다면 다음 작업 수행
                _valueDataDicList = humanTempDataListQueue.Dequeue();

                StoreCoordinatesGroup(_valueDataDicList, ref idListGroup, ref xListGroup, ref yListGroup, ref timestampGroup, ref typeListGroup);

                List<string> id = new List<string>();
                List<float> x = new List<float>();
                List<float> y = new List<float>();
                List<long> timestamp = new List<long>();
                List<string> type = new List<string>();

                List<List<long>> _idTotal = new List<List<long>>();
                List<List<float>> _xTotal = new List<List<float>>();
                List<List<float>> _yTotal = new List<List<float>>();
                List<List<long>> _timestampTotal = new List<List<long>>();
                List<List<string>> _typeTotal = new List<List<string>>();

                StoreCoordinates(ref idListGroup, ref xListGroup, ref yListGroup, ref timestampGroup, ref typeListGroup, ref _idTotal, ref _xTotal, ref _yTotal, ref _timestampTotal, ref _typeTotal);

                totalIdx = _idTotal.Count;

                // _timestampTotal의 값은 2차원 리스트로 되어 있어 여으며
                // 리스트 속 값이 동일하므로 값들 중 하나의 timestamp값만 추출해서
                // _timestamp속에 대입
                List<long> _timestamp = new List<long>();
                for (int i = 0; i < _timestampTotal.Count; i++)
                {
                    _timestamp.Add(_timestampTotal[i][0]);
                }

                if (_idTotal.Count <= idxForAnim)
                    yield return null;

                #endregion

                while (idxForAnim <= _idTotal.Count - 1) // 만약 idxForAnim(1초에 넘어오는 애니메이션 배열 수 만큼 증가하는 변수)가 1초에 넘어오는 애니메이션 배열 수보다 작은 동안 실행.
                                                         // (_idTotal.Count : id 갯수 만큼 애니메이션 값이 넘어오므로 이 것을 비교 대상으로 사용함.)
                {
                    //timeStampText.text = $"timestamp : {UnixTimeToDateTime(_timestampTotal[idxForAnim][0])}";
                    timeStampText.text = $"timestamp : {_timestampTotal[idxForAnim][0]}";
                    humanCountText.text = $"Human Count : {_idTotal[idxForAnim].Count}";
                    //Debug.Log(_idTotal[i].Count);
                    

                    //Debug.Log(idxForAnim);
                    for (int i = 0; i < _idTotal[idxForAnim].Count; i++)
                    {
                        //Debug.Log(i + " : " + _idTotal[idxForAnim][i]);
                        // 실제 위치에 맞게 좌표 수정 코드
                        float TempX = -_xTotal[idxForAnim][i];
                        //float Xvalue = (265.35f * TempX) / 100.0f;
                        float Xvalue = (270.0f * TempX) / 100.0f;
                        float TempY = _yTotal[idxForAnim][i];
                        //float Yvalue = (193f * TempY) / 100.0f;
                        float Yvalue = (180.0f * TempY) / 100.0f;

                        string Type = _typeTotal[idxForAnim][i];

                        // 이동 백터를 dic에 저장 (key 값 : 재실자 id값, Value 값 : 넘어온 x, y 좌표값)
                        if (dic.ContainsKey(_idTotal[idxForAnim][i].ToString()))
                        {
                            dic[_idTotal[idxForAnim][i].ToString()] = new Vector3(Xvalue, 0.0f, Yvalue);
                        }
                        else
                        {
                            dic.Add(_idTotal[idxForAnim][i].ToString(), new Vector3(Xvalue, 0.0f, Yvalue));
                        }

                        // 속도를 위한 딕셔너리 대입 부분 추가
                        if (dicVel.ContainsKey(_idTotal[idxForAnim][i].ToString()))
                        {
                            dicVel[_idTotal[idxForAnim][i].ToString()] = new Vector3(Xvalue, 0.0f, Yvalue);
                        }
                        else
                        {
                            dicVel.Add(_idTotal[idxForAnim][i].ToString(), new Vector3(Xvalue, 0.0f, Yvalue));
                        }

                        // 행동을 위한 딕셔너리 대입 부분 추가
                        if (dicType.ContainsKey(_idTotal[idxForAnim][i].ToString()))
                        {
                            dicType[_idTotal[idxForAnim][i].ToString()] = _typeTotal[idxForAnim][i];
                        }
                        else
                        {
                            dicType.Add(_idTotal[idxForAnim][i].ToString(), _typeTotal[idxForAnim][i]);
                        }

                        // 이동 백터의 원본을 dicVec에 저장 (key 값 : 재실자 id값, Value 값 : 넘어온 x, y 좌표값)
                        if (dicVec.ContainsKey(_idTotal[idxForAnim][i].ToString()))
                        {
                            dicVec[_idTotal[idxForAnim][i].ToString()] = new Vector3(_xTotal[idxForAnim][i], 0.0f, _yTotal[idxForAnim][i]);
                        }
                        else
                        {
                            dicVec.Add(_idTotal[idxForAnim][i].ToString(), new Vector3(_xTotal[idxForAnim][i], 0.0f, _yTotal[idxForAnim][i]));
                        }
                    }

                    List<string> realId = new List<string>();
                    for (int j = 0; j < _idTotal[idxForAnim].Count; j++)
                    {
                        realId.Add(_idTotal[idxForAnim][j].ToString());
                    }

                    List<string> modelId = new List<string>();
                    for (int j = 0; j < humanList.Count; j++)
                    {
                        modelId.Add(humanList[j].GetComponent<HumanSet>().id);
                    }

                    List<string> intersectionIdList = modelId.Except(realId).ToList();


                    List<int> presentHumanIdData = new List<int>();
                    for (int i = 0; i < humanList.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(humanList[i].GetComponent<HumanSet>().id))
                        {
                            presentHumanIdData.Add(int.Parse(humanList[i].GetComponent<HumanSet>().id));
                        }
                    }

                    // 데이터로 넘어오는 id 값과 현재 재실자의 id 값의 교집합
                    List<int> intersectionList = idListGroup[idxForAnim].Intersect(presentHumanIdData).ToList();
                    // 차집합 : presentHumanIdData에서 intersectionList 중복되면 제외 한다.
                    List<int> differenceList = presentHumanIdData.Except(intersectionList).ToList();//presentHumanIdData.Except(intersectionList).ToList();

                    List<int> delateIdNum = new List<int>();
                    foreach (var child in differenceList)
                        delateIdNum.Add(child);

                    // 만약 생성되어 있는 모델의 수보다 id의 수가 크다면 그만큼 새로 생성
                    if (_idTotal[idxForAnim].Count > humanList.Count)
                    {
                        int tempNum = idListGroup[idxForAnim].Count - humanList.Count;

                        for (int i = 0; i < tempNum; i++)
                        {
                            Vector3 vector;

                            if (dic.TryGetValue(_idTotal[idxForAnim][i].ToString(), out vector)) // 여기가 바뀌어야함
                            {
                                GameObject go;
                                go = Managers.Resource.Instantiate("Human_Prefab", humanGroup.transform);

                                Renderer[] renderer = go.transform.GetComponentsInChildren<Renderer>();

                                humanList.Add(go);

                                humanList[i].GetComponent<HumanSet>().enabled = true;

                                if (go.transform.position == Vector3.zero)
                                    go.SetActive(false);
                            }
                        }
                    }

                    // 2. 기존의 재실자 모델 중 id 값이 없는 경우 데이터로 넘어오는 id 값 중 아직 사용되지 않은 값을 대입한다.
                    // 차집합 : idListGroup[idxForAnim]에서 intersectionList가 중복되면 제외 한다.
                    List<int> differenceList2 = idListGroup[idxForAnim].Except(intersectionList).ToList();

                    int count = 0;
                    for (int i = 0; i < humanList.Count; i++)
                    {
                        if (string.IsNullOrEmpty(humanList[i].GetComponent<HumanSet>().id))
                        {
                            humanList[i].GetComponent<HumanSet>().id = differenceList2[count].ToString();
                            count++;
                        }
                    }

                    // 모델 컬러 변경 (알파 값)
                    for (int i = 0; i < humanList.Count; i++)
                    {
                        for (int j = 0; j < delateIdNum.Count; j++)
                        {
                            if (humanList[i].GetComponent<HumanSet>().id == delateIdNum[j].ToString())
                            {
                                StartCoroutine("ModelColorChange", i);
                            }
                        }
                    }

                    #region _timestamp 값 간격 갱신
                    timer += Time.deltaTime;

                    if (idxForAnim > 0)
                    {
                        currentSecond = UnixTimeToDateTime(_timestamp[idxForAnim]).Millisecond;
                        beforeSecond = UnixTimeToDateTime(_timestamp[idxForAnim - 1]).Millisecond;

                        if (time != (currentSecond - beforeSecond))
                        {
                            time = (currentSecond - beforeSecond);
                            waitingTime = time * 0.001f;
                            waitingTime = (float)Math.Round(waitingTime, 1);
                        }
                    }
                    #endregion

                    idTotal = _idTotal;
                    typeTotal = _typeTotal;

                    UpdateVelocity(idTotal, typeTotal, vecVel, queueBool);

                    // 애니메이션 속도 조절
                    if (humanTempDataListQueue.Count > 2)
                    {
                        yield return new WaitForSeconds((1.0f / _idTotal.Count) / 3);
                    }
                    else if (humanTempDataListQueue.Count > 0)
                    {
                        yield return new WaitForSeconds((1.0f / _idTotal.Count));
                    }
                    else
                    {
                        yield return new WaitForSeconds((1.0f / (_idTotal.Count - 3)));
                    }
                    idxForAnim++;
                }

                idxForAnim = 0;
            }

            yield return new WaitForSeconds(0.01f); // _timestamp 값 간격 만큼 휴식
        }
    }

    IEnumerator CoCreateHumanTest()
    {
        while (true)
        {
            List<Dictionary<long, List<ValueData>>> _valueDataDicList = new List<Dictionary<long, List<ValueData>>>();

            dataBufferText.text = $"Buffer Count : {humanTempDataListQueue.Count}";                        

            // 만약 queueBool가 true라면 안쪽 코드 실행
            #region 좌표 저장 코드
            List<List<int>> idListGroup = new List<List<int>>();
            List<List<float>> xListGroup = new List<List<float>>();
            List<List<float>> yListGroup = new List<List<float>>();
            List<List<long>> timestampGroup = new List<List<long>>();
            
            List<List<string>> typeListGroup = new List<List<string>>();

            if (humanTempDataListQueue.Count == 0)
            {
                DestroyAllHuman();
                break;
            }

            // TODO : 해당 부분에 이전 humanTempDataListQueue의 값과 TempQueue 값이 다르다면 다음 작업 수행
            _valueDataDicList = humanTempDataListQueue.Dequeue();

            StoreCoordinatesGroup(_valueDataDicList, ref idListGroup, ref xListGroup, ref yListGroup, ref timestampGroup, ref typeListGroup);

            List<string> id = new List<string>();
            List<float> x = new List<float>();
            List<float> y = new List<float>();
            List<long> timestamp = new List<long>();
            List<string> type = new List<string>();

            List<List<long>> _idTotal = new List<List<long>>();
            List<List<float>> _xTotal = new List<List<float>>();
            List<List<float>> _yTotal = new List<List<float>>();
            List<List<long>> _timestampTotal = new List<List<long>>();
            List<List<string>> _typeTotal = new List<List<string>>();

            StoreCoordinates(ref idListGroup, ref xListGroup, ref yListGroup, ref timestampGroup, ref typeListGroup, ref _idTotal, ref _xTotal, ref _yTotal, ref _timestampTotal, ref _typeTotal);

            totalIdx = _idTotal.Count;

            // _timestampTotal의 값은 2차원 리스트로 되어 있어 여으며
            // 리스트 속 값이 동일하므로 값들 중 하나의 timestamp값만 추출해서
            // _timestamp속에 대입
            List<long> _timestamp = new List<long>();
            for (int i = 0; i < _timestampTotal.Count; i++)
            {
                _timestamp.Add(_timestampTotal[i][0]);
            }

            if (_idTotal.Count <= idxForAnim)
                yield return null;

            #endregion

            while (idxForAnim <= _idTotal.Count - 1) // 만약 idxForAnim(1초에 넘어오는 애니메이션 배열 수 만큼 증가하는 변수)가 1초에 넘어오는 애니메이션 배열 수보다 작은 동안 실행.
                                                         // (_idTotal.Count : id 갯수 만큼 애니메이션 값이 넘어오므로 이 것을 비교 대상으로 사용함.)
            {
                timeStampText.text = $"timestamp : {UnixTimeToDateTime(_timestampTotal[idxForAnim][0])}";
                //timeStampText.text = $"timestamp : {_timestampTotal[idxForAnim][0]}";
                humanCountText.text = $"Human Count : {_idTotal[idxForAnim].Count}";
                //Debug.Log(_idTotal[i].Count);
                //Debug.Log("Human Count :" + _idTotal[idxForAnim].Count);

                //Debug.Log(idxForAnim);
                for (int i = 0; i < _idTotal[idxForAnim].Count; i++)
                {
                    //Debug.Log(i + " : " + _idTotal[idxForAnim][i]);
                    // 실제 위치에 맞게 좌표 수정 코드
                    float TempX = -_xTotal[idxForAnim][i];
                    //float Xvalue = (265.35f * TempX) / 100.0f;
                    float Xvalue = (270.0f * TempX) / 100.0f;
                    float TempY = _yTotal[idxForAnim][i];
                    //float Yvalue = (193f * TempY) / 100.0f;
                    float Yvalue = (180.0f * TempY) / 100.0f;

                    string Type = _typeTotal[idxForAnim][i];

                    // 이동 백터를 dic에 저장 (key 값 : 재실자 id값, Value 값 : 넘어온 x, y 좌표값)
                    if (dic.ContainsKey(_idTotal[idxForAnim][i].ToString()))
                    {
                        dic[_idTotal[idxForAnim][i].ToString()] = new Vector3(Xvalue, 0.0f, Yvalue);
                    }
                    else
                    {
                        dic.Add(_idTotal[idxForAnim][i].ToString(), new Vector3(Xvalue, 0.0f, Yvalue));
                    }

                    // 속도를 위한 딕셔너리 대입 부분 추가
                    if (dicVel.ContainsKey(_idTotal[idxForAnim][i].ToString()))
                    {
                        dicVel[_idTotal[idxForAnim][i].ToString()] = new Vector3(Xvalue, 0.0f, Yvalue);
                    }
                    else
                    {
                        dicVel.Add(_idTotal[idxForAnim][i].ToString(), new Vector3(Xvalue, 0.0f, Yvalue));
                    }

                    // 행동을 위한 딕셔너리 대입 부분 추가
                    if (dicType.ContainsKey(_idTotal[idxForAnim][i].ToString()))
                    {
                        dicType[_idTotal[idxForAnim][i].ToString()] = _typeTotal[idxForAnim][i];
                    }
                    else
                    {
                        dicType.Add(_idTotal[idxForAnim][i].ToString(), _typeTotal[idxForAnim][i]);
                    }

                    // 이동 백터의 원본을 dicVec에 저장 (key 값 : 재실자 id값, Value 값 : 넘어온 x, y 좌표값)
                    if (dicVec.ContainsKey(_idTotal[idxForAnim][i].ToString()))
                    {
                        dicVec[_idTotal[idxForAnim][i].ToString()] = new Vector3(_xTotal[idxForAnim][i], 0.0f, _yTotal[idxForAnim][i]);
                    }
                    else
                    {
                        dicVec.Add(_idTotal[idxForAnim][i].ToString(), new Vector3(_xTotal[idxForAnim][i], 0.0f, _yTotal[idxForAnim][i]));
                    }
                }

                List<string> realId = new List<string>();
                for (int j = 0; j < _idTotal[idxForAnim].Count; j++)
                {
                    realId.Add(_idTotal[idxForAnim][j].ToString());
                }

                List<string> modelId = new List<string>();
                for (int j = 0; j < humanList.Count; j++)
                {
                    modelId.Add(humanList[j].GetComponent<HumanSet>().id);
                }

                List<string> intersectionIdList = modelId.Except(realId).ToList();


                List<int> presentHumanIdData = new List<int>();
                for (int i = 0; i < humanList.Count; i++)
                {
                    if (!string.IsNullOrEmpty(humanList[i].GetComponent<HumanSet>().id))
                    {
                        presentHumanIdData.Add(int.Parse(humanList[i].GetComponent<HumanSet>().id));
                    }
                }

                // 데이터로 넘어오는 id 값과 현재 재실자의 id 값의 교집합
                List<int> intersectionList = idListGroup[idxForAnim].Intersect(presentHumanIdData).ToList();

                // 현재 재실자의 id 값과 데이터로 넘어오는 id 값 차집합
                List<int> pastDifferenceList = presentHumanIdData.Except(idListGroup[idxForAnim]).ToList();

                List<int> delateIdNum = new List<int>();
                foreach (var child in pastDifferenceList)
                    delateIdNum.Add(child);

                int unknownCount = 0;
                int walkCount = 0;
                int runCount = 0;
                int sitdownCount = 0;
                int standCount = 0;
                int falldownCount = 0;

                foreach (var child in _typeTotal[idxForAnim])
                {
                    switch (child)
                    {
                        case "unknown":
                            unknownCount++;
                            break;
                        case "walk":
                            walkCount++;
                            break;
                        case "run":
                            runCount++;
                            break;
                        case "sitdown":
                            sitdownCount++;
                            break;
                        case "stand":
                            standCount++;
                            break;
                        case "falldown":
                            falldownCount++;
                            break;
                    }
                }

                unknown.text = $"{unknownCount} 명";
                walk.text = $"{walkCount} 명"; 
                run.text = $"{runCount} 명"; 
                sitdown.text = $"{sitdownCount} 명"; 
                stand.text = $"{standCount} 명"; 
                falldown.text = $"{falldownCount} 명"; 

                // 우선 제거
                // 모델 컬러 변경 (알파 값)
                for (int i = 0; i < humanList.Count; i++)
                {
                    for (int j = 0; j < delateIdNum.Count; j++)
                    {
                        if (humanList[i].GetComponent<HumanSet>().id == delateIdNum[j].ToString())
                        {
                            GameObject tempGo = humanList[i];
                            humanList.RemoveAt(i);
                            Managers.Resource.Destroy(tempGo);
                        }
                    }
                }

                // 만약 생성되어 있는 모델의 수보다 id의 수가 크다면 그만큼 새로 생성
                if (_idTotal[idxForAnim].Count > humanList.Count)
                {
                    int tempNum = idListGroup[idxForAnim].Count - humanList.Count;

                    for (int i = 0; i < tempNum; i++)
                    {
                        Vector3 vector;

                        if (dic.TryGetValue(_idTotal[idxForAnim][i].ToString(), out vector)) // 여기가 바뀌어야함
                        {
                            GameObject go;
                            go = Managers.Resource.Instantiate("Human_Prefab", humanGroup.transform);

                            //Renderer[] renderer = go.transform.GetComponentsInChildren<Renderer>();

                            humanList.Add(go);

                            humanList[i].GetComponent<HumanSet>().enabled = true;

                            if (go.transform.position == Vector3.zero)
                                go.SetActive(false);
                        }
                    }
                }

                // 2. 기존의 재실자 모델 중 id 값이 없는 경우 데이터로 넘어오는 id 값 중 아직 사용되지 않은 값을 대입한다.
                // 차집합 : idListGroup[idxForAnim]에서 intersectionList가 중복되면 제외 한다.
                List<int> differenceList2 = idListGroup[idxForAnim].Except(intersectionList).ToList();
                List<int> differenceList3 = idListGroup[idxForAnim].Except(presentHumanIdData).ToList();

                int count = 0;
                for (int i = 0; i < humanList.Count; i++)
                {
                    if (string.IsNullOrEmpty(humanList[i].GetComponent<HumanSet>().id))
                    {
                        //humanList[i].GetComponent<HumanSet>().id = differenceList2[count].ToString();
                        humanList[i].GetComponent<HumanSet>().id = differenceList3[count].ToString();
                        count++;
                    }
                }

                #region _timestamp 값 간격 갱신
                timer += Time.deltaTime;

                if (idxForAnim > 0)
                {
                    currentSecond = UnixTimeToDateTime(_timestamp[idxForAnim]).Millisecond;
                    beforeSecond = UnixTimeToDateTime(_timestamp[idxForAnim - 1]).Millisecond;

                    if (time != (currentSecond - beforeSecond))
                    {
                        time = (currentSecond - beforeSecond);
                        waitingTime = time * 0.001f;
                        waitingTime = (float)Math.Round(waitingTime, 1);
                    }
                }
                #endregion

                idTotal = _idTotal;
                typeTotal = _typeTotal;

                UpdateVelocity(idTotal, typeTotal, vecVel);

                if (humanTempDataListQueue.Count > 2)
                {
                    yield return new WaitForSeconds((1.0f / _idTotal.Count) / 3);
                }
                else
                {
                    yield return new WaitForSeconds(1.0f / _idTotal.Count);
                }

                idxForAnim++;
            }

                idxForAnim = 0;

                // 만약 사람이 아무도 없다면 (totalIdx가 0이라면) 전체 제거 해주는 함수
                // 만약 사람이 아무도 없다면 (totalIdx가 0이라면) 전체 제거
                if (totalIdx <= 0)
                {
                    DestroyAllHuman();
                }
            

            yield return new WaitForSeconds(0.01f); // _timestamp 값 간격 만큼 휴식
        }
    }


    IEnumerator ModelColorChange(int i)
    {
        GameObject tempGo;
        humanList[i].GetComponent<HumanSet>().enabled = false;
        tempGo = humanList[i];
        humanList.RemoveAt(i);

        Color tempColor = tempGo.transform.GetChild(0).GetComponent<MeshRenderer>().material.color;

        // 박스형태
        for (int j = 0; j < 20; j++)
        {
            tempColor.a -= 0.05f;
            tempGo.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = tempColor;
            yield return new WaitForSeconds(0.01f);
        }

        yield return null;
        Managers.Resource.Destroy(tempGo);
    }

    #region 좌표 리스트엔 대입 부분 함수
    public void StoreCoordinatesGroup(List<Dictionary<long, List<ValueData>>> _valueDataDicList, ref List<List<int>> idListGroup, ref List<List<float>> xListGroup, ref List<List<float>> yListGroup, ref List<List<long>> timestampGroup, ref List<List<string>> typeListGroup)
    {
        foreach (Dictionary<long, List<ValueData>> child in _valueDataDicList)
        {
            foreach (KeyValuePair<long, List<ValueData>> childDic in child)
            {
                List<int> idList = new List<int>();
                List<float> xList = new List<float>();
                List<float> yList = new List<float>();
                List<long> timestampList = new List<long>();
                List<string> typeList = new List<string>();

                foreach (ValueData childValue in childDic.Value)
                {
                    idList.Add(childValue.id);
                    xList.Add(childValue.pos.x);
                    yList.Add(childValue.pos.y);
                    timestampList.Add(childValue.timestamp);
                    typeList.Add(childValue.type);
                }
                idListGroup.Add(idList);
                xListGroup.Add(xList);
                yListGroup.Add(yList);
                timestampGroup.Add(timestampList);
                typeListGroup.Add(typeList);
            }
        }
    }

    public void StoreCoordinates(ref List<List<int>> idListGroup, ref List<List<float>> xListGroup, ref List<List<float>> yListGroup, ref List<List<long>> timestampGroup, ref List<List<string>> typeListGroup, ref List<List<long>> _idTotal, ref List<List<float>> _xTotal, ref List<List<float>> _yTotal, ref List<List<long>> _timestampTotal, ref List<List<string>> _typeTotal)
    {
        for (int i = 0; i < idListGroup.Count; i++)
        {
            List<long> idTemp = new List<long>();
            List<float> xTemp = new List<float>();
            List<float> yTemp = new List<float>();
            List<long> timestampTemp = new List<long>();
            List<string> typeTemp = new List<string>();

            for (int j = 0; j < idListGroup[i].Count; j++)
            {
                idTemp.Add(idListGroup[i][j]);
                xTemp.Add(xListGroup[i][j]);
                yTemp.Add(yListGroup[i][j]);
                timestampTemp.Add(timestampGroup[i][j]);
                typeTemp.Add(typeListGroup[i][j]);
            }
            _idTotal.Add(idTemp);
            _xTotal.Add(xTemp);
            _yTotal.Add(yTemp);
            _timestampTotal.Add(timestampTemp);
            _typeTotal.Add(typeTemp);
        }
    }
    #endregion

    public void DestroyAllHuman() // 사람 전체 제거
    {
        for (int i = 0; i < humanList.Count; i++)
        {
            Managers.Resource.Destroy(humanList[i]);
        }

        humanList.Clear();
    }

    public void UpdateMoving(List<List<long>> idTotal, Vector3 _vec, bool queueBool = true)
    {
        if (!queueBool)
            return;

        List<string> str = new List<string>();

        for (int i = 0; i < humanList.Count; i++)
        {
            str.Add(humanList[i].GetComponent<HumanSet>().id);
        }

        if (idxForAnim < idTotal.Count) // 아직 idxForAnim의 수가 총 애니메이션 배열의 수보다 작다면 아래 코드 실행
        {
            for (int i = 0; i < idTotal[idxForAnim].Count; i++)
            {
                if (dic.TryGetValue(idTotal[idxForAnim][i].ToString(), out _vec))
                {
                    for (int j = 0; j < humanList.Count; j++)
                    {
                        if (humanList[j].GetComponent<HumanSet>().id == idTotal[idxForAnim][i].ToString())
                        {
                            if (_vec - humanList[j].transform.position != Vector3.zero)
                            {
                                // 회전 애니메이션 실행.
                                humanList[j].transform.rotation = Quaternion.Lerp(humanList[j].transform.rotation, Quaternion.LookRotation(_vec - humanList[j].transform.position), 0.2f);

                                ModelAnimation(_vec, humanList[j]);
                            }
                        }
                    }
                }
            }
        }
    }

    public void UpdateVelocity(List<List<long>> idTotal, List<List<string>> typeTotal, Vector3 _vec, bool queueBool = true)
    {
        if (!queueBool)
            return;

        List<string> str = new List<string>();

        for (int i = 0; i < humanList.Count; i++)
        {
            str.Add(humanList[i].GetComponent<HumanSet>().id);
        }

        if (idxForAnim < idTotal.Count) // 아직 idxForAnim의 수가 총 애니메이션 배열의 수보다 작다면 아래 코드 실행
        {
            for (int i = 0; i < idTotal[idxForAnim].Count; i++)
            {
                if (dicVel.TryGetValue(idTotal[idxForAnim][i].ToString(), out _vec))
                {
                    for (int j = 0; j < humanList.Count; j++)
                    {
                        if (humanList[j].GetComponent<HumanSet>().id == idTotal[idxForAnim][i].ToString())
                        {
                            if (_vec - humanList[j].transform.position != Vector3.zero)
                            {
                                // 회전 애니메이션 실행.
                                velocity = VelocityFunc(_vec, humanList[j]);

                                string distanceStr = humanList[j].GetComponent<HumanSet>().DistanceStr;
                                string nameStr = idTotal[idxForAnim][i].ToString();
                                string type = typeTotal[idxForAnim][i].ToString();
                                //string behave = 

                                humanList[j].transform.GetChild(1).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"{distanceStr} / {string.Format("{0:0.#}", (velocity))}";

                                if (idToggle.isOn == true)
                                {
                                    humanList[j].transform.GetChild(1).transform.GetChild(1).GetComponent<Image>().enabled = true;
                                    humanList[j].transform.GetChild(1).transform.GetChild(2).GetComponent<TextMeshProUGUI>().enabled = true;
                                    humanList[j].transform.GetChild(1).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = $"ID : {nameStr}";


                                    humanList[j].transform.GetChild(1).transform.GetChild(3).GetComponent<Image>().enabled = true;
                                    humanList[j].transform.GetChild(1).transform.GetChild(5).GetComponent<Image>().enabled = true;
                                    humanList[j].transform.GetChild(1).transform.GetChild(4).GetComponent<TextMeshProUGUI>().enabled = true;
                                    humanList[j].transform.GetChild(1).transform.GetChild(6).GetComponent<TextMeshProUGUI>().enabled = true;
                                    humanList[j].transform.GetChild(1).transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = type;

                                    Vector3 originVec;
                                    if (dicVec.TryGetValue(idTotal[idxForAnim][i].ToString(), out originVec))
                                    {
                                        humanList[j].transform.GetChild(1).transform.GetChild(6).GetComponent<TextMeshProUGUI>().text =
                                            $"X : {originVec.x.ToString()} / Y : {originVec.z.ToString()}";
                                    }
                                }
                                else
                                {
                                    humanList[j].transform.GetChild(1).transform.GetChild(1).GetComponent<Image>().enabled = false;
                                    humanList[j].transform.GetChild(1).transform.GetChild(2).GetComponent<TextMeshProUGUI>().enabled = false;

                                    humanList[j].transform.GetChild(1).transform.GetChild(3).GetComponent<Image>().enabled = false;
                                    humanList[j].transform.GetChild(1).transform.GetChild(5).GetComponent<Image>().enabled = false;
                                    humanList[j].transform.GetChild(1).transform.GetChild(4).GetComponent<TextMeshProUGUI>().enabled = false;
                                    humanList[j].transform.GetChild(1).transform.GetChild(6).GetComponent<TextMeshProUGUI>().enabled = false;
                                }

                                textProColor = humanList[j].transform.GetChild(1).transform.GetChild(0).GetComponent<TextMeshProUGUI>();

                                textProColor.color = VelocityColorFunc(velocity);
                            }
                        }
                    }
                }
            }
        }
    }


    // 속도를 활용한 Text 색상 변환
    Color _colors;
    float _valocityfloat;
    float _valocity;
    TextMeshProUGUI textProColor;
    Color VelocityColorFunc(double _valocityDouble)
    {
        _valocityDouble = (Math.Round(_valocityDouble, 2));
        _valocityfloat = (float)_valocityDouble;

        _valocityfloat *= 2;

        _valocity = Mathf.Clamp(_valocityfloat, 0, 10);
        //Debug.Log(_valocity / 10);
        _colors.r = _valocity / 10;
        //_colors.r = (float)GetDataAndMoveModel.Valocity;
        _colors.g = 0; // Random.Range(0, 100) * 0.01f;
        _colors.b = 0; // Random.Range(0, 100) * 0.01f;
        _colors.a = 1;
        //Debug.Log(_colors);

        return _colors;
        //textColor.material.color = _colors;
    }

    // 속도 구하는 함수의 핵심 부분
    double VelocityFunc(Vector3 _vec, GameObject _go)
    {
        double _velocity;

        currentPosition = _vec / 10;
        oldPosition = _go.transform.position / 10;
        var dis = (currentPosition - oldPosition);
        var S = Math.Sqrt(Math.Pow(dis.x, 2) + Math.Pow(dis.y, 2) + Math.Pow(dis.z, 2));
        //var S = (currentPosition - oldPosition).magnitude;
        var T = 1.0f / idTotal.Count;
        //_velocity = S / T;
        _velocity = S / T;
        //_velocity = S;
        return _velocity;
    }


    private Vector3 oldPosition;
    private Vector3 currentPosition;
    private double velocity;
    void ModelAnimation(Vector3 vec, GameObject obj)
    {
        oldPosition = obj.transform.position;
        if (obj.transform.position == Vector3.zero)
        {
            obj.transform.position = vec;
        }
        else
        {
            Vector3 dir = vec - obj.transform.position;
            float dist = dir.magnitude * 10 * 0.01f;

            Animator anim = obj.transform.GetComponentInChildren<Animator>();
            if (anim != null && anim.gameObject.activeSelf)
            {
                anim.SetFloat("wait_walk_run_ratio", dir.magnitude);
                anim.Play("Waut_Walk_Run");
            }

            obj.SetActive(true);
            obj.transform.position += dir.normalized * dist;
        }
    }

    public DateTime UnixTimeToDateTime(long unixtime) // timestemp 값을 일반적인 시간 값으로 변환해주는 함수.
    {
        System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
        dtDateTime = dtDateTime.AddMilliseconds(unixtime).ToLocalTime();
        return dtDateTime;
    }
    #endregion
}