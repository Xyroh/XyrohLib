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
	public static class Freshdesk
	{
		private static HttpClient client;
		private static readonly string apiPath = "/api/v2/tickets";
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

		public static async Task<int> CreateTicket(string email, string subject, string description, string[] tags)
		{
			var ticket = new FreshDeskTicket();
				
			if (Config.CanUseHelpDesk)
			{
				ticket = new FreshDeskTicket();
				ticket.email = email;
				ticket.subject = subject;
				ticket.description = description;
				ticket.tags = tags;
			}
			
			return await sendTicket(ticket);
		}
		
		public static async Task<int> CreateTicketWithAttachment(string email, string subject, string description, string[] tags, List<string> attachments)
		{
			var ticket = new FreshDeskTicket();
				
			if (Config.CanUseHelpDesk)
			{
				ticket = new FreshDeskTicket();
				ticket.email = email;
				ticket.subject = subject;
				ticket.description = description;
				ticket.tags = tags;
				ticket.attachments = attachments;
			}
			
			return await sendTicket(ticket);
		}


		private static async Task<int> sendTicket(FreshDeskTicket ticket)
		{
			int ticketId = 0;

			try
			{
				var authArray = Encoding.ASCII.GetBytes(Config.HelpDeskUser + ":" + Config.HelpDeskPass);
				client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(authArray));
				client.Timeout = TimeSpan.FromSeconds(15);
				
				var json = JsonConvert.SerializeObject(ticket);

				HttpResponseMessage response;
				
				// Got attachments?
				if (ticket.attachments != null && ticket.attachments.Count > 0)
				{

					using (var formData = new MultipartFormDataContent())
					{
						formData.Add(new StringContent(ticket.description), String.Format("\"description\""));
						formData.Add(new StringContent(ticket.email), String.Format("\"email\""));
						formData.Add(new StringContent(ticket.subject), String.Format("\"subject\""));
						formData.Add(new StringContent(ticket.priority.ToString()), String.Format("\"priority\""));
						formData.Add(new StringContent(ticket.source.ToString()), String.Format("\"source\""));
						formData.Add(new StringContent(ticket.status.ToString()), String.Format("\"status\""));
						
						foreach (var tag in ticket.tags)
						{
							formData.Add(new StringContent(tag), String.Format("\"tags[]\""));
						}
						
						foreach (var file in ticket.attachments)
						{
							//Console.WriteLine("FOUND FILE: " + file);

							var fileContent = new ByteArrayContent(File.ReadAllBytes(file));
							formData.Add(fileContent, String.Format("\"attachments[]\""), Path.GetFileName(file));

						}
						
						response = await client.PostAsync(url, formData);
					}
				}
				else
				{
					// application/json
					//client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
					var jo = JObject.Parse(json);
					jo.Property("attachments").Remove();
					json = jo.ToString();
					
					//Logger.Log("CONTENT: " + json.ToString());
					var content = new StringContent(json, Encoding.UTF8, "application/json");
				
					response = await client.PostAsync(url, content);
				}
				
				
				//response.EnsureSuccessStatusCode();
				var responseBody = await response.Content.ReadAsStringAsync();
				Logger.Log("RESP: " + responseBody);

				if (response.IsSuccessStatusCode)
				{
					var newTicket = JsonConvert.DeserializeObject<FreshDeskTicketResponse>(responseBody);
					ticketId = newTicket.id;
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




	public class FreshDeskTicket
	{
		public string subject { get; set; }
		public string description { get; set; }
		public string email { get; set; }
		public int priority = 2;
		public int status = 2;
		public int source = 9;
		public string[] tags { get; set; }
		public List<string> attachments { get; set; }
	}
	

	public class FreshDeskTicketResponse
	{
		public int id { get; set; }
	}
}