namespace WatirRecorder
{
	public class WatirMaker
	{
		/// <summary>
		/// Sets the text field.
		/// </summary>
		/// <param name="how">The how.</param>
		/// <param name="name">The name.</param>
		/// <param name="val">The val.</param>
		/// <returns></returns>
		public string SetTextField(string how, string name, string val)
		{
			string setvalue = string.Format("@browser.text_field({0}, '{1}').set('{2}')",how,name,val.Replace("'",@"\'"));
			return setvalue;
		}

		/// <summary>
		/// Checks the Checkboxe
		/// </summary>
		/// <param name="how">The how.</param>
		/// <param name="name">The name.</param>
		/// <param name="val">The val.</param>
		/// <returns></returns>
		public string Checkbox(string how, string name, string val)
		{
			string setvalue = null;
			if (val == "on")
				setvalue = string.Format("@browser.checkbox({0}, '{1}').set()",how,name);
			else
				setvalue = string.Format("@browser.checkbox({0}, '{1}').clear()",how,name);
			return setvalue;
		}

		/// <summary>
		/// Checks a radiobutton
		/// </summary>
		/// <param name="how">The how.</param>
		/// <param name="name">The name.</param>
		/// <param name="val">The val.</param>
		/// <returns></returns>
		public string Radio(string how, string name, string val)
		{
			string setvalue = null;
			setvalue = string.Format("@browser.radio({0}, '{1}','{2}').set()",how,name,val.Replace("'",@"\'"));
			return setvalue;
		}

		/// <summary>
		/// Selects the list.
		/// </summary>
		/// <param name="how">The how.</param>
		/// <param name="name">The name.</param>
		/// <param name="val">The val.</param>
		/// <returns></returns>
		public string SelectList(string how, string name, string val)
		{
			string setvalue = null;
			setvalue = string.Format("@browser.select_list({0}, '{1}').select_value('{2}')",how,name,val.Replace("'",@"\'"));
			return setvalue;
		}

		/// <summary>
		/// Clicks the link.
		/// </summary>
		/// <param name="how">The how.</param>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		public string ClickLink(string how, string name)
		{
			string setvalue = string.Format("@browser.link({0}, '{1}').click",how,name.Replace("\r\n", ""));
			return setvalue;
		}

		/// <summary>
		/// Clicks the button.
		/// </summary>
		/// <param name="how">The how.</param>
		/// <param name="name">The name.</param>
		/// <param name="val">The val.</param>
		/// <returns></returns>
		public string ClickButton(string how, string name, string val)
		{
			string setvalue = null;
			if (name != null)
			{
				setvalue = string.Format("@browser.button({0}, '{1}').click",how,name);
			}
			else if (val != null)
			{
				setvalue = string.Format("@browser.button(:value, '{0}').click",val.Replace("'",@"\'"));
			}
			return setvalue;
		}

        /// <summary>
        ///  Clicks assertion button
        /// </summary>
        /// <param name="assert">The markup expected to exist.</param>
        /// <returns></returns>
        public string AssertExists(string assert)
        {
            string setvalue = null;
            assert = assert.Replace("\"", "\\\"");
            setvalue = string.Format("assert(@browser.contains_text(\"{0}\"))", assert);
            return setvalue;
        }
	}
}