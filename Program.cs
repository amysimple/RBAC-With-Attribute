using System;

using System.Runtime.Remoting.Proxies;
using System.Runtime.Remoting.Messaging;


namespace ConsoleApplication1
{
	public class Program
	{
		public static void Main(string[] args)
		{
			  
			var op = OperationProxy.Create<Operation>();
			op.WriteInfo();
			op.Free();
		}
	}

	public interface IOperation
	{
		void WriteInfo();
	}

	public class Operation : MarshalByRefObject, IOperation
	{
		[Permisson]
		public void WriteInfo()
		{
			Console.WriteLine(Guid.NewGuid().ToString());
		}
		public void Free()
		{
			Console.WriteLine("im good\n");
		}
	}

	public abstract class OperationProxy
	{
		public static T Create<T>()
		{
			T instance = Activator.CreateInstance<T>();
			OperationRealProxy<T> realProxy = new OperationRealProxy<T>(instance);
			T transparentProxy = (T)realProxy.GetTransparentProxy();
			return transparentProxy;
		}
	}

	public class OperationRealProxy<T> : RealProxy
	{
		private T _target;
		public OperationRealProxy(T target): base (typeof (T))
		{
			this._target = target;
		}

		public override IMessage Invoke(IMessage msg)
		{
			IMethodCallMessage callmessage = (IMethodCallMessage)msg;
			object returnValue = null;
			ReturnMessage message;
			if (OnInvoke(callmessage))
			{
				OnEntry(callmessage);
				returnValue = callmessage.MethodBase.Invoke(this._target, callmessage.Args);
				message = new ReturnMessage(returnValue, new object[0], 0, null, callmessage);
				OnExit(message);
			}
			else
				message = new ReturnMessage(returnValue, new object[0], 0, null, callmessage);
			return message;
		}

		public bool OnInvoke(IMethodCallMessage msg)
		{
			Console.WriteLine( "OnInvoke\n" );
			
			bool _continue = _target.GetType().GetMethod(msg.MethodName.ToString()).IsDefined(typeof (PermissonAttribute), false);
			if (_continue)
			{
				Console.WriteLine("没有通过权限检查\n");
				return false;
			}

			return true;
		}

		public void OnEntry(IMethodCallMessage msg)
		{
			Console.WriteLine("OnEntry\n");
		}

		public void OnExit(ReturnMessage msg)
		{
			Console.WriteLine("OnExit\n");
		}
	}

	public class PermissonAttribute : Attribute
	{
		public PermissonAttribute()
		{
		}
	}
}
