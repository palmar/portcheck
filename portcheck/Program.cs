using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace portcheck
{
	class portcheck
	{
		static void Main(string[] args)
		{
			try
			{
				if (args.Length == 2)
				{
					int port = 0;
					string host = args[0];
					try
					{
						port = Convert.ToInt32(args[1]);
					}
					catch (FormatException)
					{
						Console.WriteLine("");
						Console.WriteLine("Could not parse port number.");
						Console.WriteLine("");
						Environment.Exit(2);
					}

					System.Net.Sockets.TcpClient tcpclient = new System.Net.Sockets.TcpClient();
					IAsyncResult connection = tcpclient.BeginConnect(host, port, null, null);
					System.Threading.WaitHandle waithandle = connection.AsyncWaitHandle;
					tcpclient.SendTimeout = 3000;

					try
					{
						if (!connection.AsyncWaitHandle.WaitOne(tcpclient.SendTimeout, false))
						{
							tcpclient.Close();
							throw new TimeoutException();
						}

						tcpclient.EndConnect(connection);
						Console.WriteLine("");
						Console.WriteLine("Successfully connected to server.");
						Console.WriteLine("");
						Environment.Exit(1);
					}
					catch (TimeoutException)
					{
						Console.WriteLine("");
						Console.WriteLine("The connection attempt timed out.");
						Console.WriteLine("");
						Environment.Exit(2);
					}
					catch (SocketException)
					{
						Console.WriteLine("");
						Console.WriteLine("Connection actively refused.");
						Console.WriteLine("");
					}
					finally
					{
						waithandle.Close();
					}

				}
				else
				{
					throw new ArgumentOutOfRangeException();
				}
			}
			catch (ArgumentOutOfRangeException)
			{
				Console.WriteLine("");
				Console.WriteLine("Invalid arguments");
				Console.WriteLine("");
				Environment.Exit(2);
			}
		}
	}
}