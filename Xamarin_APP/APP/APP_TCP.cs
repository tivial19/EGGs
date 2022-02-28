using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace cpXamarin_APP
{
    public partial class czAPP
    {

        const int cvTCP_Port = 15000;
        const int cvUDP_Port = 15432;

        bool isReceive_Items = false;





        public void cfSTOP_Receive_DATA()
        {
            isReceive_Items = false;
        }

        public async Task<string> cfReceive_DATA_String()
        {
            byte[] cxData = await cfReceive_DATA_Bytes();
            if (cxData != null && cxData.Any()) return Encoding.UTF8.GetString(cxData);
            else return null; 
        }
        public async Task<byte[]> cfReceive_DATA_Bytes()
        {
            isReceive_Items = true;
            byte[] cxData = null;
            try
            {
                IPAddress cxIP_Server = await cfListener_UDP_CMD_Find_IPClient(cvUDP_Port, 0, () => !isReceive_Items);
                await cfSend_UDP_CMD_Find_IPClient(cvUDP_Port, cxIP_Server);
                cxData = await cfListener_TCP_Data(cvTCP_Port);
                //await cfListener_UDP_Items(cvUDP_Port);
            }
            catch (Exception cxE)
            {
                await cfMSG_OK(cxE.Message, "Ошибка при получении данных!");
            }
            finally
            {
                isReceive_Items = false;
            }
            return cxData;
        }




        public Task cfSend_TCP_DATA(string cxData)
        {
            return cfSend_TCP_DATA(Encoding.UTF8.GetBytes(cxData));
        }

        public async Task cfSend_TCP_DATA(byte[] cxData)
        {
            try
            {
                await cfSend_UDP_CMD_Find_IPClient(cvUDP_Port);
                IPAddress cxIP_Client = await cfListener_UDP_CMD_Find_IPClient(cvUDP_Port, 5000);
                await cfSend_TCP_Data(cvTCP_Port, cxIP_Client,cxData);
                //await cfSend_UDP_Items(cvUDP_Port, cxIP_Client);
            }
            catch (Exception cxE)
            {
                await cfMSG_OK(cxE.Message, "Ошибка при отправке данных!");
            }
        }








        private static async Task cfSend_UDP_CMD_Find_IPClient(int cxPort, IPAddress cxIPAddress = null)
        {
            IPAddress cxIPtoSend = cxIPAddress == null ? IPAddress.Broadcast : cxIPAddress;
            UdpClient udp = new UdpClient();

            udp.EnableBroadcast = true;
            IPEndPoint cxEP = new IPEndPoint(cxIPtoSend, cxPort);

            byte[] msg = BitConverter.GetBytes(cxPort);
            await udp.SendAsync(msg, msg.Length, cxEP);
            udp.Close();

            return;
        }

        private static async Task<IPAddress> cfListener_UDP_CMD_Find_IPClient(int cxPort, int cxStop_TimeOut = 0, Func<bool> cxStop = null)
        {
            UdpClient udp = new UdpClient(cxPort);

            if (cxStop_TimeOut > 0)
                _ = Task.Run(() =>
                {
                    Thread.Sleep(cxStop_TimeOut);
                    udp.Close();
                    //throw new Exception("Ответа нет. Проверьте режим получения на клиенте.");
                });

            if (cxStop != null)
                _ = Task.Run(() =>
                {
                    do
                    {
                        Thread.Sleep(200);
                    } while (!cxStop());

                    udp.Close();
                });

            UdpReceiveResult result = await udp.ReceiveAsync();

            int cxMSG = BitConverter.ToInt32(result.Buffer, 0);
            if (cxMSG == cxPort) return result.RemoteEndPoint.Address;

            udp.Close();
            return null;
        }
        


        private static async Task cfSend_TCP_Data(int cxPort, IPAddress cxIPAddress, byte[] cxdata)
        {
            TcpClient tcpClient = new TcpClient();
            await tcpClient.ConnectAsync(cxIPAddress, cxPort);
            //await tcpClient.ConnectAsync(cxIPAdr, 15000);
            NetworkStream cxNetworkStream = tcpClient.GetStream();

            await cxNetworkStream.WriteAsync(cxdata, 0, cxdata.Length);

            cxNetworkStream.Close();
            cxNetworkStream.Dispose();

            tcpClient.Close();
        }



        private static async Task<byte[]> cfListener_TCP_Data(int cxPort/*, IPAddress cxIPAddress*/)
        {
            TcpListener tcpListener = new TcpListener(IPAddress.Any, cxPort);
            //TcpListener tcpListener = new TcpListener(cxIPAddress, cxPort);
            try
            {

                tcpListener.Start();

                TcpClient tcpClient = await tcpListener.AcceptTcpClientAsync();
                NetworkStream cxNetworkStream = tcpClient.GetStream();

                byte[] cxClientData = new byte[250000];
                List<byte> cxDATA = new List<byte>();

                int c = 0;
                int cxRec = 0;
                int cxRecAll = 0;
                do
                {
                    cxRec = await cxNetworkStream.ReadAsync(cxClientData, 0, cxClientData.Length);
                    cxDATA.AddRange(cxClientData.Take(cxRec));
                    c++;
                    cxRecAll += cxRec;
                } while (cxRec > 0);


                //_Trace.cfTrace($"Получено {cxDATA.Count} байт{ Environment.NewLine}За {c} подхода!");

                return cxDATA.ToArray();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                tcpListener.Stop();
            }

        }











        #region Test





        //public void cfReceive_Items_SocketT()
        //{

        //    Task.Run(() =>
        //    {
        //        try
        //        {

        //            Socket cxListen = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //            cxListen.Bind(new IPEndPoint(IPAddress.Any, 15000));

        //            cxListen.Listen(100);

        //            Socket cxSocket = cxListen.Accept();

        //            byte[] cxClientData = new byte[250000];
        //            List<byte> cxDATA = new List<byte>();


        //            int c = 0;
        //            int cxRec = 0;
        //            int cxRecAll = 0;
        //            do
        //            {
        //                cxRec = cxSocket.Receive(cxClientData);
        //                if (cxRec == 0) break;
        //                cxDATA.AddRange(cxClientData.Take(cxRec));
        //                c++;
        //                cxRecAll += cxRec;
        //            } while (true);


        //            _APP.cfDispatcherInvoke(() => _APP.cfMSG_OK($"Отправлено ?{ Environment.NewLine}Получено {cxDATA.Count}{ Environment.NewLine}За {c} подхода!"));


        //            string cxMsg = Encoding.UTF8.GetString(cxDATA.ToArray());
        //            var Qi = _Json.cfDeserialize_Object_From_String<List<czItem_Place>>(cxMsg);

        //            _APP.cfDispatcherInvoke(() => _APP.cfMSG_OK($"Добавить {Qi.Count} элементов?{ Environment.NewLine}Текущее количество {Items.Count}"));

        //            cxListen.Close();

        //        }
        //        catch (Exception cxE)
        //        {
        //            _APP.cfDispatcherInvoke(() => _APP.cfMSG_OK(cxE.Message, "Ошибка при получении данных!"));
        //        }
        //        finally
        //        {
        //            //tcpClient.Close();
        //            isReceive_Items = false;

        //            //tcpListener.Stop();
        //        }



        //    });
        //}


        //public void cfSend_Items_SocketT()
        //{
        //    Task.Run(() =>
        //    {
        //        try
        //        {
        //            Socket cxSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //            cxSocket.Connect(new IPEndPoint(IPAddress.Parse("192.168.0.12"), 15000));
        //            byte[] msg_items = Encoding.UTF8.GetBytes(_Json.cfSerialize_ObjectToString(Items));
        //            cxSocket.Send(msg_items);
        //            cxSocket.Close();
        //        }
        //        catch (Exception cxE)
        //        {
        //            _APP.cfDispatcherInvoke(() => _APP.cfMSG_OK(cxE.Message, "Ошибка при отправке данных!"));
        //        }
        //    });
        //}





        //public  void cfReceive_Items_TCPClient()
        //{

        //    Task.Run(() =>
        //    {
        //        try
        //        {
        //            TcpListener tcpListener = new TcpListener(IPAddress.Any, 15000);
        //            tcpListener.Start();

        //            TcpClient tcpClient = tcpListener.AcceptTcpClient();
        //            NetworkStream cxNetworkStream = tcpClient.GetStream();

        //            byte[] cxClientData = new byte[250000];
        //            List<byte> cxDATA = new List<byte>();

        //            int c = 0;
        //            int cxRec = 0;
        //            int cxRecAll = 0;
        //            do
        //            {
        //                cxRec = cxNetworkStream.Read(cxClientData, 0, cxClientData.Length);
        //                cxDATA.AddRange(cxClientData.Take(cxRec));
        //                c++;
        //                cxRecAll += cxRec;
        //            } while (cxRec > 0);


        //            _APP.cfDispatcherInvoke(() => _APP.cfMSG_OK($"Отправлено ?{ Environment.NewLine}Получено {cxDATA.Count}{ Environment.NewLine}За {c} подхода!"));


        //            string cxMsg = Encoding.UTF8.GetString(cxDATA.ToArray());
        //            var Qi = _Json.cfDeserialize_Object_From_String<List<czItem_Place>>(cxMsg);

        //            _APP.cfDispatcherInvoke(() => _APP.cfMSG_OK($"Добавить {Qi.Count} элементов?{ Environment.NewLine}Текущее количество {Items.Count}"));

        //            tcpListener.Stop();
        //        }
        //        catch (Exception cxE)
        //    {
        //            _APP.cfDispatcherInvoke(() => _APP.cfMSG_OK(cxE.Message, "Ошибка при получении данных!"));
        //        }
        //    finally
        //    {
        //        //tcpClient.Close();
        //        isReceive_Items = false;

        //            //tcpListener.Stop();
        //        }



        //});
        //}

        //public void cfSend_Items_TCPClient()
        //{

        //    Task.Run(() =>
        //    {

        //        try
        //        {

        //            TcpClient tcpClient = new TcpClient();
        //            IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse("192.168.0.12"), 15000);
        //            tcpClient.Connect(serverEndPoint);
        //            NetworkStream cxNetworkStream = tcpClient.GetStream();


        //            byte[] dataToSend = Encoding.UTF8.GetBytes(_Json.cfSerialize_ObjectToString(Items));

        //            cxNetworkStream.Write(dataToSend, 0, dataToSend.Length);


        //            cxNetworkStream.Close();
        //            cxNetworkStream.Dispose();

        //            tcpClient.Close();

        //        }
        //        catch (Exception cxE)
        //        {
        //            _APP.cfDispatcherInvoke(() => _APP.cfMSG_OK(cxE.Message, "Ошибка при отправке данных!"));
        //        }

        //    });
        //}




        //private async void cfReceive_ItemsU()
        //{
        //    const string cxTitle = "Ошибка при получении данных!";
        //    try
        //    {
        //        UdpClient udp = new UdpClient(15432);
        //        List<byte> cxDATA = new List<byte>();
        //        do
        //        {
        //            UdpReceiveResult result = await udp.ReceiveAsync();

        //            cxDATA.AddRange(result.Buffer);

        //            var x = cxDATA.Count();
        //            //if (result.Buffer.Length == 6)
        //            //{
        //            //    string cxScmd = Encoding.UTF8.GetString(result.Buffer);

        //            //    if(cxScmd == "cmdRun") cxItems_bytes = new List<byte>();
        //            //    if (cxScmd == "cmdEnd")
        //            //    {
        //            //        byte[] cxCountB= cxItems_bytes.Take(10).ToArray();
        //            //        string cxCountS = Encoding.UTF8.GetString(cxCountB);
        //            //        int cxC = 0;
        //            //        if (int.TryParse(cxCountS, out cxC))
        //            //        {
        //            //            byte[] cxItems_B = cxItems_bytes.Skip(10).ToArray();
        //            //            string cxItems_S = Encoding.UTF8.GetString(cxItems_B);
        //            //            var Qi = _Json.cfDeserialize_Object_From_String<List<czItem_Place>>(cxItems_S);

        //            //            if (cxC == Qi.Count)
        //            //            {
        //            //                if (await _APP.cfMSG_YesNo($"Добавить {Qi.Count} элементов?{ Environment.NewLine}Текущее количество {Items.Count}", "Запрос на добавление данных!"))
        //            //                {
        //            //                    Items = Qi;
        //            //                    cfRefresh_View();
        //            //                    Filter.cfUpdate_Filter_Variants();
        //            //                }
        //            //            }
        //            //            else await _APP.cfMSG_OK($"Количество не сооответствует.{ Environment.NewLine}Отправлено {cxC}, а получено {Qi.Count}", cxTitle);
        //            //        }
        //            //        else await _APP.cfMSG_OK("Количество не определено.", cxTitle);

        //            //        isReceive_Items = false;

        //            //    }
        //            //}
        //            //else cxItems_bytes.AddRange(result.Buffer);

        //        } while (isReceive_Items);

        //        //udp.Close();
        //    }
        //    catch (Exception cxE)
        //    {
        //        isReceive_Items = false;
        //        await _APP.cfMSG_OK(cxE.Message, cxTitle);
        //    }
        //}




        //public async void cfSend_ItemsU()
        //{
        //    try
        //    {
        //        UdpClient udp = new UdpClient();
        //        udp.EnableBroadcast = true;

        //        IPEndPoint cxEP = new IPEndPoint(IPAddress.Broadcast, 15432);
        //        //IPEndPoint cxEP = new IPEndPoint(IPAddress.Parse("192.168.0.12"), 15432);

        //        //byte[] msg_run = Encoding.UTF8.GetBytes("cmdRun");
        //        byte[] msg_items = Encoding.UTF8.GetBytes(_Json.cfSerialize_ObjectToString(Items));
        //        //byte[] msg_end = Encoding.UTF8.GetBytes($"cmdEnd");

        //        //List<byte> msg_Count_L = new List<byte>();
        //        //msg_Count_L.AddRange(Encoding.UTF8.GetBytes(Items.Count.ToString()));
        //        //while (msg_Count_L.Count < 10) msg_Count_L.Add(0);



        //        //await udp.SendAsync(msg_run, msg_run.Length, cxEP);
        //        //await Task.Delay(100);
        //        //await udp.SendAsync(msg_Count_L.ToArray(), msg_Count_L.ToArray().Length, cxEP);
        //        //await Task.Delay(100);


        //        int cxTake_Count = 8000;
        //        int cxCurTake = 1;
        //        int cxSkip = 0;
        //        int cxSended = 0;

        //        do
        //        {
        //            cxSkip = (cxCurTake - 1) * cxTake_Count;
        //            var Qmsg = msg_items.Skip(cxSkip).Take(cxTake_Count).ToArray();

        //            await udp.SendAsync(Qmsg, Qmsg.Length, cxEP);
        //            await Task.Delay(10);

        //            cxSended = cxTake_Count * cxCurTake;
        //            cxCurTake++;
        //        } while (cxSended < msg_items.Length);


        //        //await udp.SendAsync(msg_end, msg_end.Length, cxEP);

        //        //await udp.SendAsync(msg_items, msg_items.Length, cxEP);
        //        udp.Close();
        //    }
        //    catch (Exception cxE)
        //    {
        //        await _APP.cfMSG_OK(cxE.Message, "Ошибка при отправке данных!");
        //    }
        //}

        #endregion







    }
}
