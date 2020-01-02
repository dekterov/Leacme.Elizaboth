// Copyright (c) 2017 Leacme (http://leac.me). View LICENSE.md for more information.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ELIZA.NET;
using Newtonsoft.Json;

namespace Leacme.Lib.Elizaboth {

	public class Library {

		public Library() {

		}

		/// <summary>
		/// Get the ELIZA bot client to send and receive messages from. Gets the default bot in the system if not parameters specified.
		/// /// </summary>
		/// <param name="scriptName">Optionally retrieve a bot that's available in the system by its name.</param>
		/// <param name="jsonScriptContent">Optional JSON script for a bot not in the system.</param>
		/// <returns></returns>
		public ELIZALib GetElizabothClient(string scriptName = null, string jsonScriptContent = null) {
			ELIZALib el = null;

			if (scriptName != null) {
				if (!GetAvailableScripts().ContainsKey(scriptName)) {
					throw new InvalidDataException("No such script with the name.");
				}
				el = new ELIZALib(GetAvailableScripts()[scriptName]);
			}

			if (el == null && jsonScriptContent != null) {
				try {
					el = new ELIZALib(jsonScriptContent);
				} catch (JsonReaderException) {
					throw new InvalidDataException("Invalid script.");
				}
			}

			if (el == null) {
				el = new ELIZALib(GetAvailableScripts().First().Value);
			}
			return el;
		}

		/// <summary>
		/// Get the introductory line from the bot.
		/// /// </summary>
		/// <param name="client">The specfied bot client.</param>
		/// <returns></returns>
		public string GetGreeting(ELIZALib client) {
			return client.Session.GetGreeting();
		}

		/// <summary>
		/// Get the closing line from the bot.
		/// </summary>
		/// <param name="client">The specfied bot client.</param>
		/// <returns></returns>
		public string GetGoodbye(ELIZALib client) {
			return client.Session.GetGoodbye();
		}

		/// <summary>
		/// Get the response to the query from the bot.
		/// /// </summary>
		/// <param name="client">The specfied bot client.</param>
		/// <param name="query">The query to send the bot.</param>
		/// <returns></returns>
		public string GetResponse(ELIZALib client, string query) {
			return client.GetResponse(query);
		}

		/// <summary>
		/// Get available stored bot scripts in the system.
		/// /// </summary>
		/// <returns>A dictionary with the bot name and its script content.</returns>
		public Dictionary<string, string> GetAvailableScripts() {

			Dictionary<string, string> scripts = new Dictionary<string, string>();
			var asm = typeof(Library).GetTypeInfo().Assembly;

			asm.GetManifestResourceNames().ToList().Where(z => z.EndsWith(".json")).ToList().ForEach(z => {
				string content;
				using (Stream rs = asm.GetManifestResourceStream(z)) {
					using (var sr = new StreamReader(rs)) {
						content = sr.ReadToEnd();
					}
				}
				scripts.Add(z.Split(".").Reverse().Skip(1).Take(1).First(), content);
			});

			return scripts;
		}
	}
}