using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace Havit.Security
{
	/// <summary>
	/// Gener�tor hesel.
	/// </summary>
	/// <remarks>
	/// Vych�z� p�vodn� z http://www.codeproject.com/csharp/pwdgen.asp
	/// </remarks>
	public class PasswordGenerator
	{
		#region MinimumLength
		/// <summary>
		/// Minim�ln� d�lka hesla. Default 6.
		/// </summary>
		public int MinimumLength
		{
			get { return this._mininumLength; }
			set
			{
				this._mininumLength = value;
			}
		}
		private int _mininumLength;
		#endregion

		#region MaximumLength
		/// <summary>
		/// Maxim�ln� d�lka hesla. Default 10.
		/// </summary>
		public int MaximumLength
		{
			get { return this._maximumLength; }
			set
			{
				this._maximumLength = value;
			}
		}
		private int _maximumLength;
		#endregion

		#region PasswordCharacterSet
		/// <summary>
		/// Sada znak�, z n� se maj� vyb�rat znaky pro generovan� heslo.
		/// </summary>
		public PasswordCharacterSet PasswordCharacterSet
		{
			get { return this._passwordCharacterSet; }
			set
			{
				this._passwordCharacterSet = value;
				passwordCharArrayUpperBound = GetCharacterArrayUpperBound();
			}
		}
		private PasswordCharacterSet _passwordCharacterSet;
		private int passwordCharArrayUpperBound;

		#region GetCharacterArrayUpperBound
		/// <summary>
		/// Vr�t� horn� index pole znak�, do kter�ho se sm� prov�d�t v�b�r pro generovan� heslo.
		/// </summary>
		private int GetCharacterArrayUpperBound()
		{
			int upperBound = pwdCharArray.GetUpperBound(0);

			switch (this.PasswordCharacterSet)
			{
				case PasswordCharacterSet.LowerCaseLetters:
					upperBound = PasswordGenerator.LowerCaseLettersUpperBound;
					break;
				case PasswordCharacterSet.Letters:
					upperBound = PasswordGenerator.LettersUpperBound;
					break;
				case PasswordCharacterSet.LettersAndDigits:
					upperBound = PasswordGenerator.LettersAndDigitsUpperBound;
					break;
				case PasswordCharacterSet.LettersDigitsAndSpecialCharacters:
					// NOOP
					break;
			}
			return upperBound;
		}
		#endregion
		#endregion

		#region AllowRepeatingCharacters
		/// <summary>
		/// Indikuje, zda-li se sm� v heslu opakovat znaky. Zda-li m��e b�t n�kter� znak v heslu v�cekr�t. Default <c>true</c>.
		/// </summary>
		public bool AllowRepeatingCharacters
		{
			get { return this._allowRepeatingCharacters; }
			set { this._allowRepeatingCharacters = value; }
		}
		private bool _allowRepeatingCharacters;
		#endregion

		#region AllowConsecutiveCharacters
		/// <summary>
		/// Indikuje, zda-li sm� heslo obsahovat shluky stejn�ch znak�. Default <c>false</c>.
		/// </summary>
		public bool AllowConsecutiveCharacters
		{
			get { return this._allowConsecutiveCharacters; }
			set { this._allowConsecutiveCharacters = value; }
		}
		private bool _allowConsecutiveCharacters;
		#endregion

		#region Exclusions
		/// <summary>
		/// �et�zec znak�, kter� nechceme m�t v heslu.
		/// </summary>
		public string Exclusions
		{
			get { return this._exclusions; }
			set { this._exclusions = value; }
		}
		private string _exclusions;
		#endregion

		#region private consts
		private const int DefaultMinimum = 6;
		private const int DefaultMaximum = 10;
		private const int LowerCaseLettersUpperBound = 25;
		private const int LettersUpperBound = 51;
		private const int LettersAndDigitsUpperBound = 61;
		#endregion

		#region Constructor
		/// <summary>
		/// Vytvo�� instanci PasswordGeneratoru a nastav� v�choz� hodnoty pro slo�itost generovan�ho hesla.
		/// </summary>
		public PasswordGenerator()
		{
			this.MinimumLength = DefaultMinimum;
			this.MaximumLength = DefaultMaximum;
			this.AllowConsecutiveCharacters = false;
			this.AllowRepeatingCharacters = true;
			this.PasswordCharacterSet = PasswordCharacterSet.LettersAndDigits;
			this.Exclusions = null;

			rng = new RNGCryptoServiceProvider();
		}
		#endregion

		#region GetCryptographicRandomNumber (protected)
		/// <summary>
		/// Vygeneruje n�hodn� ��slo pomoc� crypto-API.
		/// </summary>
		/// <param name="lBound">doln� mez</param>
		/// <param name="uBound">horn� mez</param>
		protected int GetCryptographicRandomNumber(int lBound, int uBound)
		{
			// Assumes lBound >= 0 && lBound < uBound
			// returns an int >= lBound and < uBound
			uint urndnum;
			byte[] rndnum = new Byte[4];
			if (lBound == uBound - 1)
			{
				// test for degenerate case where only lBound can be returned   
				return lBound;
			}

			uint xcludeRndBase = (uint.MaxValue - (uint.MaxValue % (uint)(uBound - lBound)));

			do
			{
				rng.GetBytes(rndnum);
				urndnum = System.BitConverter.ToUInt32(rndnum, 0);
			} while (urndnum >= xcludeRndBase);

			return (int)(urndnum % (uBound - lBound)) + lBound;
		}
		#endregion

		#region GetRandomCharacter (protected)
		/// <summary>
		/// Vr�t� n�hodn� znak.
		/// </summary>
		protected char GetRandomCharacter()
		{
			int upperBound = GetCharacterArrayUpperBound();

			int randomCharPosition = GetCryptographicRandomNumber(pwdCharArray.GetLowerBound(0), upperBound);

			char randomChar = pwdCharArray[randomCharPosition];

			return randomChar;
		}
		#endregion

		#region Generate
		/// <summary>
		/// Vygeneruje heslo slo�itosti dle nastaven�ho gener�toru.
		/// </summary>
		/// <returns>vygenerovan� heslo</returns>
		public string Generate()
		{
			ValidateSettings();

			int passwordLength;
			if (this.MinimumLength == this.MaximumLength)
			{
				passwordLength = this.MinimumLength;
			}
			else
			{
				// Pick random length between minimum and maximum   
				passwordLength = GetCryptographicRandomNumber(this.MinimumLength, this.MaximumLength);
			}

			if ((!AllowRepeatingCharacters) && (passwordLength > passwordCharArrayUpperBound + 1))
			{
				// Pokud m� b�t heslo v�t��, ne� je mo�n� po�et znak�, pak ho mus�me zkr�tit.
				// Minim�ln� d�lku u� zaji��uje ValidateSettings();
				passwordLength = passwordCharArrayUpperBound + 1;
			}

			StringBuilder paswordBuffer = new StringBuilder();
			paswordBuffer.Capacity = this.MaximumLength;

			// Generate random characters
			char lastCharacter, nextCharacter;

			// Initial dummy character flag
			lastCharacter = nextCharacter = '\n';

			for (int i = 0; i < passwordLength; i++)
			{
				nextCharacter = GetRandomCharacter();

				if (!this.AllowConsecutiveCharacters)
				{
					while (lastCharacter == nextCharacter)
					{
						nextCharacter = GetRandomCharacter();
					}
				}

				if (!this.AllowRepeatingCharacters)
				{
					string temp = paswordBuffer.ToString();
					int duplicateIndex = temp.IndexOf(nextCharacter);
					while (-1 != duplicateIndex)
					{
						nextCharacter = GetRandomCharacter();
						duplicateIndex = temp.IndexOf(nextCharacter);
					}
				}

				if ((null != this.Exclusions))
				{
					while (-1 != this.Exclusions.IndexOf(nextCharacter))
					{
						nextCharacter = GetRandomCharacter();
					}
				}

				paswordBuffer.Append(nextCharacter);
				lastCharacter = nextCharacter;
			}

			if (null != paswordBuffer)
			{
				return paswordBuffer.ToString();
			}
			else
			{
				return String.Empty;
			}
		}
		#endregion

		#region ValidateSettings
		/// <summary>
		/// Kontroluje nastaven� gener�toru a vyhazuje p��padn� v�jimky.
		/// </summary>
		private void ValidateSettings()
		{
			if (this.MaximumLength < this.MinimumLength)
			{
				throw new InvalidOperationException("MaximumLength < MinimumLength");
			}

			if ((!AllowRepeatingCharacters) && (this.MinimumLength > passwordCharArrayUpperBound + 1))
			{
				throw new InvalidOperationException("Nen� dostatek znak� pro vygenerov�n� hesla po�adovan� d�lky.");
			}
		}
		#endregion

		#region private fields
		private RNGCryptoServiceProvider rng;
		private char[] pwdCharArray = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789`~!@#$%^&*()-_=+[]{}\\|;:'\",<.>/?".ToCharArray();
		#endregion

		#region Generate (static)
		/// <summary>
		/// Vygeneruje heslo slo�itosti dle po�adovan�ch parametr�.
		/// </summary>
		/// <param name="minimumLength">minim�ln� d�lka hesla</param>
		/// <param name="maximumLength">maxim�ln� d�lka hesla</param>
		/// <param name="passwordCharacterSet">Sada znak�, z n� se maj� vyb�rat znaky pro generovan� heslo.</param>
		/// <param name="allowRepeatingCharacters">Indikuje, zda-li se sm� v heslu opakovat znaky. Zda-li m��e b�t n�kter� znak v heslu v�cekr�t.</param>
		/// <param name="allowConsecutiveCharacters">Indikuje, zda-li sm� heslo obsahovat shluky stejn�ch znak�.</param>
		/// <returns>vygenerovan� heslo odpov�daj�c� vstupn�m po�adavk�m</returns>
		public static string Generate(int minimumLength, int maximumLength, PasswordCharacterSet passwordCharacterSet, bool allowRepeatingCharacters, bool allowConsecutiveCharacters)
		{
			PasswordGenerator passwordGenerator = new PasswordGenerator();
			passwordGenerator.MinimumLength = minimumLength;
			passwordGenerator.MaximumLength = maximumLength;
			passwordGenerator.PasswordCharacterSet = passwordCharacterSet;
			passwordGenerator.AllowRepeatingCharacters = allowRepeatingCharacters;
			passwordGenerator.AllowConsecutiveCharacters = allowConsecutiveCharacters;

			return passwordGenerator.Generate();
		}

		/// <summary>
		/// Vygeneruje heslo slo�itosti dle po�adovan�ch parametr�.
		/// </summary>
		/// <param name="minimumLength">minim�ln� d�lka hesla</param>
		/// <param name="maximumLength">maxim�ln� d�lka hesla</param>
		/// <param name="passwordCharacterSet">Sada znak�, z n� se maj� vyb�rat znaky pro generovan� heslo.</param>
		/// <returns>vygenerovan� heslo odpov�daj�c� vstupn�m po�adavk�m</returns>
		public static string Generate(int minimumLength, int maximumLength, PasswordCharacterSet passwordCharacterSet)
		{
			PasswordGenerator passwordGenerator = new PasswordGenerator();
			passwordGenerator.MinimumLength = minimumLength;
			passwordGenerator.MaximumLength = maximumLength;
			passwordGenerator.PasswordCharacterSet = passwordCharacterSet;

			return passwordGenerator.Generate();
		}

		/// <summary>
		/// Vygeneruje heslo slo�itosti dle po�adovan�ch parametr�.
		/// </summary>
		/// <param name="length"> d�lka hesla</param>
		/// <param name="passwordCharacterSet">Sada znak�, z n� se maj� vyb�rat znaky pro generovan� heslo.</param>
		/// <returns>vygenerovan� heslo odpov�daj�c� vstupn�m po�adavk�m</returns>
		public static string Generate(int length, PasswordCharacterSet passwordCharacterSet)
		{
			PasswordGenerator passwordGenerator = new PasswordGenerator();
			passwordGenerator.MinimumLength = length;
			passwordGenerator.MaximumLength = length;
			passwordGenerator.PasswordCharacterSet = passwordCharacterSet;

			return passwordGenerator.Generate();
		}
		#endregion
	}
}
