HapticString
-------------
### Arduino Part
#### MagniDrive_Ver01.ino :
> - 1. (Unity to Arduino)控制電磁鐵磁力 .
> - 2. (From Arduino to Unity)接收RotaryEncoder的值 .
> - 3. (From Arduino to Unity)接收Pressure Sensor的值 .
> - 4. 透過OSC(與Unity)連線 .
> - 特別注意 : 第43行之IP Address , 須為PC local IP .

### Unity Part
#### RecvEncoderValue.cs :
> - 1. 負責接收Arduino端藉由OSC送來的Encoder Value , OSC address : /1/fader1 .

#### RecvPressureValue.cs :
> - 1. 負責接收Arduino端藉由OSC送來的Pressure Value , OSC address : /1/fader2 .

#### SendMagnetValue.cs :
> - 1. 取得手指位置(from rotary encoder & camera(OpenCV based))以及末端pressure值(手指往前或往後),
        進行判斷 , 決定磁鐵的PWM .
> - 2. 負責藉由OSC送磁鐵的PWM去Arduino端控制電磁鐵 .

#### TouchManager.cs :
> - 1. 監控收到的Encoder值 (in Update()) .
> - 2. 提供isTouch() , 以及getRelativePWM() 給SendMagnetValue.cs呼叫 .

#### FingerForward.cs :
> - 1. 藉由pressure sensor value , 判斷Finger是往前還是往後 .
> - 2. 提供fingerForward()給SendMagnetValue.cs呼叫 .
    

-------------
### VRSphereUI Version : /Unity/VRSphereUI
#### getLeapMotionComponent.cs :
> - 1. 使用Leap Motion API , 取得手的相關資訊 .
> - 2. 目前透過 public func getIndexTipPos() 來讓外界取得Index finger指尖的位置 .

#### sphereGenerator.cs :
> - 1. Real time 產生一個Sphere UI .
> - 2. User須先將手指拉至欲產生SphereUI位置, 按下'Enter'開始收集球面上的Point data .
> - 3. 使用收集到的Point data進行least square來fit出一個Sphere .

#### ButtonCurvedEffect.cs :
> - 1. 建構Curved button(由Plane產生), 並貼在SphereUI上 .
