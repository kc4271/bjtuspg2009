# 更新日志 #

## 游戏UI部分 ##

  * 2009.12.05 [r1](https://code.google.com/p/bjtuspg2009/source/detail?r=1) 完成程序基本逻辑框架

  * 2010.01.06 [r2](https://code.google.com/p/bjtuspg2009/source/detail?r=2) 完成添加动画元素

  * 2010.01.07 [r12](https://code.google.com/p/bjtuspg2009/source/detail?r=12) 增加了宝箱等其他对象的平滑移动和事件触发;增加了站立的图片并能正确显示;完成初步代码重构.

  * 2010.01.08 [r30](https://code.google.com/p/bjtuspg2009/source/detail?r=30) 完成Chaining页面基本功能;可以进行文本输入游戏;封装了数据库连接,可以进行数据库交互.

  * 2010.01.08 [r33](https://code.google.com/p/bjtuspg2009/source/detail?r=33) 完善了GameMain页面;添加了若干新图片资源;更改字体为汉仪丫丫体简;更改按钮样式为鼠标经过放大.

  * 2010.01.09 [r38](https://code.google.com/p/bjtuspg2009/source/detail?r=38) 完善了Chaining页面,修正了所有已知错误;优化了在第一个字相同下的成语随机性;留出了道具接口; 基本实现了LoadGame页面和Shopping页面,并实现了简单的数据绑定.

  * 2010.01.10 [r42](https://code.google.com/p/bjtuspg2009/source/detail?r=42) 完成了背景动画的封装和代码重构;重新组织目录;MapInf定义了地图信息;做好了其他需要绑定XML页面的接口.

  * 2010.01.11 [r45](https://code.google.com/p/bjtuspg2009/source/detail?r=45) 封装完毕背景元素的加载;封装完毕精灵用户控件;完成了与存档XML的数据交互;完成了游戏过程中的数据流程;添加了新的资源文件.

  * 2010.03.28 [r67](https://code.google.com/p/bjtuspg2009/source/detail?r=67) 添加了语音识别引擎;完成了声学模型训练,词典和网络的生成;整个所有模块到主分支中.

## HTK封装部分 ##

  * 2010.01.12 用一种变通的方式解决了HTK到C#可用的DLL的问题.

  * 2010.01.13 训练好了一个简单的Yes/No模型.

  * 2010.01.13 将封装好的DLL和简单模型在C#命令行下成功运行.

  * 2010.03.28 整个ATK封装到托管DLL完毕,设置好了应用所需接口.