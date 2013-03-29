将补丁拷贝到目录后，需要做下面几件事情

1.配置告警的的设置

打开nanrui.alarm.sender\nanrui.alarm.config文件
找到下列MQ的配置，更改它们，不知道如何填写请问南瑞的人，对他们说是mq的配
置他们就明白了
  <MQ value="activemq:tcp://172.16.85.102:61616">
    <!-- 消息队列的用户名 -->
    <UserName value="guest"/>
    <!-- 消息队列的用户密码 -->
    <PassWord value="guest"/>
    <!-- ActiveMQ中的消息的ClientId,可以为空-->
    <ClientId value="" />
    <!-- ActiveMQ中的消息的目的地 -->
    <DestinationName value="event" />
    <!-- MQ 发生错误时,消息暂存的目录 -->
    <FailedPath value="Nanrui\AlarmQueue" />
    <!-- 消息的编码 -->
    <Encoding value="GB18030" />	
  </MQ>

打开 nms\deploy\BTNM\config\Monitor.Config文件，找到下面4个配置,需要根据用户的信息来更改
	<!-- 发生告警的地点信息，写相应公司的中文全称 -->
    <add key="IBM_TEC.AREANAME" value="江苏省电力公司本部" />
    <!-- 告警所属业务系统 -->
    <add key="IBM_TEC.BUSSINESSNAME" value="" />
    <!-- 当告警事件不能归到某个对象时指定的对象 -->
    <add key="IBM_TEC.OtherResourceType" value="" />
    <!-- 网络管理对象的前缀 -->
    <add key="IBM_TEC.PreFix" value="" />

2.配置性能数据的设置

打开nanrui.performance.sender\nanrui.performance.config文件,在这里只要更改下面的就可以了,同
样不知道如何填写请问南瑞的人，对他们说是mq的配置他们就明白了
	
<!-- 消息队列的BrokerUri -->
  <MQ value="activemq:tcp://localhost:61616">
    <!-- 消息队列的用户名 -->
    <UserName value="guest"/>
    <!-- 消息队列的用户密码 -->
    <PassWord value="guest"/>
    <!-- ActiveMQ中的消息的目的地 -->
    <DestinationName value="Tester" />    
	<!-- ActiveMQ中的消息的ClientId,可以为空-->
    <ClientId value=""/>
  </MQ>
  <Locale>
    <!-- 网络管理对象的前缀 -->
    <PrefixId value="" />
    <!-- 写相应公司的中文全称 -->
    <Corporation value="江苏省电力公司本部" />
  </Locale>
  
  注意配置有很多,其它不用改
  
3.配置配置数据接口的设置

打开 nms\deploy\BTNM\Bin\nanrui.modul.config 文件,在这里只要更改下面的就可以了,同
样不知道如何填写请问南瑞的人，对他们说是mq的配置他们就明白了

  <!-- value为ActiveMQ的通道-->
    <MQ value="activemq:tcp://172.16.85.102:61616">
    <!-- 消息队列的用户名 -->
    <UserName value="guest"/>
    <!-- 消息队列的用户密码 -->
    <PassWord value="guest"/>
    <!-- ActiveMQ中的消息的ClientId,可以为空-->
    <ClientId value=""/>
    <!-- ActiveMQ中的消息的目的地 -->
    <DestinationName value="File" />
    <!-- MQ 发生错误时,消息暂存的目录 -->
    <FailedPath value="Nanrui\AlarmQueue" />
  </MQ>
  <Locale>
    <!-- 网络管理对象的前缀 -->
    <PrefixId value="" />
    <!-- 写相应公司的中文全称 -->
    <Corporation value="江苏省电力公司本部" />
  </Locale>

4. 执行数据库脚本
如果使用的数据据库用的是ms sqlserver,请执行 nms\deploy\BTNM\bin\Nanrui.mssql.sql 脚本
如果使用的数据据库用的是oracle,请执行 nms\deploy\BTNM\bin\Nanrui.oracle.sql 脚本


