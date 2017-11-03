using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

class control
{
	[DllImport("user32.dll")]
	private static extern void mouse_event(UInt32 dwFlags, UInt32 dx, UInt32 dy, UInt32 dwData, IntPtr dwExtraInfo);

    private const uint MOUSEEVENTF_LEFTDOWN = 0x02;
    private const uint MOUSEEVENTF_LEFTUP = 0x04;
    private const uint MOUSEEVENTF_RIGHTDOWN = 0x08;
    private const uint MOUSEEVENTF_RIGHTUP = 0x10;
	
	public static void rightdown(string a, string b){
		mousemove(a,b);
		mouse_event(MOUSEEVENTF_RIGHTDOWN, Convert.ToUInt32(a), Convert.ToUInt32(b), 0, new IntPtr());
	}
	
	public static void leftdown(string a, string b){
		mousemove(a,b);
		mouse_event(MOUSEEVENTF_LEFTDOWN, Convert.ToUInt32(a), Convert.ToUInt32(b), 0, new IntPtr());
	}
	
	public static void rightup(string a, string b){
		mousemove(a,b);
		mouse_event(MOUSEEVENTF_RIGHTUP, Convert.ToUInt32(a), Convert.ToUInt32(b), 0, new IntPtr());
	}
	
	public static void leftup(string a, string b){
		mousemove(a,b);
		mouse_event(MOUSEEVENTF_LEFTUP, Convert.ToUInt32(a), Convert.ToUInt32(b), 0, new IntPtr());
	}
	
	public static void rightclick(string a, string b){
		rightdown(a,b);
		rightup(a,b);
	}
	
	public static void leftclick(string a, string b){
		leftdown(a,b);
		leftup(a,b);
	}
	
	public static void mousemove(string a, string b){
		Cursor.Position = new Point(int.Parse(a), int.Parse(b));
	}
	
	public static void key(string key){
		SendKeys.SendWait(key);
	}
}