using System;
using System.IO;

class file
{
	private string path;
	
	public file(string path){
		this.path = path;
	}
	
	public void settarget(string target){
		path = target;
	}
	
	public void rewrite(string co){
		File.WriteAllText(path,co);
	}
	
	public void write(string co){
		string pre = "";
		if(File.Exists(path))
			pre = File.ReadAllText(path);
		File.WriteAllText(path,pre+co);
	}
	
	public string read(){
		return File.ReadAllText(path);
	}
	
	public void delete(){
		File.Delete(path);
	}
	
	public void create(){
		File.Create(path);
	}
}