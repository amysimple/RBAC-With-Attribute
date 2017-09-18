using System;
using System.Threading;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            
            var op = new Operation();
            //op.WriteThreadInfo();
            op.Call("WriteThreadInfo");
            op.Write1();
            op.WriteOne();
            op.Write2();
            op.WriteThreadInfo();


           
 


            Console.ReadKey();

        }
 
    }
    
    public class Operation : PermissonAutoCheck
    {
        

        [Permisson]
        public void Write1()
        {
            Console.WriteLine("使用了Write1方法");
        }
        [Permisson]
        public void WriteOne()
        {
            Console.WriteLine("使用了WriteOne方法");
        }
        
        public void Write2()
        {
            Console.WriteLine("本类="+this.GetType().ToString());//当类名用

            Console.WriteLine("被这个类调用=" + new System.Diagnostics.StackTrace(true).GetFrame(1).GetMethod().DeclaringType.ToString());

            Console.WriteLine("被这个方法调用=" + new System.Diagnostics.StackTrace(true).GetFrame(1).GetMethod().Name);

            Console.WriteLine("本方法=" + System.Reflection.MethodBase.GetCurrentMethod().Name);
        }
        [Permisson]
        public void WriteThreadInfo()
        {
            Console.WriteLine(Thread.CurrentThread.GetHashCode().ToString());
        }
    }

    public abstract class PermissonAutoCheck {
        public void Call(string method, object[] parameters)
        {
            object target = this;

            System.Reflection.MethodInfo refMethod = target.GetType().GetMethod(method);

            //不存在的方法
            if (refMethod == null)
            {
                throw new Exception(string.Format("“{0}”未包含“{1}”的定义，并且找不到可接受第一个“{0}”类型参数的扩展方法“{1}”(是否缺少 using 指令或程序集引用 ?)", new object[] { target.GetType(), method }));
            }

            //类指定 | 方法指定 
            if (target.GetType().IsDefined(typeof(PermissonAttribute), false) | refMethod.IsDefined(typeof(PermissonAttribute), false))
            {
                //比对用户权限
                if (true) { refMethod.Invoke(target, parameters); }
                else { Console.WriteLine("权限失败，终止"); }
            }
        }
        public void Call(string method)
        {
            Call(method, null);
        }
    }


    internal class PermissonAttribute : Attribute
    {
        public PermissonAttribute() { }
       
    }
}
