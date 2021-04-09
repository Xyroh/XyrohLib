using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace com.xyroh.lib.Services
{
	public static class Zendesk
	{
		private static HttpClient client;
		private static readonly string apiPath = "/api/v2/requests.json";
		private static string url;

		public static Config Config { get; set; }

		public static void SetConfig()
		{
			if (Config.CanUseHelpDesk)
			{
				client = new HttpClient();
				url = Config.HelpDeskUrl + apiPath;

				Logger.Log("URL: " + url);
			}
		}

		public static async Task<int> CreateRequest(string email, string subject, string description, string[] tags)
		{

			var ticket = new ZendeskBaseRequest();

			if (Config.CanUseHelpDesk)
			{
				ticket.request = new ZendeskRequest();
				ticket.request.requester = new ZendeskRequester();
				ticket.request.requester.email = email;
				ticket.request.requester.name = email;
				ticket.request.comment = new ZendeskComment();
				ticket.request.comment.body = description;
				ticket.request.tags = tags;
				ticket.request.priority = "normal";
				ticket.request.subject = subject;

			}

			return await sendTicket(ticket);
		}

		/*public static async Task<int> CreateRequestWithAttachment(string email, string subject, string description, string[] tags, List<string> attachments)
		{
			var ticket = new ZendeskTicket();

			if (Config.CanUseHelpDesk)
			{
				ticket = new ZendeskTicket();
				ticket.email = email;
				ticket.subject = subject;
				ticket.description = description;
				ticket.tags = tags;
				ticket.attachments = attachments;
			}

			return await sendTicket(ticket);
		}*/


		private static async Task<int> sendTicket(ZendeskBaseRequest request)
		{
			int ticketId = 0;

			try
			{
				client.Timeout = TimeSpan.FromSeconds(15);

				var json = JsonConvert.SerializeObject(request);

				HttpResponseMessage response;

				// Got attachments?
				/*if (request.attachments != null && request.attachments.Count > 0)
				{

					using (var formData = new MultipartFormDataContent())
					{
						formData.Add(new StringContent(request.description), String.Format("\"description\""));
						formData.Add(new StringContent(request.email), String.Format("\"email\""));
						formData.Add(new StringContent(request.subject), String.Format("\"subject\""));
						formData.Add(new StringContent(request.priority.ToString()), String.Format("\"priority\""));
						formData.Add(new StringContent(request.source.ToString()), String.Format("\"source\""));
						formData.Add(new StringContent(request.status.ToString()), String.Format("\"status\""));

						foreach (var tag in request.tags)
						{
							formData.Add(new StringContent(tag), String.Format("\"tags[]\""));
						}

						foreach (var file in request.attachments)
						{
							//Console.WriteLine("FOUND FILE: " + file);

							var fileContent = new ByteArrayContent(File.ReadAllBytes(file));
							formData.Add(fileContent, String.Format("\"attachments[]\""), Path.GetFileName(file));

						}

						response = await client.PostAsync(url, formData);
					}
				}
				else
				{*/

					// Logger.Log("CONTENT: " + json.ToString());
					var content = new StringContent(json, Encoding.UTF8, "application/json");

					response = await client.PostAsync(url, content);
				//}


				//response.EnsureSuccessStatusCode();
				var responseBody = await response.Content.ReadAsStringAsync();
				Logger.Log("RESP: " + responseBody);

				if (response.IsSuccessStatusCode)
				{
					var newTicket = JsonConvert.DeserializeObject<ZendeskBaseRequest>(responseBody);
					ticketId = newTicket.request.id;
					Logger.Log("TICKET: " + ticketId);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("EX: " + ex.Message);
				Logger.LogException("FRESHDESK", ex);
			}

			return ticketId;
		}
	}


	// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
	public class ZendeskRequester
	{
		public string name { get; set; }
		public string email { get; set; }
	}

	public class ZendeskComment
	{
		public string body { get; set; }
	}

	public class ZendeskBaseRequest
	{
		public ZendeskRequest request { get; set; }
	}

	public class ZendeskRequest
	{
		public int id { get; set; }
		public ZendeskRequester requester { get; set; }
		public ZendeskComment comment { get; set; }
		public string[] tags { get; set; }

		public string[] uploads { get; set; }
		public string priority { get; set; }
		public string subject { get; set; }
	}

}
