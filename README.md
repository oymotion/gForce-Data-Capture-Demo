# gForce-Data-Capture-Demo
## 1. 说明
该项目演示了 gforce系列设备 可在Unity使用的主要功能
该项目需要一定程度的Unity开发知识

## 2. 使用
点击右侧按钮列表的 Devices
打开设备扫描界面
点击要连接的设备名的 connect 按钮
点击exit关闭扫描连接界面
演示中最多支持两个设备同时连接和传输数据
如需要更多设备实时连接,可进项目中修改

### 3DView
该页面演示了陀螺仪和手势的显示
手势需要先预先训练好

### DataView
待设备电源指示灯快速闪烁后,数据开始正确显示
EMGData         展示了设备最多8个通道的波形实时显示
QuaternionData  展示了设备的实时陀螺仪数据
### DataCaptuer
点击Begin recording 开始记录数据
点击Stop and Save 可以暂停并保存数据
点击imu或者emg可以选择是记录陀螺仪还是肌电数据  
sample Rate     可以修改采样率
channel map     可以设置打开几点通道的位置 **需要是16进制转10进制**
resolution      ADC精度
packet          包长度
apply           应用以上设置的emg参数设置

### Gesture
该界面为手势演示专用画面,通过不同的手势可以要不同的区域亮红色
### neucirLite
此界面是正对neuCirLite做的演示界面
Max angle       设置最大角度
Min angle       设置最小角度
蓝色方块         可以移动弯曲的角度
粉色方块         当前完全角度

## 3.开发
脚本介绍
----
### dataShow
该类负责更新ui上的数据显示
* UpdataDeviceName          更新设备名称
* UpdateEMGData(byte,int)   更新肌电数据显示
   byte  具体的肌电值
   int   有效的肌电数量
* UpdateQuaternionDataText  更新陀螺仪四元数数据
* UpdataQuaternionDataObj   更新物体的四元数
* UpdataGestureDataText     更新设备当前手势显示
### Scattergram
该类负责绘画肌电数据的波形
### UIPanel
该类负责各个界面的开关
### IGameController
接口类
一些获取设备信息的接口类
### GameControllerManager
GameController管理类
负责管理IGameController实例出来的实例
* CreateGameController  实例一个 GameController 类
* SetGameController     设置 GameController 的编号
* GetGameController     获取指定编号的 GameController
### GameGforceListener
该类负责设备的回调到UI层的控制
#### UIListenerImpl
此为内部类,负责设备回调的分发
* onDeviceConnected     设备连接完成的回调
* onDeviceDiscard       设备丢失的意外回调
* onDeviceDisconnected  设备断开连接的回调
* onDeviceFound         找到设备的回调
* onScanFinished        扫描结束的回调
* onStateChanged        hub状态改变的回调
* onDeviceStatusChanged 设备状态改变的回调
    ReCenter            点按电源键
    UsbPlugged          充电口插上
    UsbPulled           充电口拔下
* UIListenerImpl        注册回调  


* SetGForceListener     设置当前回调类
* FoundDevice           扫到设备,发送给界面更新界面
* RestOrientationData   记录要重制四元数的设备
### GForceGameController
该类封装了一些设备数据的存贮和转化以及获取的功能
* GetQuaternionX        单独获取四元数的x
* GetQuaternionY        单独获取四元数的y
* GetQuaternionZ        单独获取四元数的z
* GetQuaternionW        单独获取四元数的w
* GetName               获取设备名称
* GetStatus             获取当前设备状态
* Tick                  更新设备
* UpdateEMGData         更新加点肌电数据
* GetEmgValue           获取肌电数据
* Map                   将数值转化为输入值的区间
* Clamp                 将值限定在氛围内
### QuatUtils
四元数和欧拉角的转换
* QuatToEuler           四元数转欧拉角
* EulerToSimpleQuat     欧拉角转四元数
### GForceDevice
负责对设备回调的包装处理
* GetDevice             获取具体的设备
* GetQuaternion         获取四元数
* RestOrientationData   重制四元数
* GetGesture            获取手势编号
* GetRawADC             获取 neucir 当前弯曲角度
* GetUpdata             获取累计的所有肌电数据并清空
* ClearUpdataData       只清空累计的所有肌电数据
* AddUpdataData         添加肌电数据到缓存
* GetDeviceState        获取设备当前状态
* onDeviceConnected     设备连接完成的回调
* onDeviceDiscard       设备意外丢失的回调
* onDeviceDisconnected  设备断开连接的回调
* onDeviceStatusChanged 设备状态改变的回调
* SetEMGConfig          设置肌电数据的采样率,16进制的通道数量,adc精度,包长度
* GetEMGConfigSampleRate获取设置的采样率
* GetEMGConfigChannelMap获取设置的16进制通道数量
* GetEMGConfigResolution获取设置的adc精度
* GetEMGConfigPacketLen 获取设置的包长度
* AddDataSwitch         添加设备功能通道
* RestDataSwitch        清楚设备所有功能通道
* CheckDataSwitchMap    检查设备是否具有此功能
* SetDataSwitch         打开设备功能通道
* TickGForce            更新gforce
* UpdateEMGData         更新肌电数据
* UpdateMagAngleData    更新 neucir 当前角度
* UpdateOrientationData 更新设备的陀螺仪数据
* GetAppControlMode     获取 neucir 当前的控制模式
* SetAppControlMode     设置 neucir 的控制模式
* TurnToAngle           使 neucir 旋转到指定角度
* GetNeuCirParams       获取 neucir 的控制属性
* SetNeuCirParams       设置 neucir 的控制属性
* UpdateGesture         更新设备当前手势
* GetConnectedDevices   获取连接的所有设备
* GetControllerForDevice获取指定设备对应的gforceDeivce
* EMGConfigCallback     emg设置完成回调
* DataSwitchConfigCallback 设备功能设置完成回调
#### GfroceData
gforceDevice的内部类,存贮回调一次的数据和数据类型,并提供包装后的设备调用和回调的实际接受
* GetQuaternion         获取四元数
* RestOrientationData   重制四元数
* GetGesture            获取手势
* GetRawADC             获取最近一次回调的额外数据
* GetUpdata             获取存储的额外数据并清空
* ClearUpdataData       清楚存储的额外数据
* AddUpdataData         添加数据到额外数据存储
* GetDeviceState        获取设备状态
  
##### 以下为gforceDevice对于回调中对于回调的实际接受
* onDeviceConnected
* onDeviceDiscard
* onDeviceDisconnected
* onDeviceStatusChanged
* SetEMGConfig
* GetEMGConfigSampleRate
* GetEMGConfigChannelMap
* GetEMGConfigResolution
* GetEMGConfigPacketLen
* AddDataSwitch
* RestDataSwitch
* CheckDataSwitchMap
* SetDataSwitch
* TickGForce
* UpdateEMGData
* UpdateMagAngleData
* UpdateOrientationData
* UpdateGesture
##### 以下为NeuCir支持的设置和回调
* GetAppControlMode
* SetAppControlMode
* TurnToAngle
* GetNeuCirParams
* SetNeuCirParams
##### 以下为获取设备的外部调用
* GetConnectedDevices
* GetControllerForDevice

##### 以下为设置的完成回调
* EMGConfigCallback
* DataSwitchConfigCallback


### GForceHub
对hub的使用的封装
* Prepare               初始化
* Terminate             销毁
* runThreadFn           心跳函数
### GForceListener
* onScanFinished        扫描结束回调
* onStateChanged        hub设备状态变化回调
* onDeviceConnected     设备连接完成回调
* onDeviceDiscard       设备意外丢失回调
* onDeviceDisconnected  设备意外丢失完成回调
* onDeviceFound         设备找到回调
* onDeviceStatusChanged 设备状态发送变化回调
* onExtendedDeviceData  设备其他数据回调
* onGestureData         设备手势回调
* onOrientationData     设备陀螺仪回调
* RegisterUIListener    注册回调
* UnregisterUIListener  取消回调
* RegisterGForceDevice  注册回调的gforceDevice
* UnregisterGForceDevice取消注册的gforceDevice回调
### UIListener
设备回调的抽象类
* onDeviceConnected     设备连接完成回调
* onDeviceDiscard       设备意外丢失回调
* onDeviceDisconnected  设备意外丢失完成的回调
* onDeviceFound         设备扫找的回到
* onScanFinished        扫描结束的回调
* onStateChanged        hub状态发送改变
* onDeviceStatusChanged 设备状态发送改变
### DataPanel
数据保存界面的控制类
* AddData               添加数据到缓存
* SaveData              保存数据并清楚缓存
* OnSetBC               设置数据成功显示回调
### UIDataBase
UI数据更新的基类
* init                  初始化
* TickData              更新数据
### UIDataEmg
继承UIDataBase的EMG数据更新类,和UIDataBase相同用法
### DeviceShow
显示扫描到的设备和连接
* Init                  初始化
* UpdateInfo            更新对应的设备
* Connect               连接对应的设备
### EMGandIMUPanel
EMG和IMU显示界面的控制和显示
* Tick                  更新指定设备的肌电和陀螺仪数据
* TickEmg               更新指定设备的肌电数据
* TickIMU               更新指定设备的陀螺仪数据
### GameMain
提供对设备的管理和对设备各种的取值
* Tick                  依次刷新已连接设备的每个tick,同时获取数据刷新到界面上
* RestOrientationData   设置要重制四元数的设备
* SetdataSwitchAll      设置所有连接设备的功能通道开关
* CheckDataSwichMap     检查指定设备是否支持此通道开关
* CheckDataSwichMapALL  检查连接的所有设备是否都支持此功能
* SetdataSwitch         设置指定设备的功能通道开关
* AddDataSwitch         给指定设备的通道开关添加此功能
* AddDataSwitchAll      给连接的所有设备功能通道开关添加此功能
* RestDataSwitch        重置指定设备的功能通道开关
* RestDataSwitchAll     重置连接的所有设备设备的功能通道开关
* GetGesture            获取指定设备的手势
* GetEMGData            获取指定设备的肌电数据
* GetDeviceType         获取指定设备的设备状态
* GetAppControlMode     获取指定 neucir 的控制模式
* SetAppControlMode     设置指定 neucir 的控制模式
* TurnToAngle           旋转指定 neucir 旋转到指定角度
* GetNeuCirParams       获取指定 neucir 的设置
* SetMaxNeuCirParams    设置指定 neucir 的最大值设置
* GetGForceGameController 获取指定的 GForceGameController
* CheckGfroceGameCtrlerList 检查是否存在指定的 GForceGameController
* GetUpdataData         获取指定的设备数据
* ClearUpdataData       清空指定设备的存储的数据缓存
* ClearUpdataDataAll    清空所有设备的存储的数据缓存
* SetDevicEMGConfige    设置指定设备的emg设置
* SetDevicEMGConfigeAll 设置连接所有设备的emg设置
* GetDevicEMGConfigeSampleRate  获取指定设备的emg采样率
* GetDevicEMGConfigeChannelMap  获取指定设备的emg通达打开情况
* GetDevicEMGConfigeResolution  获取指定射的adc精度
* GetDevicEMGConfigePacketLen   获取指定设备的跑长度设置
* AddCtrler             添加GForceGameController
* RemoveCtrler          删除GForceGameController
### GameMenu
扫描界面扫描设备添加设备的实际应用
* StartScan             开始扫描
* FoundDevice           找到设备
* CreateDevice          创建设备连接UI
* ClearDevices          清空创建的设备UI
* RestOrientationData   设置要重制四元数的设备
* ExitMenu              突出界面
### NeuCirLitePanel
neucir 控制界面脚本
### MyInputField
基于Unity InputField    自定义的移动拖拽组件
### MyScrollbar
基于Unity Scrollbar     自定义的移动拖拽组件
### RemoteCtl
手势显示界面控制脚本
* setCurColor           设置当前获取值后演示的颜色
* UpdataCcolor          UpdataCcolor更新默认选颜色
* SetBtnColor           初始化按钮

使用导言
----

除了我们提供的demo工程以外,你也可以基于本项目自己编写

    #  通过gf来引用
    using gf;  
    using GForce;  
    using GameCtrler

    # 初始化hub
    GForceHub.instance.Prepare();
    #扫描设备
    Hub.Instance.startScan();
    #停止扫描
    Hub.Instance.stopScan();


    #新建并注册回调
    gForceListener = new GForceListener();
    Hub.Instance.registerListener(gForceListener);
    
    # 新建并且注册收监控的gforceDevice和GForceGameController
        for (int i = 0; i < deciceCount; i++)
        {
            IGForceGameControllerGroup[i] = GameControllerManager.CreateGameController("GForceGameController");
            gForceListener.RegisterGForceDevice(IGForceGameControllerGroup[i] as GForceDevice);
            ctrlerGroup[i] = (GForceGameController)IGForceGameControllerGroup[i];
            AddCtrler(ctrlerGroup[i]);
        }

    #每帧调用
    ctrlerGroup[_index].Tick();

    #获取设备名字
    ctrlerGroup[_index].GetName()
    
    #获取陀螺仪数据
    ctrlerGroup[_index].GetQuaternionX();
    ctrlerGroup[_index].GetQuaternionY();
    ctrlerGroup[_index].GetQuaternionZ();
    ctrlerGroup[_index].GetQuaternionW();

    #获取肌电数据
    ctrlerGroup[_index].UpdateEMGData();

    #获取手势
    ctrlerGroup[_index].GetGesture();

##  4.常见问题
###使用问题
1.  Q: windows 端扫描不到设备
    A: windows 端扫描设备需要 duangou,目前windows端扫描设备需要更多少时间,部分时候需要重启软件
2.  Q: 多设备连接去情况下,会出现只有第一个设备在正常工作
    A: 多设备的连接和数据传输需要更好的蓝牙设备
3.  Q: 手势识别没有反应
    A: 此项目仅提供显示,手势需要预先训练,点击[GforceAPP](https://gforce-portal.oymotion.com/) 下载并训练手势,注意,此软件目前不支持windows端需要用Android设备安装,ios请前往appStore 搜索OYmotion gForce User App 下载
4.  Q:如果我需要多个设备连接需要修改哪里?
    A: 在 GameMain 中修改 deciceCount 变量即可
5.  Q: 为什么我设置的采样率和实际的数据量不对
    A: 收蓝牙传输速率影响,实际量和设置的计算量会有出入,如果特别大,可能是采样率过高或者多设备对使用设备的蓝牙压力过大,需要减少采样率或者更换使用设备
6.  Q:打开的界面不对
    A:此项目主要是演示功能,部分UI在一些分辨率和长宽比例会出现问题,推荐使用1980*1080 的分辨率
### 开发问题
1.  Q:为什么项目中的陀螺仪的XYZW要分开获取并且有的还会取反?
    A:设备中的陀螺仪的摆放位置有关,在实际应用中需要进行处理
2.  Q:为什么项目中的陀螺仪的XYZW中z和Y要相反?
    A:Unity中对于四元数的封装和Gforce内部的四元数封装不同,需要调换位置
3.  Q:为什么肌电数据要先存起来?
    A:Gforce设备的数据都是在子线程中回调过来的,在Unity中,子线程的变量不能直接更新UI需要转存,并且gforce的数据会根据设置的采样率等数据回调速度会不同,为了不丢失数据,会先保存在GForceDevice中,在Unity中的需要取用的时候再去取用
    