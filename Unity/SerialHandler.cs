﻿using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System.Threading;

public class SerialHandler : MonoBehaviour
{
	public delegate void SerialDataReceivedEventHandler(string message);
	public event SerialDataReceivedEventHandler OnDataReceived;

	public string portName = "/dev/cu.usbmodem1411";
	public int baudRate    = 115200;

	private SerialPort serialPort_;
	private Thread thread_;
	private bool isRunning_ = false;

	private string message_;
	private bool isNewMessageReceived_ = false;

	void Awake()
	{
		Open();
	}

	void Update()
	{
		if (isNewMessageReceived_) {
			Debug.Log("received: " + message_);
		}
	}

	void OnDestroy()
	{
		Close();
	}

	private void Open()
	{
		string[] portNames = SerialPort.GetPortNames();
		foreach(string name in portNames)
			Debug.Log(name);

		serialPort_ = new SerialPort(portName, baudRate, Parity.None, 8, StopBits.One);
		serialPort_.Open();

		isRunning_ = true;

		thread_ = new Thread(Read);
		//thread_.Start();						/* 不開 read thread */
	}

	private void Close()
	{
		isRunning_ = false;

		if (thread_ != null && thread_.IsAlive) {
			thread_.Join();
		}

		if (serialPort_ != null && serialPort_.IsOpen) {
			serialPort_.Close();
			serialPort_.Dispose();
		}
	}

	private void Read()
	{
		while (isRunning_ && serialPort_ != null && serialPort_.IsOpen) {
			try {
				if (serialPort_.BytesToRead > 0) {
					message_ = serialPort_.ReadLine();
					isNewMessageReceived_ = true;
				}
			} catch (System.Exception e) {
				Debug.LogWarning(e.Message);
			}
		}
	}

	public void Write(string message)
	{
		try {
			serialPort_.Write(message);
		} catch (System.Exception e) {
			//Debug.LogWarning(e.Message);
		}
	}
}
