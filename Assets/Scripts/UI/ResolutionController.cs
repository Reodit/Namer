using UnityEngine;

public class ResolutionController : MonoBehaviour
{
    Camera cameraView;

    private void Start()
    {
        cameraView = GetComponent<Camera>();
        SetResolution();
        CameraView();
    }

    /* 해상도 설정하는 함수 */
    public void SetResolution()
    {
        int setWidth = 2280; // 사용자 설정 너비
        int setHeight = 1080; // 사용자 설정 높이

        int deviceWidth = Screen.width; // 기기 너비 저장
        int deviceHeight = Screen.height; // 기기 높이 저장

        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true);

        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight)
        {
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight);
            cameraView.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f);
        }
        else
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight);
            cameraView.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight);
        }
    }

    public void CameraView()
    {
        Rect rt = cameraView.rect;

        float scale_height = ((float)Screen.width / Screen.height) / ((float)19 / 9);
        float scale_width = 1f / scale_height;

        if (scale_height < 1)
        {
            rt.height = scale_height;
            rt.y = (1f - scale_height) / 2f;
        }
        else
        {
            rt.width = scale_width;
            rt.x = (1f - scale_width) / 2f;
        }

        cameraView.rect = rt;
    }

    void OnPreCull()
    {
        if(cameraView.gameObject.name == "TitleCamera" || cameraView.gameObject.name == "Main Camera")
        {
            GL.Clear(true, true, Color.black);
        }
    }
}