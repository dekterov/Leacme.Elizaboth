// Copyright (c) 2017 Leacme (http://leac.me). View LICENSE.md for more information.
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using ELIZA.NET;
using Leacme.Lib.Elizaboth;

namespace Leacme.App.Elizaboth {

	public class AppUI {

		private StackPanel rootPan = (StackPanel)Application.Current.MainWindow.Content;
		private Library lib = new Library();
		private ELIZALib currentBot = null;
		private (StackPanel holder, TextBlock label, ComboBox comboBox) selP1 = App.ComboBoxWithLabel;
		private TextBox outp1 = App.TextBox;

		public AppUI() {

			selP1.label.Text = "Chatbot Personality:";
			selP1.holder.HorizontalAlignment = HorizontalAlignment.Center;
			selP1.comboBox.Items = lib.GetAvailableScripts().Keys;
			selP1.comboBox.SelectedIndex = 0;
			selP1.comboBox.SelectionChanged += (z, zz) => {
				InitChat();

			};
			InitChat();

			outp1.IsReadOnly = true;
			outp1.Width = 900;
			outp1.Height = 375;
			outp1.TextAlignment = TextAlignment.Center;

			var repl1 = App.HorizontalFieldWithButton;
			repl1.holder.HorizontalAlignment = HorizontalAlignment.Center;
			repl1.label.Text = "Reply:";
			repl1.field.Width = 700;
			repl1.button.Content = "Send";
			repl1.button.Click += (z, zz) => {
				if (!string.IsNullOrWhiteSpace(repl1.field.Text)) {
					outp1.Text = outp1.Text + "\n" + "You:" + "\n" + repl1.field.Text + "\n";
					outp1.Text = outp1.Text + "\n" + selP1.comboBox.SelectedItem + " Bot:" + "\n" + lib.GetResponse(currentBot, repl1.field.Text) + "\n";
					repl1.field.Text = string.Empty;
				}
			};

			rootPan.Children.AddRange(new List<IControl> { selP1.holder, outp1, repl1.holder });
		}

		private void InitChat() {
			currentBot = lib.GetElizabothClient((string)selP1.comboBox.SelectedItem);
			outp1.Text = string.Empty;
			outp1.Text = outp1.Text + "\n" + selP1.comboBox.SelectedItem + " Bot:" + "\n" + lib.GetGreeting(currentBot) + "\n";
		}

	}
}