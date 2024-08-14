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
		 *		var channel = new Channel("localhost:50051", ChannelCredentials.Insecure))
		 *		
		 *		var client = new Greeter.GreeterClient(channel);
		 *			
		 *		// 서버로 메시지를 전송하고 응답을 받습니다.
		 *		var reply = await client.SayHelloAsync(new HelloRequest { Name = "World" });
		 *			
		 *		// 응답 메시지를 출력합니다.
		 *		Console.WriteLine("Greeting: " + reply.Message);
		 *			
		 *		// 채널을 닫습니다.
		 *		await channel.ShutdownAsync();
		 *	
		 *			
		 *		Console.WriteLine("If you want to exit this console, input 'y' or 'Y' and press enter.");
		 *		isExit = Console.ReadLine();
		 *	}
		 *		
		 *		Console.WriteLine("Service terminated, press any key to quit this console.");
		 *		Console.ReadKey();
		 * }
		 */
		#endregion

		#region 단방향, 채널을 새로 만들지 않고 반복
		/**
		 * static async Task Main (string[] args)
		 * {
		 *	// 서버 주소와 포트를 설정합니다.
		 *	var channel = new Channel("localhost:50051", ChannelCredentials.Insecure);
		 *	// GreeterClient 객체를 생성합니다.
		 *	var client = new Greeter.GreeterClient(channel);
		 *	
		 *	Console.WriteLine("If u want exit this console, input \"y\" or \"Y\" and press enter.");
		 *	string isExit = Console.ReadLine();
		 *	
		 *		while (isExit != "y" && isExit != "Y")
		 *		{
		 *			// 서버로 메시지를 전송하고 응답을 받습니다.
		 *			var reply = await client.SayHelloAsync(new HelloRequest { Name = "World" });
		 *		
		 *			// 응답 메시지를 출력합니다.
		 *			Console.WriteLine("Greeting: " + reply.Message);
		 *		
		 *			Console.WriteLine("If u want exit this console, input \"y\" or \"Y\" and press enter.");
		 *			isExit = Console.ReadLine();
		 *		}
		 *	
		 *		Console.WriteLine("service teminated, press any key to quit this console.");
		 *		Console.ReadLine();
		 *	
		 *		// 채널을 닫습니다.
		 *		await channel.ShutdownAsync();
		 *	}
		 */
		#endregion

		#region 서버 측 스트리밍
		/**
		 * static async Task Main (string[] args)
		 * {
		 *	Console.WriteLine("If you want to exit this console, input 'y' or 'Y' and press enter.");
		 *	string isExit = Console.ReadLine();
		 *	
		 *	while (isExit != "y" && isExit != "Y")
		 *	{
		 *		// 요청마다 새로운 채널을 생성합니다.
		 *		var channel = new Channel("localhost:50051", ChannelCredentials.Insecure);
		 *		
		 *		var client = new Greeter.GreeterClient(channel);
		 *		
		 *		// 클라이언트에서 SayHelloStream rpc 실행, HelloRequest를 받음
		 *		// call은 서버 측 스트리밍을 뜻하는 데이터
		 *		using (var call = client.SayHelloStream(new HelloRequest { Name = "World" }))
		 *		{
		 *			// 서버 측 스트리밍의 응답을 보고 MoveNext()를 할 수 없을 때까지, 그러니까 모든 응답을 완료할 때까지
		 *			while (await call.ResponseStream.MoveNext())
		 *			{
		 *				// 메시지는 현재 응답 스트림의 현재
		 *				var message = call.ResponseStream.Current;
		 *				// 현재 메시지를 출력
		 *				Console.WriteLine("Greeting: " + message.Message);
		 *			}
		 *		}
		 *		
		 *		await channel.ShutdownAsync();
		 *		
		 *		Console.WriteLine("If you want to exit this console, input 'y' or 'Y' and press enter.");
		 *		isExit = Console.ReadLine();
		 *	}
		 *	Console.WriteLine("Service terminated, press any key to quit this console.");
		 *	Console.ReadKey();
		 * }
		 */
		#endregion

		#region 클라이언트 측 스트리밍 메서드
		static async Task Main (string[] args)
		{
			Console.WriteLine("If you want to exit this console, input 'y' or 'Y' and press enter.");
			string isExit = Console.ReadLine();

			while (isExit != "y" && isExit != "Y")
			{
				// 요청마다 새로운 채널을 생성합니다.
				var channel = new Channel("localhost:50051", ChannelCredentials.Insecure);

				var client = new Greeter.GreeterClient(channel);

				// 클라이언트에서 SayHelloClientStream rpc 실행, 당장은 인수가 없다
				// call은 클라이언트 측 스트리밍을 뜻하는 데이터
				using (var call = client.SayHelloClientStream())
				{
					// 반복을 통해서
					for (int i = 0; i < 5; i++)
					{
						// call의 요청 스트리밍에 접근해서 비동기로 작성한다, 이때 Hello Request를 반영
						await call.RequestStream.WriteAsync(new HelloRequest { Name = $"World {i + 1}" });
						await Task.Delay(1000); // 1초 대기
					}

					// 완료되면 call의 요청 스트림에 접근해서 비동기로 완료처리
					await call.RequestStream.CompleteAsync();

					// call이 다시 비동기로 응답 받아온다
					var reply = await call.ResponseAsync;

					// 응답을 출력
					Console.WriteLine("Greeting: " + reply.Message);
				}

				await channel.ShutdownAsync();

				Console.WriteLine("If you want to exit this console, input 'y' or 'Y' and press enter.");
				isExit = Console.ReadLine();
			}

			Console.WriteLine("Service terminated, press any key to quit this console.");
			Console.ReadKey();
		}
		#endregion
	}
}
