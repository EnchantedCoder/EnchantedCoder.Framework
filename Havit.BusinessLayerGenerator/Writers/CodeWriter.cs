﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.Business.BusinessLayerGenerator.TfsClient;
using Havit.Web;

namespace Havit.Business.BusinessLayerGenerator.Writers
{
	/// <summary>
	/// Třída CodeWriter zajišťuje generování kódu. Zajišťuje odsazení, v budoucnu může zajišťovat ochranu před maximální délkou řádky kódu, apod.
	/// </summary>
	public class CodeWriter
	{
		#region Private const - IndentString
		/// <summary>
		/// Řetězec reprezentující úroveň odsazení.
		/// </summary>
		private const string IndentString = "\t";
		#endregion

		#region Private fields
		/// <summary>
		/// Celá cílová cesta pro zapisovaný kód.
		/// </summary>
		private readonly string fullPath;

	    private bool fileExists;
	    private string fileContent;
	    private bool fileHasByteOrderMask;
        private readonly Task preloadFileTask;

		/// <summary>
		/// Průběžně tvořený obsah souboru
		/// </summary>
		private readonly StringBuilder code;
		
		private readonly SourceControlClient sourceControlClient;

		private readonly bool eliminateEmtpyLinesBeforeClosingParenthesis;

		private int emtpyLinesBeforeClosingParenthesisCounter = 0;

		#endregion

		#region Constructor
		public CodeWriter(string fullPath, SourceControlClient sourceControlClient, bool eliminateEmtpyLinesBeforeClosingParenthesis = false)
		{
			this.fullPath = fullPath;
			this.code = new StringBuilder(51200);
			this.sourceControlClient = sourceControlClient;
			this.eliminateEmtpyLinesBeforeClosingParenthesis = eliminateEmtpyLinesBeforeClosingParenthesis;
		    preloadFileTask = Task.Factory.StartNew(() =>
		    {
		        fileExists = File.Exists(this.fullPath);
		        if (fileExists)
		        {
		            fileContent = File.ReadAllText(this.fullPath);
		            fileHasByteOrderMask = HasByteOrderMask();
		        }
		    });
		}
		#endregion

		#region Metody pro výpis - Write...
		/// <summary>
		/// Vypíše prázdný řádek. Odsazení je dodrženo.
		/// </summary>
		public void WriteLine()
		{
			WriteLine(String.Empty);
		}

		/// <summary>
		/// Vypíše řádek kódu, provede jeho odsazení.
		/// Pokud řádek kódu je právě "{", zvýší se odsazení, pokud je "}", sníží se odsazení.
		/// </summary>
		public void WriteLine(string line)
		{
			if (eliminateEmtpyLinesBeforeClosingParenthesis)
			{
				if (String.IsNullOrEmpty(line))
				{
					emtpyLinesBeforeClosingParenthesisCounter += 1;
					return;
				}
				if (line != "}")
				{
					for (int i = 0; i < emtpyLinesBeforeClosingParenthesisCounter; i++)
					{
						WriteRawLine("");
					}
				}
				emtpyLinesBeforeClosingParenthesisCounter = 0;
			}

			if (line.StartsWith("#if ") || line.StartsWith("#endif") || line.StartsWith("#else") || line.StartsWith("#pragma warning"))
			{
				WriteRawLine(line);
			}
			else
			{
				if (line == "}")
				{
					Unindent();
				}

				for (int i = 0; i < indentLevel; i++)
				{
					code.Append(IndentString);
				}
				WriteRawLine(line);

				if (line == "{")
				{
					Indent();
				}
			}
		}

		/// <summary>
		/// Vypíše řádky kódu, provede odstazení.
		/// </summary>
		public void WriteLines(string[] lines)
		{
			foreach (string line in lines)
			{
				WriteLine(line);
			}
		}

		/// <summary>
		/// Vypíše komentář (na začátek řetězce umístí ///).
		/// </summary>
		public void WriteCommentLine(string comment)
		{
			foreach (string commentLine in comment.Trim('\r', '\n').Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None))
			{
				// ignorované komentáře (asi bych měl mít jinde, ale...)
				if (commentLine == "SET ANSI_NULLS ON")
				{
					continue;
				}

				if (commentLine == "SET QUOTED_IDENTIFIER ON")
				{
					continue;
				}

				WriteLine("/// " + commentLine);
			}
		}

		/// <summary>
		/// Vypíše dokumentační komentář &lt;summary&gt;...&lt;/summary&gt;.
		/// </summary>
		public void WriteCommentSummary(string comment)
		{
			if (!String.IsNullOrEmpty(comment))
			{
				WriteCommentLine("<summary>");
				WriteCommentLine(HttpUtilityExt.HtmlEncode(comment, HtmlEncodeOptions.IgnoreNonASCIICharacters));
				WriteCommentLine("</summary>");
			}
		}

		/// <summary>
		/// Vypíše dokumentační komentář &lt;remarks&gt;...&lt;/remarks&gt;.
		/// </summary>
		public void WriteCommentRemarks(string comment)
		{
			if (!String.IsNullOrEmpty(comment))
			{

				WriteCommentLine("<remarks>");
				WriteCommentLine(HttpUtilityExt.HtmlEncode(comment, HtmlEncodeOptions.IgnoreNonASCIICharacters));
				WriteCommentLine("</remarks>");
			}
		}

		/// <summary>
		/// Otevře region (na začátek řetězce umístí #region).
		/// </summary>
		public void WriteOpenRegion(string region)
		{
			WriteLine("#region " + region);
		}

		/// <summary>
		/// Zavře region (vypíše #endregion).
		/// </summary>
		public void WriteCloseRegion()
		{
			WriteLine("#endregion");
			WriteLine();
		}

		/// <summary>
		/// Zapíše contract. Připraveno jako metoda pro případné podmínění projektů, kde nechceme contracty.
		/// </summary>
		public void WriteHavitContract(string line)
		{
			WriteLine(line);
		}

		/// <summary>
		/// Zapíše contract. Připraveno jako metoda pro případné podmínění projektů, kde nechceme contracty.
		/// Nyní deaktivováno.
		/// </summary>
		public void WriteMicrosoftContract(string line)
		{
			// Code Contracts deaktivovány.
			// WriteLine(line);
		}

		/// <summary>
		/// Zapíše atribut identifikující generovaný kód.
		/// </summary>
		public void WriteGeneratedCodeAttribute()
		{
			this.WriteLine("[System.CodeDom.Compiler.GeneratedCode(\"Havit.BusinessLayerGenerator\", \"1.0\")]");
		}

		/// <summary>
		/// Vypíše // NOOP komentář.
		/// </summary>
		public void WriteNOOP()
		{
			WriteLine("// NOOP");
		}

		/// <summary>
		/// Zapíše řádku do výstupu tak, jak je (bez doplnění odsazování, atp.).
		/// </summary>
		internal void WriteRawLine(string line)
		{
			code.AppendLine(line);
		}

		/// <summary>
		/// Zapíše řádky do výstupu tak, jak jsou (bez doplnění odsazování, atp.).
		/// </summary>
		internal void WriteRawLines(IEnumerable<string> lines)
		{
			foreach (string line in lines)
			{
				WriteRawLine(line);
			}
		}

		#endregion

		#region Indend
		/// <summary>
		/// Aktuální odsazení.
		/// </summary>
		private int indentLevel = 0;

		/// <summary>
		/// Zvýší úroveň odsazení.
		/// </summary>
		public void Indent()
		{
			indentLevel += 1;
		}

		/// <summary>
		/// Sníží úroveň odsazení.
		/// </summary>
		public void Unindent()
		{
			emtpyLinesBeforeClosingParenthesisCounter = 0;

			if (indentLevel == 0)
			{
				throw new InvalidOperationException("Nelze snížit odsazení, již jsme na nule.");
			}

			indentLevel -= 1;
		}
		#endregion

		#region GetContent
		/// <summary>
		/// Vrátí obsah (kód) ve writeru.
		/// </summary>
		public string GetContent()
		{
			return code.ToString();
		}
		#endregion

		#region Save
		/// <summary>
		/// Zapíšeme obsah do souboru (jen tehdy, pokud se neliší od současného obsahu souboru).
		/// </summary>
		public void Save()
		{
		    preloadFileTask.Wait();

            if (!this.AlreadyExistsTheSame() || !fileHasByteOrderMask)
			{
				string directory = Path.GetDirectoryName(this.fullPath);
				if (!String.IsNullOrEmpty(directory))
				{
					Directory.CreateDirectory(directory);
				}

				if (!fileExists && (sourceControlClient != null))
				{
					File.WriteAllText(this.fullPath, this.GetContent(), Encoding.UTF8);
					sourceControlClient.PendAdd(this.fullPath);
				}
				else
				{
					File.WriteAllText(this.fullPath, this.GetContent(), Encoding.UTF8);					
				}
			}
		}
		#endregion

		#region AlreadyExistsTheSame
		/// <summary>
		/// Vrací true, pokud již existuje soubor se stejným jménem a obsahem.
		/// </summary>
		public bool AlreadyExistsTheSame()
		{
		    preloadFileTask.Wait();
            return (fileExists && (fileContent == this.GetContent()));
		}
		#endregion

		#region HasByteOrderMask
		/// <summary>
		/// Vrací true, pokud soubor již existuje a má UTF-8 byte order mask.
		/// </summary>
		private bool HasByteOrderMask()
		{            
			byte[] utf8BOM = Encoding.UTF8.GetPreamble();
			if (!File.Exists(this.fullPath) || (new FileInfo(this.fullPath).Length < utf8BOM.Length))
			{
				return false;
			}

			byte[] fileBOM;
			using (FileStream fileStream = new FileStream(this.fullPath, FileMode.Open))
			{
				using (BinaryReader reader = new BinaryReader(fileStream))
				{
					fileBOM = reader.ReadBytes(utf8BOM.Length);
				}
			}
			return utf8BOM.SequenceEqual(fileBOM);
		}
		#endregion
	}
}
