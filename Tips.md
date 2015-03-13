# Tips #

  * x64平台下的编译选项要选择Any CPU **x86** (.Net平台使用非托管的DLL时会抛出BadImageFormatException异常)

# WPF部分 #

## WPF页面继承要点 ##

  * 基页面必须全用 **CS** 代码方式实现，不能用**XAML** 编写

  * 继承页的根属性用 **命名空间关键字：类名** 的方式编写，命名空间关键字要明确指定为哪个命名空间,如

```
 <game:GamePage xmlns:game="clr-namespace:Demo">
 </game:GamePage>
```


# HTK部分 #

## 编译 ##

  * 所有编译任务进行之前要注意换行符编码,win下以CR/LF表示换行，而linux的换行符是LF,二者区别请Google.

  * HSLab在Win32系统下使用HGraf\_WIN32.c编译，不要使用HGraf.null.c和HGraf.c；在vs2008中默认编码为Unicode，此时生成的HSLab显示为乱码，要修正这个问题，要把HTKLib和HTKTools的编译选项中的Property->Configuration Proproties->General->Character Set设为Not Set。

  * HTKLib一般被编译成静态库，项目的Property->Configuration Proproties->General->Configuration Type为Static Libraray


## 封装 ##

  * VS2008中奇怪问题：要把HTK封装成c#可以调用的DLL，需要设置Property->Configuration Proproties->General->Common Language Runtime support为Common Language Runtime Support(/clr)，这是一般会出现错误cl : Command line error D8045 : cannot compile C file '.\esig\_asc.c' with the /clr option，经查阅文档可知/clr选项需要所编译代码为c++形式，所以Property->Configuration Proproties->C/C++->Advanced->Compile As需要设为Compile as C++ Code(/TP)，但VS2008默认此项就是如此，经常是发现只要把此选项改为Default编译一下，遇到同样错误后再把该选项改回来就可以正常编译了。

# 其他 #