using System;
using System.Collections.Generic;
using System.Text;

namespace meijing
{
    public interface IRunnable
    {
        void Run();
    }

    //public class PVInfoTask : IRunnable
    //{
    //    private static log4net.ILog _logger = log4net.LogManager.GetLogger("Betanetworks.Task");
    //    IDictionary<int, Betanetworks.Nanrui.Model.Sender.PVInfo[]> _pvInfos;
    //    int _devId;

    //    public PVInfoTask(IDictionary<int, Betanetworks.Nanrui.Model.Sender.PVInfo[]> pvInfos
    //        , int devId)
    //    {
    //        _pvInfos = pvInfos;
    //        _devId = devId;
    //    }

    //    public void Run()
    //    {
    //        _logger.InfoFormat("尝试取设备{0}的 PVInfo!", _devId);

    //        Betanetworks.Nanrui.Model.Sender.PVInfo[] pvInfos = Betanetworks.Nanrui.Model.Sender.Program.GetPVInfo(_devId);
    //        if (null != pvInfos && 0 != pvInfos.Length)
    //        {
    //            _logger.DebugFormat("设备{0}取到{1}个 PVInfo 信息", _devId, pvInfos.Length);
    //            _pvInfos[_devId] = pvInfos;
    //        }
    //    }
    //}

    //public class OSDiskUtilTask : IRunnable
    //{
    //    private static log4net.ILog _logger = log4net.LogManager.GetLogger("Betanetworks.Task");
    //    IDictionary<int, Betanetworks.Nanrui.Model.Sender.OSDiskUtil[]> _diskUtils;
    //    int _devId;

    //    public OSDiskUtilTask(IDictionary<int, Betanetworks.Nanrui.Model.Sender.OSDiskUtil[]> diskUtils
    //        , int devId)
    //    {
    //        _diskUtils = diskUtils;
    //        _devId = devId;
    //    }

    //    public void Run()
    //    {
    //        _logger.InfoFormat("尝试取设备{0}的 OSDiskUtil!", _devId);

    //        Betanetworks.Nanrui.Model.Sender.OSDiskUtil[] diskUtils = Betanetworks.Nanrui.Model.Sender.Program.GetOSDiskUtil(_devId);
    //        if (null != diskUtils && 0 != diskUtils.Length)
    //        {
    //            _logger.DebugFormat("设备{0}取到{1}个 OSDiskUtil 信息", _devId, diskUtils.Length);
    //            _diskUtils[_devId] = diskUtils;
    //        }
    //    }
    //}


    //public class TableSpaceTask : IRunnable
    //{
    //    private static log4net.ILog _logger = log4net.LogManager.GetLogger("Betanetworks.Task");
    //    IDictionary<int, Betanetworks.Nanrui.Model.Sender.TableSpace[]> _diskUtils;
    //    int _devId;
    //    int _devType;

    //    public TableSpaceTask(IDictionary<int, Betanetworks.Nanrui.Model.Sender.TableSpace[]> diskUtils
    //        , int devId, int devType)
    //    {
    //        _diskUtils = diskUtils;
    //        _devType = devType;
    //        _devId = devId;
    //    }

    //    public void Run()
    //    {
    //        _logger.InfoFormat("尝试取设备{0}的 TableSpace!", _devId);

    //        Betanetworks.Nanrui.Model.Sender.TableSpace[] diskUtils = Betanetworks.Nanrui.Model.Sender.Program.GetTableSpace(_devId, _devType);
    //        if (null != diskUtils && 0 != diskUtils.Length)
    //        {
    //            _logger.DebugFormat("设备{0}取到{1}个 TableSpace 信息", _devId, diskUtils.Length);
    //            _diskUtils[_devId] = diskUtils;
    //        }
    //    }
    //}

    //public class VersionTask : IRunnable
    //{
    //    private static log4net.ILog _logger = log4net.LogManager.GetLogger("Betanetworks.Task");
    //    IDictionary<int, string> _diskUtils;
    //    int _devId;
    //    int _devType;

    //    public VersionTask(IDictionary<int, string> diskUtils
    //        , int devId, int devType)
    //    {
    //        _diskUtils = diskUtils;
    //        _devType = devType;
    //        _devId = devId;
    //    }

    //    public void Run()
    //    {
    //        _logger.InfoFormat("尝试取设备{0}的 Version!", _devId);

    //        string diskUtils = Betanetworks.Nanrui.Model.Sender.Program.GetVersion(_devId, _devType);
    //        if (null != diskUtils && 0 != diskUtils.Length)
    //        {
    //            _logger.DebugFormat("设备{0}取到 Version 信息", _devId);
    //            _diskUtils[_devId] = diskUtils;
    //        }
    //    }
    //}
    //public class CPUTask : IRunnable
    //{
    //    private static log4net.ILog _logger = log4net.LogManager.GetLogger("Betanetworks.Task");
    //    IDictionary<int, Betanetworks.Nanrui.Model.Sender.OSCPU[]> _oscpus;
    //    int _devId;

    //    public CPUTask(IDictionary<int, Betanetworks.Nanrui.Model.Sender.OSCPU[]> cpus
    //        , int devId)
    //    {
    //        _oscpus = cpus;
    //        _devId = devId;
    //    }

    //    public void Run()
    //    {
    //        _logger.InfoFormat("尝试取设备{0}的 CPU!", _devId);

    //        Betanetworks.Nanrui.Model.Sender.OSCPU[] cpus = Betanetworks.Nanrui.Model.Sender.Program.GetOSCPU(_devId);
    //        if (null != cpus && 0 != cpus.Length)
    //        {
    //            _logger.DebugFormat("设备{0}取到{1}个 CPU 信息", _devId, cpus.Length);
    //            _oscpus[_devId] = cpus;
    //        }
    //    }
    //}

    //public class FileSystemTask : IRunnable
    //{
    //    private static log4net.ILog _logger = log4net.LogManager.GetLogger("Betanetworks.Task");
    //    IDictionary<int, Betanetworks.Nanrui.Model.Sender.OSFileSystem[]> _fileSystems;
    //    int _devId;

    //    public FileSystemTask(IDictionary<int, Betanetworks.Nanrui.Model.Sender.OSFileSystem[]> dict
    //        , int devId)
    //    {
    //        _fileSystems = dict;
    //        _devId = devId;
    //    }

    //    public void Run()
    //    {
    //        _logger.InfoFormat("尝试取设备{0}的 FileSystem!", _devId);

    //        Betanetworks.Nanrui.Model.Sender.OSFileSystem[] fs = Betanetworks.Nanrui.Model.Sender.Program.GetFileSystem(_devId);
    //        if (null != fs && 0 != fs.Length)
    //        {
    //            _logger.DebugFormat("设备{0}取到{1}个文件系统", _devId, fs.Length);
    //            _fileSystems[_devId] = fs;
    //        }
    //    }
    //}

    public class TaskPool : IDisposable
    {
        private static log4net.ILog _logger = log4net.LogManager.GetLogger("Betanetworks.Task");
        bool _isStop;
        TimeSpan _pollInterval = TimeSpan.FromSeconds(1);
        object _lockObj = new object();
        Queue<IRunnable> _runnableList = new Queue<IRunnable>();
        List<System.Threading.Thread> _threads = new List<System.Threading.Thread>();

        public TaskPool(string nm, int threads, int pollInterval)
        {
            _isStop = false;
            _pollInterval = TimeSpan.FromSeconds(pollInterval);

            for (int i = 0; i < threads; ++i)
            {
                System.Threading.Thread thread = new System.Threading.Thread( this.Run );
                thread.IsBackground = true;
                thread.Name = nm;
                thread.Start();
                _threads.Add(thread);
            }
        }

        IRunnable Get()
        {
            lock (_lockObj)
            {
                if (0 == _runnableList.Count)
                    return null;

                return _runnableList.Dequeue();
            }
        }

        public void Send(IRunnable runnable)
        {
            lock (_lockObj)
            {
                _runnableList.Enqueue(runnable);
            }
        }

        void RunOnce()
        {
            IRunnable runnable = null;
            while (null != (runnable = Get()))
            {
                runnable.Run();
            }
        }

        void Run()
        {
            try
            {
                while (!_isStop)
                {
                    RunOnce();
                    System.Threading.Thread.Sleep(_pollInterval);
                    RunOnce();
                }

                _logger.InfoFormat("线程{0}退出！", System.Threading.Thread.CurrentThread.Name);
            }
            catch( Exception e)
            {
                _logger.Error(e);
            }
        }

        public void Dispose()
        {
            _isStop = true;

            foreach (System.Threading.Thread thread in _threads)
            {
                thread.Join();
            }
        }
    }
}
