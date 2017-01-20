HapticString
-------------
### Arduino Part
#### MagniDrive_Ver01.ino :
> - 1. 控制電磁鐵磁力 .
> - 2. 接收RotaryEncoder的值 .
> - 3. 透過OSC(與Unity)連線 .
> - 特別注意 : 第43行之IP Address , 須為PC local IP .

### Unity Part
#### RecvEncoderValue.cs	:
> - 1. 負責接收Arduino端藉由OSC送來的Encoder Value .

#### SendMagnetValue.cs :
> - 1. 負責藉由OSC送值去Arduino端控制電磁鐵 .

#### 待新增 :
##### TouchManager :
> - 1. 監控收到的Encoder值 (in Update()) .
> - 2. 判斷是否Touch到Object , call SendMagnetValue 去控制相對應磁力 .
    
    
