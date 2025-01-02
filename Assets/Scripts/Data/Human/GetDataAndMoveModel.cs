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
    #region �ʵ�
    public bool RunTestVersion;

    public static string jsonText;

    TextAsset textAsset;

    Queue<List<Dictionary<long, List<ValueData>>>> humanTempDataListQueue = new Queue<List<Dictionary<long, List<ValueData>>>>();

    [Header("- For Human")]
    [SerializeField] GameObject humanGroup;

    List<GameObject> humanList = new List<GameObject>();

    // �ð� ���� �뵵 ����
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

    #region ���ٿ� Ŭ����
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
                            unknown.text = $"0 ��";
                            walk.text = $"0 ��"; ;
                            run.text = $"0 ��"; ;
                            sitdown.text = $"0 ��"; ;
                            stand.text = $"0 ��"; ;
                            falldown.text = $"0 ��"; ;
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
            #region �ۼ��Ǿ� �ִ� Json ������ Ȱ�� �ڵ�
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

    #region ������ (�ִϸ��̼� �ڵ�)
    Dictionary<string, Vector3> dic = new Dictionary<string, Vector3>();
    Dictionary<string, Vector3> dicVec = new Dictionary<string, Vector3>();
    Dictionary<string, string> dicType = new Dictionary<string, string>();
    Dictionary<string, Vector3> dicVel = new Dictionary<string, Vector3>();
    List<List<long>> idTotal = new List<List<long>>();
    List<List<string>> typeTotal = new List<List<string>>();
    Vector3 vec = new Vector3();

    long time = 0;

    int idxForAnim = 0; // �ʴ� ������ ��ǥ�� �ޱ� ���� �ε���
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
            //���� �ڵ� : ���۰� 30�� ���̸� �Ҹ����� true�� 0�̸� 30�� ���� ������ ���
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

            // ���� queueBool�� true��� ���� �ڵ� ����
            if (queueBool == true)// && bufferTextOnSign == true)
            {
                #region ��ǥ ���� �ڵ�
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
                // TODO : �ش� �κп� ���� humanTempDataListQueue�� ���� TempQueue ���� �ٸ��ٸ� ���� �۾� ����
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

                // _timestampTotal�� ���� 2���� ����Ʈ�� �Ǿ� �־� ������
                // ����Ʈ �� ���� �����ϹǷ� ���� �� �ϳ��� timestamp���� �����ؼ�
                // _timestamp�ӿ� ����
                List<long> _timestamp = new List<long>();
                for (int i = 0; i < _timestampTotal.Count; i++)
                {
                    _timestamp.Add(_timestampTotal[i][0]);
                }

                if (_idTotal.Count <= idxForAnim)
                    yield return null;

                #endregion

                while (idxForAnim <= _idTotal.Count - 1) // ���� idxForAnim(1�ʿ� �Ѿ���� �ִϸ��̼� �迭 �� ��ŭ �����ϴ� ����)�� 1�ʿ� �Ѿ���� �ִϸ��̼� �迭 ������ ���� ���� ����.
                                                         // (_idTotal.Count : id ���� ��ŭ �ִϸ��̼� ���� �Ѿ���Ƿ� �� ���� �� ������� �����.)
                {
                    //timeStampText.text = $"timestamp : {UnixTimeToDateTime(_timestampTotal[idxForAnim][0])}";
                    timeStampText.text = $"timestamp : {_timestampTotal[idxForAnim][0]}";
                    humanCountText.text = $"Human Count : {_idTotal[idxForAnim].Count}";
                    //Debug.Log(_idTotal[i].Count);
                    

                    //Debug.Log(idxForAnim);
                    for (int i = 0; i < _idTotal[idxForAnim].Count; i++)
                    {
                        //Debug.Log(i + " : " + _idTotal[idxForAnim][i]);
                        // ���� ��ġ�� �°� ��ǥ ���� �ڵ�
                        float TempX = -_xTotal[idxForAnim][i];
                        //float Xvalue = (265.35f * TempX) / 100.0f;
                        float Xvalue = (270.0f * TempX) / 100.0f;
                        float TempY = _yTotal[idxForAnim][i];
                        //float Yvalue = (193f * TempY) / 100.0f;
                        float Yvalue = (180.0f * TempY) / 100.0f;

                        string Type = _typeTotal[idxForAnim][i];

                        // �̵� ���͸� dic�� ���� (key �� : ����� id��, Value �� : �Ѿ�� x, y ��ǥ��)
                        if (dic.ContainsKey(_idTotal[idxForAnim][i].ToString()))
                        {
                            dic[_idTotal[idxForAnim][i].ToString()] = new Vector3(Xvalue, 0.0f, Yvalue);
                        }
                        else
                        {
                            dic.Add(_idTotal[idxForAnim][i].ToString(), new Vector3(Xvalue, 0.0f, Yvalue));
                        }

                        // �ӵ��� ���� ��ųʸ� ���� �κ� �߰�
                        if (dicVel.ContainsKey(_idTotal[idxForAnim][i].ToString()))
                        {
                            dicVel[_idTotal[idxForAnim][i].ToString()] = new Vector3(Xvalue, 0.0f, Yvalue);
                        }
                        else
                        {
                            dicVel.Add(_idTotal[idxForAnim][i].ToString(), new Vector3(Xvalue, 0.0f, Yvalue));
                        }

                        // �ൿ�� ���� ��ųʸ� ���� �κ� �߰�
                        if (dicType.ContainsKey(_idTotal[idxForAnim][i].ToString()))
                        {
                            dicType[_idTotal[idxForAnim][i].ToString()] = _typeTotal[idxForAnim][i];
                        }
                        else
                        {
                            dicType.Add(_idTotal[idxForAnim][i].ToString(), _typeTotal[idxForAnim][i]);
                        }

                        // �̵� ������ ������ dicVec�� ���� (key �� : ����� id��, Value �� : �Ѿ�� x, y ��ǥ��)
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

                    // �����ͷ� �Ѿ���� id ���� ���� ������� id ���� ������
                    List<int> intersectionList = idListGroup[idxForAnim].Intersect(presentHumanIdData).ToList();
                    // ������ : presentHumanIdData���� intersectionList �ߺ��Ǹ� ���� �Ѵ�.
                    List<int> differenceList = presentHumanIdData.Except(intersectionList).ToList();//presentHumanIdData.Except(intersectionList).ToList();

                    List<int> delateIdNum = new List<int>();
                    foreach (var child in differenceList)
                        delateIdNum.Add(child);

                    // ���� �����Ǿ� �ִ� ���� ������ id�� ���� ũ�ٸ� �׸�ŭ ���� ����
                    if (_idTotal[idxForAnim].Count > humanList.Count)
                    {
                        int tempNum = idListGroup[idxForAnim].Count - humanList.Count;

                        for (int i = 0; i < tempNum; i++)
                        {
                            Vector3 vector;

                            if (dic.TryGetValue(_idTotal[idxForAnim][i].ToString(), out vector)) // ���Ⱑ �ٲ�����
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

                    // 2. ������ ����� �� �� id ���� ���� ��� �����ͷ� �Ѿ���� id �� �� ���� ������ ���� ���� �����Ѵ�.
                    // ������ : idListGroup[idxForAnim]���� intersectionList�� �ߺ��Ǹ� ���� �Ѵ�.
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

                    // �� �÷� ���� (���� ��)
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

                    #region _timestamp �� ���� ����
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

                    // �ִϸ��̼� �ӵ� ����
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

            yield return new WaitForSeconds(0.01f); // _timestamp �� ���� ��ŭ �޽�
        }
    }

    IEnumerator CoCreateHumanTest()
    {
        while (true)
        {
            List<Dictionary<long, List<ValueData>>> _valueDataDicList = new List<Dictionary<long, List<ValueData>>>();

            dataBufferText.text = $"Buffer Count : {humanTempDataListQueue.Count}";                        

            // ���� queueBool�� true��� ���� �ڵ� ����
            #region ��ǥ ���� �ڵ�
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

            // TODO : �ش� �κп� ���� humanTempDataListQueue�� ���� TempQueue ���� �ٸ��ٸ� ���� �۾� ����
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

            // _timestampTotal�� ���� 2���� ����Ʈ�� �Ǿ� �־� ������
            // ����Ʈ �� ���� �����ϹǷ� ���� �� �ϳ��� timestamp���� �����ؼ�
            // _timestamp�ӿ� ����
            List<long> _timestamp = new List<long>();
            for (int i = 0; i < _timestampTotal.Count; i++)
            {
                _timestamp.Add(_timestampTotal[i][0]);
            }

            if (_idTotal.Count <= idxForAnim)
                yield return null;

            #endregion

            while (idxForAnim <= _idTotal.Count - 1) // ���� idxForAnim(1�ʿ� �Ѿ���� �ִϸ��̼� �迭 �� ��ŭ �����ϴ� ����)�� 1�ʿ� �Ѿ���� �ִϸ��̼� �迭 ������ ���� ���� ����.
                                                         // (_idTotal.Count : id ���� ��ŭ �ִϸ��̼� ���� �Ѿ���Ƿ� �� ���� �� ������� �����.)
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
                    // ���� ��ġ�� �°� ��ǥ ���� �ڵ�
                    float TempX = -_xTotal[idxForAnim][i];
                    //float Xvalue = (265.35f * TempX) / 100.0f;
                    float Xvalue = (270.0f * TempX) / 100.0f;
                    float TempY = _yTotal[idxForAnim][i];
                    //float Yvalue = (193f * TempY) / 100.0f;
                    float Yvalue = (180.0f * TempY) / 100.0f;

                    string Type = _typeTotal[idxForAnim][i];

                    // �̵� ���͸� dic�� ���� (key �� : ����� id��, Value �� : �Ѿ�� x, y ��ǥ��)
                    if (dic.ContainsKey(_idTotal[idxForAnim][i].ToString()))
                    {
                        dic[_idTotal[idxForAnim][i].ToString()] = new Vector3(Xvalue, 0.0f, Yvalue);
                    }
                    else
                    {
                        dic.Add(_idTotal[idxForAnim][i].ToString(), new Vector3(Xvalue, 0.0f, Yvalue));
                    }

                    // �ӵ��� ���� ��ųʸ� ���� �κ� �߰�
                    if (dicVel.ContainsKey(_idTotal[idxForAnim][i].ToString()))
                    {
                        dicVel[_idTotal[idxForAnim][i].ToString()] = new Vector3(Xvalue, 0.0f, Yvalue);
                    }
                    else
                    {
                        dicVel.Add(_idTotal[idxForAnim][i].ToString(), new Vector3(Xvalue, 0.0f, Yvalue));
                    }

                    // �ൿ�� ���� ��ųʸ� ���� �κ� �߰�
                    if (dicType.ContainsKey(_idTotal[idxForAnim][i].ToString()))
                    {
                        dicType[_idTotal[idxForAnim][i].ToString()] = _typeTotal[idxForAnim][i];
                    }
                    else
                    {
                        dicType.Add(_idTotal[idxForAnim][i].ToString(), _typeTotal[idxForAnim][i]);
                    }

                    // �̵� ������ ������ dicVec�� ���� (key �� : ����� id��, Value �� : �Ѿ�� x, y ��ǥ��)
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

                // �����ͷ� �Ѿ���� id ���� ���� ������� id ���� ������
                List<int> intersectionList = idListGroup[idxForAnim].Intersect(presentHumanIdData).ToList();

                // ���� ������� id ���� �����ͷ� �Ѿ���� id �� ������
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

                unknown.text = $"{unknownCount} ��";
                walk.text = $"{walkCount} ��"; 
                run.text = $"{runCount} ��"; 
                sitdown.text = $"{sitdownCount} ��"; 
                stand.text = $"{standCount} ��"; 
                falldown.text = $"{falldownCount} ��"; 

                // �켱 ����
                // �� �÷� ���� (���� ��)
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

                // ���� �����Ǿ� �ִ� ���� ������ id�� ���� ũ�ٸ� �׸�ŭ ���� ����
                if (_idTotal[idxForAnim].Count > humanList.Count)
                {
                    int tempNum = idListGroup[idxForAnim].Count - humanList.Count;

                    for (int i = 0; i < tempNum; i++)
                    {
                        Vector3 vector;

                        if (dic.TryGetValue(_idTotal[idxForAnim][i].ToString(), out vector)) // ���Ⱑ �ٲ�����
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

                // 2. ������ ����� �� �� id ���� ���� ��� �����ͷ� �Ѿ���� id �� �� ���� ������ ���� ���� �����Ѵ�.
                // ������ : idListGroup[idxForAnim]���� intersectionList�� �ߺ��Ǹ� ���� �Ѵ�.
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

                #region _timestamp �� ���� ����
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

                // ���� ����� �ƹ��� ���ٸ� (totalIdx�� 0�̶��) ��ü ���� ���ִ� �Լ�
                // ���� ����� �ƹ��� ���ٸ� (totalIdx�� 0�̶��) ��ü ����
                if (totalIdx <= 0)
                {
                    DestroyAllHuman();
                }
            

            yield return new WaitForSeconds(0.01f); // _timestamp �� ���� ��ŭ �޽�
        }
    }


    IEnumerator ModelColorChange(int i)
    {
        GameObject tempGo;
        humanList[i].GetComponent<HumanSet>().enabled = false;
        tempGo = humanList[i];
        humanList.RemoveAt(i);

        Color tempColor = tempGo.transform.GetChild(0).GetComponent<MeshRenderer>().material.color;

        // �ڽ�����
        for (int j = 0; j < 20; j++)
        {
            tempColor.a -= 0.05f;
            tempGo.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = tempColor;
            yield return new WaitForSeconds(0.01f);
        }

        yield return null;
        Managers.Resource.Destroy(tempGo);
    }

    #region ��ǥ ����Ʈ�� ���� �κ� �Լ�
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

    public void DestroyAllHuman() // ��� ��ü ����
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

        if (idxForAnim < idTotal.Count) // ���� idxForAnim�� ���� �� �ִϸ��̼� �迭�� ������ �۴ٸ� �Ʒ� �ڵ� ����
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
                                // ȸ�� �ִϸ��̼� ����.
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

        if (idxForAnim < idTotal.Count) // ���� idxForAnim�� ���� �� �ִϸ��̼� �迭�� ������ �۴ٸ� �Ʒ� �ڵ� ����
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
                                // ȸ�� �ִϸ��̼� ����.
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


    // �ӵ��� Ȱ���� Text ���� ��ȯ
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

    // �ӵ� ���ϴ� �Լ��� �ٽ� �κ�
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

    public DateTime UnixTimeToDateTime(long unixtime) // timestemp ���� �Ϲ����� �ð� ������ ��ȯ���ִ� �Լ�.
    {
        System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
        dtDateTime = dtDateTime.AddMilliseconds(unixtime).ToLocalTime();
        return dtDateTime;
    }
    #endregion
}