# RBAC-With-Attribute

<ul>
Usage用法 
  <li><strong>Operation继承MarshalByRefObject</strong>
<br>方法打上标记Attribute
<hr>
<li>动态的透明代理OperationProxy 
<hr>
<li>OperationRealProxy继承RealProxy 
<br>实现方法 
<br>OnInvoke
<br>OnEntry
<br>OnExit
<hr>
<li>调用，织入 
<br>var op = OperationProxy.Create<Operation>();
<br>op.WriteInfo();
<br>op.Free();
<hr>
<li>对比 
<br>var op = new Operation();
<br>op.WriteInfo();
<br>op.Free();
  </li>
 </ul>
Online Test在线测试代码
https://dotnetfiddle.net/QxmwVQ
