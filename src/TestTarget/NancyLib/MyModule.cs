using Nancy;

namespace TestTarget.NancyLib
{
    public class MyModule : NancyModule
    {
        public MyModule()
        {
            var myObject = new { Text = "text1" };

            Get["/bare_string"] = _ => "text2";
            Get["/use_this"] = _ => this.GetType();
            Get["/closure_no_this"] = _ => myObject.Text;
            Get["/closure_with_this"] = _ => this.GetType().FullName + myObject.Text;
        }
    }
}