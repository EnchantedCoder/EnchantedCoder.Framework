using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Text;
using System.Web.Compilation;
using System.Web.UI;

namespace Havit.Web.Compilation
{
	/// <summary>
	/// Prost� expression-builder pro v�razy v podob� &lt;%$ Expression: MujVyraz %&gt;.
	/// </summary>
	/// <example>
	/// Do webov� aplikace zavedeme pomoc� web.config:<br/>
	/// <code>
	/// &lt;compilation&gt;<br/>
	///		&lt;expressionBuilders&gt;<br/>
	///			&lt;add expressionPrefix="Expression" type="Havit.Web.Compilation.CodeExpressionBuilder, Havit.Web" /&gt;<br/>
	///		&lt;/expressionBuilders&gt;<br/>
	///	&lt;/compilation&gt;<br/>
	/// </code>
	/// Ve str�nce pou��v�me ji� jako klasick� expression:<br/>
	/// <code>
	/// &lt;asp:TextBox ID="NoveHesloTB" TextMode="Password" MaxLength="&lt;%$ Expression: Uzivatel.Properties.Password.MaximumLength %&gt;" runat="server" /&gt;<br/>
	/// </code>
	/// </example>
	[ExpressionPrefix("Expression")]
	public class CodeExpressionBuilder : ExpressionBuilder
	{
		/// <summary>
		/// Vrac� k�d, kter� se pou�ije nam�sto vyhodnocovan�ho v�razu p�i kompilaci.
		/// </summary>
		/// <param name="entry">objekt reprezentuj�c� informace o property, na kterou se v�raz navazuje</param>
		/// <param name="parsedData">objekt obsahuj�c� parsovan� data vr�cen� metodou ParseExpression</param>
		/// <param name="context">kontext</param>
		/// <returns>CodeExpression pro p�i�ezen� k property</returns>
		public override CodeExpression GetCodeExpression(BoundPropertyEntry entry, object parsedData, ExpressionBuilderContext context)
		{
			return new CodeSnippetExpression(entry.Expression);
		}
	}
}
