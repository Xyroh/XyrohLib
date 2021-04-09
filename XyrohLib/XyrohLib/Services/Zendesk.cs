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
		private static readonly string apiPathAttachments = "/api/v2/uploads";
		private static string urlAttachments;

		public static Config Config { get; set; }

		public static void SetConfig()
		{
			if (Config.CanUseHelpDesk)
			{
				client = new HttpClient();
				url = Config.HelpDeskUrl + apiPath;
				urlAttachments = Config.HelpDeskUrl + apiPathAttachments;

				Logger.Log("URL: " + url);
				Logger.Log("URL: " + urlAttachments);
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

			return await sendTicket(ticket, null);
		}

		public static async Task<int> CreateRequestWithAttachment(string email, string subject, string description, string[] tags, List<string> attachments)
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

			return await sendTicket(ticket, attachments);
		}


		private static async Task<int> sendTicket(ZendeskBaseRequest baseRequest, List<string> attachments)
		{
			int ticketId = 0;

			try
			{
				// var authArray = Encoding.ASCII.GetBytes(Config.HelpDeskUser + ":" + Config.HelpDeskPass);
				// client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(authArray));
				client.Timeout = TimeSpan.FromSeconds(15);

				var token = string.Empty;

				HttpResponseMessage response;

				// Got attachments?
				if (attachments!=null && attachments.Count > 0)
				{
					// Upload attachments to get tokens, then create the ticket

					foreach (var file in attachments)
					{
						Logger.Log("FOUND FILE: " + file);

						var attachemntUrl = urlAttachments;
						if (string.IsNullOrEmpty(token))
						{
							attachemntUrl = attachemntUrl + "?filename=" + Path.GetFileName(file);
						}else{
							attachemntUrl = attachemntUrl + "?filename=" + Path.GetFileName(file) + "&token=" + token;
						}
						Logger.Log("URL: " + attachemntUrl);

						var fileContent = new ByteArrayContent(File.ReadAllBytes(file));
						fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/binary");
						response = await client.PostAsync(attachemntUrl, fileContent);

						var responseBodyAttachments = await response.Content.ReadAsStringAsync();
						// Logger.Log("RESP: " + responseBodyAttachments);

						if (response.IsSuccessStatusCode)
						{
							var newUpload = JsonConvert.DeserializeObject<ZendeskBaseUpload>(responseBodyAttachments);
							var newToken = newUpload.upload.token;
							// Logger.Log("TOKEN: " + newToken);

							if (string.IsNullOrEmpty(token))
							{
								token = newToken;

								baseRequest.request.comment.uploads = new List<string>();
								baseRequest.request.comment.uploads.Add(token);
							}
						}

					}

				}

				var json = JsonConvert.SerializeObject(baseRequest);

				// Logger.Log("CONTENT: " + json.ToString());
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				response = await client.PostAsync(url, content);

				//response.EnsureSuccessStatusCode();
				var responseBody = await response.Content.ReadAsStringAsync();
				// Logger.Log("RESP: " + responseBody);

				if (response.IsSuccessStatusCode)
				{
					var newTicket = JsonConvert.DeserializeObject<ZendeskBaseRequest>(responseBody);
					ticketId = newTicket.request.id;
					// Logger.Log("TICKET: " + ticketId);
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
		public List<string> uploads { get; set; }
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


		public string priority { get; set; }
		public string subject { get; set; }
	}

	public class ZendeskBaseUpload
	{
		public ZendeskUpload upload { get; set; }
	}

	public class ZendeskUpload
	{
		public string token { get; set; }
	}
}
