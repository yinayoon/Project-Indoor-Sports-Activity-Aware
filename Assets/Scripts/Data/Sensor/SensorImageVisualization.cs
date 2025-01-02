using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SensorImageVisualization : MonoBehaviour
{
    string url;

    [Header("- Start Input Field")]
    [SerializeField] InputField sInputField_Y;
    [SerializeField] InputField sInputField_M;
    [SerializeField] InputField sInputField_D;
    [SerializeField] InputField sInputField_Hh;
    [SerializeField] InputField sInputField_Mm;
    [SerializeField] InputField sInputField_Ss;

    [Header("- End Input Field")]
    [SerializeField] InputField eInputField_Y;
    [SerializeField] InputField eInputField_M;
    [SerializeField] InputField eInputField_D;
    [SerializeField] InputField eInputField_Hh;
    [SerializeField] InputField eInputField_Mm;
    [SerializeField] InputField eInputField_Ss;

    [Header("- ETC GameObject")]
    [SerializeField] GameObject errorMessage;

    [Header("- GUI")]
    [SerializeField] Text serialText;
    [SerializeField] Image img;
    [SerializeField] Dropdown dropdown;
    [SerializeField] GameObject DataGui;

    // 시간
    string startY;
    string startM;
    string startD;
    string startHh;
    string startMm;
    string startSs;

    string endY;
    string endM;
    string endD;
    string endHh;
    string endMm;
    string endSs;

    string serial;
    string sensor;

    // Start is called before the first frame update
    void Start()
    {
        serialText.text = null;

        #region 날짜 시간 관련 문자열 데이터
        DateTime StartDate = DateTime.Now.AddMinutes(-20);
        DateTime EndDate = DateTime.Now;

        startY = StartDate.Year.ToString();
        startM = string.Format("{0:D2}", StartDate.Month);
        startD = string.Format("{0:D2}", StartDate.Day);
        startHh = string.Format("{0:D2}", StartDate.Hour);
        startMm = string.Format("{0:D2}", StartDate.Minute);
        startSs = string.Format("{0:D2}", StartDate.Second);

        endY = EndDate.Year.ToString();
        endM = string.Format("{0:D2}", EndDate.Month);
        endD = string.Format("{0:D2}", EndDate.Day);
        endHh = string.Format("{0:D2}", EndDate.Hour);
        endMm = string.Format("{0:D2}", EndDate.Minute);
        endSs = string.Format("{0:D2}", EndDate.Second);
        #endregion

        sensor = "not_literal_or(tm)";
        serial = null;

        sInputField_Y.text = startY;
        sInputField_M.text = startM;
        sInputField_D.text = startD;
        sInputField_Hh.text = startHh;
        sInputField_Mm.text = startMm;
        sInputField_Ss.text = startSs;

        eInputField_Y.text = endY;
        eInputField_M.text = endM;
        eInputField_D.text = endD;
        eInputField_Hh.text = endHh;
        eInputField_Mm.text = endMm;
        eInputField_Ss.text = endSs;

        if (serial != null)
        {
            url = $"http://io.energyiotlab.com:54242/q?" +
                $"start={sInputField_Y.text}/{sInputField_M.text}/{sInputField_D.text}-{sInputField_Hh.text}:{sInputField_Mm.text}:{sInputField_Ss.text}" +
                $"&" +
                $"end={eInputField_Y.text}/{eInputField_M.text}/{eInputField_D.text}-{eInputField_Hh.text}:{eInputField_Mm.text}:{eInputField_Ss.text}" +
                $"&m=sum:kw-iaq-sensor-kiot%7B" +
                $"sensor={sensor}," +
                $"serial={serial}" +
                $"%7D&o=&yrange=%5B0:%5D&key=out%20center%20top%20horiz&wxh=1024x768&style=linespoint&png";

            GatData();
        }
    }

    bool initializationFlag = false;
    public void Update()
    {
        if (DataGui.activeSelf)
        {
            if (initializationFlag == false)
            {
                DateInirialization();

                initializationFlag = true;
            }

            CompareToBeforeDataToNowData();
        }
    }

    void DateInirialization()
    {        
        DateTime StartDate = DateTime.Now.AddMinutes(-20);
        DateTime EndDate = DateTime.Now;

        startY = StartDate.Year.ToString();
        startM = string.Format("{0:D2}", StartDate.Month);
        startD = string.Format("{0:D2}", StartDate.Day);
        startHh = string.Format("{0:D2}", StartDate.Hour);
        startMm = string.Format("{0:D2}", StartDate.Minute);
        startSs = string.Format("{0:D2}", StartDate.Second);

        endY = EndDate.Year.ToString();
        endM = string.Format("{0:D2}", EndDate.Month);
        endD = string.Format("{0:D2}", EndDate.Day);
        endHh = string.Format("{0:D2}", EndDate.Hour);
        endMm = string.Format("{0:D2}", EndDate.Minute);
        endSs = string.Format("{0:D2}", EndDate.Second);

        dropdown.value = 0;
        serial = null;

        sInputField_Y.text = startY;
        sInputField_M.text = startM;
        sInputField_D.text = startD;
        sInputField_Hh.text = startHh;
        sInputField_Mm.text = startMm;
        sInputField_Ss.text = startSs;

        eInputField_Y.text = endY;
        eInputField_M.text = endM;
        eInputField_D.text = endD;
        eInputField_Hh.text = endHh;
        eInputField_Mm.text = endMm;
        eInputField_Ss.text = endSs;
    }

    #region 한자리수를 두자리수로 변경하기 위한 변수 필드 
    string sy;
    string sm;
    string sd;
    string shh;
    string smm;
    string sss;
    string ey;
    string em;
    string ed;
    string ehh;
    string emm;
    string ess;
    #endregion
    void CompareToBeforeDataToNowData()
    {
        if (ChangeDropdownValue(dropdown.options[dropdown.value].text) != sensor)
        {
            sensor = ChangeDropdownValue(dropdown.options[dropdown.value].text);

            GatData();
        }
        else if (sy != sInputField_Y.text.PadLeft(2, '0') || sm != sInputField_M.text.PadLeft(2, '0') || 
            sd != sInputField_D.text.PadLeft(2, '0') || shh != sInputField_Hh.text.PadLeft(2, '0') || 
            smm != sInputField_Mm.text.PadLeft(2, '0') || sss != sInputField_Ss.text.PadLeft(2, '0') ||
            ey != eInputField_Y.text.PadLeft(2, '0') || em != eInputField_M.text.PadLeft(2, '0') || 
            ed != eInputField_D.text.PadLeft(2, '0') || ehh != eInputField_Hh.text.PadLeft(2, '0') || 
            emm != eInputField_Mm.text.PadLeft(2, '0') || ess != eInputField_Ss.text.PadLeft(2, '0'))
        {
            GatData();
        }
        // 센서 정보를 넘어온 데이터로 변경
        if (serial != RaycastIndoorSensor.SensorSerial)
        {
            serial = RaycastIndoorSensor.SensorSerial;

            if (serial != null)
                serialText.text = serial;

            GatData();
        }
    }

    void GatData()
    {
        #region 한자리 수 두자리로 변경 코드
        sy = int.Parse(sInputField_Y.text).ToString("D4");

        sm = sInputField_M.text;
        sd = sInputField_D.text;
        shh = sInputField_Hh.text;
        smm = sInputField_Mm.text;
        sss = sInputField_Ss.text;

        ey = int.Parse(eInputField_Y.text).ToString("D4");

        em = eInputField_M.text;
        ed = eInputField_D.text;
        ehh = eInputField_Hh.text;
        emm = eInputField_Mm.text;
        ess = eInputField_Ss.text;

        if (sm.Length == 1)
            sm = sm.PadLeft(2, '0');
        else
            sm = sInputField_M.text;

        if (sd.Length == 1)
            sd = sd.PadLeft(2, '0');
        else
            sd = sInputField_D.text;

        if (shh.Length == 1)
            shh = shh.PadLeft(2, '0');
        else
            shh = sInputField_Hh.text;

        if (smm.Length == 1)
            smm = smm.PadLeft(2, '0');
        else
            smm = sInputField_Mm.text;

        if (sss.Length == 1)
            sss = sss.PadLeft(2, '0');
        else
            sss = sInputField_Ss.text;


        if (em.Length == 1)
            em = em.PadLeft(2, '0');
        else
            em = eInputField_M.text;

        if (ed.Length == 1)
            ed = ed.PadLeft(2, '0');
        else
            ed = eInputField_D.text;

        if (ehh.Length == 1)
            ehh = ehh.PadLeft(2, '0');
        else
            ehh = eInputField_Hh.text;

        if (emm.Length == 1)
            emm = emm.PadLeft(2, '0');
        else
            emm = eInputField_Mm.text;

        if (ess.Length == 1)
            ess = ess.PadLeft(2, '0');
        else
            ess = eInputField_Ss.text;
        #endregion

        url = $"http://io.energyiotlab.com:54242/q?" +
            $"start={sy}/{sm}/{sd}-{shh}:{smm}:{sss}" +
            $"&" +
            $"end={ey}/{em}/{ed}-{ehh}:{emm}:{ess}" +
            $"&m=sum:kw-iaq-sensor-kiot%7B" +
            $"sensor={sensor}," +
            $"serial={serial}" +
            $"%7D&o=&yrange=%5B0:%5D&key=out%20center%20top%20horiz&wxh=1024x768&style=linespoint&png";
                        
        StartCoroutine("GetImage");
    }

    string ChangeDropdownValue(string str)
    {
        string val = null;

        if (str == "All")
            val = "not_literal_or(tm)";
        else
            val = str;

        return val;
    }

    IEnumerator GetImage()
    {
        yield return new WaitForSeconds(0.5f);

        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url))
        {
            yield return uwr.SendWebRequest();

            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(uwr.error);
                errorMessage.SetActive(true);
            }
            else
            {
                // Get downloaded asset bundle
                errorMessage.SetActive(false);
                var texture = DownloadHandlerTexture.GetContent(uwr);
                Texture2D tex = DownloadHandlerTexture.GetContent(uwr);
                img.transform.GetComponent<Image>().sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            }
        }
    }

    public void ExitDataGui()
    {
        DataGui.SetActive(false);

        // 센서 정보를 null로 변경
        RaycastIndoorSensor.SensorSerial = null;
        img.transform.GetComponent<Image>().sprite = null;
        initializationFlag = false;
    }
}
