using System;
using System.IO;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

using Apache.NMS;
using Apache.NMS.Util;

namespace meijing
{
    public class MapMessage
    {
        //Apache.NMS.ActiveMQ.OpenWire.PrimitiveMap _header = new Apache.NMS.ActiveMQ.OpenWire.PrimitiveMap();
        //Apache.NMS.ActiveMQ.OpenWire.PrimitiveMap _body = new Apache.NMS.ActiveMQ.OpenWire.PrimitiveMap();

        IDictionary<string, object> _header = new Dictionary<string, object>();
        IDictionary<string, object> _body = new Dictionary<string, object>();


        public IDictionary<string, object> Properties
        {
            get { return _header; }
        }


        public IDictionary<string, object> Body
        {
            get { return _body; }
        }
    }

    public enum Destination
    {
        MODEL,
        ALERT,
        PERFORMANCE,
        DESKTERMINAL
    }

    public class InternalSession
    {
        static log4net.ILog _logger = log4net.LogManager.GetLogger("Betanetworks.MQ");
        string _nm;
        bool isFast = true;
        bool isDefault = false;
        Exception e;
        IConnection connection;
        ISession session;
        IDestination modeldestination;
        IMessageProducer modelProducer;
        IDestination alertDestination;
        IMessageProducer alertProducer;
        IDestination performanceDestination;
        IMessageProducer performanceProducer;
        IDestination deskTerminalDestination;
        IMessageProducer deskTerminalProducer;

        public InternalSession(string nm, Exception e)
        {
            this._nm = nm;
            this.e = e;
        }

        public void setDefault()
        {
            isDefault = true;
        }

        public InternalSession(string nm, IConnection connection, string model, string alert, string performance, string deskTerminal)
        {
            try
            {
                this._nm = nm;
                this.connection = connection;
                this.connection.Start();
                this.session = this.connection.CreateSession(AcknowledgementMode.AutoAcknowledge);

                modeldestination = Apache.NMS.Util.SessionUtil.GetDestination(this.session, model);
                alertDestination = Apache.NMS.Util.SessionUtil.GetDestination(this.session, alert);
                performanceDestination = Apache.NMS.Util.SessionUtil.GetDestination(this.session, performance);
                deskTerminalDestination = Apache.NMS.Util.SessionUtil.GetDestination(this.session, deskTerminal);
            }
            catch
            {
                Dispose();
                throw;
            }
        }

        public string Name
        {
            get { return _nm; }
        }


        public void Send(Destination destination, string message)
        {
            if (null != this.e)
                throw this.e;

            switch (destination)
            {
                case Destination.ALERT:
                    if (null == alertProducer)
                        alertProducer = this.session.CreateProducer(alertDestination);
                    Send(alertProducer, message);
                    return;
                case Destination.DESKTERMINAL:
                    if (null == deskTerminalProducer)
                        deskTerminalProducer = this.session.CreateProducer(deskTerminalDestination);
                    Send(deskTerminalProducer, message);
                    return;
                case Destination.MODEL:
                    if (null == modelProducer)
                        modelProducer = this.session.CreateProducer(modeldestination);
                    Send(modelProducer, message);
                    return;
                case Destination.PERFORMANCE:
                    if (null == performanceProducer)
                        performanceProducer = this.session.CreateProducer(performanceDestination);
                    Send(performanceProducer, message);
                    return;
            }
            throw new ArgumentOutOfRangeException();
        }

        public void Send(Destination destination, MapMessage message)
        {
            if (null != this.e)
                throw this.e;
            switch (destination)
            {
                case Destination.ALERT:
                    if (null == alertProducer)
                        alertProducer = this.session.CreateProducer(alertDestination);
                    Send(alertProducer, message);
                    return;
                case Destination.DESKTERMINAL:
                    if (null == deskTerminalProducer)
                        deskTerminalProducer = this.session.CreateProducer(deskTerminalDestination);
                    Send(deskTerminalProducer, message);
                    return;
                case Destination.MODEL:
                    if (null == modelProducer)
                        modelProducer = this.session.CreateProducer(modeldestination);
                    Send(modelProducer, message);
                    return;
                case Destination.PERFORMANCE:
                    if (null == performanceProducer)
                        performanceProducer = this.session.CreateProducer(performanceDestination);
                    Send(performanceProducer, message);
                    return;
            }
            throw new ArgumentOutOfRangeException();
        }

        void Send(IMessageProducer producer, string message)
        {
            ITextMessage txtMessage = producer.CreateTextMessage();
            txtMessage.Text = message;

            _logger.DebugFormat("开始向 {0} 发送消息", _nm);
            try
            {
                producer.Send(txtMessage);
                _logger.DebugFormat("向 {0} 发送消息完成", _nm);
            }
            catch (Exception e)
            {
                if (_logger.IsDebugEnabled)
                    _logger.Debug(string.Format("向 {0} 发送消息发生错误", _nm), e);
                throw;
            }
        }
        void Send(IMessageProducer producer, MapMessage message)
        {
            IMapMessage mapMessage = producer.CreateMapMessage();
            foreach (KeyValuePair<string, object> kp in message.Properties)
            {
                mapMessage.Properties[kp.Key] = kp.Value;
            }

            foreach (KeyValuePair<string, object> kp in message.Body)
            {
                mapMessage.Body[kp.Key] = kp.Value;
            }

            _logger.DebugFormat("开始向 {0} 发送消息", _nm);
            try
            {
                if (isDefault)
                {
                    producer.Send(mapMessage);
                }
                else if (isFast)
                {
                    DateTime old = DateTime.Now;
                    producer.Send(mapMessage);
                    isFast = ((DateTime.Now - old) < TimeSpan.FromSeconds(1));
                    if (!isFast)
                        _logger.DebugFormat("向 {0} 发送消息发现耗时太长, 决定以后跳过!", _nm);
                }
                else
                {
                    throw new ApplicationException("发送消息耗时太长, 决定跳过");
                }

            }
            catch (Exception e)
            {
                if (_logger.IsDebugEnabled)
                    _logger.Debug(string.Format("向 {0} 发送消息发生错误", _nm), e);
                throw;
            }
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (null != connection)
                {
                    //connection.Stop();
                    connection.Close();
                    connection.Dispose();
                    connection = null;
                }
                if (null != session)
                {
                    session.Close();
                    session.Dispose();
                    session = null;
                }
                if (null != modelProducer)
                {
                    modelProducer.Close();
                    modelProducer.Dispose();
                    modelProducer = null;
                }
                if (null != alertProducer)
                {
                    alertProducer.Close();
                    alertProducer.Dispose();
                    alertProducer = null;
                }
                if (null != performanceProducer)
                {
                    performanceProducer.Close();
                    performanceProducer.Dispose();
                    performanceProducer = null;
                }
                if (null != deskTerminalProducer)
                {
                    deskTerminalProducer.Close();
                    deskTerminalProducer.Dispose();
                    deskTerminalProducer = null;
                }
            }
        }
    }


    public class Session : IDisposable
    {
        InternalSession _default;
        List<InternalSession> _sessions = new List<InternalSession>();
        List<Error> _laseErrors = new List<Error>();

        public class Error
        {
            public Exception exception;
            public string Name;

            public Error(string nm, Exception e)
            {
                this.exception = e;
                this.Name = nm;
            }
        }

        public Session(InternalSession session)
        {
            _default = session;
            _default.setDefault();
        }

        public void Add(InternalSession session)
        {
            _sessions.Add(session);
        }

        public void Send(Destination destination, string message)
        {
            _default.Send(destination, message);

            _laseErrors.Clear();

            foreach (InternalSession session in _sessions)
            {
                try
                {
                    session.Send(destination, message);
                }
                catch (Exception e)
                {
                    _laseErrors.Add(new Error(session.Name, e));
                }
            }
        }
        public void Send(Destination destination, MapMessage message)
        {
            _default.Send(destination, message);

            _laseErrors.Clear();

            foreach (InternalSession session in _sessions)
            {
                try
                {
                    session.Send(destination, message);
                }
                catch (Exception e)
                {
                    _laseErrors.Add(new Error(session.Name, e));
                }
            }
        }

        public Error[] GetLastErrors()
        {
            return _laseErrors.ToArray();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (null != _default)
                {
                    _default.Dispose();
                    _default = null;
                }

                foreach (InternalSession session in _sessions)
                {
                    session.Dispose();
                }
                _sessions.Clear();
            }
        }
    }


    /// <summary>
    /// 处理配置文件，并建立连接
    /// </summary>
    public class NMSSupport
    {
        private static object[] GetFactoryParams(XmlSetting paramEntry)
        {
            if (null == paramEntry)
                return null;

            ArrayList factoryParams = new ArrayList();
            foreach (XmlSetting paramNode in paramEntry.Select("param"))
            {
                string paramType = paramNode.ReadSetting("@type");
                string paramValue = paramNode.ReadSetting("@value");

                switch (paramType)
                {
                    case "string":
                        factoryParams.Add(paramValue);
                        break;

                    case "int":
                        factoryParams.Add(int.Parse(paramValue));
                        break;
                }
            }

            if (factoryParams.Count > 0)
                return factoryParams.ToArray();

            return null;
        }


        public static Session Create(XmlSetting xmlSetting)
        {
            InternalSession defaultSession = CreateInternalSession(xmlSetting.SelectOne("/configuration/MQs/DefaultMQ"));

            Session session = new Session(defaultSession);

            XmlSetting[] mqSettings = xmlSetting.Select("/configuration/MQs/MQ");
            if (null == mqSettings || 0 == mqSettings.Length)
                return session;

            foreach (XmlSetting mqSetting in mqSettings)
            {
                session.Add(CreateInternalSession(mqSetting));

            }
            return session;
        }

        public static InternalSession CreateInternalSession(XmlSetting entry)
        {
            string nm = entry.ReadSetting("@Name", "");
            string brokerUri = entry.ReadSetting("@value");
            object[] factoryParams = GetFactoryParams(entry.SelectOne("FactoryParams"));

            string clientId = entry.ReadSetting("ClientId/@value", null);// GetNodeValueAttribute(uriNode, "clientId", "NMSTestClientId");
            string userName = entry.ReadSetting("UserName/@value"); //GetNodeValueAttribute(uriNode, "userName", "guest");
            string passWord = entry.ReadSetting("PassWord/@value"); //GetNodeValueAttribute(uriNode, "passWord", "guest");
            //string queueName = entry.ReadSetting("QueueName/@value"); //GetNodeValueAttribute(uriNode, "QueueName", "QueueName");
            int timeout = 0;
            if (!int.TryParse(entry.ReadSetting("Timeout/@value", null), out timeout))
                timeout = 5;

            try
            {

                Apache.NMS.NMSConnectionFactory nmsFactory = (null == factoryParams) ?
                    new Apache.NMS.NMSConnectionFactory(brokerUri)
                    : new Apache.NMS.NMSConnectionFactory(brokerUri, factoryParams);

                IConnection newConnection = nmsFactory.ConnectionFactory.CreateConnection(userName, passWord);
                if (newConnection == null)
                    throw new ApplicationException(string.Format("创建到 {0} 的连接失败!", brokerUri));

                newConnection.RequestTimeout = TimeSpan.FromSeconds(timeout);
                if (!string.IsNullOrEmpty(clientId))
                    newConnection.ClientId = clientId;


                return new InternalSession(nm, newConnection
                    , entry.ReadSetting("modelDestinationName/@value")
                    , entry.ReadSetting("alertDestinationName/@value")
                    , entry.ReadSetting("perfDestinationName/@value")
                    , entry.ReadSetting("deskDestinationName/@value"));
            }
            catch (Exception e)
            {
                return new InternalSession(nm, new ApplicationException(string.Format("创建到 {0} 的连接失败!", brokerUri), e));
            }
        }



    }
}
