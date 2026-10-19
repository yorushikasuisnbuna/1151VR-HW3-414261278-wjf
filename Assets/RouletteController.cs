using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class RouletteController : MonoBehaviour
{
    float rotSpeed = 0f;          // 目前旋轉速度
    float maxSpeed = 20f;         // 加速上限
    float accel = 0.5f;           // 每個影格的加速量
    float decay = 0.98f;          // 衰減係數（越小停越快）
    float wheelStopSpeed = 0.1f;  // 低於此速度，輪盤完全停止
    float soundStopSpeed = 0.5f;  // 低於此速度，聲音先停止

    bool spinning = false;        // 輪盤是否在轉動中
    bool accelerating = false;    // 是否在加速階段
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.loop = false;          // 不循環，避免重播開頭
            audioSource.playOnAwake = false;
            audioSource.Stop();
        }
    }

    bool Clicked()
    {
#if ENABLE_INPUT_SYSTEM
        return (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            || (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame);
#else
        return Input.GetMouseButtonDown(0);
#endif
    }

    void Update()
    {
        // ===== 輪盤沒在轉：確保完全無聲，等待點擊 =====
        if (!spinning)
        {
            if (audioSource != null && audioSource.isPlaying) audioSource.Stop();

            if (Clicked())
            {
                spinning = true;
                accelerating = true;
                rotSpeed = 0f;
                if (audioSource != null)
                {
                    audioSource.volume = 0f;
                    audioSource.Play();        // 每次都從音效開頭播放
                }
            }
            return;
        }

        // ===== 轉動中 =====
        if (accelerating)
        {
            rotSpeed += accel;
            if (rotSpeed >= maxSpeed)
            {
                rotSpeed = maxSpeed;
                accelerating = false;          // 到最高速後開始減速
            }
        }
        else
        {
            rotSpeed *= decay;                 // 指數衰減
        }

        // 聲音：音量跟著速度走，速度降到 soundStopSpeed 時音量為 0 並停止
        if (audioSource != null)
        {
            if (!accelerating && rotSpeed < soundStopSpeed)
            {
                audioSource.volume = 0f;
                audioSource.Stop();
            }
            else if (audioSource.isPlaying)
            {
                audioSource.volume = Mathf.Clamp01((rotSpeed - soundStopSpeed) / 3f);
            }
        }

        // 輪盤：速度極小時完全停止
        if (!accelerating && rotSpeed < wheelStopSpeed)
        {
            rotSpeed = 0f;
            spinning = false;
            return;
        }

        transform.Rotate(0, 0, rotSpeed);
    }
}