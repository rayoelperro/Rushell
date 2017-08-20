using System;
using System.Windows.Forms;
class frame
{
    public static void Main(string[] args){
        Form window = new Form();
        window.Text = args[0];
        Application.Run(window);
    }
}