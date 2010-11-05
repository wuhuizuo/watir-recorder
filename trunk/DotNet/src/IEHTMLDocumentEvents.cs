using System;
using mshtml;

namespace WatirRecorder
{
	public class IEHTMLDocumentEvents : HTMLDocumentEvents2
	{
        private bool isSelecting = false;
        private string assertion = null;

		public IEHTMLDocumentEvents(frmMain back)
		{
			formBackpointer = back;
		}

		private frmMain formBackpointer = null;
		private WatirMaker watirMaker = new WatirMaker();

        public string GetAssertion()
        {
            if (this.assertion != null)
            {
                return assertion;
            } else 
            {
                return "";
            }
        }

        public void ResetAssertion()
        {
            this.assertion = null;
        }

		public void ondataavailable(IHTMLEventObj pEvtObj)    
		{  
  
		}

		public bool onbeforedeactivate(IHTMLEventObj pEvtObj)  
		{    
			return true;  
		}

		public bool onstop(IHTMLEventObj pEvtObj)  
		{  
			return true;  
		}

		public void onrowsinserted(IHTMLEventObj pEvtObj)  
		{  
 
		}

		public bool onselectstart(IHTMLEventObj pEvtObj)  
		{  
			string log = String.Format("On Select Start {0}", pEvtObj.srcElement.id);
			formBackpointer.LogEvent(log);
            this.isSelecting = true;
			return true;  
		}

		public bool onkeypress(IHTMLEventObj pEvtObj)  
		{
			//string log = String.Format("onkeydown {0} {1}", pEvtObj.keyCode.ToString(), (char)pEvtObj.keyCode);
			//formBackpointer.LogEvent(log);
			return true;  
		}

		public bool onhelp(IHTMLEventObj pEvtObj)  
		{  
			return true;  
		}

		public void onpropertychange(IHTMLEventObj pEvtObj)    
		{  
  
		}

		public void oncellchange(IHTMLEventObj pEvtObj)  
		{  
  
		}

		public bool oncontextmenu(IHTMLEventObj pEvtObj)  
		{  
			return true;  
		}

		public bool ondblclick(IHTMLEventObj pEvtObj)  
		{  
			return true;  
		}

		public void onfocusin(IHTMLEventObj pEvtObj)  
		{  
			string log = String.Format("On Focus In {0} {1}", pEvtObj.keyCode.ToString(), (char)pEvtObj.keyCode);
			formBackpointer.LogEvent(log);
		}

		public void ondatasetcomplete(IHTMLEventObj pEvtObj)  
		{  
  
		}

		public void onkeyup(IHTMLEventObj pEvtObj)  
		{  
  
		}

		public bool onclick(IHTMLEventObj pEvtObj)  
		{  
			string code = null;
			if(pEvtObj.srcElement is HTMLAnchorElementClass)
			{
				HTMLAnchorElementClass a = (HTMLAnchorElementClass) pEvtObj.srcElement;
				if (string.IsNullOrEmpty(a.id) == false)
		                {
		                    code = watirMaker.ClickLink(":id", a.id);
		                }
				else if (string.IsNullOrEmpty(a.name) == false)
		                {
		                    code = watirMaker.ClickLink(":name", a.name);
		                }
				else if (string.IsNullOrEmpty(a.innerText) == false)
		                {
		                    code = watirMaker.ClickLink(":text", a.innerText);
		                }
		                else
		                {
		                    code = watirMaker.ClickLink(":url", a.href);
		                }
			}
			else if(pEvtObj.srcElement is HTMLButtonElementClass)
			{
				HTMLButtonElementClass b = pEvtObj.srcElement as HTMLButtonElementClass;
				if (b.id != null && b.id.Length > 0)
				{
					code = watirMaker.ClickButton(":id",b.id,b.value);
				}
				else //use name or value
				{
					code = watirMaker.ClickButton(":name",b.name,b.value);
				}
			}
			else if(pEvtObj.srcElement is HTMLInputElementClass)
			{
				HTMLInputElementClass text = pEvtObj.srcElement as HTMLInputElementClass;
				string log = String.Format("Focus Out \"{0}\"", (text.id != null ? text.id	: text.name));
				switch(text.type)
				{
					case "radio":
						if (text.id != null && text.id.Length > 0)					
						{
							code = watirMaker.Radio(":id", text.id, text.value);
						}
						else if (text.name != null && text.name.Length > 0) //use name
						{
							code = watirMaker.Radio(":name", text.name, text.value);
						}
						break;
					case "submit":
					case "button":
					case "image":
						if (text.id != null && text.id.Length > 0)
						{
							code = watirMaker.ClickButton(":id",text.id,text.value);
						}
						else //use name or value
						{
							code = watirMaker.ClickButton(":name",text.name,text.value);
						}
						break;
				}
				formBackpointer.LogEvent(log);
			}
			if (code != null) 
			{
				formBackpointer.SuppressOneGoto();
				formBackpointer.AppendText(code);
			}
			return true;  
		}

		public void onfocusout(IHTMLEventObj pEvtObj)  
		{  
			string log;
			string code = null;
			if(pEvtObj.srcElement is HTMLInputElementClass && 
				!(pEvtObj.srcElement is HTMLButtonElementClass))
			{
				HTMLInputElementClass text = pEvtObj.srcElement as HTMLInputElementClass;
				log = String.Format("Focus Out \"{0}\"", (text.id != null ? text.id	: text.name));
				switch(text.type)
				{
					case "text":
					case "password":
						if (text.id != null && text.id.Length > 0)					
						{
							code = watirMaker.SetTextField(":id", text.id, text.value);
						}
						else if (text.name != null && text.name.Length > 0) //use name
						{
							code = watirMaker.SetTextField(":name", text.name, text.value);
						}
						break;
					case "checkbox":
						if (text.id != null && text.id.Length > 0)					
						{
							code = watirMaker.Checkbox(":id", text.id, text.value);
						}
						else if (text.name != null && text.name.Length > 0) //use name
						{
							code = watirMaker.Checkbox(":name", text.name, text.value);
						}
						break;
				}
				formBackpointer.LogEvent(log);
			}
            else if (pEvtObj.srcElement is mshtml.HTMLTextAreaElement)
            {
                HTMLInputElement text = pEvtObj.srcElement as HTMLInputElement;
                switch (text.type)
                {
                    case "textarea":
                        if (text.id != null && text.id.Length > 0)
                        {
                            code = watirMaker.SetTextField(":id", text.id, text.value);
                        }
                        else if (text.name != null && text.name.Length > 0) //use name
                        {
                            code = watirMaker.SetTextField(":name", text.name, text.value);
                        }
                        break;
                }
            }
			else if(pEvtObj.srcElement is HTMLSelectElementClass)
			{
				HTMLSelectElementClass list = pEvtObj.srcElement as HTMLSelectElementClass;
				log = String.Format("Focus Out \"{0}\"", (list.id != null ? list.id	: list.name));
				if (list.id != null && list.id.Length > 0)					
				{
					code = watirMaker.SelectList(":id", list.id, list.value);
				}
				else if (list.name != null && list.name.Length > 0) //use name
				{
					code = watirMaker.SelectList(":name", list.name, list.value);
				}
				formBackpointer.LogEvent(log);
			}
			if (code != null) formBackpointer.AppendText(code);
		}

		public void onbeforeeditfocus(IHTMLEventObj pEvtObj)    
		{  
			string log = String.Format("On Before Edit Focus {0}", pEvtObj.srcElement.id);
			formBackpointer.LogEvent(log);
		}

		public bool ondragstart(IHTMLEventObj pEvtObj)   
		{  
			return true;    
		}

		public bool oncontrolselect(IHTMLEventObj pEvtObj)  
		{  
			string log = String.Format("On Control Select {0}", pEvtObj.srcElement.id);
			formBackpointer.LogEvent(log);
			return true;  
		}

		public void onactivate(IHTMLEventObj pEvtObj)  
		{  
			string log = String.Format("On Activate {0}", pEvtObj.srcElement.id);
			formBackpointer.LogEvent(log);
		}

		public void onmouseup(IHTMLEventObj pEvtObj)   
		{
            if (this.isSelecting)
            {
                this.assertion = @watirMaker.AssertExists(pEvtObj.srcElement.innerHTML);
                this.isSelecting = false;
            }
		}

		public bool onbeforeactivate(IHTMLEventObj pEvtObj)   
		{   
			string log = String.Format("On Before Activate {0}", pEvtObj.srcElement.id);
			formBackpointer.LogEvent(log);
			return true;   
		}

		public void onkeydown(IHTMLEventObj pEvtObj)   
		{   

		}

		public bool onrowexit(IHTMLEventObj pEvtObj)  
		{  
			return true;    
		}

		public bool onbeforeupdate(IHTMLEventObj pEvtObj)  
		{  
			return true;  
		}

		public void onrowsdelete(IHTMLEventObj pEvtObj)  
		{  
  
		}

		public void onreadystatechange(IHTMLEventObj pEvtObj)  
		{  

		}

		public void onmousemove(IHTMLEventObj pEvtObj)  
		{  
  
		}

		public void onrowenter(IHTMLEventObj pEvtObj)  
		{  
			string log = String.Format("On Row Enter {0}", pEvtObj.srcElement.id);
			formBackpointer.LogEvent(log);
		}

		public void onafterupdate(IHTMLEventObj pEvtObj)   
		{  
			string log = String.Format("On After Update {0}", pEvtObj.srcElement.id);
			formBackpointer.LogEvent(log);
		}

		public void ondeactivate(IHTMLEventObj pEvtObj)  
		{  
  
		}

		public void onselectionchange(IHTMLEventObj pEvtObj)  
		{  
			string log = String.Format("On Selection Change {0}", pEvtObj.srcElement.id);
			formBackpointer.LogEvent(log);
		}

		public void ondatasetchanged(IHTMLEventObj pEvtObj)  
  
		{  
  
		}

		public void onmouseover(IHTMLEventObj pEvtObj)  
		{  
  
		}

		public bool onmousewheel(IHTMLEventObj pEvtObj)  
		{  
			return true;  
		}

		public bool onerrorupdate(IHTMLEventObj pEvtObj)  
		{  
			string log = String.Format("On Error Update {0}", pEvtObj.srcElement.id);
			formBackpointer.LogEvent(log);
			return true;  
		}

		public void onmouseout(IHTMLEventObj pEvtObj)  
		{  
  
		}

		public void onmousedown(IHTMLEventObj pEvtObj)  
		{  
  
		}
	}
}
