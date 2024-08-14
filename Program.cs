using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gRPCClientBasedDotnetFramework
{
	internal class Program
	{
		#region 요청마다 새로운 채널 생성 및 작업 끝나면 채널 닫기
		/**
		 * static async Task Main(string[] args)
		 * {
		 *	Console.WriteLine("If you want to exit this console, input 'y' or 'Y' and press enter.");
		 *	string isExit = Console.ReadLine();
		 *	
		 *	while (isExit != "y" && isExit != "Y")
		 *	{
		 *		// 요청마다 새로운 채널을 생성합니다.
		 *		using (var channel = new Channel("localhost:50051", ChannelCredentials.Insecure))
		 *		{
		 *				var client = new Greeter.GreeterClient(channel);
		 *			
		 *				// 서버로 메시지를 전송하고 응답을 받습니다.
		 *				var reply = await client.SayHelloAsync(new HelloRequest { Name = "World" });
		 *			
		 *				// 응답 메시지를 출력합니다.
		 *				Console.WriteLine("Greeting: " + reply.Message);
		 *			
		 *				// 채널을 닫습니다.
		 *				await channel.ShutdownAsync();
		 *			}
		 *			
		 *			Console.WriteLine("If you want to exit this console, input 'y' or 'Y' and press enter.");
		 *			isExit = Console.ReadLine();
		 *		}
		 *		
		 *		Console.WriteLine("Service terminated, press any key to quit this console.");
		 *		Console.ReadKey();
		 *	}
		 */
		#endregion

		static async Task Main (string[] args)
		{
			// 서버 주소와 포트를 설정합니다.
			var channel = new Channel("localhost:50051", ChannelCredentials.Insecure);

			// GreeterClient 객체를 생성합니다.
			var client = new Greeter.GreeterClient(channel);

			Console.WriteLine("If u want exit this console, input \"y\" or \"Y\" and press enter.");
			string isExit = Console.ReadLine();

			while (isExit != "y" && isExit != "Y")
			{
				// 서버로 메시지를 전송하고 응답을 받습니다.
				var reply = await client.SayHelloAsync(new HelloRequest { Name = "World" });

				// 응답 메시지를 출력합니다.
				Console.WriteLine("Greeting: " + reply.Message);

				Console.WriteLine("If u want exit this console, input \"y\" or \"Y\" and press enter.");
				isExit = Console.ReadLine();
			}

			Console.WriteLine("service teminated, press any key to quit this console.");
			Console.ReadLine();

			// 채널을 닫습니다.
			await channel.ShutdownAsync();
		}
	}
}
