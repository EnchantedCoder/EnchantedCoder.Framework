﻿#pragma warning disable 1591
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Reflection;

[assembly: WebResource("Havit.Web.UI.Adapters.CssAdapters.CssTreeViewAdapter.js", "text/javascript")]
[assembly: WebResource("Havit.Web.UI.Adapters.CssAdapters.CssTreeViewAdapter.css", "text/css")]

namespace Havit.Web.UI.Adapters.CssAdapters
{
	/// <summary>
	/// TreeViewAdapter z CSS Friendly Control Adapters. Renderuje TreeView pomocí HTML elementů ul, li, CSS stylů a JScriptu.
	/// </summary>
	/// <remarks>
	/// ASP.NET 2.0 CSS Friendly Control Adapters 1.0, http://www.asp.net/CSSAdapters/, This version was last updated on 20 November 2006.
	/// </remarks>
	public class CssTreeViewAdapter : System.Web.UI.WebControls.Adapters.HierarchicalDataBoundControlAdapter, IPostBackEventHandler, IPostBackDataHandler
	{
		#region Private fields
		private int _checkboxIndex = 1;
		private readonly HiddenField _viewState = null;
		private bool _updateViewState = false;
		private string _newViewState = "";
		#endregion

		#region Extender
		private WebControlAdapterExtender Extender
		{
			get
			{
				if (((_extender == null) && (Control != null)) ||
					((_extender != null) && (Control != _extender.AdaptedControl)))
				{
					_extender = new WebControlAdapterExtender(Control);
				}

				System.Diagnostics.Debug.Assert(_extender != null, "CSS Friendly adapters internal error", "Null extender instance");
				return _extender;
			}
		}
		private WebControlAdapterExtender _extender = null;
		#endregion

		#region Constructor
		public CssTreeViewAdapter()
		{
			if (_viewState == null)
			{
				_viewState = new HiddenField();
			}
		}
		#endregion

		#region LoadPostData
		// Implementation of IPostBackDataHandler
		public virtual bool LoadPostData(string postDataKey, NameValueCollection postCollection)
		{
			return true;
		}
		#endregion

		#region RaisePostDataChangedEvent
		public virtual void RaisePostDataChangedEvent()
		{
			TreeView treeView = Control as TreeView;
			if (treeView != null)
			{
				TreeNodeCollection items = treeView.Nodes;
				_checkboxIndex = 1;
				UpdateCheckmarks(items);
			}
		}
		#endregion

		#region RaisePostBackEvent
		// Implementation of IPostBackEventHandler
		public void RaisePostBackEvent(string eventArgument)
		{
			TreeView treeView = Control as TreeView;
			if (treeView != null)
			{
				TreeNodeCollection items = treeView.Nodes;
				if (!String.IsNullOrEmpty(eventArgument))
				{
					if (eventArgument.StartsWith("s") || eventArgument.StartsWith("e"))
					{
						string selectedNodeValuePath = eventArgument.Substring(1).Replace("\\", "/");
						TreeNode selectedNode = treeView.FindNode(selectedNodeValuePath);
						if (selectedNode != null)
						{
							bool bSelectedNodeChanged = selectedNode != treeView.SelectedNode;
							ClearSelectedNode(items);
							selectedNode.Selected = true; // does not raise the SelectedNodeChanged event so we have to do it manually (below).
							if (eventArgument.StartsWith("e"))
							{
								selectedNode.Expand();
							}

							if (bSelectedNodeChanged)
							{
								Extender.RaiseAdaptedEvent("SelectedNodeChanged", new EventArgs());
							}
						}
					}
					else if (eventArgument.StartsWith("p"))
					{
						string parentNodeValuePath = eventArgument.Substring(1).Replace("\\", "/");
						TreeNode parentNode = treeView.FindNode(parentNodeValuePath);
						if ((parentNode != null) && ((parentNode.ChildNodes == null) || (parentNode.ChildNodes.Count == 0)))
						{
							parentNode.Expand(); // Raises the TreeNodePopulate event
						}
					}
				}
			}
		}
		#endregion

		#region SaveAdapterViewState
		protected override Object SaveAdapterViewState()
		{
			string retStr = "";
			TreeView treeView = Control as TreeView;
			if ((treeView != null) && (_viewState != null))
			{
				if ((_viewState != null) && (Page != null) && (Page.Form != null) && (!Page.Form.Controls.Contains(_viewState)))
				{
					Panel panel = new Panel();
					panel.Controls.Add(_viewState);
					Page.Form.Controls.Add(panel);
					string script = "document.getElementById('" + _viewState.ClientID + "').value = GetViewState__AspNetTreeView('" + Extender.MakeChildId("UL") + "');";
					Page.ClientScript.RegisterOnSubmitStatement(typeof(CssTreeViewAdapter), _viewState.ClientID, script);
				}
				retStr = _viewState.UniqueID + "|" + ComposeViewState(treeView.Nodes, "");
			}
			return retStr;
		}
		#endregion

		#region LoadAdapterViewState
		protected override void LoadAdapterViewState(Object state)
		{
			TreeView treeView = Control as TreeView;
			string oldViewState = state as String;
			if ((treeView != null) && (oldViewState != null) && (oldViewState.Split('|').Length == 2))
			{
				string hiddenInputName = oldViewState.Split('|')[0];
				string oldExpansionState = oldViewState.Split('|')[1];
				if (!treeView.ShowExpandCollapse)
				{
					_newViewState = oldExpansionState;
					_updateViewState = true;
				}
				else if (!String.IsNullOrEmpty(Page.Request.Form[hiddenInputName]))
				{
					_newViewState = Page.Request.Form[hiddenInputName];
					_updateViewState = true;
				}
			}
		}
		#endregion

		#region OnInit
		protected override void OnInit(EventArgs e)
		{
			if (Extender.AdapterEnabled)
			{
				_updateViewState = false;
				_newViewState = "";

				TreeView treeView = Control as TreeView;
				if (treeView != null)
				{
					treeView.EnableClientScript = false;
				}
			}

			base.OnInit(e);

			if (Extender.AdapterEnabled)
			{
				RegisterScripts();
				RegisterStyles();
			}
		}
		#endregion

		#region OnLoad
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			TreeView treeView = Control as TreeView;
			if (Extender.AdapterEnabled && _updateViewState && (treeView != null))
			{
				treeView.CollapseAll();
				ExpandToState(treeView.Nodes, _newViewState);
				_updateViewState = false;
			}
		}
		#endregion

		#region RegisterScripts
		private void RegisterScripts()
		{
			Extender.RegisterScripts();

			Page.ClientScript.RegisterClientScriptResource(typeof(CssTreeViewAdapter), "Havit.Web.UI.Adapters.CssAdapters.CssTreeViewAdapter.js");

			// původní kód originálního CssAdaptéru
			//string folderPath = WebConfigurationManager.AppSettings.Get("CSSFriendly-JavaScript-Path");
			//if (String.IsNullOrEmpty(folderPath))
			//{
			//    folderPath = "~/JavaScript";
			//}
			//string filePath = folderPath.EndsWith("/") ? folderPath + "TreeViewAdapter.js" : folderPath + "/TreeViewAdapter.js";
			//Page.ClientScript.RegisterClientScriptInclude(GetType(), GetType().ToString(), Page.ResolveUrl(filePath));
		}
		#endregion

		#region RegisterStyles
		private void RegisterStyles()
		{
			HttpContext context = HttpContext.Current;

			if (Page.Header != null)
			{
				bool registered = (bool)(context.Items["Havit.Web.UI.Adapters.CssAdapters.CssTreeViewAdapter_registered"] ?? false);

				if (!registered)
				{
					HtmlLink stylesBasic = new HtmlLink();
					stylesBasic.Href = Page.ClientScript.GetWebResourceUrl(typeof(CssTreeViewAdapter), "Havit.Web.UI.Adapters.CssAdapters.CssTreeViewAdapter.css");
					stylesBasic.Attributes.Add("rel", "stylesheet");
					stylesBasic.Attributes.Add("type", "text/css");
					Page.Header.Controls.Add(stylesBasic);

					context.Items["Havit.Web.UI.Adapters.CssAdapters.CssTreeViewAdapter_registered"] = true;
				}
			}
		}
		#endregion

		#region RenderBeginTag
		protected override void RenderBeginTag(HtmlTextWriter writer)
		{
			if (Extender.AdapterEnabled)
			{
				Extender.RenderBeginTag(writer, "AspNet-TreeView");
			}
			else
			{
				base.RenderBeginTag(writer);
			}
		}
		#endregion

		#region RenderEndTag
		protected override void RenderEndTag(HtmlTextWriter writer)
		{
			if (Extender.AdapterEnabled)
			{
				Extender.RenderEndTag(writer);
			}
			else
			{
				base.RenderEndTag(writer);
			}
		}
		#endregion

		#region RenderContents
		protected override void RenderContents(HtmlTextWriter writer)
		{
			if (Extender.AdapterEnabled)
			{
				TreeView treeView = Control as TreeView;
				if (treeView != null)
				{
					writer.Indent++;
					_checkboxIndex = 1;
					BuildItems(treeView.Nodes, true, true, writer);
					writer.Indent--;
					writer.WriteLine();
				}
			}
			else
			{
				base.RenderContents(writer);
			}
		}
		#endregion

		#region BuildItems
		private void BuildItems(TreeNodeCollection items, bool isRoot, bool isExpanded, HtmlTextWriter writer)
		{
			if (items.Count > 0)
			{
				writer.WriteLine();

				writer.WriteBeginTag("ul");

				if (isRoot)
				{
					writer.WriteAttribute("id", Extender.MakeChildId("UL"));
				}
				if (!isExpanded)
				{
					writer.WriteAttribute("class", "AspNet-TreeView-Hide");
				}
				writer.Write(HtmlTextWriter.TagRightChar);
				writer.Indent++;

				foreach (TreeNode item in items)
				{
					BuildItem(item, writer);
				}

				writer.Indent--;
				writer.WriteLine();
				writer.WriteEndTag("ul");
			}
		}
		#endregion

		#region BuildItem
		private void BuildItem(TreeNode item, HtmlTextWriter writer)
		{
			TreeView treeView = Control as TreeView;
			if ((treeView != null) && (item != null) && (writer != null))
			{
				writer.WriteLine();
				writer.WriteBeginTag("li");
				writer.WriteAttribute("class", GetNodeClass(item));
				writer.Write(HtmlTextWriter.TagRightChar);
				writer.Indent++;
				writer.WriteLine();

				if (IsExpandable(item) && treeView.ShowExpandCollapse)
				{
					WriteNodeExpander(treeView, item, writer);
				}

				if (IsCheckbox(treeView, item))
				{
					WriteNodeCheckbox(treeView, item, writer);
				}
				else if (IsLink(item))
				{
					WriteNodeLink(treeView, item, writer);
				}
				else
				{
					WriteNodePlain(treeView, item, writer);
				}

				if (HasChildren(item))
				{
					BuildItems(item.ChildNodes, false, item.Expanded.Equals(true), writer);
				}

				writer.Indent--;
				writer.WriteLine();
				writer.WriteEndTag("li");
			}
		}
		#endregion

		#region WriteNodeExpander
		private void WriteNodeExpander(TreeView treeView, TreeNode item, HtmlTextWriter writer)
		{
			writer.WriteBeginTag("span");
			writer.WriteAttribute("class", (item.Expanded.Equals(true) ? "AspNet-TreeView-Collapse" : "AspNet-TreeView-Expand"));
			if (HasChildren(item))
			{
				writer.WriteAttribute("onclick", "ExpandCollapse__AspNetTreeView(this)");
			}
			else
			{
				writer.WriteAttribute("onclick", Page.ClientScript.GetPostBackEventReference(treeView, "p" + (Page.Server.HtmlEncode(item.ValuePath)).Replace("/", "\\"), true));
			}
			writer.Write(HtmlTextWriter.TagRightChar);
			writer.Write("&nbsp;");
			writer.WriteEndTag("span");
			writer.WriteLine();
		}
		#endregion

		#region WriteNodeImage
		private void WriteNodeImage(TreeView treeView, TreeNode item, HtmlTextWriter writer)
		{
			string imgSrc = GetImageSrc(treeView, item);
			if (!String.IsNullOrEmpty(imgSrc))
			{
				writer.WriteBeginTag("img");
				writer.WriteAttribute("src", treeView.ResolveClientUrl(imgSrc));
				writer.WriteAttribute("alt", !String.IsNullOrEmpty(item.ToolTip) ? item.ToolTip : (!String.IsNullOrEmpty(treeView.ToolTip) ? treeView.ToolTip : item.Text));
				writer.Write(HtmlTextWriter.SelfClosingTagEnd);
			}
		}
		#endregion

		#region WriteNodeCheckbox
		private void WriteNodeCheckbox(TreeView treeView, TreeNode item, HtmlTextWriter writer)
		{
			writer.WriteBeginTag("input");
			writer.WriteAttribute("type", "checkbox");
			writer.WriteAttribute("id", treeView.ClientID + "n" + _checkboxIndex.ToString() + "CheckBox");
			writer.WriteAttribute("name", treeView.UniqueID + "n" + _checkboxIndex.ToString() + "CheckBox");

			if (!String.IsNullOrEmpty(treeView.Attributes["OnClientClickedCheckbox"]))
			{
				writer.WriteAttribute("onclick", treeView.Attributes["OnClientClickedCheckbox"]);
			}

			if (item.Checked)
			{
				writer.WriteAttribute("checked", "checked");
			}
			writer.Write(HtmlTextWriter.SelfClosingTagEnd);

			if (!String.IsNullOrEmpty(item.Text))
			{
				writer.WriteLine();
				writer.WriteBeginTag("label");
				writer.WriteAttribute("for", treeView.ClientID + "n" + _checkboxIndex.ToString() + "CheckBox");
				writer.Write(HtmlTextWriter.TagRightChar);
				writer.Write(item.Text);
				writer.WriteEndTag("label");
			}

			_checkboxIndex++;
		}
		#endregion

		#region WriteNodeLink
		private void WriteNodeLink(TreeView treeView, TreeNode item, HtmlTextWriter writer)
		{
			writer.WriteBeginTag("a");

			if (!String.IsNullOrEmpty(item.NavigateUrl))
			{
				writer.WriteAttribute("href", Extender.ResolveUrl(item.NavigateUrl));
			}
			else
			{
				string codePrefix = "";
				if (item.SelectAction == TreeNodeSelectAction.Select)
				{
					codePrefix = "s";
				}
				else if (item.SelectAction == TreeNodeSelectAction.SelectExpand)
				{
					codePrefix = "e";
				}
				else if (item.PopulateOnDemand)
				{
					codePrefix = "p";
				}
				writer.WriteAttribute("href", Page.ClientScript.GetPostBackClientHyperlink(treeView, codePrefix + (Page.Server.HtmlEncode(item.ValuePath)).Replace("/", "\\"), true));
			}

			WebControlAdapterExtender.WriteTargetAttribute(writer, item.Target);

			if (!String.IsNullOrEmpty(item.ToolTip))
			{
				writer.WriteAttribute("title", item.ToolTip);
			}
			else if (!String.IsNullOrEmpty(treeView.ToolTip))
			{
				writer.WriteAttribute("title", treeView.ToolTip);
			}
			writer.Write(HtmlTextWriter.TagRightChar);
			writer.Indent++;
			writer.WriteLine();

			WriteNodeImage(treeView, item, writer);
			writer.Write(item.Text);

			writer.Indent--;
			writer.WriteEndTag("a");
		}
		#endregion

		#region WriteNodePlain
		private void WriteNodePlain(TreeView treeView, TreeNode item, HtmlTextWriter writer)
		{
			writer.WriteBeginTag("span");
			if (IsExpandable(item))
			{
				writer.WriteAttribute("class", "AspNet-TreeView-ClickableNonLink");
				if (treeView.ShowExpandCollapse)
				{
					writer.WriteAttribute("onclick", "ExpandCollapse__AspNetTreeView(this.parentNode.getElementsByTagName('span')[0])");
				}
			}
			else
			{
				writer.WriteAttribute("class", "AspNet-TreeView-NonLink");
			}
			writer.Write(HtmlTextWriter.TagRightChar);
			writer.Indent++;
			writer.WriteLine();

			WriteNodeImage(treeView, item, writer);
			writer.Write(item.Text);

			writer.Indent--;
			writer.WriteEndTag("span");
		}
		#endregion

		#region UpdateCheckmarks
		private void UpdateCheckmarks(TreeNodeCollection items)
		{
			TreeView treeView = Control as TreeView;
			if ((treeView != null) && (items != null))
			{
				foreach (TreeNode item in items)
				{
					if (IsCheckbox(treeView, item))
					{
						string name = treeView.UniqueID + "n" + _checkboxIndex.ToString() + "CheckBox";
						bool bIsNowChecked = (Page.Request.Form[name] != null);
						if (item.Checked != bIsNowChecked)
						{
							item.Checked = bIsNowChecked;
							Extender.RaiseAdaptedEvent("TreeNodeCheckChanged", new TreeNodeEventArgs(item));
						}
						_checkboxIndex++;
					}

					if (HasChildren(item))
					{
						UpdateCheckmarks(item.ChildNodes);
					}
				}
			}
		}
		#endregion

		#region IsLink
		private bool IsLink(TreeNode item)
		{
			return (item != null) && ((!String.IsNullOrEmpty(item.NavigateUrl)) || item.PopulateOnDemand || (item.SelectAction == TreeNodeSelectAction.Select) || (item.SelectAction == TreeNodeSelectAction.SelectExpand));
		}
		#endregion

		#region IsCheckbox
		private bool IsCheckbox(TreeView treeView, TreeNode item)
		{
			bool bItemCheckBoxDisallowed = (item.ShowCheckBox != null) && (item.ShowCheckBox.Value == false);
			bool bItemCheckBoxWanted = (item.ShowCheckBox != null) && (item.ShowCheckBox.Value == true);
			bool bTreeCheckBoxWanted =
				 (treeView.ShowCheckBoxes == TreeNodeTypes.All) ||
				 ((treeView.ShowCheckBoxes == TreeNodeTypes.Leaf) && (!IsExpandable(item))) ||
				 ((treeView.ShowCheckBoxes == TreeNodeTypes.Parent) && (IsExpandable(item))) ||
				 ((treeView.ShowCheckBoxes == TreeNodeTypes.Root) && (item.Depth == 0));

			return (!bItemCheckBoxDisallowed) && (bItemCheckBoxWanted || bTreeCheckBoxWanted);
		}
		#endregion

		#region GetNodeClass
		private string GetNodeClass(TreeNode item)
		{
			string value = "AspNet-TreeView-Leaf";
			if (item != null)
			{
				if (item.Depth == 0)
				{
					if (IsExpandable(item))
					{
						value = "AspNet-TreeView-Root";
					}
					else
					{
						value = "AspNet-TreeView-Root AspNet-TreeView-Leaf";
					}
				}
				else if (IsExpandable(item))
				{
					value = "AspNet-TreeView-Parent";
				}

				if (item.Selected)
				{
					value += " AspNet-TreeView-Selected";
				}
				else if (IsChildNodeSelected(item))
				{
					value += " AspNet-TreeView-ChildSelected";
				}
				else if (IsParentNodeSelected(item))
				{
					value += " AspNet-TreeView-ParentSelected";
				}
			}
			return value;
		}
		#endregion

		#region GetImageSrc
		private string GetImageSrc(TreeView treeView, TreeNode item)
		{
			string imgSrc = "";

			if ((treeView != null) && (item != null))
			{
				imgSrc = item.ImageUrl;

				if (String.IsNullOrEmpty(imgSrc))
				{
					if (item.Depth == 0)
					{
						if ((treeView.RootNodeStyle != null) && (!String.IsNullOrEmpty(treeView.RootNodeStyle.ImageUrl)))
						{
							imgSrc = treeView.RootNodeStyle.ImageUrl;
						}
					}
					else
					{
						if (!IsExpandable(item))
						{
							if ((treeView.LeafNodeStyle != null) && (!String.IsNullOrEmpty(treeView.LeafNodeStyle.ImageUrl)))
							{
								imgSrc = treeView.LeafNodeStyle.ImageUrl;
							}
						}
						else if ((treeView.ParentNodeStyle != null) && (!String.IsNullOrEmpty(treeView.ParentNodeStyle.ImageUrl)))
						{
							imgSrc = treeView.ParentNodeStyle.ImageUrl;
						}
					}
				}

				if ((String.IsNullOrEmpty(imgSrc)) && (treeView.LevelStyles != null) && (treeView.LevelStyles.Count > item.Depth))
				{
					if (!String.IsNullOrEmpty(treeView.LevelStyles[item.Depth].ImageUrl))
					{
						imgSrc = treeView.LevelStyles[item.Depth].ImageUrl;
					}
				}
			}

			return imgSrc;
		}
		#endregion

		#region HasChildren
		private bool HasChildren(TreeNode item)
		{
			return ((item != null) && ((item.ChildNodes != null) && (item.ChildNodes.Count > 0)));
		}
		#endregion

		#region IsExpandable
		private bool IsExpandable(TreeNode item)
		{
			return (HasChildren(item) || ((item != null) && item.PopulateOnDemand));
		}
		#endregion

		#region ClearSelectedNode
		private void ClearSelectedNode(TreeNodeCollection nodes)
		{
			if (nodes != null)
			{
				foreach (TreeNode node in nodes)
				{
					if (node.Selected)
					{
						node.Selected = false;
					}
					if (node.ChildNodes != null)
					{
						ClearSelectedNode(node.ChildNodes);
					}
				}
			}
		}
		#endregion

		#region IsChildNodeSelected
		private bool IsChildNodeSelected(TreeNode item)
		{
			bool bRet = false;

			if ((item != null) && (item.ChildNodes != null))
			{
				bRet = IsChildNodeSelected(item.ChildNodes);
			}

			return bRet;
		}
		#endregion

		#region IsChildNodeSelected
		private bool IsChildNodeSelected(TreeNodeCollection nodes)
		{
			bool bRet = false;

			if (nodes != null)
			{
				foreach (TreeNode node in nodes)
				{
					if (node.Selected || IsChildNodeSelected(node.ChildNodes))
					{
						bRet = true;
						break;
					}
				}
			}

			return bRet;
		}
		#endregion

		#region IsParentNodeSelected
		private bool IsParentNodeSelected(TreeNode item)
		{
			bool bRet = false;

			if ((item != null) && (item.Parent != null))
			{
				if (item.Parent.Selected)
				{
					bRet = true;
				}
				else
				{
					bRet = IsParentNodeSelected(item.Parent);
				}
			}

			return bRet;
		}
		#endregion

		#region ComposeViewState
		private string ComposeViewState(TreeNodeCollection nodes, string state)
		{
			if (nodes != null)
			{
				foreach (TreeNode node in nodes)
				{
					if (IsExpandable(node))
					{
						if (node.Expanded.Equals(true))
						{
							state += "e";
							state = ComposeViewState(node.ChildNodes, state);
						}
						else
						{
							state += "n";
						}
					}
				}
			}

			return state;
		}
		#endregion

		#region ExpandToState
		private string ExpandToState(TreeNodeCollection nodes, string state)
		{
			if ((nodes != null) && (!String.IsNullOrEmpty(state)))
			{
				foreach (TreeNode node in nodes)
				{
					if (IsExpandable(node))
					{
						bool bExpand = (state[0] == 'e');
						state = state.Substring(1);
						if (bExpand)
						{
							node.Expand();
							state = ExpandToState(node.ChildNodes, state);
						}
					}
				}
			}

			return state;
		}
		#endregion

		#region ExpandToDepth
		static public void ExpandToDepth(TreeNodeCollection nodes, int expandDepth)
		{
			if (nodes != null)
			{
				foreach (TreeNode node in nodes)
				{
					if (node.Depth < expandDepth)
					{
						node.Expand();
						ExpandToDepth(node.ChildNodes, expandDepth);
					}
				}
			}
		}
		#endregion
    }
}
#pragma warning restore 1591
