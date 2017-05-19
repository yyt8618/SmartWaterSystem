﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Common
{
    /// <summary>
    /// 记录仪命令参数
    /// </summary>
    public enum NOISE_LOG_COMMAND
    {
        #region 更新配置
        /// <summary>
        /// 记录仪时间
        /// </summary>
        WRITE_TIME = 0x11,
        /// <summary>
        /// 记录仪无线通讯
        /// </summary>
        WRITE_WIRELESS = 0x12,
        /// <summary>
        /// 采集数据时间段
        /// </summary>
        WRITE_START_END_TIME = 0x13,
        /// <summary>
        /// 采集时间间隔
        /// </summary>
        WRITE_INTERVAL = 0x14,
        /// <summary>
        /// 远传功能开关
        /// </summary>
        WRITE_REMOTE_SWITCH = 0x16,
        /// <summary>
        /// 远传发送时间
        /// </summary>
        WRITE_REMOTE_SEND_TIME = 0x17,
        /// <summary>
        /// 串口设置记录仪启动值(标准值)
        /// </summary>
        WRITE_NOISE_STANDVALUE = 0x15,
        #endregion

        #region 读取配置
        /// <summary>
        /// 串口读取记录仪时间
        /// </summary>
        READ_TIME = 0x41,
        /// <summary>
        /// 串口读取记录仪无线通讯的【收发频率＋无线速率＋发射功率＋串口速率＋唤醒时间】
        /// </summary>
        READ_WIRELESS = 0x42,
        /// <summary>
        /// 串口读取记录仪采集数据的时间
        /// </summary>
        READ_START_END_TIME = 0x43,
        /// <summary>
        /// 串口读取记录仪采集时间间隔
        /// </summary>
        READ_INTERVAL = 0x44,
        /// <summary>
        /// 串口读取记录仪远传功能
        /// </summary>
        READ_REMOTE = 0x46,
        /// <summary>
        /// 串口读取记录仪远传发送时间
        /// </summary>
        READ_REMOTE_SEND_TIME = 0x47,
        /// <summary>
        /// 串口读取记录仪的ID
        /// </summary>
        READ_NOISE_LOG_ID = 0x4e,
        /// <summary>
        /// 串口读取记录仪启动值(标准值)
        /// </summary>
        READ_NOISE_STANDVALUE=0x45,
        #endregion

        #region 发送
        /// <summary>
        /// 响应命令发送
        /// </summary>
        SEND_RESPONSE_DATA = 0xa0,
        /// <summary>
        /// 相应启动命令(静态标准值)
        /// </summary>
        SEND_RESPONSE_DATA_ORIGITY = 0xa1,
        #endregion

        #region 控制
        /// <summary>
        /// 串口发送控制记录仪【启动/停止】命令
        /// </summary>
        CTRL_START_OR_STOP = 0x70,
        /// <summary>
        /// 串口读取记录仪数据总数和启动值
        /// </summary>
        CTRL_START_READ = 0x71,
        /// <summary>
        /// 清除FLASH
        /// </summary>
        CTRL_CLEAR_FLASH = 0x72,
        /// <summary>
        /// 读取记录仪数据
        /// </summary>
        CTRL_READDATA = 0xa0,
        /// <summary>
        /// 读取静态基准值
        /// </summary>
        CTRL_READORIGITY = 0xa1
        #endregion
    }
    /// <summary>
    /// 远传控制器命令参数
    /// </summary>
    public enum NOISE_CTRL_COMMAND
    {
        #region 设置
        /// <summary>
        /// 设置时间(远传校时)
        /// </summary>
        SET_NOISE_GPRSTIME=0x11,
        /// <summary>
        /// 远传控制器ID
        /// </summary>
        WRITE_NOISE_CTRL_ID=0x1d,
        /// <summary>
        /// 远传功能开关
        /// </summary>
        WRITE_REMOTE_SWITCH = 0x1f,
        /// <summary>
        /// 设置时间(串口)
        /// </summary>
        WRITE_TIME =0x17,
        /// <summary>
        /// 远传发送时间
        /// </summary>
        WRITE_REMOTE_SEND_TIME = 0x1e,
        /// <summary>
        /// 远传发送时间(远传)
        /// </summary>
        WRITE_REMOTE_GPRSSEND_TIME = 0x17,
        /// <summary>
        /// 无线通讯
        /// </summary>
        WRITE_WIRELESS = 0x18,
        /// <summary>
        /// 串口与GPRS模块的通讯波特率
        /// </summary>
        WRITE_GPRS_BAUDRATE = 0x19,
        /// <summary>
        /// 远传控制器IP
        /// </summary>
        WRITE_IP = 0x1a,
        /// <summary>
        /// 远传控制器端口号
        /// </summary>
        WRITE_PORT = 0x1b,
        /// <summary>
        /// 远传控制器设备与记录仪设备对应的ID
        /// </summary>
        WRITE_CTRL_NOISE_LOG_ID = 0x1c,
        /// <summary>
        /// 复位
        /// </summary>
        RESET=0x20,
        /// <summary>
        /// 远传通讯方式
        /// </summary>
        WRITE_COMTYPE=0x22,
        #endregion

        #region 读取
        /// <summary>
        /// 串口读取远传控制器时间
        /// </summary>
        READ_TIME=0x47,
        /// <summary>
        /// 串口读取远传控制器无线通讯
        /// </summary>
        READ_WIRELESS = 0x48,
        /// <summary>
        /// 串口与GPRS模块的通讯波特率
        /// </summary>
        READ_GPRS_BAUDRATE = 0x49,
        /// <summary>
        /// 串口读取远传控制器IP
        /// </summary>
        READ_IP = 0x4a,
        /// <summary>
        /// 串口读取远传控制器端口号
        /// </summary>
        READ_PORT = 0x4b,
        /// <summary>
        /// 串口读取远传控制器设备与记录仪设备对应的ID号
        /// </summary>
        READ_CTRL_NOISE_LOG_ID = 0x4c,
        /// <summary>
        /// 串口读取远传控制器远传发送时间
        /// </summary>
        READ_REMOTE_SEND_TIME = 0x4d,
        /// <summary>
        /// 串口读取远传控制器远传开关
        /// </summary>
        READ_REMOTE =0x4e,
        /// <summary>
        /// 串口读取远程控制器的ID
        /// </summary>
        READ_NOISE_CTRL_ID = 0x4f,
        /// <summary>
        /// 串口读取远传控制器场强、电压
        /// </summary>
        READ_FIELDSTRENGTH= 0x50,
        /// <summary>
        /// 串口读取远传控制器通讯方式
        /// </summary>
        READ_COMTYPE = 0x52,
        #endregion
    }

    /// <summary>
    /// 巡视仪命令参数(Test->Form1)
    /// </summary>
    public enum NOISE_TOUR_COMMAND
    {
        /// <summary>
        /// 串口设置巡视仪【收发频率＋无线速率＋串口速率】
        /// </summary>
        SETTING = 0x10,

        /// <summary>
        /// 串口读取巡视仪 收发频率＋无线速率+串口速率
        /// </summary>
        READ_WIRELESS = 0x40,
        /// <summary>
        /// 串口读取巡视仪的ID
        /// </summary>
        READ_NOISE_TOUR_ID = 0x4d
    }

    public enum UNIVERSAL_COMMAND
    {
        #region 设置
        /// <summary>
        /// 设置时间
        /// </summary>
        [Description("设置时间")]
        SET_TIME = 0x10,
        /// <summary>
        /// 设置从站脉冲时间间隔
        /// </summary>
        [Description("设置从站脉冲时间间隔")]
        SET_PLUSEINTERVAL = 0x11,
        /// <summary>
        /// 设置从站模拟量时间间隔
        /// </summary>
        [Description("设置从站模拟量时间间隔")]
        SET_SIMINTERVAL = 0x2C,
        /// <summary>
        /// 设置从站485采集时间间隔
        /// </summary>
        [Description("设置从站485采集时间间隔")]
        SET_485INTERVAL = 0x14,
        /// <summary>
        /// 设置从站采集配置功能
        /// </summary>
        [Description("设置从站采集配置功能")]
        SET_COLLECTCONFIG = 0x1B,
        /// <summary>
        /// 设置从站485采集MODBUS执行标识
        /// </summary>
        [Description("设置从站485采集MODBUS执行标识")]
        SET_MODBUSEXEFLAG = 0x2D,
        /// <summary>
        /// 设置485采集modbus协议
        /// </summary>
        [Description("设置485采集modbus协议")]
        SET_MODBUSPROTOCOL = 0x30,
        /// <summary>
        /// 设置从站ID
        /// </summary>
        [Description("设置从站ID")]
        SET_ID = 0x1E,
        /// <summary>
        /// 设置手机号
        /// </summary>
        [Description("设置手机号")]
        SET_CELLPHONE = 0x21,
        /// <summary>
        /// 设置从站IP
        /// </summary>
        [Description("设置从站IP")]
        SET_IP = 0x1F,
        /// <summary>
        /// 设置从站端口号
        /// </summary>
        [Description("设置从站端口号")]
        SET_PORT = 0x20,
        /// <summary>
        /// 设置从站心跳间隔
        /// </summary>
        [Description("设置从站心跳间隔")]
        SET_HEART = 0x1A,
        /// <summary>
        /// 设置从站通信方式
        /// </summary>
        [Description("设置从站通信方式")]
        SET_COMTYPE = 0x1C,
        /// <summary>
        /// 设置从站联网模式
        /// </summary>
        [Description("设置从站联网模式")]
        SET_NETWORKTYPE = 0x29,
        /// <summary>
        /// 设置第一路脉冲基准数
        /// </summary>
        [Description("设置第一路脉冲基准数")]
        SET_PLUSEBASIC1 = 0x31,
        /// <summary>
        /// 设置第二路脉冲基准数
        /// </summary>
        [Description("设置第二路脉冲基准数")]
        SET_PLUSEBASIC2 = 0x32,
        /// <summary>
        /// 设置第三路脉冲基准数
        /// </summary>
        [Description("设置第三路脉冲基准数")]
        SET_PLUSEBASIC3 = 0x33,
        /// <summary>
        /// 设置第四路脉冲基准数
        /// </summary>
        [Description("设置第四路脉冲基准数")]
        SET_PLUSEBASIC4 = 0x34,
        /// <summary>
        /// 设置485波特率
        /// </summary>
        [Description("设置485波特率")]
        SET_485BAUD = 0x1D,
        /// <summary>
        /// 设置电压采集时间间隔
        /// </summary>
        [Description("设置电压采集时间间隔")]
        SET_VOLINTERVAL = 0x26,
        /// <summary>
        /// 设置电压报警下限
        /// </summary>
        [Description("设置电压报警下限")]
        SET_VOLLOWER = 0x17,
        /// <summary>
        /// 设置报警上限值
        /// </summary>
        [Description("设置报警上限值")]
        SET_PREUPLIMIT = 0x15,
        /// <summary>
        /// 设置模拟量报警上限值
        /// </summary>
        [Description("设置模拟量报警上限值")]
        SET_SIMUPLIMIT = 0x36,
        /// <summary>
        /// 设置流量报警上限值
        /// </summary>
        [Description("设置流量报警上限值")]
        SET_FLOWUPLIMIT = 0x3B,
        /// <summary>
        /// 设置压力报警下限值
        /// </summary>
        [Description("设置压力报警下限值")]
        SET_PRELOWLIMIT = 0x16,
        /// <summary>
        /// 设置模拟量报警下限值
        /// </summary>
        [Description("设置模拟量报警下限值")]
        SET_SIMLOWLIMIT = 0x37,
        /// <summary>
        /// 设置流量报警下限值
        /// </summary>
        [Description("设置流量报警下限值")]
        SET_FLOWLOWLIMIT = 0x3C,
        /// <summary>
        /// 设置压力斜率报警上限值
        /// </summary>
        [Description("设置压力斜率报警上限值")]
        SET_PRESLOPUPLIMIT = 0x18,
        /// <summary>
        /// 设置模拟量斜率报警上限值
        /// </summary>
        [Description("设置模拟量斜率报警上限值")]
        SET_SIMSLOPUPLIMIT = 0x38,
        /// <summary>
        /// 设置流量斜率报警上限值
        /// </summary>
        [Description("设置流量斜率报警上限值")]
        SET_FLOWSLOPUPLIMIT = 0x3D,
        /// <summary>
        /// 设置压力斜率报警下限值
        /// </summary>
        [Description("设置压力斜率报警下限值")]
        SET_PRESLOPLOWLIMIT = 0x19,
        /// <summary>
        /// 设置模拟量斜率报警下限值
        /// </summary>
        [Description("设置模拟量斜率报警下限值")]
        SET_SIMSLOPLOWLIMIT = 0x39,
        /// <summary>
        /// 设置流量斜率报警下限值
        /// </summary>
        [Description("设置流量斜率报警下限值")]
        SET_FLOWSLOPLOWLIMIT = 0x3E,

        /// <summary>
        /// 设置上限投退
        /// </summary>
        [Description("设置上限投退")]
        SET_UPENABLE = 0x22,
        /// <summary>
        /// 设置下限报警投退
        /// </summary>
        [Description("设置下限报警投退")]
        SET_LOWENABLE = 0x23,
        /// <summary>
        /// 设置斜率上限报警投退
        /// </summary>
        [Description("设置斜率上限报警投退")]
        SET_SLOPUPENABLE = 0x24,
        /// <summary>
        /// 设置斜率下限报警投退
        /// </summary>
        [Description("设置斜率下限报警投退")]
        SET_SLOPLOWENABLE = 0x25,
        /// <summary>
        /// 设置压力量程
        /// </summary>
        [Description("设置压力量程")]
        SET_PRERANGE = 0x28,
        /// <summary>
        /// 设置模拟量量程
        /// </summary>
        [Description("设置模拟量量程")]
        SET_SIMRANGE = 0x35,
        /// <summary>
        /// 设置偏移值
        /// </summary>
        [Description("设置偏移值")]
        SET_PREOFFSET = 0x12,
        /// <summary>
        /// 设置短信发送时间间隔
        /// </summary>
        [Description("设置短信发送时间间隔")]
        SET_SMSINTERVAL = 0x3A,
        /// <summary>
        /// 设置脉冲计数单位
        /// </summary>
        [Description("设置脉冲计数单位")]
        SET_PLUSEUNIT = 0x2A,
        /// <summary>
        /// 设置报警次数
        /// </summary>
        [Description("设置报警次数")]
        SET_ALARMLEN = 0x27,
        /// <summary>
        /// 设置招测使能
        /// </summary>
        [Description("设置招测使能")]
        SET_CALLENABLE = 0x70,
        /// <summary>
        /// 设置分体式液位量程
        /// </summary>
        [Description("设置分体式液位量程")]
        SET_LEVELRANGE = 0xC0,
        /// <summary>
        /// 分体式液位基值
        /// </summary>
        [Description("分体式液位基值")]
        SET_LEVELBASE = 0xC1,
        /// <summary>
        /// 分体式液位上限值
        /// </summary>
        [Description("分体式液位上限值")]
        SET_LEVELUPLIMIT = 0xC2,
        /// <summary>
        /// 分体式液位下限值
        /// </summary>
        [Description("分体式液位下限值")]
        SET_LEVELLOWLIMIT = 0xC3,
        #endregion

        #region 读取
        /// <summary>
        /// 读取从站时间
        /// </summary>
        [Description("读取从站时间")]
        READ_TIME = 0x40,
        /// <summary>
        /// 读取从站波特率
        /// </summary>
        [Description("读取从站波特率")]
        READ_485BAUD = 0x4D,
        /// <summary>
        /// 读取从站模拟量时间间隔
        /// </summary>
        [Description("读取从站模拟量时间间隔")]
        READ_SIMINTERVAL = 0x5C,
        /// <summary>
        /// 读取从站RS485时间间隔
        /// </summary>
        [Description("读取从站RS485时间间隔")]
        READ_485INTERVAL = 0x44,
        /// <summary>
        /// 读取从站脉冲时间间隔
        /// </summary>
        [Description("读取从站脉冲时间间隔")]
        READ_PLUSEINTERVAL = 0x41,
        /// <summary>
        /// 读取从站心跳间隔
        /// </summary>
        [Description("读取从站心跳间隔")]
        READ_HEARTINTERVAL = 0x4A,
        /// <summary>
        /// 读取从站采集功能配置
        /// </summary>
        [Description("读取从站采集功能配置")]
        READ_COLLECTCONFIG = 0x4B,
        /// <summary>
        /// 读取从站MODBUS协议执行标识
        /// </summary>
        [Description("读取从站MODBUS协议执行标识")]
        READ_MODBUSEXEFLAG = 0x5D,
        /// <summary>
        /// 读取从站485采集modbus协议
        /// </summary>
        [Description("读取从站485采集modbus协议")]
        READ_MODBUSPROTOCOL = 0x60,
        /// <summary>
        /// 读取从站
        /// </summary>
        [Description("读取从站")]
        READ_ID = 0x4E,
        [Description("读取从站IP")]
        READ_IP = 0x4F,
        [Description("读取从站端口号")]
        READ_PORT = 0x50,
        [Description("读取从站手机号")]
        READ_CELLPHONE = 0x51,
        [Description("读取从站通信方式")]
        READ_COMTYPE = 0x4C,
        /// <summary>
        /// 读取从站电池电压时间间隔
        /// </summary>
        [Description("读取从站电池电压时间间隔")]
        READ_VOLINTERVAL = 0x56,
        /// <summary>
        /// 读取电压报警下限值(%)
        /// </summary>
        [Description("读取电压报警下限值")]
        READ_VOLLOWER = 0x47,
        /// <summary>
        /// 读取联网模式
        /// </summary>
        [Description("读取联网模式")]
        READ_NETWORKTYPE = 0x59,
        /// <summary>
        /// 读取压力报警上限值
        /// </summary>
        [Description("读取压力报警上限值")]
        READ_PREUPLIMIT = 0x45,
        /// <summary>
        /// 读取模拟量1、2报警上限值
        /// </summary>
        [Description("读取模拟量1、2报警上限值")]
        READ_SIMUPLIMIT = 0x68,
        /// <summary>
        /// 读取流量报警上限值
        /// </summary>
        [Description("读取流量报警上限值")]
        READ_FLOWUPLIMIT = 0x61,
        /// <summary>
        /// 读取压力报警下限值
        /// </summary>
        [Description("读取压力报警下限值")]
        READ_PRELOWLIMIT = 0x46,
        /// <summary>
        /// 读取模拟量1、2报警下限值
        /// </summary>
        [Description("读取模拟量1、2报警下限值")]
        READ_SIMLOWLIMIT = 0x69,
        /// <summary>
        /// 读取流量报警下限值
        /// </summary>
        [Description("读取流量报警下限值")]
        READ_FLOWLOWLIMIT = 0x62,
        /// <summary>
        /// 读取压力斜率报警上限值
        /// </summary>
        [Description("读取压力斜率报警上限值")]
        READ_PRESLOPUPLIMIT = 0x48,
        /// <summary>
        /// 读取模拟量1、2报警上限值
        /// </summary>
        [Description("读取模拟量1、2报警上限值")]
        READ_SIMSLOPUPLIMIT = 0x6A,
        /// <summary>
        /// 读取流量斜率报警上限值
        /// </summary>
        [Description("读取流量斜率报警上限值")]
        READ_FLOWSLOPUPLIMIT = 0x63,
        /// <summary>
        /// 读取压力斜率报警下限值
        /// </summary>
        [Description("读取压力斜率报警下限值")]
        READ_PRESLOPLOWLIMIT = 0x49,
        /// <summary>
        /// 读取模拟量1、2斜率报警下限值
        /// </summary>
        [Description("读取模拟量1、2斜率报警下限值")]
        READ_SIMSLOPLOWLIMIT = 0x6B,
        /// <summary>
        /// 读取流量斜率报警下限值
        /// </summary>
        [Description("读取流量斜率报警下限值")]
        READ_FLOWSLOPLOWLIMIT = 0x64,
        /// <summary>
        /// 读取压力、模拟量、流量、分体式液位报警上限投退
        /// </summary>
        [Description("读取压力、模拟量、流量报警上限投退")]
        READ_UPENABLE = 0x52,
        /// <summary>
        /// 读取压力、模拟量、流量、分体式液位报警下限投退
        /// </summary>
        [Description("读取压力、模拟量、流量报警下限投退")]
        READ_LOWENABLE = 0x53,
        /// <summary>
        /// 读取压力、模拟量、流量斜率报警上限投退
        /// </summary>
        [Description("读取压力、模拟量、流量斜率报警上限投退")]
        READ_SLOPUPENABLE = 0x54,
        /// <summary>
        /// 读取压力、模拟量、流量斜率报警下限投退
        /// </summary>
        [Description("读取压力、模拟量、流量斜率报警下限投退")]
        READ_SLOPLOWENABLE = 0x55,
        /// <summary>
        /// 读取压力量程
        /// </summary>
        [Description("读取压力量程")]
        READ_PRERANGE = 0x58,
        /// <summary>
        /// 读取模拟量量程
        /// </summary>
        [Description("读取模拟量量程")]
        READ_SIMRANGE = 0x67,
        /// <summary>
        /// 读取压力偏移量
        /// </summary>
        [Description("读取压力偏移量")]
        READ_PREOFFSET = 0x42,
        /// <summary>
        /// 读取版本号
        /// </summary>
        [Description("读取版本号")]
        READ_VER = 0x5B,
        /// <summary>
        /// 读取短信发送间隔
        /// </summary>
        [Description("读取短信发送间隔")]
        READ_SMSINTERVAL = 0x6C,
        /// <summary>
        /// 读取脉冲计数单位
        /// </summary>
        [Description("读取脉冲计数单位")]
        READ_PLUSEUNIT = 0x5A,
        /// <summary>
        /// 读取场强\电压
        /// </summary>
        [Description("读取场强")]
        READ_FIELDSTRENGTH = 0x5E,
        /// <summary>
        /// 读取取消报警时间长度
        /// </summary>
        [Description("读取报警次数")]
        READ_ALARMLEN = 0x57,
        /// <summary>
        /// 读取分体式液位量程
        /// </summary>
        [Description("读取分体式液位量程")]
        READ_LEVELRANGE = 0xE0,
        /// <summary>
        /// 读取分体式液位基值
        /// </summary>
        [Description("读取分体式液位基值")]
        READ_LEVELBASE = 0xE1,
        /// <summary>
        /// 读取分体式液位上限值
        /// </summary>
        [Description("读取分体式液位上限值")]
        READ_LEVELUPLIMIT = 0xE2,
        /// <summary>
        /// 读取分体式液位下限值
        /// </summary>
        [Description("读取分体式液位下限值")]
        READ_LEVELLOWLIMIT = 0xE3,

        #endregion

        #region 控制命令
        /// <summary>
        /// 复位命令
        /// </summary>
        [Description("复位命令")]
        RESET = 0x72,
        /// <summary>
        /// 启动终端采集功能命令
        /// </summary>
        [Description("启动终端采集功能命令")]
        EnableCollect = 0x73,
        /// <summary>
        /// 从站第1路模拟量零点值校准
        /// </summary>
        [Description("从站第1路模拟量零点值校准")]
        CalibartionSimualte1 = 0x65,
        /// <summary>
        /// 从站第2路模拟量零点值校准
        /// </summary>
        [Description("从站第2路模拟量零点值校准")]
        CalibartionSimualte2 = 0x66,
        /// <summary>
        /// 开启永久在线 0x01：表示开启 0x00表示关闭永久在线
        /// </summary>
        [Description("开启永久在线")]
        OnLine = 0x70,
        /// <summary>
        /// 招测压力1路
        /// </summary>
        [Description("招测压力1路")]
        CallData_Pre1 = 0x71,
        /// <summary>
        /// 招测模拟一路
        /// </summary>
        [Description("招测模拟一路")]
        CallData_Sim1 = 0x78,
        /// <summary>
        /// 招测招测模拟二路
        /// </summary>
        [Description("招测招测模拟二路")]
        CallData_Sim2 = 0x79,
        /// <summary>
        /// 招测脉冲量
        /// </summary>
        [Description("招测脉冲量")]
        CallData_Pluse = 0x82,
        /// <summary>
        /// 招测RS485 1路
        /// </summary>
        [Description("招测RS485 1路")]
        CallData_RS4851 = 0x7a,
        /// <summary>
        /// 招测RS485 2路
        /// </summary>
        [Description("招测RS485 2路")]
        CallData_RS4852 = 0x7b,
        /// <summary>
        /// 招测RS485 3路
        /// </summary>
        [Description("招测RS485 3路")]
        CallData_RS4853 = 0x7c,
        /// <summary>
        /// 招测RS485 4路
        /// </summary>
        [Description("招测RS485 4路")]
        CallData_RS4854 = 0x7d,

        [Description("设置实时报警")]
        EnableAlarm=0x2B,
        #endregion
    }

    public enum OLWQ_COMMAND
    {
        #region 设置
        /// <summary>
        /// 设置时间
        /// </summary>
        SET_TIME = 0x10,
        /// <summary>
        /// 设置从站浊度时间间隔
        /// </summary>
        SET_TURBIDITYINTERVAL = 0x11,

        /// <summary>
        /// 设置从站浊度上限值
        /// </summary>
        SET_TURBIDITYUPLIMIT = 0x29,

        /// <summary>
        /// 设置从站余氯下限值
        /// </summary>
        SET_RESIDUALCLLOWLIMIT = 0x13,

        /// <summary>
        /// 设置从站余氯采集时间间隔
        /// </summary>
        SET_RESIDUALCLINTERVAL = 0x14,

        /// <summary>
        /// 设置从站余氯校准值
        /// </summary>
        SET_RESIDUALCLSTANDVALUE = 0x16,

        /// <summary>
        /// 终端采集功能配置:、0X01浊度、0x02――余氯  0x04：ph    0x80：电导率
        /// </summary>
        SET_COLLECTCONFIG = 0x17,
        /// <summary>
        /// 设置从站ID
        /// </summary>
        SET_ID = 0x18,
        /// <summary>
        /// 设置从站IP
        /// </summary>
        SET_IP = 0x19,
        /// <summary>
        /// 设置从站端口号
        /// </summary>
        SET_PORT = 0x1a,

        /// <summary>
        /// 设置从站PH间隔
        /// </summary>
        SET_PH_INTERVAL = 0x1b,

        /// <summary>
        /// 设置从站清洗时间间隔 1c
        /// </summary>
        SET_CLEARINTERVAL = 0x1c,

        /// <summary>
        /// 设置从站加报时间间隔
        /// </summary>
        SET_DATAINTERVAL = 0x13,

        /// <summary>
        /// 设置从站余氯零点值
        /// </summary>
        SET_RESIDUALCLZERO = 0x1f,

        /// <summary>
        /// 设置从站余氯灵敏度
        /// </summary>
        SET_RESIDUALCLSENSITIVITY = 0x20,

        /// <summary>
        /// 设置从站供电方式 01表示电池02表示市电
        /// </summary>
        SET_POWERSUPPLYTYPE = 0x21,

        /// <summary>
        /// 设置从站电导率采集发送时间间隔
        /// </summary>
        SET_CONDUCTIVITYINTERVAL = 0x1d,

        /// <summary>
        /// 设置从站浊度上限值
        /// </summary>
        SET_TURBIDITYLOWLIMIT = 0x30,
        /// <summary>
        /// 设置温度上限值
        /// </summary>
        SET_TEMPUPLIMIT = 0x23,
        /// <summary>
        /// 设置温度下限值
        /// </summary>
        SET_TEMPLOWLIMIT = 0x24,
        /// <summary>
        /// 设置温度加报阀值
        /// </summary>
        SET_TEMPADDTION = 0x36,
        /// <summary>
        /// 设置PH上限值
        /// </summary>
        SET_PHUPLIMIT = 0x25,
        /// <summary>
        /// 设置PH下限值
        /// </summary>
        SET_PHLOWLIMIT = 0x26,
        /// <summary>
        /// 设置从站电导率上限值
        /// </summary>
        SET_CONDUCTIVITYUPLIMIT = 0x27,
        /// <summary>
        /// 设置从站电导率下限值
        /// </summary>
        SET_CONDUCTIVITYLOWLIMIT = 0x28,
        /// <summary>
        /// 设置从站地址
        /// </summary>
        SET_TERADDR = 0x15,
        /// <summary>
        /// 设置中心站地址
        /// </summary>
        SET_CENTERADDR = 0x14,
        /// <summary>
        /// 设置密码
        /// </summary>
        SET_PWD = 0x16,
        /// <summary>
        /// 设置工作方式
        /// </summary>
        SET_WORKTYPE = 0x31,
        /// <summary>
        /// 设置GPRS开关
        /// </summary>
        SET_GPRSSWITCH= 0x22,
        #endregion

        #region 读取
        /// <summary>
        /// 读取从站时间
        /// </summary>
        READ_TIME = 0x40,
        /// <summary>
        /// 读取从站浊度时间间隔
        /// </summary>
        READ_TURBIDITYINTERVAL = 0x41,

        /// <summary>
        /// 读取从站余氯时间间隔
        /// </summary>
        READ_RESIDUALCLINTERVAL = 0x44,

        /// <summary>
        /// 读取从站ph采集时间间隔
        /// </summary>
        READ_PHINTERVAL = 0x4b,

        /// <summary>
        /// 读取从站电导率采集时间间隔
        /// </summary>
        READ_CONDUCTIVITY = 0x50,

        /// <summary>
        /// 读取从站采集功能配置
        /// </summary>
        READ_COLLECTCONFIG = 0x47,

        /// <summary>
        /// 读取从站余氯下限值
        /// </summary>
        READ_RESIDUALCLLOWLIMIT = 0x43,

        /// <summary>
        /// 读取从站温度上限值
        /// </summary>
        READ_TEMPUPLIMIT = 0x51,

        /// <summary>
        /// 读取从站温度下限值
        /// </summary>
        READ_TEMPLOWLIMIT = 0x52,

        /// <summary>
        /// 读取从站温度加报阀值
        /// </summary>
        READ_TEMPADDTION = 0x5F,

        /// <summary>
        /// 读取从站PH上限值
        /// </summary>
        READ_PHUPLIMIT = 0x53,

        /// <summary>
        /// 读取从站PH上限值
        /// </summary>
        READ_PHLOWLIMIT = 0x54,

        /// <summary>
        /// 读取从站电导率上限值
        /// </summary>
        READ_CONDUCTIVITYUPLIMIT = 0x55,

        /// <summary>
        /// 读取从站电导率上限值
        /// </summary>
        READ_CONDUCTIVITYLOWLIMIT = 0x56,

        /// <summary>
        /// 读取从站浊度上限值
        /// </summary>
        READ_TURBIDITYUPLIMIT = 0x57,

        /// <summary>
        /// 读取从站浊度下限值
        /// </summary>
        READ_TURBIDITYLOWLIMIT = 0x58,

        /// <summary>
        /// 读取从站余氯校准值
        /// </summary>
        READ_RESIDUALCLSTANDVALUE = 0x46,

        /// <summary>
        /// 读取从站清洗时间间隔
        /// </summary>
        READ_CLEARINTERVAL = 0x4c,

        /// <summary>
        /// 读取从站加报时间间隔
        /// </summary>
        READ_DATAINTERVAL= 0x4d,

        /// <summary>
        /// 读取从余氯零点值
        /// </summary>
        READ_RESIDUALCLZERO = 0x4d,

        /// <summary>
        /// 读取从站余氯灵敏度
        /// </summary>
        READ_RESIDUALCLSENSITIVITY = 0x4e,

        /// <summary>
        /// 读取从站供电方式
        /// </summary>
        READ_POWERSUPPLYTYPE = 0x5E,

        /// <summary>
        /// 读取从站
        /// </summary>
        READ_ID = 0x48,
        READ_IP = 0x49,
        READ_PORT = 0x4a,
        /// <summary>
        /// 招测
        /// </summary>
        //CallData = 0x64,
        /// <summary>
        /// 读取GPRS开关状态
        /// </summary>
        READ_GPRSSWITCH = 0x4B,
        /// <summary>
        /// 读取工作方式
        /// </summary>
        READ_WORKTYPE = 0x5A,
        /// <summary>
        /// 读取密码
        /// </summary>
        READ_PASSWORD = 0x50,
        /// <summary>
        /// 读取中心站地址
        /// </summary>
        READ_CENTERADDR = 0x4E,
        /// <summary>
        /// 读取遥测站地址
        /// </summary>
        READ_TERADDR = 0x4F,
        #endregion

        #region 控制命令
        /// <summary>
        /// 复位命令
        /// </summary>
        RESET = 0x72,
        /// <summary>
        /// 启动终端采集功能命令
        /// </summary>
        EnableCollect = 0x73
        #endregion
    }

    public enum HYDRANT_COMMAND
    {
        #region 发送
        /// <summary>
        /// 响应命令发送
        /// </summary>
        SEND_RESPONSE_DATA = 0xa0,
        #endregion

        #region 设置
        /// <summary>
        /// 复位
        /// </summary>
        RESET = 0x16,
        /// <summary>
        /// 设置通讯时间
        /// </summary>
        SET_COMTIME= 0x17,
        /// <summary>
        /// 设置压力量程
        /// </summary>
        SET_PRERANGE=0x18,
        /// <summary>
        /// 设置压力偏移值
        /// </summary>
        SET_PREOFFSET= 0x19,
        /// <summary>
        /// 设置时间
        /// </summary>
        SET_TIME = 0x10,
        /// <summary>
        /// 设置消防栓编号
        /// </summary>
        SET_ID = 0x11,
        /// <summary>
        /// 设置消防栓IP地址
        /// </summary>
        SET_IP = 0x12,
        /// <summary>
        /// 设置消防栓端口号
        /// </summary>
        SET_PORT = 0x13,
        /// <summary>
        /// 设置压力配置
        /// </summary>
        SET_PRECONFIG = 0x14,
        /// <summary>
        /// 设置启动采集
        /// </summary>
        ENABLECOLLECT = 0x15,
        #endregion

        #region 读取
        /// <summary>
        /// 读取时间
        /// </summary>
        READ_TIME = 0x40,
        /// <summary>
        /// 读取编号
        /// </summary>
        READ_ID = 0x41,
        /// <summary>
        /// 读取ip地址
        /// </summary>
        READ_IP = 0x42,
        /// <summary>
        /// 读取端口号
        /// </summary>
        READ_PORT = 0x43,
        /// <summary>
        /// 读取通讯时间
        /// </summary>
        READ_COMTIME= 0x47,
        /// <summary>
        /// 读取压力量程
        /// </summary>
        READ_PRERANGE = 0x48,
        /// <summary>
        /// 读取压力偏移量
        /// </summary>
        READ_PREOFFSET = 0x49,
        /// <summary>
        /// 读取压力实时数据
        /// </summary>
        READ_REALTIMEDATA = 0x4A,
        /// <summary>
        /// 读取电话号码
        /// </summary>
        READ_TELNUM = 0x14,
        /// <summary>
        /// 读取通讯方式
        /// </summary>
        READ_COMMTYPE = 0x15,
        /// <summary>
        /// 读取消防栓打开历史数据
        /// </summary>
        READ_OPEN_HISTORY = 0xA0,
        /// <summary>
        /// 读取消防栓关闭历史数据
        /// </summary>
        READ_CLOSE_HISTORY = 0xA1,
        /// <summary>
        /// 读取消防栓开度历史数据
        /// </summary>
        READ_OPENANGLE_HISTORY = 0xA2,
        /// <summary>
        /// 读取消防栓被撞历史数据
        /// </summary>
        READ_IMPACT_HISTORY = 0xA3,
        /// <summary>
        /// 读取消防栓撞倒历史数据
        /// </summary>
        READ_KNOCKOVER_HISTORY = 0xA4,
        /// <summary>
        /// 读取消防栓压力配置
        /// </summary>
        READ_PRECONFIG = 0x44,
        /// <summary>
        /// 读取消防栓开度
        /// </summary>
        READ_NUMOFTURNS = 0x45,
        /// <summary>
        /// 读取消防栓开关状态
        /// </summary>
        READ_ENABLE = 0x46,
        #endregion
    }

    public enum SL651_COMMAND
    {
        /// <summary>
        /// 测试报
        /// </summary>
        TestReport = 0x30,
        /// <summary>
        /// 均匀时段水文报
        /// </summary>
        UniformityTimeReport = 0x31,
        /// <summary>
        /// 定时报
        /// </summary>
        TimingReport = 0x32,
        /// <summary>
        /// 遥测站加报
        /// </summary>
        AddtionReport = 0x33,
        /// <summary>
        /// 遥测站小时报
        /// </summary>
        HourReport = 0x34,
        /// <summary>
        /// 人工置数报
        /// </summary>
        ManualSetParmReport = 0x35,
        /// <summary>
        /// 查询实时数据
        /// </summary>
        QueryCurData = 0x37,
        /// <summary>
        /// 查询时段降水量
        /// </summary>
        QueryPrecipitation = 0x38,
        /// <summary>
        /// 读取人工置数
        /// </summary>
        QueryManualSetParm = 0x39,
        /// <summary>
        /// 查询指定要素
        /// </summary>
        QueryElements = 0x3A,
        /// <summary>
        /// 修改基本配置表
        /// </summary>
        SetBasicConfig = 0x40,
        /// <summary>
        /// 读取基本配置表
        /// </summary>
        ReadBasiConfig = 0x41,
        /// <summary>
        /// 修改运行配置表
        /// </summary>
        SetRunConfig = 0x42,
        /// <summary>
        /// 读取运行配置表
        /// </summary>
        ReadRunConfig = 0x43,
        /// <summary>
        /// 查询版本
        /// </summary>
        QueryVer = 0x45,
        /// <summary>
        /// 查询状态和报警
        /// </summary>
        QueryAlarm = 0x46,
        /// <summary>
        /// 初始化固态存储数据
        /// </summary>
        InitFlash = 0x47,
        /// <summary>
        /// 恢复出厂设置
        /// </summary>
        Init = 0x48,
        /// <summary>
        /// 修改密码
        /// </summary>
        ChPwd = 0x49,
        /// <summary>
        /// 设置时间
        /// </summary>
        SetTime = 0x4A,
        /// <summary>
        /// 水量定值控制
        /// </summary>
        PrecipitationConstantCtrl = 0xE5,  // 0x4F,
        /// <summary>
        /// 查询事件记录
        /// </summary>
        QueryEvent = 0x50,
        /// <summary>
        /// 查询时间
        /// </summary>
        QueryTime = 0x51,
        /// <summary>
        /// 设置人工置数内容
        /// </summary>
        SetManualSetParm = 0xE0,
        /// <summary>
        /// 设置校准水位1
        /// </summary>
        SetCalibration1 = 0xE1,
        /// <summary>
        /// 设置校准水位2
        /// </summary>
        SetCalibration2 = 0xE2,
        /// <summary>
        /// 设置均匀时段报上传时间
        /// </summary>
        SetTimeIntervalReportTime = 0xE3,
        /// <summary>
        /// 读取均匀时段报上传时间
        /// </summary>
        ReadTimeIntervalReportTime = 0xE4,
        
    }

    /// <summary>
    /// 控制码类型
    /// </summary>
    public enum CTRL_COMMAND_TYPE
    {
        /// <summary>
        /// 由主站发出的命令帧(应答帧) 如：主站向从站设置时钟 0x8
        /// </summary>
        REQUEST_BY_MASTER = 0x8,
        /// <summary>
        /// 由主站发出的应答帧 如：主站回应从站接收采集数据成功。0x4
        /// </summary>
        RESPONSE_BY_MASTER = 0x4,
        /// <summary>
        /// 由从站发出的数据帧 如：从站向主站传送采集数据。0x2
        /// </summary>
        REQUEST_BY_SLAVE = 0x2,
        /// <summary>
        /// 由从站发出的应答帧 如：从站应答主站读取终端采集时间间隔。0x1
        /// </summary>
        RESPONSE_BY_SLAVE = 0x1,
    }

    #region 远传命令
    /// <summary>
    /// 压力终端远传命令参数
    /// </summary>
    public enum PreTer_COMMAND
    {
        /// <summary>
        /// 设置时间
        /// </summary>
        SET_TIME = 0X10,
        /// <summary>
        /// 压力时间间隔
        /// </summary>
        WRITE_PREINTERVAL = 0x11,
        /// <summary>
        /// 压力偏移量
        /// </summary>
        WRITE_PREOFFSET = 0x12,
        /// <summary>
        /// 流量时间间隔
        /// </summary>
        WRITE_FLOWINTERVAL = 0x14,
        /// <summary>
        /// 压力报警上限值
        /// </summary>
        WRITE_PRE_UPPERLIMIT = 0x15,
        /// <summary>
        /// 压力报警下限值
        /// </summary>
        WRITE_PRE_LOWLIMIT = 0x16,
        /// <summary>
        /// 电池电压报警下限值
        /// </summary>
        WRITE_BATTERY_LOWLIMIT = 0x17,
        /// <summary>
        /// 斜率报警上限值
        /// </summary>
        WRITE_SLOPE_UPPERLIMIT = 0x18,
        /// <summary>
        /// 斜率报警下限值
        /// </summary>
        WRITE_SLOPE_LOWLIMIT = 0x19,
        /// <summary>
        /// 心跳时间间隔
        /// </summary>
        WRITE_HEARTINTERVAL = 0x1A,
        /// <summary>
        /// 设置采集功能配置
        /// </summary>
        EnableCollect = 0x1B,
        /// <summary>
        /// 设置通讯方式
        /// </summary>
        WRITE_COMTYPE = 0x1C,
        /// <summary>
        /// 485波特率
        /// </summary>
        WRITE_BAUD = 0x1D,
        /// <summary>
        /// 设置ID
        /// </summary>
        WRITE_SETID = 0x1E,
        /// <summary>
        /// 设置IP
        /// </summary>
        WRITE_IP = 0x1F,
        /// <summary>
        /// 设置端口
        /// </summary>
        WRITE_PORT = 0x20,
        /// <summary>
        /// 设置短信手机号
        /// </summary>
        WRITE_CELLPHONE = 0x21,
        /// <summary>
        /// 上限报警功能投/退
        /// </summary>
        WRITE_PREUPPERALARM_ENABLE = 0x22,
        /// <summary>
        /// 下限报警功能投/退
        /// </summary>
        WRITE_PRELOWALARM_ENABLE = 0x23,
        /// <summary>
        /// 斜率上限报警投/退
        /// </summary>
        WRITE_SLOPEUPPERALARM_ENABLE = 0x24,
        /// <summary>
        /// 斜率下限报警投/退
        /// </summary>
        WRITE_SLOPELOWALARM_ENABLE = 0x25,
        /// <summary>
        /// 电池电压采集时间间隔
        /// </summary>
        WRITE_BATTERY_INTERVAL = 0x26,
        /// <summary>
        /// 压力量程
        /// </summary>
        WRITE_PRE_SPAN = 0x28,
        /// <summary>
        /// 复位
        /// </summary>
        RESET = 0x72,
        /// <summary>
        /// 读取时间
        /// </summary>
        READ_TIME = 0x40,
        /// <summary>
        /// 压力时间间隔
        /// </summary>
        READ_PREINTERVAL = 0x41,
        /// <summary>
        /// 压力偏移量
        /// </summary>
        READ_PREOFFSET = 0x42,
        /// <summary>
        /// 流量时间间隔
        /// </summary>
        READ_FLOWINTERVAL = 0x44,
        /// <summary>
        /// 压力报警上限值
        /// </summary>
        READ_PRE_UPPERLIMIT = 0x45,
        /// <summary>
        /// 压力报警下限值
        /// </summary>
        READ_PRE_LOWLIMIT = 0x46,
        /// <summary>
        /// 电池电压报警下限值
        /// </summary>
        READ_BATTERY_LOWLIMIT = 0x47,
        /// <summary>
        /// 斜率报警上限值
        /// </summary>
        READ_SLOPE_UPPERLIMIT = 0x48,
        /// <summary>
        /// 斜率报警下限值
        /// </summary>
        READ_SLOPE_LOWLIMIT = 0x49,
        /// <summary>
        /// 心跳时间间隔
        /// </summary>
        READ_HEARTINTERVAL = 0x4A,
        /// <summary>
        /// 读取采集功能配置
        /// </summary>
        READ_EnableCollect = 0x4B,
        /// <summary>
        /// 设置通讯方式
        /// </summary>
        READ_COMTYPE = 0x4C,
        /// <summary>
        /// 485波特率
        /// </summary>
        READ_BAUD = 0x4D,
        /// <summary>
        /// ID
        /// </summary>
        READ_ID = 0x4E,
        /// <summary>
        /// IP
        /// </summary>
        READ_IP = 0x4F,
        /// <summary>
        /// 端口
        /// </summary>
        READ_PORT = 0x50,
        /// <summary>
        /// 设置短信手机号
        /// </summary>
        READ_CELLPHONE = 0x51,
        /// <summary>
        /// 上限报警功能投/退
        /// </summary>
        READ_PREUPPERALARM_ENABLE = 0x52,
        /// <summary>
        /// 下限报警功能投/退
        /// </summary>
        READ_PRELOWALARM_ENABLE = 0x53,
        /// <summary>
        /// 斜率上限报警投/退
        /// </summary>
        READ_SLOPEUPPERALARM_ENABLE = 0x54,
        /// <summary>
        /// 斜率下限报警投/退
        /// </summary>
        READ_SLOPELOWALARM_ENABLE = 0x55,
        /// <summary>
        /// 电池电压采集时间间隔
        /// </summary>
        READ_BATTERY_INTERVAL = 0x56,
        /// <summary>
        /// 压力量程
        /// </summary>
        READ_PRE_SPAN = 0x58,
        /// <summary>
        /// 设置是否允许招测
        /// </summary>
        EnableCallData = 0x70,
        /// <summary>
        /// 读取招测数据
        /// </summary>
        READ_CALLDATA= 0x71,
    }

    public enum GPRS_READ
    {
        /// <summary>
        /// 从站向主站发送压力采集数据
        /// </summary>
        READ_PREDATA = 0xA0,
        /// <summary>
        /// 从站向主站发送流量采集数据
        /// </summary>
        READ_FLOWDATA = 0xA1,
        /// <summary>
        /// 从站向主站发送设备报警信息
        /// </summary>
        READ_ALARMINFO = 0xA2,
        /// <summary>
        /// 通用终端发送心跳
        /// </summary>
        READ_UNIVERSAL_HEART = 0xA3,
        /// <summary>
        /// 压力从站向主站发送流量采集数据
        /// </summary>
        READ_PREFLOWDATA = 0xA4,
        /// <summary>
        /// 通用终端发送脉冲数据
        /// </summary>
        READ_UNIVERSAL_PLUSE=0xA7,
        /// <summary>
        /// 通用终端发送模拟量数据
        /// </summary>
        READ_UNIVERSAL_SIM=0xA8,
        /// <summary>
        /// 通用终端发送RS485 数据
        /// </summary>
        READ_UNVERSAL_RS485=0xA4,
        /// <summary>
        /// 水质终端发送浊度数据
        /// </summary>
        READ_TURBIDITY=0xA0,
        /// <summary>
        /// 水质终端发送余氯数据
        /// </summary>
        READ_RESIDUALCL=0xA1,
        /// <summary>
        /// 水质终端发送PH数据
        /// </summary>
        READ_PH = 0xA2,
        /// <summary>
        /// 水质终端发送电导率数据
        /// </summary>
        READ_CONDUCTIVITY=0xA3,
        /// <summary>
        /// 水质终端流量
        /// </summary>
        READ_OLWQFLOW=0xA4,
        /// <summary>
        /// 水质终端温度
        /// </summary>
        READ_TEMP = 0xA5,
        /// <summary>
        /// 水质终端报警
        /// </summary>
        READ_OLWQALARM= 0xAB,
        /// <summary>
        /// 消防栓打开
        /// </summary>
        READ_HYDRANT_OPEN = 0xA0,
        /// <summary>
        /// 消防栓关闭
        /// </summary>
        READ_HYDRANT_CLOSE=0xA1,
        /// <summary>
        /// 消防栓开度
        /// </summary>
        READ_HYDRANT_OPENANGLE = 0xA2,
        /// <summary>
        /// 消防栓撞击
        /// </summary>
        READ_HYDRANT_IMPACT = 0xA3,
        /// <summary>
        /// 消防栓撞倒
        /// </summary>
        READ_HYDRANT_KNOCKOVER = 0xA4,
        /// <summary>
        /// 消防栓定时远传
        /// </summary>
        READ_HYDRANT_TIMEGPRS = 0xA5,
        /// <summary>
        /// 远传控制器向主站发送噪声采集数据
        /// </summary>
        READ_NOISEDATA = 0xA4,
        /// <summary>
        /// 水厂PLC采集数据
        /// </summary>
        READ_WATERWORKSDATA = 0xA0,
    }

    public enum PRECTRL_COMMAND
    {
        /// <summary>
        /// 设置时间
        /// </summary>
        SET_TIME=0x10,
        /// <summary>
        /// 读取压力和流量数据
        /// </summary>
        READ_DATA= 0xA3,
    }
    #endregion

    class CommandInfo
    {

    }
}
