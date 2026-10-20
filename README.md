1151VR 作業三 專案製作說明


一、製作步驟
1. 建立專案與匯入素材
 在 Unity Hub 建立 2D 專案，把輪盤與指針兩張透明背景的 PNG 拖進 Project 視窗。
2. 場景配置
把兩張圖拖進 Scene，輪盤放在 (0, 0, 0)，指針放在輪盤正上方並置中，並用 Inspector 的 Scale 調整大小。
指針的 Order in Layer 設為 1，確保畫在輪盤上面。
Game/Simulator 視窗選 Samsung 手機的橫式尺寸，預覽實際畫面。
3. 撰寫輪盤腳本 RouletteController.cs
在 Update 每個影格用 transform.Rotate(0, 0, rotSpeed) 讓輪盤繞 Z 軸旋轉。
狀態分三段：待機（等點擊）→ 加速（每影格 +0.5，上限 20）→ 減速（每影格乘以衰減係數 0.98，指數衰減）。
速度低於 0.1 時輪盤完全停止，才能再次點擊。
點擊判斷同時支援電腦滑鼠與手機觸控，用預處理指令 #if ENABLE_INPUT_SYSTEM 相容 Unity 6 新版 Input System。
腳本掛在輪盤物件上，指針不掛腳本。
4. 加入音效
音效檔拖進專案，輪盤加上 Audio Source 元件並放入 AudioClip，Play On Awake 與 Loop 都取消勾選。
點擊時Play()，音量隨輪盤速度變化，輪盤減速到接近停止時音量淡出並Stop()。
5. 電腦端測試
 按 Play 並點擊畫面，確認輪盤會加速、減速、停止，音效同步。

<img width="959" height="502" alt="螢幕擷取畫面 2026-10-19 222446" src="https://github.com/user-attachments/assets/489bb7cc-e74c-4d06-ae10-7758cc395294" />


6. 手機與 Build 設定
手機：開啟開發人員選項與 USB 偵錯，關閉 Auto Blocker，USB 模式選「傳輸檔案」並允許偵錯授權。
Player Settings：Default Orientation 設為 Landscape Left，Package Name 設為合法格式 com.hw3.roulette。
Build Profiles：平台選 Android，Scene List 只勾選實際完成的場景，Run Device 選手機，按 Build And Run。

影片連結:　https://youtu.be/8VG984vwVPk?si=nDl471VEmo8AOOE6
Github連結: https://github.com/yorushikasuisnbuna/1151VR-HW3-414261278-wjf
