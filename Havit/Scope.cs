using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Havit
{
	/// <summary>
	/// Thread-specific Scope obaluj�c� dosah platnosti ur�it�ho objektu (transakce, identity mapy, atp.),
	/// kter� je n�sledn� p��stupn� p�es property <see cref="Current"/>.
	/// </summary>
	/// <example>
	/// <code>
	/// using (new Scope&lt;IdentityMap&gt;(new IdentityMap()))
	/// {
    ///		Console.WriteLine(Scope.Current.SomeMethod("outer scope"));
	/// 
    ///		using (new Scope&lt;IdentityMap&gt;(new IdentityMap()))
	///		{
    ///			Console.WriteLine(Scope.Current.SomeMethod("inner scope"));
	///		}
    ///		
	///		Console.WriteLine(Scope.Current.SomeMethod("inner scope"));
	///	}
	/// </code>
	/// </example>
	/// <remarks>
	/// Implementace vych�zej�c� z MSDN Magazine �l�nku <a href="http://msdn.microsoft.com/msdnmag/issues/06/09/netmatters/default.aspx">Stephen Toub: Scope&lt;T&gt; and More</a>.
	/// </remarks>
	/// <typeparam name="T">typ objektu, jeho� scope �e��me</typeparam>
	public class Scope<T> : IDisposable
		where T : class
	{
		#region private fields
		/// <summary>
		/// Indikuje, zda-li ji� prob�hl Dispose t��dy.
		/// </summary>
		private bool disposed;
		
		/// <summary>
		/// Indikuje, zda-li je instance scopem vlastn�n�, tj. m�me-li ji na konci scope disposovat.
		/// </summary>
		private bool ownsInstance;
		
		/// <summary>
		/// Instance, kterou scope obaluje.
		/// </summary>
		private T instance;
		
		/// <summary>
		/// Nad�azen� scope v linked-listu nestovan�ch scope.
		/// </summary>
		private Scope<T> parent;
		#endregion

		#region Constructors
		/// <summary>
		/// Vytvo�� instanci t��dy <see cref="Scope"/> kolem instance. Instance bude p�i disposingu Scope t� disposov�na.
		/// </summary>
		/// <param name="instance">instance, kterou scope obaluje</param>
		public Scope(T instance) : this(instance, true) { }

		/// <summary>
		/// Vytvo�� instanci t��dy <see cref="Scope"/> kolem instance.
		/// </summary>
		/// <param name="instance">instance, kterou scope obaluje</param>
		/// <param name="ownsInstance">indikuje, zda-li instanci vlastn�me, tedy zda-li ji m�me s koncem scopu disposovat</param>
		public Scope(T instance, bool ownsInstance)
		{
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}
			this.instance = instance;
			this.ownsInstance = ownsInstance;

			// linked-list pro nestov�n� scopes
			this.parent = head;
			head = this;
		}
		#endregion

		#region Dispose (IDisposable)
		/// <summary>
		/// Ukon�� scope a disposuje vlastn�n� instance.
		/// </summary>
		/// <remarks>
		/// ResourceWrapper pattern nepot�ebujeme, proto�e nem�me ��dn� unmanaged resources, kter� bychom museli jistit destructorem.
		/// </remarks>
		public virtual void Dispose()
		{
			if (!disposed)
			{
				disposed = true;

				Debug.Assert(this == head, "Disposed out of order.");
				head = parent;

				if (ownsInstance)
				{
					IDisposable disposable = instance as IDisposable;
					if (disposable != null) disposable.Dispose();
				}
			}
		}
		#endregion

		#region private field (static)
		/// <summary>
		/// Aktu�ln� konec linked-listu nestovan�ch scope.
		/// </summary>
		[ThreadStatic]
		private static Scope<T> head;
		#endregion

		#region Current (static)
		/// <summary>
		/// Aktu�ln� instance obalovan� scopem.
		/// </summary>
		public static T Current
		{
			get { return head != null ? head.instance : null; }
		}
		#endregion
	}

}
