using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

class SocketServer
{
	private TcpListener listener;
	
	private TcpClient client;
	
	public SocketServer(string pport)
	{
		int port = int.Parse(pport);
		client = null;
		listener = new TcpListener(IPAddress.Any, port);
        listener.Start();
	}
	
	public void waitclient(){
		client = listener.AcceptTcpClient();
	}
	
	public string getmsg(){
		if(client != null){
			NetworkStream stream = client.GetStream();
			return new StreamReader(stream).ReadToEnd();
		}
		return null;
	}
	
	public void close(){
		listener.Stop();
	}
}

class SocketClient
{
	private TcpClient connect;
	
	public SocketClient(string adrs, string pport)
	{
		int port = int.Parse(pport);
		connect = new TcpClient(adrs,port);
	}
	
	public void sendmsg(string text){
		NetworkStream sm = connect.GetStream();
        StreamWriter sw = new StreamWriter(sm);
        sw.Write(text);
        sw.Close();
        sm.Close();
	}
	
	public void close(){
		connect.Close();
	}
}