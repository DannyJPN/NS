using BackPropag;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace BasicDriverNET
{
    class Program
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("shell32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsUserAnAdmin();

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;
        private static RaceConnector raceConnector;
        private static string[] controls = new string[] { "Left", "Right", "Up", "Down", "Space" };
        private static float change = 0.05f;
        private static int learniters = 150000;
        static void Main(string[] args)
        {
            Thread KeyThread = new Thread(ThreadTask);
            String host = "localhost";
            String filename = @"Carnet.xml";
            int port = 9460;
            String raceName = "Zavod";
            String driverName = "KRU0142";
            String carType = null;
            raceConnector = null;
            
            if (args.Length < 4)
            {
                // kontrola argumentu programu
                raceConnector = new RaceConnector(host, port, null);
                Console.WriteLine("argumenty: server port nazev_zavodu jmeno_ridice [typ_auta]");
                List<String> raceList = raceConnector.listRaces();
                raceName = raceList[new Random().Next(raceList.Count)];
                List<String> carList = raceConnector.listCars(raceName);
                carType = carList[new Random().Next(carList.Count)];
                driverName += "_" + carType;
                //			host = JOptionPane.showInputDialog("Host:", host);
                //			port = Integer.parseInt(JOptionPane.showInputDialog("Port:", Integer.toString(port)));
                //			raceName = JOptionPane.showInputDialog("Race name:", raceName);
                //			driverName = JOptionPane.showInputDialog("Driver name:", driverName);
            }
            else
            {
                // nacteni parametu
                host = args[0];
                Console.WriteLine("Arg0 = {0}", args[0]);
                port = int.Parse(args[1]);
                Console.WriteLine("Arg1 = {0}", args[1]);
                raceName = args[2];
                Console.WriteLine("Arg2 = {0}", args[2]);
                driverName = args[3];
                Console.WriteLine("Arg3 = {0}", args[3]);
                if (args.Length > 4)
                {
                    carType = args[4];
                    Console.WriteLine("Arg4 = {0}", args[4]);
                }
                raceConnector = new RaceConnector(host, port, null);

            }
            // vytvoreni klienta




            raceConnector.bp = new BackPropagationTask();
            raceConnector.bp.LoadFromXML(filename);
            List<double[]> points = new List<double[]>();
            foreach (TrainSetElement tse in raceConnector.bp.MultiLayerNetwork.TrainSet)
            {
                points.Add(tse.Inputs);
            }
            List<List<double>> expect = new List<List<double>>();
            foreach (TrainSetElement tse in raceConnector.bp.MultiLayerNetwork.TrainSet)
            {
                expect.Add(tse.Outputs.ToList());
            }

            raceConnector.bp.MultiLayerNetwork.Train(points, expect, learniters);
            raceConnector.bp.StoreToXML(filename);
            raceConnector.setDriver(new SimpleDriver());
            KeyThread.Start();
            raceConnector.start(raceName, driverName, carType);

            KeyThread.Join();
            //UnhookWindowsHookEx(_hookID);
        }

        private static void ThreadTask()
        {
            IntPtr h = Process.GetCurrentProcess().MainWindowHandle;
            _hookID = SetHook(_proc);

            Console.WriteLine("Keys Listening");
            Application.Run();
            UnhookWindowsHookEx(_hookID);
        }
        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                //Console.WriteLine(curProcess.MainModule);
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0); //Getmodule-návrat na sebe ??
            }
        }



        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {

            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                string key = String.Format("{0}", (Keys)vkCode);
                //Console.WriteLine("{0}", key);
                if (controls.Contains(key))
                {
                    Console.WriteLine("{0}", key);
                    List<double> inputs = new List<double>();
                    List<double> outputs = new List<double>();



                    try
                    {
                        switch (key)
                        {
                            case "Up":
                                {



                                    
                                    //if (raceConnector.Values["speed"] <= 0.51) { raceConnector.Responses["acc"] = 0.7f; }
                                    raceConnector.Responses["acc"] =0.8f;
                                    //if (raceConnector.Responses["acc"] >= 1) { raceConnector.Responses["acc"] = 1; }
                                    raceConnector.Responses["wheel"] = 0.5f;
                                    break;
                                }
                            case "Right":
                                {

                                   // if (raceConnector.Responses["wheel"] >= 1) { raceConnector.Responses["wheel"] = 1; }
                                   // if (raceConnector.Responses["wheel"] < 0.5) { raceConnector.Responses["wheel"] = 0.55f; }
                                    raceConnector.Responses["wheel"] =0.75f;

                                    if (raceConnector.Values["speed"] < 0.5)
                                    {
                                        raceConnector.Responses["acc"] = 0.4f;
                                    }
                                    else if (raceConnector.Values["speed"] > 0.5)
                                    {
                                        raceConnector.Responses["acc"] =0.6f;
                                    }
                                    //if (raceConnector.Responses["acc"] <= 0) { raceConnector.Responses["acc"] = 0; }
                                    //if (raceConnector.Responses["acc"] >= 1) { raceConnector.Responses["acc"] = 1; }



                                    break;
                                }
                            case "Left":
                                {

                                    //if (raceConnector.Responses["wheel"] <= 0) { raceConnector.Responses["wheel"] = 0; }
                                    //if (raceConnector.Responses["wheel"] > 0.5) { raceConnector.Responses["wheel"] = 0.45f; }
                                    raceConnector.Responses["wheel"] = 0.25f;
                                    if (raceConnector.Values["speed"] < 0.5)
                                    {
                                        raceConnector.Responses["acc"] = 0.4f;
                                    }
                                    else if (raceConnector.Values["speed"] > 0.5)
                                    {
                                        raceConnector.Responses["acc"] = 0.6f;
                                    }
                                    //if (raceConnector.Responses["acc"] <= 0) { raceConnector.Responses["acc"] = 0; }
                                    //if (raceConnector.Responses["acc"] >= 1) { raceConnector.Responses["acc"] = 1; }
                                    break;
                                }
                            case "Down":
                                {

                                    
                                   // if (raceConnector.Responses["speed"] >= 0.51) { raceConnector.Responses["acc"] = 0.45f; }
                                    raceConnector.Responses["acc"] =0.2f;
                                    //if (raceConnector.Responses["acc"] <= 0) { raceConnector.Responses["acc"] = 0; }
                                    raceConnector.Responses["wheel"] = 0.5f;


                                    break;
                                }
                            case "Space":
                                {
                                    if (raceConnector.Values["speed"] < 0.5)
                                    {
                                        raceConnector.Responses["acc"] =0.45f;
                                    }
                                    else if (raceConnector.Values["speed"] > 0.5)
                                    {
                                        raceConnector.Responses["acc"] =0.55f;
                                    }

                                    break;
                                }

                        }
                    }
                    catch (Exception)
                    {
                        raceConnector.bp.StoreToXML("Carnet.xml");

                    }


                    Dictionary<string, float> LocalValues = new Dictionary<string, float>(raceConnector.Values);
                    Dictionary<string, float> LocalResponses = new Dictionary<string, float>(raceConnector.Responses);

                    foreach (KeyValuePair<string, float> kvp in LocalValues)
                    {
                        Console.WriteLine("{0}={1}", kvp.Key, kvp.Value);
                        inputs.Add(kvp.Value);
                    }
                    foreach (KeyValuePair<string, float> kvp in LocalResponses)
                    {
                        Console.WriteLine("{0}={1}", kvp.Key, kvp.Value);
                        outputs.Add(kvp.Value);
                    }




                    TrainSetElement newts = new TrainSetElement(inputs.ToArray(), outputs.ToArray());

                    raceConnector.bp.MultiLayerNetwork.TrainSet.Add(newts);




                }
                else if (key == "S")
                {
                    Console.WriteLine("Save");
                    raceConnector.bp.StoreToXML("Carnet.xml");
                }
                else if (key == "L" || key=="R")
                {
                    Console.WriteLine("Learn");

                    List<double[]> points = new List<double[]>();
                    foreach (TrainSetElement tse in raceConnector.bp.MultiLayerNetwork.TrainSet)
                    {
                        points.Add(tse.Inputs);
                    }
                    List<List<double>> expect = new List<List<double>>();
                    foreach (TrainSetElement tse in raceConnector.bp.MultiLayerNetwork.TrainSet)
                    {
                        expect.Add(tse.Outputs.ToList());
                    }
                    int iters = 1;
                    if (key == "R")
                    { iters = learniters <100000?learniters:learniters/10;Console.WriteLine("ITERS: {0}", iters); }
                    raceConnector.bp.MultiLayerNetwork.Train(points, expect, iters);

                }

                else
                {
                    //Console.WriteLine("BAD: {0}", key);
                }
                /*string dayepoch = DateTime.Now.ToShortDateString();
				
				string folderpath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/Consolidated/";
				if (!Directory.Exists(folderpath))
				{
					Directory.CreateDirectory(folderpath);
				}
				
				string logfile = String.Format("{1}/Trace_{0}_log.log", dayepoch, folderpath);
				
				WriteFile(logfile, key);
				*/






            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }
        private static void WriteFile(string logfile, string content)
        {
            using (StreamWriter sw = new StreamWriter(logfile, true))
            {
                sw.WriteLine("{0}", content);

            }
        }
    }
}
