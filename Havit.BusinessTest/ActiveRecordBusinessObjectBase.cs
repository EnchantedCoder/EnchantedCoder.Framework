using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Havit.BusinessLayerTest;
using Havit.Data;
using System.Data.Common;
using Havit.Business;

namespace Havit.BusinessTest
{
	/// <summary>
	/// Unit test na ActiveRecordBusinessObjectBase
	/// </summary>
	[TestClass]
	public class ActiveRecordBusinessObjectBase
	{
		/// <summary>
		/// Defect 329: CheckConstraints je obejito, pokud je objekt ulo�en p�es MinimalInsert.
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(ConstraintViolationException))]
		public void MinimalInsertUsesCheckConstraint()
		{			
			Subjekt subjekt = Subjekt.CreateObject();
			Uzivatel uzivatel = Uzivatel.CreateObject(); 
			subjekt.Uzivatel = uzivatel;
			
			// nastav�me �et�zec del��, ne� je povoleno
			string username = "";
			while (username.Length <= Uzivatel.Properties.Username.MaximumLength)
			{
				username = username + "0";
			}
			uzivatel.Username = username;

			// u�ivatel mus� b�t otestov�n pomoc� CheckConstraint, o�ek�v�me v�jimku ConstraintViolationException
			subjekt.Save();
		}
	}
}
