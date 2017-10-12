using System;

class lib02
{
	string name;
	
	public lib02(string name){
		this.name = name;
	}
	
	public string greets2(string surname){
		return ("Hello " + name + " " + surname);
	}
	
	public static string greets(string name){
		return ("Hello " + name);
	}
}