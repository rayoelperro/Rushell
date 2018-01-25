using System;
using System.IO;

class file
{
	string path;

	public file(string path){
		this.path = path;
	}

	public void SetPath(string path){
		this.path = path;
	}

	public string Read(){
		return File.ReadAllText(path);
	}

	public void Write(string text){
		File.WriteAllText(path,text);
	}
}