using FrankenToilet.Core;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using UnityEngine.Video;
using static UnityEngine.GraphicsBuffer;

namespace FrankenToilet.prideunique;
public static class Popups
{
    private static GameObject MainPrefab;
    private static List<VideoClip> VideoClips = new List<VideoClip>();
    
    private static RenderTexture BaseRenderTexture;
    public static Dictionary<GameObject, RenderTexture> RenderTextures = new Dictionary<GameObject, RenderTexture>();

    public static AudioClip VideoCloseSound;


    public static void Init()
    {
        if (!AssetsController.AssetsLoaded)
            return;

        if (!CameraController.Instance)
            return;

        PopupCloser.Instance.Awake();

        MainPrefab = AssetsController.LoadAsset<GameObject>("assets/aizoaizo/popup.prefab");
        MainPrefab.SetActive(false);

        BaseRenderTexture = AssetsController.LoadAsset<RenderTexture>("assets/aizoaizo/videotexture.rendertexture");

        VideoCloseSound = AssetsController.LoadAsset<AudioClip>("assets/aizoaizo/pum.ogg");
        for (int i = 1; i <= 19; i++)
        {
            if (i == 14) // had problems with this one
                continue;

            VideoClips.Add(AssetsController.LoadAsset<VideoClip>("assets/aizoaizo/" + i.ToString() + ".mp4"));
        }

        CoroutineRunner.Run(PopupHandler());
    }

    private static IEnumerator PopupHandler()
    {
        while (true)
        {
            VideoClips.Shuffle();

            if (AssetsController.IsSlopSafe)
            {
                SpawnPopup(VideoClips[0]);
                SpawnPopup(VideoClips[1]);
                SpawnPopup(VideoClips[2]);
                
                yield return new WaitForSeconds(((float)VideoClips[0].length) * 3);
            }
            else
            {
                SpawnPopup(VideoClips[0]);
                SpawnPopup(VideoClips[1]);

                yield return new WaitForSeconds(((float)VideoClips[0].length) * 7);
            }
        }

        yield return null;
    }

    private static GameObject SpawnPopup(VideoClip videoClip)
    {
        GameObject go = UnityEngine.Object.Instantiate(MainPrefab);
        go.SetActive(true);

        RenderTexture renderTexture = new RenderTexture(BaseRenderTexture);
        renderTexture.Create();

        RenderTextures.Add(go, renderTexture);

        VideoPlayer videoPlayer = go.GetComponentInChildren<VideoPlayer>();
        videoPlayer.targetTexture = renderTexture;
        
        RawImage rawImage = go.GetComponentInChildren<RawImage>();
        rawImage.rectTransform.sizeDelta = new Vector2(videoClip.width, videoClip.height);
        rawImage.texture = renderTexture;

        Popup pu = rawImage.gameObject.AddComponent<Popup>();
        pu.Parent = go;
        pu.CloseSound = VideoCloseSound;

        videoPlayer.source = VideoSource.VideoClip;
        videoPlayer.clip = videoClip;
        videoPlayer.SetDirectAudioVolume(0, PrefsManager.Instance.GetFloat("allVolume", 0f));

        videoPlayer.Prepare();

        videoPlayer.prepareCompleted += (vp) => 
        {
            Texture tex = vp.texture;
            if (tex != null && tex.width > 0 && tex.height > 0)
            {
                // if UI is screen-space, use pixels
                rawImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, tex.width);
                rawImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, tex.height);
            }

            Vector3 dir = Random.onUnitSphere;
            Vector3 pos = dir.normalized * 512.0f;

            Follow f = go.gameObject.AddComponent<Follow>();
            f.target = CameraController.Instance.transform;
            f.mimicPosition = true;
             
            go.transform.GetChild(0).position = pos;
            go.transform.GetChild(0).LookAt(CameraController.Instance.transform.position);

            vp.Play();
        };

        return go;
    }
}
