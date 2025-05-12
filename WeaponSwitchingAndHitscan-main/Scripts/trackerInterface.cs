using Godot;
using System;
using System.IO.Ports;

public partial class trackerInterface : Node
{
	public SerialPort serialPort;
	public SerialPort serialPort2;
	bool discardedTrackerLastFrame;
	bool discardedTriggerLastFrame;
	public float x;
	public float y;
	public float z;
	public bool triggerDown;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		serialPort = new SerialPort();
		serialPort.PortName = "/dev/ttyUSB1";
		serialPort.BaudRate = 19200;
		serialPort.Open();
		serialPort2 = new SerialPort();
		serialPort2.PortName = "/dev/ttyUSB0";
		serialPort2.BaudRate = 9600;
		serialPort2.Open();
	}
	
	public void ReOpenTrigger(){
		serialPort2.Close();
		serialPort2 = new SerialPort();
		serialPort2.PortName = "/dev/ttyUSB0";
		serialPort2.BaudRate = 9600;
		serialPort2.Open();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
		if(!serialPort.IsOpen){
			x = -3.14f;
			return;
		}
		
		string message = "";
		
		if(discardedTrackerLastFrame){
			serialPort.ReadLine();
		}
		message = serialPort.ReadLine();
		if(serialPort.BytesToRead > 8000){
			serialPort.DiscardInBuffer();
			discardedTrackerLastFrame = true;
		}
		else{
			discardedTrackerLastFrame = false;
		}
		//while(serialPort.BytesToRead > 5000){
			//message = serialPort.ReadLine();
		//}
		//serialPort.DiscardInBuffer();
		
		if(message.Length > 0){
			string xS = message.Substring(0, message.IndexOf('/'));
			x = float.Parse(xS);
			string m2 = message.Substring(message.IndexOf('/') + 1);
			string yS = m2.Substring(0, m2.IndexOf('/'));
			y = float.Parse(yS);
			string zS = m2.Substring(m2.IndexOf('/') + 1);
			z = float.Parse(zS);
		}
		//x = -6.28f;
		
		if(!serialPort2.IsOpen){
			triggerDown = false;
			GD.Print("aaaaaaa");
			return;
		}
		
		//if(discardedTriggerLastFrame){
			//serialPort2.ReadLine();
		//}
		//message = serialPort2.ReadLine();
		//if(serialPort2.BytesToRead > 8000){
			//serialPort2.DiscardInBuffer();
			//discardedTriggerLastFrame = true;
		//}
		//else{
			//discardedTriggerLastFrame = false;
		//}
		while(serialPort2.BytesToRead > 500){
			message = serialPort2.ReadLine();
		}
		message = serialPort2.ReadLine();
		GD.Print(serialPort2.BytesToRead);
		GD.Print(message);
		GD.Print(serialPort.BytesToRead);
		//serialPort2.DiscardInBuffer();
		
		if(message.Length > 0){
			triggerDown = (message.Substring(0, 1) == "t");
		}
	}
}
